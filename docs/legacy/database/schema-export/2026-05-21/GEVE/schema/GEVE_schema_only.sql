USE [master]
GO
/****** Object:  Database [GEVE]    Script Date: 20/05/2026 14:31:15 ******/
CREATE DATABASE [GEVE]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GEVE_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.RACCA\MSSQL\DATA\GEVE.mdf' , SIZE = 8192000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'GEVE_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.RACCA\MSSQL\DATA\GEVE_1.ldf' , SIZE = 8351552KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [GEVE] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GEVE].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [GEVE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GEVE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GEVE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GEVE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GEVE] SET ARITHABORT OFF 
GO
ALTER DATABASE [GEVE] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GEVE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GEVE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GEVE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GEVE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GEVE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GEVE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GEVE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GEVE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GEVE] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GEVE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GEVE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GEVE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GEVE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GEVE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GEVE] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GEVE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GEVE] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GEVE] SET  MULTI_USER 
GO
ALTER DATABASE [GEVE] SET PAGE_VERIFY TORN_PAGE_DETECTION  
GO
ALTER DATABASE [GEVE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GEVE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GEVE] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [GEVE] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GEVE] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [GEVE] SET QUERY_STORE = OFF
GO
USE [GEVE]
GO
/****** Object:  User [REDACTED_DB_USER_1]    Script Date: REDACTED ******/
-- REDACTED: database user omitted from documentation export
GO
/****** Object:  Schema [REDACTED_DB_SCHEMA_1]    Script Date: REDACTED ******/
-- REDACTED: database schema/user placeholder omitted from documentation export
GO
/****** Object:  UserDefinedFunction [dbo].[CLIENTE_VALIDO]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[CLIENTE_VALIDO] (@CLCOD AS VARCHAR(5),@REGFAT AS SMALLINT, @REGPA AS SMALLINT)
RETURNS SMALLINT

BEGIN

DECLARE @RAGSOC AS VARCHAR(60), @PIVA VARCHAR(11), @CFIS VARCHAR(16), @PIVAEST VARCHAR(20), @INDIRIZZO VARCHAR(50), @CAP VARCHAR(5), @CITTA VARCHAR(50),
        @NAZIONE VARCHAR(2), @COD_DESTINATARIO VARCHAR(7), @ERRORE as smallint


SET @ERRORE = 0

SELECT @RAGSOC=Anadesc,@PIVA=AnaPiva,@CFIS=AnaCfis,@PIVAEST=AnaPivaEst,@INDIRIZZO=AnaIndirizzo,@CAP=AnaCap,@CITTA=AnaCitta,
       @NAZIONE = CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END 

FROM VDOX.DBO.TbAna
WHERE AnaCod = @CLCOD and anagrp = 'CL'


SET  @COD_DESTINATARIO=ISNULL((Select ClFteDestinatario from TbFteCli where clFteCod = @clcod),'')
IF @COD_DESTINATARIO = ''
   BEGIN
   IF @PIVAEST > ''
      BEGIN
      SET @COD_DESTINATARIO = 'XXXXXXX'
	  END
   ELSE
      BEGIN
      SET @COD_DESTINATARIO = '0000000'
	  END
   END

IF @REGFAT = @REGPA 
   BEGIN
    IF LEN(@COD_DESTINATARIO) <> 6
      BEGIN
      -------MANCA CODICE DESTINATARIO O CODICE NON VALIDO
      SET @ERRORE = 1
      GOTO FINE
      END
   END
 ELSE
   BEGIN
    IF LEN(@COD_DESTINATARIO) <> 7
     BEGIN
     -------MANCA CODICE DESTINATARIO O CODICE NON VALIDO
     SET @ERRORE = 1
     GOTO FINE
     END
   END
   

IF RTRIM(@PIVAEST) = '' AND RTRIM(@PIVA) = '' AND RTRIM(@CFIS) = ''
   BEGIN
   ---- MANCA PARTITA IVA E CODICE FISCALE (ALMENO UNO DEI DUE)
   SET @ERRORE = 2
   GOTO FINE
   END

IF RTRIM(@INDIRIZZO) = ''
   BEGIN
   ---- MANCA INDIRIZZO
   SET @ERRORE = 3
   GOTO FINE
   END

IF RTRIM(@CITTA) = ''
   BEGIN
   ---- MANCA COMUNE
   SET @ERRORE = 4
   GOTO FINE
   END

IF LEN(@CAP) < 5 AND RTRIM(@PIVAEST) = ''
   BEGIN
   ---- MANCA CAP O CAP ERRATO
   SET @ERRORE = 5
   GOTO FINE
   END

IF RTRIM(@NAZIONE) = ''
   BEGIN
   ---- MANCA NAZIONE
   SET @ERRORE = 6
   GOTO FINE
   END

   
FINE:

RETURN(@ERRORE)

END


GO
/****** Object:  UserDefinedFunction [dbo].[EstraiConfezione]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   FUNCTION [dbo].[EstraiConfezione] (@testoInput NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
    IF @testoInput IS NULL OR @testoInput = '' RETURN '';
    
    DECLARE @Inizio INT, @Fine INT, @Risultato NVARCHAR(MAX);
    DECLARE @TempInput NVARCHAR(MAX) = UPPER(@testoInput); -- Portiamo tutto in maiuscolo per sicurezza

    -- 1. IDENTIFICAZIONE INIZIO (Dopo CONF, CARTA, ecc.)
    SET @Inizio = CHARINDEX('CONF', @TempInput);
    
    IF @Inizio > 0 
    BEGIN
        SET @Inizio = @Inizio + 4; 
        -- Saltiamo eventuali punti o spazi subito dopo "CONF"
        WHILE @Inizio <= LEN(@TempInput) AND SUBSTRING(@TempInput, @Inizio, 1) IN ('.', ' ')
            SET @Inizio = @Inizio + 1;
    END
    ELSE 
    BEGIN
        -- Se manca "CONF", cerchiamo le altre parole chiave note
        IF CHARINDEX('CARTA', @TempInput) > 0 SET @Inizio = CHARINDEX('CARTA', @TempInput);
        ELSE IF CHARINDEX('ASTUCCIO', @TempInput) > 0 SET @Inizio = CHARINDEX('ASTUCCIO', @TempInput);
        ELSE IF CHARINDEX('STOFFA', @TempInput) > 0 SET @Inizio = CHARINDEX('STOFFA', @TempInput);
        ELSE IF CHARINDEX('VELINA', @TempInput) > 0 SET @Inizio = CHARINDEX('VELINA', @TempInput);
    END

    -- 2. IDENTIFICAZIONE FINE (Il Peso: GR o KG)
    -- Cerchiamo la posizione di "GR" o "KG" seguiti da un punto o uno spazio o un numero
    SET @Fine = PATINDEX('%GR[ .0-9]%', @TempInput);
    IF @Fine = 0 SET @Fine = PATINDEX('%KG[ .0-9]%', @TempInput);
    -- Caso limite: l'unità è alla fine esatta della stringa
    IF @Fine = 0 AND RIGHT(@TempInput, 2) IN ('GR', 'KG') SET @Fine = LEN(@TempInput) - 1;

    -- 3. ESTRAZIONE E PULIZIA
    IF @Inizio > 0
    BEGIN
        IF @Fine > @Inizio 
            SET @Risultato = SUBSTRING(@TempInput, @Inizio, @Fine - @Inizio);
        ELSE
            SET @Risultato = SUBSTRING(@TempInput, @Inizio, LEN(@TempInput));

        -- Rimuoviamo residui finali (punti, virgole, spazi, trattini)
        SET @Risultato = LTRIM(RTRIM(@Risultato));
        WHILE LEN(@Risultato) > 0 AND RIGHT(@Risultato, 1) IN ('.', ',', '-', ' ', '+')
        BEGIN
            SET @Risultato = LEFT(@Risultato, LEN(@Risultato) - 1);
        END
    END
    ELSE
    BEGIN
        SET @Risultato = '';
    END

    RETURN LTRIM(RTRIM(@Risultato));
END
GO
/****** Object:  UserDefinedFunction [dbo].[EstraiPeso]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   FUNCTION [dbo].[EstraiPeso] (@testoInput NVARCHAR(MAX))
RETURNS NVARCHAR(50)
AS
BEGIN
    IF @testoInput IS NULL OR @testoInput = '' RETURN '';

    DECLARE @Inizio INT, @Fine INT, @SubStr NVARCHAR(100);
    SET @testoInput = UPPER(@testoInput);

    -- 1. Cerchiamo dove inizia GR o KG
    SET @Inizio = PATINDEX('%GR%', @testoInput);
    IF @Inizio = 0 SET @Inizio = PATINDEX('%KG%', @testoInput);

    -- Se non trova né GR né KG, esce
    IF @Inizio = 0 RETURN '';

    -- 2. Prendiamo una porzione di testo dopo l'unità (max 15 caratteri)
    SET @SubStr = SUBSTRING(@testoInput, @Inizio, 15);

    -- 3. Identifichiamo dove finisce il numero del peso.
    -- Scorriamo finché troviamo caratteri validi: G, R, K, punto, spazio, cifre o virgola.
    SET @Fine = 1;
    WHILE @Fine <= LEN(@SubStr)
    BEGIN
        IF SUBSTRING(@SubStr, @Fine, 1) NOT LIKE '[GRK0-9,. ]'
            BREAK;
        SET @Fine = @Fine + 1;
    END

    -- 4. Pulizia finale: togliamo spazi extra o punti finali rimasti
    DECLARE @Risultato NVARCHAR(50) = RTRIM(SUBSTRING(@SubStr, 1, @Fine - 1));
    
    -- Se l'ultimo carattere è un punto o una virgola isolata, lo puliamo
    IF RIGHT(@Risultato, 1) IN ('.', ',') SET @Risultato = LEFT(@Risultato, LEN(@Risultato)-1);

    RETURN LTRIM(@Risultato);
END
GO
/****** Object:  UserDefinedFunction [dbo].[EstraiPesoVeloce]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[EstraiPesoVeloce] (@testoInput NVARCHAR(MAX))
RETURNS NVARCHAR(50)
AS
BEGIN
    IF @testoInput IS NULL RETURN NULL
    
    SET @testoInput = UPPER(@testoInput)
    DECLARE @Pos INT, @Risultato NVARCHAR(50) = ''

    -- 1. Trova dove inizia GR o KG
    SET @Pos = PATINDEX('%[KG]G%', @testoInput)
    IF @Pos = 0 RETURN ''

    -- 2. Estrai dalla sigla in poi (es: "GR. 750 + COP")
    SET @Risultato = SUBSTRING(@testoInput, @Pos, 20)

    -- 3. Pulizia: prendiamo solo i caratteri validi (K, G, R, punto, spazio, cifre e virgola)
    -- Ci fermiamo al primo carattere non valido (come il "+" o la "C" di C.A.)
    DECLARE @i INT = 1
    DECLARE @Output NVARCHAR(50) = ''
    DECLARE @Char NCHAR(1)

    WHILE @i <= LEN(@Risultato)
    BEGIN
        SET @Char = SUBSTRING(@Risultato, @i, 1)
        -- Permetti lettere dell'unità, cifre, virgola, punto e spazio
        IF @Char LIKE '[KGR0-9,. ]'
            SET @Output = @Output + @Char
        ELSE
            BREAK -- Appena trovi altro (es. "+"), fermati
        
        SET @i = @i + 1
    END

    RETURN RTRIM(LTRIM(@Output))
END
GO
/****** Object:  UserDefinedFunction [dbo].[FnTotDoc]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[FnTotDoc] (@TIPODOC as VARCHAR(1), @NUMRIF AS int, @NIMP as smallint)
RETURNS decimal(11,2) 
AS

BEGIN


	DECLARE @TOTALE AS DECIMAL(11,2), @CORIMPORTO AS DECIMAL(9,2), @CORCIVA AS SMALLINT, @PERC AS SMALLINT, @COROMAGGI AS VARCHAR(1), @TOTIMPO AS DECIMAL(9,2), @TOTIVA AS DECIMAL(9,2)

	SET @TOTALE = 0
	  	

	DECLARE PIPPO CURSOR FOR
	SELECT CORIMPORTO = SUM(CORIMPORTO),CORCIVA,COROMAGGI
	from TbCor
	WHERE CorTipoDoc = @TIPODOC and CorRif = @NUMRIF
	GROUP BY CORCIVA,COROMAGGI


	OPEN PIPPO
	FETCH NEXT FROM PIPPO
	INTO @CORIMPORTO,@CORCIVA,@COROMAGGI 


	WHILE @@FETCH_STATUS = 0
		BEGIN
	
	       SET @TOTIMPO = CASE WHEN @COROMAGGI= 'S' THEN 0 ELSE  @CORIMPORTO END

		   SET @PERC = CASE WHEN @NIMP <> 0 THEN (SELECT CIIALI FROM COGE.dbo.TbCii where ciicod = @NIMP)  ELSE (SELECT CIIALI FROM COGE.dbo.TbCii where ciicod = @CORCIVA) END

		   SET @TOTIVA = @CORIMPORTO * @PERC / 100

		   SET @TOTALE = @TOTALE + @TOTIMPO + @TOTIVA

		   FETCH NEXT FROM PIPPO
		   INTO @CORIMPORTO,@CORCIVA,@COROMAGGI 
		END

	CLOSE PIPPO
	DEALLOCATE PIPPO
 
	

   
   
FINE:
return (@TOTALE) 
END


GO
/****** Object:  UserDefinedFunction [dbo].[ISOweek]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ISOweek]  (@DATE datetime)
RETURNS int
AS
BEGIN
   DECLARE @ISOweek int
   SET @ISOweek= DATEPART(wk,@DATE)+1
      -DATEPART(wk,CAST(DATEPART(yy,@DATE) as CHAR(4))+'0104')
   --Special cases: Jan 1-3 may belong to the previous year
   IF (@ISOweek=0) 
      SET @ISOweek=dbo.ISOweek(CAST(DATEPART(yy,@DATE)-1 
         AS CHAR(4))+'12'+ CAST(24+DATEPART(DAY,@DATE) AS CHAR(2)))+1
   --Special case: Dec 29-31 may belong to the next year
   IF ((DATEPART(mm,@DATE)=12) AND 
      ((DATEPART(dd,@DATE)-DATEPART(dw,@DATE))>= 28))
      SET @ISOweek=1
   RETURN(@ISOweek)
END
GO
/****** Object:  UserDefinedFunction [dbo].[ISOyear]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ISOyear]  (@DATE datetime)
RETURNS int
AS
BEGIN
   DECLARE @ISOyear int, @WEEK as int, @ISOweek as int, @ANNO AS INT
   SET @ANNO = DATEPART(YEAR,@DATE)
   SET @WEEK =  DATEPART(WEEK,@DATE)
   SET @ISOweek = dbo.ISOweek(@DATE)
   SET @ISOyear = @ANNO
   IF @ISOweek > @WEEK + 50
      BEGIN
	     SET @ISOyear  = @ANNO - 1
	  END
   IF @WEEK > @ISOweek + 50
      BEGIN
	     SET @ISOyear  = @ANNO + 1
	  END
   RETURN(@ISOyear)
END
GO
/****** Object:  UserDefinedFunction [dbo].[NUMBER_TO_STR_BASE]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[NUMBER_TO_STR_BASE] (@base int,@number int)
RETURNS varchar(MAX)
WITH EXECUTE AS CALLER
AS
BEGIN
     DECLARE @dividend int = @number
        ,@remainder int = 0 
        ,@numberString varchar(MAX) = CASE WHEN @number = 0 THEN '0' ELSE '' END ;
     SET @base = CASE WHEN @base <= 36 THEN @base ELSE 36 END;--The max base is 36, includes the range of [0-9A-Z]
     WHILE (@dividend > 0 OR @remainder > 0)
         BEGIN
            SET @remainder = @dividend % @base ; --The reminder by the division number in base
            SET @dividend = @dividend / @base ; -- The integer part of the division, becomes the new divident for the next loop
            IF(@dividend > 0 OR @remainder > 0)--check that not correspond the last loop when quotient and reminder is 0
                SET @numberString =  CHAR( (CASE WHEN @remainder <= 9 THEN ASCII('0') ELSE ASCII('A')-10 END) + @remainder ) + @numberString;
     END;
     RETURN(@numberString);
END
GO
/****** Object:  UserDefinedFunction [dbo].[TCI]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create function [dbo].[TCI] (@civa as smallint)
returns SMALLINT
as

begin

declare @CODCODIVA AS SMALLINT

SET  @CODCODIVA = ( CASE 
       WHEN @CIVA = 1 THEN (SELECT TOP 1 TaiPi1 from TbTai order by TaiAnno Desc)
        WHEN @CIVA = 2 THEN (SELECT TOP 1 TaiPi2 from TbTai order by TaiAnno desc)
         WHEN @CIVA = 3 THEN (SELECT TOP 1 TaiPi3 from TbTai order by TaiAnno desc)
          WHEN @CIVA = 4 THEN (SELECT TOP 1 TaiPi4 from TbTai order by TaiAnno desc)
           WHEN @CIVA = 5 THEN (SELECT TOP 1 TaiPi5 from TbTai order by TaiAnno desc)
            WHEN @CIVA = 6 THEN (SELECT TOP 1 TaiPi6 from TbTai order by TaiAnno desc)
             WHEN @CIVA = 7 THEN (SELECT TOP 1 TaiPi7 from TbTai order by TaiAnno desc)
              WHEN @CIVA = 8 THEN (SELECT TOP 1 TaiPi8 from TbTai order by TaiAnno desc)
               WHEN @CIVA = 9 THEN (SELECT TOP 1 TaiPi9 from TbTai order by TaiAnno desc)
                WHEN @CIVA = 10 THEN (SELECT TOP 1 TaiPi10 from TbTai order by TaiAnno desc)
                 WHEN @CIVA = 11 THEN (SELECT TOP 1 TaiPi11 from TbTai order by TaiAnno desc)
                  WHEN @CIVA = 12 THEN (SELECT TOP 1 TaiPi12 from TbTai order by TaiAnno desc)
                 else 0 end)
                   
 RETURN(@CODCODIVA)                  
 end       
 
 
GO
/****** Object:  Table [dbo].[TbArt]    Script Date: 20/05/2026 14:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbArt](
	[ArtId] [int] IDENTITY(1,1) NOT NULL,
	[ArtCod] [varchar](20) NOT NULL,
	[ArtDesc] [varchar](200) NOT NULL,
	[ArtMinidesc] [varchar](25) NOT NULL,
	[ArtTipoProd] [varchar](1) NOT NULL,
	[ArtCat] [varchar](2) NOT NULL,
	[ArtTip] [varchar](2) NOT NULL,
	[ArtSiglaSconto] [varchar](2) NOT NULL,
	[ArtUmisura] [varchar](2) NOT NULL,
	[ArtPesoCnf] [decimal](5, 3) NOT NULL,
	[ArtNumImballo] [decimal](5, 0) NOT NULL,
	[ArtPeso] [decimal](6, 3) NOT NULL,
	[ArtCodiva] [smallint] NOT NULL,
	[ArtCptcon] [varchar](5) NOT NULL,
	[ArtFlagAttivo] [bit] NOT NULL,
	[ArtScadenza] [smallint] NOT NULL,
	[ArtDescLis] [varchar](60) NOT NULL,
	[ArtNoteLis] [varchar](25) NOT NULL,
	[ArtDistinta] [bit] NOT NULL,
	[ArtUmVen] [varchar](2) NOT NULL,
	[ArtUmTec] [varchar](2) NOT NULL,
	[ArtDiBase] [smallint] NOT NULL,
	[ArtCiAcq] [smallint] NOT NULL,
	[ArtCptAcq] [varchar](5) NOT NULL,
	[ArtMultimb] [bit] NOT NULL,
	[ArtDesc2] [varchar](40) NOT NULL,
	[ArtDesc3] [varchar](40) NOT NULL,
	[ArtDesc4] [varchar](40) NOT NULL,
	[ArtDesc5] [varchar](40) NOT NULL,
	[ArtFamiglia] [smallint] NOT NULL,
	[ArtAcquistato] [bit] NOT NULL,
	[ArtRipieno] [smallint] NOT NULL,
	[ArtTipo62] [tinyint] NOT NULL,
	[ArtEAN13] [varchar](13) NOT NULL,
	[ArtAttivoStore] [bit] NOT NULL,
	[ArtProduzStore] [bit] NOT NULL,
	[ArtSfuso] [bit] NOT NULL,
	[ArtImballoStore] [smallint] NOT NULL,
	[ArtPesoCnfStore] [decimal](5, 3) NOT NULL,
	[ArtProducer] [varchar](1) NOT NULL,
	[ArtTest] [bit] NOT NULL,
	[ArtCatP] [varchar](1) NOT NULL,
	[ArtSL] [smallint] NOT NULL,
	[ArtGruppo] [smallint] NOT NULL,
	[ArtPrzAcquisto] [decimal](9, 2) NOT NULL,
	[ArtPrzVendita] [decimal](9, 2) NOT NULL,
	[ArtSLC] [smallint] NOT NULL,
	[Art_CAMPIONATURA] [bit] NOT NULL,
	[ArtXDesc] [varchar](40) NOT NULL,
	[ArtXDesc2] [varchar](40) NOT NULL,
	[ArtXDesc3] [varchar](40) NOT NULL,
	[ArtXDesc4] [varchar](40) NOT NULL,
	[ArtXDesc5] [varchar](40) NOT NULL,
	[ArtXFlag] [bit] NOT NULL,
	[ArtFormato] [int] NOT NULL,
	[ArtDisp_Effettiva] [tinyint] NOT NULL,
	[ArtColore] [int] NOT NULL,
	[ArtDbTipo] [varchar](1) NOT NULL,
	[ArtImbKit] [smallint] NOT NULL,
	[ArtBarcode] [varchar](50) NOT NULL,
	[PesoGr] [int] NOT NULL,
	[ArtIngredienti] [varbinary](max) NULL,
	[ArtBilancia] [bit] NOT NULL,
	[ArtCodBil] [int] NOT NULL,
	[ArtBarcodeForn] [varchar](50) NOT NULL,
	[ArtEtixFoglio] [smallint] NOT NULL,
	[ArtUltLotto] [varchar](20) NOT NULL,
	[ArtUltScadenza] [smalldatetime] NULL,
	[ArtUlt_modifica] [smalldatetime] NULL,
	[ArtUtente] [varchar](20) NULL,
	[ArtAUlt_modifica] [smalldatetime] NULL,
	[ArtAUtente] [varchar](20) NULL,
 CONSTRAINT [PK_TbArt] PRIMARY KEY CLUSTERED 
(
	[ArtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbNewFasAtt]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbNewFasAtt](
	[FasAtt] [smallint] NOT NULL,
	[FasCategoria] [varchar](2) NOT NULL,
	[FasArtId] [int] NOT NULL,
	[FasSc1] [decimal](5, 2) NOT NULL,
	[FasSc2] [decimal](5, 2) NOT NULL,
	[FasSc3] [decimal](5, 2) NOT NULL,
	[FasMg1] [decimal](5, 2) NOT NULL,
	[FasMg2] [decimal](5, 2) NOT NULL,
	[FasMg3] [decimal](5, 2) NOT NULL,
	[FasImb] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_TbNewFasAtt_1] PRIMARY KEY CLUSTERED 
(
	[FasAtt] ASC,
	[FasCategoria] ASC,
	[FasArtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCat]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCat](
	[CatCod] [varchar](2) NOT NULL,
	[CatDesc] [varchar](30) NOT NULL,
	[CatStat] [bit] NOT NULL,
 CONSTRAINT [PK_TbCat1] PRIMARY KEY CLUSTERED 
(
	[CatCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VTbNewFasAtt]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[VTbNewFasAtt]
as

SELECT FasAtt,
       FasCategoria,
	   FasArtId,
	   FasSc1,
	   CATEGORIA = isnull((Select CatDesc from TbCat where CatCod = FasCategoria),''),
	   ARTCOD = isnull(ArtCod,''),
	   PRODOTTO = isnull(ArtDesc,'')
from TbNewFasAtt left outer join
     TbArt on ArtId = FasArtId
GO
/****** Object:  Table [dbo].[TbCor]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCor](
	[CorTipoDoc] [varchar](1) NOT NULL,
	[CorRif] [int] NOT NULL,
	[CorProg] [int] NOT NULL,
	[CorArtID] [int] NOT NULL,
	[CorCodArt] [varchar](20) NOT NULL,
	[CorDesc] [varchar](200) NOT NULL,
	[CorQuaCon] [decimal](8, 3) NOT NULL,
	[CorPrezzo] [decimal](9, 3) NOT NULL,
	[CorSc1] [decimal](5, 3) NOT NULL,
	[CorSc2] [decimal](5, 3) NOT NULL,
	[CorSc3] [decimal](5, 3) NOT NULL,
	[CorSc4] [decimal](5, 3) NOT NULL,
	[CorSc5] [decimal](5, 3) NOT NULL,
	[CorNetto] [decimal](11, 5) NOT NULL,
	[CorImporto] [decimal](11, 2) NOT NULL,
	[CorUrgenza] [bit] NOT NULL,
	[CorSpot] [bit] NOT NULL,
	[CorPromo] [varchar](1) NOT NULL,
	[CorAssortito] [bit] NOT NULL,
	[CorVariato] [bit] NOT NULL,
	[CorCampagna] [varchar](10) NOT NULL,
	[CorCiva] [smallint] NOT NULL,
	[CorCntrp] [varchar](5) NOT NULL,
	[CorCau] [smallint] NOT NULL,
	[CorMacro] [varchar](6) NOT NULL,
	[CorOrdRif] [int] NOT NULL,
	[CorOrdine] [varchar](8) NOT NULL,
	[CorArtFor] [varchar](15) NOT NULL,
	[CorPlRif] [int] NOT NULL,
	[CorPlCassa] [int] NOT NULL,
	[CorImballo] [decimal](9, 2) NOT NULL,
	[CorMg1] [decimal](5, 3) NOT NULL,
	[CorMg2] [decimal](5, 3) NOT NULL,
	[CorMg3] [decimal](5, 3) NOT NULL,
	[CorMerc] [int] NOT NULL,
	[CorBrand] [int] NOT NULL,
	[CorOrdProg] [int] NULL,
	[CorUM] [varchar](2) NOT NULL,
	[CorLotto] [varchar](20) NULL,
	[CorDataScad] [smalldatetime] NULL,
	[CorAnnota] [varchar](max) NULL,
	[CorOmaggi] [varchar](1) NULL,
 CONSTRAINT [PK_TbCor] PRIMARY KEY CLUSTERED 
(
	[CorTipoDoc] ASC,
	[CorRif] ASC,
	[CorProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFat]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFat](
	[FatRif] [int] IDENTITY(1000000,1) NOT NULL,
	[FatTipoDoc] [varchar](1) NOT NULL,
	[FatData] [smalldatetime] NOT NULL,
	[FatNum] [int] NOT NULL,
	[FatAge] [smallint] NOT NULL,
	[FatCliCons] [varchar](5) NOT NULL,
	[FatPagCod] [smallint] NOT NULL,
	[FatAbi] [int] NOT NULL,
	[FatCab] [int] NOT NULL,
	[FatEsespe] [smallint] NOT NULL,
	[FatNimp] [smallint] NOT NULL,
	[FatCau] [smallint] NOT NULL,
	[FatSculter] [decimal](4, 2) NOT NULL,
	[FatPorto] [varchar](10) NOT NULL,
	[FatVett1] [smallint] NOT NULL,
	[FatVett2] [smallint] NOT NULL,
	[FatCliFat] [varchar](5) NOT NULL,
	[FatNumReg] [smallint] NOT NULL,
	[FatBanca] [varchar](25) NULL,
	[FatTrasf] [bit] NULL,
	[FatNcolli] [smallint] NOT NULL,
	[FatNVolumi] [smallint] NOT NULL,
	[FatDMV] [varchar](12) NOT NULL,
	[FatDiff] [varchar](1) NOT NULL,
	[FatAspBeni] [varchar](50) NOT NULL,
	[FatVarDest] [varchar](100) NOT NULL,
	[FatAnnota] [varchar](100) NOT NULL,
	[FatNDep] [smallint] NOT NULL,
	[FatNote] [varchar](50) NOT NULL,
	[FatTipoFte] [varchar](4) NOT NULL,
	[FatPvv] [varchar](5) NOT NULL,
	[FatPesoNetto] [decimal](9, 3) NOT NULL,
	[FatPesoLordo] [decimal](9, 3) NOT NULL,
	[FatDataRitiro1] [smalldatetime] NULL,
	[FatDataRitiro2] [smalldatetime] NULL,
	[FatDataTrasp] [smalldatetime] NULL,
	[FatListino] [smallint] NOT NULL,
 CONSTRAINT [PK_TbFat] PRIMARY KEY CLUSTERED 
(
	[FatRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VIEW1]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE   VIEW [dbo].[VIEW1]
AS
SELECT     dbo.TbFat.FatRif, dbo.TbFat.FatTipoDoc, dbo.TbFat.FatData, dbo.TbFat.FatNum, dbo.TbFat.FatCliCons, dbo.TbFat.FatPagCod, dbo.TbFat.FatAbi, 
                      dbo.TbFat.FatCab, dbo.TbFat.FatEsespe, dbo.TbFat.FatNimp, dbo.TbFat.FatSculter, dbo.TbFat.FatNumReg, dbo.TbFat.FatBanca, dbo.TbCor.CorProg, 
                      dbo.TbCor.CorArtID, dbo.TbCor.CorCodArt, dbo.TbCor.CorDesc, dbo.TbCor.CorQuaCon, dbo.TbCor.CorPrezzo, dbo.TbCor.CorSc1, dbo.TbCor.CorSc2, 
                      dbo.TbCor.CorSc3, dbo.TbCor.CorNetto, dbo.TbCor.CorImporto, dbo.TbCor.CorPromo, dbo.TbCor.CorSpot, dbo.TbCor.CorUrgenza, 
                      dbo.TbCor.CorAssortito, dbo.TbCor.CorVariato, dbo.TbCor.CorCampagna, dbo.TbCor.CorCiva, dbo.TbCor.CorCntrp, dbo.TbCor.CorCau, 
                      dbo.TbCor.CorMacro, dbo.TbCor.CorImballo, dbo.TbFat.FatCliFat, CorOmaggi = ISNULL(CoroMAGGI,'')
FROM         dbo.TbFat INNER JOIN
                      dbo.TbCor ON dbo.TbFat.FatRif = dbo.TbCor.CorRif AND dbo.TbFat.FatTipoDoc = dbo.TbCor.CorTipoDoc





GO
/****** Object:  Table [dbo].[TbBol]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbBol](
	[BolRif] [int] IDENTITY(2000000,1) NOT NULL,
	[BolTipoDoc] [varchar](1) NOT NULL,
	[BolData] [smalldatetime] NOT NULL,
	[BolNum] [int] NOT NULL,
	[BolAge] [smallint] NOT NULL,
	[BolRifFat] [int] NOT NULL,
	[BolPrefAgg] [int] NOT NULL,
	[BolCliCons] [varchar](5) NOT NULL,
	[BolPagCod] [smallint] NOT NULL,
	[BolNrag] [int] NOT NULL,
	[BolAbi] [int] NOT NULL,
	[BolCab] [int] NOT NULL,
	[BolEsespe] [smallint] NOT NULL,
	[BolNimp] [smallint] NOT NULL,
	[BolCau] [smallint] NOT NULL,
	[BolSculter] [decimal](4, 2) NOT NULL,
	[BolPorto] [varchar](10) NOT NULL,
	[BolVett1] [smallint] NOT NULL,
	[BolVett2] [smallint] NOT NULL,
	[BolCliFat] [varchar](5) NOT NULL,
	[BolNumReg] [smallint] NOT NULL,
	[BolNumFat] [int] NULL,
	[BolFattData] [smalldatetime] NULL,
	[BolTrasf] [bit] NULL,
	[BolNColli] [smallint] NOT NULL,
	[BolCF] [varchar](1) NOT NULL,
	[BolNVolumi] [smallint] NOT NULL,
	[BolDMV] [varchar](12) NOT NULL,
	[BolAspBeni] [varchar](50) NOT NULL,
	[BolVarDest] [varchar](100) NOT NULL,
	[BolAnnota] [varchar](100) NOT NULL,
	[BolNDep] [smallint] NOT NULL,
	[BolDataTrasp] [smalldatetime] NULL,
	[BolDataRitiro1] [smalldatetime] NULL,
	[BolDataRitiro2] [smalldatetime] NULL,
	[BolTotale] [decimal](11, 2) NOT NULL,
	[BolNote] [varchar](50) NOT NULL,
	[BolTipoFte] [varchar](4) NOT NULL,
	[BolPvv] [varchar](5) NOT NULL,
	[BolPesoNetto] [decimal](9, 3) NOT NULL,
	[BolPesoLordo] [decimal](9, 3) NOT NULL,
	[BolListino] [smallint] NOT NULL,
 CONSTRAINT [PK_TbBol] PRIMARY KEY CLUSTERED 
(
	[BolRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTCau]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTCau](
	[MgCauId] [smallint] NOT NULL,
	[MgCauDesc] [varchar](30) NOT NULL,
	[MgCauGancio] [smallint] NOT NULL,
	[MgCauCli] [bit] NOT NULL,
	[MgCauFor] [bit] NOT NULL,
	[MgCauGestione] [bit] NOT NULL,
	[MgCauModulo] [bit] NOT NULL,
	[MgCauRPrezzo] [bit] NULL,
	[MgCauCErrori] [bit] NULL,
	[MgCauOmaggi] [smallint] NULL,
	[MgCauOil] [smallint] NULL,
 CONSTRAINT [PK_TbTcau] PRIMARY KEY CLUSTERED 
(
	[MgCauId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VDocBol]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE   VIEW [dbo].[VDocBol]
AS
SELECT     dbo.TbBol.BolRif, dbo.TbBol.BolTipoDoc, dbo.TbBol.BolData, dbo.TbBol.BolNum, dbo.TbBol.BolPrefAgg, dbo.TbBol.BolCliCons, dbo.TbBol.BolPagCod, BolPagDesc='',BolOperatore='',
                      dbo.TbBol.BolNrag, dbo.TbBol.BolAbi, dbo.TbBol.BolCab, dbo.TbBol.BolEsespe, dbo.TbBol.BolNimp, dbo.TbBol.BolCliFat, dbo.TbBol.BolNumReg, 
                      dbo.TbBol.BolNumFat, dbo.TbBol.BolFattData, ISNULL(dbo.TbBol.BolTrasf, 0) AS BolTrasf, VDOX.dbo.TbAna.AnaDesc, dbo.TbBol.BolRifFat, 
                      CAST(ISNULL(dbo.TbFat.FatNum, 0) AS bit) AS fatturato, isnull(fatnum, 0) AS FatNum, FatData = CASE WHEN (BolRifFat = 0) 
                      THEN '' ELSE CONVERT(varchar, FatData, 103) END,BolNDep,Causale=(select MgCauDesc from TbTCau where BolCau=MgCauId),BolTotale
FROM         dbo.TbBol INNER JOIN
                      VDOX.dbo.TbAna ON VDOX.dbo.TbAna.AnaCod = dbo.TbBol.BolCliFat LEFT OUTER JOIN
                      dbo.TbFat ON dbo.TbBol.BolRifFat = dbo.TbFat.FatRif







GO
/****** Object:  Table [dbo].[TbDcg]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbDcg](
	[DcgNumRif] [int] NOT NULL,
	[DcgTipo] [varchar](1) NOT NULL,
	[DcgTrasf] [bit] NOT NULL,
	[DcgData] [smalldatetime] NOT NULL,
	[DcgRegistro] [smallint] NOT NULL,
	[DcgNumero] [int] NOT NULL,
	[DcgCli] [varchar](5) NOT NULL,
	[DcgCodAge] [smallint] NOT NULL,
	[DcgImp1] [decimal](12, 2) NOT NULL,
	[DcgCodIva1] [smallint] NOT NULL,
	[DcgIva1] [decimal](12, 2) NOT NULL,
	[DcgCptCon1] [varchar](5) NOT NULL,
	[DcgImp2] [decimal](12, 2) NOT NULL,
	[DcgCodIva2] [smallint] NOT NULL,
	[DcgIva2] [decimal](12, 2) NOT NULL,
	[DcgCptCon2] [varchar](5) NOT NULL,
	[DcgImp3] [decimal](12, 2) NOT NULL,
	[DcgCodIva3] [smallint] NOT NULL,
	[DcgIva3] [decimal](12, 2) NOT NULL,
	[DcgCptCon3] [varchar](5) NOT NULL,
	[DcgImp4] [decimal](12, 2) NOT NULL,
	[DcgCodIva4] [smallint] NOT NULL,
	[DcgIva4] [decimal](12, 2) NOT NULL,
	[DcgCptCon4] [varchar](5) NOT NULL,
	[DcgImp5] [decimal](12, 2) NOT NULL,
	[DcgCodIva5] [smallint] NOT NULL,
	[DcgIva5] [decimal](12, 2) NOT NULL,
	[DcgCptCon5] [varchar](5) NOT NULL,
	[DcgImp6] [decimal](12, 2) NOT NULL,
	[DcgCodIva6] [smallint] NOT NULL,
	[DcgIva6] [decimal](12, 2) NOT NULL,
	[DcgCptCon6] [varchar](5) NOT NULL,
	[DcgImp7] [decimal](12, 2) NOT NULL,
	[DcgCodIva7] [smallint] NOT NULL,
	[DcgIva7] [decimal](12, 2) NOT NULL,
	[DcgCptCon7] [varchar](5) NOT NULL,
	[DcgImp8] [decimal](12, 2) NOT NULL,
	[DcgCodIva8] [smallint] NOT NULL,
	[DcgIva8] [decimal](12, 2) NOT NULL,
	[DcgCptCon8] [varchar](5) NOT NULL,
	[DcgImp9] [decimal](12, 2) NOT NULL,
	[DcgCodIva9] [smallint] NOT NULL,
	[DcgIva9] [decimal](12, 2) NOT NULL,
	[DcgCptCon9] [varchar](5) NOT NULL,
	[DcgImp10] [decimal](12, 2) NOT NULL,
	[DcgCodIva10] [smallint] NOT NULL,
	[DcgIva10] [decimal](12, 2) NOT NULL,
	[DcgCptCon10] [varchar](5) NOT NULL,
	[DcgImp11] [decimal](12, 2) NOT NULL,
	[DcgCodIva11] [smallint] NOT NULL,
	[DcgIva11] [decimal](12, 2) NOT NULL,
	[DcgCptCon11] [varchar](5) NOT NULL,
	[DcgAcconti] [decimal](12, 2) NOT NULL,
	[DcgNBol] [int] NOT NULL,
	[DcgTotMerce] [decimal](12, 2) NOT NULL,
	[DcgSpeBolli] [decimal](12, 2) NOT NULL,
	[DcgSpeBolliCi] [smallint] NOT NULL,
	[DcgSpeRb] [decimal](12, 2) NOT NULL,
	[DcgSpeRbCi] [smallint] NOT NULL,
	[DcgRieImp1] [decimal](12, 2) NOT NULL,
	[DcgRieCi1] [smallint] NOT NULL,
	[DcgRieIva1] [decimal](12, 2) NOT NULL,
	[DcgRieImp2] [decimal](12, 2) NOT NULL,
	[DcgRieCi2] [smallint] NOT NULL,
	[DcgRieIva2] [decimal](12, 2) NOT NULL,
	[DcgRieImp3] [decimal](12, 2) NOT NULL,
	[DcgRieCi3] [smallint] NOT NULL,
	[DcgRieIva3] [decimal](12, 2) NOT NULL,
	[DcgRieImp4] [decimal](12, 2) NOT NULL,
	[DcgRieCi4] [smallint] NOT NULL,
	[DcgRieIva4] [decimal](12, 2) NOT NULL,
	[DcgRieImp5] [decimal](12, 2) NOT NULL,
	[DcgRieCi5] [smallint] NOT NULL,
	[DcgRieIva5] [decimal](12, 2) NOT NULL,
	[DcgRieImp6] [decimal](12, 2) NOT NULL,
	[DcgRieCi6] [smallint] NOT NULL,
	[DcgRieIva6] [decimal](12, 2) NOT NULL,
	[DcgRieImp7] [decimal](12, 2) NOT NULL,
	[DcgRieCi7] [smallint] NOT NULL,
	[DcgRieIva7] [decimal](12, 2) NOT NULL,
	[DcgRieTotImp] [decimal](12, 2) NOT NULL,
	[DcgRieTotEse] [decimal](12, 2) NOT NULL,
	[DcgRieTotIva] [decimal](12, 2) NOT NULL,
	[DcgRieTotFat] [decimal](12, 2) NOT NULL,
	[DcgRieTotOma] [decimal](12, 2) NOT NULL,
	[DcgRieTotAcc] [decimal](12, 2) NOT NULL,
	[DcgRieTotale] [decimal](12, 2) NOT NULL,
	[DcgRieScCond] [decimal](12, 2) NOT NULL,
	[DcgRieNetto] [decimal](12, 2) NOT NULL,
	[DcgCodPag] [smallint] NOT NULL,
	[DcgSculter] [decimal](5, 2) NOT NULL,
	[DcgRieTotSconto] [decimal](12, 2) NOT NULL,
	[DcgSpeImb] [decimal](12, 2) NOT NULL,
	[DcgSpeImbCi] [smallint] NOT NULL,
 CONSTRAINT [PK_TbDcg] PRIMARY KEY CLUSTERED 
(
	[DcgNumRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VDocFat]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[VDocFat]
AS
SELECT     dbo.TbFat.FatRif, dbo.TbFat.FatTipoDoc, dbo.TbFat.FatNum, dbo.TbFat.FatData, dbo.TbFat.FatCliCons, dbo.TbFat.FatPagCod, FatPagDesc=(select Pagdesc from coge.dbo.tbpag where fatpagcod=pagcod),dbo.TbFat.FatAbi,  
                      dbo.TbFat.FatCab, dbo.TbFat.FatEsespe, dbo.TbFat.FatNimp, dbo.TbFat.FatCliFat, dbo.TbFat.FatNumReg, ISNULL(dbo.TbFat.FatTrasf, 0) AS FatTrasf, 
                      vdox.dbo.TbAna.AnaDesc, dbo.TbFat.FatDiff,FatNDep, FatImponibile = DcgRieTotImp, FatIva = DcgRieTotIva, FatEsente = DcgRieTotEse, Fattotale = DcgRieTotale
FROM         dbo.TbFat INNER JOIN
             tBdCG ON DcgNumrif = FATRIF INNER JOIN
             vdox.dbo.TbAna ON dbo.TbFat.FatCliCons = vdox.dbo.TbAna.AnaCod
WHERE     (vdox.dbo.TbAna.AnaGrp = 'CL')
GO
/****** Object:  Table [dbo].[TbPvv]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPvv](
	[PvvCli] [varchar](5) NOT NULL,
	[PvvPuntoVendita] [varchar](5) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VPuntiVendita]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   View [dbo].[VPuntiVendita]
as
Select PvvCli,PvvPuntoVendita,Anagrafica=AnaRag1 + ' ' + AnaIndirizzo  + ' ' + AnaCap + ' ' + AnaCitta  + ' ' + AnaProv 
from TbPvv inner join 
vdox.dbo.tbana on Anacod=PvvPuntoVendita
GO
/****** Object:  Table [dbo].[TbSos]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSos](
	[SosRif] [int] IDENTITY(2000000,1) NOT NULL,
	[SosTipoDoc] [varchar](1) NOT NULL,
	[SosData] [smalldatetime] NOT NULL,
	[SosNum] [int] NOT NULL,
	[SosAge] [smallint] NOT NULL,
	[SosCliCons] [varchar](5) NOT NULL,
	[SosPagCod] [smallint] NOT NULL,
	[SosNrag] [int] NOT NULL,
	[SosAbi] [int] NOT NULL,
	[SosCab] [int] NOT NULL,
	[SosEsespe] [smallint] NOT NULL,
	[SosNimp] [smallint] NOT NULL,
	[SosCau] [smallint] NOT NULL,
	[SosSculter] [decimal](5, 3) NOT NULL,
	[SosCliFat] [varchar](5) NOT NULL,
	[SosTrasf] [bit] NOT NULL,
	[SosWebRif] [int] NOT NULL,
	[SosCreato] [smalldatetime] NOT NULL,
	[SosOperatore] [varchar](20) NOT NULL,
	[SosNDep] [smallint] NOT NULL,
	[SosVardest] [varchar](100) NOT NULL,
	[SosFP] [bit] NOT NULL,
	[SosNote] [varchar](200) NOT NULL,
	[SosPvv] [varchar](5) NOT NULL,
	[SosConsegna] [smalldatetime] NULL,
	[SosPrivato] [int] NOT NULL,
	[SosOraRitiro] [datetime] NULL,
	[SosStampato] [bit] NOT NULL,
	[SosStampaNum] [int] NOT NULL,
	[SosAccesso] [varchar](1) NOT NULL,
	[SosTipoConsegna] [varchar](1) NOT NULL,
	[SosMailConferma] [bit] NOT NULL,
	[SosMailPronto] [bit] NOT NULL,
 CONSTRAINT [PK_TbSos] PRIMARY KEY CLUSTERED 
(
	[SosRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VDocSos]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE   VIEW [dbo].[VDocSos]
AS
SELECT     dbo.TbSos.SosRif, dbo.TbSos.SosTipoDoc, dbo.TbSos.SosData, dbo.TbSos.SosNum, dbo.TbSos.SosAge, dbo.TbSos.SosCliCons, dbo.TbSos.SosPagCod,SosPagDesc='', 
                      dbo.TbSos.SosNrag, dbo.TbSos.SosAbi, dbo.TbSos.SosCab, dbo.TbSos.SosEsespe, dbo.TbSos.SosNimp, dbo.TbSos.SosCau,
                      dbo.TbSos.SosCliFat, ISNULL(dbo.TbSos.SosTrasf, 0) AS SosTrasf, VDOX.dbo.TbAna.AnaDesc,SosNDep
FROM         dbo.TbSos INNER JOIN
                      VDOX.dbo.TbAna ON VDOX.dbo.TbAna.AnaCod = dbo.TbSos.SosCliFat







GO
/****** Object:  View [dbo].[VIEW2]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[VIEW2]
AS
SELECT     TOP 100 PERCENT dbo.TbBol.BolNum, dbo.TbBol.BolData, dbo.TbFat.FatRif, dbo.TbBol.BolRif, ISNULL(dbo.TbBol.BolTrasf, 0) AS BolTrasf
FROM         dbo.TbFat INNER JOIN
                      dbo.TbBol ON dbo.TbFat.FatRif = dbo.TbBol.BolRifFat
ORDER BY dbo.TbFat.FatRif, dbo.TbBol.BolData, dbo.TbBol.BolNum





GO
/****** Object:  Table [dbo].[TbArtDG]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbArtDG](
	[ADGArtCod] [varchar](20) NOT NULL,
	[ADGDogana] [varchar](16) NOT NULL,
 CONSTRAINT [PK_TbArtDG] PRIMARY KEY CLUSTERED 
(
	[ADGArtCod] ASC,
	[ADGDogana] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTDis]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTDis](
	[TDisID] [int] IDENTITY(1,1) NOT NULL,
	[TDisCod] [varchar](20) NOT NULL,
	[TDisDesc] [varchar](50) NOT NULL,
	[TDisItaliano] [varbinary](max) NULL,
	[TDisFrancese] [varbinary](max) NULL,
	[TDisInglese] [varbinary](max) NULL,
	[TDisTedesco] [varbinary](max) NULL,
	[TDisSpagnolo] [varbinary](max) NULL,
	[TDisEtixFoglio] [smallint] NOT NULL,
	[TDisFileITA] [varchar](1000) NOT NULL,
	[TDisCat] [varchar](2) NULL,
 CONSTRAINT [PK_TbTDis0] PRIMARY KEY CLUSTERED 
(
	[TDisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbLis]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbLis](
	[LisId] [int] NOT NULL,
	[LisValiditaDal] [smalldatetime] NOT NULL,
	[LisVendita] [decimal](10, 3) NOT NULL,
 CONSTRAINT [PK_TbLis] PRIMARY KEY CLUSTERED 
(
	[LisId] ASC,
	[LisValiditaDal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VArtCat]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE   VIEW [dbo].[VArtCat]
as
SELECT ArtId, 
       ArtCod, 
	   ArtDesc, 
	   ArtMinidesc, 
	   ArtCat, 
	   ArtCodiva, 
       ArtUmisura, 
	   ArtCptcon,
	   ArtFlagAttivo,
	   ArtUmVen, 	  
	   ArtUmTec, 
	   ArtCiAcq, 
	   ArtCptAcq,
	   CatDesc = isnull(CatDesc,''),                        
       PERCIVA = (select CiiAli from Coge.dbo.TbCii where CiiCod = DBO.TCI(ArtCodiva)),
       DESCIVA = (select CiiDES from Coge.dbo.TbCii where CiiCod = DBO.TCI(ArtCodiva)),                        
	   ArtDogana = isnull((select ADGDogana from TbArtDG where ADGArtCod = ArtCod),''),
	   PERCIVAQ = (select CiiAli from Coge.dbo.TbCii where CiiCod = DBO.TCI(ArtCiAcq)),
       DESCIVAQ = (select CiiDES from Coge.dbo.TbCii where CiiCod = DBO.TCI(ArtCiAcq)),						
	   LISTINO = isnull(LisVendita,0),
	   VALIDITA = Isnull(LisValiditaDal,CAST(getdate() AS DATE)),
	   ArtBarcode,
	   ArtNumImballo,
	   ArtIngredienti,
	   ArtBilancia,
	   ArtCodbil,
	   ArtBarcodeForn,
	   ArtEAN13,
	   ArtEtixFoglio=ISNULL((select TDisEtixFoglio from TbTDis where TDisID = ArtDibase),0),
	   ArtUltLotto,
	   ArtUltScadenza,
	   ArtIngrITA = (select TDisItaliano from TbTDis WHERE TDisID = ArtDibase),
	   ArtIngrFRA = (select TDisFrancese from TbTDis WHERE TDisID = ArtDibase ),
	   ArtIngrENG = (select TDisInglese from TbTDis WHERE TDisID = ArtDibase),
	   ArtIngrGER= (select TDisTedesco from TbTDis WHERE TDisID = ArtDibase ),
	   ArtDiBase,
	   ArtUlt_modifica,
	   ArtUtente,
	   ArtAUlt_modifica,
	   ArtAUtente
from TbArt Left outer join
     TbCat ON CatCod = ArtCat JOIN
	 Tblis on lisid = Artid
WHERE (LisValiditaDal IN (SELECT MAX(LisValiditaDal) FROM TbLis WHERE LisId = Artid AND LisValiditaDal <= cast(getdate() as date)))
GO
/****** Object:  Table [dbo].[TBBLOCK]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBBLOCK](
	[IDBLOCCO] [int] NOT NULL,
	[IDRIFERIM] [int] NOT NULL,
	[IDNUMREG] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Semaforo]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Semaforo](
	[SemID] [int] IDENTITY(1,1) NOT NULL,
	[SemProc] [varchar](50) NULL,
 CONSTRAINT [PK_Semaforo] PRIMARY KEY CLUSTERED 
(
	[SemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VIEW6]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[VIEW6]
AS

SELECT     TOP 100 PERCENT IDBLOCCO,SemProc, MIN(FatNum) AS DAFT, MAX(FatNum) AS AAFT, 
                      ISNULL(FatTrasf, 0) AS CONTROLLO, dbo.TbFat.FatNumReg,FatNDep
FROM         dbo.TbFat INNER JOIN
                      dbo.TBBLOCK ON dbo.TbFat.FatRif = dbo.TBBLOCK.IDRIFERIM INNER JOIN
                      dbo.Semaforo ON dbo.TBBLOCK.IDBLOCCO = dbo.Semaforo.SemID
GROUP BY IDBLOCCO, SemProc, FatTipoDoc, ISNULL(FatTrasf, 0), FatNumReg, FatNDep
HAVING      (FatTipoDoc = 'F')
ORDER BY MAX(FatNum) DESC





GO
/****** Object:  Table [dbo].[TbVettori]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbVettori](
	[VetCod] [smallint] NOT NULL,
	[VetCodFor] [varchar](5) NOT NULL,
	[VetAttivo] [bit] NOT NULL,
	[VetNazione] [varchar](2) NOT NULL,
 CONSTRAINT [PK_TbVettori] PRIMARY KEY CLUSTERED 
(
	[VetCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VVETTORI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VVETTORI]
as
select distinct VetCod,VetNazione,VetCodFor,VetAttivo,TbAna.* from tbVettori
 inner join vdox.dbo.tbana on anacod=Vetcodfor and anagrp='FO'
 

GO
/****** Object:  Table [dbo].[TbFor]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFor](
	[FoCod] [varchar](5) NOT NULL,
	[FoPagam] [smallint] NOT NULL,
	[FoPagDesc] [varchar](60) NOT NULL,
	[FoAbi] [int] NOT NULL,
	[FoCab] [int] NOT NULL,
	[FoCC] [varchar](12) NOT NULL,
	[FoCinEur] [varchar](2) NOT NULL,
	[FoCin] [varchar](1) NOT NULL,
	[FoCodBan] [smallint] NOT NULL,
	[FoSoggRit] [bit] NOT NULL,
	[FoAttivo] [bit] NULL,
	[FoBlackList] [bit] NULL,
	[FoSbloccoFat] [bit] NULL,
	[FoGruppoAt] [smallint] NULL,
	[FoNazione] [varchar](2) NOT NULL,
	[FoEnasarco] [smallint] NOT NULL,
	[FoMatGrezzo] [bit] NOT NULL,
 CONSTRAINT [PK_TbFor] PRIMARY KEY CLUSTERED 
(
	[FoCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[DXFornitori]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












CREATE VIEW [dbo].[DXFornitori]
AS
SELECT  TOP 100 PERCENT  Cod=AnaCod ,[Ragione Sociale]=AnaDesc,[Partita Iva]=AnaPiva,[Codice Fisc]=AnaCfis,[Part.Iva Estera]=AnaPivaEst,Indirizzo=AnaIndirizzo,Cap=AnaCap,Città=AnaCitta,Pr=AnaProv,[Nazione]=FoNazione,
					  Telefono=AnaTel1,Fax=AnaFax,[Telefono 2]=AnaTel2,Cellulare=AnaTel3,Email=AnaEmail,Web=AnaWww,Contatto=AnaResp,
                      Pagamento=CAST(PagCod AS varchar) + ' ' + PagDesc,Banca=CaDescFt,
					  Iban=ISNULL(CaPaese, 'IT') + REPLICATE('0', 2 - DATALENGTH(FoCinEur)) + FoCinEur + REPLICATE(' ',1 - DATALENGTH(FoCin)) + FoCin + REPLICATE('0', 5 - DATALENGTH(CAST(CaAbi AS varchar))) 
                      + CAST(CaAbi AS varchar) + REPLICATE('0', 5 - DATALENGTH(CAST(CaCab AS varchar))) + CAST(CaCab AS varchar) + REPLICATE('0', 12 - DATALENGTH(FoCC)) + FoCC, 
                      ABI=FoAbi,CAB=FoCab,[Cod.Banca]=FocodBan,[Sogg.Rit]=FoSoggRit,[Sblocco Ft]=FoSbloccoFat,[G r u p p o]=FoGruppoAt,Attivo=FoAttivo,[No Rubrica]=AnaNoRubrica 
   FROM         VDOX.dbo.TbAna 
   INNER JOIN   TbFor ON VDOX.dbo.TbAna.AnaCod = FoCod 
   LEFT OUTER JOIN COGE.dbo.TbPag ON FoPagam = COGE.dbo.TbPag.PagCod
   LEFT OUTER JOIN COGE.dbo.TbCab ON FoAbi = COGE.dbo.TbCab.CaAbi AND FoCab = COGE.dbo.TbCab.CaCab
   WHERE     (VDOX.dbo.TbAna.AnaGrp = 'FO') ORDER BY FOCOD

   	  

GO
/****** Object:  Table [dbo].[TBDXMENU]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBDXMENU](
	[BARGROUP] [smallint] NOT NULL,
	[BARID] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VDXMenu]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[VDXMenu]
AS
SELECT     VDOX.dbo.TbGrupLav.*, dbo.TbDXMenu.BARID AS Indice
FROM         VDOX.dbo.TbGrupLav LEFT OUTER JOIN
                      dbo.TbDXMenu ON VDOX.dbo.TbGrupLav.GrupLavId = dbo.TbDXMenu.BARGROUP






GO
/****** Object:  Table [dbo].[TbFteStato_Doc]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFteStato_Doc](
	[FteStatoCod] [smallint] NOT NULL,
	[FteStatoDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbFteStato_Doc] PRIMARY KEY CLUSTERED 
(
	[FteStatoCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFte]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFte](
	[FteRif] [int] NOT NULL,
	[FteDDTNum] [varchar](20) NOT NULL,
	[FteDDTData] [smalldatetime] NULL,
	[FteOrdNum] [varchar](20) NOT NULL,
	[FteOrdData] [smalldatetime] NULL,
	[FteCIG] [varchar](20) NOT NULL,
	[FteModPag] [varchar](4) NOT NULL,
	[FteNatura] [varchar](2) NOT NULL,
	[FteProgrInvio] [int] NOT NULL,
	[FteErr_Cod] [smallint] NOT NULL,
	[FteErr_desc] [varchar](2000) NOT NULL,
	[FteDaInviare] [bit] NOT NULL,
	[FteIdElab] [int] NOT NULL,
	[FteElaborataSia] [bit] NOT NULL,
	[FteDataOra] [datetime] NULL,
	[FteProgSIA] [int] NOT NULL,
	[FteStato] [varchar](20) NOT NULL,
	[FteNomeSDI] [varchar](50) NOT NULL,
	[FteNomeFile] [varchar](30) NOT NULL,
	[FteKBFile] [int] NOT NULL,
	[FteAnno] [smallint] NOT NULL,
	[FteNumero] [int] NOT NULL,
	[FteRegistro] [smallint] NOT NULL,
	[FteData] [smalldatetime] NULL,
	[FteAlfanum] [varchar](10) NOT NULL,
	[FteStatoAttive] [smallint] NOT NULL,
	[FteUltOp] [varchar](50) NOT NULL,
	[FteCodStato] [smallint] NOT NULL,
	[FteStatoDaPrenotare] [bit] NOT NULL,
	[FteStatoDaRecuperare] [bit] NOT NULL,
	[FteIdConservazione] [int] NOT NULL,
	[FteNote] [varchar](2000) NOT NULL,
	[FteCliente] [varchar](5) NOT NULL,
	[FteRagSoc] [varchar](100) NOT NULL,
	[FteTotFat] [decimal](12, 2) NULL,
	[FteCUP] [varchar](15) NOT NULL,
	[FteBlueNext] [bit] NOT NULL,
	[FtePA] [bit] NOT NULL,
	[FteIdBN] [varchar](50) NOT NULL,
	[FteIdSDI] [varchar](30) NOT NULL,
	[FteNomeFileBN] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbFte] PRIMARY KEY CLUSTERED 
(
	[FteRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VFTEAttive]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VFTEAttive]
as

SELECT ANNO=datepart(year,fteData),
       FATNUMREG=Fteregistro,
	   FATNUM=case WHEN FteRif < 20000000 THEN CAST(ftenumero AS VARCHAR) ELSE FteAlfanum end,
	   FATDATA=FteData,
	   CLIE=FteCliente,
	   RAGSOC = FteRagSoc,
	   FteDaInviare,FteIdElab,FteProgSIA,FteDataOra,FteNomeFile,FteRif,FteErr_Cod,FteErr_desc,FteUltOp, 
	   DcgRieTotale = FteTotFat,
	   FteCodStato,
	   STATO=ISNULL((Select FteStatoDesc from TbFteStato_Doc where FteStatoCod = FteCodStato),''),
	   FteNomeSDI,FteIdConservazione,FteNote,
	   FteBlueNext,FtePA,FteIdBN,FteIdSDI,FteNomeFileBN
FROM TbFte 


GO
/****** Object:  Table [dbo].[TbCli]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCli](
	[ClCod] [varchar](5) NOT NULL,
	[ClPagam] [smallint] NOT NULL,
	[ClPagDesc] [varchar](60) NOT NULL,
	[ClAbi] [int] NOT NULL,
	[ClCab] [int] NOT NULL,
	[ClCntRid] [varchar](30) NOT NULL,
	[ClCodAge] [varchar](3) NOT NULL,
	[ClCodMan] [varchar](3) NOT NULL,
	[ClCodivani] [smallint] NOT NULL,
	[ClClifatt] [varchar](5) NOT NULL,
	[ClDirezione] [smallint] NULL,
	[ClAttivo] [bit] NOT NULL,
	[ClEseSpe] [bit] NOT NULL,
	[ClCodBan] [smallint] NOT NULL,
	[ClCC] [varchar](12) NOT NULL,
	[ClCinEur] [varchar](2) NOT NULL,
	[ClCin] [varchar](1) NOT NULL,
	[ClInviaMail] [bit] NOT NULL,
	[ClMaxScop] [decimal](9, 2) NOT NULL,
	[Cl1RidData] [smalldatetime] NULL,
	[ClNazione] [varchar](2) NOT NULL,
	[ClRegione] [varchar](40) NULL,
	[ClSc1] [decimal](4, 2) NOT NULL,
	[ClSc2] [decimal](4, 2) NOT NULL,
	[ClPortoFranco] [bit] NOT NULL,
	[ClGrCanale] [smallint] NOT NULL,
	[ClBlackList] [bit] NULL,
	[ClStato] [bit] NOT NULL,
	[ClSculter] [decimal](4, 2) NOT NULL,
	[ClMailAmministra] [varchar](100) NOT NULL,
	[ClMailComunica] [varchar](100) NOT NULL,
	[ClClasseFattura] [varchar](1) NOT NULL,
	[ClCreateDataOra] [datetime] NULL,
	[ClUpdateDataOra] [datetime] NULL,
	[ClOrario] [varchar](100) NOT NULL,
	[ClBolRag] [bit] NOT NULL,
	[ClEspSconti] [varchar](1) NOT NULL,
	[ClSplitPay] [bit] NOT NULL,
 CONSTRAINT [PK_TbCli] PRIMARY KEY CLUSTERED 
(
	[ClCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTipoCons]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTipoCons](
	[ConsTipo] [varchar](1) NOT NULL,
	[ConsDesc] [varchar](35) NOT NULL,
 CONSTRAINT [PK_TbTipoCons] PRIMARY KEY CLUSTERED 
(
	[ConsTipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VPRINTSOS]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  VIEW [dbo].[VPRINTSOS]
AS
SELECT TOP 100 PERCENT SOSRIF,SOSNUM,SOSDATA,SOSCLICONS,SOSCLIFAT,CORPROG,CORARTID,CORCODART,SOSWEBRIF,
DESCRIZIONE = CORDESC,
URGENZA = CASE WHEN CorUrgenza = 1 THEN 'U' ELSE '' END,
NETTO = CASE WHEN CorSc1 = 0 AND CorSc2 = 0 AND CorSc3 = 0 THEN '$' ELSE '' END,
UM = CorUM,
QTA = CORQUACON,
PREZZO = CORPREZZO,
CORSC1,
CORSC2,
CORSC3,
CORIMPORTO = CASE WHEN CORCAU = 3 then - (CORIMPORTO + corimballo) else CORIMPORTO + corimballo end,
D_ANARAG1 = D.ANARAG1,
D_ANARAG2 = D.ANARAG2,
D_ANAINDIRIZZO = D.ANAINDIRIZZO,
D_ANACAP = D.ANACAP,
D_ANACITTA = D.ANACITTA,
D_ANAPROV = D.ANAPROV,
D_CITTA = D.ANACAP + ' - ' +  D.ANACITTA + ' - ' +  D.ANAPROV,
ANARAG1 = A.ANARAG1,
ANARAG2 = A.ANARAG2,
ANAINDIRIZZO =A.ANAINDIRIZZO,
ANACAP = A.ANACAP,
ANACITTA = A.ANACITTA,
ANAPROV = A.ANAPROV,
ANACFIS = A.ANACFIS,
ANAPIVA = A.ANAPIVA,
ANAPIVAEST = A.ANAPIVAEST,
CITTA = A.ANACAP + ' - ' +  A.ANACITTA + ' - ' +  A.ANAPROV,
SOSTIPODOC,
SOSNOTE,
SOSCONSEGNA,
TIPO_CONSEGNA = ISNULL(ConsDesc,''),
SOSSTAMPATO
FROM TBSOS
INNER JOIN TBCOR ON SOSRIF = CORRIF AND SOSTIPODOC = CORTIPODOC 
--LEFT JOIN TbPvv ON PvvCli = SosCliFat
LEFT JOIN VDOX.DBO.TBANA AS A ON A.ANACOD = SOSCLIFAT AND A.ANAGRP = 'CL'
LEFT JOIN VDOX.DBO.TBANA AS D ON D.ANACOD = CASE WHEN SosPvv='' THEN SOSCLICONS ELSE SOSPVV END AND D.ANAGRP = 'CL'
LEFT OUTER JOIN TBCLI ON CLCOD=SOSCLICONS 
LEFT OUTER JOIN TbTipoCons on ConsTipo = SosTipoConsegna

WHERE SOSTIPODOC = 'S'




GO
/****** Object:  Table [dbo].[TbFteStato_DocBN]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFteStato_DocBN](
	[FteStatoCod] [smallint] NOT NULL,
	[FteStatoDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbFteStato_DocBN] PRIMARY KEY CLUSTERED 
(
	[FteStatoCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VFTEAttiveBN]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VFTEAttiveBN]
as

SELECT ANNO=datepart(year,fteData),
       FATNUMREG=Fteregistro,
	   FATNUM=case WHEN FteRif < 20000000 THEN CAST(ftenumero AS VARCHAR) ELSE FteAlfanum end,
	   FATDATA=FteData,
	   CLIE=FteCliente,
	   RAGSOC = FteRagSoc,
	   FteDaInviare,FteIdElab,FteProgSIA,FteDataOra,FteNomeFile,FteRif,FteErr_Cod,FteErr_desc,FteUltOp, 
	   DcgRieTotale = FteTotFat,
	   FteCodStato,
	   STATO=ISNULL((Select FteStatoDesc from TbFteStato_DocBN where FteStatoCod = FteCodStato),''),
	   FteNomeSDI,FteIdConservazione,FteNote,
	   FteBlueNext,FtePA,FteIdBN,FteIdSDI,FteNomeFileBN,
	   PARTIVA = (SELECT TOP 1 AnaPiva from vdox.dbo.TbAna where AnaGrp = 'AZ' and AnaPiva > '')
FROM TbFte 


GO
/****** Object:  View [dbo].[XINGREDIENTI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   VIEW [dbo].[XINGREDIENTI]
AS

SELECT ArtCod,
       ArtDesc,
	   ArtBarcode,
	   ArtIngredienti
FROM TbArt
GO
/****** Object:  View [dbo].[CRCLI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRCLI]
AS
SELECT     VDOX.dbo.TbAna.AnaDesc, VDOX.dbo.TbAna.AnaPiva, VDOX.dbo.TbAna.AnaCfis, VDOX.dbo.TbAna.AnaIndirizzo, VDOX.dbo.TbAna.AnaCap, 
                      VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, VDOX.dbo.TbAna.AnaTel1, VDOX.dbo.TbAna.AnaTel2, VDOX.dbo.TbAna.AnaTel3, 
                      VDOX.dbo.TbAna.AnaWww, VDOX.dbo.TbAna.AnaEmail, VDOX.dbo.TbAna.AnaGrp, VDOX.dbo.TbAna.AnaCod, VDOX.dbo.TbAna.AnaResp, 
                      VDOX.dbo.TbAna.AnaNote, VDOX.dbo.TbAna.AnaFax, VDOX.dbo.TbAna.AnaRag1, VDOX.dbo.TbAna.AnaRag2, VDOX.dbo.TbAna.AnaPivaEst, 
                      ISNULL(COGE.dbo.TbPag.PagDesc, ' ') AS pagdesc, dbo.TbCli.ClPagam, dbo.TbCli.ClAbi, dbo.TbCli.ClCab, dbo.TbCli.ClCntRid, dbo.TbCli.ClCodBan, 
                      dbo.TbCli.ClCC, dbo.TbCli.ClAttivo
FROM         VDOX.dbo.TbAna INNER JOIN
                      dbo.TbCli ON VDOX.dbo.TbAna.AnaCod = dbo.TbCli.ClCod LEFT OUTER JOIN
                      COGE.dbo.TbPag ON dbo.TbCli.ClPagam = COGE.dbo.TbPag.PagCod
WHERE     (VDOX.dbo.TbAna.AnaGrp = 'CL')
GO
/****** Object:  Table [dbo].[TbOpzioniTorte]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbOpzioniTorte](
	[OpzID] [int] IDENTITY(1,1) NOT NULL,
	[OpzCat] [char](1) NOT NULL,
	[OpzDesc] [varchar](100) NOT NULL,
	[OpzAttivo] [bit] NOT NULL,
	[OpzOrd] [int] NULL,
	[OpzIngredienti] [varchar](max) NOT NULL,
	[OpzIcona] [smallint] NOT NULL,
	[OpzBagna] [bit] NOT NULL,
 CONSTRAINT [PK_TbOpzioniTorte] PRIMARY KEY CLUSTERED 
(
	[OpzID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbOrdTorte]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbOrdTorte](
	[OrdtRif] [int] IDENTITY(1,1) NOT NULL,
	[OrdtNum] [int] NOT NULL,
	[OrdtData] [smalldatetime] NOT NULL,
	[OrdtCliente] [varchar](5) NOT NULL,
	[OrdtPrivati] [int] NOT NULL,
	[OrdtDataRitiro] [smalldatetime] NULL,
	[OrdOraRitiro] [datetime] NULL,
	[OrdtQta] [smallint] NOT NULL,
	[OrdTNAdulti] [smallint] NOT NULL,
	[OrdtNBambini] [smallint] NOT NULL,
	[OrdtPesoKg] [decimal](6, 3) NOT NULL,
	[OrdtTorta] [int] NOT NULL,
	[OrdtBase] [int] NOT NULL,
	[OrdtPersBase] [varchar](100) NOT NULL,
	[OrdtBagna] [int] NOT NULL,
	[OrdtOpEventi] [int] NOT NULL,
	[OrdtPersOpEventi] [varchar](100) NOT NULL,
	[OrdtFotoForma] [varbinary](max) NULL,
	[OrdtFarcitura] [int] NOT NULL,
	[OrdtFarcitura2] [int] NOT NULL,
	[OrdtFarcitura3] [int] NOT NULL,
	[OrdtPersFarcitura] [varchar](100) NOT NULL,
	[OrdtBordoDec] [int] NOT NULL,
	[OrdtPersBordoDec] [varchar](100) NOT NULL,
	[OrdtFotoDecorazione] [varbinary](max) NULL,
	[OrdtDecSuperficiale] [int] NOT NULL,
	[OrdtPersDecSuperficiale] [varchar](100) NOT NULL,
	[OrdtUltStrato] [int] NOT NULL,
	[OrdtPersUltStrato] [varchar](100) NOT NULL,
	[OrdtFrase] [varchar](500) NOT NULL,
	[OrdtAllergie] [varchar](100) NOT NULL,
	[OrdtImmagineNum] [varchar](20) NOT NULL,
	[OrdtImmagine] [varbinary](max) NULL,
	[OrdtNote] [varchar](250) NOT NULL,
	[OrdtNoFrutta] [varchar](100) NOT NULL,
	[OrdtCanaleFoto] [varchar](100) NOT NULL,
	[OrdtUnoQta] [smallint] NOT NULL,
	[OrdtUnoColore] [int] NOT NULL,
	[OrdtDueQta] [smallint] NOT NULL,
	[OrdtDueColore] [int] NOT NULL,
	[OrdtTreQta] [smallint] NOT NULL,
	[OrdtTreColore] [int] NOT NULL,
	[OrdtQuattroQta] [smallint] NOT NULL,
	[OrdtQuattroColore] [int] NOT NULL,
	[OrdtCinqueQta] [smallint] NOT NULL,
	[OrdtCinqueColore] [int] NOT NULL,
	[OrdtSeiQta] [smallint] NOT NULL,
	[OrdtSeiColore] [int] NOT NULL,
	[OrdtSetteQta] [smallint] NOT NULL,
	[OrdtSetteColore] [int] NOT NULL,
	[OrdtOttoQta] [smallint] NOT NULL,
	[OrdtOttoColore] [int] NOT NULL,
	[OrdtNoveQta] [smallint] NOT NULL,
	[OrdtNoveColore] [int] NOT NULL,
	[OrdtZeroQta] [smallint] NOT NULL,
	[OrdtZeroColore] [int] NOT NULL,
	[OrdtCandelinaQta] [smallint] NOT NULL,
	[OrdtCandelinaColore] [int] NOT NULL,
	[OrdtNumeroTipo] [varchar](3) NOT NULL,
	[OrdtNumeroColore] [int] NOT NULL,
	[OrdtAcconto] [decimal](9, 2) NOT NULL,
	[OrdtStampato] [bit] NOT NULL,
	[OrdtStampaNum] [int] NOT NULL,
	[OrdtMailInviata] [bit] NOT NULL,
	[OrdtTipoAccessori] [int] NOT NULL,
	[OrdtDescAccessori] [varchar](30) NOT NULL,
	[OrdTStato] [tinyint] NOT NULL,
	[OrdtTipoTorta] [varchar](1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbStatoOrd]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbStatoOrd](
	[StCod] [tinyint] NOT NULL,
	[StDesc] [varchar](25) NOT NULL,
 CONSTRAINT [PK_TbStatoOrd] PRIMARY KEY CLUSTERED 
(
	[StCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VOrdTorte]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VOrdTorte]
as
SELECT 
    OrdtRif,
    ORDINE_NUMERO = OrdtNum,
    ORDINE_DATA = OrdtData ,
    DATA_RITIRO = OrdtDataRitiro,
    ORA_RITIRO = OrdOraRitiro,
	N_TORTE = OrdtQta,
    PESO = OrdtPesoKg,	
		DETTAGLIO_HTML =  CASE WHEN OrdtTipoTorta = 'P' THEN
						'<div style="font-family:Arial; font-size:8pt; color:black;">' +  
						
						        '<b>FORMA: </b> ' + CASE WHEN OrdtPesoKg > 1.8 THEN 'RETTANGOLARE' ELSE 'ROTONDA' END + '<br>' +
								-- Base
								CASE WHEN (B.OpzDesc <> '' OR OrdtPersBase <> '') 
									 THEN '<b>BASE:</b> ' + ISNULL(UPPER(B.OpzDesc), '') + CASE WHEN B.OpzDesc <> '' AND OrdtPersBase <> '' THEN ' - ' ELSE '' END + OrdtPersBase + '<br>' 
									 ELSE '' END +
								-- Bagna
								CASE WHEN BA.OpzDesc <> '' 
									 THEN '<b>BAGNA:</b> ' + UPPER(BA.OpzDesc) + '<br>' 
									 ELSE '' END +
								-- Opzione Eventi
								CASE WHEN (EV.OpzDesc <> '' OR OrdtPersOpEventi <> '') 
									 THEN '<b>OPZIONE EV.:</b> ' + ISNULL(UPPER(EV.OpzDesc), '') + CASE WHEN EV.OpzDesc <> '' AND OrdtPersOpEventi <> '' THEN ' - ' ELSE '' END + OrdtPersOpEventi + '<br>' 
									 ELSE '' END +
								-- Farcitura
								CASE WHEN (F.OpzDesc <> '' OR OrdtPersFarcitura <> '') 
									 THEN '<b>FARCITURA:</b> ' + ISNULL(UPPER(F.OpzDesc), '') + (CASE WHEN F2.OpzDesc <> '' then ', ' + UPPER(F2.OpzDesc)  ELSE '' END) + (CASE WHEN F3.OpzDesc <> '' then ', '+ UPPER(F3.OpzDesc)  ELSE '' END) + CASE WHEN F.OpzDesc <> '' AND OrdtPersFarcitura <> '' THEN ' - ' ELSE '' END + OrdtPersFarcitura + '<br>' 
									 ELSE '' END +
								-- Bordo
								CASE WHEN (BD.OpzDesc <> '' OR OrdtPersBordoDec <> '') 
									 THEN '<b>BORDO:</b> ' + ISNULL(UPPER(BD.OpzDesc), '') + CASE WHEN BD.OpzDesc <> '' AND OrdtPersBordoDec <> '' THEN ' - ' ELSE '' END + OrdtPersBordoDec + '<br>' 
									 ELSE '' END +
								-- Decorazione Superfic.
								CASE WHEN (DS.OpzDesc <> '' OR OrdtPersDecSuperficiale <> '') 
									 THEN '<b>DECOR. SUPERF.:</b> ' + ISNULL(UPPER(DS.OpzDesc), '') + CASE WHEN DS.OpzDesc <> '' AND OrdtPersDecSuperficiale <> '' THEN ' - ' ELSE '' END + OrdtPersDecSuperficiale + '<br>' 
									 ELSE '' END +
								 -- Ultimo Strato
								CASE WHEN (US.OpzDesc <> '' OR OrdtPersUltStrato <> '') 
									 THEN '<b>ULT. STRATO:</b> ' + ISNULL(UPPER(US.OpzDesc), '') + CASE WHEN US.OpzDesc <> '' AND OrdtPersUltStrato <> '' THEN ' - ' ELSE '' END + OrdtPersUltStrato + '<br>' 
									 ELSE '' END +
								CASE  WHEN ISNULL(OrdtNoFrutta, '') <> '' 
									 THEN '<b>FRUTTA DA ESCLUDERE:</b> ' + UPPER(OrdtNoFrutta) + '<br>'
								ELSE '' END +   
						'</div>'
		            ELSE
					     '<div style="font-family:Arial; font-size:8pt; color:black;">' +    
						  '<b>FORMA: </b> ' + CASE WHEN OrdtPesoKg > 1.8 THEN 'RETTANGOLARE' ELSE 'ROTONDA' END + '<br>' +
								-- Ingredienti Fissi
								CASE WHEN (T.OpzIngredienti <> '') 
									 THEN '<b>INGREDIENTI :</b> ' + ISNULL(UPPER(T.OpzIngredienti ), '') + '<br>' 
									 ELSE '' END +
								-- Bagna
								CASE WHEN BA.OpzDesc <> '' 
									 THEN '<b>BAGNA:</b> ' + UPPER(BA.OpzDesc) + '<br>' 
									 ELSE '' END +	
								CASE  WHEN ISNULL(OrdtNoFrutta, '') <> '' 
									 THEN '<b>FRUTTA DA ESCLUDERE:</b> ' + UPPER(OrdtNoFrutta) + '<br>'
								ELSE '' END +   
						'</div>'
					END
					  ,

    -- Decodifica delle scelte principali dal documento
    TIPO_TORTA = ISNULL(upper(T.OpzDesc),''),
    BASE = (CASE WHEN B.OpzDesc <> '' then 'BASE: ' ELSE '' END) + ISNULL(upper(B.OpzDesc), '')  + (CASE WHEN B.OpzDesc <> '' AND OrdtPersBase <> '' then ' - ' ELSE '' END) + OrdtPersBase ,     
    BAGNA = (CASE WHEN BA.OpzDesc <> '' then 'BAGNA: ' ELSE '' END) + ISNULL(UPPER(BA.OpzDesc), ''),  
	OP_EVENTI =(CASE WHEN EV.OpzDesc <> '' then 'OPZIONE EV.: ' ELSE '' END) + ISNULL(UPPER(EV.OpzDesc), '') + (CASE WHEN EV.OpzDesc <> '' AND OrdtPersOpEventi <> ''then ' - ' ELSE '' END) + OrdtPersOpEventi, 
    FARCITURA =(CASE WHEN F.OpzDesc <> '' then 'FARCITURA: ' ELSE '' END) + ISNULL(UPPER(F.OpzDesc),'') + (CASE WHEN F2.OpzDesc <> '' then ', '+ UPPER(F2.OpzDesc)  ELSE '' END) + (CASE WHEN F3.OpzDesc <> '' then ', '+ UPPER(F3.OpzDesc)  ELSE '' END) + (CASE WHEN F.OpzDesc <> '' AND OrdtPersFarcitura <> '' then ' - ' ELSE '' END) + OrdtPersFarcitura,
    BORDO_DEC =(CASE WHEN BD.OpzDesc <> '' then 'BORDO: ' ELSE '' END) +  ISNULL(UPPER(BD.OpzDesc),'') + (CASE WHEN BD.OpzDesc <> '' AND OrdtPersBordoDec <> '' then ' - ' ELSE '' END) + OrdtPersBordoDec ,
    DEC_SUP =(CASE WHEN DS.OpzDesc <> '' then 'DECOR. SUPERF.: ' ELSE '' END) + ISNULL(UPPER(DS.OpzDesc), '') + (CASE WHEN DS.OpzDesc <> '' AND OrdtPersDecSuperficiale <> '' then ' - ' ELSE '' END) + OrdtPersDecSuperficiale,
    ULT_STRATO=(CASE WHEN US.OpzDesc <> '' then 'ULT. STRATO : ' ELSE '' END) + ISNULL(UPPER(US.OpzDesc), '') + (CASE WHEN US.OpzDesc <> '' AND OrdtPersUltStrato <> '' then ' - ' ELSE '' END) + OrdtPersUltStrato,
   
   -- Altri dettagli
    FRASE = OrdtFrase , 
    ALLERGIE = OrdtAllergie, 
	NOFRUTTA = (CASE WHEN OrdtNoFrutta <> '' thEn 'DA ESCLUDERE : ' ELSE '' END) + OrdtNoFrutta,
	
    NUMERO_CERA = OrdtNumeroTipo, --+ ' (' + ISNULL(C.OpzDesc, '') + ')', 
    
    ACCONTO = OrdtAcconto,
	STATO = ISNULL(StDesc, ''),
	OrdTNAdulti, OrdtNBambini, OrdtPesoKg, OrdtTorta, OrdtBase, OrdtPersBase, OrdtBagna, OrdtOpEventi, OrdtPersOpEventi, OrdtFotoForma, OrdtFarcitura, OrdtPersFarcitura, OrdtBordoDec, OrdtPersBordoDec, 
                         OrdtFotoDecorazione, OrdtDecSuperficiale, OrdtPersDecSuperficiale,OrdtUltStrato,OrdtPersUltStrato, OrdtFrase, OrdtAllergie, OrdtImmagineNum, OrdtImmagine, OrdtNote, OrdtNoFrutta, OrdtCanaleFoto,
                         OrdtAcconto, OrdtStampato, OrdtStampaNum, OrdtPrivati,OrdtMailInviata,OrdtFarcitura2,OrdtFarcitura3,
						 OrdtTipoAccessori,OrdtDescAccessori, OrdTStato, OrdtTipoTorta,

	-------INGREDIENTI TORTE CLASSICHE
	INGREDIENTI = T.OpzIngredienti
    
FROM [dbo].[TbOrdTorte] O
LEFT JOIN [dbo].[TbOpzioniTorte] T  ON O.[OrdtTorta] = T.[OpzID]      
LEFT JOIN [dbo].[TbOpzioniTorte] B  ON O.[OrdtBase] = B.[OpzID]       
LEFT JOIN [dbo].[TbOpzioniTorte] EV ON O.[OrdtoPeventi] = EV.[OpzID]     
LEFT JOIN [dbo].[TbOpzioniTorte] BA ON O.[OrdtBagna] = BA.[OpzID]     
LEFT JOIN [dbo].[TbOpzioniTorte] F  ON O.[OrdtFarcitura] = F.[OpzID]
LEFT JOIN [dbo].[TbOpzioniTorte] F2  ON O.[OrdtFarcitura2] = F2.[OpzID]  
LEFT JOIN [dbo].[TbOpzioniTorte] F3  ON O.[OrdtFarcitura3] = F3.[OpzID]  
LEFT JOIN [dbo].[TbOpzioniTorte] BD ON O.[OrdtBordoDec] = BD.[OpzID]  
LEFT JOIN [dbo].[TbOpzioniTorte] DS ON O.[OrdtDecSuperficiale] = DS.[OpzID] 
LEFT JOIN [dbo].[TbOpzioniTorte] US ON O.[OrdtUltStrato] = US.[OpzID] 
LEFT JOIN [dbo].[TbStatoOrd] ST  ON O.[OrdTStato] = ST.[StCod]

GO
/****** Object:  View [dbo].[VTbTDis]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   VIEW [dbo].[VTbTDis]
as

SELECT *,
       CATEGORIA = (sELECT CatDesc FROM TbCat WHERE CatCod = TDisCat)
from TbTDis
GO
/****** Object:  View [dbo].[VFatBol]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VFatBol]
AS
SELECT      BolRif,BolNum,BolData,BolTipoDoc,BolCau,BolNumReg,BolCliFat,BolCliCons,BolRifFat,BolPagCod,BolNrag,
            AnaDesc 
FROM        TbBol INNER JOIN
            TbCli ON BolCliCons = ClCod INNER JOIN
            vdox.dbo.TbAna ON AnaCod = BolCliCons AND AnaGrp = 'CL'
WHERE     BolTipoDoc = 'B' AND BolNum > 0 AND BolNum <> 999999 AND Bolcau <> 20

GO
/****** Object:  Table [dbo].[TbTai]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTai](
	[TaiAnno] [smallint] NOT NULL,
	[TaiDitta] [varchar](50) NULL,
	[TaiDoc1] [int] NOT NULL,
	[TaiDoc2] [int] NOT NULL,
	[TaiDoc3] [int] NOT NULL,
	[TaiDoc4] [int] NOT NULL,
	[TaiDoc5] [int] NOT NULL,
	[TaiDoc6] [int] NOT NULL,
	[TaiDoc7] [int] NOT NULL,
	[TaiDoc8] [int] NOT NULL,
	[TaiDoc9] [int] NOT NULL,
	[TaiDoc10] [int] NOT NULL,
	[TaiDoc11] [int] NOT NULL,
	[TaiDoc12] [int] NOT NULL,
	[TaiDoc13] [int] NOT NULL,
	[TaiDoc14] [int] NOT NULL,
	[TaiDoc15] [int] NOT NULL,
	[TaiDoc16] [int] NOT NULL,
	[TaiDoc17] [int] NOT NULL,
	[TaiDoc18] [int] NOT NULL,
	[TaiDoc19] [int] NOT NULL,
	[TaiDoc20] [int] NOT NULL,
	[TaiCiv1] [smallint] NOT NULL,
	[TaiCiv2] [smallint] NOT NULL,
	[TaiCiv3] [smallint] NOT NULL,
	[TaiCiv4] [smallint] NOT NULL,
	[TaiCiv5] [smallint] NOT NULL,
	[TaiCiv6] [smallint] NOT NULL,
	[TaiCpt1] [varchar](5) NOT NULL,
	[TaiCpt2] [varchar](5) NOT NULL,
	[TaiCpt3] [varchar](5) NOT NULL,
	[TaiCpt4] [varchar](5) NOT NULL,
	[TaiCpt5] [varchar](5) NOT NULL,
	[TaiCpt6] [varchar](5) NOT NULL,
	[TaiReg1] [smallint] NOT NULL,
	[TaiReg2] [smallint] NOT NULL,
	[TaiReg3] [smallint] NOT NULL,
	[TaiReg4] [smallint] NOT NULL,
	[TaiReg5] [smallint] NOT NULL,
	[TaiReg6] [smallint] NOT NULL,
	[TaiReg7] [smallint] NOT NULL,
	[TaiReg8] [smallint] NOT NULL,
	[TaiReg9] [smallint] NOT NULL,
	[TaiReg10] [smallint] NOT NULL,
	[TaiReg11] [smallint] NOT NULL,
	[TaiReg12] [smallint] NOT NULL,
	[TaiReg13] [smallint] NOT NULL,
	[TaiReg14] [smallint] NOT NULL,
	[TaiReg15] [smallint] NOT NULL,
	[TaiReg16] [smallint] NOT NULL,
	[TaiReg17] [smallint] NOT NULL,
	[TaiReg18] [smallint] NOT NULL,
	[TaiReg19] [smallint] NOT NULL,
	[TaiReg20] [smallint] NOT NULL,
	[TaiPi1] [smallint] NOT NULL,
	[TaiPi2] [smallint] NOT NULL,
	[TaiPi3] [smallint] NOT NULL,
	[TaiPi4] [smallint] NOT NULL,
	[TaiPi5] [smallint] NOT NULL,
	[TaiPi6] [smallint] NOT NULL,
	[TaiPi7] [smallint] NOT NULL,
	[TaiPi8] [smallint] NOT NULL,
	[TaiPi9] [smallint] NOT NULL,
	[TaiPi10] [smallint] NOT NULL,
	[TaiPi11] [smallint] NOT NULL,
	[TaiPi12] [smallint] NOT NULL,
	[TaiAnnoLav] [smallint] NULL,
	[TaiIvaForn] [smallint] NOT NULL,
	[TaiCauInc] [smallint] NOT NULL,
	[TaiDes1] [varchar](30) NOT NULL,
	[TaiDes2] [varchar](30) NOT NULL,
	[TaiDes3] [varchar](30) NOT NULL,
	[TaiDes4] [varchar](30) NOT NULL,
	[TaiDes5] [varchar](30) NOT NULL,
	[TaiDes6] [varchar](30) NOT NULL,
	[TaiDes7] [varchar](30) NOT NULL,
	[TaiDes8] [varchar](30) NOT NULL,
	[TaiDes9] [varchar](30) NOT NULL,
	[TaiDes10] [varchar](30) NOT NULL,
	[TaiDes11] [varchar](30) NOT NULL,
	[TaiDes12] [varchar](30) NOT NULL,
	[TaiDes13] [varchar](30) NOT NULL,
	[TaiDes14] [varchar](30) NOT NULL,
	[TaiDes15] [varchar](30) NOT NULL,
	[TaiDes16] [varchar](30) NOT NULL,
	[TaiDes17] [varchar](30) NOT NULL,
	[TaiDes18] [varchar](30) NOT NULL,
	[TaiDes19] [varchar](30) NOT NULL,
	[TaiDes20] [varchar](30) NOT NULL,
	[TaiSpeseRb] [decimal](4, 2) NOT NULL,
	[TaipathXeur] [varchar](50) NULL,
	[TaiEffRiep] [bit] NULL,
	[TaiPathEffetti] [varchar](50) NULL,
	[TaiCauEff] [smallint] NOT NULL,
	[TaiPagOmaggi] [smallint] NOT NULL,
	[TaiPercSpese] [decimal](6, 3) NOT NULL,
	[TaiMinRiga] [decimal](7, 2) NOT NULL,
	[TaiBoll1] [varchar](45) NOT NULL,
	[TaiBoll2] [varchar](45) NOT NULL,
	[TaiBoll3] [varchar](45) NOT NULL,
	[TaiDataAggLis] [smalldatetime] NOT NULL,
	[TaiRegAcq] [smallint] NOT NULL,
	[TaiCliMag] [varchar](5) NOT NULL,
	[TaiUltInv] [smallint] NULL,
	[TaiCheckInv] [bit] NULL,
	[TaiInvRifer] [int] NULL,
	[TaiStatoInv] [smallint] NOT NULL,
	[TaiAggUtente] [varchar](50) NOT NULL,
	[TaiAggPassword] [varchar](50) NOT NULL,
	[TaiCptAcq] [varchar](5) NOT NULL,
	[TaiContanti] [smallint] NULL,
	[TaiIpPubblico] [varchar](16) NULL,
	[TaiMailFrom] [varchar](50) NOT NULL,
	[TaiMailCC] [varchar](50) NOT NULL,
	[TaiMailSmtp] [varchar](50) NOT NULL,
	[TaiMailDest] [varchar](50) NOT NULL,
	[TaiSmtpUser] [varchar](50) NOT NULL,
	[TaiPecFrom] [varchar](50) NOT NULL,
	[TaiPecSmtp] [varchar](50) NOT NULL,
	[TaiPecSmtpUser] [varchar](50) NOT NULL,
	[TaiPecSmtpPassword] [varchar](50) NOT NULL,
	[TaiPecSmtpPORTA] [int] NOT NULL,
	[TaiSmtpPassword] [varchar](50) NOT NULL,
	[TaiSmtpAuth] [bit] NOT NULL,
	[TaiMailDestOrdini] [varchar](50) NOT NULL,
	[TaiArtLibero] [varchar](2) NOT NULL,
	[TaiForMag] [varchar](5) NULL,
	[TaiSmtpPorta] [smallint] NOT NULL,
	[TaiSmtpSSL] [bit] NOT NULL,
	[TaiSpeseAmm] [decimal](9, 2) NOT NULL,
	[TaiNoteEstero] [varchar](300) NOT NULL,
	[TaiUltDataProvv] [smalldatetime] NULL,
	[TaiTipoFte_Differita] [varchar](4) NOT NULL,
	[TaiTipoFte_Libera] [varchar](4) NOT NULL,
	[TaiTipoFte_Ncr] [varchar](4) NOT NULL,
	[TaiCliFatt] [varchar](5) NOT NULL,
	[TaiUnder] [smallint] NOT NULL,
	[TaiHistory] [smallint] NOT NULL,
	[TaiBlockAnno] [smallint] NOT NULL,
	[TaiInvioMag] [bit] NOT NULL,
	[TaiCiv7] [smallint] NOT NULL,
	[TaiCpt7] [varchar](5) NOT NULL,
	[TaiBloccoMagazzino] [bit] NOT NULL,
	[TaiUltDataXprovv] [smalldatetime] NULL,
	[TaiCCOrdini] [varchar](50) NOT NULL,
	[TaiBodyOrdini] [ntext] NOT NULL,
	[TaiCivaOrd] [smallint] NOT NULL,
	[TaiMesiFile] [tinyint] NOT NULL,
	[TaiMgFM] [decimal](5, 2) NOT NULL,
	[TaiForn360] [varchar](5) NOT NULL,
	[TaiFornBivetro] [varchar](5) NOT NULL,
	[TaiFornAcrilico] [varchar](5) NOT NULL,
	[TaiTermInattivita] [smallint] NOT NULL,
	[TaiMaggFori] [decimal](7, 2) NOT NULL,
	[TaiMaggTacche] [decimal](7, 2) NOT NULL,
	[TaiEnergiaBivetro] [decimal](7, 2) NOT NULL,
	[TaiEnergia360] [decimal](7, 2) NOT NULL,
	[TaiUMEnergiaBivetro] [varchar](2) NOT NULL,
	[TaiUMEnergia360] [varchar](2) NOT NULL,
	[TaiMaxData] [smalldatetime] NULL,
	[TaiPagQua] [smallint] NOT NULL,
	[TaiLimCorr] [int] NOT NULL,
	[TaiPathBilance] [varchar](100) NOT NULL,
	[TaiCliCorr] [varchar](5) NOT NULL,
	[TaiRecBollo] [bit] NOT NULL,
	[TaiLimBollo] [decimal](7, 2) NOT NULL,
	[TaiImpBollo] [decimal](7, 2) NOT NULL,
	[TaiCodBollo] [varchar](20) NOT NULL,
	[TaiSplit10] [smallint] NOT NULL,
	[TaiSplit20] [smallint] NOT NULL,
	[TaiPrBilVar] [varchar](20) NOT NULL,
	[TaiCliTorte] [varchar](5) NOT NULL,
	[TaiCodTorte] [varchar](10) NOT NULL,
	[TaiNote] [varchar](200) NOT NULL,
	[TaiKeibCassa] [bit] NOT NULL,
	[TaiSogliaListinoDitte] [decimal](11, 2) NOT NULL,
	[TaiListinoDitte] [smallint] NOT NULL,
	[TaiPorzioneAdulti] [int] NOT NULL,
	[TaiPorzioneBimbi] [int] NOT NULL,
	[TaiPesoMinimoTorta] [int] NOT NULL,
	[TaiPorzioneAdulti2] [int] NOT NULL,
	[TaiPorzioneBimbi2] [int] NOT NULL,
	[TaiPesoMinimoTorta2] [int] NOT NULL,
 CONSTRAINT [PK_TbTai] PRIMARY KEY CLUSTERED 
(
	[TaiAnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[VFatture]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[VFatture]
AS
SELECT    FatTipoDoc,FatRif,FatNum,FatData,FatNumReg,FatNdep, 
          VDOX.dbo.TbAna.AnaCitta AS Citta, 
		  VDOX.dbo.TbAna.AnaDesc AS RagSoc, 
		  VDOX.dbo.TbAna.AnaCod AS Codice,                       
		  VDOX.dbo.TbAna.AnaGrp AS Grp, dbo.TbFat.FatNumReg AS Expr1
FROM      VDOX.dbo.TbAna INNER JOIN
          TbFat ON VDOX.dbo.TbAna.AnaCod = dbo.TbFat.FatCliCons INNER JOIN
          TbTai ON dbo.TbFat.FatNumReg = dbo.TbTai.TaiReg10 and taiAnno = datepart(year,FatData)
WHERE     (VDOX.dbo.TbAna.AnaGrp = 'CL')
-----ORDER BY dbo.TbFat.FatCliCons


	




GO
/****** Object:  View [dbo].[OLD_VOrdTorte]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[OLD_VOrdTorte]
as
SELECT 
    OrdtRif,
    ORDINE_NUMERO = OrdtNum,
    ORDINE_DATA = OrdtData ,
    DATA_RITIRO = OrdtDataRitiro,
    ORA_RITIRO = OrdOraRitiro,
	N_TORTE = OrdtQta,
    PESO = OrdtPesoKg,

	--DETTAGLIO_HTML = 
	--    '<div style="font-family:Arial; font-size:8pt; color:black;">' +
 --       CASE 
 --           -- Se esiste frutta da escludere (caso Crostata/Torta di frutta), mostra solo questo
 --           WHEN ISNULL(OrdtNoFrutta, '') <> '' 
 --           THEN '<b>DA ESCLUDERE:</b> ' + UPPER(OrdtNoFrutta) + '<br>'
            
 --           -- Altrimenti, concatena i componenti della torta personalizzata
 --           ELSE 
 --               -- Base
 --               CASE WHEN (B.OpzDesc <> '' OR OrdtPersBase <> '') 
 --                    THEN '<b>BASE:</b> ' + ISNULL(UPPER(B.OpzDesc), '') + CASE WHEN B.OpzDesc <> '' AND OrdtPersBase <> '' THEN ' - ' ELSE '' END + OrdtPersBase + '<br>' 
 --                    ELSE '' END +
 --               -- Bagna
 --               CASE WHEN BA.OpzDesc <> '' 
 --                    THEN '<b>BAGNA:</b> ' + UPPER(BA.OpzDesc) + '<br>' 
 --                    ELSE '' END +
 --               -- Opzione Eventi
 --               CASE WHEN (EV.OpzDesc <> '' OR OrdtPersOpEventi <> '') 
 --                    THEN '<b>OPZIONE EV.:</b> ' + ISNULL(UPPER(EV.OpzDesc), '') + CASE WHEN EV.OpzDesc <> '' AND OrdtPersOpEventi <> '' THEN ' - ' ELSE '' END + OrdtPersOpEventi + '<br>' 
 --                    ELSE '' END +
 --               -- Farcitura
 --               CASE WHEN (F.OpzDesc <> '' OR OrdtPersFarcitura <> '') 
 --                    THEN '<b>FARCITURA:</b> ' + ISNULL(UPPER(F.OpzDesc), '') + (CASE WHEN F2.OpzDesc <> '' then ', ' + UPPER(F2.OpzDesc)  ELSE '' END) + (CASE WHEN F3.OpzDesc <> '' then ', '+ UPPER(F3.OpzDesc)  ELSE '' END) + CASE WHEN F.OpzDesc <> '' AND OrdtPersFarcitura <> '' THEN ' - ' ELSE '' END + OrdtPersFarcitura + '<br>' 
 --                    ELSE '' END +
 --               -- Bordo
 --               CASE WHEN (BD.OpzDesc <> '' OR OrdtPersBordoDec <> '') 
 --                    THEN '<b>BORDO:</b> ' + ISNULL(UPPER(BD.OpzDesc), '') + CASE WHEN BD.OpzDesc <> '' AND OrdtPersBordoDec <> '' THEN ' - ' ELSE '' END + OrdtPersBordoDec + '<br>' 
 --                    ELSE '' END +
 --               -- Decorazione Superfic.
 --               CASE WHEN (DS.OpzDesc <> '' OR OrdtPersDecSuperficiale <> '') 
 --                    THEN '<b>DECOR. SUPERF.:</b> ' + ISNULL(UPPER(DS.OpzDesc), '') + CASE WHEN DS.OpzDesc <> '' AND OrdtPersDecSuperficiale <> '' THEN ' - ' ELSE '' END + OrdtPersDecSuperficiale + '<br>' 
 --                    ELSE '' END
 --       END +
	--	'</div>',
		DETTAGLIO_HTML = 
	    '<div style="font-family:Arial; font-size:8pt; color:black;">' +
                 
                -- Base
                CASE WHEN (B.OpzDesc <> '' OR OrdtPersBase <> '') 
                     THEN '<b>BASE:</b> ' + ISNULL(UPPER(B.OpzDesc), '') + CASE WHEN B.OpzDesc <> '' AND OrdtPersBase <> '' THEN ' - ' ELSE '' END + OrdtPersBase + '<br>' 
                     ELSE '' END +
                -- Bagna
                CASE WHEN BA.OpzDesc <> '' 
                     THEN '<b>BAGNA:</b> ' + UPPER(BA.OpzDesc) + '<br>' 
                     ELSE '' END +
                -- Opzione Eventi
                CASE WHEN (EV.OpzDesc <> '' OR OrdtPersOpEventi <> '') 
                     THEN '<b>OPZIONE EV.:</b> ' + ISNULL(UPPER(EV.OpzDesc), '') + CASE WHEN EV.OpzDesc <> '' AND OrdtPersOpEventi <> '' THEN ' - ' ELSE '' END + OrdtPersOpEventi + '<br>' 
                     ELSE '' END +
                -- Farcitura
                CASE WHEN (F.OpzDesc <> '' OR OrdtPersFarcitura <> '') 
                     THEN '<b>FARCITURA:</b> ' + ISNULL(UPPER(F.OpzDesc), '') + (CASE WHEN F2.OpzDesc <> '' then ', ' + UPPER(F2.OpzDesc)  ELSE '' END) + (CASE WHEN F3.OpzDesc <> '' then ', '+ UPPER(F3.OpzDesc)  ELSE '' END) + CASE WHEN F.OpzDesc <> '' AND OrdtPersFarcitura <> '' THEN ' - ' ELSE '' END + OrdtPersFarcitura + '<br>' 
                     ELSE '' END +
                -- Bordo
                CASE WHEN (BD.OpzDesc <> '' OR OrdtPersBordoDec <> '') 
                     THEN '<b>BORDO:</b> ' + ISNULL(UPPER(BD.OpzDesc), '') + CASE WHEN BD.OpzDesc <> '' AND OrdtPersBordoDec <> '' THEN ' - ' ELSE '' END + OrdtPersBordoDec + '<br>' 
                     ELSE '' END +
                -- Decorazione Superfic.
                CASE WHEN (DS.OpzDesc <> '' OR OrdtPersDecSuperficiale <> '') 
                     THEN '<b>DECOR. SUPERF.:</b> ' + ISNULL(UPPER(DS.OpzDesc), '') + CASE WHEN DS.OpzDesc <> '' AND OrdtPersDecSuperficiale <> '' THEN ' - ' ELSE '' END + OrdtPersDecSuperficiale + '<br>' 
                     ELSE '' END +
				 -- Ultimo Strato
                CASE WHEN (US.OpzDesc <> '' OR OrdtPersUltStrato <> '') 
                     THEN '<b>ULT. STRATO:</b> ' + ISNULL(UPPER(US.OpzDesc), '') + CASE WHEN US.OpzDesc <> '' AND OrdtPersUltStrato <> '' THEN ' - ' ELSE '' END + OrdtPersUltStrato + '<br>' 
                     ELSE '' END +
				CASE  WHEN ISNULL(OrdtNoFrutta, '') <> '' 
				     THEN '<b>FRUTTA DA ESCLUDERE:</b> ' + UPPER(OrdtNoFrutta) + '<br>'
				ELSE '' END +

   
		'</div>',
	CANDELINE_HTML = '<div style="font-family:Arial; font-size:8pt;">' + 
    ISNULL(
        CASE WHEN OrdtUnoQta > 0 THEN 'N. ' + CAST(OrdtUnoQta AS VARCHAR) + ' cand. num. 1 (' + ISNULL(C1.OpzDesc,'') + ')<br>' ELSE '' END +
        CASE WHEN OrdtDueQta > 0 THEN 'N. ' + CAST(OrdtDueQta AS VARCHAR) + ' cand. num. 2 (' + ISNULL(C2.OpzDesc,'') + ')<br>' ELSE '' END +
		CASE WHEN OrdtTreQta > 0 THEN 'N. ' + CAST(OrdtTreQta AS VARCHAR) + ' cand. num. 3 (' + ISNULL(C3.OpzDesc,'') + ')<br>' ELSE '' END +
		CASE WHEN OrdtQuattroQta > 0 THEN 'N. ' + CAST(OrdtQuattroQta AS VARCHAR) + ' cand. num. 4 (' + ISNULL(C4.OpzDesc,'') + ')<br>' ELSE '' END +
		CASE WHEN OrdtCinqueQta > 0 THEN 'N. ' + CAST(OrdtCinqueQta AS VARCHAR) + ' cand. num. 5 (' + ISNULL(C5.OpzDesc,'') + ')<br>' ELSE '' END +
		CASE WHEN OrdtSeiQta > 0 THEN 'N. ' + CAST(OrdtSeiQta AS VARCHAR) + ' cand. num. 6 (' + ISNULL(C6.OpzDesc,'') + ')<br>' ELSE '' END +
		CASE WHEN OrdtSetteQta > 0 THEN 'N. ' + CAST(OrdtSetteQta AS VARCHAR) + ' cand. num. 7 (' + ISNULL(C7.OpzDesc,'') + ')<br>' ELSE '' END +
	    CASE WHEN OrdtOttoQta > 0 THEN 'N. ' + CAST(OrdtOttoQta AS VARCHAR) + ' cand. num. 8 (' + ISNULL(C8.OpzDesc,'') + ')<br>' ELSE '' END +
	    CASE WHEN OrdtNoveQta > 0 THEN 'N. ' + CAST(OrdtNoveQta AS VARCHAR) + ' cand. num. 9 (' + ISNULL(C9.OpzDesc,'') + ')<br>' ELSE '' END +
		CASE WHEN OrdtZeroQta > 0 THEN 'N. ' + CAST(OrdtZeroQta AS VARCHAR) + ' cand. num. 0 (' + ISNULL(C0.OpzDesc,'') + ')<br>' ELSE '' END +
        
        CASE WHEN OrdtCandelinaQta > 0 THEN 'N. ' + CAST(OrdtCandelinaQta AS VARCHAR) + ' cand. stelo (' + ISNULL(CA.OpzDesc,'') + ')<br>' ELSE '' END +
        CASE WHEN OrdtNumeroTipo > 0 THEN 'N. 1 Numero Cera ' + CAST(OrdtNumeroTipo AS VARCHAR) + ' (' + ISNULL(C.OpzDesc,'') + ')<br>' ELSE '' END
    , 'Nessuna candelina ordinata') + 
	'</div>',

	ACCESSORI_HTML = '<div style="font-family:Arial; font-size:8pt;">' + 
	    ISNULL(
	        CASE WHEN OrdtDescAccessori <> '' THEN  ISNULL(A.OpzDesc,'') +' (' + OrdtDescAccessori +')<br>' ELSE '' END 
	    , 'Nessuna candelina ordinata') + 
	'</div>',

    -- Decodifica delle scelte principali dal documento
    TIPO_TORTA = ISNULL(upper(T.OpzDesc),''),
    BASE = (CASE WHEN B.OpzDesc <> '' then 'BASE: ' ELSE '' END) + ISNULL(upper(B.OpzDesc), '')  + (CASE WHEN B.OpzDesc <> '' AND OrdtPersBase <> '' then ' - ' ELSE '' END) + OrdtPersBase ,     
    BAGNA = (CASE WHEN BA.OpzDesc <> '' then 'BAGNA: ' ELSE '' END) + ISNULL(UPPER(BA.OpzDesc), ''),  
	OP_EVENTI =(CASE WHEN EV.OpzDesc <> '' then 'OPZIONE EV.: ' ELSE '' END) + ISNULL(UPPER(EV.OpzDesc), '') + (CASE WHEN EV.OpzDesc <> '' AND OrdtPersOpEventi <> ''then ' - ' ELSE '' END) + OrdtPersOpEventi, 
    FARCITURA =(CASE WHEN F.OpzDesc <> '' then 'FARCITURA: ' ELSE '' END) + ISNULL(UPPER(F.OpzDesc),'') + (CASE WHEN F2.OpzDesc <> '' then ', '+ UPPER(F2.OpzDesc)  ELSE '' END) + (CASE WHEN F3.OpzDesc <> '' then ', '+ UPPER(F3.OpzDesc)  ELSE '' END) + (CASE WHEN F.OpzDesc <> '' AND OrdtPersFarcitura <> '' then ' - ' ELSE '' END) + OrdtPersFarcitura,
    BORDO_DEC =(CASE WHEN BD.OpzDesc <> '' then 'BORDO: ' ELSE '' END) +  ISNULL(UPPER(BD.OpzDesc),'') + (CASE WHEN BD.OpzDesc <> '' AND OrdtPersBordoDec <> '' then ' - ' ELSE '' END) + OrdtPersBordoDec ,
    DEC_SUP =(CASE WHEN DS.OpzDesc <> '' then 'DECOR. SUPERF.: ' ELSE '' END) + ISNULL(UPPER(DS.OpzDesc), '') + (CASE WHEN DS.OpzDesc <> '' AND OrdtPersDecSuperficiale <> '' then ' - ' ELSE '' END) + OrdtPersDecSuperficiale,
    ULT_STRATO=(CASE WHEN US.OpzDesc <> '' then 'ULT. STRATO : ' ELSE '' END) + ISNULL(UPPER(US.OpzDesc), '') + (CASE WHEN US.OpzDesc <> '' AND OrdtPersUltStrato <> '' then ' - ' ELSE '' END) + OrdtPersUltStrato,
   
   -- Altri dettagli
    FRASE = OrdtFrase , 
    ALLERGIE = OrdtAllergie, 
	NOFRUTTA = (CASE WHEN OrdtNoFrutta <> '' thEn 'DA ESCLUDERE : ' ELSE '' END) + OrdtNoFrutta,
	
    NUMERO_CERA = OrdtNumeroTipo, --+ ' (' + ISNULL(C.OpzDesc, '') + ')', 
    
    ACCONTO = OrdtAcconto,
	STATO = ISNULL(StDesc, ''),
	OrdTNAdulti, OrdtNBambini, OrdtPesoKg, OrdtTorta, OrdtBase, OrdtPersBase, OrdtBagna, OrdtOpEventi, OrdtPersOpEventi, OrdtFotoForma, OrdtFarcitura, OrdtPersFarcitura, OrdtBordoDec, OrdtPersBordoDec, 
                         OrdtFotoDecorazione, OrdtDecSuperficiale, OrdtPersDecSuperficiale,OrdtUltStrato,OrdtPersUltStrato, OrdtFrase, OrdtAllergie, OrdtImmagineNum, OrdtImmagine, OrdtNote, OrdtNoFrutta, OrdtCanaleFoto, OrdtUnoQta, OrdtUnoColore, OrdtDueQta, OrdtDueColore, 
                         OrdtTreQta, OrdtTreColore, OrdtQuattroQta, OrdtQuattroColore, OrdtCinqueQta, OrdtCinqueColore, OrdtSeiQta, OrdtSeiColore, OrdtSetteQta, OrdtSetteColore, OrdtOttoQta, OrdtOttoColore, OrdtNoveQta, OrdtNoveColore, 
                         OrdtZeroQta, OrdtZeroColore, OrdtCandelinaQta, OrdtCandelinaColore, OrdtNumeroTipo, OrdtNumeroColore, OrdtAcconto, OrdtStampato, OrdtStampaNum, OrdtPrivati,OrdtMailInviata,OrdtFarcitura2,OrdtFarcitura3,
						 OrdtTipoAccessori,OrdtDescAccessori, OrdTStato

    
FROM [dbo].[TbOrdTorte] O
LEFT JOIN [dbo].[TbOpzioniTorte] T  ON O.[OrdtTorta] = T.[OpzID]      
LEFT JOIN [dbo].[TbOpzioniTorte] B  ON O.[OrdtBase] = B.[OpzID]       
LEFT JOIN [dbo].[TbOpzioniTorte] EV ON O.[OrdtoPeventi] = EV.[OpzID]     
LEFT JOIN [dbo].[TbOpzioniTorte] BA ON O.[OrdtBagna] = BA.[OpzID]     
LEFT JOIN [dbo].[TbOpzioniTorte] F  ON O.[OrdtFarcitura] = F.[OpzID]
LEFT JOIN [dbo].[TbOpzioniTorte] F2  ON O.[OrdtFarcitura2] = F2.[OpzID]  
LEFT JOIN [dbo].[TbOpzioniTorte] F3  ON O.[OrdtFarcitura3] = F3.[OpzID]  
LEFT JOIN [dbo].[TbOpzioniTorte] BD ON O.[OrdtBordoDec] = BD.[OpzID]  
LEFT JOIN [dbo].[TbOpzioniTorte] DS ON O.[OrdtDecSuperficiale] = DS.[OpzID] 
LEFT JOIN [dbo].[TbOpzioniTorte] US ON O.[OrdtUltStrato] = US.[OpzID] 
LEFT JOIN [dbo].[TbOpzioniTorte] C  ON O.[OrdtNumeroColore] = C.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] CA  ON O.[OrdtCandelinaColore] = CA.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C1  ON O.[OrdtUnoColore] = C1.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C2  ON O.[OrdtDueColore] = C2.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C3  ON O.[OrdtTreColore] = C3.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C4  ON O.[OrdtQuattroColore] = C4.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C5  ON O.[OrdtCinqueColore] = C5.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C6  ON O.[OrdtSeiColore] = C6.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C7  ON O.[OrdtSetteColore] = C7.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C8  ON O.[OrdtOttoColore] = C8.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C9  ON O.[OrdtNoveColore] = C9.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] C0  ON O.[OrdtZeroColore] = C0.[OpzID]    
LEFT JOIN [dbo].[TbOpzioniTorte] A  ON O.[OrdtTipoAccessori] = A.[OpzID]
LEFT JOIN [dbo].[TbStatoOrd] ST  ON O.[OrdTStato] = ST.[StCod]









GO
/****** Object:  View [dbo].[VIEW5]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[VIEW5]
AS
SELECT TOP 100 PERCENT IDBLOCCO,FatNum,FatData,FatNumReg,FatCliCons,AnaDesc,FatCliFat,FatRif,FatPagCod,FatDiff,IDNUMREG,FatNDep
FROM TBBLOCK INNER JOIN
     TbFat ON IDRIFERIM = FatRif INNER JOIN
     VDOX.dbo.TbAna ON FatCliCons = AnaCod
WHERE  AnaGrp = 'CL'
ORDER BY dbo.TbFat.FatData, dbo.TbFat.FatNum, dbo.TbFat.FatNumReg








GO
/****** Object:  View [dbo].[XVPRINTFATTFM]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE   VIEW [dbo].[XVPRINTFATTFM]
AS

SELECT FATRIF,FATNUM,FATNUMREG,FATDATA,FATPAGCOD,PAGDESC,FATCLICONS,FATCLIFAT,FATABI,FATCAB,
CADESCTFT = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7  THEN ISNULL(CADESCFT,'')
                 WHEN PAGTIPO = 5  THEN ISNULL(BANDES,'')
                 ELSE '' END,
--PAGAMENTO = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(FATABI AS VARCHAR(5))+ ' CAB '+CAST(FATCAB AS VARCHAR(5)) 
--             WHEN PAGTIPO = 5 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(BANABI AS VARCHAR(5))+ ' CAB '+CAST(BANCAB AS VARCHAR(5))              
--             else CAST(fatpagcod as varchar(3))+' '+PAGDESC end,
PAGAMENTO = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(FATABI AS VARCHAR(5))+ ' CAB '+CAST(FATCAB AS VARCHAR(5)) 
             WHEN PAGTIPO = 5 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' IBAN: '+isnull((select CaPaese+' '+BanCinEur+' '+BanCin+' '+FORMAT(CaAbi,'00000')+ ' ' +FORMAT(CaCab,'00000')+' ' + bancc from  COGE.dbo.TbCab
              where CaAbi = BanAbi and CaCab = BanCab),'IT 26 T 03069 09212 100000103876')            
             else CAST(fatpagcod as varchar(3))+' '+PAGDESC end,    
D_ANARAG1 = D.ANARAG1,
D_ANARAG2 = D.ANARAG2,
D_ANAINDIRIZZO = D.ANAINDIRIZZO,
D_ANACAP = D.ANACAP,
D_ANACITTA = D.ANACITTA,
D_ANAPROV = D.ANAPROV,
D_CITTA = D.ANACAP + ' - ' +  D.ANACITTA + ' - ' +  D.ANAPROV,
ANARAG1 = A.ANARAG1,
ANARAG2 = A.ANARAG2,
ANAINDIRIZZO =A.ANAINDIRIZZO,
ANACAP = A.ANACAP,
ANACITTA = A.ANACITTA,
ANAPROV = A.ANAPROV,
ANACFIS = A.ANACFIS,
ANAPIVA = A.ANAPIVA,
ANAPIVAEST = A.ANAPIVAEST,
CITTA = A.ANACAP + ' - ' +  A.ANACITTA + ' - ' +  A.ANAPROV,                      
FATPIVA = CASE  WHEN RTRIM(A.ANAPIVAEST) <> '' THEN A.ANAPIVAEST ELSE A.ANAPIVA END,
VETANA1 = V1.ANADESC,
VETANA2 = V2.ANADESC,
VETDOM1 = LTRIM(V1.ANAINDIRIZZO) + ' ' + LTRIM(V1.ANACAP) + ' ' + LTRIM(V1.ANACITTA)  + ' ' + LTRIM(V1.ANAPROV),
VETDOM2 = LTRIM(V2.ANAINDIRIZZO) + ' ' + LTRIM(V2.ANACAP) + ' ' + LTRIM(V2.ANACITTA)  + ' ' + LTRIM(V2.ANAPROV),
FATTRASF,PAGTIPO,FATDMV,MGCAUDESC,FATNCOLLI,FATPESONETTO,FATPESOLORDO,FATPORTO,FATASPBENI,
FATDATATRASP,FATDATARITIRO1,FATDATARITIRO2,
TIPODOC = CASE WHEN FATTIPODOC = 'Y' THEN 'PROFORMA' ELSE (CASE WHEN FATNUMREG = (SELECT TOP 1 TAIREG11 FROM TbTai order by taianno desc) THEN 'NOTA CREDITO' ELSE 'F A T T U R A' end) END,
FATTOTMERCE = DCGTOTMERCE - ISNULL((select CorImporto from TbCor where cortipodoc = 'F' AND CORRIF = fatrif and corcodart = 'BOLLO'),0),
FATTOTSCONTO = DCGRIETOTSCONTO,
FATTOTIMPO = DCGTOTMERCE - DCGRIETOTSCONTO  - ISNULL((select CorImporto from TbCor where cortipodoc = 'F' AND CORRIF = fatrif and corcodart = 'BOLLO'),0),
FATANNOTA,
FATSL = ISNULL((select RivaSL from coge.dbo.TbRegiva where RivaAnno = datepart(Year,FatData) and RivaNReg = FatNumreg),''),
CORCODART = CASE WHEN RTRIM(A.ANAPIVAEST) <> '' THEN CORCODART + ' - ' + ISNULL(ADGDogana,'') ELSE CORCODART END,
CORDESC,
CORUM,
CORQUACON,
CORPREZZO,
CORSC1,
CORIMPORTO = cast(CORQUACON * CORPREZZO * (1 - CORSC1 / 100) as decimal(11,2)),
--CORCIVA = case when FatNimp = 0 then DBO.tci(CORCIVA) ELSE FatNimp END,
CORCIVA = case when FatNimp = 0 then DBO.tci(CORCIVA) ELSE ( CASE  WHEN CORCODART <> 'BOLLO' THEN FatNimp ELSE DBO.tci(CORCIVA) END) END,
DESCOMAGGI = case when COROMAGGI = 'S' THEN 'OM.SOGG.' WHEN COROMAGGI = 'E' THEN 'OMAGGIO' ELSE '' END,
CLESPSCONTI,
DCGNUMRIF,DCGTIPO,DCGDATA,DCGRIECI1,DCGRIEIMP1,DCGRIEIVA1,DCGRIECI2,DCGRIEIMP2,DCGRIEIVA2,DCGRIECI3,DCGRIEIMP3,DCGRIEIVA3,DCGRIECI4,DCGRIEIMP4,DCGRIEIVA4,
IMPOSTA1 = CASE WHEN DCGRIECI1 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI1) ELSE '' END,
ALI1 = CASE WHEN DCGRIECI1 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI1) ELSE ''END,
IMPOSTA2 = CASE WHEN DCGRIECI2 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI2) ELSE '' END,
ALI2 = CASE WHEN DCGRIECI2 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI2) ELSE '' END,
IMPOSTA3 = CASE WHEN DCGRIECI3 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI3) ELSE '' END,
ALI3 = CASE WHEN DCGRIECI3 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI3) ELSE '' END,
IMPOSTA4 = CASE WHEN DCGRIECI4 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI4) ELSE '' END,
ALI4 = CASE WHEN DCGRIECI4 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI4) ELSE'' END,
DCGTOTMERCE,DCGRIETOTIMP,DCGRIETOTESE,
--DCGSCULTER,DCGRIETOTSCONTO,
DCGRIETOTIVA,DCGRIETOTFAT,DCGRIENETTO,DCGSPEBOLLI,DCGSPERB,DCGRIETOTACC,
ALIRB = CASE WHEN DCGSPERBCI > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DBO.TCI(DCGSPERBCI)) ELSE '' END,
ALIIMB = CASE WHEN DCGSPEIMBCI > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGSPEIMBCI) ELSE '' END,
CORPROG,BOLRIF,BOLNUM,BOLDATA,CORARTID
FROM TBFAT
INNER JOIN TbBol on Bolriffat = FATRIF
INNER JOIN  TbCor ON (CORRIF = BOLRIF AND CORTIPODOC = BOLTIPODOC ) or (CORRIF = FATRIF AND CORCODART = 'BOLLO' AND BOLRIF = (SELECT MAX(BOLRIF) FROM tBbOL WHERE BOLRIFFAT = FATRIF))
INNER JOIN TBDCG ON DCGNUMRIF = FATRIF
INNER JOIN TbCli on CLCOD=FATCLIFAT
LEFT JOIN coge.dbo.TBCAB ON FATABI = CAABI AND FATCAB = CACAB 
LEFT JOIN coge.dbo.TBPAG ON PAGCOD = FATPAGCOD
LEFT OUTER JOIN VVETTORI v1 ON V1.VETCOD=FATVETT1
LEFT OUTER JOIN VVETTORI v2 ON V2.VETCOD=FATVETT2             
LEFT JOIN VDOX.DBO.TBANA AS A ON A.ANACOD = FATCLIFAT AND A.ANAGRP = 'CL'
LEFT JOIN VDOX.DBO.TBANA AS D ON D.ANACOD = CASE WHEN FatPvv='' THEN FATCLICONS ELSE FATPVV END AND D.ANAGRP = 'CL'
LEFT JOIN TBTCAU ON MGCAUID=FATCAU
LEFT OUTER JOIN COGE.dbo.TbBan ON ClcodBan = COGE.dbo.TbBan.BanCod  
left join TbArtDg on ADGArtCod = CorCodArt
WHERE FATTIPODOC = 'F' AND FATDIFF = 'D'
GO
/****** Object:  View [dbo].[XVPRINTFAT]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE   VIEW [dbo].[XVPRINTFAT]
AS

SELECT FATRIF,FATNUM,FATNUMREG,FATDATA,FATPAGCOD,PAGDESC,FATCLICONS,FATCLIFAT,FATABI,FATCAB,
CADESCTFT = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7  THEN ISNULL(CADESCFT,'')
                 WHEN PAGTIPO = 5  THEN ISNULL(BANDES,'')
                 ELSE '' END,
--PAGAMENTO = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(FATABI AS VARCHAR(5))+ ' CAB '+CAST(FATCAB AS VARCHAR(5)) 
--             WHEN PAGTIPO = 5 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(BANABI AS VARCHAR(5))+ ' CAB '+CAST(BANCAB AS VARCHAR(5))              
--             else CAST(fatpagcod as varchar(3))+' '+PAGDESC end,
PAGAMENTO = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(FATABI AS VARCHAR(5))+ ' CAB '+CAST(FATCAB AS VARCHAR(5)) 
             WHEN PAGTIPO = 5 then CAST(FATPAGCOD AS VARCHAR(3))+' '+PAGDESC+' IBAN: '+isnull((select CaPaese+' '+BanCinEur+' '+BanCin+' '+FORMAT(CaAbi,'00000')+ ' ' +FORMAT(CaCab,'00000')+' ' + bancc from  COGE.dbo.TbCab
              where CaAbi = BanAbi and CaCab = BanCab),'IT 26 T 03069 09212 100000103876')            
             else CAST(fatpagcod as varchar(3))+' '+PAGDESC end,
IBAN =isnull((select CaPaese+' '+BanCinEur+' '+BanCin+' '+'0'+cast(CaAbi as varchar)+' '+cast(CaCab as varchar)+' ' + bancc from  COGE.dbo.TbCab
              where CaAbi = BanAbi and CaCab = BanCab),'IT 26 T 03069 09212 100000103876')  + ' ' + isnull(BanDes,'INTESA SAN PAOLO TO 19'),
D_ANARAG1 = D.ANARAG1,
D_ANARAG2 = D.ANARAG2, 
D_ANAINDIRIZZO = D.ANAINDIRIZZO,
D_ANACAP = D.ANACAP,
D_ANACITTA = D.ANACITTA,
D_ANAPROV = D.ANAPROV,
D_CITTA = D.ANACAP + ' - ' +  D.ANACITTA + ' - ' +  D.ANAPROV,
ANARAG1 = A.ANARAG1,
ANARAG2 = A.ANARAG2,
ANAINDIRIZZO =A.ANAINDIRIZZO,
ANACAP = A.ANACAP,
ANACITTA = A.ANACITTA,
ANAPROV = A.ANAPROV,
ANACFIS = A.ANACFIS,
ANAPIVA = A.ANAPIVA,
ANAPIVAEST = A.ANAPIVAEST,
CITTA = A.ANACAP + ' - ' +  A.ANACITTA + ' - ' +  A.ANAPROV,                      
FATPIVA = CASE  WHEN RTRIM(A.ANAPIVAEST) <> '' THEN A.ANAPIVAEST ELSE A.ANAPIVA END,
VETANA1 = V1.ANADESC,
VETANA2 = V2.ANADESC,
VETDOM1 = LTRIM(V1.ANAINDIRIZZO) + ' ' + LTRIM(V1.ANACAP) + ' ' + LTRIM(V1.ANACITTA)  + ' ' + LTRIM(V1.ANAPROV),
VETDOM2 = LTRIM(V2.ANAINDIRIZZO) + ' ' + LTRIM(V2.ANACAP) + ' ' + LTRIM(V2.ANACITTA)  + ' ' + LTRIM(V2.ANAPROV),
FATTRASF,PAGTIPO,FATDMV,MGCAUDESC,FATNCOLLI,FATPESONETTO,FATPESOLORDO,FATPORTO,FATASPBENI,
FATDATATRASP,FATDATARITIRO1,FATDATARITIRO2,
TIPODOC = CASE WHEN FATTIPODOC = 'Y' THEN 'PROFORMA' ELSE (CASE WHEN FATNUMREG = (SELECT TOP 1 TAIREG11 FROM TbTai order by taianno desc) THEN 'NOTA CREDITO' ELSE 'F A T T U R A' end) END,
FATTOTMERCE = DCGTOTMERCE,
FATTOTSCONTO = DCGRIETOTSCONTO,
FATTOTIMPO = DCGTOTMERCE - DCGRIETOTSCONTO,
FATANNOTA, 
MESSAGGIO = TaiNote,
FATSL = ISNULL((select RivaSL from coge.dbo.TbRegiva where RivaAnno = datepart(Year,FatData) and RivaNReg = FatNumreg),''),
CORCODART = CASE WHEN RTRIM(A.ANAPIVAEST) <> '' THEN CORCODART + ' - ' + ISNULL(ADGDogana,'') ELSE CORCODART END,
CORDESC,
CORUM,
CORQUACON,
CORPREZZO,
CORSC1,
CORIMPORTO = cast(CORQUACON * CORPREZZO * (1 - CORSC1 / 100) as decimal(11,2)),
CORCIVA = case when FatNimp = 0 then DBO.tci(CORCIVA) ELSE ( CASE  WHEN CORCODART <> 'BOLLO' THEN FatNimp ELSE DBO.tci(CORCIVA) END) END,
DESCOMAGGI = case when COROMAGGI = 'S' THEN 'OM.SOGG.' WHEN COROMAGGI = 'E' THEN 'OMAGGIO' ELSE '' END,
CLESPSCONTI,
DCGNUMRIF,DCGTIPO,DCGDATA,DCGRIECI1,DCGRIEIMP1,DCGRIEIVA1,DCGRIECI2,DCGRIEIMP2,DCGRIEIVA2,DCGRIECI3,DCGRIEIMP3,DCGRIEIVA3,DCGRIECI4,DCGRIEIMP4,DCGRIEIVA4,
IMPOSTA1 = CASE WHEN DCGRIECI1 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI1) ELSE '' END,
ALI1 = CASE WHEN DCGRIECI1 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI1) ELSE ''END,
IMPOSTA2 = CASE WHEN DCGRIECI2 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI2) ELSE '' END,
ALI2 = CASE WHEN DCGRIECI2 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI2) ELSE '' END,
IMPOSTA3 = CASE WHEN DCGRIECI3 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI3) ELSE '' END,
ALI3 = CASE WHEN DCGRIECI3 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI3) ELSE '' END,
IMPOSTA4 = CASE WHEN DCGRIECI4 > 0 THEN (SELECT ISNULL(CIIDES,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI4) ELSE '' END,
ALI4 = CASE WHEN DCGRIECI4 > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGRIECI4) ELSE'' END,
DCGTOTMERCE,DCGRIETOTIMP,DCGRIETOTESE,
--DCGSCULTER,DCGRIETOTSCONTO,
DCGRIETOTIVA,DCGRIETOTFAT,DCGRIENETTO,DCGSPEBOLLI,DCGSPERB,DCGRIETOTACC,
ALIRB = CASE WHEN DCGSPERBCI > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DBO.TCI(DCGSPERBCI)) ELSE '' END,
ALIIMB = CASE WHEN DCGSPEIMBCI > 0 THEN (SELECT ISNULL(CIIALI,'') FROM coge.dbo.TBCII WHERE CIICOD = DCGSPEIMBCI) ELSE '' END,
CORPROG,CORARTID,FATPVV
FROM TBFAT
INNER JOIN  TbCor ON CORRIF = FATRIF AND CORTIPODOC = FATTIPODOC 
INNER JOIN TBDCG ON DCGNUMRIF = FATRIF
INNER JOIN TbCli on CLCOD=FATCLIFAT
LEFT JOIN coge.dbo.TBCAB ON FATABI = CAABI AND FATCAB = CACAB 
LEFT JOIN coge.dbo.TBPAG ON PAGCOD = FATPAGCOD
LEFT OUTER JOIN VVETTORI v1 ON V1.VETCOD=FATVETT1
LEFT OUTER JOIN VVETTORI v2 ON V2.VETCOD=FATVETT2             
LEFT JOIN VDOX.DBO.TBANA AS A ON A.ANACOD = FATCLIFAT AND A.ANAGRP = 'CL'
LEFT JOIN VDOX.DBO.TBANA AS D ON D.ANACOD = CASE WHEN FatPvv='' THEN FATCLICONS ELSE FATPVV END AND D.ANAGRP = 'CL'
LEFT JOIN TBTCAU ON MGCAUID=FATCAU
LEFT OUTER JOIN COGE.dbo.TbBan ON ClcodBan = COGE.dbo.TbBan.BanCod  
left join TbArtDg on ADGArtCod = CorCodArt 
INNER JOIN TbTai on TaiAnno = datepart(year, FatData)
WHERE FATTIPODOC = 'F' AND FATDIFF <> 'D'
GO
/****** Object:  View [dbo].[VIEW4]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create VIEW [dbo].[VIEW4]
AS
SELECT     dbo.TbBol.BolNum, dbo.TbBol.BolRifFat, dbo.TbFat.FatNum, dbo.TbFat.FatData, dbo.TbFat.FatCliCons, dbo.TbBol.BolRif, dbo.TbBol.BolData,IsNull(FatTrasf,0) as FatTrasf
FROM         dbo.TbFat INNER JOIN
                      dbo.TbBol ON dbo.TbFat.FatRif = dbo.TbBol.BolRifFat







GO
/****** Object:  View [dbo].[VricDoc]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VricDoc]
AS
SELECT     Tipo = BolTipoDoc, DesTipo = 'D.D.T.', Numero = BolNum, Data = BolData, Cliente = BolCliCons, RagSoc = AnaDesc, Numrif = BolRif, 
                      Cau = BolCau,WebRif = 0, Deposito = BolNDep, CAUSALE = isnull((SELECT MgCauDesc from TbTCau where MgCauId = BolCau),''),Note = BolNote
FROM         TbBol INNER JOIN
                      vdox.dbo.TbAna ON Anacod = BolCliCons
UNION
SELECT     Tipo = FatTipoDoc, DesTipo = 'FATTURA', Numero = FatNum, Data = FatData, Cliente = FatCliCons, RagSoc = AnaDesc, Numrif = FatRif, 
                      Cau = FatCau,WebRif = 0, Deposito = FatNDep, CAUSALE = isnull((SELECT MgCauDesc from TbTCau where MgCauId = FatCau),''),Note = FatNote
FROM         TbFat INNER JOIN
                        vdox.dbo.TbAna ON Anacod = FatCliCons
WHERE     fatNumreg =
                          (SELECT     taiReg2
                            FROM          TbTai
                            WHERE      taiAnno = datepart(year, FatData))
UNION
SELECT     Tipo = SosTipoDoc, DesTipo = 'SOSPESO', Numero = SosNum, Data = SosData, Cliente = SosCliCons, RagSoc = AnaDesc, Numrif = SosRif, 
                      Cau = SosCau,WebRif = SosWebRif, Deposito = SosNDep, CAUSALE = isnull((SELECT MgCauDesc from TbTCau where MgCauId = SosCau),''),Note=SosNote
FROM         TbSos INNER JOIN
                        vdox.dbo.TbAna ON Anacod  = SosCliCons
WHERE     SosTrasf <> 1 AND SosTipoDoc = 'S'

UNION
SELECT     Tipo = SosTipoDoc, DesTipo = 'PREVENTIVO', Numero = SosNum, Data = SosData, Cliente = SosCliCons, RagSoc = AnaDesc, Numrif = SosRif, 
                      Cau = SosCau,WebRif = 0,Deposito = SosNDep, CAUSALE = isnull((SELECT MgCauDesc from TbTCau where MgCauId = SosCau),''),Note=SosNote
FROM         TbSos INNER JOIN
                        vdox.dbo.TbAna ON Anacod = SosCliCons
WHERE      SosTipoDoc = 'P' and  SosTrasf <> 1












GO
/****** Object:  View [dbo].[VCliEff]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[VCliEff]
AS
SELECT     dbo.TbCli.ClCod, vdox.dbo.TbAna.AnaDesc, dbo.TbCli.ClAbi, dbo.TbCli.ClCab
FROM         vdox.dbo.TbAna INNER JOIN
                      dbo.TbCli ON vdox.dbo.TbAna.AnaCod = dbo.TbCli.ClCod
WHERE     (vdox.dbo.TbAna.AnaGrp = 'CL')





GO
/****** Object:  Table [dbo].[TbFteCli]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFteCli](
	[ClFteCod] [varchar](5) NOT NULL,
	[ClFteDestinatario] [varchar](7) NOT NULL,
	[ClFtePec] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[DXCLIENTI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[DXCLIENTI]
AS
SELECT   TOP 100 PERCENT Cod=AnaCod ,[Ragione Sociale]=AnaDesc,[Partita Iva]=AnaPiva,[Codice Fisc]=AnaCfis,[Part.Iva Estera]=AnaPivaEst,Indirizzo=AnaIndirizzo,Cap=AnaCap,Città=AnaCitta,Pr=AnaProv,[Nazione]=ClNazione,[Regione]=ClRegione,
					  Telefono=AnaTel1,Fax=AnaFax,[Telefono 2]=AnaTel2,Cellulare=AnaTel3,Email=AnaEmail,Web=AnaWww,Contatto=AnaResp,Agente=ClCodage,[Area Manager]=ClCodMan,
                      Pagamento=CAST(PagCod AS varchar) + ' ' + PagDesc,Banca=CaDescFt,
					  Iban=ISNULL(CaPaese, 'IT') + REPLICATE('0', 2 - DATALENGTH(ClCinEur)) + ClCinEur + REPLICATE(' ',1 - DATALENGTH(ClCin)) + ClCin + REPLICATE('0', 5 - DATALENGTH(CAST(CaAbi AS varchar))) 
                      + CAST(CaAbi AS varchar) + REPLICATE('0', 5 - DATALENGTH(CAST(CaCab AS varchar))) + CAST(CaCab AS varchar) + REPLICATE('0', 12 - DATALENGTH(ClCC)) + ClCC, 
                      ABI=ClAbi,CAB=ClCab,[Cod.Banca]=ClcodBan, [Mandato Rid]=ClCntRid,
					  [Destinatario FTE]=GEVE.DBO.TbFteCli.ClFteDestinatario,[Pec FTE]=GEVE.DBO.TbFteCli.ClFtePec, [Iva NI]=ClCodivani,[Grup Att]=ClGrCanale, 
                      [Esente Spese]=ClEseSpe,[Invia Mail]=isnull(ClInviaMail,0),Attivo=ClAttivo,[No Rubrica]=AnaNoRubrica,					 
                      [Data Creazione] = ClCreateDataOra,
					  [Data Modifica]  = ClUpdateDataOra,[Note Orario]=ClOrario,[Raggruppo Bolle]=case when ClBolRag=1 then 'SI' else 'NO' end
   FROM         VDOX.dbo.TbAna 
   INNER JOIN   TbCli ON VDOX.dbo.TbAna.AnaCod = ClCod 
   LEFT OUTER JOIN COGE.dbo.TbPag ON ClPagam = COGE.dbo.TbPag.PagCod
   LEFT OUTER JOIN COGE.dbo.TbCab ON ClAbi = COGE.dbo.TbCab.CaAbi AND ClCab = COGE.dbo.TbCab.CaCab
   LEFT OUTER JOIN GEVE.DBO.TbFteCli ON ClCod = ClFteCod
   WHERE     (VDOX.dbo.TbAna.AnaGrp = 'CL') order by clcod



GO
/****** Object:  View [dbo].[VFATTUREEMESSE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE view [dbo].[VFATTUREEMESSE]
AS

select FatRif,FatNum,FatData,FatNumReg,FatTrasf,FatCliFat,FatCliCons,ISNULL(DcgRieTotFat,0) as DcgRieTotFat,FatPagCod,FatAbi,FatCab,
       FatNimp,
       CLIENTEFATTURA = (select AnaDesc from Vdox.Dbo.TbAna where anacod = FatCliFat and AnaGrp = 'CL'),
       CLIENTECONSEGNA = (select AnaDesc from Vdox.Dbo.TbAna where anacod = FatCliCons and AnaGrp = 'CL'),
       FatDiff,FatNDep
from TbFat inner join
     vdox.Dbo.TbAna on anacod = FatCliFat and AnaGrp = 'CL' LEFT OUTER JOIN
     TbDcg on dcgNumrif = FatRif
     






GO
/****** Object:  Table [dbo].[TbNewFasCli]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbNewFasCli](
	[FasCli] [varchar](5) NOT NULL,
	[FasCategoria] [varchar](2) NOT NULL,
	[FasArtId] [int] NOT NULL,
	[FasSc1] [decimal](5, 2) NOT NULL,
	[FasSc2] [decimal](5, 2) NOT NULL,
	[FasSc3] [decimal](5, 2) NOT NULL,
	[FasMg1] [decimal](5, 2) NOT NULL,
	[FasMg2] [decimal](5, 2) NOT NULL,
	[FasMg3] [decimal](5, 2) NOT NULL,
	[FasImb] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_TbNewFasCli_1] PRIMARY KEY CLUSTERED 
(
	[FasCli] ASC,
	[FasCategoria] ASC,
	[FasArtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VTbNewFasCli]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[VTbNewFasCli]
as

SELECT FasCli,
       FasCategoria,
	   FasArtId,
	   FasSc1,
	   CATEGORIA = isnull((Select CatDesc from TbCat where CatCod = FasCategoria),''),
	   ARTCOD = isnull(ArtCod,''),
	   PRODOTTO = isnull(ArtDesc,'')
from TbNewFasCli left outer join
     TbArt on ArtId = FasArtId
GO
/****** Object:  Table [dbo].[TbTes]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTes](
	[TesRif] [int] IDENTITY(1,1) NOT NULL,
	[TesTipoDoc] [varchar](1) NOT NULL,
	[TesNum] [int] NOT NULL,
	[TesData] [smalldatetime] NOT NULL,
	[TesCau] [smallint] NOT NULL,
	[TesCliFor] [varchar](5) NOT NULL,
	[TesGClFo] [varchar](2) NOT NULL,
	[TesNdep] [smallint] NOT NULL,
	[TesPagCod] [smallint] NOT NULL,
	[TesAbi] [int] NOT NULL,
	[TesCab] [int] NOT NULL,
	[TesSculter] [decimal](4, 2) NOT NULL,
	[TesPorto] [varchar](10) NOT NULL,
	[TesVett1] [smallint] NOT NULL,
	[TesVett2] [smallint] NOT NULL,
	[TesAcconto] [decimal](9, 2) NOT NULL,
	[TesNumReg] [smallint] NOT NULL,
	[TesNumProt] [int] NOT NULL,
	[TesNumAnno] [smallint] NOT NULL,
	[TesBanca] [varchar](25) NOT NULL,
	[TesAge] [smallint] NOT NULL,
	[TesTrasf] [bit] NOT NULL,
	[TesDepDep] [smallint] NOT NULL,
	[TesDepRif] [int] NOT NULL,
	[TesGancioRif] [int] NOT NULL,
	[TesDMV] [varchar](12) NOT NULL,
	[TesDataRitiro1] [smalldatetime] NULL,
	[TesDataRitiro2] [smalldatetime] NULL,
	[TesNColli] [int] NOT NULL,
	[TesPeso] [decimal](7, 3) NOT NULL,
	[TesDataTrasp] [smalldatetime] NULL,
	[TesAspBeni] [varchar](25) NOT NULL,
	[TesValBol] [bit] NOT NULL,
	[TesAnnota] [varchar](150) NOT NULL,
	[TesBolRif] [int] NOT NULL,
	[TesFatRif] [int] NOT NULL,
	[TesDataArrivo] [datetime] NULL,
	[TesBEvasione] [bit] NOT NULL,
 CONSTRAINT [PK_TbTes] PRIMARY KEY CLUSTERED 
(
	[TesRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[XVPRINTDDTF]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[XVPRINTDDTF]
AS
SELECT TOP 100 PERCENT TESRIF AS BOLRIF,TESNUM AS BOLNUM,TESDATA AS BOLDATA,TESCLIFOR AS BOLCLICONS,TESCLIFOR AS BOLCLIFAT
                       --v1.VETANA as VETANA1,v2.VETANA as VETANA2,v1.VETDOM as VETDOM1,v2.VETDOM as VETDOM2,
                       --CORCODART =CASE WHEN ARTCOD > 0 THEN (REPLICATE('0', 4 - dataLENGTH(cast(ARTCOD  as varchar))) + cast(ARTCOD  as varchar)) ELSE CAST(CORCODART AS VARCHAR(5)) END,
                       --CORDESC =  CORDESC,                                    
                       --CORLOTTO,CORPREZZO,CORUMISVEN = CORUMISURA,CORQUACON AS CORQUAVEN,CORSCONTO1 AS SC1, CORSCONTO2 AS SC2,
                       --CORIMPORTO = (CORPREZZO * ( 1 - CORSCONTO1 / 100) * (1 - CORSCONTO2 / 100)) * CORQUACON ,
                       --CORCIVA,
                       --A.ANARAG1 AS ANARAG1,A.ANARAG2 AS ANARAG2,A.ANAINDIRIZZO AS ANAINDIRIZZO,A.ANACAP AS ANACAP,A.ANACITTA AS ANACITTA,A.ANAPROV AS ANAPROV,
                       --B.ANARAG1 AS FATRAG1,B.ANARAG2 AS FATRAG2,B.ANAINDIRIZZO AS FATINDIRIZZO,B.ANACAP AS FATCAP,B.ANACITTA AS FATCITTA,B.ANAPROV AS FATPROV,
                       --FATPIVA = CASE  WHEN RTRIM(B.ANAPIVAEST) <> '' THEN B.ANAPIVAEST ELSE B.ANAPIVA END,
                       --TESDMV,MGCAUDESC,TESNCOLLI,TESPESO,TESPORTO, TESASPBENI,B.ANACFIS AS FATCFIS,
                       --PAGAM = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7 then CAST(TESPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(TESABI AS VARCHAR(5))+ ' CAB '+CAST(TESCAB AS VARCHAR(5)) else CAST(TESPAGCOD as varchar(3))+' '+PAGDESC end,
                       --ISNULL(CADESCFT,'') AS CADESCFT,
                       --TESDATATRASP,TESDATARITIRO1,TESDATARITIRO2,TESSPVARIE = 0,TESSPTRASP = 0,TESVALBOL,TESTOTALE = 0,TESTOTMERCE = 0,
                       --TESTOTSCONTO =0,vocsoloimporto=Cast(0 as bit),TESANNOTA
                       FROM TBTES
                       INNER JOIN TBCOR ON TESRIF = CORRIF AND TESTIPODOC = CORTIPODOC 
                       --LEFT JOIN VartMat on ArtCod = CorCodart
                       LEFT JOIN VDOX.DBO.TBANA AS B ON B.ANACOD = TESCLIFOR AND B.ANAGRP = 'FO'
                       LEFT JOIN VDOX.DBO.TBANA AS A ON A.ANACOD = TESCLIFOR AND A.ANAGRP = 'FO'
                       LEFT JOIN TBTCAU ON MGCAUID=TESCAU
                      -- LEFT OUTER JOIN TBVET v1 ON TESVETT1=v1.VETID
                       --LEFT OUTER JOIN TBVET v2 ON TESVETT2=v2.VETID
                       LEFT OUTER JOIN TBFOR ON FOCOD=TESCLIFOR
                       LEFT JOIN coge.dbo.TBPAG ON PAGCOD = TESPAGCOD
                       LEFT JOIN coge.dbo.TBCAB ON TESABI = CAABI AND TESCAB = CACAB 
                       WHERE TESTIPODOC = 'D' 
                       order by TESNum,CorProg
GO
/****** Object:  View [dbo].[XVPRINTDDT]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[XVPRINTDDT]
AS
SELECT  BOLRIF,BOLNUM,BOLDATA,BOLCLICONS,BOLCLIFAT,
VETANA1 = V1.ANADESC,
VETANA2 = V2.ANADESC,
VETDOM1 = LTRIM(V1.ANAINDIRIZZO) + ' ' + LTRIM(V1.ANACAP) + ' ' + LTRIM(V1.ANACITTA)  + ' ' + LTRIM(V1.ANAPROV),
VETDOM2 = LTRIM(V2.ANAINDIRIZZO) + ' ' + LTRIM(V2.ANACAP) + ' ' + LTRIM(V2.ANACITTA)  + ' ' + LTRIM(V2.ANAPROV),
CORCODART ,
CORDESC,
CORUM,
CORQUACON,
CORPREZZO,
CORSC1,
CORIMPORTO = cast(CORQUACON * CORPREZZO * (1 - CORSC1 / 100) as decimal(11,2)),                      
CORCIVA = case when bolNImp = 0 then DBO.tci(CORCIVA) ELSE BOLNIMP END,
D_ANARAG1 = D.ANARAG1,
D_ANARAG2 = D.ANARAG2,
D_ANAINDIRIZZO = D.ANAINDIRIZZO,
D_ANACAP = D.ANACAP,
D_ANACITTA = D.ANACITTA,
D_ANAPROV = D.ANAPROV,
D_CITTA = D.ANACAP + ' - ' +  D.ANACITTA + ' - ' +  D.ANAPROV,
ANARAG1 = A.ANARAG1,
ANARAG2 = A.ANARAG2,
ANAINDIRIZZO =A.ANAINDIRIZZO,
ANACAP = A.ANACAP,
ANACITTA = A.ANACITTA,
ANAPROV = A.ANAPROV,
ANACFIS = A.ANACFIS,
ANAPIVA = A.ANAPIVA,
ANAPIVAEST = A.ANAPIVAEST,
CITTA = A.ANACAP + ' - ' +  A.ANACITTA + ' - ' +  A.ANAPROV,                      
FATPIVA = CASE  WHEN RTRIM(A.ANAPIVAEST) <> '' THEN A.ANAPIVAEST ELSE A.ANAPIVA END,
BOLDMV,MGCAUDESC,BOLNCOLLI, BOLPESONETTO, BOLPESOLORDO, BOLPORTO, BOLASPBENI,
PAGAM = CASE WHEN PAGTIPO = 2 or PAGTIPO = 7 then CAST(BOLPAGCOD AS VARCHAR(3))+' '+PAGDESC+' ABI '+CAST(BOLABI AS VARCHAR(5))+ ' CAB '+CAST(BOLCAB AS VARCHAR(5)) else CAST(BOLpagcod as varchar(3))+' '+PAGDESC end,
CADESCFT = ISNULL(CADESCFT,''),
BOLDATATRASP,BOLDATARITIRO1,BOLDATARITIRO2,
BOLTOTALE,
BOLANNOTA,
MESSAGGIO = TaiNote, 
CLESPSCONTI,
BOLPVV
FROM TBBOL
INNER JOIN TbCor ON BOLRIF = CORRIF AND BOLTIPODOC = CORTIPODOC
INNER JOIN TbCli on CLCOD=BOLCLIFAT
LEFT JOIN coge.dbo.TBPAG ON PAGCOD = BOLPAGCOD
LEFT JOIN VDOX.DBO.TBANA AS A ON A.ANACOD = BOLCLIFAT AND A.ANAGRP = 'CL'
LEFT JOIN VDOX.DBO.TBANA AS D ON D.ANACOD = CASE WHEN BolPvv='' THEN BOLCLICONS ELSE BOLPVV END AND D.ANAGRP = 'CL'
LEFT JOIN TBTCAU ON MGCAUID=BOLCAU
LEFT OUTER JOIN VVETTORI v1 ON V1.VETCOD=BOLVETT1
LEFT OUTER JOIN VVETTORI v2 ON V2.VETCOD=BOLVETT2               
LEFT JOIN coge.dbo.TBCAB ON BOLABI = CAABI AND BOLCAB = CACAB 
INNER JOIN TbTai on TaiAnno = datepart(year, BolData)
WHERE BOLTIPODOC = 'B' 

GO
/****** Object:  Table [dbo].[ANACLI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANACLI](
	[Codice] [nvarchar](255) NULL,
	[Denominazione] [nvarchar](255) NULL,
	[Indirizzo] [nvarchar](255) NULL,
	[Cap] [nvarchar](255) NULL,
	[Comune] [nvarchar](255) NULL,
	[Prov#] [nvarchar](255) NULL,
	[Telefono] [nvarchar](255) NULL,
	[Fax] [nvarchar](255) NULL,
	[EMail] [nvarchar](255) NULL,
	[Referente] [nvarchar](255) NULL,
	[Pagamento] [nvarchar](255) NULL,
	[Partita IVA] [nvarchar](255) NULL,
	[Codice Fiscale] [nvarchar](255) NULL,
	[Nazione] [nvarchar](255) NULL,
	[PagCod] [smallint] NOT NULL,
	[XClCod] [varchar](5) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANAPROD]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANAPROD](
	[CODICE] [nvarchar](255) NULL,
	[DESCRIZIONE] [nvarchar](255) NULL,
	[MINIDESC] [nvarchar](255) NULL,
	[CATEGORIA] [nvarchar](255) NULL,
	[PREZZO] [float] NULL,
	[UMA] [nvarchar](255) NULL,
	[CATCOD] [varchar](2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Back_TbArt]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Back_TbArt](
	[ArtId] [int] IDENTITY(1,1) NOT NULL,
	[ArtCod] [varchar](20) NOT NULL,
	[ArtDesc] [varchar](100) NOT NULL,
	[ArtMinidesc] [varchar](25) NOT NULL,
	[ArtTipoProd] [varchar](1) NOT NULL,
	[ArtCat] [varchar](2) NOT NULL,
	[ArtTip] [varchar](2) NOT NULL,
	[ArtSiglaSconto] [varchar](2) NOT NULL,
	[ArtUmisura] [varchar](2) NOT NULL,
	[ArtPesoCnf] [decimal](5, 3) NOT NULL,
	[ArtNumImballo] [decimal](5, 0) NOT NULL,
	[ArtPeso] [decimal](6, 3) NOT NULL,
	[ArtCodiva] [smallint] NOT NULL,
	[ArtCptcon] [varchar](5) NOT NULL,
	[ArtFlagAttivo] [bit] NOT NULL,
	[ArtScadenza] [smallint] NOT NULL,
	[ArtDescLis] [varchar](60) NOT NULL,
	[ArtNoteLis] [varchar](25) NOT NULL,
	[ArtDistinta] [bit] NOT NULL,
	[ArtUmVen] [varchar](2) NOT NULL,
	[ArtUmTec] [varchar](2) NOT NULL,
	[ArtDiBase] [smallint] NOT NULL,
	[ArtCiAcq] [smallint] NOT NULL,
	[ArtCptAcq] [varchar](5) NOT NULL,
	[ArtMultimb] [bit] NOT NULL,
	[ArtDesc2] [varchar](40) NOT NULL,
	[ArtDesc3] [varchar](40) NOT NULL,
	[ArtDesc4] [varchar](40) NOT NULL,
	[ArtDesc5] [varchar](40) NOT NULL,
	[ArtFamiglia] [smallint] NOT NULL,
	[ArtAcquistato] [bit] NOT NULL,
	[ArtRipieno] [smallint] NOT NULL,
	[ArtTipo62] [tinyint] NOT NULL,
	[ArtEAN13] [varchar](13) NOT NULL,
	[ArtAttivoStore] [bit] NOT NULL,
	[ArtProduzStore] [bit] NOT NULL,
	[ArtSfuso] [bit] NOT NULL,
	[ArtImballoStore] [smallint] NOT NULL,
	[ArtPesoCnfStore] [decimal](5, 3) NOT NULL,
	[ArtProducer] [varchar](1) NOT NULL,
	[ArtTest] [bit] NOT NULL,
	[ArtCatP] [varchar](1) NOT NULL,
	[ArtSL] [smallint] NOT NULL,
	[ArtGruppo] [smallint] NOT NULL,
	[ArtPrzAcquisto] [decimal](9, 2) NOT NULL,
	[ArtPrzVendita] [decimal](9, 2) NOT NULL,
	[ArtSLC] [smallint] NOT NULL,
	[Art_CAMPIONATURA] [bit] NOT NULL,
	[ArtXDesc] [varchar](40) NOT NULL,
	[ArtXDesc2] [varchar](40) NOT NULL,
	[ArtXDesc3] [varchar](40) NOT NULL,
	[ArtXDesc4] [varchar](40) NOT NULL,
	[ArtXDesc5] [varchar](40) NOT NULL,
	[ArtXFlag] [bit] NOT NULL,
	[ArtFormato] [int] NOT NULL,
	[ArtDisp_Effettiva] [tinyint] NOT NULL,
	[ArtColore] [int] NOT NULL,
	[ArtDbTipo] [varchar](1) NOT NULL,
	[ArtImbKit] [smallint] NOT NULL,
	[ArtBarcode] [varchar](50) NOT NULL,
	[PesoGr] [int] NOT NULL,
	[ArtIngredienti] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[back_Tbart_09102024]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[back_Tbart_09102024](
	[ArtId] [int] IDENTITY(1,1) NOT NULL,
	[ArtCod] [varchar](20) NOT NULL,
	[ArtDesc] [varchar](100) NOT NULL,
	[ArtMinidesc] [varchar](25) NOT NULL,
	[ArtTipoProd] [varchar](1) NOT NULL,
	[ArtCat] [varchar](2) NOT NULL,
	[ArtTip] [varchar](2) NOT NULL,
	[ArtSiglaSconto] [varchar](2) NOT NULL,
	[ArtUmisura] [varchar](2) NOT NULL,
	[ArtPesoCnf] [decimal](5, 3) NOT NULL,
	[ArtNumImballo] [decimal](5, 0) NOT NULL,
	[ArtPeso] [decimal](6, 3) NOT NULL,
	[ArtCodiva] [smallint] NOT NULL,
	[ArtCptcon] [varchar](5) NOT NULL,
	[ArtFlagAttivo] [bit] NOT NULL,
	[ArtScadenza] [smallint] NOT NULL,
	[ArtDescLis] [varchar](60) NOT NULL,
	[ArtNoteLis] [varchar](25) NOT NULL,
	[ArtDistinta] [bit] NOT NULL,
	[ArtUmVen] [varchar](2) NOT NULL,
	[ArtUmTec] [varchar](2) NOT NULL,
	[ArtDiBase] [smallint] NOT NULL,
	[ArtCiAcq] [smallint] NOT NULL,
	[ArtCptAcq] [varchar](5) NOT NULL,
	[ArtMultimb] [bit] NOT NULL,
	[ArtDesc2] [varchar](40) NOT NULL,
	[ArtDesc3] [varchar](40) NOT NULL,
	[ArtDesc4] [varchar](40) NOT NULL,
	[ArtDesc5] [varchar](40) NOT NULL,
	[ArtFamiglia] [smallint] NOT NULL,
	[ArtAcquistato] [bit] NOT NULL,
	[ArtRipieno] [smallint] NOT NULL,
	[ArtTipo62] [tinyint] NOT NULL,
	[ArtEAN13] [varchar](13) NOT NULL,
	[ArtAttivoStore] [bit] NOT NULL,
	[ArtProduzStore] [bit] NOT NULL,
	[ArtSfuso] [bit] NOT NULL,
	[ArtImballoStore] [smallint] NOT NULL,
	[ArtPesoCnfStore] [decimal](5, 3) NOT NULL,
	[ArtProducer] [varchar](1) NOT NULL,
	[ArtTest] [bit] NOT NULL,
	[ArtCatP] [varchar](1) NOT NULL,
	[ArtSL] [smallint] NOT NULL,
	[ArtGruppo] [smallint] NOT NULL,
	[ArtPrzAcquisto] [decimal](9, 2) NOT NULL,
	[ArtPrzVendita] [decimal](9, 2) NOT NULL,
	[ArtSLC] [smallint] NOT NULL,
	[Art_CAMPIONATURA] [bit] NOT NULL,
	[ArtXDesc] [varchar](40) NOT NULL,
	[ArtXDesc2] [varchar](40) NOT NULL,
	[ArtXDesc3] [varchar](40) NOT NULL,
	[ArtXDesc4] [varchar](40) NOT NULL,
	[ArtXDesc5] [varchar](40) NOT NULL,
	[ArtXFlag] [bit] NOT NULL,
	[ArtFormato] [int] NOT NULL,
	[ArtDisp_Effettiva] [tinyint] NOT NULL,
	[ArtColore] [int] NOT NULL,
	[ArtDbTipo] [varchar](1) NOT NULL,
	[ArtImbKit] [smallint] NOT NULL,
	[ArtBarcode] [varchar](50) NOT NULL,
	[PesoGr] [int] NOT NULL,
	[ArtIngredienti] [varbinary](max) NULL,
	[ArtBilancia] [bit] NOT NULL,
	[ArtCodBil] [int] NOT NULL,
	[ArtBarcodeForn] [varchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[back_TbArt_141024]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[back_TbArt_141024](
	[ArtId] [int] IDENTITY(1,1) NOT NULL,
	[ArtCod] [varchar](20) NOT NULL,
	[ArtDesc] [varchar](100) NOT NULL,
	[ArtMinidesc] [varchar](25) NOT NULL,
	[ArtTipoProd] [varchar](1) NOT NULL,
	[ArtCat] [varchar](2) NOT NULL,
	[ArtTip] [varchar](2) NOT NULL,
	[ArtSiglaSconto] [varchar](2) NOT NULL,
	[ArtUmisura] [varchar](2) NOT NULL,
	[ArtPesoCnf] [decimal](5, 3) NOT NULL,
	[ArtNumImballo] [decimal](5, 0) NOT NULL,
	[ArtPeso] [decimal](6, 3) NOT NULL,
	[ArtCodiva] [smallint] NOT NULL,
	[ArtCptcon] [varchar](5) NOT NULL,
	[ArtFlagAttivo] [bit] NOT NULL,
	[ArtScadenza] [smallint] NOT NULL,
	[ArtDescLis] [varchar](60) NOT NULL,
	[ArtNoteLis] [varchar](25) NOT NULL,
	[ArtDistinta] [bit] NOT NULL,
	[ArtUmVen] [varchar](2) NOT NULL,
	[ArtUmTec] [varchar](2) NOT NULL,
	[ArtDiBase] [smallint] NOT NULL,
	[ArtCiAcq] [smallint] NOT NULL,
	[ArtCptAcq] [varchar](5) NOT NULL,
	[ArtMultimb] [bit] NOT NULL,
	[ArtDesc2] [varchar](40) NOT NULL,
	[ArtDesc3] [varchar](40) NOT NULL,
	[ArtDesc4] [varchar](40) NOT NULL,
	[ArtDesc5] [varchar](40) NOT NULL,
	[ArtFamiglia] [smallint] NOT NULL,
	[ArtAcquistato] [bit] NOT NULL,
	[ArtRipieno] [smallint] NOT NULL,
	[ArtTipo62] [tinyint] NOT NULL,
	[ArtEAN13] [varchar](13) NOT NULL,
	[ArtAttivoStore] [bit] NOT NULL,
	[ArtProduzStore] [bit] NOT NULL,
	[ArtSfuso] [bit] NOT NULL,
	[ArtImballoStore] [smallint] NOT NULL,
	[ArtPesoCnfStore] [decimal](5, 3) NOT NULL,
	[ArtProducer] [varchar](1) NOT NULL,
	[ArtTest] [bit] NOT NULL,
	[ArtCatP] [varchar](1) NOT NULL,
	[ArtSL] [smallint] NOT NULL,
	[ArtGruppo] [smallint] NOT NULL,
	[ArtPrzAcquisto] [decimal](9, 2) NOT NULL,
	[ArtPrzVendita] [decimal](9, 2) NOT NULL,
	[ArtSLC] [smallint] NOT NULL,
	[Art_CAMPIONATURA] [bit] NOT NULL,
	[ArtXDesc] [varchar](40) NOT NULL,
	[ArtXDesc2] [varchar](40) NOT NULL,
	[ArtXDesc3] [varchar](40) NOT NULL,
	[ArtXDesc4] [varchar](40) NOT NULL,
	[ArtXDesc5] [varchar](40) NOT NULL,
	[ArtXFlag] [bit] NOT NULL,
	[ArtFormato] [int] NOT NULL,
	[ArtDisp_Effettiva] [tinyint] NOT NULL,
	[ArtColore] [int] NOT NULL,
	[ArtDbTipo] [varchar](1) NOT NULL,
	[ArtImbKit] [smallint] NOT NULL,
	[ArtBarcode] [varchar](50) NOT NULL,
	[PesoGr] [int] NOT NULL,
	[ArtIngredienti] [varbinary](max) NULL,
	[ArtBilancia] [bit] NOT NULL,
	[ArtCodBil] [int] NOT NULL,
	[ArtBarcodeForn] [varchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Back_TbLis]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Back_TbLis](
	[LisId] [int] NOT NULL,
	[LisValiditaDal] [smalldatetime] NOT NULL,
	[LisVendita] [decimal](10, 3) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[back_TbLis_09102024]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[back_TbLis_09102024](
	[LisId] [int] NOT NULL,
	[LisValiditaDal] [smalldatetime] NOT NULL,
	[LisVendita] [decimal](10, 3) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[back_TbSlCli_142024]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[back_TbSlCli_142024](
	[SlCli] [varchar](5) NOT NULL,
	[SlArtCod] [varchar](20) NOT NULL,
	[SlCliPrezzo] [decimal](9, 3) NOT NULL,
	[SlNo] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CATEGORIE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CATEGORIE](
	[COD] [varchar](2) NULL,
	[CATEGORIA] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CIMBALLO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CIMBALLO](
	[CODICE] [nvarchar](255) NULL,
	[DESCRIZIONE] [nvarchar](255) NULL,
	[PREZZO] [float] NULL,
	[PREZZIVA] [float] NULL,
	[QTA] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COND_PAGAM]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COND_PAGAM](
	[PAGAMENTO] [nvarchar](255) NULL,
	[CODICE] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DOPPI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DOPPI](
	[NEW_CODICE] [varchar](20) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ESTERE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ESTERE](
	[Xnazione] [nvarchar](255) NULL,
	[sigla] [varchar](2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MODIFICA_CODICE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MODIFICA_CODICE](
	[ArtId] [int] NOT NULL,
	[OLD_CODICE] [varchar](20) NOT NULL,
	[NEW_CODICE] [varchar](20) NOT NULL,
	[DESCRIZIONE] [varchar](100) NOT NULL,
	[CATEGORIA] [varchar](30) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Panettoni]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Panettoni](
	[CodiceProdotto] [nvarchar](10) NOT NULL,
	[EAN] [nvarchar](20) NULL,
	[NomeProdotto] [nvarchar](255) NULL,
	[DescrizioneCompleta] [nvarchar](max) NULL,
	[Dibase] [varchar](20) NULL,
	[DiBaseID] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pasticceria]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pasticceria](
	[CodiceProdotto] [nvarchar](10) NOT NULL,
	[EAN] [nvarchar](20) NULL,
	[NomeProdotto] [nvarchar](255) NULL,
	[DescrizioneCompleta] [nvarchar](max) NULL,
	[DiBase] [varchar](20) NULL,
	[DiBaseId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CodiceProdotto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[EAN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PIMBALLO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PIMBALLO](
	[CODICE] [nvarchar](255) NULL,
	[DESCRIZIONE] [nvarchar](255) NULL,
	[PREZZO] [float] NULL,
	[PREZZIVA] [float] NULL,
	[QTA] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pulizia]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pulizia](
	[iArtCod] [varchar](20) NOT NULL,
	[iArtDesc] [varchar](100) NOT NULL,
	[DaEliminare] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbAPrezzo]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAPrezzo](
	[APId] [int] IDENTITY(1,1) NOT NULL,
	[APCodice] [varchar](20) NOT NULL,
 CONSTRAINT [PK_TbAPrezzo] PRIMARY KEY CLUSTERED 
(
	[APId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbArtPF]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbArtPF](
	[PFArtCod] [int] NOT NULL,
	[PFArtCodPad] [int] NOT NULL,
 CONSTRAINT [PK_TbArtPF] PRIMARY KEY CLUSTERED 
(
	[PFArtCod] ASC,
	[PFArtCodPad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbAspBeni]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAspBeni](
	[AspBeniCod] [int] IDENTITY(1,1) NOT NULL,
	[AspBeniDes] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbAspBeni] PRIMARY KEY CLUSTERED 
(
	[AspBeniCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbAtt]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAtt](
	[AttCod] [smallint] NOT NULL,
	[AttDesc] [varchar](25) NOT NULL,
	[AttAnaCli] [bit] NOT NULL,
 CONSTRAINT [PK_TbAtt] PRIMARY KEY CLUSTERED 
(
	[AttCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbAziPA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAziPA](
	[APRegFiscale] [varchar](4) NOT NULL,
	[APReaProv] [varchar](2) NOT NULL,
	[APReaNum] [varchar](20) NOT NULL,
	[APReaCapSoc] [varchar](20) NOT NULL,
	[APReaSocioUnico] [tinyint] NOT NULL,
	[APReaLiquidazione] [bit] NOT NULL,
	[APUltimoINVIO] [bigint] NOT NULL,
	[APCartellaINVIO] [varchar](50) NOT NULL,
	[APCodFiscTrasm] [varchar](16) NOT NULL,
	[APCodFisc3Int] [varchar](16) NOT NULL,
	[APRagSoc3Int] [varchar](60) NOT NULL,
	[APAttiva] [bit] NOT NULL,
	[APNome3Int] [varchar](50) NULL,
	[APCognome3Int] [varchar](50) NULL,
	[APAllegoPDF] [bit] NOT NULL,
	[APUser] [varchar](30) NOT NULL,
	[APPassword] [varchar](30) NOT NULL,
	[APPostazione] [varchar](30) NOT NULL,
	[APIstituto] [varchar](30) NOT NULL,
	[APCodiceAzienda] [varchar](30) NOT NULL,
	[APPortaleTest] [bit] NOT NULL,
	[APGGIndietro] [smallint] NOT NULL,
	[APRegEsterne] [smallint] NOT NULL,
	[APBluenext] [bit] NOT NULL,
	[APBnUser] [varchar](30) NOT NULL,
	[APBnPassword] [varchar](30) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbButtom]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbButtom](
	[MgiD] [smallint] NOT NULL,
	[MgDesc] [varchar](30) NOT NULL,
	[MgPos] [smallint] NOT NULL,
	[MgFormatQ] [varchar](20) NOT NULL,
	[MgFormatV] [varchar](20) NOT NULL,
	[MgFormula] [varchar](20) NOT NULL,
	[MgCheckQ] [bit] NOT NULL,
	[MgCheckV] [bit] NOT NULL,
 CONSTRAINT [PK_TbButtom] PRIMARY KEY CLUSTERED 
(
	[MgiD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCalendarioIso]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCalendarioIso](
	[ISOAAAASS] [varchar](6) NOT NULL,
	[ISOANNO] [int] NOT NULL,
	[ISOSETT] [int] NOT NULL,
	[ISOINIZIO] [smalldatetime] NULL,
	[ISOFINE] [smalldatetime] NULL,
	[MONANNO] [int] NOT NULL,
	[MONSETT] [int] NOT NULL,
	[MONINIZIO] [smalldatetime] NULL,
	[MONFINE] [smalldatetime] NULL,
 CONSTRAINT [PK_TbCalendarioIso] PRIMARY KEY CLUSTERED 
(
	[ISOAAAASS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCCau]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCCau](
	[MgCauCid] [smallint] NOT NULL,
	[MgCauCButtom] [smallint] NOT NULL,
	[MgCauCheckQ] [bit] NOT NULL,
	[MgCauCheckV] [bit] NOT NULL,
 CONSTRAINT [PK_TbCCau] PRIMARY KEY CLUSTERED 
(
	[MgCauCid] ASC,
	[MgCauCButtom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbColori]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbColori](
	[ColId] [int] IDENTITY(1,1) NOT NULL,
	[ColDesc] [varchar](30) NOT NULL,
 CONSTRAINT [PK_TbColori] PRIMARY KEY CLUSTERED 
(
	[ColId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbDep]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbDep](
	[DepCod] [smallint] NOT NULL,
	[DepDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbDep] PRIMARY KEY CLUSTERED 
(
	[DepCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbDFA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbDFA](
	[DFatId] [int] IDENTITY(1,1) NOT NULL,
	[DFatNum] [int] NOT NULL,
	[DFatData] [smalldatetime] NOT NULL,
	[DFatNumReg] [smallint] NOT NULL,
	[DFatTrasf] [bit] NOT NULL,
	[DFatTerminale] [varchar](20) NOT NULL,
	[DFatDataOra] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_TbDFat] PRIMARY KEY CLUSTERED 
(
	[DFatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbDogana]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbDogana](
	[DGCodice] [varchar](16) NOT NULL,
	[DGDescri] [varchar](100) NOT NULL,
	[DGTipo] [varchar](1) NOT NULL,
 CONSTRAINT [PK_TbDogana_1] PRIMARY KEY CLUSTERED 
(
	[DGCodice] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbEccezioniChiusura]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEccezioniChiusura](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DataInizio] [date] NOT NULL,
	[DataFine] [date] NOT NULL,
	[CodiceEccezione] [int] NOT NULL,
	[Slot] [int] NULL,
	[OraInizio] [time](7) NULL,
	[OraFine] [time](7) NULL,
	[Note] [varchar](100) NULL,
	[ParentID] [int] NOT NULL,
 CONSTRAINT [PK__TbEccezi__3214EC27460380CE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFam]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFam](
	[FamCod] [smallint] NOT NULL,
	[FamDesc] [varchar](35) NOT NULL,
 CONSTRAINT [PK_TbFam] PRIMARY KEY CLUSTERED 
(
	[FamCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFoglioIngr]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFoglioIngr](
	[EtixFoglio] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFormati]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFormati](
	[FrmId] [int] IDENTITY(1,1) NOT NULL,
	[FrmDesc] [varchar](30) NOT NULL,
 CONSTRAINT [PK_TbFormati] PRIMARY KEY CLUSTERED 
(
	[FrmId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFrasi]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFrasi](
	[FrId] [smallint] IDENTITY(1,1) NOT NULL,
	[FrDesc] [varchar](75) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFtCIG]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFtCIG](
	[FtRif] [int] NOT NULL,
	[FtOrdNum] [varchar](20) NOT NULL,
	[FtOrdData] [smalldatetime] NULL,
	[FtCIG] [varchar](20) NOT NULL,
	[FtCUP] [varchar](15) NOT NULL,
 CONSTRAINT [PK_TbFtCIG] PRIMARY KEY CLUSTERED 
(
	[FtRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFte_Eff]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFte_Eff](
	[RicProg] [int] IDENTITY(1,1) NOT NULL,
	[RicClie] [varchar](5) NOT NULL,
	[RicNfat] [int] NOT NULL,
	[RicImpFatt] [decimal](13, 2) NOT NULL,
	[RicAbi] [int] NOT NULL,
	[RicCab] [int] NOT NULL,
	[RicImpRata] [decimal](13, 2) NOT NULL,
	[RicTPag] [smallint] NOT NULL,
	[RicNRata] [smallint] NOT NULL,
	[RicDfat] [smalldatetime] NOT NULL,
	[RicDsca] [smalldatetime] NOT NULL,
	[RicRifFat] [int] NULL,
 CONSTRAINT [PK_TbEff] PRIMARY KEY CLUSTERED 
(
	[RicProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFte_Esterne]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFte_Esterne](
	[RifEsterne] [int] IDENTITY(20000000,1) NOT NULL,
	[DataOra] [datetime] NOT NULL,
 CONSTRAINT [PK_TbFte_Esterne] PRIMARY KEY CLUSTERED 
(
	[RifEsterne] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFte_Liste_Passive]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFte_Liste_Passive](
	[FteLID] [int] IDENTITY(1,1) NOT NULL,
	[FteLDataOra] [datetime] NOT NULL,
	[FteLLista] [int] NOT NULL,
	[FteLElab] [bit] NOT NULL,
 CONSTRAINT [PK_TbFte_Liste_Passive] PRIMARY KEY CLUSTERED 
(
	[FteLID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFte_Passiva]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFte_Passiva](
	[FteRif] [int] IDENTITY(1,1) NOT NULL,
	[FteIdSDI] [bigint] NOT NULL,
	[FteDataOra] [datetime] NOT NULL,
	[FteProgSIA] [int] NOT NULL,
	[FtePartIva] [varchar](20) NOT NULL,
	[FteRagSoc] [varchar](100) NOT NULL,
	[FteNomeFile] [varchar](50) NOT NULL,
	[FteAnno] [smallint] NOT NULL,
	[FteNumero] [varchar](20) NOT NULL,
	[FteData] [smalldatetime] NULL,
	[FteDataRicezione] [smalldatetime] NULL,
	[FteTotFat] [decimal](12, 2) NULL,
	[FteRifPri] [int] NOT NULL,
	[FtePrintM] [bit] NOT NULL,
	[FtePrintA] [bit] NOT NULL,
	[FteDataConsegnaSDI] [smalldatetime] NULL,
	[FteTotImp] [decimal](12, 2) NOT NULL,
	[FteTotIva] [decimal](12, 2) NOT NULL,
	[FteFinoAl] [smalldatetime] NULL,
	[FteIdBN] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbFte_Passiva] PRIMARY KEY CLUSTERED 
(
	[FteRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFteDDt]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFteDDt](
	[FteID] [int] IDENTITY(1,1) NOT NULL,
	[FteRif] [int] NOT NULL,
	[FteDDtNum] [varchar](20) NOT NULL,
	[FteDDTData] [smalldatetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFtEle]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFtEle](
	[FERif] [int] NOT NULL,
	[FEPath] [varchar](100) NOT NULL,
	[FEInviata] [bit] NOT NULL,
 CONSTRAINT [PK_TbFtEle] PRIMARY KEY CLUSTERED 
(
	[FERif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbGcau]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbGcau](
	[GId] [smallint] IDENTITY(1,1) NOT NULL,
	[GCaugestione] [varchar](15) NOT NULL,
	[GCauId] [smallint] NOT NULL,
	[GCauQuadri] [bit] NOT NULL,
	[GCauDefault] [bit] NOT NULL,
	[GCauRegistro] [smallint] NOT NULL,
 CONSTRAINT [PK_TbGcau] PRIMARY KEY CLUSTERED 
(
	[GId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbIntrCC]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbIntrCC](
	[CCCod] [varchar](5) NOT NULL,
	[CCDescri] [varchar](50) NOT NULL,
	[CCGruppo] [varchar](1) NOT NULL,
	[CCDefault] [bit] NOT NULL,
 CONSTRAINT [PK_TbIntrCC] PRIMARY KEY CLUSTERED 
(
	[CCCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbIntrMT]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbIntrMT](
	[MTCod] [smallint] NOT NULL,
	[MTDescri] [varchar](50) NOT NULL,
	[MTDefault] [bit] NOT NULL,
 CONSTRAINT [PK_TbIntrMT] PRIMARY KEY CLUSTERED 
(
	[MTCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbLingue]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbLingue](
	[LngCod] [smallint] NOT NULL,
	[LngDesc] [varchar](40) NOT NULL,
 CONSTRAINT [PK_TbLingue] PRIMARY KEY CLUSTERED 
(
	[LngCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbLisOrig]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbLisOrig](
	[LisId] [smallint] NOT NULL,
	[LisValiditaDal] [smalldatetime] NOT NULL,
	[LisPrezzo1] [decimal](7, 2) NOT NULL,
	[LisPrezzo2] [decimal](7, 2) NOT NULL,
	[LisPrezzo3] [decimal](7, 2) NOT NULL,
	[LisPrezzo4] [decimal](7, 2) NOT NULL,
	[LisPrezzo5] [decimal](7, 2) NOT NULL,
	[LisPrezzo6] [decimal](7, 2) NOT NULL,
	[LisPrezzo7] [decimal](7, 2) NOT NULL,
	[LisPrezzo8] [decimal](7, 2) NOT NULL,
	[LisPrezzo9] [decimal](7, 2) NOT NULL,
	[LisPrezzo10] [decimal](7, 2) NOT NULL,
	[LisPrezzo11] [decimal](7, 2) NOT NULL,
	[LisPrezzo12] [decimal](7, 2) NOT NULL,
	[LisPrezzo13] [decimal](7, 2) NOT NULL,
	[LisPrezzo14] [decimal](7, 2) NOT NULL,
	[LisPrezzo15] [decimal](7, 2) NOT NULL,
	[LisPrezzo16] [decimal](7, 2) NOT NULL,
	[LisPrezzo17] [decimal](7, 2) NOT NULL,
	[LisPrezzo18] [decimal](7, 2) NOT NULL,
	[LisPrezzo19] [decimal](7, 2) NOT NULL,
	[LisPrezzo20] [decimal](7, 2) NOT NULL,
	[LisDataGen] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_TbLisOrig] PRIMARY KEY CLUSTERED 
(
	[LisId] ASC,
	[LisValiditaDal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbLock]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbLock](
	[IdStampa] [tinyint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbMailBody]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbMailBody](
	[MailBodyId] [varchar](1) NOT NULL,
	[MailLingua] [varchar](1) NOT NULL,
	[MailOggetto] [varchar](100) NOT NULL,
	[MailInizioBody] [varchar](1000) NOT NULL,
	[MailFirma] [varchar](500) NOT NULL,
	[MailFrom] [varchar](100) NOT NULL,
 CONSTRAINT [PK_TbMailBody] PRIMARY KEY CLUSTERED 
(
	[MailBodyId] ASC,
	[MailLingua] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbMailDoc]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbMailDoc](
	[MailRif] [int] NOT NULL,
	[MailTipoDoc] [varchar](1) NOT NULL,
	[MailData] [smalldatetime] NULL,
 CONSTRAINT [PK_TbMailDoc] PRIMARY KEY CLUSTERED 
(
	[MailRif] ASC,
	[MailTipoDoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbMailPec]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbMailPec](
	[MPID] [int] IDENTITY(1,1) NOT NULL,
	[MPClie] [varchar](5) NOT NULL,
	[MPEMail] [varchar](50) NOT NULL,
	[MPPec] [bit] NOT NULL,
	[MPUte] [varchar](60) NOT NULL,
	[MPFatture] [bit] NOT NULL,
	[MPTipo] [varchar](2) NOT NULL,
	[MPBolle] [bit] NOT NULL,
	[MPPrevOrd] [bit] NOT NULL,
 CONSTRAINT [PK_TbMailPec] PRIMARY KEY CLUSTERED 
(
	[MPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbMenuGrup]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbMenuGrup](
	[MenuGrupId] [int] NOT NULL,
	[MenuGrupInd] [varchar](5) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbNazioni]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbNazioni](
	[NazID] [smallint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[NazDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbNazioni] PRIMARY KEY CLUSTERED 
(
	[NazID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbOrariStandard]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbOrariStandard](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GiornoSettimana] [smallint] NOT NULL,
	[Slot] [smallint] NOT NULL,
	[OraInizio] [time](7) NULL,
	[OraFine] [time](7) NULL,
	[IsChiuso] [bit] NOT NULL,
 CONSTRAINT [PK__OrariSta__3214EC2702846736] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPOS_Pagam]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPOS_Pagam](
	[PPagCod] [smallint] NOT NULL,
	[PPagDesc] [varchar](20) NOT NULL,
	[PPagPagCod] [smallint] NOT NULL,
	[PPagCPT] [varchar](5) NOT NULL,
 CONSTRAINT [PK_TbPOS_Pagam] PRIMARY KEY CLUSTERED 
(
	[PPagCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPrivati]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPrivati](
	[PrivId] [int] IDENTITY(1,1) NOT NULL,
	[PrivCognomeNome] [varchar](100) NOT NULL,
	[PrivTelefono] [varchar](20) NOT NULL,
	[PrivMail] [varchar](50) NOT NULL,
	[PrivIndirizzo] [varchar](50) NOT NULL,
	[PrivCap] [varchar](5) NOT NULL,
	[PrivCitta] [varchar](50) NOT NULL,
	[PrivProv] [varchar](2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbProforma]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbProforma](
	[PFatRif] [int] NOT NULL,
	[PFatNumero] [int] NOT NULL,
	[PFatData] [smalldatetime] NOT NULL,
	[PFatAnno] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbRor]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRor](
	[RorRif] [int] NOT NULL,
	[RorProgr] [int] NOT NULL,
	[RorArtId] [int] NOT NULL,
	[RorCodArt] [varchar](20) NOT NULL,
	[RorCodImballo] [smallint] NOT NULL,
	[RorQuaOrd] [decimal](7, 2) NOT NULL,
	[RorPrezzo] [decimal](7, 2) NOT NULL,
	[RorSc1] [decimal](5, 2) NOT NULL,
	[RorSc2] [decimal](5, 2) NOT NULL,
	[RorNetto] [decimal](7, 2) NOT NULL,
	[RorImporto] [decimal](9, 2) NOT NULL,
	[RorOfferta] [bit] NOT NULL,
	[RorExpNetto] [decimal](7, 2) NOT NULL,
	[RorQuaIOrd] [int] NOT NULL,
	[RorTipOrd] [varchar](1) NOT NULL,
	[RorScalaDispOggi] [bit] NOT NULL,
	[RorTerzoStep] [bit] NOT NULL,
	[RorTerzoStepScadenza] [smalldatetime] NULL,
	[RorTerzoStepSc] [decimal](5, 2) NOT NULL,
	[RorFuoriLotto] [bit] NOT NULL,
	[RorFoglio] [int] NOT NULL,
	[RorMessaInVendita] [smalldatetime] NULL,
	[RorScadenza] [smalldatetime] NULL,
	[RorNoPrenotazione] [bit] NOT NULL,
 CONSTRAINT [PK_TbRor] PRIMARY KEY CLUSTERED 
(
	[RorRif] ASC,
	[RorProgr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSCor]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSCor](
	[SCorTipoDoc] [varchar](1) NOT NULL,
	[SCorRif] [int] NOT NULL,
	[SCorProg] [int] NOT NULL,
	[SSosTipoDoc] [varchar](1) NOT NULL,
	[SSosRif] [int] NOT NULL,
	[SSosProg] [int] NOT NULL,
 CONSTRAINT [PK_TbSCor] PRIMARY KEY CLUSTERED 
(
	[SCorTipoDoc] ASC,
	[SCorRif] ASC,
	[SCorProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSlAtt]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSlAtt](
	[SlAtt] [smallint] NOT NULL,
	[SlArtCod] [varchar](20) NOT NULL,
	[SlAttPrezzo] [decimal](9, 3) NOT NULL,
	[SlUlt_modifica] [smalldatetime] NULL,
	[SlUtente] [varchar](20) NULL,
	[SlStampa] [bit] NOT NULL,
 CONSTRAINT [PK_TbSlAtt] PRIMARY KEY CLUSTERED 
(
	[SlAtt] ASC,
	[SlArtCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSlCli]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSlCli](
	[SlCli] [varchar](5) NOT NULL,
	[SlArtCod] [varchar](20) NOT NULL,
	[SlCliPrezzo] [decimal](9, 3) NOT NULL,
	[SlNo] [bit] NOT NULL,
	[SlUlt_modifica] [smalldatetime] NULL,
	[SlUtente] [varchar](20) NULL,
	[SlStampa] [bit] NOT NULL,
 CONSTRAINT [PK_TbSlCli] PRIMARY KEY CLUSTERED 
(
	[SlCli] ASC,
	[SlArtCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSosP]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSosP](
	[PTipoDoc] [varchar](1) NOT NULL,
	[PRif] [int] NOT NULL,
	[PProg] [int] NOT NULL,
	[PCProg] [int] NOT NULL,
	[PQta] [decimal](8, 3) NOT NULL,
	[PLotto] [varchar](20) NOT NULL,
	[PDataScad] [smalldatetime] NULL,
 CONSTRAINT [PK_TbSosP_1] PRIMARY KEY CLUSTERED 
(
	[PTipoDoc] ASC,
	[PRif] ASC,
	[PProg] ASC,
	[PCProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSosPronti]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSosPronti](
	[SosPrTipodoc] [varchar](1) NOT NULL,
	[SosPrRif] [int] NOT NULL,
	[DocPrTipoDoc] [varchar](1) NOT NULL,
	[DocPrRif] [int] NOT NULL,
	[SosPrDataOra] [datetime] NOT NULL,
	[SosPOperatore] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbSosPronti] PRIMARY KEY CLUSTERED 
(
	[SosPrTipodoc] ASC,
	[SosPrRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTip]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTip](
	[TipCategoria] [varchar](2) NOT NULL,
	[TipCod] [varchar](2) NOT NULL,
	[TipDesc] [varchar](25) NOT NULL,
 CONSTRAINT [PK_TbCat2] PRIMARY KEY CLUSTERED 
(
	[TipCategoria] ASC,
	[TipCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTipDoc]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTipDoc](
	[TdId] [smallint] IDENTITY(1,1) NOT NULL,
	[TdDesc] [varchar](30) NOT NULL,
	[TdTDoc] [varchar](2) NOT NULL,
	[TdQuadri] [bit] NOT NULL,
	[TdGestione] [varchar](15) NOT NULL,
	[TdRegistro] [smallint] NOT NULL,
 CONSTRAINT [PK_TdTipDoc] PRIMARY KEY CLUSTERED 
(
	[TdId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTor]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTor](
	[TorRif] [int] IDENTITY(1,1) NOT NULL,
	[TorNumero] [int] NOT NULL,
	[TorData] [smalldatetime] NOT NULL,
	[TorCliente] [varchar](5) NOT NULL,
	[TorStampato] [bit] NOT NULL,
	[TorInPreparazione] [bit] NOT NULL,
	[TorInBolla] [bit] NOT NULL,
	[TorNote] [ntext] NOT NULL,
	[TorBolRif] [int] NOT NULL,
	[TorConsegna] [smalldatetime] NULL,
	[TorTipoDoc] [varchar](1) NOT NULL,
	[TorWebRif] [int] NOT NULL,
	[TorEvaso] [bit] NULL,
	[TorTipOrd] [varchar](1) NOT NULL,
	[TorNoteSped] [ntext] NOT NULL,
	[TorNoLotti] [bit] NOT NULL,
	[TorLottoFresco] [bit] NOT NULL,
	[TorSpTrasp] [decimal](7, 2) NOT NULL,
	[TorValore] [decimal](11, 2) NOT NULL,
	[TorSpAcc] [bit] NOT NULL,
	[TorBloccato] [bit] NOT NULL,
	[TorProforma] [bit] NOT NULL,
	[TorPrenotato] [bit] NOT NULL,
	[TorOperatore] [varchar](50) NOT NULL,
	[TorDataStampa] [datetime] NULL,
	[TorDataChiusura] [datetime] NULL,
	[TorASaldo] [bit] NOT NULL,
 CONSTRAINT [PK_TbTor] PRIMARY KEY CLUSTERED 
(
	[TorRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbUMis]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbUMis](
	[UMisSigla] [varchar](3) NOT NULL,
	[UMisDec] [bit] NOT NULL,
	[UMisSiglaInt] [varchar](3) NOT NULL,
	[UMLivConv] [tinyint] NOT NULL,
 CONSTRAINT [PK_TbUMis] PRIMARY KEY CLUSTERED 
(
	[UMisSigla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbZone]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbZone](
	[ZoID] [smallint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ZoNazID] [smallint] NOT NULL,
	[ZoDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbZone] PRIMARY KEY CLUSTERED 
(
	[ZoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_ETICHETTE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_ETICHETTE](
	[TERM] [varchar](20) NOT NULL,
	[TESTO] [varbinary](max) NOT NULL,
	[EAN13] [varchar](13) NOT NULL,
	[ARTCOD] [varchar](20) NOT NULL,
	[LOTTO] [varchar](20) NOT NULL,
	[SCADENZA] [varchar](10) NOT NULL,
	[DESCRIZIONE] [varchar](100) NOT NULL,
	[BARCODE] [varchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_FTE_PASSIVA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_FTE_PASSIVA](
	[TERM] [varchar](30) NOT NULL,
	[NOMEFILE] [varchar](50) NOT NULL,
	[ARCHIVEID] [varchar](50) NOT NULL,
	[SDID] [bigint] NOT NULL,
	[CREATEDTIME] [datetime] NOT NULL,
	[RECEPTIONTIME] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_FTE_SELEZIONA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_FTE_SELEZIONA](
	[TERM] [varchar](30) NOT NULL,
	[FTERIF] [int] NOT NULL,
	[FTENOMEFILE] [varchar](20) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_IMBALLO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_IMBALLO](
	[TERM] [varchar](20) NOT NULL,
	[ARTCOD] [varchar](20) NOT NULL,
	[DESCRIZIONE] [varchar](100) NOT NULL,
	[NUMIMBALLO] [smallint] NOT NULL,
	[LOTTO] [varchar](20) NOT NULL,
	[SCADENZA] [varchar](10) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_LISTINI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_LISTINI](
	[GRUPPO] [smallint] NULL,
	[ArtId] [int] NOT NULL,
	[ArtCod] [varchar](20) NOT NULL,
	[ArtDesc] [varchar](100) NOT NULL,
	[ArtUmisura] [varchar](2) NOT NULL,
	[LISTINO] [decimal](10, 3) NOT NULL,
	[SlAttPrezzo] [decimal](9, 2) NOT NULL,
	[VALIDITA] [smalldatetime] NOT NULL,
	[ArtCodiva] [smallint] NOT NULL,
	[PERCIVA] [smallint] NULL,
	[ArtCat] [varchar](2) NOT NULL,
	[CatDesc] [varchar](30) NOT NULL,
	[ArtNumImballo] [decimal](5, 0) NOT NULL,
	[AttDesc] [varchar](25) NOT NULL,
	[CLIENTE] [varchar](5) NULL,
	[RAG_SOC] [varchar](90) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_LISTVENDITA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_LISTVENDITA](
	[TERM] [varchar](40) NOT NULL,
	[GRUPPO] [smallint] NOT NULL,
	[CATEGORIA] [varchar](2) NOT NULL,
	[CLIENTE] [varchar](5) NOT NULL,
	[ARTID] [int] NOT NULL,
	[CODICE] [varchar](20) NOT NULL,
	[DESCRIZIONE] [varchar](200) NOT NULL,
	[PREZZO] [decimal](11, 3) NOT NULL,
	[PRESENTE] [bit] NOT NULL,
	[NOLISTINO] [bit] NOT NULL,
	[SCONTOFAS] [decimal](5, 2) NOT NULL,
	[SCONTOCLI] [decimal](5, 2) NOT NULL,
	[CATDESC] [varchar](30) NOT NULL,
	[UMV] [varchar](3) NOT NULL,
	[PREZZO_BASE] [decimal](11, 3) NOT NULL,
	[PREZZO_IVA] [decimal](11, 2) NOT NULL,
	[PREZZO_BASE_IVA] [decimal](11, 2) NOT NULL,
	[ALIQ] [decimal](5, 2) NOT NULL,
	[ultima_modifica] [smalldatetime] NULL,
	[Utente] [varchar](20) NULL,
	[STAMPALIS] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_MODIFICA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_MODIFICA](
	[LISTINO] [smallint] NOT NULL,
	[TERM] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TMP_MODIFICA] PRIMARY KEY CLUSTERED 
(
	[LISTINO] ASC,
	[TERM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_NEWORD_TORTE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_NEWORD_TORTE](
	[TERM] [varchar](20) NOT NULL,
	[ORDTRIF] [int] NOT NULL,
	[ORDTNUM] [int] NOT NULL,
	[ORDTDATA] [smalldatetime] NOT NULL,
	[COGNOMENOME] [varchar](100) NOT NULL,
	[TELEFONO] [varchar](20) NOT NULL,
	[ORDTCONSEGNA] [smalldatetime] NOT NULL,
	[ORDTDATARITIRO] [datetime] NOT NULL,
	[ORDTPESO] [decimal](6, 3) NOT NULL,
	[ORDTQTA] [smallint] NOT NULL,
	[ORDTTORTA] [varchar](100) NOT NULL,
	[ORDTNOFRUTTA] [varchar](100) NOT NULL,
	[ORDTALLERGIE] [varchar](100) NOT NULL,
	[ORDTFRASE] [varchar](250) NOT NULL,
	[ORDTNOTE] [varchar](100) NOT NULL,
	[ORDTBASE] [varchar](150) NOT NULL,
	[ORDTBAGNA] [varchar](50) NOT NULL,
	[ORDTOPEVENTI] [varchar](150) NOT NULL,
	[ORDTFARCITURA] [varchar](150) NOT NULL,
	[ORDTBORDODEC] [varchar](150) NOT NULL,
	[ORDTDECSUP] [varchar](150) NOT NULL,
	[ORDTDETTAGLIO_HTML] [varchar](max) NOT NULL,
	[ORDTCANDELINE_HTML] [varchar](max) NOT NULL,
	[ORDTACCONTO] [decimal](11, 2) NOT NULL,
	[IMGFORMA] [varbinary](max) NULL,
	[IMGDECORAZIONE] [varbinary](max) NULL,
	[IMGIMMAGINE] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_ORD_PANETTONI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_ORD_PANETTONI](
	[TERM] [varchar](20) NOT NULL,
	[SOSRIF] [int] NOT NULL,
	[SOSNUM] [int] NOT NULL,
	[SOSDATA] [smalldatetime] NULL,
	[SOSCONSEGNA] [smalldatetime] NULL,
	[TIPO_CONSEGNA] [varchar](35) NOT NULL,
	[SOSCLICONS] [varchar](5) NOT NULL,
	[ANARAG] [varchar](90) NOT NULL,
	[CORPROG] [int] NOT NULL,
	[CORCODART] [varchar](20) NOT NULL,
	[QTA] [smallint] NOT NULL,
	[DESCRIZIONE] [varchar](200) NOT NULL,
	[PEZZATURA] [varchar](20) NOT NULL,
	[CONFEZIONE] [varchar](50) NOT NULL,
	[PRODOTTO] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_ORD_TORTE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_ORD_TORTE](
	[TERM] [varchar](20) NOT NULL,
	[SOSTIPODOC] [varchar](1) NOT NULL,
	[SOSRIF] [int] NOT NULL,
	[SOSNUM] [int] NOT NULL,
	[SOSDATA] [smalldatetime] NULL,
	[COGNOMENOME] [varchar](100) NOT NULL,
	[TELEFONO] [varchar](20) NOT NULL,
	[MAIL] [varchar](50) NULL,
	[SOSCONSEGNA] [smalldatetime] NULL,
	[SOSORARITIRO] [datetime] NULL,
	[CORANNOTA] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_PLU]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_PLU](
	[IDPLU] [smallint] NOT NULL,
	[TMPARTID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_SELEZIONATI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_SELEZIONATI](
	[TERM] [varchar](30) NOT NULL,
	[SORDRIF] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPANNO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPANNO](
	[TBLOCK] [int] NOT NULL,
	[TANNO] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPCAT]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPCAT](
	[TERM] [varchar](20) NOT NULL,
	[CATEGORIA] [varchar](2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TmpEtichette]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TmpEtichette](
	[TmpArtID] [int] NULL,
	[TmpArtCod] [varchar](20) NULL,
	[TmpArtDesc] [varchar](100) NULL,
	[TmpArtUmVen] [varchar](2) NULL,
	[TmpCatDesc] [varchar](30) NULL,
	[TmpArtCat] [varchar](2) NULL,
	[TmpArtNumImballo] [int] NULL,
	[TmpQta] [smallint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TmpRim]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TmpRim](
	[TmpRif] [int] NOT NULL,
	[TmpTerm] [varchar](20) NOT NULL,
	[TmpCliente] [varchar](5) NULL,
	[TmpCliCons] [varchar](5) NULL,
	[TmpDataFatt] [smalldatetime] NULL,
	[TmpDataBol] [smalldatetime] NULL,
	[TmpCodPag] [smallint] NULL,
	[TmpBolNrag] [int] NULL,
	[TmpTipo] [varchar](1) NULL,
 CONSTRAINT [PK_TmpRim] PRIMARY KEY CLUSTERED 
(
	[TmpRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TmpSCor]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TmpSCor](
	[CorTipoDoc] [varchar](1) NOT NULL,
	[CorRif] [int] NOT NULL,
	[CorProg] [int] NOT NULL,
	[CorArtID] [int] NOT NULL,
	[CorCodArt] [varchar](20) NOT NULL,
	[CorDesc] [varchar](50) NOT NULL,
	[CorQuaCon] [decimal](8, 2) NOT NULL,
	[CorPrezzo] [decimal](9, 2) NOT NULL,
	[CorSc1] [decimal](5, 3) NOT NULL,
	[CorSc2] [decimal](5, 3) NOT NULL,
	[CorSc3] [decimal](5, 3) NOT NULL,
	[CorSc4] [decimal](5, 3) NOT NULL,
	[CorSc5] [decimal](5, 3) NOT NULL,
	[CorNetto] [decimal](9, 2) NOT NULL,
	[CorImporto] [decimal](11, 2) NOT NULL,
	[CorUrgenza] [bit] NOT NULL,
	[CorSpot] [bit] NOT NULL,
	[CorPromo] [varchar](1) NOT NULL,
	[CorAssortito] [bit] NOT NULL,
	[CorVariato] [bit] NOT NULL,
	[CorCampagna] [varchar](10) NOT NULL,
	[CorCiva] [smallint] NOT NULL,
	[CorCntrp] [varchar](5) NOT NULL,
	[CorCau] [smallint] NOT NULL,
	[CorMacro] [varchar](5) NOT NULL,
	[CorOrdRif] [int] NOT NULL,
	[CorOrdine] [varchar](8) NOT NULL,
	[CorArtFor] [varchar](15) NOT NULL,
	[CorPlRif] [int] NOT NULL,
	[CorPlCassa] [int] NOT NULL,
	[CorImballo] [decimal](9, 2) NOT NULL,
	[CorMg1] [decimal](5, 3) NOT NULL,
	[CorMg2] [decimal](5, 3) NOT NULL,
	[CorMg3] [decimal](5, 3) NOT NULL,
	[CorMerc] [int] NOT NULL,
	[CorBrand] [int] NOT NULL,
	[CorOrdProg] [int] NULL,
	[CorUM] [varchar](2) NOT NULL,
	[CorLotto] [varchar](20) NULL,
	[CorDataScad] [smalldatetime] NULL,
	[CorAnnota] [varchar](max) NULL,
	[CorOmaggi] [varchar](1) NULL,
 CONSTRAINT [PK_TmpSCor] PRIMARY KEY CLUSTERED 
(
	[CorTipoDoc] ASC,
	[CorRif] ASC,
	[CorProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_OpzCat]    Script Date: 20/05/2026 14:31:16 ******/
CREATE NONCLUSTERED INDEX [IX_OpzCat] ON [dbo].[TbOpzioniTorte]
(
	[OpzCat] ASC,
	[OpzAttivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TbTes]    Script Date: 20/05/2026 14:31:16 ******/
CREATE NONCLUSTERED INDEX [IX_TbTes] ON [dbo].[TbTes]
(
	[TesData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ANACLI] ADD  CONSTRAINT [DF_ANACLI_PagCod]  DEFAULT ((0)) FOR [PagCod]
GO
ALTER TABLE [dbo].[ANACLI] ADD  CONSTRAINT [DF_ANACLI_XClCod]  DEFAULT ('') FOR [XClCod]
GO
ALTER TABLE [dbo].[COND_PAGAM] ADD  CONSTRAINT [DF_COND_PAGAM_CODICE]  DEFAULT ((0)) FOR [CODICE]
GO
ALTER TABLE [dbo].[ESTERE] ADD  CONSTRAINT [DF_ESTERE_sigla]  DEFAULT ('') FOR [sigla]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDesc]  DEFAULT ('') FOR [ArtDesc]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_aRTmINIdESC]  DEFAULT ('') FOR [ArtMinidesc]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtTipoProd]  DEFAULT ('') FOR [ArtTipoProd]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtCodMer]  DEFAULT ('') FOR [ArtCat]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtCat11]  DEFAULT ('') FOR [ArtTip]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtSiglaSconto]  DEFAULT ('') FOR [ArtSiglaSconto]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtUmisura]  DEFAULT ('') FOR [ArtUmisura]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtPesoCnf]  DEFAULT ((0)) FOR [ArtPesoCnf]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtNumImballo]  DEFAULT ((0)) FOR [ArtNumImballo]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtPeso]  DEFAULT ((0)) FOR [ArtPeso]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtCodiva]  DEFAULT ((0)) FOR [ArtCodiva]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtCptcon]  DEFAULT ('') FOR [ArtCptcon]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtFlagAt__21E0EDE6]  DEFAULT ((1)) FOR [ArtFlagAttivo]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtScadenza]  DEFAULT ((0)) FOR [ArtScadenza]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDescLis]  DEFAULT ('') FOR [ArtDescLis]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtNoteLis]  DEFAULT ('') FOR [ArtNoteLis]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDistinta]  DEFAULT ((0)) FOR [ArtDistinta]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtUmAcq]  DEFAULT ('') FOR [ArtUmVen]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtUmTec]  DEFAULT ((0)) FOR [ArtUmTec]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDiBase]  DEFAULT ('') FOR [ArtDiBase]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtCiAcq]  DEFAULT ((0)) FOR [ArtCiAcq]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtCptAcq]  DEFAULT ('') FOR [ArtCptAcq]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtMultimb]  DEFAULT ((0)) FOR [ArtMultimb]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDesc2]  DEFAULT ('') FOR [ArtDesc2]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDesc3]  DEFAULT ('') FOR [ArtDesc3]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDesc4]  DEFAULT ('') FOR [ArtDesc4]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDesc5]  DEFAULT ('') FOR [ArtDesc5]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtFamiglia]  DEFAULT ((0)) FOR [ArtFamiglia]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtAcquistato]  DEFAULT ((0)) FOR [ArtAcquistato]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtRipien__766C7FFC]  DEFAULT ((0)) FOR [ArtRipieno]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtTipo62__6F2063EF]  DEFAULT ((0)) FOR [ArtTipo62]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtEAN13__1DE63FD0]  DEFAULT ('') FOR [ArtEAN13]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtAttivo__1EDA6409]  DEFAULT ((0)) FOR [ArtAttivoStore]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtProduz__1FCE8842]  DEFAULT ((0)) FOR [ArtProduzStore]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtSfuso__20C2AC7B]  DEFAULT ((0)) FOR [ArtSfuso]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtImball__403B57D4]  DEFAULT ((0)) FOR [ArtImballoStore]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtPesoCn__412F7C0D]  DEFAULT ((0)) FOR [ArtPesoCnfStore]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtProduc__1022305E]  DEFAULT ('') FOR [ArtProducer]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtTest__351EAAE3]  DEFAULT ((0)) FOR [ArtTest]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtCatP__464936E5]  DEFAULT ('') FOR [ArtCatP]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtSL__3282355A]  DEFAULT ((0)) FOR [ArtSL]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtGruppo__33765993]  DEFAULT ((0)) FOR [ArtGruppo]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtPrzAcq__3EE80C3F]  DEFAULT ((0)) FOR [ArtPrzAcquisto]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtPrzVen__3FDC3078]  DEFAULT ((0)) FOR [ArtPrzVendita]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtSLC__0856F164]  DEFAULT ((0)) FOR [ArtSLC]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__Art_CAMPI__1B69C5D8]  DEFAULT ((0)) FOR [Art_CAMPIONATURA]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtXDesc__2F26A5C2]  DEFAULT ('') FOR [ArtXDesc]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtXDesc2__301AC9FB]  DEFAULT ('') FOR [ArtXDesc2]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtXDesc3__310EEE34]  DEFAULT ('') FOR [ArtXDesc3]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtXDesc4__3203126D]  DEFAULT ('') FOR [ArtXDesc4]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtXDesc5__32F736A6]  DEFAULT ('') FOR [ArtXDesc5]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtXFlag__33EB5ADF]  DEFAULT ((0)) FOR [ArtXFlag]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtFormat__7F428876]  DEFAULT ((0)) FOR [ArtFormato]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtDisp_E__24740D25]  DEFAULT ((0)) FOR [ArtDisp_Effettiva]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF__TbArt__ArtColore__76432E21]  DEFAULT ((0)) FOR [ArtColore]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtDbTipo]  DEFAULT ('') FOR [ArtDbTipo]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtImbKit]  DEFAULT ((1)) FOR [ArtImbKit]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtBarcode]  DEFAULT ('') FOR [ArtBarcode]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_PesoGr]  DEFAULT ((0)) FOR [PesoGr]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtBilancia]  DEFAULT ((0)) FOR [ArtBilancia]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtCodBil]  DEFAULT ((0)) FOR [ArtCodBil]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtBarcodeForn]  DEFAULT ('') FOR [ArtBarcodeForn]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtEtixFoglio]  DEFAULT ((0)) FOR [ArtEtixFoglio]
GO
ALTER TABLE [dbo].[TbArt] ADD  CONSTRAINT [DF_TbArt_ArtUltLotto]  DEFAULT ('') FOR [ArtUltLotto]
GO
ALTER TABLE [dbo].[TbAtt] ADD  CONSTRAINT [DF_TbAtt_AttAnaCli]  DEFAULT ((1)) FOR [AttAnaCli]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_AziPAREAprov]  DEFAULT ('') FOR [APReaProv]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_AziPAReaNum]  DEFAULT ('') FOR [APReaNum]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_AziPAreaCapSoc]  DEFAULT ('') FOR [APReaCapSoc]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_AziPaReaSocioUnico]  DEFAULT ((0)) FOR [APReaSocioUnico]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_APReaLiquidazione]  DEFAULT ((0)) FOR [APReaLiquidazione]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_APUltimoINVIO]  DEFAULT ((0)) FOR [APUltimoINVIO]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_APCartellaINVIO]  DEFAULT ('') FOR [APCartellaINVIO]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APCodFi__6B123DEE]  DEFAULT ('') FOR [APCodFiscTrasm]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_APCodFisc3Int]  DEFAULT ('') FOR [APCodFisc3Int]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_APRagSoc3Int]  DEFAULT ('') FOR [APRagSoc3Int]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF_TbAziPA_APAttiva]  DEFAULT ((0)) FOR [APAttiva]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APAlleg__744F2D60]  DEFAULT ((0)) FOR [APAllegoPDF]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APUser__10EB6C0E]  DEFAULT ('') FOR [APUser]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APPassw__11DF9047]  DEFAULT ('REDACTED_DEFAULT_PASSWORD') FOR [APPassword]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APPosta__12D3B480]  DEFAULT ('') FOR [APPostazione]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APIstit__13C7D8B9]  DEFAULT ('') FOR [APIstituto]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APCodic__14BBFCF2]  DEFAULT ('') FOR [APCodiceAzienda]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APPorta__15B0212B]  DEFAULT ((0)) FOR [APPortaleTest]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  CONSTRAINT [DF__TbAziPA__APGGInd__6F4B4C78]  DEFAULT ((5)) FOR [APGGIndietro]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  DEFAULT ((0)) FOR [APRegEsterne]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  DEFAULT ((0)) FOR [APBluenext]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  DEFAULT ('') FOR [APBnUser]
GO
ALTER TABLE [dbo].[TbAziPA] ADD  DEFAULT ('') FOR [APBnPassword]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolPrefAgg]  DEFAULT ((0)) FOR [BolPrefAgg]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolPorto]  DEFAULT ('') FOR [BolPorto]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolVett1]  DEFAULT ((0)) FOR [BolVett1]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolVett2]  DEFAULT ((0)) FOR [BolVett2]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolNColli]  DEFAULT ((0)) FOR [BolNColli]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolNVolumi]  DEFAULT ((0)) FOR [BolNVolumi]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolDMV]  DEFAULT ('') FOR [BolDMV]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF__TbBol__BolAspBen__3E131840]  DEFAULT ('') FOR [BolAspBeni]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF__TbBol__BolVarDes__3F073C79]  DEFAULT ('') FOR [BolVarDest]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF__TbBol__BolAnnota__1F247CCC]  DEFAULT ('') FOR [BolAnnota]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolNDep]  DEFAULT ((0)) FOR [BolNDep]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolTotale]  DEFAULT ((0)) FOR [BolTotale]
GO
ALTER TABLE [dbo].[TbBol] ADD  DEFAULT ('') FOR [BolNote]
GO
ALTER TABLE [dbo].[TbBol] ADD  DEFAULT ('') FOR [BolTipoFte]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolPvv]  DEFAULT ('') FOR [BolPvv]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolPesoNetto]  DEFAULT ((0)) FOR [BolPesoNetto]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolPesoLordo]  DEFAULT ((0)) FOR [BolPesoLordo]
GO
ALTER TABLE [dbo].[TbBol] ADD  CONSTRAINT [DF_TbBol_BolListino]  DEFAULT ((0)) FOR [BolListino]
GO
ALTER TABLE [dbo].[TbCalendarioIso] ADD  CONSTRAINT [DF_TbCalendarioIso_MONANNO]  DEFAULT ((0)) FOR [MONANNO]
GO
ALTER TABLE [dbo].[TbCalendarioIso] ADD  CONSTRAINT [DF_TbCalendarioIso_MONSETT]  DEFAULT ((0)) FOR [MONSETT]
GO
ALTER TABLE [dbo].[TbCat] ADD  CONSTRAINT [DF_TbCat1_Cat1Desc]  DEFAULT ('') FOR [CatDesc]
GO
ALTER TABLE [dbo].[TbCat] ADD  CONSTRAINT [DF_TbCat_CatStat]  DEFAULT ((0)) FOR [CatStat]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClPagDesc]  DEFAULT ('') FOR [ClPagDesc]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClCodAge]  DEFAULT ('') FOR [ClCodAge]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClCodMan]  DEFAULT ('') FOR [ClCodMan]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClDirezione]  DEFAULT ((0)) FOR [ClDirezione]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClAttivo]  DEFAULT ((1)) FOR [ClAttivo]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClAddSpese]  DEFAULT ((0)) FOR [ClEseSpe]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClCodBan]  DEFAULT ((1)) FOR [ClCodBan]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClInviaMail]  DEFAULT ((0)) FOR [ClInviaMail]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClMaxScop]  DEFAULT ((0.00)) FOR [ClMaxScop]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClNazione]  DEFAULT ('IT') FOR [ClNazione]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClSc1]  DEFAULT ((0.00)) FOR [ClSc1]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClSc2]  DEFAULT ((0.00)) FOR [ClSc2]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClPortoFranco]  DEFAULT ((0)) FOR [ClPortoFranco]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClGrCanale]  DEFAULT ((0)) FOR [ClGrCanale]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClBlackList]  DEFAULT ((0)) FOR [ClBlackList]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF__TbCli__ClStato__7F179FED]  DEFAULT ((0)) FOR [ClStato]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClSculter]  DEFAULT ((0.00)) FOR [ClSculter]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClMailAmministra]  DEFAULT ('') FOR [ClMailAmministra]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClMailComunica]  DEFAULT ('') FOR [ClMailComunica]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClClasseFattura]  DEFAULT ('') FOR [ClClasseFattura]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClOrario]  DEFAULT ('') FOR [ClOrario]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClBolRag]  DEFAULT ((1)) FOR [ClBolRag]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClEspSconti]  DEFAULT ('') FOR [ClEspSconti]
GO
ALTER TABLE [dbo].[TbCli] ADD  CONSTRAINT [DF_TbCli_ClSplitPay]  DEFAULT ((0)) FOR [ClSplitPay]
GO
ALTER TABLE [dbo].[TbColori] ADD  CONSTRAINT [DF_TbColori_ColDesc]  DEFAULT ('') FOR [ColDesc]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorSc4]  DEFAULT ((0)) FOR [CorSc4]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorSc5]  DEFAULT ((0)) FOR [CorSc5]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorPlRif]  DEFAULT ((0)) FOR [CorPlRif]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorPlCassa]  DEFAULT ((0)) FOR [CorPlCassa]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorImballo]  DEFAULT ((0)) FOR [CorImballo]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorMg1]  DEFAULT ((0)) FOR [CorMg1]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorMg2]  DEFAULT ((0)) FOR [CorMg2]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_Corg3]  DEFAULT ((0)) FOR [CorMg3]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorMerc]  DEFAULT ((0)) FOR [CorMerc]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorBrand]  DEFAULT ((0)) FOR [CorBrand]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorOrdProg]  DEFAULT ((0)) FOR [CorOrdProg]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorUM]  DEFAULT ('') FOR [CorUM]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorLotto]  DEFAULT ('') FOR [CorLotto]
GO
ALTER TABLE [dbo].[TbCor] ADD  CONSTRAINT [DF_TbCor_CorOmaggio]  DEFAULT ('') FOR [CorOmaggi]
GO
ALTER TABLE [dbo].[TbDFA] ADD  CONSTRAINT [DF_TbDFA_DFatDataOra]  DEFAULT (getdate()) FOR [DFatDataOra]
GO
ALTER TABLE [dbo].[TbDogana] ADD  CONSTRAINT [DF_TbDogana_DGTipo]  DEFAULT ('P') FOR [DGTipo]
GO
ALTER TABLE [dbo].[TbFam] ADD  CONSTRAINT [DF_TbFam_FamDesc]  DEFAULT ('') FOR [FamDesc]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatPorto]  DEFAULT ('') FOR [FatPorto]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatVett1]  DEFAULT ((0)) FOR [FatVett1]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatVett2]  DEFAULT ((0)) FOR [FatVett2]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatNcolli]  DEFAULT ((0)) FOR [FatNcolli]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatNVolumi]  DEFAULT ((0)) FOR [FatNVolumi]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatDMV]  DEFAULT ('') FOR [FatDMV]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatDiff]  DEFAULT ('') FOR [FatDiff]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF__TbFat__FatAspBen__238A0A8D]  DEFAULT ('') FOR [FatAspBeni]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF__TbFat__FatVarDes__247E2EC6]  DEFAULT ('') FOR [FatVarDest]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF__TbFat__FatAnnota__257252FF]  DEFAULT ('') FOR [FatAnnota]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatNDep]  DEFAULT ((0)) FOR [FatNDep]
GO
ALTER TABLE [dbo].[TbFat] ADD  DEFAULT ('') FOR [FatNote]
GO
ALTER TABLE [dbo].[TbFat] ADD  DEFAULT ('') FOR [FatTipoFte]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatPvv]  DEFAULT ('') FOR [FatPvv]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatPesoNetto]  DEFAULT ((0)) FOR [FatPesoNetto]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatPesoLordo]  DEFAULT ((0)) FOR [FatPesoLordo]
GO
ALTER TABLE [dbo].[TbFat] ADD  CONSTRAINT [DF_TbFat_FatListino]  DEFAULT ((0)) FOR [FatListino]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoPagam]  DEFAULT ((0)) FOR [FoPagam]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoPagDesc]  DEFAULT ('') FOR [FoPagDesc]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoAbi]  DEFAULT ((0)) FOR [FoAbi]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoCab]  DEFAULT ((0)) FOR [FoCab]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoCC]  DEFAULT ('') FOR [FoCC]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoCinEur]  DEFAULT ('') FOR [FoCinEur]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoCin]  DEFAULT ('') FOR [FoCin]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoNba]  DEFAULT ((0)) FOR [FoCodBan]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoSoggRit]  DEFAULT ((0)) FOR [FoSoggRit]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoAttivo]  DEFAULT ((1)) FOR [FoAttivo]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoBlackList]  DEFAULT ((0)) FOR [FoBlackList]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoSbloccoFat]  DEFAULT ((1)) FOR [FoSbloccoFat]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoGruppoAt]  DEFAULT ((0)) FOR [FoGruppoAt]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoNazione]  DEFAULT ('IT') FOR [FoNazione]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoEnasarco]  DEFAULT ((0)) FOR [FoEnasarco]
GO
ALTER TABLE [dbo].[TbFor] ADD  CONSTRAINT [DF_TbFor_FoMatGrezzo]  DEFAULT ((0)) FOR [FoMatGrezzo]
GO
ALTER TABLE [dbo].[TbFormati] ADD  CONSTRAINT [DF_TbFormati_FrmDesc]  DEFAULT ('') FOR [FrmDesc]
GO
ALTER TABLE [dbo].[TbFrasi] ADD  CONSTRAINT [DF_TbFrasi_FrDesc]  DEFAULT ('') FOR [FrDesc]
GO
ALTER TABLE [dbo].[TbFtCIG] ADD  DEFAULT ('') FOR [FtCUP]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteDDTNum]  DEFAULT ('') FOR [FteDDTNum]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteOrdNum]  DEFAULT ('') FOR [FteOrdNum]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteCIG]  DEFAULT ('') FOR [FteCIG]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteModPag]  DEFAULT ('') FOR [FteModPag]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteNatura]  DEFAULT ('') FOR [FteNatura]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteFileInvio]  DEFAULT ((0)) FOR [FteProgrInvio]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteErr_Cod]  DEFAULT ((0)) FOR [FteErr_Cod]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteErr_desc]  DEFAULT ('') FOR [FteErr_desc]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteDaInviare]  DEFAULT ((1)) FOR [FteDaInviare]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteIdElab]  DEFAULT ((0)) FOR [FteIdElab]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteElaborataSia]  DEFAULT ((0)) FOR [FteElaborataSia]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteProgSIA]  DEFAULT ((0)) FOR [FteProgSIA]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteStato]  DEFAULT ('') FOR [FteStato]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteNomeSDI]  DEFAULT ('') FOR [FteNomeSDI]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteNomeFile]  DEFAULT ('') FOR [FteNomeFile]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteKBFile]  DEFAULT ((0)) FOR [FteKBFile]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteAnno]  DEFAULT ((0)) FOR [FteAnno]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteNumero]  DEFAULT ((0)) FOR [FteNumero]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteRegistro]  DEFAULT ((0)) FOR [FteRegistro]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF_TbFte_FteAlfanum]  DEFAULT ('') FOR [FteAlfanum]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteStatoA__7F81B441]  DEFAULT ((0)) FOR [FteStatoAttive]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteUltOp__0075D87A]  DEFAULT ('') FOR [FteUltOp]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteCodSta__0169FCB3]  DEFAULT ((0)) FOR [FteCodStato]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteStatoD__025E20EC]  DEFAULT ((0)) FOR [FteStatoDaPrenotare]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteStatoD__03524525]  DEFAULT ((0)) FOR [FteStatoDaRecuperare]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteIdCons__0446695E]  DEFAULT ((0)) FOR [FteIdConservazione]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteNote__053A8D97]  DEFAULT ('') FOR [FteNote]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteClient__090B1E7B]  DEFAULT ('') FOR [FteCliente]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteRagSoc__09FF42B4]  DEFAULT ('') FOR [FteRagSoc]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteTotFat__0AF366ED]  DEFAULT ((0)) FOR [FteTotFat]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteCUP__18A267C6]  DEFAULT ('') FOR [FteCUP]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteBlueNe__67C004A0]  DEFAULT ((0)) FOR [FteBlueNext]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FtePA__68B428D9]  DEFAULT ((0)) FOR [FtePA]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteIdBN__69A84D12]  DEFAULT ('') FOR [FteIdBN]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteIdSDI__6A9C714B]  DEFAULT ('') FOR [FteIdSDI]
GO
ALTER TABLE [dbo].[TbFte] ADD  CONSTRAINT [DF__TbFte__FteNomeFi__6B909584]  DEFAULT ('') FOR [FteNomeFileBN]
GO
ALTER TABLE [dbo].[TbFte_Liste_Passive] ADD  CONSTRAINT [DF_TbFte_Liste_Passive_FteLDataOra]  DEFAULT (getdate()) FOR [FteLDataOra]
GO
ALTER TABLE [dbo].[TbFte_Liste_Passive] ADD  CONSTRAINT [DF_TbFte_Liste_Passive_FteLLista]  DEFAULT ((0)) FOR [FteLLista]
GO
ALTER TABLE [dbo].[TbFte_Liste_Passive] ADD  CONSTRAINT [DF_TbFte_Liste_Passive_FteLElab]  DEFAULT ((0)) FOR [FteLElab]
GO
ALTER TABLE [dbo].[TbFte_Passiva] ADD  CONSTRAINT [DF_TbFte_Passiva_FteDataOra]  DEFAULT (getdate()) FOR [FteDataOra]
GO
ALTER TABLE [dbo].[TbFte_Passiva] ADD  DEFAULT ((0)) FOR [FteRifPri]
GO
ALTER TABLE [dbo].[TbFte_Passiva] ADD  DEFAULT ((0)) FOR [FtePrintM]
GO
ALTER TABLE [dbo].[TbFte_Passiva] ADD  DEFAULT ((0)) FOR [FtePrintA]
GO
ALTER TABLE [dbo].[TbFte_Passiva] ADD  CONSTRAINT [DF_TbFte_Passiva_FteTotImp]  DEFAULT ((0)) FOR [FteTotImp]
GO
ALTER TABLE [dbo].[TbFte_Passiva] ADD  CONSTRAINT [DF_TbFte_Passiva_FteTotIva]  DEFAULT ((0)) FOR [FteTotIva]
GO
ALTER TABLE [dbo].[TbFte_Passiva] ADD  DEFAULT ('') FOR [FteIdBN]
GO
ALTER TABLE [dbo].[TbFteCli] ADD  CONSTRAINT [DF_TbFteCli_ClFteDestinatario]  DEFAULT ('') FOR [ClFteDestinatario]
GO
ALTER TABLE [dbo].[TbFteCli] ADD  CONSTRAINT [DF_TbFteCli_ClFtePec]  DEFAULT ('') FOR [ClFtePec]
GO
ALTER TABLE [dbo].[TbFtEle] ADD  CONSTRAINT [DF_TbFtEle_FEInviata]  DEFAULT ((0)) FOR [FEInviata]
GO
ALTER TABLE [dbo].[TbGcau] ADD  CONSTRAINT [DF_TbGcau_GCauDefault]  DEFAULT ((0)) FOR [GCauDefault]
GO
ALTER TABLE [dbo].[TbGcau] ADD  CONSTRAINT [DF_TbGcau_GCauRegistro]  DEFAULT ((0)) FOR [GCauRegistro]
GO
ALTER TABLE [dbo].[TbIntrCC] ADD  CONSTRAINT [DF_TbIntrCC_CCDescri]  DEFAULT ('') FOR [CCDescri]
GO
ALTER TABLE [dbo].[TbIntrCC] ADD  CONSTRAINT [DF_TbIntrCC_CCGruppo]  DEFAULT ('') FOR [CCGruppo]
GO
ALTER TABLE [dbo].[TbIntrCC] ADD  CONSTRAINT [DF_TbIntrCC_CCDefault]  DEFAULT ((0)) FOR [CCDefault]
GO
ALTER TABLE [dbo].[TbIntrMT] ADD  CONSTRAINT [DF_TbIntrMT_MTDescri]  DEFAULT ('') FOR [MTDescri]
GO
ALTER TABLE [dbo].[TbIntrMT] ADD  CONSTRAINT [DF_TbIntrMT_MTDefault]  DEFAULT ((0)) FOR [MTDefault]
GO
ALTER TABLE [dbo].[TbLis] ADD  CONSTRAINT [DF_TbLis_LisVendita]  DEFAULT ((0)) FOR [LisVendita]
GO
ALTER TABLE [dbo].[TbLisOrig] ADD  CONSTRAINT [DF_TbLisOrig_LisDataGen]  DEFAULT (getdate()) FOR [LisDataGen]
GO
ALTER TABLE [dbo].[TbLock] ADD  CONSTRAINT [DF_TbLock_IdStampa]  DEFAULT ((0)) FOR [IdStampa]
GO
ALTER TABLE [dbo].[TbMailBody] ADD  CONSTRAINT [DF_TbMailBody_MailBodyId]  DEFAULT ('0') FOR [MailBodyId]
GO
ALTER TABLE [dbo].[TbMailBody] ADD  CONSTRAINT [DF_TbMailBody_MailLingua]  DEFAULT ((0)) FOR [MailLingua]
GO
ALTER TABLE [dbo].[TbMailBody] ADD  CONSTRAINT [DF_TbMailBody_MailOggetto]  DEFAULT ('') FOR [MailOggetto]
GO
ALTER TABLE [dbo].[TbMailBody] ADD  CONSTRAINT [DF_TbMailBody_MailInizioBody]  DEFAULT ('') FOR [MailInizioBody]
GO
ALTER TABLE [dbo].[TbMailBody] ADD  CONSTRAINT [DF_TbMailBody_MailFirma]  DEFAULT ('') FOR [MailFirma]
GO
ALTER TABLE [dbo].[TbMailBody] ADD  CONSTRAINT [DF_TbMailBody_MailFrom]  DEFAULT ('') FOR [MailFrom]
GO
ALTER TABLE [dbo].[TbMailPec] ADD  CONSTRAINT [DF_TbMailPec_MPFatture]  DEFAULT ((0)) FOR [MPFatture]
GO
ALTER TABLE [dbo].[TbMailPec] ADD  CONSTRAINT [DF_TbMailPec_MPTipo]  DEFAULT ('') FOR [MPTipo]
GO
ALTER TABLE [dbo].[TbMailPec] ADD  CONSTRAINT [DF_TbMailPec_MPBolle]  DEFAULT ((0)) FOR [MPBolle]
GO
ALTER TABLE [dbo].[TbMailPec] ADD  CONSTRAINT [DF_TbMailPec_MPPrevOrd]  DEFAULT ((1)) FOR [MPPrevOrd]
GO
ALTER TABLE [dbo].[TbNewFasAtt] ADD  CONSTRAINT [DF_TbNewFasAtt_FasSc1]  DEFAULT ((0)) FOR [FasSc1]
GO
ALTER TABLE [dbo].[TbNewFasAtt] ADD  CONSTRAINT [DF_TbNewFasAtt_FasSc2]  DEFAULT ((0)) FOR [FasSc2]
GO
ALTER TABLE [dbo].[TbNewFasAtt] ADD  CONSTRAINT [DF_TbNewFasAtt_FasSc3]  DEFAULT ((0)) FOR [FasSc3]
GO
ALTER TABLE [dbo].[TbNewFasAtt] ADD  CONSTRAINT [DF_TbNewFasAtt_FasMg1]  DEFAULT ((0)) FOR [FasMg1]
GO
ALTER TABLE [dbo].[TbNewFasAtt] ADD  CONSTRAINT [DF_TbNewFasAtt_FasMg2]  DEFAULT ((0)) FOR [FasMg2]
GO
ALTER TABLE [dbo].[TbNewFasAtt] ADD  CONSTRAINT [DF_TbNewFasAtt_FasMg3]  DEFAULT ((0)) FOR [FasMg3]
GO
ALTER TABLE [dbo].[TbNewFasAtt] ADD  CONSTRAINT [DF_TbNewFasAtt_FasImb]  DEFAULT ((0)) FOR [FasImb]
GO
ALTER TABLE [dbo].[TbNewFasCli] ADD  CONSTRAINT [DF_TbNewFasCli_FasSc2]  DEFAULT ((0)) FOR [FasSc2]
GO
ALTER TABLE [dbo].[TbNewFasCli] ADD  CONSTRAINT [DF_TbNewFasCli_FasSc3]  DEFAULT ((0)) FOR [FasSc3]
GO
ALTER TABLE [dbo].[TbNewFasCli] ADD  CONSTRAINT [DF_TbNewFasCli_FasMg1]  DEFAULT ((0)) FOR [FasMg1]
GO
ALTER TABLE [dbo].[TbNewFasCli] ADD  CONSTRAINT [DF_TbNewFasCli_FasMg2]  DEFAULT ((0)) FOR [FasMg2]
GO
ALTER TABLE [dbo].[TbNewFasCli] ADD  CONSTRAINT [DF_TbNewFasCli_FasMg3]  DEFAULT ((0)) FOR [FasMg3]
GO
ALTER TABLE [dbo].[TbNewFasCli] ADD  CONSTRAINT [DF_TbNewFasCli_FasImb]  DEFAULT ((0)) FOR [FasImb]
GO
ALTER TABLE [dbo].[TbOpzioniTorte] ADD  CONSTRAINT [DF_TbOpzioniTorte_OpzAttivo]  DEFAULT ((1)) FOR [OpzAttivo]
GO
ALTER TABLE [dbo].[TbOpzioniTorte] ADD  CONSTRAINT [DF_TbOpzioniTorte_OpzIngredienti]  DEFAULT ('') FOR [OpzIngredienti]
GO
ALTER TABLE [dbo].[TbOpzioniTorte] ADD  CONSTRAINT [DF_TbOpzioniTorte_OpzIcona]  DEFAULT ((0)) FOR [OpzIcona]
GO
ALTER TABLE [dbo].[TbOpzioniTorte] ADD  CONSTRAINT [DF_TbOpzioniTorte_OpzBagna]  DEFAULT ((0)) FOR [OpzBagna]
GO
ALTER TABLE [dbo].[TbOrariStandard] ADD  CONSTRAINT [DF__OrariStan__IsChi__00CC74E3]  DEFAULT ((0)) FOR [IsChiuso]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_Table_1_OrdTCliente]  DEFAULT ('') FOR [OrdtCliente]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtQta]  DEFAULT ((1)) FOR [OrdtQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtTorta]  DEFAULT ((0)) FOR [OrdtTorta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtBase]  DEFAULT ((0)) FOR [OrdtBase]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtPersBase]  DEFAULT ('') FOR [OrdtPersBase]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtBagna]  DEFAULT ((0)) FOR [OrdtBagna]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtOpEventi]  DEFAULT ((0)) FOR [OrdtOpEventi]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtPersOpEventi]  DEFAULT ('') FOR [OrdtPersOpEventi]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtFarcitura]  DEFAULT ((0)) FOR [OrdtFarcitura]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtFarcitura2]  DEFAULT ((0)) FOR [OrdtFarcitura2]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtFarcitura3]  DEFAULT ((0)) FOR [OrdtFarcitura3]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtPersFarcitura]  DEFAULT ('') FOR [OrdtPersFarcitura]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtDecoraz]  DEFAULT ((0)) FOR [OrdtBordoDec]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtPersdecoraz]  DEFAULT ('') FOR [OrdtPersBordoDec]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtDecorazSuperficiale]  DEFAULT ((0)) FOR [OrdtDecSuperficiale]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtPersDecSuperficiale]  DEFAULT ('') FOR [OrdtPersDecSuperficiale]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtUltStrato]  DEFAULT ((0)) FOR [OrdtUltStrato]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtPersUltStrato]  DEFAULT ('') FOR [OrdtPersUltStrato]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtNote]  DEFAULT ('') FOR [OrdtNote]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtFruttaNo]  DEFAULT ('') FOR [OrdtNoFrutta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtCanaleFoto]  DEFAULT ('') FOR [OrdtCanaleFoto]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtUnoQta]  DEFAULT ((0)) FOR [OrdtUnoQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtUnoColore]  DEFAULT ((0)) FOR [OrdtUnoColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtDueQta]  DEFAULT ((0)) FOR [OrdtDueQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtDueColore]  DEFAULT ((0)) FOR [OrdtDueColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtTreQta]  DEFAULT ((0)) FOR [OrdtTreQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtTreColore]  DEFAULT ((0)) FOR [OrdtTreColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtQuattroQta]  DEFAULT ((0)) FOR [OrdtQuattroQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtQuattroColore]  DEFAULT ((0)) FOR [OrdtQuattroColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtCinqueQta]  DEFAULT ((0)) FOR [OrdtCinqueQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtCinqueColore]  DEFAULT ((0)) FOR [OrdtCinqueColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtSeiQta]  DEFAULT ((0)) FOR [OrdtSeiQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtSeiColore]  DEFAULT ((0)) FOR [OrdtSeiColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtSetteQta]  DEFAULT ((0)) FOR [OrdtSetteQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtSetteColore]  DEFAULT ((0)) FOR [OrdtSetteColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtOttoQta]  DEFAULT ((0)) FOR [OrdtOttoQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtOttoColore]  DEFAULT ((0)) FOR [OrdtOttoColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtNoveQta]  DEFAULT ((0)) FOR [OrdtNoveQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtNoveColore]  DEFAULT ((0)) FOR [OrdtNoveColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtZeroQta]  DEFAULT ((0)) FOR [OrdtZeroQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtZeroColore]  DEFAULT ((0)) FOR [OrdtZeroColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtCandelinaQta]  DEFAULT ((0)) FOR [OrdtCandelinaQta]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtCandelinaColore]  DEFAULT ('') FOR [OrdtCandelinaColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtNumeroTipo]  DEFAULT ((0)) FOR [OrdtNumeroTipo]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtNumeroColore]  DEFAULT ((0)) FOR [OrdtNumeroColore]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtAcconto]  DEFAULT ((0)) FOR [OrdtAcconto]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtStampata]  DEFAULT ((0)) FOR [OrdtStampato]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtStampaNum]  DEFAULT ((0)) FOR [OrdtStampaNum]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtMailInviata]  DEFAULT ((0)) FOR [OrdtMailInviata]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtTipoAccessori]  DEFAULT ((0)) FOR [OrdtTipoAccessori]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtDescAccessori]  DEFAULT ('') FOR [OrdtDescAccessori]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdTStato]  DEFAULT ((0)) FOR [OrdTStato]
GO
ALTER TABLE [dbo].[TbOrdTorte] ADD  CONSTRAINT [DF_TbOrdTorte_OrdtTipoTorta]  DEFAULT ('') FOR [OrdtTipoTorta]
GO
ALTER TABLE [dbo].[TbPOS_Pagam] ADD  CONSTRAINT [DF_TbPOS_Pagam_PPagPagCod]  DEFAULT ((0)) FOR [PPagPagCod]
GO
ALTER TABLE [dbo].[TbPOS_Pagam] ADD  CONSTRAINT [DF_TbPOS_Pagam_PPagCPT]  DEFAULT ('') FOR [PPagCPT]
GO
ALTER TABLE [dbo].[TbPrivati] ADD  CONSTRAINT [DF_TbPrivati_PrivTelefono]  DEFAULT ('') FOR [PrivTelefono]
GO
ALTER TABLE [dbo].[TbPrivati] ADD  CONSTRAINT [DF_TbPrivati_PrivMail]  DEFAULT ('') FOR [PrivMail]
GO
ALTER TABLE [dbo].[TbPrivati] ADD  CONSTRAINT [DF_TbPrivati_PrivIndirizzo]  DEFAULT ('') FOR [PrivIndirizzo]
GO
ALTER TABLE [dbo].[TbPrivati] ADD  CONSTRAINT [DF_TbPrivati_PrivCap]  DEFAULT ('') FOR [PrivCap]
GO
ALTER TABLE [dbo].[TbPrivati] ADD  CONSTRAINT [DF_TbPrivati_PrivCitta]  DEFAULT ('') FOR [PrivCitta]
GO
ALTER TABLE [dbo].[TbPrivati] ADD  CONSTRAINT [DF_TbPrivati_PrivProv]  DEFAULT ('') FOR [PrivProv]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorCodImballo]  DEFAULT ((0)) FOR [RorCodImballo]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorQuaOrd]  DEFAULT ((0)) FOR [RorQuaOrd]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorPrezzo]  DEFAULT ((0)) FOR [RorPrezzo]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorSc1]  DEFAULT ((0)) FOR [RorSc1]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorSc2]  DEFAULT ((0)) FOR [RorSc2]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorNetto]  DEFAULT ((0)) FOR [RorNetto]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorImporto]  DEFAULT ((0)) FOR [RorImporto]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorOfferta]  DEFAULT ((0)) FOR [RorOfferta]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF__TbRor__RorExpNet__143CDA05]  DEFAULT ((0)) FOR [RorExpNetto]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF__TbRor__RorQuaIOr__63B99E3B]  DEFAULT ((0)) FOR [RorQuaIOrd]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF__TbRor__RorTipOrd__2BD537CB]  DEFAULT ('') FOR [RorTipOrd]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorScalaDispOggi]  DEFAULT ((0)) FOR [RorScalaDispOggi]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorTerzoStep]  DEFAULT ((0)) FOR [RorTerzoStep]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorTerzoStepSc]  DEFAULT ((0)) FOR [RorTerzoStepSc]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorFuoriLotto]  DEFAULT ((0)) FOR [RorFuoriLotto]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorFoglio]  DEFAULT ((0)) FOR [RorFoglio]
GO
ALTER TABLE [dbo].[TbRor] ADD  CONSTRAINT [DF_TbRor_RorNonPrenotato]  DEFAULT ((0)) FOR [RorNoPrenotazione]
GO
ALTER TABLE [dbo].[TbSlAtt] ADD  CONSTRAINT [DF_TbSlAtt_SlAttPrezzo]  DEFAULT ((0)) FOR [SlAttPrezzo]
GO
ALTER TABLE [dbo].[TbSlAtt] ADD  CONSTRAINT [DF_TbSlAtt_SlStampa]  DEFAULT ((1)) FOR [SlStampa]
GO
ALTER TABLE [dbo].[TbSlCli] ADD  CONSTRAINT [DF_TbSlCli_SlCliPrezzo]  DEFAULT ((0)) FOR [SlCliPrezzo]
GO
ALTER TABLE [dbo].[TbSlCli] ADD  CONSTRAINT [DF_TbSlCli_SlStampa]  DEFAULT ((1)) FOR [SlStampa]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosTrasf]  DEFAULT ((0)) FOR [SosTrasf]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosWebRif]  DEFAULT ((0)) FOR [SosWebRif]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosCreato]  DEFAULT (getdate()) FOR [SosCreato]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosOperatore]  DEFAULT ('') FOR [SosOperatore]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosNDep]  DEFAULT ((0)) FOR [SosNDep]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF__TbSos__SosVardes__40DC0D00]  DEFAULT ('') FOR [SosVardest]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF__TbSos__SosFP__41D03139]  DEFAULT ((0)) FOR [SosFP]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF__TbSos__SosNote__42C45572]  DEFAULT ('') FOR [SosNote]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosPvv]  DEFAULT ('') FOR [SosPvv]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosPrivato]  DEFAULT ((0)) FOR [SosPrivato]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosStampato]  DEFAULT ((0)) FOR [SosStampato]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosStampaNum]  DEFAULT ((0)) FOR [SosStampaNum]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosAccesso]  DEFAULT ('') FOR [SosAccesso]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosTipoConsegna]  DEFAULT ('') FOR [SosTipoConsegna]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosMailConferma]  DEFAULT ((0)) FOR [SosMailConferma]
GO
ALTER TABLE [dbo].[TbSos] ADD  CONSTRAINT [DF_TbSos_SosMailPronto]  DEFAULT ((0)) FOR [SosMailPronto]
GO
ALTER TABLE [dbo].[TbSosP] ADD  CONSTRAINT [DF_TbSosP_PCProg]  DEFAULT ((0)) FOR [PCProg]
GO
ALTER TABLE [dbo].[TbSosP] ADD  CONSTRAINT [DF_TbSosP_PQta]  DEFAULT ((0)) FOR [PQta]
GO
ALTER TABLE [dbo].[TbSosP] ADD  CONSTRAINT [DF_TbSosP_PLotto]  DEFAULT ('') FOR [PLotto]
GO
ALTER TABLE [dbo].[TbSosPronti] ADD  CONSTRAINT [DF_TbSosPronti_DocPrTipoDoc]  DEFAULT ('') FOR [DocPrTipoDoc]
GO
ALTER TABLE [dbo].[TbSosPronti] ADD  CONSTRAINT [DF_TbSosPronti_DocPrRif]  DEFAULT ((0)) FOR [DocPrRif]
GO
ALTER TABLE [dbo].[TbSosPronti] ADD  CONSTRAINT [DF_TbSosPronti_SosPrDataOra]  DEFAULT (getdate()) FOR [SosPrDataOra]
GO
ALTER TABLE [dbo].[TbSosPronti] ADD  CONSTRAINT [DF_TbSosPronti_SosOperatore]  DEFAULT ('') FOR [SosPOperatore]
GO
ALTER TABLE [dbo].[TbStatoOrd] ADD  CONSTRAINT [DF_TbStatoOrd_StCod]  DEFAULT ((0)) FOR [StCod]
GO
ALTER TABLE [dbo].[TbStatoOrd] ADD  CONSTRAINT [DF_Table_1_StatoOrd]  DEFAULT ('') FOR [StDesc]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF__TbTai__TaiArtLib__68736660]  DEFAULT ('FF') FOR [TaiArtLibero]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF__TbTai__TaiForMag__677F4227]  DEFAULT ('') FOR [TaiForMag]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF__TbTai__TaiSmtpPo__69678A99]  DEFAULT ((25)) FOR [TaiSmtpPorta]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF__TbTai__TaiSmtpSS__6A5BAED2]  DEFAULT ((0)) FOR [TaiSmtpSSL]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF__TbTai__TaiSpeseA__6B4FD30B]  DEFAULT ((0)) FOR [TaiSpeseAmm]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('') FOR [TaiNoteEstero]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('TD01') FOR [TaiTipoFte_Differita]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('TD01') FOR [TaiTipoFte_Libera]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('TD04') FOR [TaiTipoFte_Ncr]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('') FOR [TaiCliFatt]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiUnder]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiHistory]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiBlockAnno]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiInvioMag]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiCiv7]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('') FOR [TaiCpt7]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiBloccoMagazzino]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('') FOR [TaiCCOrdini]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('') FOR [TaiBodyOrdini]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiCivaOrd]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((18)) FOR [TaiMesiFile]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ((0)) FOR [TaiMgFM]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('') FOR [TaiForn360]
GO
ALTER TABLE [dbo].[TbTai] ADD  DEFAULT ('') FOR [TaiFornBivetro]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiFornAcrilico]  DEFAULT ('') FOR [TaiFornAcrilico]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiTermInattivita]  DEFAULT ((10)) FOR [TaiTermInattivita]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiMaggFori]  DEFAULT ((0)) FOR [TaiMaggFori]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiMaggTacche]  DEFAULT ((0)) FOR [TaiMaggTacche]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiEnergiaBivetro]  DEFAULT ((0)) FOR [TaiEnergiaBivetro]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiEnergia360]  DEFAULT ((0)) FOR [TaiEnergia360]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiUMEnegiaBivetro]  DEFAULT ('MQ') FOR [TaiUMEnergiaBivetro]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiUMEnergia360]  DEFAULT ('KG') FOR [TaiUMEnergia360]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPagQua]  DEFAULT ((0)) FOR [TaiPagQua]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiLimCorr]  DEFAULT ((0)) FOR [TaiLimCorr]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPathBilance]  DEFAULT ('') FOR [TaiPathBilance]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiCliCorr]  DEFAULT ('') FOR [TaiCliCorr]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiRecBollo]  DEFAULT ((0)) FOR [TaiRecBollo]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiLimBollo]  DEFAULT ((77.47)) FOR [TaiLimBollo]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiImpBollo]  DEFAULT ((0)) FOR [TaiImpBollo]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiCodBollo]  DEFAULT ('BOLLO') FOR [TaiCodBollo]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiSplit10]  DEFAULT ((4)) FOR [TaiSplit10]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiSplit20]  DEFAULT ((5)) FOR [TaiSplit20]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPrBilVar]  DEFAULT ('TORTA') FOR [TaiPrBilVar]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiCliTorte]  DEFAULT ('') FOR [TaiCliTorte]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiCodTorte]  DEFAULT ('') FOR [TaiCodTorte]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiNote]  DEFAULT ('') FOR [TaiNote]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiKeibCassa]  DEFAULT ((0)) FOR [TaiKeibCassa]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiSogliaListinoDitte]  DEFAULT ((350)) FOR [TaiSogliaListinoDitte]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiListinoDitte]  DEFAULT ((0)) FOR [TaiListinoDitte]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPorzioneAdulti]  DEFAULT ((0)) FOR [TaiPorzioneAdulti]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPorzioneBambini]  DEFAULT ((0)) FOR [TaiPorzioneBimbi]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPesoMinimoTorta]  DEFAULT ((0)) FOR [TaiPesoMinimoTorta]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPorzioneAdulti2]  DEFAULT ((0)) FOR [TaiPorzioneAdulti2]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPorzioneBimbi2]  DEFAULT ((0)) FOR [TaiPorzioneBimbi2]
GO
ALTER TABLE [dbo].[TbTai] ADD  CONSTRAINT [DF_TbTai_TaiPesoMinimoTorta2]  DEFAULT ((0)) FOR [TaiPesoMinimoTorta2]
GO
ALTER TABLE [dbo].[TbTDis] ADD  CONSTRAINT [DF_TbTDis0_TDisCod]  DEFAULT ('') FOR [TDisCod]
GO
ALTER TABLE [dbo].[TbTDis] ADD  CONSTRAINT [DF_TbTDis0_TDisDesc]  DEFAULT ('') FOR [TDisDesc]
GO
ALTER TABLE [dbo].[TbTDis] ADD  CONSTRAINT [DF_TbTDis0_TDisEtixFoglio]  DEFAULT ((0)) FOR [TDisEtixFoglio]
GO
ALTER TABLE [dbo].[TbTDis] ADD  CONSTRAINT [DF_TbTDis0_TDisFileITA]  DEFAULT ('') FOR [TDisFileITA]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesNum]  DEFAULT (0) FOR [TesNum]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesCau]  DEFAULT (0) FOR [TesCau]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesCliFor]  DEFAULT ('') FOR [TesCliFor]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesGClFo]  DEFAULT ('') FOR [TesGClFo]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesNdep']  DEFAULT (0) FOR [TesNdep]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesPagCod]  DEFAULT (0) FOR [TesPagCod]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesAbi]  DEFAULT (0) FOR [TesAbi]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesCab]  DEFAULT (0) FOR [TesCab]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesSculter]  DEFAULT (0) FOR [TesSculter]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesPorto]  DEFAULT ('') FOR [TesPorto]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesVett1]  DEFAULT (0) FOR [TesVett1]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesVett2]  DEFAULT (0) FOR [TesVett2]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesAcconto]  DEFAULT (0) FOR [TesAcconto]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesNumReg]  DEFAULT (0) FOR [TesNumReg]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesNumProt]  DEFAULT (0) FOR [TesNumProt]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesNumAnno]  DEFAULT (0) FOR [TesNumAnno]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesBanca]  DEFAULT ('') FOR [TesBanca]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesAge]  DEFAULT (0) FOR [TesAge]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesTrasf]  DEFAULT (0) FOR [TesTrasf]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesDepDep]  DEFAULT (0) FOR [TesDepDep]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesDepRif]  DEFAULT (0) FOR [TesDepRif]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesGancioRif]  DEFAULT (0) FOR [TesGancioRif]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesDMV]  DEFAULT ('') FOR [TesDMV]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesNColli]  DEFAULT (0) FOR [TesNColli]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesPeso]  DEFAULT (0) FOR [TesPeso]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesAspBeni]  DEFAULT ('') FOR [TesAspBeni]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesValBol]  DEFAULT (0) FOR [TesValBol]
GO
ALTER TABLE [dbo].[TbTes] ADD  CONSTRAINT [DF_TbTes_TesAnnota]  DEFAULT ('') FOR [TesAnnota]
GO
ALTER TABLE [dbo].[TbTes] ADD  DEFAULT (0) FOR [TesBolRif]
GO
ALTER TABLE [dbo].[TbTes] ADD  DEFAULT ((0)) FOR [TesFatRif]
GO
ALTER TABLE [dbo].[TbTes] ADD  DEFAULT ((0)) FOR [TesBEvasione]
GO
ALTER TABLE [dbo].[TbTip] ADD  CONSTRAINT [DF_TbCat2_Cat2Desc]  DEFAULT ('') FOR [TipDesc]
GO
ALTER TABLE [dbo].[TbTipDoc] ADD  CONSTRAINT [DF_TdTipDoc_TdQuadri]  DEFAULT ((0)) FOR [TdQuadri]
GO
ALTER TABLE [dbo].[TbTipDoc] ADD  CONSTRAINT [DF_TdTipDoc_TdGestione]  DEFAULT ('') FOR [TdGestione]
GO
ALTER TABLE [dbo].[TbTipDoc] ADD  CONSTRAINT [DF_TbTipDoc_TdRegistro]  DEFAULT ((0)) FOR [TdRegistro]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorCliente]  DEFAULT ('') FOR [TorCliente]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorStampato]  DEFAULT (0) FOR [TorStampato]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorInPreparazione]  DEFAULT (0) FOR [TorInPreparazione]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorInBolla]  DEFAULT (0) FOR [TorInBolla]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorNote]  DEFAULT ('') FOR [TorNote]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorBolRif]  DEFAULT (0) FOR [TorBolRif]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorTipoDoc]  DEFAULT ('') FOR [TorTipoDoc]
GO
ALTER TABLE [dbo].[TbTor] ADD  DEFAULT (0) FOR [TorWebRif]
GO
ALTER TABLE [dbo].[TbTor] ADD  DEFAULT ('') FOR [TorTipOrd]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorNoteSped]  DEFAULT ('') FOR [TorNoteSped]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorNoLotti]  DEFAULT ((0)) FOR [TorNoLotti]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorLottofresco]  DEFAULT ((0)) FOR [TorLottoFresco]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorSpTrasp]  DEFAULT ((0)) FOR [TorSpTrasp]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorValore]  DEFAULT ((0)) FOR [TorValore]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorSpAcc]  DEFAULT ((0)) FOR [TorSpAcc]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorBloccato]  DEFAULT ((0)) FOR [TorBloccato]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorProforma]  DEFAULT ((0)) FOR [TorProforma]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorPrenotato]  DEFAULT ((0)) FOR [TorPrenotato]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorOperatore]  DEFAULT ('') FOR [TorOperatore]
GO
ALTER TABLE [dbo].[TbTor] ADD  CONSTRAINT [DF_TbTor_TorASaldo]  DEFAULT ((0)) FOR [TorASaldo]
GO
ALTER TABLE [dbo].[TbUMis] ADD  CONSTRAINT [DF_TbUMis_UMLivConv]  DEFAULT ((1)) FOR [UMLivConv]
GO
ALTER TABLE [dbo].[TbVettori] ADD  CONSTRAINT [DF_TbVettori_VetCodFor]  DEFAULT ('') FOR [VetCodFor]
GO
ALTER TABLE [dbo].[TbVettori] ADD  CONSTRAINT [DF_TbVettori_VetAttivo]  DEFAULT ((0)) FOR [VetAttivo]
GO
ALTER TABLE [dbo].[TbVettori] ADD  CONSTRAINT [DF_TbVettori_VetNazione]  DEFAULT ('') FOR [VetNazione]
GO
ALTER TABLE [dbo].[TMP_ETICHETTE] ADD  CONSTRAINT [DF_TMP_ETICHETTE_ARTCOD]  DEFAULT ('') FOR [ARTCOD]
GO
ALTER TABLE [dbo].[TMP_ETICHETTE] ADD  CONSTRAINT [DF_TMP_ETICHETTE_LOTTO]  DEFAULT ('') FOR [LOTTO]
GO
ALTER TABLE [dbo].[TMP_ETICHETTE] ADD  CONSTRAINT [DF_TMP_ETICHETTE_SCADENZA]  DEFAULT ('') FOR [SCADENZA]
GO
ALTER TABLE [dbo].[TMP_ETICHETTE] ADD  CONSTRAINT [DF_TMP_ETICHETTE_DESCRIZIONE]  DEFAULT ('') FOR [DESCRIZIONE]
GO
ALTER TABLE [dbo].[TMP_ETICHETTE] ADD  CONSTRAINT [DF_TMP_ETICHETTE_BARCODE]  DEFAULT ('') FOR [BARCODE]
GO
ALTER TABLE [dbo].[TMP_IMBALLO] ADD  CONSTRAINT [DF_TMP_IMBALLO_LOTTO]  DEFAULT ('') FOR [LOTTO]
GO
ALTER TABLE [dbo].[TMP_IMBALLO] ADD  CONSTRAINT [DF_TMP_IMBALLO_SCADENZA]  DEFAULT ('') FOR [SCADENZA]
GO
ALTER TABLE [dbo].[TMP_LISTVENDITA] ADD  CONSTRAINT [DF_TMP_LISTVENDITA_UMV]  DEFAULT ('') FOR [UMV]
GO
ALTER TABLE [dbo].[TMP_LISTVENDITA] ADD  CONSTRAINT [DF_TMP_LISTVENDITA_PREZZO_BASE]  DEFAULT ((0)) FOR [PREZZO_BASE]
GO
ALTER TABLE [dbo].[TMP_LISTVENDITA] ADD  CONSTRAINT [DF_TMP_LISTVENDITA_PREZZO_IVA]  DEFAULT ((0)) FOR [PREZZO_IVA]
GO
ALTER TABLE [dbo].[TMP_LISTVENDITA] ADD  CONSTRAINT [DF_TMP_LISTVENDITA_PREZZO_BASE_IVA]  DEFAULT ((0)) FOR [PREZZO_BASE_IVA]
GO
ALTER TABLE [dbo].[TMP_LISTVENDITA] ADD  CONSTRAINT [DF_TMP_LISTVENDITA_ALIQ]  DEFAULT ((0)) FOR [ALIQ]
GO
ALTER TABLE [dbo].[TMP_LISTVENDITA] ADD  CONSTRAINT [DF_TMP_LISTVENDITA_STAMPALIS]  DEFAULT ((0)) FOR [STAMPALIS]
GO
ALTER TABLE [dbo].[TMP_NEWORD_TORTE] ADD  DEFAULT ('') FOR [ORDTDETTAGLIO_HTML]
GO
ALTER TABLE [dbo].[TMP_NEWORD_TORTE] ADD  CONSTRAINT [DF_TMP_NEWORD_TORTE_ORDTCANDELINE_HTML]  DEFAULT ('') FOR [ORDTCANDELINE_HTML]
GO
ALTER TABLE [dbo].[TMP_NEWORD_TORTE] ADD  CONSTRAINT [DF_TMP_NEWORD_TORTE_ORDTACCONTO]  DEFAULT ((0)) FOR [ORDTACCONTO]
GO
ALTER TABLE [dbo].[TMP_ORD_PANETTONI] ADD  CONSTRAINT [DF_TMP_ORD_PANETTONI_PESO]  DEFAULT ('') FOR [PEZZATURA]
GO
ALTER TABLE [dbo].[TMP_ORD_PANETTONI] ADD  CONSTRAINT [DF_TMP_ORD_PANETTONI_CONFEZIONE]  DEFAULT ('') FOR [CONFEZIONE]
GO
ALTER TABLE [dbo].[TMP_ORD_PANETTONI] ADD  CONSTRAINT [DF_TMP_ORD_PANETTONI_PRODOTTO]  DEFAULT ('') FOR [PRODOTTO]
GO
ALTER TABLE [dbo].[TMP_ORD_TORTE] ADD  CONSTRAINT [DF_TMP_ORD_TORTE_CORANNOTA]  DEFAULT ('') FOR [CORANNOTA]
GO
ALTER TABLE [dbo].[TMP_PLU] ADD  CONSTRAINT [DF_TMP_PLU_TMPARTID]  DEFAULT ((0)) FOR [TMPARTID]
GO
ALTER TABLE [dbo].[TmpSCor] ADD  CONSTRAINT [DF_TmpSCor_CorOrdProg]  DEFAULT ((0)) FOR [CorOrdProg]
GO
ALTER TABLE [dbo].[TmpSCor] ADD  CONSTRAINT [DF_TmpSCor_CorUM]  DEFAULT ('') FOR [CorUM]
GO
ALTER TABLE [dbo].[TmpSCor] ADD  CONSTRAINT [DF_TmpSCor_CorLotto]  DEFAULT ('') FOR [CorLotto]
GO
/****** Object:  StoredProcedure [dbo].[ControllaSpeseRb]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create Proc [dbo].[ControllaSpeseRb] @FATRIF AS INT, @DFATT AS SMALLDATETIME, @CLIENTE AS VARCHAR(5), @PAGAM as smallint
as

declare @controllo AS bit, @TEST AS SMALLINT


set @controllo = 0
    
    
SELECT   @controllo  

GO
/****** Object:  StoredProcedure [dbo].[CreaPrefattura]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[CreaPrefattura] @DEP as smallint,@DataFatt smalldatetime, @DataBol smalldatetime, @CodCli varchar(5), @CodCliCons varchar(5), @CodPag smallint, @BolNRag int 
as

DECLARE @CODAGE  AS SMALLINT,  @ULRIF AS int, @TIPOFTE AS VARCHAR(4)

SET @CODAGE = (SELECT CLCODAGE FROM TBCLI WHERE CLCOD = @CODCLI)

SET @TIPOFTE =  (SELECT TOP 1 TaiTipoFte_Differita FROM TbTai order by TaiAnno desc)

SET @ULRIF = 0

IF @CODAGE = 0
   BEGIN
   SET @CODAGE = 99
   END

INSERT INTO TbFat(FatTipoDoc,FatData,FatNum,FatAge,FatCliCons,FatPagCod,FatAbi,FatCab,FatEsespe,FatNimp,FatCau,FatSculter,FatCliFat,
		  FatNumReg,FatTrasf,FatDiff,FatNdep,FatTipoFte)

select distinct 'F', @DataFatt,0,@codage, BolCliCons,BolPagCod,MIN(BolAbi),MIN(BolCab),MIN(BolEsespe),min(BolNimp),5,
                 0,BolCliFat,BolNumReg,0,'D',@DEP,@TIPOFTE
from TbBol 
where 
        -----BolNdep = @DEP AND
		BolData<=@DataBol and DATEPART(year,boldata) = datepart(year,@datafatt) and 
		BolCliCons=@CodCliCons and
        BolCliFat=@CodCli and
		BolPagCod=@CodPag and
		BolNRag=@BolNRag and
        BolTipoDoc = 'B' and
        BolPrefAgg = 0 and bolnum <> 999999 and ( BolCau = 5 or bolcau = 35)
group by BolCliCons,BolPagCod,BolCliFat,BolNumReg

        
	-- Aggiorno i Riferimenti alle Fatture che ho appena inserito

set @ULRIF = (select @@identity)

	Update TbBol
	set BolRifFat=@ULRIF
	where
	    ------BolNdep = @DEP and 
		BolData<=@DataBol and DATEPART(year,boldata) = datepart(year,@datafatt) and 
		BolCliCons=@CodCliCons and
        BolCliFat=@CodCli and
		BolPagCod=@CodPag and
		BolNRag=@BolNRag and
                BolTipoDoc = 'B' and
                BolPrefAgg = 0 and BolNum <> 999999 and ( BolCau = 5 or bolcau = 35)



SELECT  @ULRIF as CorRif ,'F' as CorTipoDoc,IDENTITY(INT,1,1) as CorProg,CorArtID,CorCodARt,CorDesc,Sum(CorQuaCon) as CorQuaCon,CorPrezzo,CorSc1,CorSc2,CorSc3,CorSc4,CorSc5,
        CorNetto,Sum(CorImporto) as CorImporto,0 as CorUrgenza,CorSpot,' ' as CorPromo,0 as CorAssortito,0 as CorVariato,' ' as CorCampagna,CorCiva,CorCnTrp,CorCau,CorMacro,0 as CorOrdRif,
        ' ' as CorOrdine,' ' as CorArtFor,sum(CorImballo) as CorImballo,CorMerc,CorBrand
into #tmp
       	
FROM   TbCor inner join
	   TbBol on Bolrif = CorRif and BolTipoDoc = CorTipoDoc	  
WHERE BolRifFat = @ULRIF and CorArtId <> 0
group by CorArtID,CorCodArt,CorDesc,CorPrezzo,CorSc1,CorSc2,CorSc3,CorSc4,CorSc5,CorNetto,CorSpot,CorCiva,CorCnTrp,CorCau,CorMacro,CorMerc,CorBrand


INSERT into TbCor  (CorRif ,CorTipoDoc,CorProg,CorArtID,CorCodARt,CorDesc,CorQuaCon,CorPrezzo,CorSc1,CorSc2,CorSc3,CorSc4,CorSc5,
        CorNetto,CorImporto,CorUrgenza,CorSpot,CorPromo,CorAssortito,CorVariato,CorCampagna,CorCiva,CorCnTrp,CorCau,CorMacro,CorOrdRif,
        CorOrdine,CorArtFor,CorImballo,CorMerc,CorBrand)
select * from #tmp


EXEC X_SPOSTA_CODIVA @Ulrif

EXEC X_RECUPERA_BOLLO @Ulrif


SELECT @Ulrif
GO
/****** Object:  StoredProcedure [dbo].[DELBLK]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   procedure [dbo].[DELBLK] @BLOK int,@REGISTRO smallint
as
delete from TbFat where FatRif in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK AND IDNUMREG = @REGISTRO )
delete from TbDcg  where DcgTipo = 'F' and DcgNumRif in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK AND IDNUMREG = @REGISTRO )
delete from TbCor  where CorTipoDoc = 'F' and CorRif in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK AND IDNUMREG = @REGISTRO )
Update TbBol set BolPrefAgg = 0,BolRifFat = 0 where BolRifFat in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK AND IDNUMREG = @REGISTRO)
DELETE FROM Coge.dbo.TbEff where RicRifFat in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK AND IDNUMREG = @REGISTRO )
DELETE FROM TbFte where FteRif in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK AND IDNUMREG = @REGISTRO )
Delete from TbBLOCK where IDBLOCCO =  @BLOK  AND IDNUMREG = @REGISTRO
GO
/****** Object:  StoredProcedure [dbo].[DxVendutoAnno]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     Proc [dbo].[DxVendutoAnno] @IDBLK AS INT
as


DECLARE @TUTTI AS INT, @DATAMIN AS SMALLDATETIME

SET @DATAMIN = '23/04/2025'   ------DSATA IN CUI SONO UNA S.R.L.

SET @TUTTI = 1000001

--------
select *
into #tmpgqua
from GQUA.DBO.TbFat

delete from #tmpgqua where fatrif in (select airif from GQUA.DBO.TbAddinfo where AiEliminato = 1)

------   SOLO BOLLE NON FATTURATE + TUTTE LE FATTURE

select  ANNO = datepart(year,BolData),
        MESE = datepart(month,BolData),
        TRIMESTRE = CASE WHEN datepart(month,BolData) BETWEEN 1 AND 3 THEN 1  WHEN datepart(month,BolData) BETWEEN 4 AND 6 THEN 2  WHEN datepart(month,BolData) BETWEEN 7 AND 9 THEN 3  ELSE 4 END,
        GLOBALE = 1,
        CLIENTE = BolCliCons,
	    TPAG = (SELECT PagTipo from COGE.dbo.TbPag WHERE PagCod = BolPagCod),
        Artid = CorArtId,	
        VENDUTO_QTA = SUM (CASE WHEN CORSPOT = 0 THEN CorQuaCon ELSE 0 END),   
        VENDUTO_VALORE = SUM (CASE WHEN CORSPOT = 0 THEN cORImporto ELSE 0 END),
        C_VENDUTO_QTA = CAST(0 AS DECIMAL(10,2)),
        C_VENDUTO_VALORE = CAST(0 AS DECIMAL(10,2))

into #tmp        
from TbCor inner join
     TbBol on BolTipoDoc = CorTipoDoc and BolRif = CorRif 
WHERE corartid > 0 and datepart(year,BolData) IN (SELECT TANNO from tmpanno where tblock = @IDBLK) AND BOLNUM <> 999999 AND BolCau <> 20 and Bolriffat = 0
group by DATEPART(yEAR,BolData),datepart(month,Boldata),BolCliCons,CorArtId,CorCau,CorSpot,BolPagCod

UNION ALL

select  ANNO = datepart(year,FatData),
        MESE = datepart(month,FatData),
        TRIMESTRE = CASE WHEN datepart(month,FatData) BETWEEN 1 AND 3 THEN 1  WHEN datepart(month,FatData) BETWEEN 4 AND 6 THEN 2  WHEN datepart(month,FatData) BETWEEN 7 AND 9 THEN 3  ELSE 4 END,
        GLOBALE = 1,
        CLIENTE = FatCliCons,
		TPAG = (SELECT PagTipo from COGE.dbo.TbPag WHERE PagCod = FatPagCod),
        Artid = CorArtId,
        VENDUTO_QTA = CASE WHEN (CORCAU = 5 OR CorCau = 35) and CORSPOT = 0 then SUM(CorQuaCon) ELSE 0 END,
        VENDUTO_VALORE = CASE WHEN (CORCAU = 5 OR CorCau = 35) then SUM(CorImporto) WHEN CORCAU = 3 THEN SUM(CorImporto * -1) ELSE 0 END,
        C_VENDUTO_QTA = CAST(0 AS DECIMAL(10,2)),
        C_VENDUTO_VALORE = CAST(0 AS DECIMAL(10,2))

from TbCor inner join
     TbFat on FatTipoDoc = CorTipoDoc and FatRif = CorRif 
WHERE corartid > 0 and datepart(year,FatData) IN (SELECT TANNO from tmpanno where tblock = @IDBLK) and fatcau = 5 and fatnum <> 999999
group by DATEPART(yEAR,FatData),datepart(month,Fatdata),FatCliCons,CorArtId,CorCau,CorSpot,FatPagCod


UNION ALL

select  ANNO = datepart(year,FatData),
        MESE = datepart(month,FatData),
        TRIMESTRE = CASE WHEN datepart(month,FatData) BETWEEN 1 AND 3 THEN 1  WHEN datepart(month,FatData) BETWEEN 4 AND 6 THEN 2  WHEN datepart(month,FatData) BETWEEN 7 AND 9 THEN 3  ELSE 4 END,
        GLOBALE = 1,
        CLIENTE = FatCliCons,
		TPAG = (SELECT PagTipo from COGE.dbo.TbPag WHERE PagCod = FatPagCod),
        Artid = CorArtId,
        VENDUTO_QTA = CAST(0 AS DECIMAL(10,2)),
        VENDUTO_VALORE = CAST(0 AS DECIMAL(10,2)),
        C_VENDUTO_QTA = CASE WHEN CORCAU = 5 and CORSPOT = 0 then SUM(CorQuaCon) ELSE 0 END,
        C_VENDUTO_VALORE = CASE WHEN CORCAU = 5  and CORSPOT = 0 then SUM(CorImporto) WHEN CORCAU = 7 THEN SUM(CorImporto) WHEN CORCAU = 9 THEN SUM(CorImporto * -1) ELSE 0 END

from GQUA.DBO.TbCor inner join
     #tmpgqua on FatTipoDoc = CorTipoDoc and FatRif = CorRif  
WHERE corartid > 0 AND FATTIPODOC = 'F' and  datepart(year,FatData) IN (SELECT TANNO from tmpanno where tblock = @IDBLK) and fatdiff <> 'D' and FatData > @DATAMIN
group by DATEPART(yEAR,FatData),datepart(month,Fatdata),FatCliCons,CorArtId,CorCau,CorSpot,FatPagCod



SELECT ANNO,
       MESE = CASE WHEN MESE = 1 THEN '01 GEN.' 
                   WHEN MESE = 2 THEN '02 FEB.' 
                   WHEN MESE = 3 THEN '03 MAR.' 
                   WHEN MESE = 4 THEN '04 APR.' 
                   WHEN MESE = 5 THEN '05 MAG.' 
                   WHEN MESE = 6 THEN '06 GIU.' 
                   WHEN MESE = 7 THEN '07 LUG.' 
                   WHEN MESE = 8 THEN '08 AGO.' 
                   WHEN MESE = 9 THEN '09 SET.' 
                   WHEN MESE = 10 THEN '10 OTT.' 
                   WHEN MESE = 11 THEN '11 NOV.' 
                   WHEN MESE = 12 THEN '12 DIC.' END,     
       TRIMESTRE =  CASE WHEN TRIMESTRE = 1 THEN 'I TRIMESTRE' 
                         WHEN TRIMESTRE = 2 THEN 'II TRIMESTRE' 
                         WHEN TRIMESTRE = 3 THEN 'III TRIMESTRE.' 
                         WHEN TRIMESTRE = 4 THEN 'IV TRIMESTRE' END,   
       GLOBALE = 'GLOBALE',                                    
       CLIENTE = CAST(CLIENTE AS VARCHAR(5)) + '  ' + ISNULL((SELECT AnaDesc from vdox.dbo.TbAna where anacod = CAST(CLIENTE AS VARCHAR(5))),''),
       
       GRUPPO_ATTIVITA = cast(CLGRCANALE as varchar(2)) + '  ' + (SELECT AttDesc from TbAtt where AttCod = ClGrCanale),
	   CATEGORIA = (SELECT CatDesc from TbCat where CatCod = ArtCat),
       PRODOTTO = ArtCod + ' ' + ArtDesc, 
       CORRISP_VALORE =  C_VENDUTO_VALORE,
       CORRISP_QTA = C_VENDUTO_QTA,
       GEVE_VALORE = VENDUTO_VALORE,
       GEVE_QTA = VENDUTO_QTA,
       VALORE = VENDUTO_VALORE + C_VENDUTO_VALORE,  
       QTA = VENDUTO_QTA +  C_VENDUTO_QTA
       --PMACQ = (SELECT MGPMEDIO FROM TbMgP WHERE MGPMAG = 0 AND MGPANNO = ANNO AND MGPID = 25 AND MGPARTID = a.ARTID),
	   --TIPO_PAGAMENTO = CASE WHEN TPAG = 1 THEN 'TRATTA'
	                         --WHEN TPAG = 2 THEN 'RI.BA.'
							 --WHEN TPAG = 3 THEN 'RIM.DIRETTA'
							 --WHEN TPAG = 4 THEN 'CONTANTI'
							 --WHEN TPAG = 5 THEN 'BONIFICO'
							 --WHEN TPAG = 6 THEN 'CONTRASSEGNO'
							 --WHEN TPAG = 7 THEN 'R.I.D.' END
      
FROM #TMP A INNER JOIN
     TbArt b on b.ArtId = a.ArtId  INNER JOIN 
     TbCli on ClCod = CLIENTE
GO
/****** Object:  StoredProcedure [dbo].[DxVendutoPeriodo]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     Proc [dbo].[DxVendutoPeriodo] @DAL AS SMALLDATETIME, @AL AS SMALLDATETIME, @DEP AS SMALLINT
as


DECLARE @TUTTI AS INT, @DATAMIN AS SMALLDATETIME

SET @TUTTI = 1000001


SET @DATAMIN = '24/04/2025'   ------DSATA IN CUI SONO UNA S.R.L.

IF @DAL < @DATAMIN
   BEGIN

    SET @DAL =  @DATAMIN

   END

select *
into #tmpgqua
from GQUA.DBO.TbFat

delete from #tmpgqua where fatrif in (select airif from GQUA.DBO.TbAddinfo where AiEliminato = 1)


------   SOLO BOLLE NON FATTURATE + TUTTE LE FATTURE

select  ANNO = datepart(year,BolData),
        MESE = datepart(month,BolData),
        TRIMESTRE = CASE WHEN datepart(month,BolData) BETWEEN 1 AND 3 THEN 1  WHEN datepart(month,BolData) BETWEEN 4 AND 6 THEN 2  WHEN datepart(month,BolData) BETWEEN 7 AND 9 THEN 3  ELSE 4 END,
        GLOBALE = 1,
        CLIENTE = BolCliCons,
	    TPAG = (SELECT PagTipo from COGE.dbo.TbPag WHERE PagCod = BolPagCod),
        Artid = CorArtId,	
        VENDUTO_QTA = SUM (CASE WHEN CORSPOT = 0 THEN CorQuaCon ELSE 0 END),   
        VENDUTO_VALORE = SUM (CASE WHEN CORSPOT = 0 THEN cORImporto ELSE 0 END),
        C_VENDUTO_QTA = CAST(0 AS DECIMAL(10,2)),
        C_VENDUTO_VALORE = CAST(0 AS DECIMAL(10,2))

into #tmp        
from TbCor inner join
     TbBol on BolTipoDoc = CorTipoDoc and BolRif = CorRif 
WHERE BolNDep = @DEP AND corartid > 0 and (BOLDATA BETWEEN @DAL AND @AL ) AND BOLNUM <> 999999 AND BolCau <> 20 and bolriffat = 0
group by DATEPART(yEAR,BolData),datepart(month,Boldata),BolCliCons,CorArtId,CorCau,CorSpot,BolPagCod

UNION ALL

select  ANNO = datepart(year,FatData),
        MESE = datepart(month,FatData),
        TRIMESTRE = CASE WHEN datepart(month,FatData) BETWEEN 1 AND 3 THEN 1  WHEN datepart(month,FatData) BETWEEN 4 AND 6 THEN 2  WHEN datepart(month,FatData) BETWEEN 7 AND 9 THEN 3  ELSE 4 END,
        GLOBALE = 1,
        CLIENTE = FatCliCons,
		TPAG = (SELECT PagTipo from COGE.dbo.TbPag WHERE PagCod = FatPagCod),
        Artid = CorArtId,
        VENDUTO_QTA = CASE WHEN (CORCAU = 5 OR CorCau = 35) and CORSPOT = 0 then SUM(CorQuaCon) ELSE 0 END,
        VENDUTO_VALORE = CASE WHEN (CORCAU = 5 OR CorCau = 35) then SUM(CorImporto) WHEN CORCAU =3THEN SUM(CorImporto * -1) ELSE 0 END,
        C_VENDUTO_QTA = CAST(0 AS DECIMAL(10,2)),
        C_VENDUTO_VALORE = CAST(0 AS DECIMAL(10,2))

from TbCor inner join
     TbFat on FatTipoDoc = CorTipoDoc and FatRif = CorRif 
WHERE FatNDep = @DEP AND corartid > 0 and  FATDATA BETWEEN @DAL AND @AL and fatcau = 5 and fatnum <> 999999
group by DATEPART(yEAR,FatData),datepart(month,Fatdata),FatCliCons,CorArtId,CorCau,CorSpot,FatPagCod

UNION ALL

select  ANNO = datepart(year,FatData),
        MESE = datepart(month,FatData),
        TRIMESTRE = CASE WHEN datepart(month,FatData) BETWEEN 1 AND 3 THEN 1  WHEN datepart(month,FatData) BETWEEN 4 AND 6 THEN 2  WHEN datepart(month,FatData) BETWEEN 7 AND 9 THEN 3  ELSE 4 END,
        GLOBALE = 1,
        CLIENTE = FatCliCons,
		TPAG = (SELECT PagTipo from COGE.dbo.TbPag WHERE PagCod = FatPagCod),
        Artid = CorArtId,
        VENDUTO_QTA = CAST(0 AS DECIMAL(10,2)),
        VENDUTO_VALORE = CAST(0 AS DECIMAL(10,2)),
        C_VENDUTO_QTA = CASE WHEN CORCAU = 5 and CORSPOT = 0 then SUM(CorQuaCon) ELSE 0 END,
        C_VENDUTO_VALORE = CASE WHEN CORCAU = 5  and CORSPOT = 0 then SUM(CorImporto) WHEN CORCAU = 7 THEN SUM(CorImporto) WHEN CORCAU = 9 THEN SUM(CorImporto * -1) ELSE 0 END

from GQUA.DBO.TbCor inner join
     #tmpgqua on FatTipoDoc = CorTipoDoc and FatRif = CorRif  
WHERE FatNDep=@DEP AND corartid > 0 AND FATTIPODOC = 'F' and  FATDATA BETWEEN @DAL AND @AL and fatdiff <> 'D'
group by DATEPART(yEAR,FatData),datepart(month,Fatdata),FatCliCons,CorArtId,CorCau,CorSpot,FatPagCod



SELECT ANNO,
       MESE = CASE WHEN MESE = 1 THEN '01 GEN.' 
                   WHEN MESE = 2 THEN '02 FEB.' 
                   WHEN MESE = 3 THEN '03 MAR.' 
                   WHEN MESE = 4 THEN '04 APR.' 
                   WHEN MESE = 5 THEN '05 MAG.' 
                   WHEN MESE = 6 THEN '06 GIU.' 
                   WHEN MESE = 7 THEN '07 LUG.' 
                   WHEN MESE = 8 THEN '08 AGO.' 
                   WHEN MESE = 9 THEN '09 SET.' 
                   WHEN MESE = 10 THEN '10 OTT.' 
                   WHEN MESE = 11 THEN '11 NOV.' 
                   WHEN MESE = 12 THEN '12 DIC.' END,     
       TRIMESTRE =  CASE WHEN TRIMESTRE = 1 THEN 'I TRIMESTRE' 
                         WHEN TRIMESTRE = 2 THEN 'II TRIMESTRE' 
                         WHEN TRIMESTRE = 3 THEN 'III TRIMESTRE.' 
                         WHEN TRIMESTRE = 4 THEN 'IV TRIMESTRE' END,   
       GLOBALE = 'GLOBALE',                                    
       CLIENTE = CAST(CLIENTE AS VARCHAR(5)) + '  ' + ISNULL((SELECT AnaDesc from vdox.dbo.TbAna where anacod = CAST(CLIENTE AS VARCHAR(5))),''),
       
       GRUPPO_ATTIVITA = cast(CLGRCANALE as varchar(2)) + '  ' + (SELECT AttDesc from TbAtt where AttCod = ClGrCanale),
	   CATEGORIA = (SELECT CatDesc from TbCat where CatCod = ArtCat),
       PRODOTTO = ArtCod + ' ' + ArtDesc, 
       CORRISP_VALORE =  C_VENDUTO_VALORE,
       CORRISP_QTA = C_VENDUTO_QTA,
       GEVE_VALORE = VENDUTO_VALORE,
       GEVE_QTA = VENDUTO_QTA,
       VALORE = VENDUTO_VALORE + C_VENDUTO_VALORE,  
       QTA = VENDUTO_QTA +  C_VENDUTO_QTA
       --PMACQ = (SELECT MGPMEDIO FROM TbMgP WHERE MGPMAG = 0 AND MGPANNO = ANNO AND MGPID = 25 AND MGPARTID = a.ARTID),
	   --TIPO_PAGAMENTO = CASE WHEN TPAG = 1 THEN 'TRATTA'
	                         --WHEN TPAG = 2 THEN 'RI.BA.'
							 --WHEN TPAG = 3 THEN 'RIM.DIRETTA'
							 --WHEN TPAG = 4 THEN 'CONTANTI'
							 --WHEN TPAG = 5 THEN 'BONIFICO'
							 --WHEN TPAG = 6 THEN 'CONTRASSEGNO'
							 --WHEN TPAG = 7 THEN 'R.I.D.' END
      
FROM #TMP A INNER JOIN
     TbArt b on b.ArtId = a.ArtId  INNER JOIN 
     TbCli on ClCod = CLIENTE
GO
/****** Object:  StoredProcedure [dbo].[NEW_X_FPA_RIGHE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       PROC [dbo].[NEW_X_FPA_RIGHE] @FATRIF AS INT
AS

DECLARE @DIFFERITA AS VARCHAR(1), @NIMP AS SMALLINT, @SPE_RB DECIMAL(9,2), @SPE_IMB DECIMAL(9,2), @CII_RB SMALLINT , @CII_IMB SMALLINT, @TOT_SCONTO DECIMAL(9,2),
        @PERC_SCONTO AS DECIMAL(5,2), @MIN_RIGA AS SMALLINT, @MAX_RIGA AS SMALLINT, @VERSIONE AS VARCHAR(25), @SWHOUSE AS VARCHAR(50), @CII_ASW AS SMALLINT,
		@CII_STANDARD AS SMALLINT, @CLIENTE AS VARCHAR(5), @CLESPSCONTI AS VARCHAR(1), @COD_BOLLO AS VARCHAR(20), @TOT_OMAGGIO_SOGG DECIMAL(9,2), @CII_OMAGGIO AS SMALLINT, @CIV_OMAGGIO AS SMALLINT,
		@TOT_OMAGGIO_ESE DECIMAL(9,2)


SET @CII_ASW = 1

SELECT @CLIENTE = FatCliFat, @DIFFERITA = FatDiff, @NIMP = FatNimp
FROM TbFat
WHERE FatRif = @FATRIF

SET @CLESPSCONTI = (SELECT ClEspSconti from TbCli where clcod = @CLIENTE)

SET @COD_BOLLO = (SELECT top 1 TAICODBOLLO FROM TbTai order by taianno desc)


SET @VERSIONE = 'Versione #Asw0100#'

SET @SWHOUSE = 'Selco s.n.c. di Castiglia T. & C. / DEDALO'

CREATE TABLE #tmprighe (
    [RIGA] int identity(1,1) not null, 
	[TIPO_CESSIONE] [varchar](2) NOT NULL,
	[CorArtId] [int] NOT NULL,
	[CorCodArt] [varchar](20) NOT NULL,
	[CorUM] [varchar](2) NOT NULL,
	[CorDesc] [varchar](200) NOT NULL,
	[CorLotto] [varchar](20) NOT NULL,
	[CorCiva] [smallint] NOT NULL,
	[CorQuaCon] [decimal](9, 3) NOT NULL,
	[CorPrezzo] [decimal](9, 3) NOT NULL,
	[CorSc1]  [decimal](5, 2) NOT NULL,
	[CorSc2]  [decimal](5, 2) NOT NULL,
	[CorSc3]  [decimal](5, 2) NOT NULL,
	[CorImporto] [decimal](9, 2) NOT NULL,
	[CorOmaggio] [varchar](1) NOT NULL,
	[CorDescIva] [varchar](35) NOT NULL,
	[CorDataScad] [smalldatetime] NULL,
	[CorSculter] [decimal](9, 2) NOT NULL,
 ) ON [PRIMARY]


 SET @CIV_OMAGGIO = (SELECT top 1 TaiCiv3 from TbTai order by TaiAnno desc)
 SET @CII_OMAGGIO = DBO.TCI(@CIV_OMAGGIO)

 SET @CII_STANDARD = (SELECT TOP 1  DBO.TCI(CorCiva) from TbCor where cortipodoc = 'F' AND CORRIF = @FATRIF AND CORCIVA > 0)
 SELECT @SPE_RB= DcgSpeRB, @SPE_IMB= DcgSpeImb, @CII_RB = DBO.TCI(DcgSpeRBCI), @CII_IMB=DcgSpeImbCI, @TOT_SCONTO=DcgRieTotSconto, @PERC_SCONTO = DcgSculter
 from TbDcg
 where dcgnumrif =  @FATRIF



 ----------- CORLOTTO NON ESISTE

if @DIFFERITA <> 'D'
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter)
   select '',CorArtId, CorCodArt, CorDesc, '', DBO.TCI(CorCiva), CorQuaCon,
        ---  case when @CLESPSCONTI = 'N' THEN CorPrezzo  * (1 -corSc1/100) else  CorPrezzo end, 
		 --- case when @CLESPSCONTI = 'N' then 0 else CorSc1 END, 
		 CorPrezzo,
		 CorSc1,
		 CorSc2, CorSc3, cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100)* (1 -corSc3/100) as decimal(11,5)) * CorQuaCon, isnull(CorOmaggi,''), '', null, CorUm ,
		 CorSculter =  (cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100) as decimal(11,5)) * CorQuaCon) - CorImporto 
   from TbFat inner join
        TbCor on cortipodoc = Fattipodoc and corrif = fatrif
   WHERE FATRIF = @FATRIF AND CorDesc > ''
   order by corprog
   END
ELSE
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter)
   select '',CorArtId, CorCodArt, CorDesc, '',  DBO.TCI(CorCiva), CorQuaCon, 
          case when @CLESPSCONTI = 'N' THEN CorPrezzo  * (1 -corSc1/100) else  CorPrezzo end, 
		  case when @CLESPSCONTI = 'N' then 0 else CorSc1 END, 
		  CorSc2, CorSc3, cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100)* (1 -corSc3/100) as decimal(11,5)) * CorQuaCon, isnull(CorOmaggi,''), '', null, CorUM,
		  CorSculter =  (cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100) as decimal(11,5)) * CorQuaCon) - CorImporto 
   FROM TbFat INNER JOIN
        TbBol on Bolriffat = FATRIF INNER JOIN
        TbCor ON (CORRIF = BOLRIF AND CORTIPODOC = BOLTIPODOC ) or (CORRIF = FATRIF AND CORCODART = 'BOLLO' AND BOLRIF = (SELECT MAX(BOLRIF) FROM tBbOL WHERE BOLRIFFAT = FATRIF))
    WHERE  FATRIF = @FATRIF AND CorArtId <>0 and CorCiva <> 0
  --- order by Boldata,BolRif,CorCodArt
   order by Boldata,BolRif,CorProg
   END


UPDATE #TMPRIGHE SET Corciva = @CII_STANDARD, CorDesciva = 'd'
where corciva = 0

UPDATE #TMPRIGHE SET CorPrezzo = CorImporto where corprezzo=0 and corimporto <> 0


SET @TOT_OMAGGIO_SOGG = ISNULL((select sum(CorImporto)  from #TMPRIGHE where corOmaggio = 'S'),0)


-------------------------------- CALCOLO STORNO X OMAGGIO ESENTE

SELECT TOT_STORNO_IMP = sum(CorImporto),
       ALIQ = CASE WHEN @NIMP = 0 THEN cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = CORCIVA),0) AS DECIMAL (5,2)) ELSE cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = @NIMP),0) AS DECIMAL (5,2)) END,
	   TOT_STORNO = CAST(0 AS DECIMAL(9,2))  
into #OMAGGIO_ESENTE
FROM #TMPRIGHE
where corOmaggio = 'E'
GROUP BY CorCiva

UPDATE #OMAGGIO_ESENTE SET TOT_STORNO = TOT_STORNO_IMP * (1 + ALIQ / 100)

SET @TOT_OMAGGIO_ESE = isnull((SELECT isnull(SUM(TOT_STORNO),0) from #OMAGGIO_ESENTE),0)

--------------------------------------------------------------------------------------------

SET @MIN_RIGA = (SELECT MIN(RIGA) FROM #tmprighe)
SET @MAX_RIGA = (SELECT MAX(RIGA) FROM #tmprighe)

IF @SPE_RB > 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter  )
   select 'AC',0, '', 'SPESE R.B.', '', @CII_RB, 0, @SPE_RB, 0, 0, 0, @SPE_RB, '', 'Spese Incasso #SP01#', null,'',0   
   END

IF @SPE_IMB > 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter  )
   select 'AC',0, '', 'CONTR.SPESE', '', @CII_IMB, 0, @SPE_IMB, 0, 0, 0, @SPE_IMB, '', 'Spese Imballo #SP04#', null, '',0   
   END

IF @TOT_SCONTO <> 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM, CorSculter  )

   select 'SC',0, '', 'IMPORTO SCONTO', '', corciva, 0, CASE WHEN @TOT_SCONTO < 0 THEN SUM(CorSculter) ELSE -SUM(CorSculter) END, 0, 0, 0, CASE WHEN @TOT_SCONTO < 0 THEN SUM(CorSculter) ELSE -SUM(CorSculter) END, '','',NULL,'',0 
   FROM #TMPRIGHE
   where isnull(CorSculter,0) <> 0
   group by CorCiva
   END

IF @TOT_OMAGGIO_SOGG > 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad,CorUM,CorSculter  )
   select '',0, '', 'Storno Valori Omaggi', '', @CII_OMAGGIO, 0, -@TOT_OMAGGIO_SOGG, 0, 0, 0, -@TOT_OMAGGIO_SOGG, '', 'STO', null,'',0   
   END

--IF @TOT_OMAGGIO_ESE> 0
--   BEGIN
--   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad,CorUM )
--   select '',0, '', 'Storno per omaggio senza rivalsa', '', @CII_OMAGGIO, 0, -@TOT_OMAGGIO_ESE, 0, 0, 0, -@TOT_OMAGGIO_ESE, '', 'STO', null,''   
--   END

---------RIGA ASSOSOFTWARE
---insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad )
---select '',0, '', 'Riga descrittiva contenente informazioni tecniche ed aggiuntive del documento', '', @CII_STANDARD, 0, 0, 0, 0, 0, 0, '', 'desc', null   

SELECT NUM_LINEA = RIGA,
       TipoCessione = TIPO_CESSIONE,
       Codice_tipo = CASE WHEN CorCodArt <> '' THEN 'AswArtFor' ELSE '' END,
	   CodiceValore = CorCodArt,
	   Descrizione = CorDesc,
	   Quantita = CASE WHEN CorQuaCon < 0 THEN cast(CorQuaCon * -1 as decimal(10,3)) ELSE cast(CorQuaCon as decimal(10,3)) END,
	   UM = CorUM,
	   PrezzoUnitario =CASE WHEN CorQuaCon < 0 THEN corPrezzo * -1 ELSE corPrezzo END,
	   Sc1 = case when CorOmaggio = 'E' THEN 100.00 ELSE cast(CorSc1 as decimal(5,2)) END,
	--   Sc1 = cast(CorSc1 as decimal(5,2)),
	   Sc2 = cast(CorSc2 as decimal(5,2)),
	   Sc3 = cast(CorSc3 as decimal(5,2)),
	   SCULTER = 0, 
	   Importo = CASE WHEN COROMAGGIO <> 'E' THEN cast(CorImporto as decimal(10,2)) ELSE 0 END,
	   --Importo = cast(CorImporto as decimal(10,2)),
	   ALIQ =CASE WHEN @NIMP = 0 THEN cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = CORCIVA),0) AS DECIMAL (5,2)) ELSE cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = @NIMP),0) AS DECIMAL (5,2)) END,
	   Natura = CASE WHEN (@NIMP = 0  or CorCodArt = @COD_BOLLO) THEN isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = CORCIVA),'') ELSE isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = @NIMP),'') END, 
	   Altri1_TIPO = case when CorLotto <> '' THEN 'Lotto' when TIPO_CESSIONE = 'AC' THEN 'AswSpAcces'  when TIPO_CESSIONE = 'SC' THEN 'AswTRiga'  
	                      when CorDescIva = 'desc' THEN 'AswTRiga'  when CorDescIva = 'd' THEN 'AswTRiga' ELSE '' END ,
	   Altri1_TESTO=CASE WHEN CorDescIva = 'desc' THEN 'Informazioni documento #ID#'  when TIPO_CESSIONE = 'SC' THEN 'Riga sconto #SC#'  when TIPO_CESSIONE = 'AC' THEN CorDesciva when CorDescIva = 'd' THEN 'Descrittivo #DE#' ELSE CorLotto END,
	   Altri1_NUMERO = '',
	   Altri1_DATA = '',
	   Altri2_TIPO = CASE WHEN CorDescIva = 'desc' THEN 'AswRelStd'  when TIPO_CESSIONE = 'SC' THEN 'AswRifRiga' when isdate(CorDataScad) = 1 THEN 'Scadenza' ELSE '' END,
	   Altri2_TESTO= CASE WHEN CorDescIva = 'desc' THEN @VERSIONE when TIPO_CESSIONE = 'SC' THEN 'RigaSconto #' + CAST(@MIN_RIGA AS VARCHAR)+ '-'  + CAST(@MAX_RIGA AS VARCHAR) + '#'ELSE '' END,
	   Altri2_NUMERO = '',
	   Altri2_DATA = ISNULL(convert(varchar(10),CorDataScad,126),''), 
	   Altri3_TIPO = CASE WHEN CorDescIva = 'desc' THEN 'AswSwHouse' ELSE '' END,
	   Altri3_TESTO= CASE WHEN CorDescIva = 'desc' THEN  @SWHOUSE ELSE '' END,
	   Altri3_NUMERO = '',
	   Altri3_DATA = ISNULL(convert(varchar(10),CorDataScad,126),''), 
	   Altri4_TIPO = CASE WHEN CorDescIva = 'desc' THEN 'AswTratSco' ELSE '' END,
	   Altri4_TESTO= CASE WHEN CorDescIva = 'desc' THEN 'Valori come righe sconto #PRS#' ELSE '' END,
	   Altri4_NUMERO = '',
	   Altri4_DATA = ISNULL(convert(varchar(10),CorDataScad,126),'') 

from #tmprighe
GO
/****** Object:  StoredProcedure [dbo].[NumeraDoc]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[NumeraDoc]  @Tipo as Varchar(1), @Numrif as int
as

Declare @UltNumero as int,@Data as smalldatetime,@Reg as smallint, @Cau as smallint, @NUMERO AS INT, @DEP as smallint

if @Tipo = 'F'
		begin

			SELECT @DEP =FatNDep, @data=FatData, @Reg=FatNumReg, @NUMERO=FatNum
			from TbFat where FatRif = @numrif

			IF @NUMERO = 999999
			   BEGIN

			   IF @Reg = 0
					BEGIN
						set @UltNumero = ( select isnull(max(PFatNumero),0) from TbProforma
									where PFatAnno = datepart(year,@data))

								INSERT INTO TbProforma
								SELECT @Numrif, @UltNumero+1, @Data,datepart(year,@data)
				END

			ELSE


				BEGIN
					set @UltNumero = ( select isnull(max(FatNum),0) from TbFat
							   where fatNum < 999999 and fatTipoDoc = @Tipo and FatNDep = @DEP and FatNumreg = @reg and datepart(year,fatdata) = datepart(year,@data))

				END
   
		   update Tbfat set fatNum = @UltNumero + 1 where FatRif = @Numrif

		   SELECT @UltNumero + 1
		   END
		 ELSE
		   BEGIN
		   SELECT @NUMERO
		   END

		end
else if (@Tipo = 'S' or @tipo = 'P')
		begin

		SELECT @DEP =SosNdep, @data=SosData 
		from TbSos where Sosrif = @numrif

		set @UltNumero = ( select isnull(max(SosNum),0) from TbSos
		where SosNum < 999999 and SosTipoDoc = @Tipo and SosNdep = @DEP and datepart(year,Sosdata) = datepart(year,@data))

		update TbSos set SosNum = @UltNumero + 1 where SosRif = @Numrif

		SELECT @UltNumero + 1
 
		end
else if @Tipo = 'B'
		begin

		SELECT @DEP =BolNdep, @data=BolData 
		from TbBol where Bolrif = @numrif


		update TbBol set BolNum = ( select isnull(max(BolNum),0) from TbBol
		where BolNum < 999999 and BolTipoDoc = @Tipo  and BolNDep = @DEP and datepart(year,Boldata) = datepart(year,@data))+ 1 where BolRif = @Numrif

		set @UltNumero = (Select BolNum from TbBol where BolRif = @Numrif)

		SELECT @UltNumero 
 
		end

else if @Tipo = 'T'
		begin

		SELECT @data=OrdtData
		from TbOrdTorte where OrdtRif = @numrif


		update TbOrdTorte set OrdtNum = ( select isnull(max(OrdtNum),0) from TbOrdTorte
		where OrdtNum < 999999 and datepart(year,OrdtData) = datepart(year,OrdtData)) + 1 where OrdtRif = @Numrif

		set @UltNumero = (Select OrdtNum from TbOrdTorte where OrdtRif = @Numrif)

		SELECT @UltNumero 
 
		end

else 

		begin
		SELECT @DEP =TesNdep, @data=TesData, @cau=TesCau
		from TbTes where tesrif = @numrif

		set @UltNumero = ( select isnull(max(TesNum),0) from TbTes
		where TesNum < 999999 and TesTipoDoc = @Tipo and TesCau=@Cau and TesNdep = @DEP and datepart(year,Tesdata) = datepart(year,@data))

		update TbTes set TesNum = @UltNumero + 1 where TesRif = @Numrif

		SELECT @UltNumero + 1
 
		end

GO
/****** Object:  StoredProcedure [dbo].[NumeraDocBolFat]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[NumeraDocBolFat]
    @Tipo   VARCHAR(1),        -- 'B' = Bolla / 'F' = Fattura
    @NumRif INT,               -- PK della testata da aggiornare (BolRif o FatRif)
    @Numero INT,               -- Nuovo numero documento scelto manualmente
    @Data   SMALLDATETIME,     -- Nuova data documento
    @Dep    SMALLINT           -- Deposito
AS
BEGIN
    SET NOCOUNT ON;            -- evita messaggi "X rows affected"

    -- Variabili di lavoro
    DECLARE
        @ERRORE           SMALLINT = 0,   -- 0 = ok, 1/2/3
        @DataMaxPossibile SMALLDATETIME,  -- limite superiore consentito per la data
        @DataMinPossibile SMALLDATETIME,  -- limite inferiore consentito per la data
        @Minimo_Anno      DATE;           -- 01/01 dell'anno della data inserita

    -- 01/01 dell'anno della data scelta: usato come fallback se non ci sono documenti "precedenti"
    SET @Minimo_Anno = DATEFROMPARTS(DATEPART(YEAR, @Data), 1, 1);

    ----------------------------------------------------------------------
    -- 1) RAMO BOLLA / DDT
    ----------------------------------------------------------------------
    IF (@Tipo = 'B')
    BEGIN
        -- Calcolo della data massima consentita:
        -- prendo il documento con numero subito "dopo" (@Numero) e uso la sua data come limite.
        -- escludo 999999.
        SET @DataMaxPossibile = ISNULL(
            (SELECT TOP 1 BolData
             FROM TbBol
             WHERE BolNDep = @Dep
               AND DATEPART(YEAR, BolData) = DATEPART(YEAR, @Data)
               AND BolNum > @Numero
               AND BolNum <> 999999
             ORDER BY BolNum),
            GETDATE()
        );

        -- Calcolo della data minima consentita:
        -- prendo il documento con numero subito "prima" (@Numero) e uso la sua data come limite.
        SET @DataMinPossibile = ISNULL(
            (SELECT TOP 1 BolData
             FROM TbBol
             WHERE BolNDep = @Dep
               AND DATEPART(YEAR, BolData) = DATEPART(YEAR, @Data)
               AND BolNum < @Numero
             ORDER BY BolNum DESC),
            @Minimo_Anno
        );

		---- CONTROLLO IL NUMERO
		IF (SELECT COUNT(*) FROM TbBol WHERE  BolNDep = @Dep AND BolNum = @Numero AND DATEPART(YEAR,BolData) = DATEPART(YEAR,@Data)) <> 0
			BEGIN
				SET @ERRORE = 1
				GOTO FINE
			END
   
		----CONTROLLO LA DATA MAX
		IF @DATA > @DataMaxPossibile
			BEGIN
				SET @ERRORE = 2
				GOTO FINE
			END
    
		----CONTROLLO LA DATA MIN
		IF @DATA < @DataMinPossibile
			BEGIN
				SET @ERRORE = 3
				GOTO FINE
			END 

        --
        UPDATE TbBol SET BolNum = @Numero, BolData = @Data
		WHERE BolRif = @NumRif

        -- Alla fine del ramo, salta a FINE per restituire @ERRORE
        GOTO FINE;
    END

    ----------------------------------------------------------------------
    -- 2) RAMO FATTURA
    ----------------------------------------------------------------------
    ELSE IF (@Tipo = 'F')
    BEGIN
        -- Stessa logica delle bolle, ma su TbFat e colonne Fat*
        SET @DataMaxPossibile = ISNULL(
            (SELECT TOP 1 FatData
             FROM TbFat
             WHERE FatNDep = @Dep
               AND DATEPART(YEAR, FatData) = DATEPART(YEAR, @Data)
               AND FatNum > @Numero
               AND FatNum <> 999999
             ORDER BY FatNum),
            GETDATE()
        );

        SET @DataMinPossibile = ISNULL(
            (SELECT TOP 1 FatData
             FROM TbFat
             WHERE FatNDep = @Dep
               AND DATEPART(YEAR, FatData) = DATEPART(YEAR, @Data)
               AND FatNum < @Numero
             ORDER BY FatNum DESC),
            @Minimo_Anno
        );

		---- CONTROLLO IL NUMERO
		IF (SELECT COUNT(*) FROM TbFat WHERE  FatNDep = @Dep AND FatNum = @Numero AND DATEPART(YEAR,FatData) = DATEPART(YEAR,@Data)) <> 0
			BEGIN
				SET @ERRORE = 1
				GOTO FINE
			END
   
		----CONTROLLO LA DATA MAX
		IF @DATA > @DataMaxPossibile
			BEGIN
				SET @ERRORE = 2
				GOTO FINE
			END
    
		----CONTROLLO LA DATA MIN
		IF @DATA < @DataMinPossibile
			BEGIN
				SET @ERRORE = 3
				GOTO FINE
			END 

        --
        UPDATE TbFat SET FatNum = @Numero, FatData = @Data
		WHERE FatRif = @NumRif

        GOTO FINE;
    END

    ----------------------------------------------------------------------
    -- 3) Tipo documento non valido, quindi <> da F o B
    ----------------------------------------------------------------------
    ELSE
    BEGIN
        -- codice errore extra
        SET @ERRORE = 9;
    END

    ----------------------------------------------------------------------
    -- 4) Uscita unica: una sola label, un solo SELECT finale
    ----------------------------------------------------------------------
    FINE:
    SELECT @ERRORE;
END
GO
/****** Object:  StoredProcedure [dbo].[NumeraDocM]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   PROC [dbo].[NumeraDocM]  @Tipo as Varchar(1), @Numrif as int, @Numero as int, @Data as smalldatetime, @DEP as smallint
as


DECLARE @ERRORE AS SMALLINT, @DATAMAXPOSSIBILE AS SMALLDATETIME, @DATAMINPOSSIBILE AS SMALLDATETIME, @MINIMO_ANNO AS DATE

SET @MINIMO_ANNO = DATEFROMPARTS(DATEPART(YEAR,@DATA),1,1)

set @ERRORE = 0

SET @DATAMAXPOSSIBILE =isnull((SELECT TOP 1 BolData FROM TbBol where BolNDep = @DEP AND DATEPART(YEAR,Boldata) = DATEPART(year,@data) and BolNum > @Numero and bolnum <> 999999 order by bolnum),GETDATE())
SET @DATAMINPOSSIBILE =isnull((SELECT TOP 1 BolData FROM TbBol where BolNDep = @DEP AND DATEPART(YEAR,Boldata) = DATEPART(year,@data) and BolNum < @Numero order by bolnum DESC),@MINIMO_ANNO)


---- CONTROLLO IL NUMERO

IF (SELECT COUNT(*) FROM TbBol WHERE  BolNDep = @DEP AND BolNum = @Numero AND DATEPART(YEAR,Boldata) = DATEPART(year,@data)) <> 0
   BEGIN
   SET @ERRORE = 1
   GOTO FINE
   END
   
----CONTROLLO LA DATA  
IF @DATA > @DATAMAXPOSSIBILE
   BEGIN
   SET @ERRORE = 2
   GOTO FINE
   END
    
   ----CONTROLLO LA DATA  
IF @DATA < @DATAMINPOSSIBILE
   BEGIN
   SET @ERRORE = 3
   GOTO FINE
   END 


Update TbBol set BolNum = @Numero, BolData = @Data
WHERE BolRif = @Numrif


FINE:
SELECT @ERRORE


GO
/****** Object:  StoredProcedure [dbo].[SlegaFattura]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROC [dbo].[SlegaFattura] @NUMRIF AS INT, @TERMINALE AS VARCHAR(20)
as


select * into #tmpfat from TbFat where FatRif = @NUMRIF

delete from TbFat where FatRif = @NUMRIF

delete from TbCor where CorTipoDoc = 'F' AND CorRif = @NUMRIF

delete from TbDcg where DcgNumrif = @NUMRIF

DELETE FROM TbFte where FteRif = @NUMRIF

delete from TBBLOCK where IDRIFERIM = @NUMRIF 

Update TbBol set BolRifFat = 0, BolPrefAgg = 0 where BolRifFat = @NUMRIF

delete FROM COGE.DBO.TbEff where RicRifFat  = @NUMRIF

Insert Into TbDFA (DFatNumReg,DFatNum,DFatData,DFatTrasf,DFatTerminale)
select FatNumReg,FatNum,FatData,isnull(FatTrasf,cast(0 as bit)),@TERMINALE
from #tmpfat  




GO
/****** Object:  StoredProcedure [dbo].[SospToBoll]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE   PROC [dbo].[SospToBoll]  @Tipo as Varchar(1), @Numrif as int, @progr as int, @Cau as smallint,@SospRif as int, @DATADOC AS SMALLDATETIME
as

DECLARE  @ARTID AS INT, @CAUSALE AS SMALLINT, @QTA AS DECIMAL(7,2), @IMPORTO AS DECIMAL(9,2), @SEGNO AS SMALLINT , @DEP AS SMALLINT, @VARDEST AS VARCHAR(100),@NOTE as varchar(50)

SET @DEP = (SELECT SosNdep from TbSos where Sosrif = @SospRif)

set @NOTE = (SELECT SosNote from tbsos where SosRif=@SOSPRIF)

SET @VARDEST = case WHEN @Tipo = 'B' THEN (SELECT BolVardest from Tbbol where Bolrif = @Numrif) 
                    WHEN  @Tipo = 'F' THEN (SELECT FatVardest from TbFat where Fatrif = @Numrif)
					ELSE '' END

---- AGGIORNO TESTATA NOTE
IF @TIPO='B'
BEGIN
UPDATE TBBOL SET BOLNOTE = @NOTE where Bolrif=@Numrif
END

IF @TIPO='F'
BEGIN
UPDATE TBFAT SET FATNOTE = @NOTE where Fatrif=@Numrif
END

---- DISAGGIORNO IL MAGAZZINO PER IL DOCUMENTO S E LO AGGIORNO CON LA NUOVA CAUSALE
--DECLARE RIGHE CURSOR FOR
--select CORARTID,CORQUACON,CORIMPORTO,CORCAU 
--from TbCor where CorTipoDoc = 'S' and CorRif = @SospRif  AND CorArtId <> 0

--OPEN RIGHE
--FETCH NEXT FROM RIGHE
--INTO  @ARTID, @QTA, @IMPORTO, @CAUSALE

--WHILE @@FETCH_STATUS = 0
--      BEGIN

	
--	  SET @SEGNO = -1
--	  EXEC X_AGG_QTAVAL @DEP, @DATADOC,@ARTID,@CAUSALE,@QTA,@IMPORTO,@SEGNO

--	  SET @SEGNO = 1
--	  EXEC X_AGG_QTAVAL @DEP, @DATADOC,@ARTID,@Cau,@QTA,@IMPORTO,@SEGNO


--	  FETCH NEXT FROM RIGHE
--      INTO  @ARTID, @QTA, @IMPORTO, @CAUSALE
--	  END


--CLOSE RIGHE
--DEALLOCATE RIGHE


-------- CREO LE RIGHE DI MOVIMENTO

select @Tipo AS CorTipoDoc,@Numrif as CorRif,CorProg = IDENTITY(int, 1 ,1) ,CorArtId,CorCodArt,CorDesc,CorQuaCon,CorPrezzo,CorSc1,CorSc2,CorSc3,CorSc4,CorSc5,
       CorNetto,CorImporto,CorUrgenza,CorSpot,CorPromo,CorAssortito,CorVariato,CorCampagna,CorCiva,CorCntrp,@Cau as CorCau,CorMacro,
       CorOrdRif,CorOrdine,CorArtFor,CorPlRif,CorPlCassa,CorImballo,CorMg1,CorMg2,CorMg3,CorMerc,CorBrand,CorOrdProg,CorUM 
into #tmp
from TbCor where CorTipoDoc = 'S' and CorRif = @SospRif

select @Tipo AS SCorTipoDoc,@Numrif as SCorRif,SCorProg = IDENTITY(int, 1 ,1),CorTipoDoc,CorRif,CorProg 
into #tmpSCor
from TbCor where CorTipoDoc = 'S' and CorRif = @SospRif

insert into TbCor (CorTipoDoc, CorRif, CorProg, CorArtID, CorCodArt, CorDesc, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorSc4, CorSc5, CorNetto, CorImporto, CorUrgenza, CorSpot, CorPromo, CorAssortito, CorVariato, CorCampagna, 
                   CorCiva, CorCntrp, CorCau, CorMacro, CorOrdRif, CorOrdine, CorArtFor, CorPlRif, CorPlCassa, CorImballo, CorMg1, CorMg2, CorMg3, CorMerc, CorBrand, CorOrdProg, CorUM)
select  CorTipoDoc, CorRif,CorProg = CorProg + @progr ,CorArtId,CorCodArt,CorDesc,CorQuaCon,CorPrezzo,CorSc1,CorSc2,CorSc3,CorSc4,CorSc5,
       CorNetto,CorImporto,CorUrgenza,CorSpot,CorPromo,CorAssortito,CorVariato,CorCampagna,CorCiva,CorCntrp,CorCau,CorMacro,
       CorOrdRif,CorOrdine,CorArtFor,CorPlRif,CorPlCassa,CorImballo,CorMg1,CorMg2,CorMg3,CorMerc,CorBrand,CorOrdProg,CorUM   
from #tmp

Insert into TbSCor 
select SCorTipoDoc,SCorRif,SCorProg = SCorProg + @progr,CorTipoDoc,CorRif,CorProg 
from #tmpSCor


---AGGIORNO IL DOCUMENTO S

Update TbSos set SosTrasf = 1 where SosRif = @SospRif


Insert Into TmpSCor select * from TbCor where CorTipoDoc = 'S' and CorRif = @SospRif 

Delete from TbCor where CorTipoDoc = 'S' and CorRif = @SospRif

---AGGIORN EVENTUALI DATI DI TESTATA

IF @VARDEST = ''
   BEGIN
   IF @TIPO = 'B'
      BEGIN
	  UPDATE TbBol set BolPvv = isnull((Select SosPvv from TbSos where SosRif = @SospRif),'') where Bolrif = @NUMRIF
	  END
	ELSE
	  BEGIN
	  UPDATE TbFat set FatPvv = isnull((Select SosPvv from TbSos where SosRif = @SospRif),'') where FatRif = @NUMRIF and fatTipodoc = @TIPO
	  END
	END


---- RESITUISCO IL PROGRESSIVO RIGA RICALCOLATO

select isnull(max(CorProg),0) from TbCor where CorTipoDoc = @Tipo and CorRif = @Numrif





GO
/****** Object:  StoredProcedure [dbo].[TMP2FTE_PASSIVE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[TMP2FTE_PASSIVE] @TERM VARCHAR(30)
AS

Insert into TbFte_Passiva ( FteNomeFile, FteIdBN, FteIdSDI, FteDataRicezione, FteDataConsegnaSDI, FteProgSIA, FtePartIva, FteRagSoc,FteAnno,FteNumero)
SELECT NOMEFILE,ARCHIVEID,SDID,CREATEDTIME,RECEPTIONTIME,0,'','',0,''
FROM TMP_FTE_PASSIVA
WHERE TERM = @TERM AND SDID NOT IN (SELECT FteIdSDI FROM TbFte_Passiva)
GO
/****** Object:  StoredProcedure [dbo].[UPCOGE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[UPCOGE] @BLOK int
as
Update TbFat set FatTrasf = 1 where FatRif in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK )
Update TbDcg set DcgTrasf = 1 where DcgNumRif in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK)
Update TbBol set BolTrasf = 1 where BolRifFat in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK)
DELETE from TbBLOCK where IDRIFERIM in (select IDRIFERIM from TbBLOCK where IDBLOCCO =  @BLOK) and IDBLOCCO <> @BLOK 
Delete from TbBLOCK where IDBLOCCO =  @BLOK 
GO
/****** Object:  StoredProcedure [dbo].[UpgSculter]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[UpgSculter] @Numrif as int, @Tipo as varchar(1)
as

declare @Sculter as decimal(5,2), @Clie as varchar(5)

if @Tipo = 'F'
   begin
   SET @Sculter = (select FatSculter from TbFat where FatRif = @Numrif)
   set @Clie = (select FatCliCons from TbFat where FatRif = @Numrif)
   end
else if @Tipo = 'S'
 begin
   SET @Sculter = (select SosSculter from TbSos where SosRif = @Numrif)
   set @Clie = (select SosCliCons from TbSos where SosRif = @Numrif)
   end
else if @Tipo = 'P'
 begin
   SET @Sculter = (select SosSculter from TbSos where SosRif = @Numrif)
   set @Clie = (select SosCliCons from TbSos where SosRif = @Numrif)
   end   
else
   begin
   set @sculter = (select BolSculter from TbBol where BolRif = @Numrif)
   set @Clie = (select BolCliCons from TbBol where BolRif = @Numrif)
   end

Update TbCor set CorNetto = CorPrezzo * (1 - CorSc1/100) * (1 - CorSc2 / 100) * (1 - CorSc3 / 100) * (1 - @SCULTER / 100) 
             where CorRif =@Numrif and CorTipoDoc = @Tipo

Update TbCor set CorImporto = CorNetto * CorQuaCon                
             where CorRif =@Numrif and CorTipoDoc = @Tipo



--EXEC UpgImballo @Numrif,@Tipo,@Clie


GO
/****** Object:  StoredProcedure [dbo].[X_AGGIORNA_PLU]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_AGGIORNA_PLU]
AS

Update TbArt set ArtCodBil = 0

Update TbArt set ArtCodbil = IDPLU
from TbArt inner join
     TMP_PLU ON tmpartid = ArtId

TRUNCATE TABLE TMP_PLU
GO
/****** Object:  StoredProcedure [dbo].[X_CAL_IMPO_SOGLIA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[X_CAL_IMPO_SOGLIA] @TIPODOC AS VARCHAR(1), @RIFER AS INT
AS

SELECT isnull(SUM(CORIMPORTO),0)
FROM TbCor INNER JOIN
     TbArt on ArtId = CorArtId
WHERE Cortipodoc = @TIPODOC and Corrif = @RIFER
      AND (ArtCat = 'CO' OR ArtCat = 'PA')
GO
/****** Object:  StoredProcedure [dbo].[X_CAMBIA_PREZZI_DOC]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[X_CAMBIA_PREZZI_DOC] @TIPODOC AS VARCHAR(1), @NUMRIF AS INT, @CLIENTE AS VARCHAR(5), @LISTINO AS SMALLINT = 0
AS

declare @OLD_CLIENTE AS VARCHAR(5), @OLD_GRUPAT AS SMALLINT, @GRUPAT AS SMALLINT, @ESP_SCONTI AS VARCHAR(1), @OLD_ESP_SCONTI AS VARCHAR(1), @OLD_LISTINO AS SMALLINT


SELECT @GRUPAT = ClGrCanale, @ESP_SCONTI = ClEspSconti
from TbCli
WHERE CLCOD = @CLIENTE

IF @TIPODOC = 'F'
   BEGIN
     select @OLD_CLIENTE  = FatCliFat, @OLD_LISTINO = FatListino
	 from TbFat where fatrif = @NUMRIF
   END
ELSE
   BEGIN
     select @OLD_CLIENTE  = BolCliFat, @OLD_LISTINO = BolListino
	 from TbBol where Bolrif = @NUMRIF  
   END

SELECT @OLD_GRUPAT = ClGrCanale, @OLD_ESP_SCONTI = ClEspSconti
from TbCli
WHERE CLCOD = @OLD_CLIENTE

IF @OLD_LISTINO = 0
   begin
     set @OLD_LISTINO = @OLD_GRUPAT
   end

IF @LISTINO = 0
   begin
     set @LISTINO = @GRUPAT
   end


---------------SE IL CLIENTE E' IL MEDESIMO ESCO
IF @CLIENTE = @OLD_CLIENTE  and @LISTINO = @OLD_LISTINO
   BEGIN
     GOTO ESCI
   END


---------------------------- comincio ad azzerare gli sconti nel caso debbano essere ricalcolati
UPDATE TbCor SET CorSc1 = 0
WHERE CorTipodoc = @TIPODOC and CorRif = @NUMRIF


-------------------------- carico eventuali sconti nuovi (SE IL CLIENTE è GESTITO A SCONTI)
IF @ESP_SCONTI = 'S'
	    BEGIN
		     -------------------CERCO SCONTISTICA PER CLIENTE
			 UPDATE TbCor
			  SET CorSc1 = FasSc1
			  FROM TbCor INNER JOIN
				   TbNewFascli on FasCli = @CLIENTE and FasArtId = CorArtId 
			  WHERE CorTipodoc = @TIPODOC and CorRif = @NUMRIF
			  AND FasCategoria = '' and CorSc1 = 0

             UPDATE TbCor
		      SET CorSc1 = FasSc1
			  FROM TbCor INNER JOIN
				   TbNewFascli on FasCli = @CLIENTE and FasCategoria = (SELECT ArtCat from TbArt WHERE ArtId = CorArtId)
			  WHERE CorTipodoc = @TIPODOC and CorRif = @NUMRIF
			  AND FasArtId = 0 and CorSc1 = 0
			  
		

		 END

----------------------------CERCO I PREZZI IN BASE AL GRUPPO ATTIVITA E/O IL CLIENTE (SOLO SE HA PREZZI PERSONALIZZATI)

update TbCor
set CorNetto = SlAttPrezzo, CorPrezzo = SlAttPrezzo
from TbCor INNER JOIN
    TbSlAtt on SlAtt = @LISTINO and SlArtCod = CorCodArt
WHERE CorTipodoc = @TIPODOC and CorRif = @NUMRIF

IF @ESP_SCONTI = 'N'
   BEGIN

		update TbCor
		set CorNetto = SlCliPrezzo, CorPrezzo = SlCliPrezzo
		from TbCor INNER JOIN
			TbSlCli on SlCli = @CLIENTE and SlArtCod = CorCodArt
		where SlNo = 0
		and CorTipodoc = @TIPODOC and CorRif = @NUMRIF

   END

-----aggiorno le righe per effetto del cambio prezzi e/o sconti

UPDATE TBCOR SET CorNetto = Corprezzo * (1 - CorSc1 / 100),
				CorImporto = Corprezzo * (1 - CorSc1 / 100) * CorQuaCon
WHERE CorTipodoc = @TIPODOC and CorRif = @NUMRIF


ESCI:
GO
/****** Object:  StoredProcedure [dbo].[X_CAMBIA_PREZZI_DOC_LISTINO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[X_CAMBIA_PREZZI_DOC_LISTINO]	@TIPODOC AS VARCHAR(1), 
												@NUMRIF AS INT,	
												@LISTINO  SMALLINT = 0
AS

BEGIN
    SET NOCOUNT ON;

DECLARE @L SMALLINT;

-- Se non passato, lo leggo dalla testata
    IF @LISTINO = 0
    BEGIN
        IF @TIPODOC = 'F'
            SELECT @L = ISNULL(FatListino, 0) FROM TbFat WHERE FatRif = @NUMRIF;
        ELSE
            SELECT @L = ISNULL(BolListino, 0) FROM TbBol WHERE BolRif = @NUMRIF;
    END
    ELSE
        SET @L = @LISTINO;

    -- Se non c’è listino, esco
    IF ISNULL(@L, 0) = 0
        RETURN;

    -- Aggiorno prezzo da listino (solo dove esiste la riga listino/articolo)
    UPDATE C
        SET C.CorPrezzo = S.SlAttPrezzo,
            C.CorNetto  = S.SlAttPrezzo
    FROM TbCor C
    INNER JOIN TbSlAtt S
        ON S.SlAtt = @L
       AND S.SlArtCod = C.CorCodArt
    WHERE C.CorTipodoc = @TIPODOC
      AND C.CorRif     = @NUMRIF;

    -- Ricalcolo netto e importo usando lo sconto già presente
    UPDATE TbCor
       SET CorNetto   = CorPrezzo * (1 - CorSc1 / 100.0),
           CorImporto = CorPrezzo * (1 - CorSc1 / 100.0) * CorQuaCon
     WHERE CorTipodoc = @TIPODOC
       AND CorRif     = @NUMRIF;
END
GO
/****** Object:  StoredProcedure [dbo].[X_CARICA_ECCEZIONI_ATTIVE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[X_CARICA_ECCEZIONI_ATTIVE]
AS
BEGIN
    -- 1. Recuperiamo il prossimo ParentId (Max + 1)
    DECLARE @NextParentId INT;
    SELECT @NextParentId = ISNULL(MAX(ParentId), 0) + 1 FROM TbEccezioniChiusura;

    -- 2. Selezione dei dati raggruppati
    SELECT 
        ParentId,
        DataInizio, 
        DataFine, 
        CodiceEccezione, -- 1=Chiuso, 2=Speciale
        -- Appiattimento degli slot (Mattina)
        MAX(CASE WHEN Slot = 1 THEN OraInizio END) AS MattinoInizio,
        MAX(CASE WHEN Slot = 1 THEN OraFine END) AS MattinoFine,
        -- Appiattimento degli slot (Pomeriggio)
        MAX(CASE WHEN Slot = 2 THEN OraInizio END) AS PomeriggioInizio,
        MAX(CASE WHEN Slot = 2 THEN OraFine END) AS PomeriggioFine,
        Note,
        @NextParentId AS SuggeritoNextId -- Utile per il nuovo inserimento
    FROM TbEccezioniChiusura
    WHERE DataFine >= CAST(GETDATE() AS DATE) -- Visualizza solo eventi attivi o futuri
    GROUP BY ParentId, DataInizio, DataFine, CodiceEccezione, Note
    ORDER BY DataInizio ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[X_CARICA_PLU]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_CARICA_PLU]
AS

TRUNCATE TABLE TMP_PLU

INSERT INTO TMP_PLU  
SELECT ArtCodBil,ArtId
FROM TbArt 
where ArtBilancia = 1
ORDER BY ArtCodBil


SELECT IDPLU,
       ArtId,
       Artdesc = case when ArtMinidesc > '' then ArtMinidesc else ArtDesc end
FROM TbArt inner join
     TMP_PLU ON tmpartid = ArtId
ORDER BY IDPLU
GO
/****** Object:  StoredProcedure [dbo].[X_CREA_STAMPA_IMBALLI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   proc [dbo].[X_CREA_STAMPA_IMBALLI] @TERM VARCHAR(20), @ARTCOD VARCHAR(20), @ETICHE SMALLINT, @LOTTO  VARCHAR(20), @SCADENZA VARCHAR(10)
AS

DECLARE  @DESCRIZIONE VARCHAR(100), @I AS INTEGER, @NUMIMBALLO AS SMALLINT

SELECT  @DESCRIZIONE = ArtDesc, @NUMIMBALLO = ArtNumImballo
FROM TbArt
WHERE ArtCod = @ARTCOD



SET @I = 0

DELETE FROM TMP_IMBALLO WHERE TERM = @TERM

WHILE @I < @ETICHE
     BEGIN
	    SET @I = @I + 1

		INSERT INTO TMP_IMBALLO
		SELECT @TERM, @ARTCOD, @DESCRIZIONE, @NUMIMBALLO, @LOTTO,  @SCADENZA

	 END


GO
/****** Object:  StoredProcedure [dbo].[X_CREA_STAMPA_INGREDIENTI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   proc [dbo].[X_CREA_STAMPA_INGREDIENTI] @TERM VARCHAR(20), @ARTCOD VARCHAR(20), @ETICHE SMALLINT, @LOTTO  VARCHAR(20), @SCADENZA VARCHAR(10), @LINGUA SMALLINT
AS

DECLARE @TESTO VARBINARY(MAX), @EAN13 VARCHAR(13), @DESCRIZIONE VARCHAR(100), @I AS INTEGER, @DIBASE AS SMALLINT, @BARCODE AS VARCHAR(50)

SELECT @DIBASE = ArtDiBase, @EAN13 = ArtEan13, @DESCRIZIONE = ArtDesc, @BARCODE = ArtBarcode
FROM TbArt
WHERE ArtCod = @ARTCOD



IF @LINGUA = 1
	BEGIN 
		SET @TESTO = (SELECT TDisItaliano FROM TbTDis WHERE TDisID = @DIBASE)
	END

ELSE IF @LINGUA = 2
	BEGIN 
	SET @TESTO = (SELECT TDisFrancese FROM TbTDis WHERE TDisID = @DIBASE)
	END

ELSE IF @LINGUA = 3
	BEGIN 
	SET @TESTO = (SELECT TDisInglese FROM TbTDis WHERE TDisID = @DIBASE)
	END

ELSE IF @LINGUA = 4
	BEGIN 
	SET @TESTO = (SELECT TDisTedesco FROM TbTDis WHERE TDisID = @DIBASE)
	END



SET @I = 0

DELETE FROM TMP_ETICHETTE WHERE TERM = @TERM

WHILE @I < @ETICHE
     BEGIN
	    SET @I = @I + 1

		INSERT INTO TMP_ETICHETTE
		SELECT @TERM, @TESTO, @EAN13, @ARTCOD, @LOTTO, @SCADENZA, @DESCRIZIONE, @BARCODE

	 END


GO
/****** Object:  StoredProcedure [dbo].[X_CREA_STAMPA_TORTE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_CREA_STAMPA_TORTE] @TERM AS VARCHAR(20), @STAMPANUM AS INT = 0, @ORDRIF AS INT = 0
AS

IF @ORDRIF = 0
   BEGIN 
			IF @STAMPANUM = 0
				   BEGIN
						SET  @STAMPANUM = (SELECT MAX(SosStampaNum) from TbSos) + 1
						SELECT * 
						INTO #TMP
						FROM TbSos INNER JOIN
							 TbCor on CorTipoDoc = SosTipoDoc and CorRif = SosRif INNER JOIN
							 TbPrivati on PrivId = SosPrivato
						WHERE SosStampato = 0 and SosPrivato <> 0
						Order by SosRif

						DELETE FROM TMP_ORD_TORTE WHERE TERM = @TERM

						INSERT INTO TMP_ORD_TORTE (TERM, SOSTIPODOC, SOSRIF, SOSNUM, SOSDATA, COGNOMENOME, TELEFONO, MAIL, SOSCONSEGNA, SOSORARITIRO, CORANNOTA)
						SELECT @TERM, SOSTIPODOC, SOSRIF, SOSNUM, SOSDATA, PrivCognomeNome, PrivTelefono, PrivMail, SOSCONSEGNA, SOSORARITIRO,CORANNOTA
						FROM #TMP

						update TbSos set SosStampato = 1, SosStampaNum = @STAMPANUM
						WHERE SosRif in (SELECT SosRif from #TMP)

						SELECT COUNT(*) FROM  #TMP
				   END
				ELSE
				   BEGIN
      					SELECT * 
						INTO #TMPR
						FROM TbSos INNER JOIN
							 TbCor on CorTipoDoc = SosTipoDoc and CorRif = SosRif INNER JOIN
							 TbPrivati on PrivId = SosPrivato
						WHERE SosStampaNum = @STAMPANUM and SosPrivato <> 0
						Order by SosRif

						DELETE FROM TMP_ORD_TORTE WHERE TERM = @TERM

						INSERT INTO TMP_ORD_TORTE (TERM, SOSTIPODOC, SOSRIF, SOSNUM, SOSDATA, COGNOMENOME, TELEFONO, MAIL, SOSCONSEGNA, SOSORARITIRO, CORANNOTA)
						SELECT @TERM, SOSTIPODOC, SOSRIF, SOSNUM, SOSDATA, PrivCognomeNome, PrivTelefono, PrivMail, SOSCONSEGNA, SOSORARITIRO,CORANNOTA
						FROM #TMPR

						SELECT COUNT(*) FROM  #TMPR
				   END

   END
ELSE 
  BEGIN
		SET  @STAMPANUM = (SELECT MAX(SosStampaNum) from TbSos) + 1
		SELECT * 
		INTO #TMPS
		FROM TbSos INNER JOIN
				TbCor on CorTipoDoc = SosTipoDoc and CorRif = SosRif INNER JOIN
				TbPrivati on PrivId = SosPrivato
		WHERE SosRif = @ORDRIF

		DELETE FROM TMP_ORD_TORTE WHERE TERM = @TERM

		INSERT INTO TMP_ORD_TORTE (TERM, SOSTIPODOC, SOSRIF, SOSNUM, SOSDATA, COGNOMENOME, TELEFONO, MAIL, SOSCONSEGNA, SOSORARITIRO, CORANNOTA)
		SELECT @TERM, SOSTIPODOC, SOSRIF, SOSNUM, SOSDATA, PrivCognomeNome, PrivTelefono, PrivMail, SOSCONSEGNA, SOSORARITIRO,CORANNOTA
		FROM #TMPS

		update TbSos set SosStampato = 1, SosStampaNum = @STAMPANUM
		WHERE SosRif = @ORDRIF

		SELECT COUNT(*) FROM  #TMPS
	END

GO
/****** Object:  StoredProcedure [dbo].[X_DOCUMENTI_BANCO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE   proc [dbo].[X_DOCUMENTI_BANCO]  @TIPODOC as varchar(1), @ANNOLAVORO AS SMALLINT, @DEP AS SMALLINT, @NREG AS SMALLINT
AS


declare @REGFAT AS SMALLINT

SET @REGFAT = (SELECT top 1 TaiReg2 from TbTai order by TaiAnno desc)

---   PREPARAZIONE TABELLA VUOTA 
CREATE TABLE [#TMPDOC] (
	[CLIFOR] VARCHAR(5) NOT NULL ,
	[ANADESC] varchar(100) NULL ,
	[TIPOD] varchar(20) NOT NULL ,
	[TESRIF] INT NOT NULL ,
	[TESNUM] INT NOT NULL ,
	[TESDATA] SMALLDATETIME NOT NULL ,
	[CAU] SMALLINT NOT  NULL ,
    [CAUDESC] varchar(150) NOT NULL ,
	[TRASFORMATO] varchar(85) NOT NULL DEFAULT '',
	[SosFP] bit NOT NULL DEFAULT(0),
	[Registro] SMALLINT NOT  NULL ,
	) ON [PRIMARY]

IF @TIPODOC = 'B'
   BEGIN
   insert into #TMPDOC
   select top 500 BolCliCons,Anadesc,
                  TIPOD = @TIPODOC ,
                  BolRif,BolNum,BolData,BolCau,MgCaudesc=MgCaudesc + ' ' + BolNote,'',0,BolNumReg
   from TbBol inner join
        vdox.dbo.TbAna on AnaCod = BolCliCons inner join
        TbTcau on MgCauId = BolCau
   where BolNDep = @DEP AND Bolnum <> 999999 AND DATEPART(YEAR,BOLDATA) = @ANNOLAVORO
   order by BolRif desc
   GOTO FINE
   END

IF @TIPODOC = 'F'
   BEGIN
   insert into #TMPDOC
   select top 500 FatCliCons,Anadesc,
                  TIPOD = @TIPODOC ,
                  FatRif,FatNum,FatData,Fatcau,
				  MgCaudesc=CASE WHEN fatnumreg = 0 THEN MgCaudesc + '- PROFORMA ' + FatNote ELSE MgCaudesc + ' ' + FatNote END,
				  '',0,Fatnumreg
   from TbfAT inner join
        vdox.dbo.TbAna on AnaCod = FatCliCons inner join
        TbTcau on MgCauId = FatCau
   where FatNDep = @DEP AND Fatnum <> 999999 AND DATEPART(YEAR,FATDATA) = @ANNOLAVORO and (fatnumreg = @NREG or (fatnumreg = 0 and @nreg = @REGFAT))
   order by FatRif desc
   GOTO FINE
   END

IF @TIPODOC = 'S'
   BEGIN
   insert into #TMPDOC
   select top 500 SosCliCons,Anadesc,
                  TIPOD = @TIPODOC ,
                 ---- SosRif,SosNum,SosData,Soscau,MgCaudesc=MgCaudesc + ' ' + SosNote,'',0
				  SosRif,SosNum,SosData,Soscau,MgCaudesc=MgCaudesc ,'',0,0
   from TbSos inner join
        vdox.dbo.TbAna on AnaCod = SosCliCons inner join
        TbTcau on MgCauId = Soscau 
   where SosNDep = @DEP AND Sosnum <> 999999 AND DATEPART(YEAR,SosDATA) = @ANNOLAVORO AND SosTipodoc = @TIPODOC  
   order by SosRif desc
   GOTO FINE
   END

IF @TIPODOC = 'P'  
   BEGIN
   insert into #TMPDOC
   select top 500 SosCliCons,Anadesc,
                  TIPOD = @TIPODOC ,
                  SosRif,SosNum,SosData,Soscau,MgCaudesc,
				  TRASFORMATO= CASE WHEN SosTrasf = 1 then 'TRASFORMATO' + ' ' + SosNote ELSE  SosNote END,SosFP,0
   from TbSos inner join
        vdox.dbo.TbAna on AnaCod = SosCliCons inner join
        TbTcau on MgCauId = Soscau
   where SosNDep = @DEP AND Sosnum <> 999999 AND DATEPART(YEAR,SosDATA) = @ANNOLAVORO AND SosTipodoc = @TIPODOC  
   order by SosRif desc
   GOTO FINE
   END

FINE:
SELECT * FROM #TMPDOC        






GO
/****** Object:  StoredProcedure [dbo].[X_DOCUMENTI_FATNCR]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[X_DOCUMENTI_FATNCR]  @TIPODOC as varchar(1), @ANNOLAVORO AS SMALLINT, @DEP AS SMALLINT, @REGFAT AS SMALLINT
AS

---   PREPARAZIONE TABLELLA VUOTA 
CREATE TABLE [#TMPDOC] (
	[CLIFOR] VARCHAR(5) NOT NULL ,
	[ANADESC] varchar(60) NULL ,
	[TIPOD] varchar(20) NOT NULL ,
	[TESRIF] INT NOT NULL ,
	[TESNUM] INT NOT NULL ,
	[TESDATA] SMALLDATETIME NOT NULL ,
	[CAU] SMALLINT NOT  NULL ,
    [CAUDESC] varchar(30) NOT NULL ,
	) ON [PRIMARY]

insert into #TMPDOC
select top 500 FatCliCons,Anadesc,
               TIPOD = @TIPODOC ,
               FatRif,FatNum,FatData,Fatcau,MgCaudesc
from TbFat inner join
     vdox.dbo.TbAna on AnaCod = FatCliCons inner join
     TbTcau on MgCauId = FatCau
where Fatnum <> 999999 AND DATEPART(YEAR,FATDATA) = @ANNOLAVORO and fatnumreg = @REGFAT and FatDiff <> 'D' AND FatNDep = @DEP
order by FatRif desc

SELECT * FROM #TMPDOC     
GO
/****** Object:  StoredProcedure [dbo].[X_EVADISOS_REGISTRA_TESTATA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   PROC [dbo].[X_EVADISOS_REGISTRA_TESTATA] @TIPODOC AS VARCHAR(1), @DATA AS SMALLDATETIME, @NUMREG AS SMALLINT, @CLIENTE AS VARCHAR(5), @CAU as smallint, @TERM VARCHAR(50) = '', @UTENTE VARCHAR(50) = ''
AS


DECLARE  @NUMRIF AS INT
SET @NUMRIF = 0

IF @TIPODOC = 'B'
   BEGIN
  
	  Insert into TbBol WITH (TABLOCKX) (BolTipoDoc,BolData,BolNum,BolAge,BolRifFat,BolCliCons,BolPagCod,BolAbi,BolCab,BolEsespe,BolNimp,BolCau,BolSculter,BolCliFat,BolNumReg,BolCF,BolPorto,BolNrag,BolNDep,BolNote) 
	  SELECT @TIPODOC,@DATA,999999, CASE WHEN ClCodage > 0 THEN ClCodage ELSE 99 END,0,@CLIENTE,ClPagam,ClAbi,ClCab,ClEsespe,ClCodivani,@CAU,ClSculter,@CLIENTE,@NUMREG,'C','ASSEGNATO',0,0,'' 
	  FROM TbCli 
	  where CLCod = @CLIENTE

	  SET @NUMRIF = (SELECT @@IDENTITY)	

	  --insert into TbDoc 
	  --select @TIPODOC,@NUMRIF,@TERM,@UTENTE

	END


iF @TIPODOC = 'F'
   BEGIN
  
      Insert into TbFat WITH (TABLOCKX) (FatTipoDoc,FatData,FatNum,FatAge,FatCliCons,FatPagCod,FatAbi,FatCab,FatEsespe,FatNimp,FatCau,FatSculter,FatCliFat,FatNumReg,FatPorto,FatTrasf,FatNDep,FatNote) 
      SELECT @TIPODOC,@DATA,999999, CASE WHEN ClCodage > 0 THEN ClCodage ELSE 99 END,@CLIENTE,ClPagam,ClAbi,ClCab,ClEsespe,ClCodivani,@CAU,ClSculter,@CLIENTE,@NUMREG,'ASSEGNATO',0,0,''
      FROM TbCli
	  where CLCod = @CLIENTE

      SET @NUMRIF = (SELECT @@IDENTITY)
 
   --    insert into TbDoc 
	  --select @TIPODOC,@NUMRIF,@TERM,@UTENTE
	END


SELECT @NUMRIF
GO
/****** Object:  StoredProcedure [dbo].[X_FPA_CONTROLLO_COMMITTENTI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROC [dbo].[X_FPA_CONTROLLO_COMMITTENTI]  @FATRIF AS INT, @DIFFERITA AS BIT, @REGFAT AS SMALLINT
AS

DECLARE @ERRORE AS SMALLINT, @MESSAGGIO AS VARCHAR(100), @CLCOD AS VARCHAR(5), @REGPA AS SMALLINT 

set @REGPA= (select top 1 TaiReg12 from Tbtai order by TaiAnno Desc)

SET @ERRORE = 0
SET @MESSAGGIO = ''

IF @DIFFERITA = 0
   BEGIN
   set @CLCOD = (SELECT DcgCli from TbDcg Where DcgNumrif = @FATRIF)
   SET @ERRORE = (SELECT Dbo.CLIENTE_VALIDO(@CLCOD,@REGFAT,@REGPA))
   SET @MESSAGGIO = CASE
                    WHEN @ERRORE = 1 THEN 'MANCA CODICE DESTINATARIO O CODICE NON VALIDO'
					 WHEN @ERRORE = 2 THEN 'MANCA PARTITA IVA E CODICE FISCALE'
					  WHEN @ERRORE = 3 THEN 'MANCA INDIRIZZO'
					   WHEN @ERRORE = 4 THEN 'MANCA COMUNE'
					    WHEN @ERRORE = 5 THEN 'MANCA CAP O CAP ERRATO'
						  WHEN @ERRORE = 6 THEN 'MANCA NAZIONE' ELSE '' END

   SELECT ERRORE = @ERRORE, MESSAGGIO = @MESSAGGIO
   GOTO FINE
   END

IF @DIFFERITA = 1
   BEGIN
   SELECT DISTINCT CLIENTE= TmpCliente,
                   RAGSOC = CAST('' AS VARCHAR(60)),
                   ERRORE = Dbo.CLIENTE_VALIDO(TmpCliente,@REGFAT,@REGPA),
				   MESSAGGIO = CAST('' AS VARCHAR(100)) 
   into #tmp
   FROM TmpRim where tmpcliente > '' 

   delete from #tmp where ERRORE = 0

   update #tmp set MESSAGGIO = CASE
                    WHEN ERRORE = 1 THEN 'MANCA CODICE DESTINATARIO O CODICE NON VALIDO'
					 WHEN ERRORE = 2 THEN 'MANCA PARTITA IVA E CODICE FISCALE'
					  WHEN ERRORE = 3 THEN 'MANCA INDIRIZZO'
					   WHEN ERRORE = 4 THEN 'MANCA COMUNE'
					    WHEN ERRORE = 5 THEN 'MANCA CAP O CAP ERRATO'
						  WHEN ERRORE = 6 THEN 'MANCA NAZIONE' ELSE '' END,
				   RAGSOC = (Select AnaDesc from vdox.dbo.tBAna where AnaCod = CLIENTE)

   DELETE FROM TmpRim WHERE TmpCliente in (SELECT CLIENTE from #tmp where errore <> 0)

   SELECT * FROM #tmp
   END

FINE:


GO
/****** Object:  StoredProcedure [dbo].[X_FPA_DDT]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[X_FPA_DDT] @FATRIF AS INT
AS


SELECT RIGA = IDENTITY(INT,1,1),BolRif,BolNum,BolData,Quanti = 0 
INTO #TMPBOL
FROM TbBol INNER JOIN
     TbCor ON CORTIPODOC = BolTipoDoc and corrif = bolrif
WHERE BOLRIFFAT = @FATRIF  AND CorArtId <>0 and CorCiva <> 0
order by Boldata,BolRif,CorProg

update #tmpbol set quanti = (select count(distinct BolRif) from #tmpbol)

select RIGA,
       DDT_Num  = cast(BolNum as varchar(10)),
       DDT_DATA = ISNULL(convert(varchar(10),BolData,126),''),
	   Quanti
FROM #TMPBOL

GO
/****** Object:  StoredProcedure [dbo].[X_FPA_DDTL]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROC [dbo].[X_FPA_DDTL] @FATRIF AS INT
AS


SELECT RIGA = IDENTITY(INT,1,1),FteDDtNum,FteDDtData,Quanti = 0 
INTO #TMPBOL
FROM TbFteDDt
WHERE FteRif = @FATRIF 
order by FteDDtData

select RIGA,
       DDT_Num  = FteDDtNum,
       DDT_DATA = ISNULL(convert(varchar(10),FteDDtData,126),''),
	   Quanti
FROM #TMPBOL


GO
/****** Object:  StoredProcedure [dbo].[X_FPA_HEADER]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE   PROC [dbo].[X_FPA_HEADER] @FATRIF AS INT
AS

DECLARE @REGFAT as int, @REGNCR  as int, @PROGRESSIVO AS int, @FORMATO_TRASM VARCHAR(5), @COD_DESTINATARIO VARCHAR(7), @DIRFILE VARCHAR(50),
        @ERR_COD SMALLINT, @ERR_DESC VARCHAR(100), @PEC_DESTINATARIO VARCHAR(100), @NREG SMALLINT, @SERIE_LETTERALE AS VARCHAR(5),
		@ESIGIBILITA_IVA VARCHAR(1) , @PIVAEST VARCHAR(20), @PIVA VARCHAR(11), @CFIS VARCHAR(16), @REGPA AS SMALLINT, @DA_ELABORARE AS VARCHAR(20), @TIPOFTE VARCHAR(4),
		@PIVA_MITTENTE VARCHAR(11)

SET @DA_ELABORARE = 'DA_ELABORARE\'
SET @ERR_COD = 0
SET @ERR_DESC = ''

SET @PIVA_MITTENTE = (SELECT top 1 AnaPIva FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az')


SET @TIPOFTE = (SELECT FatTipoFte FROM tBfAT WHERE FATRIF = @FATRIF)
IF @TIPOFTE = ''
   BEGIN
   SET @TIPOFTE = 'TD01'
   END

set @REGFAT = (select top 1 TaiReg10 from Tbtai order by TaiAnno Desc)
set @REGNCR = (select top 1 TaiReg11 from Tbtai order by TaiAnno Desc)
set @REGPA= (select top 1 TaiReg12 from Tbtai order by TaiAnno Desc)

SET @SERIE_LETTERALE = isnull((SELECT TOP 1 RivaSL FROM COGE.dbo.TbRegIva where RIvaNReg = (Select FatNumReg from TbFat where FatRif= @FATRIF) order by RIvaAnno Desc),'')

SET @FORMATO_TRASM = 'FPR12'

SELECT TbCli.*,ClFteDestinatario=isnull(ClFteDestinatario,''),ClFtePec=isnull(ClFtePec,'')
INTO #TMPCLI
from tbCli LEFT OUTER join
     TbFTeCli on ClFteCod = ClCod
where ClCod = (select fatCliFat from Tbfat where fatrif = @FATRIF)

SELECT * 
INTO #TMPANACLI
from VDOX.DBO.TbAna
where AnaCod = (select fatCliFat from Tbfat where fatrif = @FATRIF) 

SELECT * 
INTO #BANCA
from COGE.DBO.TbBan where BanCod = (select ClCodBan from #TMPCLI)

SELECT @PIVAEST=AnaPivaEst, @PIVA=AnaPiva, @CFIS =AnaCFis
FROM #TMPANACLI

SELECT @COD_DESTINATARIO = ClFteDestinatario, @PEC_DESTINATARIO = ClFtePec
FROM #TMPCLI
IF @COD_DESTINATARIO = ''
   BEGIN
   IF @PIVAEST > ''
      BEGIN
      SET @COD_DESTINATARIO = 'XXXXXXX'
	  END
   ELSE
      BEGIN
      SET @COD_DESTINATARIO = '0000000'
	  END
   END

---------------------se autofattura creo td27

IF @PIVA_MITTENTE = @PIVA
   BEGIN
     SET @TIPOFTE = 'TD27'
   END

-------AGGIORNAMENTO FTE

UPDATE TbFte SET FteAnno = datepart(year,fatdata),
                 FteNumero = FatNum,
				 FteRegistro = FatNumReg,
				 FteData = FatData,
				 FteAlfaNum =  CASE WHEN @SERIE_LETTERALE <> '' then cast(FatNum as varchar(10)) + '/' + @SERIE_LETTERALE ELSE cast(FatNum as varchar(10)) + '/' + cast(FatNumreg as varchar) END,
				 FteCliente = FatCliFat,
				 FteRagSoc =isnull((select top 1 Anadesc from vdox.dbo.TbAna where anacod = FatCliFat),''),
				 FteTotFat =  isnull((select DcgRieTotale from TbDcg where dcgNumrif = FatRif),0),
				 FteOrdNum =ISNULL( FtOrdNum,''),
				 FteOrdData=FtOrdData,
				 FteCIG=isnull(FtCIG,''),
				 FteCUP=isnull(FtCUP,'')
FROM TbFat inner join
     TbFte on Fterif = fatRif LEFT OUTER JOIN
	 tbFtCig on FtRif = fatrif
WHERE FatRif = @FATRIF

---------- INIZIO GENERAZIONE

select  @DIRFILE = APCartellaINVIO
from TbAziPa

SET @ESIGIBILITA_IVA = 'I'

IF LEN(@COD_DESTINATARIO) = 6
   BEGIN
   SET @FORMATO_TRASM = 'FPA12'
   SET @ESIGIBILITA_IVA = 'S'
   END

select PaCodiva into #split from Coge.dbo.TbPaCii where PATIPO='IVA'

PREPARA_FILE:

SELECT Err_cod = @ERR_COD,
       Err_desc = @ERR_DESC,
       Id_Trasm_Paese = 'IT',
       Id_Trasm_Codice = (SELECT top 1 APCodFiscTrasm FROM TbAziPa),
	   Prog_Invio = @PROGRESSIVO,
	   DirFile = @DIRFILE + @DA_ELABORARE,
	   Prog_ALFA =  DBO.NUMBER_TO_STR_BASE(36,@PROGRESSIVO),
	   FormTrasm = @FORMATO_TRASM,
	   CodDestinatario  = @COD_DESTINATARIO,
	   PECDestinatario =  @PEC_DESTINATARIO,
	   Id_Fisc_Paese = 'IT',
	   Id_Fisc_Codice = @PIVA_MITTENTE,
	   Denominazione = (SELECT top 1 AnaDesc FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   Nome = (SELECT top 1 AnaRag2 FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   Cognome = (SELECT top 1 AnaRag1 FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   Regime_Fiscale = (SELECT TOP 1 APRegFiscale from TbAziPA),
	   Indirizzo = (SELECT top 1 AnaIndirizzo FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   Cap = (SELECT top 1 AnaCap FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   Comune = (SELECT top 1 AnaCitta FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   Prov = (SELECT top 1 AnaProv FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   Naz = 'IT',
	   REA_Ufficio = (SELECT TOP 1 APReaProv from TbAziPA), 
	   REA_Numero = (SELECT TOP 1 APReaNum from TbAziPA),
	   REA_CapSoc = (SELECT TOP 1 APReaCapSoc from TbAziPA),
	   REA_SocioUnico =CASE WHEN (SELECT TOP 1 APReaSocioUnico from TbAziPA) = 0 THEN '' WHEN  (SELECT TOP 1 APReaSocioUnico from TbAziPA) = 1 THEN  'SU' ELSE 'SM' END,
	   REA_StatoLiq = CASE WHEN (SELECT TOP 1 APReaLiquidazione from TbAziPA) = 0 THEN 'LN' ELSE 'LS' END,
	   RIFERIMENTO_AMMINISTRAZIONE = '',

	   CLI_PIVA_Paese =CASE WHEN rtrim(@PIVAEST) > '' THEN SUBSTRING(@PIVAEST,1,2) ELSE 'IT' END ,
	   CLI_PIVA_Codice = CASE WHEN rtrim(@PIVAEST) > '' THEN substring(@PIVAEST,3,LEN(@PIVAEST) - 2) ELSE (CASE WHEN LTRIM(@PIVA) > '' THEN rtrim(@PIVA) ELSE '' END) END ,
	   CLI_CODFISC = RTRIM(@CFIS),
	   CLI_DENOMINAZIONE = (SELECT AnaDesc FROM #TMPANACLI),
	   CLI_INDIRIZZO = (SELECT AnaIndirizzo FROM #TMPANACLI),
	   CLI_CAP = (SELECT AnaCap FROM #TMPANACLI),
	   CLI_COMUNE = (SELECT AnaCitta FROM #TMPANACLI),
	   CLI_PROV = (SELECT AnaProv FROM #TMPANACLI),
	   CLI_NAZ = CASE WHEN rtrim(@PIVAEST) > '' THEN SUBSTRING(@PIVAEST,1,2) ELSE 'IT' END ,

	   TerzoInt_Paese = 'IT',
       TerzoInt_Codice = (SELECT top 1 APCodFisc3Int FROM TbAziPa),
	   TerzoInt_RagSoc = (SELECT top 1 APRagSoc3Int FROM TbAziPa),
	   TerzoInt_Nome = (SELECT top 1 APNome3Int FROM TbAziPa),
	   TerzoInt_Cognome = (SELECT top 1 APCognome3Int FROM TbAziPa),

	    --TipoDocum = CASE WHEN FatNumReg = @REGNCR THEN 'TD04' ELSE 'TD01' END,
	   TipoDocum = CASE WHEN isnull((select DcgRieTotFat from TbDcg where dcgNumrif = FatRif),0) < 0 then 'TD04' ELSE @TIPOFTE END,
	   Divisa =  'EUR',
	   Data = convert(varchar(10),FatData,126),
	   Numero = FteAlfaNum,

	   SCONTO_Tipo = CASE WHEN DcgRieTotOma > 0 THEN 'SC' else '' END,
	   SCONTO_Importo = ABS(DcgRieTotOma),
	   TOTALE_Doc = ABS(DcgRieNetto),

	   ORDACQ_Id = isnull(FteOrdNum,''),
	   ORDACQ_DATA = ISNULL(convert(varchar(10),FteOrdData,126),''),
	   ORDACQ_CIG = isnull(FteCIG,''),
	   ORDACQ_CUP = isnull(FteCUP,''),

	   DIFFERITA = FatDiff,

	   DDT_Num = isnull(FteDDTNum,''),
	   DDT_DATA = ISNULL(convert(varchar(10),FteDDTData,126),''),

	   DCGRIECI1,
	   ALIQ1 = cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = DCGRIECI1),0) AS DECIMAL (5,2)),
	   NATURA1 = CASE WHEN DCGRIECI1 > 0 THEN  isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = DCGRIECI1),'') ELSE '' END,
	   IMPON1 = CASE WHEN isnull((select DcgRieTotFat from TbDcg where dcgNumrif = FatRif),0) < 0 THEN CAST(DCGRIEIMP1 * -1 AS DECIMAL(10,2)) ELSE CAST(DCGRIEIMP1 AS DECIMAL(10,2)) END ,
	   IVA1 = CASE WHEN isnull((select DcgRieTotFat from TbDcg where dcgNumrif = FatRif),0) < 0 THEN CAST(DCGRIEIVA1 * -1 AS DECIMAL(10,2)) ELSE CAST(DCGRIEIVA1 AS DECIMAL(10,2)) END,
	   ESIG1 = CASE WHEN DCGRIEIVA1 > 0 THEN (CASE WHEN DCGRIECI1 in (select distinct PaCodiva FROM #SPLIT) THEN 'S' ELSE 'I' END) ELSE '' END,
	   DCGRIECI2,
	   ALIQ2 = cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = DCGRIECI2),0) AS DECIMAL (5,2)),
	   NATURA2 = CASE WHEN DCGRIECI2 > 0 THEN  isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = DCGRIECI2),'') ELSE '' END,
	   IMPON2 = CASE WHEN isnull((select DcgRieTotFat from TbDcg where dcgNumrif = FatRif),0) < 0 THEN CAST(DCGRIEIMP2 * -1 AS DECIMAL(10,2)) ELSE CAST(DCGRIEIMP2 AS DECIMAL(10,2)) END ,
	   IVA2 = CASE WHEN isnull((select DcgRieTotFat from TbDcg where dcgNumrif = FatRif),0) < 0 THEN CAST(DCGRIEIVA2 * -1 AS DECIMAL(10,2)) ELSE CAST(DCGRIEIVA2 AS DECIMAL(10,2)) END,
	   ESIG2 = CASE WHEN DCGRIEIVA2 > 0 THEN  (CASE WHEN DCGRIECI2 in (select distinct PaCodiva FROM #SPLIT) THEN 'S' ELSE 'I' END) ELSE '' END,
	   DCGRIECI3,
	   ALIQ3 = cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = DCGRIECI3),0) AS DECIMAL (5,3)),
	   NATURA3 = CASE WHEN DCGRIECI3 > 0 THEN  isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = DCGRIECI3),'') ELSE '' END,
	   IMPON3 = CASE WHEN isnull((select DcgRieTotFat from TbDcg where dcgNumrif = FatRif),0) < 0 THEN CAST(DCGRIEIMP3 * -1 AS DECIMAL(10,2)) ELSE CAST(DCGRIEIMP3 AS DECIMAL(10,2)) END ,
	   IVA3 =CASE WHEN isnull((select DcgRieTotFat from TbDcg where dcgNumrif = FatRif),0) < 0 THEN CAST(DCGRIEIVA3 * -1 AS DECIMAL(10,2)) ELSE CAST(DCGRIEIVA3 AS DECIMAL(10,2)) END,
	   ESIG3 = CASE WHEN DCGRIEIVA3 > 0 THEN  (CASE WHEN DCGRIECI3 in (select distinct PaCodiva FROM #SPLIT) THEN 'S' ELSE 'I' END) ELSE '' END,
	   
	   COND_PAG = case when (PagTipo <> 4 and PagTipo <> 6 and PagTipo <> 8)  THEN 'TP01' ELSE 'TP02' END,
	   TBDCG= 'TbDcg'  
	 

from TbFat inner join
     TbDcg on DcgNumrif = FatRif left outer join
	 TbFte on FteRif = fatrif INNER JOIN
	 COGE.DBO.TbPag on PagCod = FatPagCod
where Fatrif = @FATRIF


FINE:

GO
/****** Object:  StoredProcedure [dbo].[X_FPA_PAG]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[X_FPA_PAG] @FATRIF AS INT
AS

DECLARE @CODBAN AS SMALLINT

SELECT @CODBAN =  ClCodBan
from tbCli where ClCod = (select fatCliFat from Tbfat where fatrif = @FATRIF)

SELECT * 
INTO #BANCA
from COGE.DBO.TbBan where BanCod =  @CODBAN  


SELECT RATA = RicNRata,
       MOD_PAG = isnull((Select PagCodFe from coge.dbo.TbPag where pagcod = FatPagCod),''),	 
	   SCAD_PAG = CASE when (PagTipo <> 4 and PagTipo <> 6 and PagTipo <> 8) THEN isnull(convert(varchar(10),RicDSca ,126),'') else '' end,
	   IMPO_PAG =  CAST(ABS(RicImpRata) AS DECIMAL(10,2)),
	   ISTITUTO = isnull((select cadescft from coge.dbo.TbCab where CaAbi = RicAbi and CaCab = RicCab),''),
	   IBAN = CASE WHEN ( RicTPag = 5 AND @CODBAN <> 0)
	               THEN 'IT' + (select BANCINEUR FROM #BANCA)+ (select BANCIN FROM #BANCA)
				        + REPLICATE('0', 5 - DATALENGTH(CAST(ISNULL((select BANABI FROM #BANCA), 0) AS VARCHAR(5)))) + CAST(ISNULL((select BANABI FROM #BANCA), 0) AS VARCHAR(5)) 
						+ REPLICATE('0', 5 - DATALENGTH(CAST(ISNULL((select BANCAB FROM #BANCA), 0) AS VARCHAR(5)))) + CAST(ISNULL((select BANCAB FROM #BANCA), 0) AS VARCHAR(5)) 
	                    + ISNULL((select BANCC FROM #BANCA), ' ') ELSE '' END,
       ABI = CASE WHEN  (PagTipo = 2 or PagTipo = 7 )
	              THEN RicAbi ELSE '' END, 
       CAB = CASE WHEN  (PagTipo = 2 or PagTipo = 7 )
	              THEN RicCab ELSE '' END
from TbFte_Eff inner join
     TbFat on FatRif = RicRifFat inner join
	 TbFte on FteRif = fatrif INNER JOIN
	 COGE.DBO.TbPag on PagCod = FatPagCod
where Fatrif = @FATRIF

order by ricnrata


GO
/****** Object:  StoredProcedure [dbo].[X_FPA_RIGHE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       PROC [dbo].[X_FPA_RIGHE] @FATRIF AS INT
AS

DECLARE @DIFFERITA AS VARCHAR(1), @NIMP AS SMALLINT, @SPE_RB DECIMAL(9,2), @SPE_IMB DECIMAL(9,2), @CII_RB SMALLINT , @CII_IMB SMALLINT, @TOT_SCONTO DECIMAL(9,2),
        @PERC_SCONTO AS DECIMAL(5,2), @MIN_RIGA AS SMALLINT, @MAX_RIGA AS SMALLINT, @VERSIONE AS VARCHAR(25), @SWHOUSE AS VARCHAR(50), @CII_ASW AS SMALLINT,
		@CII_STANDARD AS SMALLINT, @CLIENTE AS VARCHAR(5), @CLESPSCONTI AS VARCHAR(1), @COD_BOLLO AS VARCHAR(20), @TOT_OMAGGIO_SOGG DECIMAL(9,2), @CII_OMAGGIO AS SMALLINT, @CIV_OMAGGIO AS SMALLINT,
		@TOT_OMAGGIO_ESE DECIMAL(9,2)


SET @CII_ASW = 1

SELECT @CLIENTE = FatCliFat, @DIFFERITA = FatDiff, @NIMP = FatNimp
FROM TbFat
WHERE FatRif = @FATRIF

SET @CLESPSCONTI = (SELECT ClEspSconti from TbCli where clcod = @CLIENTE)

SET @COD_BOLLO = (SELECT top 1 TAICODBOLLO FROM TbTai order by taianno desc)


SET @VERSIONE = 'Versione #Asw0100#'

SET @SWHOUSE = 'Selco s.n.c. di Castiglia T. & C. / DEDALO'

CREATE TABLE #tmprighe (
    [RIGA] int identity(1,1) not null, 
	[TIPO_CESSIONE] [varchar](2) NOT NULL,
	[CorArtId] [int] NOT NULL,
	[CorCodArt] [varchar](20) NOT NULL,
	[CorUM] [varchar](2) NOT NULL,
	[CorDesc] [varchar](200) NOT NULL,
	[CorLotto] [varchar](20) NOT NULL,
	[CorCiva] [smallint] NOT NULL,
	[CorQuaCon] [decimal](9, 3) NOT NULL,
	[CorPrezzo] [decimal](9, 3) NOT NULL,
	[CorSc1]  [decimal](5, 2) NOT NULL,
	[CorSc2]  [decimal](5, 2) NOT NULL,
	[CorSc3]  [decimal](5, 2) NOT NULL,
	[CorImporto] [decimal](9, 2) NOT NULL,
	[CorOmaggio] [varchar](1) NOT NULL,
	[CorDescIva] [varchar](35) NOT NULL,
	[CorDataScad] [smalldatetime] NULL,
	[CorSculter] [decimal](9, 2) NOT NULL,
 ) ON [PRIMARY]


 SET @CIV_OMAGGIO = (SELECT top 1 TaiCiv3 from TbTai order by TaiAnno desc)
 SET @CII_OMAGGIO = DBO.TCI(@CIV_OMAGGIO)

 SET @CII_STANDARD = (SELECT TOP 1  DBO.TCI(CorCiva) from TbCor where cortipodoc = 'F' AND CORRIF = @FATRIF AND CORCIVA > 0)
 SELECT @SPE_RB= DcgSpeRB, @SPE_IMB= DcgSpeImb, @CII_RB = DBO.TCI(DcgSpeRBCI), @CII_IMB=DcgSpeImbCI, @TOT_SCONTO=DcgRieTotSconto, @PERC_SCONTO = DcgSculter
 from TbDcg
 where dcgnumrif =  @FATRIF



 ----------- CORLOTTO NON ESISTE

if @DIFFERITA <> 'D'
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter)
   select '',CorArtId, CorCodArt, CorDesc, '', DBO.TCI(CorCiva), CorQuaCon,
        ---  case when @CLESPSCONTI = 'N' THEN CorPrezzo  * (1 -corSc1/100) else  CorPrezzo end, 
		 --- case when @CLESPSCONTI = 'N' then 0 else CorSc1 END, 
		 CorPrezzo,
		 CorSc1,
		 CorSc2, CorSc3, cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100)* (1 -corSc3/100) as decimal(11,5)) * CorQuaCon, isnull(CorOmaggi,''), '', null, CorUm ,
		 CorSculter =  (cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100) as decimal(11,5)) * CorQuaCon) - CorImporto 
   from TbFat inner join
        TbCor on cortipodoc = Fattipodoc and corrif = fatrif
   WHERE FATRIF = @FATRIF AND CorDesc > ''
   order by corprog
   END
ELSE
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter)
   select '',CorArtId, CorCodArt, CorDesc, '',  DBO.TCI(CorCiva), CorQuaCon, 
          case when @CLESPSCONTI = 'N' THEN CorPrezzo  * (1 -corSc1/100) else  CorPrezzo end, 
		  case when @CLESPSCONTI = 'N' then 0 else CorSc1 END, 
		  CorSc2, CorSc3, cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100)* (1 -corSc3/100) as decimal(11,5)) * CorQuaCon, isnull(CorOmaggi,''), '', null, CorUM,
		  CorSculter =  (cast(CorPrezzo * (1 -corSc1/100)* (1 -corSc2/100) as decimal(11,5)) * CorQuaCon) - CorImporto 
   FROM TbFat INNER JOIN
        TbBol on Bolriffat = FATRIF INNER JOIN
        TbCor ON (CORRIF = BOLRIF AND CORTIPODOC = BOLTIPODOC ) or (CORRIF = FATRIF AND CORCODART = 'BOLLO' AND BOLRIF = (SELECT MAX(BOLRIF) FROM tBbOL WHERE BOLRIFFAT = FATRIF))
    WHERE  FATRIF = @FATRIF AND CorArtId <>0 and CorCiva <> 0
  --- order by Boldata,BolRif,CorCodArt
   order by Boldata,BolRif,CorProg
   END


UPDATE #TMPRIGHE SET Corciva = @CII_STANDARD, CorDesciva = 'd'
where corciva = 0

UPDATE #TMPRIGHE SET CorPrezzo = CorImporto where corprezzo=0 and corimporto <> 0


SET @TOT_OMAGGIO_SOGG = ISNULL((select sum(CorImporto)  from #TMPRIGHE where corOmaggio = 'S'),0)


-------------------------------- CALCOLO STORNO X OMAGGIO ESENTE

SELECT TOT_STORNO_IMP = sum(CorImporto),
       ALIQ = CASE WHEN @NIMP = 0 THEN cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = CORCIVA),0) AS DECIMAL (5,2)) ELSE cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = @NIMP),0) AS DECIMAL (5,2)) END,
	   TOT_STORNO = CAST(0 AS DECIMAL(9,2))  
into #OMAGGIO_ESENTE
FROM #TMPRIGHE
where corOmaggio = 'E'
GROUP BY CorCiva

UPDATE #OMAGGIO_ESENTE SET TOT_STORNO = TOT_STORNO_IMP * (1 + ALIQ / 100)

SET @TOT_OMAGGIO_ESE = isnull((SELECT isnull(SUM(TOT_STORNO),0) from #OMAGGIO_ESENTE),0)

--------------------------------------------------------------------------------------------

SET @MIN_RIGA = (SELECT MIN(RIGA) FROM #tmprighe)
SET @MAX_RIGA = (SELECT MAX(RIGA) FROM #tmprighe)

IF @SPE_RB > 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter  )
   select 'AC',0, '', 'SPESE R.B.', '', @CII_RB, 0, @SPE_RB, 0, 0, 0, @SPE_RB, '', 'Spese Incasso #SP01#', null,'',0   
   END

IF @SPE_IMB > 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM,CorSculter  )
   select 'AC',0, '', 'CONTR.SPESE', '', @CII_IMB, 0, @SPE_IMB, 0, 0, 0, @SPE_IMB, '', 'Spese Imballo #SP04#', null, '',0   
   END

IF @TOT_SCONTO <> 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad, CorUM, CorSculter  )

   select 'SC',0, '', 'IMPORTO SCONTO', '', corciva, 0, CASE WHEN @TOT_SCONTO < 0 THEN SUM(CorSculter) ELSE -SUM(CorSculter) END, 0, 0, 0, CASE WHEN @TOT_SCONTO < 0 THEN SUM(CorSculter) ELSE -SUM(CorSculter) END, '','',NULL,'',0 
   FROM #TMPRIGHE
   where isnull(CorSculter,0) <> 0
   group by CorCiva
   END

IF @TOT_OMAGGIO_SOGG > 0
   BEGIN
   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad,CorUM,CorSculter  )
   select '',0, '', 'Storno Valori Omaggi', '', @CII_OMAGGIO, 0, -@TOT_OMAGGIO_SOGG, 0, 0, 0, -@TOT_OMAGGIO_SOGG, '', 'STO', null,'',0   
   END

--IF @TOT_OMAGGIO_ESE> 0
--   BEGIN
--   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad,CorUM )
--   select '',0, '', 'Storno per omaggio senza rivalsa', '', @CII_OMAGGIO, 0, -@TOT_OMAGGIO_ESE, 0, 0, 0, -@TOT_OMAGGIO_ESE, '', 'STO', null,''   
--   END

---------RIGA ASSOSOFTWARE
---insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorOmaggio, CorDescIva, CorDataScad )
---select '',0, '', 'Riga descrittiva contenente informazioni tecniche ed aggiuntive del documento', '', @CII_STANDARD, 0, 0, 0, 0, 0, 0, '', 'desc', null   

SELECT NUM_LINEA = RIGA,
       TipoCessione = TIPO_CESSIONE,
       Codice_tipo = CASE WHEN CorCodArt <> '' THEN 'AswArtFor' ELSE '' END,
	   CodiceValore = CorCodArt,
	   Descrizione = CorDesc,
	   Quantita = CASE WHEN CorQuaCon < 0 THEN cast(CorQuaCon * -1 as decimal(10,3)) ELSE cast(CorQuaCon as decimal(10,3)) END,
	   UM = CorUM,
	   PrezzoUnitario =CASE WHEN CorQuaCon < 0 THEN corPrezzo * -1 ELSE corPrezzo END,
	   Sc1 = case when CorOmaggio = 'E' THEN 100.00 ELSE cast(CorSc1 as decimal(5,2)) END,
	--   Sc1 = cast(CorSc1 as decimal(5,2)),
	   Sc2 = cast(CorSc2 as decimal(5,2)),
	   Sc3 = cast(CorSc3 as decimal(5,2)),
	   SCULTER = 0, 
	   Importo = CASE WHEN COROMAGGIO <> 'E' THEN cast(CorImporto as decimal(10,2)) ELSE 0 END,
	   --Importo = cast(CorImporto as decimal(10,2)),
	   ALIQ =CASE WHEN @NIMP = 0 THEN cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = CORCIVA),0) AS DECIMAL (5,2)) ELSE cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = @NIMP),0) AS DECIMAL (5,2)) END,
	   Natura = CASE WHEN (@NIMP = 0  or CorCodArt = @COD_BOLLO) THEN isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = CORCIVA),'') ELSE isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = @NIMP),'') END, 
	   Altri1_TIPO = case when CorLotto <> '' THEN 'Lotto' when TIPO_CESSIONE = 'AC' THEN 'AswSpAcces'  when TIPO_CESSIONE = 'SC' THEN 'AswTRiga'  
	                      when CorDescIva = 'desc' THEN 'AswTRiga'  when CorDescIva = 'd' THEN 'AswTRiga' ELSE '' END ,
	   Altri1_TESTO=CASE WHEN CorDescIva = 'desc' THEN 'Informazioni documento #ID#'  when TIPO_CESSIONE = 'SC' THEN 'Riga sconto #SC#'  when TIPO_CESSIONE = 'AC' THEN CorDesciva when CorDescIva = 'd' THEN 'Descrittivo #DE#' ELSE CorLotto END,
	   Altri1_NUMERO = '',
	   Altri1_DATA = '',
	   Altri2_TIPO = CASE WHEN CorDescIva = 'desc' THEN 'AswRelStd'  when TIPO_CESSIONE = 'SC' THEN 'AswRifRiga' when isdate(CorDataScad) = 1 THEN 'Scadenza' ELSE '' END,
	   Altri2_TESTO= CASE WHEN CorDescIva = 'desc' THEN @VERSIONE when TIPO_CESSIONE = 'SC' THEN 'RigaSconto #' + CAST(@MIN_RIGA AS VARCHAR)+ '-'  + CAST(@MAX_RIGA AS VARCHAR) + '#'ELSE '' END,
	   Altri2_NUMERO = '',
	   Altri2_DATA = ISNULL(convert(varchar(10),CorDataScad,126),''), 
	   Altri3_TIPO = CASE WHEN CorDescIva = 'desc' THEN 'AswSwHouse' ELSE '' END,
	   Altri3_TESTO= CASE WHEN CorDescIva = 'desc' THEN  @SWHOUSE ELSE '' END,
	   Altri3_NUMERO = '',
	   Altri3_DATA = ISNULL(convert(varchar(10),CorDataScad,126),''), 
	   Altri4_TIPO = CASE WHEN CorDescIva = 'desc' THEN 'AswTratSco' ELSE '' END,
	   Altri4_TESTO= CASE WHEN CorDescIva = 'desc' THEN 'Valori come righe sconto #PRS#' ELSE '' END,
	   Altri4_NUMERO = '',
	   Altri4_DATA = ISNULL(convert(varchar(10),CorDataScad,126),'') 

from #tmprighe
GO
/****** Object:  StoredProcedure [dbo].[X_FTE_EST_HEADER]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[X_FTE_EST_HEADER] @PRIID AS INT, @TIPOFTE AS VARCHAR(4)
AS

DECLARE @PROGRESSIVO AS int, @FORMATO_TRASM VARCHAR(5), @COD_DESTINATARIO VARCHAR(7), @DIRFILE VARCHAR(50),
        @ERR_COD SMALLINT, @ERR_DESC VARCHAR(100), @PEC_DESTINATARIO VARCHAR(100), @NREG SMALLINT, @SERIE_LETTERALE AS VARCHAR(5),
		@ESIGIBILITA_IVA VARCHAR(1) , @PIVAEST VARCHAR(20), @PIVA VARCHAR(11), @CFIS VARCHAR(16), @DA_ELABORARE AS VARCHAR(20),@FTE_NUMERO AS INT, @FTE_DATA AS SMALLDATETIME,@FTE_PROT AS INT,
		@FO_COD AS VARCHAR(5), @TOT_FAT AS DECIMAL(10,2), @FTE_RIF AS INT, @BASE_RIF AS INT, @TOT_IVA AS DECIMAL(10,2), @TOT_IMP AS DECIMAL(10,2), @CODIVA AS SMALLINT, @ANAPIVAEST AS VARCHAR(20)

SET @DA_ELABORARE = 'DA_ELABORARE\'
SET @ERR_COD = 0
SET @ERR_DESC = ''
SET @BASE_RIF= 300000000

SET @FORMATO_TRASM = 'FPR12'
SET @COD_DESTINATARIO = '0000000' 
SET @PEC_DESTINATARIO = '' 
set @FTE_RIF = @BASE_RIF+@PRIID

SELECT @NREG = PriRegiva, @FTE_NUMERO=PriDocEst, @FTE_DATA = PriDataEst,@FTE_PROT = PriNumProt, @FO_COD = Pricoavere, @TOT_FAT=PriImpAvere,@TOT_IVA=PriImpDare,@TOT_IMP=PriImpAvere - PriImpDare,
       @CODIVA= PriCodIva
from COGE.DBO.TbPri
WHERE PriId = @PRIID and PriCausale = 3

------DATI FORNITORE (CEDENTE/PRESTATORE)

SELECT *
INTO #TMPANAFOR
FROM VDOX.dbo.TbAna
WHERE AnaGrp = 'FO' and Anacod = @FO_COD

SET @ANAPIVAEST = (SELECT AnaPivaEst FROM #TMPANAFOR)


---------REGISTRAZIONE FTE
--IF (SELECT COUNT(*) FROM TbFte WHERE FteRif = @FTE_RIF) = 0 
--    BEGIN  
--	  INSERT INTO TbFte (FteRif,FteDataOra,FteBlueNext) VALUES (@FTE_RIF,getdate(),1) 
--	END 

---------AGGIORNAMENTO FTE

UPDATE TbFte SET FteAnno = datepart(year,@FTE_DATA),
                 FteNumero = @FTE_PROT,
				 FteRegistro = @NREG,
				 FteData = @FTE_DATA,
				 FteAlfaNum = cast(@FTE_NUMERO as varchar(10)),
				 FteCliente = @FO_COD,
				 FteRagSoc =isnull((select top 1 Anadesc from vdox.dbo.TbAna where anacod = @FO_COD),''),
				 FteTotFat = @TOT_FAT,
				 FteDaInviare = 1,
				 FteDataOra = GetDate()
WHERE FTERIF = @FTE_RIF


---------- INIZIO GENERAZIONE

select  @DIRFILE = APCartellaINVIO
from TbAziPa

SET @ESIGIBILITA_IVA = 'I'


PREPARA_FILE:

SELECT Err_cod = @ERR_COD,
       Err_desc = @ERR_DESC,
       Id_Trasm_Paese = 'IT',
       Id_Trasm_Codice = (SELECT top 1 APCodFiscTrasm FROM TbAziPa),
	   Prog_Invio = @PROGRESSIVO,
	   DirFile = @DIRFILE + @DA_ELABORARE,
	   Prog_ALFA =  DBO.NUMBER_TO_STR_BASE(36,@PROGRESSIVO),
	   FormTrasm = @FORMATO_TRASM,
	   CodDestinatario  = @COD_DESTINATARIO,
	   PECDestinatario =  @PEC_DESTINATARIO,

	   ---CEDENTE/PRESTATORE

	   Id_Fisc_Paese = SUBSTRING(@ANAPIVAEST,1,2),
	   Id_Fisc_Codice =SUBSTRING(@ANAPIVAEST,3,LEN(@ANAPIVAEST) -2),
	   Denominazione = (SELECT AnaDesc FROM #TMPANAFOR),
	   Nome = (SELECT AnaRag2 FROM #TMPANAFOR),
	   Cognome = (SELECT AnaRag1 FROM #TMPANAFOR),
	   Regime_Fiscale = 'RF18',
	   Indirizzo = (SELECT AnaIndirizzo FROM #TMPANAFOR),
	   Cap = (SELECT AnaCap FROM #TMPANAFOR),
	   Comune = (SELECT AnaCitta FROM #TMPANAFOR),
	   Prov = (SELECT AnaProv FROM #TMPANAFOR),
	   Naz = SUBSTRING((SELECT AnaPivaEst FROM #TMPANAFOR),1,2),
	   REA_Ufficio = '', 
	   REA_Numero = '',
	   REA_CapSoc = '',
	   REA_SocioUnico ='',
	   REA_StatoLiq = '',
	   RIFERIMENTO_AMMINISTRAZIONE = '',

	   ---CESSIONARIO/COMMITTENTE

	   CLI_PIVA_Paese = 'IT' ,
	   CLI_PIVA_Codice =(SELECT top 1 AnaPiva FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az') ,
	   CLI_CODFISC = (SELECT top 1 AnaCfis FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az') ,
	   CLI_DENOMINAZIONE = (SELECT top 1 AnaDesc FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   CLI_INDIRIZZO = (SELECT top 1 AnaIndirizzo FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   CLI_CAP = (SELECT top 1 AnaCap FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   CLI_COMUNE = (SELECT top 1 AnaCitta FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   CLI_PROV = (SELECT top 1 AnaProv FROM VDOX.DBO.tBaNA WHERE ANAGRP = 'az'),
	   CLI_NAZ = 'IT' ,


	   ---TERZO INTERMEDIARIO
	   TerzoInt_Paese = 'IT',
       TerzoInt_Codice = (SELECT top 1 APCodFisc3Int FROM TbAziPa),
	   TerzoInt_RagSoc = (SELECT top 1 APRagSoc3Int FROM TbAziPa),
	   TerzoInt_Nome = (SELECT top 1 APNome3Int FROM TbAziPa),
	   TerzoInt_Cognome = (SELECT top 1 APCognome3Int FROM TbAziPa),


	   ---DATI GENERALI
	   TipoDocum = @TIPOFTE,
	   Divisa =  'EUR',
	   Data = convert(varchar(10),@FTE_DATA,126),
	   Numero = CAST(@FTE_PROT AS VARCHAR) + '/' + CAST(@NREG AS VARCHAR),

	   SCONTO_Tipo = '',
	   SCONTO_Importo = CAST(0 AS DECIMAL(9,2)),
	   TOTALE_Doc = @TOT_FAT,

	   DIFFERITA = 'ESTERA',

	   ---DATI FATTURE COLLEGATE

	   IdDocumento = @FTE_NUMERO,
	   DataDocumento = ISNULL(convert(varchar(10),@FTE_DATA,126),''),

	   DCGRIECI1 = @CODIVA,
	   ALIQ1 = cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = @CODIVA),0) AS DECIMAL (5,2)),
	   NATURA1 = CASE WHEN @TOT_IVA <> 0 THEN  '' ELSE isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = @CODIVA),'') END,
	   IMPON1 = @TOT_IMP,
	   IVA1 = @TOT_IVA,
	   ESIG1 = CASE WHEN @TOT_IVA <> 0 THEN @ESIGIBILITA_IVA ELSE '' END,

	   DCGRIECI2 = 0,
	   ALIQ2 = cast(0 AS DECIMAL (5,2)),
	   NATURA2 = '',
	   IMPON2 = cast(0 AS DECIMAL (10,2)),
	   IVA2 = cast(0 AS DECIMAL (10,2)),
	   ESIG2 ='',

	   DCGRIECI3 = 0,
	   ALIQ3 = cast(0 AS DECIMAL (5,2)),
	   NATURA3 = '',
	   IMPON3 = cast(0 AS DECIMAL (10,2)),
	   IVA3 = cast(0 AS DECIMAL (10,2)),
	   ESIG3 ='',

	   DCGRIECI4 = 0,
	   ALIQ4 = cast(0 AS DECIMAL (5,2)),
	   NATURA4 = '',
	   IMPON4 = cast(0 AS DECIMAL (10,2)),
	   IVA4 = cast(0 AS DECIMAL (10,2)),
	   ESIG4='' 
	  
	   


FINE:

GO
/****** Object:  StoredProcedure [dbo].[X_FTE_EST_RIGHE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROC [dbo].[X_FTE_EST_RIGHE] @PRIID AS INT, @TIPOFTE AS VARCHAR(4)
AS


CREATE TABLE #tmprighe (
    [RIGA] int identity(1,1) not null, 
	[TIPO_CESSIONE] [varchar](2) NOT NULL,
	[CorArtId] [int] NOT NULL,
	[CorCodArt] [varchar](20) NOT NULL,
	[CorDesc] [varchar](100) NOT NULL,
	[CorLotto] [varchar](20) NOT NULL,
	[CorCiva] [smallint] NOT NULL,
	[CorQuaCon] [decimal](9, 2) NOT NULL,
	[CorPrezzo] [decimal](9, 2) NOT NULL,
	[CorSc1]  [decimal](5, 2) NOT NULL,
	[CorSc2]  [decimal](5, 2) NOT NULL,
	[CorSc3]  [decimal](5, 2) NOT NULL,
	[CorImporto] [decimal](9, 2) NOT NULL,
	[CorIva] [decimal](9, 2) NOT NULL,
	
 ) ON [PRIMARY]

  ----------- CORLOTTO NON ESISTE

   insert into #tmprighe (TIPO_CESSIONE,CorArtId, CorCodArt, CorDesc, CorLotto, CorCiva, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorImporto, CorIva)
   select '',0, '', 
          CorDesc = CASE WHEN @TIPOFTE = 'TD17' THEN 'ACQUISTO SERVIZI DALL''ESTERO'
		                 WHEN @TIPOFTE = 'TD18' THEN 'ACQUISTO BENI INTRACOMUNITARI '
		                 WHEN @TIPOFTE = 'TD19' THEN 'ACQUISTO BENI EX. ART.17 C.2 DPR 633/72'
						 END,
		  '',  PriCodIva, 0, SUM(PriImpDare), 0,0,0, SUM(PriImpDare), SUM(PriImpAvere)
   FROM COGE.DBO.TbPri 
   WHERE PriId = @PRIID AND PriCausale = 2
   GROUP by PriCodIva 


SELECT NUM_LINEA = RIGA,
       TipoCessione = TIPO_CESSIONE,
       Codice_tipo = CASE WHEN CorCodArt <> '' THEN 'AswArtFor' ELSE '' END,
	   CodiceValore = CorCodArt,
	   Descrizione = CorDesc,
	   Quantita = CASE WHEN CorQuaCon < 0 THEN cast(CorQuaCon * -1 as decimal(10,2)) ELSE cast(CorQuaCon as decimal(10,2)) END,
	   UM = '',
	   PrezzoUnitario =CASE WHEN CorQuaCon < 0 THEN corPrezzo * -1 ELSE corPrezzo END,
	   Sc1 = cast(CorSc1 as decimal(5,2)),
	   Sc2 = cast(CorSc2 as decimal(5,2)),
	   Sc3 = cast(CorSc3 as decimal(5,2)),
	   SCULTER = 0, 
	   Importo = cast(CorImporto as decimal(10,2)),
	   ALIQ = cast( isnull((select CiiAli from COGE.dbo.TbCii where CiiCod = CORCIVA),0) AS DECIMAL (5,2)),
	   Natura =CASE WHEN CorIva <> 0 THEN '' else isnull((select CiiNatura from COGE.dbo.TbCii where CiiCod = CORCIVA),'') END,
	   Altri1_TIPO = '',
	   Altri1_TESTO= '',
	   Altri1_NUMERO = '',
	   Altri1_DATA = '',
	   Altri2_TIPO = '',
	   Altri2_TESTO= '',
	   Altri2_NUMERO = '',
	   Altri2_DATA = '',
	   Altri3_TIPO = '',
	   Altri3_TESTO= '',
	   Altri3_NUMERO = '',
	   Altri3_DATA = '',
	   Altri4_TIPO = '',
	   Altri4_TESTO= '',
	   Altri4_NUMERO = '',
	   Altri4_DATA = ''	  
	  
from #tmprighe


GO
/****** Object:  StoredProcedure [dbo].[X_FTE_ZIP_SELEZIONA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[X_FTE_ZIP_SELEZIONA]  @TERM AS VARCHAR(30),@TOP AS BIGINT, @LIMITE25 AS BIT
AS

DECLARE @MAXLIMITE AS INT, @FTERIF as int, @SOMMABYTES AS INT, @KBFILE AS INT, @NOMEFILE as varchar(20)

SET @MAXLIMITE = 26214400  ----- 25 MB ESPRESSO IN BYTES 26214400
SET @SOMMABYTES = 0

DELETE FROM TMP_FTE_SELEZIONA WHERE TERM = @TERM

IF @LIMITE25 = 0
   BEGIN
   INSERT INTO TMP_FTE_SELEZIONA
   SELECT TOP (@TOP * 1) @TERM, FteRif,FteNomeFile
     FROM TbFte
   WHERE FteDaInviare = 1 ORDER BY FteData,FteDataOra

   UPDATE TbFte set FteDaInviare = 0
   where FteRif in (select FteRif from TMP_FTE_SELEZIONA where TERM = @TERM )

    END
 ELSE
   BEGIN
   DECLARE CURSORE CURSOR FOR
   SELECT FteRif, FteKbFile, FteNomeFile from TbFte WHERE FteDaInviare = 1 order by FteData,FteDataOra
    
   OPEN CURSORE

   FETCH NEXT FROM CURSORE
   INTO @FTERIF,@KBFILE,@NOMEFILE

	   WHILE @@FETCH_STATUS = 0  
	   BEGIN

	   SET @SOMMABYTES = @SOMMABYTES + @KBFILE

	   IF @SOMMABYTES > @MAXLIMITE
	      BEGIN
		  GOTO ESCI
		  END

       INSERT INTO TMP_FTE_SELEZIONA
	   SELECT @TERM, @FTERIF,@NOMEFILE

	   UPDATE TbFte set FteDaInviare = 0 WHERE FteRif = @FTERIF

	   FETCH NEXT FROM CURSORE
	   INTO @FTERIF,@KBFILE,@NOMEFILE
	   END

ESCI:

   CLOSE CURSORE
   DEALLOCATE CURSORE

   END

SELECT * FROM TMP_FTE_SELEZIONA WHERE TERM = @TERM

   

GO
/****** Object:  StoredProcedure [dbo].[X_GENERA_NEW_ECCEZIONE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[X_GENERA_NEW_ECCEZIONE]
as


DECLARE @NextParentId INT, @NOTE as VARCHAR(20)  

SET @NOTE = 'NUOVO EVENTO'

SELECT @NextParentId = ISNULL(MAX(ParentId), 0) + 1 FROM TbEccezioniChiusura

INSERT INTO TbEccezioniChiusura ( DataInizio, DataFine, CodiceEccezione, Slot, OraInizio, OraFine, Note, ParentID )
                      VALUES ( CAST(GETDATE() as date), CAST(GETDATE() as date), 1, 1, null, null, @NOTE, @NextParentId )

INSERT INTO TbEccezioniChiusura ( DataInizio, DataFine, CodiceEccezione, Slot, OraInizio, OraFine, Note, ParentID )
                      VALUES ( CAST(GETDATE() as date), CAST(GETDATE() as date), 1, 2, null, null, @NOTE, @NextParentId )
GO
/****** Object:  StoredProcedure [dbo].[X_GET_ORARI_ORDINE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[X_GET_ORARI_ORDINE]   @Data DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Determiniamo il giorno della settimana (0=Domenica, 1=Lunedì, ..., 6=Sabato)
    DECLARE @GiornoSettimana INT = (DATEPART(dw, @Data) + @@DATEFIRST - 1) % 7;

    -- 2. Tabella temporanea per contenere gli slot di apertura
    DECLARE @Aperture TABLE (Inizio TIME, Fine TIME);

    -- 3. Verifichiamo se esiste un'eccezione "Orario Speciale" (Codice 2)
    IF EXISTS (SELECT 1 FROM TbEccezioniChiusura WHERE @Data BETWEEN DataInizio AND DataFine AND CodiceEccezione = 2)
    BEGIN
        INSERT INTO @Aperture (Inizio, Fine)
        SELECT OraInizio, OraFine 
        FROM TbEccezioniChiusura 
        WHERE @Data BETWEEN DataInizio AND DataFine 
          AND CodiceEccezione = 2
          AND OraInizio IS NOT NULL;
    END
    ELSE
    -- 4. Altrimenti verifichiamo l'orario standard
    BEGIN
        INSERT INTO @Aperture (Inizio, Fine)
        SELECT OraInizio, OraFine 
        FROM TbOrariStandard 
        WHERE GiornoSettimana = @GiornoSettimana 
          AND IsChiuso = 0
          AND OraInizio IS NOT NULL;
    END; -- <--- IL PUNTO E VIRGOLA QUI È FONDAMENTALE

    -- 5. Generazione ricorsiva degli orari ogni 30 minuti
    WITH OrariRicorsivi AS (
        -- Punto di partenza: l'inizio di ogni slot
        SELECT Inizio AS OraSlot, Fine
        FROM @Aperture
        
        UNION ALL
        
        -- Aggiungiamo 30 minuti finché non raggiungiamo l'orario di fine
        SELECT DATEADD(minute, 30, OraSlot), Fine
        FROM OrariRicorsivi
        WHERE DATEADD(minute, 30, OraSlot) <= Fine
    )
    
    -- 6. Risultato finale
    SELECT DISTINCT 
        CAST(OraSlot AS TIME(0)) AS OraSlot
    FROM OrariRicorsivi
    ORDER BY OraSlot
    OPTION (MAXRECURSION 0); -- Previene errori se l'intervallo è molto lungo
END
GO
/****** Object:  StoredProcedure [dbo].[X_LEGGI_ART_BARCODE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_LEGGI_ART_BARCODE] @BARCODE AS VARCHAR(50)
AS

DECLARE @ARTID AS int = 0

set @ARTID = isnull((select ArtId FROM TbArt WHERE ArtCod = @BARCODE ),0)

if @ARTID = 0
   BEGIN
    SET @ARTID = isnull((select ArtId FROM TbArt WHERE ArtEAN13 = @BARCODE ),0)
   END

if @ARTID = 0
   BEGIN
    SET @ARTID = isnull((select ArtId FROM TbArt WHERE ArtBarcodeForn = @BARCODE ),0)
   END

select * from TbArt where Artid = @ARTID
GO
/****** Object:  StoredProcedure [dbo].[X_LEGGI_ORD_TORTA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROC [dbo].[X_LEGGI_ORD_TORTA] @NUMRIF AS INT = 0
AS

DECLARE @INTERVALLO AS INT = 2

if @NUMRIF <> 0
   BEGIN
		SELECT SosRif,
		       IdCli = SosPrivato,
			   SosNum,
			   SosData,
			   SosConsegna,
			   PrivCognomeNome,
			   PrivTelefono,
			   PrivMail,
			   PrivIndirizzo,
			   PrivCap,
			   PrivCitta,
			   PrivProv,
			   SosNote,
			   CorAnnota,
			   SosOraRitiro,
			   SosStampato,
			   SosStampaNum,
			   URGENTE = CASE WHEN DATEDIFF(DAY,CAST(GETDATE() AS DATE),SosConsegna) < @INTERVALLO then CAST(1 AS BIT) else CAST(0 AS BIT) END
		from TbSos inner join
			 TbPrivati on PrivId = SosPrivato inner join
			 TbCor on Cortipodoc = SosTipodoc and Corrif = SosRif  
		WHERE SosRif = @NUMRIF AND CorProg = 1

   END
ELSE
   BEGIN
		SELECT SosRif,
		       IdCli = SosPrivato,
			   SosNum,
			   SosData,
			   SosConsegna,
			   PrivCognomeNome,
			   PrivTelefono,
			   PrivMail,
			   PrivIndirizzo,
			   PrivCap,
			   PrivCitta,
			   PrivProv,
			   SosNote,
			   CorAnnota,
			   CorDesc,
			   SosOraRitiro,
			   SosStampato,
			   SosStampaNum,
			   URGENTE = CASE WHEN DATEDIFF(DAY,CAST(GETDATE() AS DATE),SosConsegna) < @INTERVALLO then CAST(1 AS BIT) else CAST(0 AS BIT) END
		from TbSos inner join
			 TbPrivati on PrivId = SosPrivato inner join
			 TbCor on Cortipodoc = SosTipodoc and Corrif = SosRif  
		WHERE SosPrivato <> 0 AND CorProg = 1 AND SosConsegna >= CAST(GETDATE() AS DATE)
		ORDER BY SosConsegna,DATEPART(HOUR,SosOraRitiro)

   END
   

GO
/****** Object:  StoredProcedure [dbo].[X_MONITOR_ATTIVA_EVASIONE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_MONITOR_ATTIVA_EVASIONE] @TIPODOC AS VARCHAR(1), @SOSRIF AS INT
AS

DECLARE @ATTIVABILE AS BIT = 0

SELECT PTipoDoc,
       PRif,
	   PProg,
	   ORDINATA = CAST(0 AS DECIMAL(9,3)),
	   ---CONSEGNATA = sum(CASE WHEN Plotto > '' and PDataScad is not null THEN  PQTA ELSE 0 END)
	   CONSEGNATA = sum(PQTA)
INTO #TMP
from TbSosP
WHERE PTipoDoc = @TIPODOC and PRif = @SOSRIF
group by PTipoDoc,PRif,PProg

update #TMP set ORDINATA = CorQuaCon
from #TMP INNER JOIN
     TbCor on CorTipodoc = PTipoDoc and CorRif = PRif and CorProg = PProg



IF (SELECT COUNT(*) FROM #TMP WHERE ORDINATA > CONSEGNATA) = 0
   BEGIN
     SET @ATTIVABILE = 1
   END

IF @ATTIVABILE = 0
   BEGIN
    DELETE FROM TbSosPronti WHERE SosPrTipodoc = @TIPODOC and SosPrRif = @SOSRIF
   END
 
--- SELECT * FROM #TMP
SELECT @ATTIVABILE
GO
/****** Object:  StoredProcedure [dbo].[X_MONITOR_ORDINI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROC [dbo].[X_MONITOR_ORDINI] 
AS

SELECT  distinct SSosRif, ScorTipoDoc, SCorrif, DOCUMENTO = CAST('' AS VARCHAR(50))
into #tmpSCor
from TbScor
group by SSosRif, ScorTipoDoc, SCorrif

UPDATE #tmpSCor SET DOCUMENTO = 'FATTURA N. ' + FORMAT(Fatnum,'000000') + ' DEL ' + FORMAT(Fatdata,'dd/MM/yyyy')  
FROM #tmpSCor INNER JOIN
     TbFat on fatrif = SCorrif
WHERE ScorTipoDoc = 'F'

UPDATE #tmpSCor SET DOCUMENTO = 'DDT N. ' + FORMAT(BolNum,'000000') + ' DEL ' + FORMAT(BolData,'dd/MM/yyyy')
FROM #tmpSCor INNER JOIN
     TbBol on Bolrif = SCorrif
WHERE ScorTipoDoc = 'B'

select SosTipoDoc,SosRif,SosNum,SosData,SosCliCons,SosConsegna,SosStampato, SosMailConferma, SosMailPronto,
        CLIENTE = (SELECT AnaDesc from Vdox.dbo.TbAna where AnaCod = SosCliCons),
		EMAIL = (SELECT AnaEmail from Vdox.dbo.TbAna where AnaCod = SosCliCons),
		NUM_RIGHE =(SELECT COUNT(*) FROM TbCor where Cortipodoc = 'S' AND CORRIF = SosRif),
		STATO_DESC = cast(CASE WHEN ISNULL(DOCUMENTO,'') <> '' then 'EVASO' ELSE (CASE WHEN ISNULL(SosPrRif,0) = 0 THEN 'INSERITO' ELSE 'PRONTO' END) END AS VARCHAR(20)),
		PRONTO = CASE WHEN ISNULL(SosPrRif,0) = 0 THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END,
		STATO = CASE WHEN ISNULL(DOCUMENTO,'') <> '' then CAST(4 AS TINYINT) ELSE (CASE WHEN ISNULL(SosPrRif,0) = 0 THEN CAST(1 AS TINYINT) ELSE CAST(3 AS TINYINT) END) END,
		DOCUMENTO = ISNULL(DOCUMENTO,'')
into #TMP
from TbSos LEFT OUTER JOIN
     TbSosPronti on SosPrTipodoc = SosTipoDoc and SosPrRif = SosRif  left outer join
	 #tmpSCor ON SSosRif = SosRif
WHERE Sostipodoc = 'S' AND SOSPRIVATO = 0

UPDATE #TMP SET STATO = CASE WHEN (SosStampato = 1 AND STATO = 1) then 2 else STATO end,
                STATO_DESC = CASE WHEN (SosStampato = 1 AND STATO = 1) then 'IN LAVORAZIONE' else STATO_DESC end

select *
from #TMP
ORDER BY SosConsegna DESC, SosData DESC
GO
/****** Object:  StoredProcedure [dbo].[X_MOV_999999]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE   PROC [dbo].[X_MOV_999999] @ANNO AS  SMALLINT, @TIPO AS VARCHAR(1), @NUMERO AS INT, @ARTID AS INT, @CLIENTE AS VARCHAR(5), @DEP as SMALLINT, @DDT AS BIT, @FATTURE AS BIT, @PREV AS BIT, @SOS AS BIT, @CORR AS BIT, @OC AS BIT, @OF AS BIT, @QUADRI AS BIT, @SINT AS BIT, @NREG AS SMALLINT, @ARTCOD AS VARCHAR(20), @MOVMAG AS BIT
AS

DECLARE @REGNCR AS SMALLINT
set @REGNCR = (SELECT TOP 1 TaiReg11 from TbTai order by TaiAnno desc)


SELECT  TIPODOC = 'D.D.T.',BOLTIPODOC,BOLRIF,BOLNUM,BOLDATA,CORARTID,CORCODART,CORQUAORD = CAST(0 AS DECIMAL(8,2)),CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,
        CORCAU, CAUSALE =cast('' AS VARCHAR(50)), BolCliCons, CLIENTE = CAST('' AS VARCHAR(100)), WEBRIF = CAST(0 AS INT),TIPORD=CAST(''AS VARCHAR(2)), QUADRI = cast(0 as bit), NREG= CAST(0 AS SMALLINT)
into #TMP
FROM TbBol inner join
     TbCor on cortipodoc = Boltipodoc and CorRif = BolRif
WHERE BolNdep = @DEP AND
      DATEPART(year,Boldata) = (case when @ANNO = 0 then DATEPART(year,Boldata) ELSE @ANNO END) AND
      BolNum = (case when @NUMERO = 0 then BolNum ELSE @NUMERO END) AND
	  BolCliCons =  (case when @CLIENTE = '' then BolCliCons ELSE @CLIENTE END) AND
	  CorArtId = (CASE WHEN @ARTID = 0 THEN CorArTid else @ARTID END) AND
	  CorCodArt like @ARTCOD + '%' AND
      CorTipoDoc = 'B' AND (@DDT = 1  OR (@TIPO = 'B' AND @QUADRI = 0))

UNION

SELECT  TIPODOC = 'FATTURA',FATTIPODOC,FATRIF,FATNUM,FATDATA,CORARTID,CORCODART,0,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,
        CORCAU, CAUSALE = '', FatCliCons, CLIENTE = '',0,'',0,FATNUMREG
FROM TbFat inner join
     TbCor on cortipodoc = FatTipoDoc and CorRif = fatRif 
WHERE FatNdep = @DEP AND
      FatNumReg = (case when @NREG = 0 THEN FatNumreg else @NREG END) AND
      DATEPART(year,FatData) = ( case when @ANNO = 0 then DATEPART(year,FatData) ELSE @ANNO END) AND
      FatNum = (case when @NUMERO = 0 then FatNum ELSE @NUMERO END) AND
	  FatCliCons =  (case when @CLIENTE = '' then FatCliCons ELSE @CLIENTE END) AND
	  CorArtId = (CASE WHEN @ARTID = 0 THEN CorArTid else @ARTID END) AND
	  CorCodArt like @ARTCOD + '%' AND
      CorTipoDoc = 'F' AND FatDiff <> 'D' and FatNumreg <> @REGNCR AND (@FATTURE = 1 OR (@TIPO = 'F' AND @QUADRI = 0))
      
UNION

SELECT  TIPODOC = 'NOTA CREDITO',FATTIPODOC,FATRIF,FATNUM,FATDATA,CORARTID,CORCODART,0,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,
        CORCAU, CAUSALE = '', FatCliCons, CLIENTE = '',0,'',0,FATNUMREG
FROM TbFat inner join
     TbCor on cortipodoc = FatTipoDoc and CorRif = fatRif 
WHERE FatNdep = @DEP AND
      FatNumReg = (case when @NREG = 0 THEN FatNumreg else @NREG END) AND
      DATEPART(year,FatData) = ( case when @ANNO = 0 then DATEPART(year,FatData) ELSE @ANNO END) AND
      FatNum = (case when @NUMERO = 0 then FatNum ELSE @NUMERO END) AND
	  FatCliCons =  (case when @CLIENTE = '' then FatCliCons ELSE @CLIENTE END) AND
	  CorArtId = (CASE WHEN @ARTID = 0 THEN CorArTid else @ARTID END) AND
	  CorCodArt like @ARTCOD + '%' AND
      CorTipoDoc = 'F'  and FatDiff <> 'D' and FatNumreg = @REGNCR AND (@FATTURE = 1 OR @TIPO = 'F' AND (@QUADRI = 0))
   
UNION

SELECT  TIPODOC = 'PREVENTIVO',SOSTIPODOC,SOSRIF,SOSNUM,SOSDATA,CORARTID,CORCODART,0,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,
        CORCAU, CAUSALE = '', SosCliCons, CLIENTE = '',0,'',0,0
FROM TbSos inner join
     TbCor on cortipodoc = SosTipoDoc and CorRif = SosRif 
WHERE sOSnDEP = @DEP AND
      DATEPART(year,SosData) = ( case when @ANNO = 0 then DATEPART(year,SosData) ELSE @ANNO END) AND
      SosNum = (case when @NUMERO = 0 then SosNum ELSE @NUMERO END) AND
	  SosCliCons =  (case when @CLIENTE = '' then SosCliCons ELSE @CLIENTE END) AND
	  CorArtId = (CASE WHEN @ARTID = 0 THEN CorArTid else @ARTID END) AND
	  CorCodArt like @ARTCOD + '%' AND
      CorTipoDoc = 'P' AND (@PREV = 1 OR @TIPO = 'P')
      
UNION

SELECT  TIPODOC = 'ORDINE TEL.',SOSTIPODOC,SOSRIF,SOSNUM,SOSDATA,CORARTID,CORCODART,0,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,
        CORCAU, CAUSALE = '', SosCliCons, CLIENTE = '',SosWebRif,'',0,0
FROM TbSos inner join
     TbCor on cortipodoc = SosTipoDoc and CorRif = SosRif 
WHERE sOSnDEP = @DEP AND
      DATEPART(year,SosData) = ( case when @ANNO = 0 then DATEPART(year,SosData) ELSE @ANNO END) AND
      SosNum = (case when @NUMERO = 0 then SosNum ELSE @NUMERO END) AND
	  SosCliCons =  (case when @CLIENTE = '' then SosCliCons ELSE @CLIENTE END) AND
	  CorArtId = (CASE WHEN @ARTID = 0 THEN CorArTid else @ARTID END) AND
	  CorCodArt like @ARTCOD + '%' AND
      CorTipoDoc = 'S'  AND (@SOS = 1 OR @TIPO = 'S')

UNION

SELECT  TIPODOC = CASE WHEN TESTIPODOC = 'D' THEN 'D.D.T.' 
                       WHEN TESTIPODOC = 'X' THEN 'FATTURA' END,
        TESTIPODOC,TESRIF,TESNUM,TESDATA,CORARTID,CORCODART,0,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,
        CORCAU, CAUSALE = '', TesCliFor, CLIENTE = '',0,'',0,0
FROM TbTes inner join
     TbCor on cortipodoc = TesTipoDoc and CorRif = TesRif 
WHERE TesNDep = @DEP AND
      DATEPART(year,TesData) = ( case when @ANNO = 0 then DATEPART(year,TesData) ELSE @ANNO END) AND
      TesNum = (case when @NUMERO = 0 then TesNum ELSE @NUMERO END) AND
	  TesCliFor =  (case when @CLIENTE = '' then TesCliFor ELSE @CLIENTE END) AND
	  CorArtId = (CASE WHEN @ARTID = 0 THEN CorArTid else @ARTID END) AND
	  CorCodArt like @ARTCOD + '%' AND
      (CorTipoDoc = 'D' OR CorTipoDoc = 'X') AND @MOVMAG = 1

UNION

SELECT  TIPODOC = 'CORRISPETTIVO',FATTIPODOC,FATRIF,FATNUM,FATDATA,CORARTID,CORCODART,0,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,
        CORCAU, CAUSALE = '', FatCliCons, CLIENTE = '',0,'',1,0
FROM GQUA.DBO.TbFat inner join
     GQUA.DBO.TbCor on cortipodoc = FatTipoDoc and CorRif = fatRif
WHERE FatNdep = @DEP AND 
      DATEPART(year,FatData) = ( case when @ANNO = 0 then DATEPART(year,FatData) ELSE @ANNO END) AND
      FatNum = (case when @NUMERO = 0 then FatNum ELSE @NUMERO END) AND
	  FatCliCons =  (case when @CLIENTE = '' then FatCliCons ELSE @CLIENTE END) AND
	  CorArtId = (CASE WHEN @ARTID = 0 THEN CorArTid else @ARTID END) AND 
	  CorCodArt like @ARTCOD + '%' AND
      CorTipoDoc = 'F'  and FatDiff <> 'D'  AND (@CORR = 1 OR (@TIPO = 'F' AND @QUADRI = 1))
     

update #TMP SET
       CAUSALE = isnull((SELECT MgCauDesc from TbTCau where MgCauId = CorCau),''),
	   CLIENTE = isnull((SELECT AnaDesc from vdox.dbo.TbAna where AnaCod = BolCliCons),'')




IF @SINT = 1  
   BEGIN
   SELECT Tipo = BOLTIPODOC,
          DesTipo = TIPODOC,
		  Numero = BolNum,
		  Data = BolData,
		  Cliente = BolCliCons,
		  RagSoc = CLIENTE,
		  Numrif = BolRif,
		  Cau = CorCau,
		  WebRif, 
		  Deposito = @DEP,
		  CAUSALE
	FROM #TMP 
	GROUP BY BOLTIPODOC,TIPODOC,BolNum,BolData,BolCliCons,CLIENTE,BolRif,CorCau,WebRif, CAUSALE
	order by data desc,RagSoc
	END
ELSE 
    BEGIN		    
    select  * from #TMP order by BolData desc
	END

GO
/****** Object:  StoredProcedure [dbo].[X_MOV_BFP]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[X_MOV_BFP] @ARTID AS INT, @CLIENTE AS VARCHAR(5), @TIPODOC AS VARCHAR(2), @NUMRIF AS INT 
AS

SELECT  TIPODOC = 'D.D.T.',BOLTIPODOC,BOLRIF,BOLNUM,BOLDATA,CORARTID,CORCODART,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,CORUM
into #TMP
FROM TbBol inner join
     TbCor on cortipodoc = Boltipodoc and CorRif = BolRif
WHERE CorTipoDoc = 'B' AND BolCliCons = @CLIENTE and CorArtId = @ARTID AND CORCAU = 5

UNION

SELECT  TIPODOC = 'FATTURA',FATTIPODOC,FATRIF,FATNUM,FATDATA,CORARTID,CORCODART,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,CORUM

FROM TbFat inner join
     TbCor on cortipodoc = FatTipoDoc and CorRif = fatRif
WHERE CorTipoDoc = 'F' AND FatCliCons = @CLIENTE and CorArtId = @ARTID and fatCau = 5 and FatDiff <> 'D'

UNION

SELECT  TIPODOC = 'PREVENTIVO',SOSTIPODOC,SOSRIF,SOSNUM,SOSDATA,CORARTID,CORCODART,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,CORUM

FROM TbSos inner join
     TbCor on cortipodoc = SosTipoDoc and CorRif = SosRif
WHERE CorTipoDoc = 'P' AND SosCliCons = @CLIENTE and CorArtId = @ARTID  

UNION

SELECT  TIPODOC = 'ORDINE TEL.',SOSTIPODOC,SOSRIF,SOSNUM,SOSDATA,CORARTID,CORCODART,CORQUACON,CORPREZZO,CORSC1,CORSC2,CORSC3,CORNETTO,CORUM

FROM TbSos inner join
     TbCor on cortipodoc = SosTipoDoc and CorRif = SosRif
WHERE CorTipoDoc = 'S' AND SosCliCons = @CLIENTE and CorArtId = @ARTID  



DELETE FROM #TMP WHERE (BOLTIPODOC = @TIPODOC AND BOLRIF = @NUMRIF) --or CORARTID = 0 OR CORCODART = 'FF'

select TOP 20 * from #TMP order by BolData desc

      

GO
/****** Object:  StoredProcedure [dbo].[X_RECUPERA_BOLLO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROC [dbo].[X_RECUPERA_BOLLO] @FATRIF AS INT
AS

DECLARE @REC_BOLLO AS BIT, @LIM_BOLLO AS DECIMAL(9,2), @IMP_BOLLO AS DECIMAL(9,2),@COD_BOLLO AS VARCHAR(20), @NIMP AS SMALLINT, @SOGG_BOLLO AS BIT,  @TOT_IMPO AS DECIMAL(11,2), @MAX_PROG AS INT

SELECT top 1 @REC_BOLLO=TaiRecBollo, @LIM_BOLLO=TaiLimBollo, @IMP_BOLLO=TaiImpBollo, @COD_BOLLO= TaiCodBollo
from TbTai order by TaiAnno desc

-----elimina il recupero bollo dalla fattura in quanto non sono stati ancora fatti i dovuti controlli
DELETE FROM TbCor
WHERE CorTipodoc = 'F' AND CORRIF = @FATRIF and CorCodArt = @COD_BOLLO

-----------------------------------------------------------------------------------------------

SET @NIMP = ISNULL((select FatNimp FROM TbFat WHERE FATRIF = @FATRIF),0)

IF @REC_BOLLO = 0
   BEGIN
    GOTO ESCI
   END

IF @NIMP = 0
   BEGIN
    GOTO ESCI
   END

SET @SOGG_BOLLO = isnull((SELECT CiiSoggBollo FROM COGE.DBO.TbCii WHERE CiiCod = @NIMP),0)

IF @SOGG_BOLLO = 0
   BEGIN
    GOTO ESCI
   END

SET @TOT_IMPO = isnull((SELECT SUM(CorImporto) from TbCor WHERE CorTipodoc = 'F' AND CORRIF = @FATRIF),0)


IF @TOT_IMPO < @LIM_BOLLO
   BEGIN
    GOTO ESCI
   END

SET @MAX_PROG = (select isnull(max(CorProg),0) from TbCor WHERE CorTipodoc = 'F' AND CORRIF = @FATRIF)

insert into TbCor (CorTipoDoc, CorRif, CorProg, CorArtID, CorCodArt, CorDesc, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorSc4, CorSc5, CorNetto, CorImporto, CorUrgenza, CorSpot, CorPromo, CorAssortito, CorVariato, CorCampagna, 
                   CorCiva, CorCntrp, CorCau, CorMacro, CorOrdRif, CorOrdine, CorArtFor, CorPlRif, CorPlCassa, CorImballo, CorMg1, CorMg2, CorMg3, CorMerc, CorBrand, CorOrdProg, CorUM)
select  'F', @FATRIF,CorProg = @MAX_PROG + 1 ,ArtId, ArtCod,ArtDesc,1,@IMP_BOLLO,0,0,0,0,0,
       @IMP_BOLLO,@IMP_BOLLO,0,0,'',0,0,'',ArtCodiva,ArtCptcon,5,'',
       0,'','',0,0,0,0,0,0,0,0,0,''   
from TbArt
WHERE ArtCod = @COD_BOLLO

ESCI:
GO
/****** Object:  StoredProcedure [dbo].[X_REG_ORD_TORTA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROC [dbo].[X_REG_ORD_TORTA] @NUMRIF AS INT, @IDCLI AS INT, @DATA AS SMALLDATETIME, @CONSEGNA AS SMALLDATETIME,@COGNOMENOME AS VARCHAR(100), @TEL AS VARCHAR(20), @MAIL AS VARCHAR(50), @INDIRIZZO AS VARCHAR(50),
                            @CAP AS VARCHAR(5), @CITTA AS VARCHAR(50), @PROV AS VARCHAR(2), @NOTE AS VARCHAR(200), @TORTA AS VARCHAR(MAX), @TERM AS VARCHAR(30), @ORACONSEGNA AS DATETIME
AS

DECLARE @CLIENTE AS VARCHAR(5), @COD_TORTE AS VARCHAR(10), @PAGCOD as int

SELECT TOP 1 @CLIENTE = TaiCliTorte, @COD_TORTE = TaiCodTorte
FROM TbTai 
ORDER BY TaiAnno DESC

SET @PAGCOD = (select ClPagam from TbCli WHERE ClCod = @CLIENTE)


----------------- inserimento eventuale cliente nuovo ( altrienti aggiornamento dati Cliente)

IF @IDCLI <> 0
   BEGIN
     UPDATE TbPrivati SET PrivCognomeNome = @COGNOMENOME, PrivTelefono = @TEL, PrivMail = @MAIL, PrivIndirizzo = @INDIRIZZO, PrivCap = @CAP, PrivCitta = @CITTA, PrivProv = @PROV
	 WHERE PrivId = @IDCLI
   END

ELSE
   BEGIN

     INSERT INTO TbPrivati (PrivCognomeNome, PrivTelefono, PrivMail, PrivIndirizzo, PrivCap, PrivCitta, PrivProv)
	 SELECT @COGNOMENOME, @TEL, @MAIL, @INDIRIZZO, @CAP, @CITTA, @PROV
	 
	 SET @IDCLI = (SELECT @@IDENTITY)

   END


----------------------------- INSERIMENTO / AGGIORNAMENTO ORDINE

IF @NUMRIF = 0
   BEGIN

     Insert into TbSos WITH (TABLOCKX) (SosTipoDoc,SosData,SosNum,SosAge,SosCliCons,SosPagCod,SosAbi,SosCab,SosEsespe,SosNimp,SosCau,SosSculter,SosCliFat,SosNrag,SosOperatore,SosNdep,SosVardest,SosFP,SosNote,SosPvv,SosConsegna,SosPrivato,SosOraRitiro) 
      SELECT 'S',@DATA,999999,0,@CLIENTE,@PAGCOD,0,0,0,0,8,0,@CLIENTE,0,@TERM,0,'',0,@NOTE,0,@CONSEGNA,@IDCLI,@ORACONSEGNA
      FROM TbCli where CLCod = @CLIENTE

     SET @NUMRIF = (SELECT @@IDENTITY)


	 insert into TbCor( CorTipoDoc, CorRif, CorProg, CorArtID, CorCodArt, CorDesc, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorSc4, CorSc5, CorNetto, CorImporto, CorUrgenza, CorSpot, CorPromo, CorAssortito, CorVariato, CorCampagna, 
                         CorCiva, CorCntrp, CorCau, CorMacro, CorOrdRif, CorOrdine, CorArtFor, CorPlRif, CorPlCassa, CorImballo, CorMg1, CorMg2, CorMg3, CorMerc, CorBrand, CorOrdProg, CorUM, CorAnnota)
	 select 'S' AS CorTipoDoc,@Numrif as CorRif,CorProg = 1 ,ArtId,ArtCod,ArtDesc,1,LISTINO,0,0,0,0,0,
       LISTINO,LISTINO,0,0,'',0,0,'',ArtCodiva,artcptcon,8 as CorCau,0,
       0,'','',0,0,0,0,0,0,0,0,0,ArtUmVen, @TORTA    
	 FROM VArtCat
	 WHERE ArtCod = @COD_TORTE

   END

ELSE
   BEGIN
     UPDATE TbSos SET SosConsegna = @CONSEGNA, SosPrivato = @IDCLI, SosNote = @NOTE, SosOraRitiro = @ORACONSEGNA
	 WHERE SosRif = @NUMRIF

	 update TBCor set CorAnnota = @TORTA
	 WHERE Cortipodoc = 'S' and Corrif = @NUMRIF and CorProg = 1


   END


 select @NUMRIF
GO
/****** Object:  StoredProcedure [dbo].[X_REGISTRA_PRODOTTO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       PROC [dbo].[X_REGISTRA_PRODOTTO] @ArtId as int, @ArtCod as varchar(20), @ArtDesc as varchar(200), @ArtMiniDesc as varchar(23), @ArtCat as varchar(2), @ArtUMisura as varchar(2), @ArtCodiva as smallint, 
                                @ArtCptCon as varchar(5), @ArtFlagAttivo as bit, @ArtUmTec as varchar(2), @ArtUmVen as varchar(2), @ArtCiAcq as smallint, @ArtCptAcq as varchar(5), @VALIDITA as SMALLDATETIME,
								@PREZZO as DECIMAL(10,3), @TARIC AS VARCHAR(16), @BARCODE AS VARCHAR(40), @NumImballo as smallint, @ArtBilancia as bit,@ArtBarcodeForn as varchar(50), @ArtEAN13  as varchar(13),
								@ArtDiBase as smallint, @OLDPREZZO as DECIMAL(10,3), @TERM VARCHAR(20), @MODIFICATO AS BIT = 0

AS

IF @ArtId = 0
   BEGIN

   INSERT INTO TbArt (ArtCod,ArtDesc,ArtMiniDesc,ArtCat,ArtUMisura,ArtCodiva,ArtCptCon,ArtFlagAttivo,ArtUmTec,ArtUmVen,ArtCiAcq,ArtCptAcq,ArtBarcode,ArtNumImballo, ArtBilancia,ArtBarcodeForn,ArtEAN13,ArtDiBase)
   SELECT @ArtCod,@ArtDesc,@ArtMiniDesc,@ArtCat,@ArtUMisura,@ArtCodiva,@ArtCptCon,@ArtFlagAttivo,@ArtUmTec,@ArtUmVen,@ArtCiAcq,@ArtCptAcq,@BARCODE, @NumImballo, @ArtBilancia,@ArtBarcodeForn,@ArtEAN13,@ArtDiBase

   SET @ArtId = (SELECT @@IDENTITY)

   insert into TbLis(LisId,LisValiditaDal,Lisvendita)
   SELECT @ArtId,@VALIDITA,@PREZZO

   END
ELSE
   BEGIN

   UPDATE TbArt set ArtDesc=@ArtDesc, ArtMiniDesc=@ArtMiniDesc, ArtCat=@ArtCat, ArtUMisura=@ArtUMisura, ArtCodiva=@ArtCodiva, ArtCptCon=@ArtCptCon, ArtFlagAttivo=@ArtFlagAttivo,
                    ArtUmTec=@ArtUmTec, ArtUmVen=@ArtUmVen, ArtCiAcq=@ArtCiAcq, ArtCptAcq=@ArtCptAcq, ArtBarcode=@BARCODE, ArtNumImballo=@NumImballo, ArtBilancia=@ArtBilancia,
					ArtBarcodeForn=@ArtBarcodeForn, ArtEAN13=@ArtEAN13, ArtDiBase=@ArtDiBase
   WHERE ArtId = @artid

   UPDATE TbLis SET Lisvendita = @PREZZO
   WHERE LISID = @ArtId AND LisValiditaDal = @VALIDITA

   IF @PREZZO <> @OLDPREZZO
      BEGIN
	    UPDATE TbArt set ArtUlt_modifica = GETDATE(), ArtUtente = @TERM, ArtAUlt_modifica = GETDATE(), ArtAUtente = @TERM
		WHERE ArtId = @artid
	  END

	IF @MODIFICATO = 1
	  BEGIN
	    UPDATE TbArt set ArtAUlt_modifica = GETDATE(), ArtAUtente = @TERM
		WHERE ArtId = @artid
	  END
   END

DELETE FROM TbArtDG WHERE ADGArtCod = @ArtCod

IF @TARIC <> ''
   BEGIN

	INSERT INTO TbArtDG
	SELECT @ArtCod,@TARIC

   END

------GENERAZIONE CODICI BILANCIA

DECLARE @CODBIL AS INT = (select ArtCodBil from TbArt WHERE Artid = @ArtId)

DECLARE @MAXCODBIL AS INT = (SELECT isnull(MAX(ArtCodBil),12) from TbArt)

IF @MAXCODBIL = 0
   BEGIN
     SET  @MAXCODBIL = 12
   END


IF @ArtBilancia = 1
   BEGIN
     IF @CODBIL = 0
	    BEGIN
		  UPDATE TbArt set ArtCodBil = @MAXCODBIL + 1
		  WHERE ArtId = @ArtId

		END
    END
ELSE
   BEGIN
      UPDATE TbArt set ArtCodBil = 0
		  WHERE ArtId = @ArtId

   END


select @ARTID
GO
/****** Object:  StoredProcedure [dbo].[X_REGISTRA_TESTATA_BANCO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_REGISTRA_TESTATA_BANCO] @DEP AS SMALLINT, @TIPODOC AS VARCHAR(1), @NUMRIF AS INT, @DATA AS SMALLDATETIME, @CLIENTE AS VARCHAR(5), @PAGCOD AS SMALLINT, @NRAG AS bit,
                                      @ABI AS int, @CAB AS int, @ESESPE AS BIT, @NIMP AS SMALLINT, @CAU AS SMALLINT, @SCULTER AS DECIMAL(5,2), @NUMREG AS SMALLINT, @OPERATORE AS VARCHAR(20), @TOTALE AS DECIMAL(11,2),
									  @VARDEST AS VARCHAR(100),@FP AS BIT,@NOTE AS VARCHAR(200), @UTENTE AS VARCHAR(50) = '', @PVV AS VARCHAR(5) = '', @CONSEGNA AS SMALLDATETIME = NULL, @PUNTIACCESSO AS VARCHAR(1) = '',
									  @LISTINO AS SMALLINT = 0
									
AS


DECLARE @AGENTE AS SMALLINT
SET @AGENTE = (SELECT ClCodage FROM TbCli where ClCod = @CLIENTE)


IF @TIPODOC = 'B'
   BEGIN
   IF @NUMRIF = 0
      BEGIN 
	  Insert into TbBol (BolTipoDoc,BolData,BolNum,BolAge,BolRifFat,BolCliCons,BolPagCod,BolAbi,BolCab,BolEsespe,BolNimp,BolCau,BolSculter,BolCliFat,BolNumReg,BolCF,BolPorto,BolNrag,BolNDep,BolNote, BolListino) 
	  SELECT @TIPODOC,@DATA,999999, CASE WHEN @AGENTE > 0 THEN @AGENTE ELSE 99 END,0,@CLIENTE,@PAGCOD,@ABI,@CAB,@ESESPE,@NIMP,@CAU,@SCULTER,@CLIENTE,@NUMREG,'C','ASSEGNATO',0,@DEP,@NOTE, @LISTINO
	  FROM TbCli where CLCod = @CLIENTE

	  SET @NUMRIF = (SELECT @@IDENTITY)	  

	  Update TbBol set BolNrag = CASE WHEN @NRAG = 1 THEN 0 else BolRif end
	  where BolRif = @NUMRIF
	  END
	ELSE
	  BEGIN
	  Update TbBol set BolData=@Data, BolPagCod =@PagCod, BolAbi=@Abi, BolCab=@Cab, BolEsespe = @Esespe, BolNimp=@Nimp, BolSculter = @Sculter, 
	                   BolNrag = CASE WHEN @NRAG = 1 THEN 0 else BolRif end, 
					   BolCliCons = @Cliente,BolCliFat = @Cliente,
	                   BolAge = CASE WHEN @AGENTE > 0 THEN @AGENTE ELSE 99 END,
					   BolTotale = @TOTALE,BolNote=@NOTE, BolListino=@LISTINO
	  where BolRif = @NUMRIF
	  END
	END

IF @TIPODOC = 'F'
   BEGIN
   IF @NUMRIF = 0
      BEGIN
      Insert into TbFat WITH (TABLOCKX) (FatTipoDoc,FatData,FatNum,FatAge,FatCliCons,FatPagCod,FatAbi,FatCab,FatEsespe,FatNimp,FatCau,FatSculter,FatCliFat,FatNumReg,FatPorto,FatTrasf,FatNDep,FatNote, FatListino) 
      SELECT @TIPODOC,@DATA,999999, CASE WHEN @AGENTE > 0 THEN @AGENTE ELSE 99 END,@CLIENTE,@PAGCOD,@ABI,@CAB,@ESESPE,@NIMP,@CAU,@SCULTER,@CLIENTE,@NUMREG,'ASSEGNATO',0,@DEP,@NOTE, @LISTINO
      FROM TbCli where CLCod = @CLIENTE

      SET @NUMRIF = (SELECT @@IDENTITY)

      END
  ELSE
     BEGIN
      Update TbFat set FatData=@Data, FatPagCod =@PagCod, FatAbi=@Abi, FatCab=@Cab, FatEsespe = @Esespe, FatNimp=@Nimp, FatSculter = @Sculter,FatCliCons = @Cliente, fatCliFat = @Cliente, 
      FatAge =  CASE WHEN @AGENTE > 0 THEN @AGENTE ELSE 99 END,
	  FatNote=@NOTE,  FatListino=@LISTINO
     where FatRif = @NUMRIF
     END
	END

IF @TIPODOC = 'S' OR @TIPODOC = 'P'
   BEGIN
   IF @NUMRIF = 0
      BEGIN
      Insert into TbSos WITH (TABLOCKX) (SosTipoDoc,SosData,SosNum,SosAge,SosCliCons,SosPagCod,SosAbi,SosCab,SosEsespe,SosNimp,SosCau,SosSculter,SosCliFat,SosNrag,SosOperatore,SosNdep,SosVardest,SosFP,SosNote,SosPvv, SosConsegna, SosAccesso) 
      SELECT @TIPODOC,@DATA,999999,0,@CLIENTE,@PAGCOD,@ABI,@CAB,@ESESPE,@NIMP,@CAU,@SCULTER,@CLIENTE,0,@OPERATORE,@DEP,@VARDEST,@FP,@NOTE,@PVV,@CONSEGNA, @PUNTIACCESSO
      FROM TbCli where CLCod = @CLIENTE

      SET @NUMRIF = (SELECT @@IDENTITY)

      END
  ELSE
     BEGIN
      Update TbSos set SosData=@Data, SosPagCod =@PagCod, SosAbi=@Abi, SosCab=@Cab, SosEsespe = @Esespe, SosNimp=@Nimp, SosSculter = @Sculter,SosCliCons = @Cliente, SosCliFat = @Cliente,
	                   Sosvardest= @VARDEST, SosFP=@FP, SosNote=@NOTE, SosPvv=@PVV, SosConsegna= @CONSEGNA, SosAccesso = @PUNTIACCESSO
      where SosRif = @NUMRIF
     END
	END


SELECT @NUMRIF
GO
/****** Object:  StoredProcedure [dbo].[X_RIATTIVA_SOS]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROC [dbo].[X_RIATTIVA_SOS] @TIPO AS VARCHAR(1), @RIFER AS INT, @PROG AS INT
AS

SELECT *
INTO #TMPSOS
FROM TbSCor 
where SCorTipoDoc = @TIPO AND SCorRif = @RIFER

if @PROG > 0
   BEGIN
   DELETE FROM #TMPSOS WHERE SCorProg <> @PROG
   END


select TmpSCor.* 
INTO #TMPMAG from TmpSCor INNER JOIN
                  (select distinct SSosTipoDoc, SSosRif, SSosProg from #TMPSOS) a ON a.SSosTipoDoc = CorTipoDoc and a.SSosRif = CorRif and a.SSosProg = CorProg
WHERE a.SSosTipoDoc <> 'P'


Insert Into TbCor SELECT * FROM #TMPMAG

UPDATE TbSos set Sostrasf=0 where Sosrif in (select SSosRif from #TMPSOS)  

delete from TmpSCor
from TmpSCor INNER JOIN
     (select distinct SSosTipoDoc, SSosRif, SSosProg from #TMPSOS) a ON a.SSosTipoDoc = CorTipoDoc and a.SSosRif = CorRif and a.SSosProg = CorProg
WHERE a.SSosTipoDoc <> 'P'

IF @PROG = 0
   BEGIN
   delete from TbSCor where SCorTipoDoc = @TIPO AND SCorRif = @RIFER  
   END
 ELSE
   BEGIN
   delete from TbSCor where SCorTipoDoc = @TIPO AND SCorRif = @RIFER AND SCorprog = @PROG  
   END


--------------------RIAGGIORNAMENTO MAGAZZINO (SOLO SE E' DI TIPO 'S')


--DECLARE  @ARTID AS INT, @CAUSALE AS SMALLINT, @QTA AS DECIMAL(7,2), @IMPORTO AS DECIMAL(9,2), @SEGNO AS SMALLINT , @DEP AS SMALLINT, @DATADOC AS smalldatetime,
--         @ORDRIF AS INT

--SELECT TOP 1 @DEP = SosNdep,
--             @DATADOC = SosData
--FROM TbSos where Sosrif in (select SSosRif from #TMPSOS)  AND SOSTIPODOC <> 'P'


--SET @SEGNO = 1

--DECLARE PIPPO CURSOR FOR
--select CORARTID,CORQUACON,CORIMPORTO,CORCAU,CORORDRIF 
--from #TMPMAG 

--OPEN PIPPO
--FETCH NEXT FROM PIPPO
--INTO  @ARTID, @QTA, @IMPORTO, @CAUSALE, @ORDRIF

--WHILE @@FETCH_STATUS = 0
--      BEGIN

	
--	    EXEC GEVE.DBO.X_AGG_QTAVAL @DEP, @DATADOC,@ARTID,@CAUSALE,@QTA,@IMPORTO,@SEGNO

--	    Update TbRor set RorQuaCon = RorQuaCon + @QTA where RorRif = @ORDRIF and RorArtID = @ARTID


--	  FETCH NEXT FROM PIPPO
--      INTO  @ARTID, @QTA, @IMPORTO, @CAUSALE, @ORDRIF
--	  END


--CLOSE PIPPO
--DEALLOCATE PIPPO

GO
/****** Object:  StoredProcedure [dbo].[X_RIGHE_MONITOR_ADD]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_RIGHE_MONITOR_ADD] @TIPODOC AS VARCHAR(1), @SOSRIF AS INT,  @SOSPROG AS INT
AS

SELECT TOP 1 *
INTO #TMP
FROM TbSosP
WHERE PTipoDoc = @TIPODOC AND PRif = @SOSRIF AND PProg = @SOSPROG
ORDER BY PCProg desc

UPDATE #TMP SET PCProg = PCPROG + 1,
                PQta = 0,
				PLotto = '',
				PDataScad = NULL

insert into TbSosP SELECT * FROM #TMP
GO
/****** Object:  StoredProcedure [dbo].[X_RIGHE_MONITOR_ORDINI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   proc [dbo].[X_RIGHE_MONITOR_ORDINI] @TIPODOC AS VARCHAR(1), @SOSRIF AS INT
AS

INSERT into TbSosP (PTipoDoc, PRif, PProg, PCProg, PQta)
SELECT CorTipodoc, Corrif, CorProg, 1, 0
FROM TbCor
WHERE CorTipodoc = @TIPODOC AND Corrif = @SOSRIF
      AND CorProg NOT IN (SELECT PProg from TbSosP WHERE PTipoDoc = @TIPODOC AND PRif = @SOSRIF)


SELECT PTipoDoc, PRif, PProg, PCProg,
       CorArtId,
	   CODICE = CorCodArt,
	   DESCRIZIONE = CorDesc,
	   UM = CorUM,
	   ORDINATA = CorQuaCon,
	   CONSEGNATA = PQta,
	   LOTTO = PLotto,
	   SCADENZA = PDataScad


FROM TbSosP INNER JOIN
     TbCor on CorTipodoc = PTipoDoc and Corrif = PRif and CorProg = PProg
WHERE CorTipodoc = @TIPODOC AND Corrif = @SOSRIF
GO
/****** Object:  StoredProcedure [dbo].[X_SALVA_LIST_VENDITA_CLI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_SALVA_LIST_VENDITA_CLI] @TERM AS VARCHAR(40), @CLIENTE AS VARCHAR(5)
AS

DECLARE @GRUPPO AS SMALLINT, @CLI_IN_CORSO AS VARCHAR(5)
set @GRUPPO = (select ClGrCanale from TbCli WHERE ClCod = @CLIENTE)

SET @CLI_IN_CORSO = (select top 1 CLIENTE FROM TMP_LISTVENDITA WHERE TERM = @TERM)

IF @CLI_IN_CORSO <> @CLIENTE
   BEGIN
     GOTO ESCI
   END

IF (SELECT COUNT(*) FROM TMP_LISTVENDITA WHERE TERM = @TERM) = 0
  BEGIN
       GOTO ESCI
  END

DELETE FROM TbSlCli WHERE SlCli = @CLIENTE

INSERT INTO TbSlCli (SlCli,SlArtCod,SlNo,SlCliPrezzo,SlUlt_modifica,SlUtente)
SELECT @CLIENTE, CODICE, 0, PREZZO, ultima_modifica, Utente
FROM TMP_LISTVENDITA
WHERE TERM = @TERM AND PRESENTE = 1 --- and CODICE NOT IN (SELECT SlArtCod from TbSlAtt where SlAtt = @GRUPPO and SlAttPrezzo = PREZZO)

delete from TbSlCli
from TbSlcli a inner join
	 TbSlAtt b on b.SlArtCod = a.SlArtcod and SlCliPrezzo = slAttPrezzo
WHERE SlCli = @CLIENTE AND SlAtt = @GRUPPO

INSERT INTO TbSlCli (SlCli,SlArtCod,SlNo)
SELECT @CLIENTE, CODICE, 1
FROM TMP_LISTVENDITA
WHERE TERM = @TERM AND PRESENTE = 0 and CODICE IN (SELECT SlArtCod from TbSlAtt where SlAtt = @GRUPPO)

ESCI:


































GO
/****** Object:  StoredProcedure [dbo].[X_SALVA_LIST_VENDITA_GR]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROC [dbo].[X_SALVA_LIST_VENDITA_GR] @TERM AS VARCHAR(40), @GRUPPO AS SMALLINT
AS

DECLARE @GRUPPO_IN_CORSO AS SMALLINT

SET @GRUPPO_IN_CORSO = (SELECT TOP 1 GRUPPO FROM TMP_LISTVENDITA WHERE TERM = @TERM)

IF (SELECT COUNT(*) FROM TMP_LISTVENDITA WHERE TERM = @TERM) = 0
  BEGIN
     GOTO ESCI
  END

IF @GRUPPO_IN_CORSO <> @GRUPPO
   BEGIN
     GOTO ESCI
   END


IF @GRUPPO <> 0
   BEGIN
		DELETE FROM TbSlAtt WHERE SlAtt = @GRUPPO

	     INSERT INTO TbSlAtt (SlAtt,SlArtCod,SlAttPrezzo,SlUlt_modifica,SlUtente,SlStampa)
		 SELECT @GRUPPO, CODICE, PREZZO, ultima_modifica, Utente, STAMPALIS
		 FROM TMP_LISTVENDITA
		 WHERE TERM = @TERM AND PRESENTE = 1
   END
ELSE
   BEGIN
		 UPDATE TbLis SET LisVendita = PREZZO
		 FROM TbLis  inner join
		      TMP_LISTVENDITA  on ARTID = LisId
		 WHERE TERM = @TERM AND PREZZO <> PREZZO_BASE	
		 
		 UPDATE TbArt set ArtUlt_modifica=Ultima_modifica, ArtUtente=Utente
		 from TbArt a inner join
		      TMP_LISTVENDITA b on b.ARTID = a.ArtId
	     WHERE TERM = @TERM AND Ultima_modifica IS NOT NULL
   END 

ESCI:

DELETE FROM TMP_MODIFICA WHERE LISTINO = @GRUPPO AND TERM=@TERM
GO
/****** Object:  StoredProcedure [dbo].[X_SCAMBIO_PLU]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_SCAMBIO_PLU] @SALI AS BIT, @IDPLU AS SMALLINT, @ARTID AS INT
AS

DECLARE @NEW_IDPLU AS SMALLINT, @MAX_IDPLU AS SMALLINT, @EXARTID AS INT

SET @MAX_IDPLU = (SELECT MAX(IDPLU) FROM TMP_PLU)

IF @SALI= 1
   BEGIN      
	   SET @NEW_IDPLU = @IDPLU - 1
	   IF @NEW_IDPLU = 0
		  BEGIN
			GOTO FINE
		  END    	  
   END
ELSE
   BEGIN      
	   SET @NEW_IDPLU = @IDPLU + 1
	   IF @NEW_IDPLU > @MAX_IDPLU
		  BEGIN
			GOTO FINE
		  END 	  
   END


SET @EXARTID = (SELECT TMPARTID FROM TMP_PLU WHERE IDPLU = @NEW_IDPLU)

UPDATE TMP_PLU SET TMPARTID = @ARTID
WHERE IDPLU = @NEW_IDPLU

UPDATE TMP_PLU SET TMPARTID = @EXARTID
WHERE IDPLU = @IDPLU




FINE:

SELECT IDPLU,
       ArtId,
       Artdesc
FROM TbArt inner join
     TMP_PLU ON tmpartid = ArtId
ORDER BY IDPLU
GO
/****** Object:  StoredProcedure [dbo].[X_SospToBoll_MONITOR]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE     PROC [dbo].[X_SospToBoll_MONITOR]  @Tipo as Varchar(1), @Numrif as int, @progr as int, @Cau as smallint,@SospRif as int, @DATADOC AS SMALLDATETIME
as

DECLARE  @ARTID AS INT, @CAUSALE AS SMALLINT, @QTA AS DECIMAL(7,2), @IMPORTO AS DECIMAL(9,2), @SEGNO AS SMALLINT , @DEP AS SMALLINT, @VARDEST AS VARCHAR(100),@NOTE as varchar(50)


-------- CREO LE RIGHE DI MOVIMENTO

select @Tipo AS CorTipoDoc,@Numrif as CorRif,CorProg = IDENTITY(int, 1 ,1) ,CorArtId,CorCodArt,CorDesc,PQta,CorPrezzo,CorSc1,CorSc2,CorSc3,CorSc4,CorSc5,
       CorNetto,CorImporto = CorNetto * PQta,CorUrgenza,CorSpot,CorPromo,CorAssortito,CorVariato,CorCampagna,CorCiva,CorCntrp,@Cau as CorCau,CorMacro,
       CorOrdRif,CorOrdine,CorArtFor,CorPlRif,CorPlCassa,CorImballo,CorMg1,CorMg2,CorMg3,CorMerc,CorBrand,CorOrdProg,CorUM,PLotto,PDataScad 
into #tmp
FROM TbSosP INNER JOIN
     TbCor on CorTipodoc = PTipoDoc and Corrif = PRif and CorProg = PProg
WHERE CorTipodoc  = 'S' AND Corrif = @SospRif

select @Tipo AS SCorTipoDoc,@Numrif as SCorRif,SCorProg = IDENTITY(int, 1 ,1),CorTipoDoc,CorRif,CorProg 
into #tmpSCor
FROM TbSosP INNER JOIN
     TbCor on CorTipodoc = PTipoDoc and Corrif = PRif and CorProg = PProg
WHERE CorTipodoc  = 'S' AND Corrif = @SospRif

insert into TbCor (CorTipoDoc, CorRif, CorProg, CorArtID, CorCodArt, CorDesc, CorQuaCon, CorPrezzo, CorSc1, CorSc2, CorSc3, CorSc4, CorSc5, CorNetto, CorImporto, CorUrgenza, CorSpot, CorPromo, CorAssortito, CorVariato, CorCampagna, 
                   CorCiva, CorCntrp, CorCau, CorMacro, CorOrdRif, CorOrdine, CorArtFor, CorPlRif, CorPlCassa, CorImballo, CorMg1, CorMg2, CorMg3, CorMerc, CorBrand, CorOrdProg, CorUM, CorLotto, CorDataScad)
select  CorTipoDoc, CorRif,CorProg = CorProg + @progr ,CorArtId,CorCodArt,CorDesc,PQta,CorPrezzo,CorSc1,CorSc2,CorSc3,CorSc4,CorSc5,
       CorNetto,CorImporto,CorUrgenza,CorSpot,CorPromo,CorAssortito,CorVariato,CorCampagna,CorCiva,CorCntrp,CorCau,CorMacro,
       CorOrdRif,CorOrdine,CorArtFor,CorPlRif,CorPlCassa,CorImballo,CorMg1,CorMg2,CorMg3,CorMerc,CorBrand,CorOrdProg,CorUM, PLotto,PDataScad   
from #tmp

Insert into TbSCor 
select SCorTipoDoc,SCorRif,SCorProg = SCorProg + @progr,CorTipoDoc,CorRif,CorProg 
from #tmpSCor


---AGGIORNO IL DOCUMENTO S

Update TbSos set SosTrasf = 1 where SosRif = @SospRif


----Insert Into TmpSCor select * from TbCor where CorTipoDoc = 'S' and CorRif = @SospRif 

---Delete from TbCor where CorTipoDoc = 'S' and CorRif = @SospRif


select isnull(max(CorProg),0) from TbCor where CorTipoDoc = @Tipo and CorRif = @Numrif





GO
/****** Object:  StoredProcedure [dbo].[X_SPOSTA_CODIVA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_SPOSTA_CODIVA] @FATRIF AS INT 
AS

DECLARE @SPLIT_PAY AS BIT, @CODIVA_SPL_10 AS SMALLINT, @CODIVA_SPL_22 AS SMALLINT

SELECT top 1 @CODIVA_SPL_10 = TaiSplit10, @CODIVA_SPL_22 = TaiSplit20 
FROM TbTai
ORDER BY TaiAnno DESC

SELECT @SPLIT_PAY = ClSplitPay
FROM TbCli INNER JOIN
     TbFat ON FatCliFat = ClCod
WHERE Fatrif = @FATRIF

IF @SPLIT_PAY = 1
   BEGIN
      UPDATE TbCor SET CorCiva = @CODIVA_SPL_10
	  WHERE CorTipoDoc = 'F' and Corrif = @FATRIF
	  AND CorCiva = 1

	  UPDATE TbCor SET CorCiva = @CODIVA_SPL_22
	  WHERE CorTipoDoc = 'F' and Corrif = @FATRIF
	  AND CorCiva = 2

	  UPDATE TbCor SET CorCiva = @CODIVA_SPL_10
	  WHERE CorTipoDoc = 'B' and Corrif IN (select Bolrif from TbBol where BolrifFat = @FATRIF)
	  AND CorCiva = 1

	  UPDATE TbCor SET CorCiva = @CODIVA_SPL_22
	  WHERE CorTipoDoc = 'B' and Corrif IN (select Bolrif from TbBol where BolrifFat = @FATRIF)
	  AND CorCiva = 2
	  	  
   END
ELSE
    BEGIN
      UPDATE TbCor SET CorCiva = 1
	  WHERE CorTipoDoc = 'F' and Corrif = @FATRIF
	  AND CorCiva = @CODIVA_SPL_10

	  UPDATE TbCor SET CorCiva = 2
	  WHERE CorTipoDoc = 'F' and Corrif = @FATRIF
	  AND CorCiva = @CODIVA_SPL_22

	  UPDATE TbCor SET CorCiva = 1
	  WHERE CorTipoDoc = 'B' and Corrif IN (select Bolrif from TbBol where BolrifFat = @FATRIF)
	  AND CorCiva = @CODIVA_SPL_10

	  UPDATE TbCor SET CorCiva = 2
	  WHERE CorTipoDoc = 'B' and Corrif IN (select Bolrif from TbBol where BolrifFat = @FATRIF)
	  AND CorCiva = @CODIVA_SPL_22
   END
GO
/****** Object:  StoredProcedure [dbo].[X_ST_LIST_VENDITA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROC [dbo].[X_ST_LIST_VENDITA] @TERM AS VARCHAR(40), @GRUPPO AS SMALLINT, @CLIENTE AS VARCHAR(5), @SOLO_ABILITATI BIT = 1, @CATDESC AS VARCHAR(30) = ''
AS

DECLARE @DEFAULT AS BIT = 0



IF @GRUPPO = 0
   BEGIN
   SET @SOLO_ABILITATI = 0
   SET @DEFAULT = 1
   END

IF @CLIENTE <> ''
   BEGIN
    SET @GRUPPO = (select ClGrCanale from TbCli WHERE ClCod = @CLIENTE)
   END

DECLARE @PRESENTE AS BIT = 0



IF (select count(*) from TbSlAtt WHERE SlAtt = @GRUPPO) = 0
   BEGIN
      SET @PRESENTE = @DEFAULT
   END

DELETE 
FROM TMP_LISTVENDITA
WHERE TERM = @TERM

INSERT INTO TMP_LISTVENDITA (TERM, GRUPPO, CATEGORIA, CLIENTE, ARTID, CODICE, DESCRIZIONE, PREZZO, PRESENTE, NOLISTINO, SCONTOFAS, SCONTOCLI, CATDESC, UMV, PREZZO_BASE, PREZZO_IVA, PREZZO_BASE_IVA, ALIQ, ultima_modifica, Utente, STAMPALIS)

SELECT @TERM,@GRUPPO,ArtCat,@CLIENTE,ArtId,ArtCod,ArtDesc,LISTINO,@PRESENTE,0,0.0,0.0,CatDesc,ArtUmVen,LISTINO,0,0,PERCIVA,NULL, NULL,0
FROM VArtCat  

IF @GRUPPO = 0 AND @CLIENTE = ''
    BEGIN
		UPDATE TMP_LISTVENDITA SET ultima_modifica=ArtUlt_modifica, Utente = ArtUtente
		FROM TMP_LISTVENDITA INNER JOIN
			 VArtCat on ArtCod = CODICE
		WHERE TERM = @TERM
   END

IF @GRUPPO > 0
   BEGIN
		UPDATE TMP_LISTVENDITA SET PRESENTE = 1, PREZZO = SlAttPrezzo, ultima_modifica=SlUlt_modifica, Utente = SlUtente, STAMPALIS = SlStampa
		FROM TMP_LISTVENDITA INNER JOIN
			 TbSlAtt on SlAtt = @GRUPPO and SlArtCod = CODICE
		WHERE TERM = @TERM
   END



IF @CLIENTE <> ''
   BEGIN
		UPDATE TMP_LISTVENDITA SET PRESENTE = 1, NOLISTINO = SlNo, PREZZO = SlCliPrezzo, ultima_modifica=SlUlt_modifica, Utente = SlUtente , STAMPALIS = case when Slno = 1 THEN 0 ELSE SlStampa END
		FROM TMP_LISTVENDITA INNER JOIN
			 TbSlCli on SlCli = @CLIENTE and SlArtCod = CODICE
		WHERE TERM = @TERM
   END

UPDATE TMP_LISTVENDITA SET PRESENTE = 0 WHERE NOLISTINO = 1 AND TERM = @TERM

update TMP_LISTVENDITA SET PREZZO_BASE_IVA = PREZZO_BASE * (1 + ALIQ / 100),
                           PREZZO_IVA = PREZZO * (1 + ALIQ / 100)
	   WHERE TERM = @TERM
    
IF @SOLO_ABILITATI = 0
   BEGIN
     IF @CATDESC = ''
	    BEGIN
			select * from TMP_LISTVENDITA
			WHERE TERM = @TERM
			ORDER BY DESCRIZIONE
		END
	 ELSE
	    BEGIN
		    select * from TMP_LISTVENDITA
			WHERE TERM = @TERM AND CATDESC = @CATDESC
			ORDER BY DESCRIZIONE
		END  		
   END
ELSE
   BEGIN
      IF @CATDESC = ''
	    BEGIN
			select * from TMP_LISTVENDITA
			WHERE TERM = @TERM AND PRESENTE = 1
			ORDER BY DESCRIZIONE
		END
	 ELSE
	    BEGIN
		    select * from TMP_LISTVENDITA
			WHERE TERM = @TERM AND PRESENTE = 1 AND CATDESC = @CATDESC
			ORDER BY DESCRIZIONE
		END  		
   END
GO
/****** Object:  StoredProcedure [dbo].[X_STAMPA_LISTINO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[X_STAMPA_LISTINO] @GRUPPO AS SMALLINT, @CLIENTE AS VARCHAR(5) = '', @CATEGORIA AS VARCHAR(2) = '', @VALIDITA AS SMALLDATETIME = NULL
AS

IF @CLIENTE <> ''
   BEGIN
    set @GRUPPO = (select ClGrCanale from TbCli WHERE ClCod = @CLIENTE)
   END


CREATE TABLE #tmpSlAtt (
    [SlAtt] [smallint] NOT NULL,
	[SlArtCod] [varchar](20) NOT NULL,
	[SlAttPrezzo] [decimal](9, 3) NOT NULL,
 ) ON [PRIMARY]

IF  @GRUPPO > 1
    BEGIN

		INSERT INTO #tmpSlAtt
		SELECT SlAtt, SlArtCod, SlAttPrezzo
		from TbSlAtt WHERE SlAtt = @GRUPPO

	END
ELSE
     BEGIN

		INSERT INTO #tmpSlAtt
		SELECT 1, ArtCod, LISTINO
		from VArtCat 

	END

SELECT GRUPPO = @GRUPPO,
       ArtId,
	   ArtCod,
	   ArtDesc,
	   ArtUmisura,
	   LISTINO,
	   SlAttPrezzo,
	   VALIDITA = ISNULL(@VALIDITA,VALIDITA),
	   ArtCodiva,
	   PERCIVA,
	   ArtCat,
	   CatDesc,
	   ArtNumImballo,
	   AttDesc,
	   CLIENTE = @CLIENTE,
	   RAG_SOC = isnull((SELECT AnaDesc from VDOX.DBO.TbAna wHERE AnaCod = @CLIENTE),''),
	   SCONTO = cast(0 as decimal(5,2))
INTO #tmp
FROM VArtCat INNER JOIN
     #tmpslatt ON SlArtCod = ArtCod INNER JOIN
	 tBaTT ON AttCod = @GRUPPO 

UNION

SELECT GRUPPO = @GRUPPO,
       ArtId,
	   ArtCod,
	   ArtDesc,
	   ArtUmisura,
	   LISTINO,
	   SlCliPrezzo,
	   VALIDITA = ISNULL(@VALIDITA,VALIDITA),
	   ArtCodiva,
	   PERCIVA,
	   ArtCat,
	   CatDesc,
	   ArtNumImballo,
	   AttDesc,
	   CLIENTE = @CLIENTE,
	   RAG_SOC = isnull((SELECT AnaDesc from VDOX.DBO.TbAna wHERE AnaCod = @CLIENTE),''),
	   SCONTO = cast(0 as decimal(5,2))

FROM VArtCat INNER JOIN
     TbSlCli ON SlCli = @CLIENTE AND SlArtCod = ArtCod INNER JOIN
	 tBaTT ON AttCod = @GRUPPO 
WHERE Slno = 0 AND ArtCod not in (Select SlArtCod from TbSlAtt where SlAtt = @GRUPPO)

UPDATE #TMP SET SlAttPrezzo = SlCliPrezzo
FROM #tmp inner join
     TbSlCli on SlArtCod = ArtCod
WHERE SlCli = @CLIENTE AND sLnO = 0

DELETE FROM #TMP WHERE ArtCod in (select SlArtCod from TbSlCli where SlCli = @CLIENTE and SlNo = 1)

--UPDATE #TMP SET SCONTO = FasSc1  
--FROM #TMP INNER JOIN 
--     TbNewfasAtt on FasAtt = GRUPPO and FasCategoria = ArtCat  
--WHERE FasArtid = 0

--UPDATE #TMP SET SCONTO = FasSc1  
--FROM #TMP INNER JOIN 
--     TbNewfasAtt on FasAtt = GRUPPO and FasArtid = ArtId
--WHERE FasCategoria = ''

--UPDATE #TMP SET SCONTO = FasSc1  
--FROM #TMP INNER JOIN 
--     TbNewFasCli on FasCli = @CLIENTE and FasCategoria = ArtCat  
--WHERE FasArtid = 0

--UPDATE #TMP SET SCONTO = FasSc1  
--FROM #TMP INNER JOIN 
--     TbNewFasCli on  FasCli = @CLIENTE and FasArtid = ArtId
--WHERE FasCategoria = ''

--UPDATE #TMP SET SlAttPrezzo = listino * ( 1 - SCONTO / 100)

IF @CATEGORIA = ''
   BEGIN
	SELECT * FROM #TMP order by artdesc
   END
ELSE
   BEGIN
	SELECT * FROM #TMP 
	WHERE ArtCat = @CATEGORIA
	order by artdesc
   END
GO
/****** Object:  StoredProcedure [dbo].[X_VENDITA_ART]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROC [dbo].[X_VENDITA_ART] @ARTID AS INT, @CLIENTE AS VARCHAR(5), @DATA AS SMALLDATETIME, @QTA AS INT,  @LISTINO AS SMALLINT = 0
AS

DECLARE @GrupAtt SMALLINT, @VALIDITAOGGI AS SMALLDATETIME,@TUTTI AS int, @VENDITA AS DECIMAL(9,2), @NETTO AS DECIMAL(9,2), @SC1 AS DECIMAL(5,2), @ARTMERC AS INT, @CLESPSCONTI VARCHAR(1)
		


SET @VALIDITAOGGI = isnull((SELECT MAX(Lisvaliditadal) from TbLis where LisId = @ARTID  AND LisValiditaDal <= @DATA),@DATA)

SELECT @GrupAtt = CLGRCANALE, @CLESPSCONTI = ClEspSconti
FROM TbCli
where ClCod = @CLIENTE

IF @LISTINO = 0
   BEGIN
   SET @LISTINO = @GrupAtt
   END

select LisId=isnull(LisId,@artid),
       LisValiditaDal = isnull(LisValiditaDal,@data),
	   LisVendita =isnull(LisVendita,0)
into #TMPLIS
from TbLis 
WHERE LisId = @ARTID AND LisValiditaDal = @VALIDITAOGGI


if (select count(*) from #tmplis) = 0
   begin
   INSERT INTO #TMPLIS
   SELECT @artid,@data,0,0,0,0,0
   END



SELECT ArtId,
       ArtCod,
	   ArtDesc,	  
	   ArtCodiva,
	   ArtCptCon, 	
	   ArtCat,
	   ArtUMisura,
	   EspSconti = @CLESPSCONTI,
       PrezzoListino = LisVendita,
	   Prezzo = CAST(0 AS DECIMAL(10,3)),
	   Sc1 = CAST(0 AS DECIMAL(5,3)),
	   Netto = CAST(0 AS DECIMAL(10,3)),
	   PERCIVA = cast((SELECT CIIALI FROM COGE.DBO.TbCII WHERE CIICOD = DBO.TCI(ArtCodiva)) as decimal(6,2)),
	   C_Prezzo = CAST(0 AS DECIMAL(10,3)),		
	   C_Netto = CAST(0 AS DECIMAL(10,3))
INTO #TMPPRODOTTI
FROM TbArt inner join
	 #TMPLIS ON LisId = ArtId
WHERE ArtId = @ARTID

update #TMPPRODOTTI set Prezzo = PrezzoListino, C_Prezzo = PrezzoListino , Netto = PrezzoListino , C_Netto = PrezzoListino


--------------------- scontistica per cliente
UPDATE #TMPPRODOTTI
SET Sc1 = FasSc1
FROM #TMPPRODOTTI INNER JOIN
     TbNewFascli on FasCli = @CLIENTE and FasArtId = ArtId 
WHERE FasCategoria = '' and Sc1 = 0

UPDATE #TMPPRODOTTI
SET Sc1 = FasSc1
FROM #TMPPRODOTTI INNER JOIN
     TbNewFascli on FasCli = @CLIENTE and FasCategoria = ArtCat 
WHERE FasArtId = 0 and Sc1 = 0

--------------------- scontistica PER GRUPPO ATTIVITà
UPDATE #TMPPRODOTTI
SET Sc1 = FasSc1
FROM #TMPPRODOTTI INNER JOIN
     TbNewFasAtt on FasAtt = @GrupAtt and FasArtId = ArtId 
WHERE FasCategoria = '' and Sc1 = 0

UPDATE #TMPPRODOTTI
SET Sc1 = FasSc1
FROM #TMPPRODOTTI INNER JOIN
     TbNewFasAtt on FasAtt = @GrupAtt and FasCategoria = ArtCat 
WHERE FasArtId = 0 and Sc1 = 0

------------------------------------------------------

update #TMPPRODOTTI
set Netto = SlAttPrezzo, C_Netto = SlAttPrezzo, Prezzo = SlAttPrezzo, C_Prezzo = SlAttPrezzo
from #TMPPRODOTTI INNER JOIN
    TbSlAtt on SlAtt = @LISTINO and SlArtCod = ArtCod

update #TMPPRODOTTI
set Netto = SlCliPrezzo, C_Netto = SlCliPrezzo, Prezzo = SlCliPrezzo, C_Prezzo = SlCliPrezzo
from #TMPPRODOTTI INNER JOIN
    TbSlCli on SlCli = @CLIENTE and SlArtCod = ArtCod
where SlNo = 0

update #tmpprodotti set C_Prezzo = C_PREZZO * (1 + PERCIVA / 100), C_Netto=  C_Netto * (1 + PERCIVA / 100)


SELECT * 
FROM #TMPPRODOTTI
GO
/****** Object:  StoredProcedure [dbo].[XAggMail]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [dbo].[XAggMail] @Rif as Int,@TipoD as varchar(1)
as 

declare @X as INT

SET @X = isnull((SELECT isnull(MailRif,0) FROM TbMailDoc WHERE MailRif = @Rif and MailTipoDoc =@TipoD),0)
if @X = 0 goto newIns

up:

UPDATE TbMailDoc SET MailData = getdate() WHERE MailRif = @Rif and MailTipoDoc =@TipoD

goto uscita

newIns:

INSERT into TbMailDoc (MailRif,MailTipoDoc,MailData)
SELECT @Rif,@TipoD,GETDATE()



uscita:



GO
/****** Object:  StoredProcedure [dbo].[XMailDoc]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XMailDoc] @DEP AS SMALLINT, @dal as smalldatetime,@al as smalldatetime, @T as smallint, @CH as smallint,@BF as smallint
as


declare @path as varchar(80), @regdiff as smallint, @regnc as smallint, @Regacc as smallint

set @path =  (SELECT Sel12 + Sel11 from vdox.dbo.TbSel where SelId = 1 )
set @regdiff = (SELECT TOP 1 TaiReg10 from Tbtai order by TaiAnno desc)
set @regnc = (SELECT TOP 1 TaiReg11 from Tbtai order by TaiAnno desc)
set @Regacc = (SELECT TOP 1 TaiReg2 from Tbtai order by TaiAnno desc)





select ClCod,
       AnaDesc = (select AnaDesc from vdox.dbo.TbAna where AnaCod = ClCod),
       NumDoc=FatNum,DataDoc=FatData,AnaEmail=MPEMail,PEC =MPPec,MailData,Invio=CAST(@CH as bit),
       Pdf=@path + 'FC' + CAST (FatRif as varchar)+ '.pdf',Rifer=FatRif,TipoDoc=FatTipoDoc,Documento = 'Fattura',
       DIFF = FatDiff 
into #Tmp
from tbcli inner join 
     TbMailPec on MPClie = ClCod and MPTipo = 'CL' and MPFatture = 1 inner join 
     TbFat on ClCod=FatCliFat left outer join 
     TbMailDoc on FatRif=MailRif and FatTipoDoc = MailTipoDoc
where FatData between @dal and @al and ClInviaMail=1 and FatNum <> 999999 and (FatNumreg = @regdiff or fatnumreg = @Regacc) AND FatNDep=@DEP
union
select ClCod,
       AnaDesc = (select AnaDesc from vdox.dbo.TbAna where AnaCod = ClCod),
       NumDoc=FatNum,DataDoc=FatData,AnaEmail=MPEMail,PEC =MPPec,MailData,Invio=CAST(@CH as bit),
       Pdf=@path + 'FC' + CAST (FatRif as varchar)+ '.pdf',Rifer=FatRif,TipoDoc=FatTipoDoc,Documento = 'Nota Credito',
       DIFF = FatDiff 
from tbcli inner join 
     TbMailPec on MPClie = ClCod and MPTipo = 'CL' and MPFatture = 1 inner join 
     TbFat on ClCod=FatCliFat left outer join 
     TbMailDoc on FatRif=MailRif and FatTipoDoc = MailTipoDoc
where FatData between @dal and @al and ClInviaMail=1 and FatNum <> 999999 and FatNumreg = @regnc and FatNDep = @DEP
union
select ClCod,AnaDesc=(select AnaDesc from vdox.dbo.TbAna where AnaCod = ClCod),
       NumDoc=BolNum,DataDoc=BolData,AnaEmail=MPEMail,PEC =MPPec,MailData,Invio=CAST(@CH as bit),
	   Pdf=@path + 'BO' + CAST (BolRif as varchar)+ '.pdf',Rifer=BolRif,TipoDoc=BolTipoDoc,Documento='D.D.T.',
	   DIFF=''
from tbcli inner join 
     TbMailPec on MPClie = ClCod and MPTipo = 'CL' and MPBolle = 1 inner join
     TbBol on ClCod=BolCliFat left outer join 
     TbMailDoc on BolRif=MailRif and BolTipoDoc = MailTipoDoc 
where BolData between @dal and @al and ClInviaMail=1 and bolNum <> 999999 and (len(MPEMail) > 5) and BolNDep = @DEP

if @BF = 1
Begin
delete from #tmp where TipoDoc = 'F'
end

if @BF =2
Begin
delete from #tmp where TipoDoc = 'B'
end


if @T = 0
begin
select * from #Tmp order by AnaDesc
goto uscita
end
if @T = 1
begin
select * from #Tmp where MailData is not null
goto uscita
end
if @T = 2
begin
select * from #Tmp where MailData is null
goto uscita
end

uscita:



GO
/****** Object:  StoredProcedure [dbo].[XVETPRINT]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XVETPRINT]
as
select distinct VetCod,VetCodFor,VetAttivo,VetNazione,TbAna.* 
INTO #TMP from TbVettori
 inner join vdox.dbo.tbana on anacod=Vetcodfor and anagrp='FO'
 
select * from #tmp order by anadesc
GO
/****** Object:  StoredProcedure [dbo].[XX_CREA_STAMPA_PANETTONI]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROC [dbo].[XX_CREA_STAMPA_PANETTONI]  @TERM as varchar(30), @DASTAMPARE AS BIT = 0
AS

DELETE FROM TMP_ORD_PANETTONI WHERE TERM = @TERM

IF @DASTAMPARE = 0
	BEGIN
			SELECT * 
			INTO #TMP 
			FROM VPRINTSOS INNER JOIN							
				 TMP_SELEZIONATI ON SORDRIF = SOSRIF
			WHERE TERM = @TERM
			ORDER by SOSRIF

			INSERT INTO TMP_ORD_PANETTONI (TERM, SOSRIF, SOSNUM, SOSDATA, SOSCONSEGNA, TIPO_CONSEGNA, SOSCLICONS, ANARAG, CORPROG, CORCODART, QTA, DESCRIZIONE, PEZZATURA, CONFEZIONE, PRODOTTO)

			SELECT @TERM, SOSRIF, SOSNUM, SOSDATA, SOSCONSEGNA, TIPO_CONSEGNA, SOSCLICONS, ANARAG = ANARAG1 + CASE WHEN ANARAG2 <> '' THEN ' ' + ANARAG2 ELSE '' END , CORPROG, CORCODART, QTA, DESCRIZIONE,
				   PEZZATURA = dbo.EstraiPeso(DESCRIZIONE) ,
				   CONFEZIONE = dbo.EstraiConfezione(DESCRIZIONE),
				   PRODOTTO = ''
			from #TMP

			UPDATE TMP_ORD_PANETTONI SET PRODOTTO = REPLACE(REPLACE(REPLACE(DESCRIZIONE,PEZZATURA,''),CONFEZIONE,''),'CONF.','')
			WHERE TERM = @TERM

			UPDATE TbSos set SosStampato = 1
			WHERE SosRif in (SELECT DISTINCT SosRif from #TMP)		

			SELECT COUNT(SOSRIF) FROM  #TMP

	END

ELSE
   BEGIN
			SELECT * 
			INTO #TMPS 
			FROM VPRINTSOS 
			WHERE SOSSTAMPATO = 0
			ORDER by SOSRIF

			INSERT INTO TMP_ORD_PANETTONI (TERM, SOSRIF, SOSNUM, SOSDATA, SOSCONSEGNA, TIPO_CONSEGNA, SOSCLICONS, ANARAG, CORPROG, CORCODART, QTA, DESCRIZIONE, PEZZATURA, CONFEZIONE, PRODOTTO)

			SELECT @TERM, SOSRIF, SOSNUM, SOSDATA, SOSCONSEGNA, TIPO_CONSEGNA, SOSCLICONS, ANARAG = ANARAG1 + CASE WHEN ANARAG2 <> '' THEN ' ' + ANARAG2 ELSE '' END , CORPROG, CORCODART, QTA, DESCRIZIONE,
				   PEZZATURA = dbo.EstraiPeso(DESCRIZIONE) ,
				   CONFEZIONE = dbo.EstraiConfezione(DESCRIZIONE),
				   PRODOTTO = ''
			from #TMPS

			UPDATE TMP_ORD_PANETTONI SET PRODOTTO = REPLACE(REPLACE(REPLACE(DESCRIZIONE,PEZZATURA,''),CONFEZIONE,''),'CONF.','')
			WHERE TERM = @TERM

			UPDATE TbSos set SosStampato = 1
			WHERE SosRif in (SELECT DISTINCT SosRif from #TMPS)		

			SELECT COUNT(SOSRIF) FROM  #TMPS

	END

GO
/****** Object:  StoredProcedure [dbo].[XX_CREA_STAMPA_TORTE]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [dbo].[XX_CREA_STAMPA_TORTE] @TERM AS VARCHAR(20), @STAMPANUM AS INT = 0, @ORDRIF AS INT = 0, @RECAP AS BIT = 0, @SELEZIONATI AS BIT = 0
AS
IF @SELEZIONATI = 0
   BEGIN
					IF @ORDRIF = 0
					   BEGIN 
								IF @STAMPANUM = 0
									   ------------------------ STAMPA ORDINI DA STAMPARE
									   BEGIN
											SET  @STAMPANUM = (SELECT MAX(OrdtStampaNum) from TbOrdTorte) + 1
											SELECT * 
											INTO #TMP
											FROM VOrdTorte INNER JOIN							
												 TbPrivati on PrivId = OrdtPrivati
											WHERE OrdtStampato = 0 and OrdtPrivati <> 0
											Order by OrdtRif

											DELETE FROM TMP_NEWORD_TORTE WHERE TERM = @TERM

											INSERT INTO TMP_NEWORD_TORTE (TERM, ORDTRIF, ORDTNUM, ORDTDATA, COGNOMENOME, TELEFONO, ORDTCONSEGNA, ORDTDATARITIRO, ORDTPESO, ORDTQTA, ORDTTORTA, ORDTNOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, ORDTBASE, 
											 ORDTBAGNA, ORDTOPEVENTI, ORDTFARCITURA, ORDTBORDODEC, ORDTDECSUP, ORDTDETTAGLIO_HTML, ORDTCANDELINE_HTML, ORDTACCONTO, IMGFORMA, IMGDECORAZIONE, IMGIMMAGINE)

											SELECT    @TERM, ORDTRIF, ORDINE_NUMERO, ORDINE_DATA, PrivCognomeNome, PrivTelefono, DATA_RITIRO, ORA_RITIRO, PESO, N_TORTE, TIPO_TORTA, NOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, BASE, 
											 BAGNA, OP_EVENTI, FARCITURA, BORDO_DEC, DEC_SUP, DETTAGLIO_HTML, '', OrdtAcconto, OrdtFotoForma, OrdtFotoDecorazione, OrdtImmagine

											FROM #TMP

											IF @RECAP = 0
  											   BEGIN
													update TbOrdTorte set OrdtStampato = 1, OrdtStampaNum = @STAMPANUM, OrdTStato = CASE WHEN OrdTStato = 1 THEN 2 ELSE OrdTStato END
													WHERE OrdtRif in (SELECT OrdtRif from #TMP)
											   END
						

											SELECT COUNT(*) FROM  #TMP
									   END
									ELSE

									   --------------------------------------STAMPA GRUPPO
									   BEGIN
      										SELECT * 
											INTO #TMPR
											FROM VOrdTorte INNER JOIN							
												 TbPrivati on PrivId = OrdtPrivati
											WHERE OrdtStampaNum = @STAMPANUM and OrdtPrivati <> 0
											Order by OrdtRif

											DELETE FROM TMP_NEWORD_TORTE WHERE TERM = @TERM

											INSERT INTO TMP_NEWORD_TORTE (TERM, ORDTRIF, ORDTNUM, ORDTDATA, COGNOMENOME, TELEFONO, ORDTCONSEGNA, ORDTDATARITIRO, ORDTPESO, ORDTQTA, ORDTTORTA, ORDTNOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, ORDTBASE, 
											 ORDTBAGNA, ORDTOPEVENTI, ORDTFARCITURA, ORDTBORDODEC, ORDTDECSUP, ORDTDETTAGLIO_HTML, ORDTCANDELINE_HTML, ORDTACCONTO, IMGFORMA, IMGDECORAZIONE, IMGIMMAGINE)

											SELECT    @TERM, ORDTRIF, ORDINE_NUMERO, ORDINE_DATA, PrivCognomeNome, PrivTelefono, DATA_RITIRO, ORA_RITIRO, PESO, N_TORTE, TIPO_TORTA, NOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, BASE, 
											 BAGNA, OP_EVENTI, FARCITURA, BORDO_DEC, DEC_SUP, DETTAGLIO_HTML, '', OrdtAcconto, OrdtFotoForma, OrdtFotoDecorazione, OrdtImmagine
											FROM #TMPR

											SELECT COUNT(*) FROM  #TMPR
									   END

					   END
					ELSE 
					  BEGIN

							--------------------------RISTAMPA ORDINE O MAIL RECAP
							SET  @STAMPANUM = (SELECT MAX(OrdtStampaNum) from TbOrdTorte) + 1
							SELECT * 
							INTO #TMPS
							FROM VOrdTorte INNER JOIN							
									TbPrivati on PrivId = OrdtPrivati		
							WHERE OrdtRif = @ORDRIF

							DELETE FROM TMP_NEWORD_TORTE WHERE TERM = @TERM

							INSERT INTO TMP_NEWORD_TORTE (TERM, ORDTRIF, ORDTNUM, ORDTDATA, COGNOMENOME, TELEFONO, ORDTCONSEGNA, ORDTDATARITIRO, ORDTPESO, ORDTQTA, ORDTTORTA, ORDTNOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, ORDTBASE, 
											 ORDTBAGNA, ORDTOPEVENTI, ORDTFARCITURA, ORDTBORDODEC, ORDTDECSUP, ORDTDETTAGLIO_HTML, ORDTCANDELINE_HTML, ORDTACCONTO, IMGFORMA, IMGDECORAZIONE, IMGIMMAGINE)

											SELECT    @TERM, ORDTRIF, ORDINE_NUMERO, ORDINE_DATA, PrivCognomeNome, PrivTelefono, DATA_RITIRO, ORA_RITIRO, PESO, N_TORTE, TIPO_TORTA, NOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, BASE, 
											 BAGNA, OP_EVENTI, FARCITURA, BORDO_DEC, DEC_SUP, DETTAGLIO_HTML, '', OrdtAcconto, OrdtFotoForma, OrdtFotoDecorazione, OrdtImmagine
							FROM #TMPS

								   IF @RECAP = 0
  									BEGIN
										update TbOrdTorte set OrdtStampato = 1, OrdtStampaNum = @STAMPANUM --, OrdTStato = CASE WHEN OrdTStato = 1 THEN 2 ELSE OrdTStato END
										WHERE OrdtRif = @ORDRIF
									END		

							SELECT COUNT(*) FROM  #TMPS
						END

   END
ELSE
   BEGIN
            ------------------------STAMPA ORDINI SELEZIONATI

            SELECT * 
			INTO #TMPSEL
			FROM VOrdTorte INNER JOIN							
					TbPrivati on PrivId = OrdtPrivati INNER JOIN
					TMP_SELEZIONATI ON SORDRIF = OrdtRif
			WHERE TERM = @TERM
			Order by OrdtRif

			DELETE FROM TMP_NEWORD_TORTE WHERE TERM = @TERM

			INSERT INTO TMP_NEWORD_TORTE (TERM, ORDTRIF, ORDTNUM, ORDTDATA, COGNOMENOME, TELEFONO, ORDTCONSEGNA, ORDTDATARITIRO, ORDTPESO, ORDTQTA, ORDTTORTA, ORDTNOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, ORDTBASE, 
											 ORDTBAGNA, ORDTOPEVENTI, ORDTFARCITURA, ORDTBORDODEC, ORDTDECSUP, ORDTDETTAGLIO_HTML, ORDTCANDELINE_HTML, ORDTACCONTO, IMGFORMA, IMGDECORAZIONE, IMGIMMAGINE)

											SELECT    @TERM, ORDTRIF, ORDINE_NUMERO, ORDINE_DATA, PrivCognomeNome, PrivTelefono, DATA_RITIRO, ORA_RITIRO, PESO, N_TORTE, TIPO_TORTA, NOFRUTTA, ORDTALLERGIE, ORDTFRASE, ORDTNOTE, BASE, 
											 BAGNA, OP_EVENTI, FARCITURA, BORDO_DEC, DEC_SUP, DETTAGLIO_HTML, '', OrdtAcconto, OrdtFotoForma, OrdtFotoDecorazione, OrdtImmagine

			FROM #TMPSEL

			
			update TbOrdTorte set OrdtStampato = 1, OrdTStato = CASE WHEN OrdTStato = 1 THEN 2 ELSE OrdTStato END
			WHERE OrdtRif in (SELECT OrdtRif from #TMPSEL)		
						

			SELECT COUNT(*) FROM  #TMPSEL

   END
GO
/****** Object:  StoredProcedure [dbo].[XX_LEGGI_ORD_TORTA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[XX_LEGGI_ORD_TORTA] @NUMRIF AS INT = 0, @STORICO AS BIT = 0
AS

DECLARE @INTERVALLO AS INT = 2

if @NUMRIF <> 0
   BEGIN
		SELECT *,
		       PrivCognomeNome,
			   PrivTelefono,
			   PrivMail,
			   URGENTE = CASE WHEN DATEDIFF(DAY,CAST(GETDATE() AS DATE),DATA_RITIRO) < @INTERVALLO then CAST(1 AS BIT) else CAST(0 AS BIT) END,
			   DESCRIZIONE = TIPO_TORTA
		from VordTorte inner join
			 TbPrivati on PrivId = OrdtPrivati 
		WHERE OrdtRif = @NUMRIF  

   END
ELSE
   BEGIN
		SELECT *,
		       PrivCognomeNome,
			   PrivTelefono,
			   PrivMail,
			   URGENTE = CASE WHEN DATEDIFF(DAY,CAST(GETDATE() AS DATE),DATA_RITIRO) < @INTERVALLO AND @STORICO = 0 then CAST(1 AS BIT) else CAST(0 AS BIT) END,
			   DESCRIZIONE = TIPO_TORTA
		from VordTorte inner join
			 TbPrivati on PrivId = OrdtPrivati 			
		WHERE (OrdtPrivati <> 0 AND DATA_RITIRO >= CAST(GETDATE() AS DATE) AND @STORICO = 0) OR (OrdtPrivati <> 0 AND @STORICO = 1)
		ORDER BY DATA_RITIRO,DATEPART(HOUR,ORA_RITIRO)
   END
   

GO
/****** Object:  StoredProcedure [dbo].[XX_REG_ORD_TORTA]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[XX_REG_ORD_TORTA] @NUMRIF AS INT, @IDCLI AS INT, @DATA AS SMALLDATETIME, @CONSEGNA AS SMALLDATETIME, @ORACONSEGNA AS DATETIME, @COGNOMENOME AS VARCHAR(100), @TEL AS VARCHAR(20), @QTA AS SMALLINT,
                              @NADULTI AS SMALLINT,@NBAMBINI AS SMALLINT, @PESOKG AS DECIMAL(6,3), @TORTE AS INT, @BASE AS INT, @PERS_BASE AS VARCHAR(100), @BAGNA AS INT, @OP_EVENTI AS INT, @PERS_OP_EVENTI AS VARCHAR(100),
							  @FOTO_FORMA VARBINARY(MAX), @FARCITURA AS INT ,@PERS_FARCITURA AS VARCHAR(100), @BORDO_DEC AS INT, @PERS_BORDO_DEC AS VARCHAR(100), @FOTO_DECORAZIONE VARBINARY(MAX), @DEC_SUP AS INT,
							  @PERS_DEC_SUP AS VARCHAR(100), @FRASE AS VARCHAR(500), @ALLERGIE AS VARCHAR(100), @IMMAGINE_NUM AS VARCHAR(20), @IMMAGINE VARBINARY(MAX), @NOTE AS VARCHAR(250), @NOFRUTTA AS VARCHAR(100),
							  @CANALE_FOTO AS VARCHAR(100), @UNO_QTA AS SMALLINT, @UNO_COLORE AS INT, @DUE_QTA AS SMALLINT, @DUE_COLORE AS INT, @TRE_QTA AS SMALLINT, @TRE_COLORE AS INT, @QUATTRO_QTA AS SMALLINT, @QUATTRO_COLORE AS INT,
							  @CINQUE_QTA AS SMALLINT, @CINQUE_COLORE AS INT,@SEI_QTA AS SMALLINT, @SEI_COLORE AS INT, @SETTE_QTA AS SMALLINT, @SETTE_COLORE AS INT, @OTTO_QTA AS SMALLINT, @OTTO_COLORE AS INT,
							  @NOVE_QTA AS SMALLINT, @NOVE_COLORE AS INT, @ZERO_QTA AS SMALLINT, @ZERO_COLORE AS INT, @CAND_QTA AS SMALLINT, @CAND_COLORE AS INT, @NUM_TIPO AS VARCHAR(3), @NUM_COLORE AS INT,
							  @ACCONTO as decimal(9,2), @EMAIL VARCHAR(50), @FARCITURA2 AS INT, @FARCITURA3 AS INT, @TIPO_ACCESSORI AS INT, @DESC_ACCESSORI AS VARCHAR(30), @STATO_ORDINE AS TINYINT, @ULT_STRATO AS INT = 0, 
							  @PERS_ULT_STRATO AS VARCHAR(100) = '', @TIPO_TORTA VARCHAR(1) = ''
AS

DECLARE @CLIENTE AS VARCHAR(5), @COD_TORTE AS VARCHAR(10), @PAGCOD AS INT, @PESO_MINIMO_TORTA AS INT

SELECT TOP 1 @CLIENTE = TaiCliTorte, @COD_TORTE = TaiCodTorte, @PESO_MINIMO_TORTA = TaiPesoMinimoTorta
FROM TbTai 
ORDER BY TaiAnno DESC

SET @PAGCOD = (select ClPagam from TbCli WHERE ClCod = @CLIENTE)

----------------- inserimento eventuale cliente nuovo ( altrienti aggiornamento dati Cliente)

IF @IDCLI <> 0
   BEGIN
     UPDATE TbPrivati SET PrivCognomeNome = @COGNOMENOME, PrivTelefono = @TEL, PrivMail = @EMAIL, PrivIndirizzo = '', PrivCap = '', PrivCitta = '', PrivProv = ''
	 WHERE PrivId = @IDCLI
   END

ELSE
   BEGIN

     INSERT INTO TbPrivati (PrivCognomeNome, PrivTelefono, PrivMail, PrivIndirizzo, PrivCap, PrivCitta, PrivProv)
	 SELECT @COGNOMENOME, @TEL, @EMAIL, '', '', '', ''
	 
	 SET @IDCLI = (SELECT @@IDENTITY)


   END


----------------------------- INSERIMENTO / AGGIORNAMENTO ORDINE

IF @NUMRIF = 0
   BEGIN

     Insert into TbOrdTorte WITH (TABLOCKX) (OrdtNum, OrdtData, OrdtCliente, OrdtPrivati,OrdtDataRitiro, OrdOraRitiro, OrdtQta, OrdTNAdulti, OrdtNBambini, OrdtPesoKg, OrdtTorta, OrdtBase, OrdtPersBase, OrdtBagna, OrdtOpEventi, OrdtPersOpEventi, 
                         OrdtFotoForma, OrdtFarcitura, OrdtPersFarcitura, OrdtBordoDec, OrdtPersBordoDec, OrdtFotoDecorazione, OrdtDecSuperficiale, OrdtPersDecSuperficiale, OrdtFrase, OrdtAllergie, OrdtImmagineNum, OrdtImmagine, OrdtNote, 
                         OrdtNoFrutta, OrdtCanaleFoto, OrdtUnoQta, OrdtUnoColore, OrdtDueQta, OrdtDueColore, OrdtTreQta, OrdtTreColore, OrdtQuattroQta, OrdtQuattroColore, OrdtCinqueQta, OrdtCinqueColore, OrdtSeiQta, OrdtSeiColore, OrdtSetteQta, 
                         OrdtSetteColore, OrdtOttoQta, OrdtOttoColore, OrdtNoveQta, OrdtNoveColore, OrdtZeroQta, OrdtZeroColore, OrdtCandelinaQta, OrdtCandelinaColore, OrdtNumeroTipo, OrdtNumeroColore, OrdtAcconto, OrdtFarcitura2, OrdtFarcitura3, 
						 OrdtTipoAccessori, OrdtDescAccessori, OrdTStato, OrdtUltStrato, OrdtPersUltStrato, OrdtTipoTorta)
     SELECT 999999, @DATA, @CLIENTE, @IDCLI, @CONSEGNA, @ORACONSEGNA, @QTA, @NADULTI, @NBAMBINI, @PESOKG, @TORTE, @BASE, @PERS_BASE, @BAGNA, @OP_EVENTI, @PERS_OP_EVENTI, 
                         @FOTO_FORMA, @FARCITURA, @PERS_FARCITURA, @BORDO_DEC, @PERS_BORDO_DEC, @FOTO_DECORAZIONE, @DEC_SUP, @PERS_DEC_SUP, @FRASE, @ALLERGIE, @IMMAGINE_NUM, @IMMAGINE, @NOTE, 
                         @NOFRUTTA, @CANALE_FOTO, @UNO_QTA, @UNO_COLORE, @DUE_QTA, @DUE_COLORE, @TRE_QTA, @TRE_COLORE, @QUATTRO_QTA, @QUATTRO_COLORE, @CINQUE_QTA, @CINQUE_COLORE, @SEI_QTA, @SEI_COLORE, @SETTE_QTA, 
                         @SETTE_COLORE, @OTTO_QTA, @OTTO_COLORE, @NOVE_QTA, @NOVE_COLORE, @ZERO_QTA, @ZERO_COLORE, @CAND_QTA, @CAND_COLORE, @NUM_TIPO, @NUM_COLORE, @ACCONTO,@FARCITURA2,@FARCITURA3, @TIPO_ACCESSORI, @DESC_ACCESSORI, 1, 
						 @ULT_STRATO, @PERS_ULT_STRATO, @TIPO_TORTA
     FROM TbCli where CLCod = @CLIENTE

     SET @NUMRIF = (SELECT @@IDENTITY)


   END

ELSE
   BEGIN

    UPDATE TbOrdTorte
			SET OrdtDataRitiro = @CONSEGNA, 
				OrdOraRitiro = @ORACONSEGNA,
				OrdtQta = @QTA,
				OrdTNAdulti = @NADULTI,
				OrdtNBambini = @NBAMBINI,
				OrdtPesoKg = @PESOKG,
				OrdtTorta = @TORTE,
				OrdtBase = @BASE,
				OrdtPersBase = @PERS_BASE,
				OrdtBagna = @BAGNA,
				OrdtOpEventi = @OP_EVENTI,
				OrdtPersOpEventi = @PERS_OP_EVENTI,
				OrdtFotoForma = @FOTO_FORMA,
				OrdtFarcitura = @FARCITURA,
				OrdtPersFarcitura = @PERS_FARCITURA,
				OrdtBordoDec = @BORDO_DEC,
				OrdtPersBordoDec = @PERS_BORDO_DEC,
				OrdtFotoDecorazione = @FOTO_DECORAZIONE,
				OrdtDecSuperficiale = @DEC_SUP,
				OrdtPersDecSuperficiale = @PERS_DEC_SUP,
				OrdtFrase = @FRASE,
				OrdtAllergie = @ALLERGIE,
				OrdtImmagineNum = @IMMAGINE_NUM,
				OrdtImmagine = @IMMAGINE,
				OrdtNote = @NOTE,
				OrdtNoFrutta = @NOFRUTTA,
				OrdtCanaleFoto = @CANALE_FOTO,
				OrdtUnoQta = @UNO_QTA,
				OrdtUnoColore = @UNO_COLORE,
				OrdtDueQta = @DUE_QTA,
				OrdtDueColore = @DUE_COLORE,
				OrdtTreQta = @TRE_QTA,
				OrdtTreColore = @TRE_COLORE,
				OrdtQuattroQta = @QUATTRO_QTA,
				OrdtQuattroColore = @QUATTRO_COLORE,
				OrdtCinqueQta = @CINQUE_QTA,
				OrdtCinqueColore = @CINQUE_COLORE,
				OrdtSeiQta = @SEI_QTA,
				OrdtSeiColore = @SEI_COLORE,
				OrdtSetteQta = @SETTE_QTA,
				OrdtSetteColore = @SETTE_COLORE,
				OrdtOttoQta = @OTTO_QTA,
				OrdtOttoColore = @OTTO_COLORE,
				OrdtNoveQta = @NOVE_QTA,
				OrdtNoveColore = @NOVE_COLORE,
				OrdtZeroQta = @ZERO_QTA,
				OrdtZeroColore = @ZERO_COLORE,
				OrdtCandelinaQta = @CAND_QTA,
				OrdtCandelinaColore = @CAND_COLORE,
				OrdtNumeroTipo = @NUM_TIPO,
				OrdtNumeroColore = @NUM_COLORE,
				OrdtAcconto = @ACCONTO,
				OrdtFarcitura2=@FARCITURA2,
				OrdtFarcitura3=@FARCITURA3,
				OrdtTipoAccessori =@TIPO_ACCESSORI, 
				OrdtDescAccessori =@DESC_ACCESSORI,
				OrdtStampaNum = 0,
				OrdtStampato = 0,
				OrdtMailInviata = 0,

				OrdTStato = CASE WHEN OrdTStato = 0 THEN 1 ELSE @STATO_ORDINE END,
				OrdtUltStrato = @ULT_STRATO,
				OrdtPersUltStrato = @PERS_ULT_STRATO,
				OrdtTipoTorta = CASE WHEN  @TIPO_TORTA = '' then OrdtTipoTorta ELSE @TIPO_TORTA END

			WHERE OrdtRif = @NUMRIF 

   END

 select @NUMRIF
GO
/****** Object:  StoredProcedure [dbo].[XX_STAMPA_LISTINO]    Script Date: 20/05/2026 14:31:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROC [dbo].[XX_STAMPA_LISTINO] @GRUPPO AS SMALLINT, @CLIENTE AS VARCHAR(5) = '', @CATEGORIA AS VARCHAR(2) = '', @VALIDITA AS SMALLDATETIME = NULL, @TERM AS VARCHAR(20) = ''
AS

IF @CLIENTE <> ''
   BEGIN
    set @GRUPPO = (select ClGrCanale from TbCli WHERE ClCod = @CLIENTE)
   END


CREATE TABLE #tmpSlAtt (
    [SlAtt] [smallint] NOT NULL,
	[SlArtCod] [varchar](20) NOT NULL,
	[SlAttPrezzo] [decimal](9, 3) NOT NULL,
 ) ON [PRIMARY]

IF  @GRUPPO > 0
    BEGIN

		INSERT INTO #tmpSlAtt
		SELECT SlAtt, SlArtCod, SlAttPrezzo
		from TbSlAtt 
		WHERE SlAtt = @GRUPPO AND SlStampa = 1

	END
ELSE
     BEGIN

		INSERT INTO #tmpSlAtt
		SELECT 1, ArtCod, LISTINO
		from VArtCat 

	END

SELECT GRUPPO = @GRUPPO,
       ArtId,
	   ArtCod,
	   ArtDesc,
	   ArtUmisura,
	   LISTINO,
	   SlAttPrezzo,
	   VALIDITA = ISNULL(@VALIDITA,VALIDITA),
	   ArtCodiva,
	   PERCIVA,
	   ArtCat,
	   CatDesc,
	   ArtNumImballo,
	   AttDesc = ISNULL(AttDesc,'BASE'),
	   CLIENTE = @CLIENTE,
	   RAG_SOC = isnull((SELECT AnaDesc from VDOX.DBO.TbAna wHERE AnaCod = @CLIENTE),''),
	   SCONTO = cast(0 as decimal(5,2))
INTO #tmp
FROM VArtCat INNER JOIN
     #tmpslatt ON SlArtCod = ArtCod LEFT OUTER JOIN
	 tBaTT ON AttCod = @GRUPPO 

UNION

SELECT GRUPPO = @GRUPPO,
       ArtId,
	   ArtCod,
	   ArtDesc,
	   ArtUmisura,
	   LISTINO,
	   SlCliPrezzo,
	   VALIDITA = ISNULL(@VALIDITA,VALIDITA),
	   ArtCodiva,
	   PERCIVA,
	   ArtCat,
	   CatDesc,
	   ArtNumImballo,
	   AttDesc,
	   CLIENTE = @CLIENTE,
	   RAG_SOC = isnull((SELECT AnaDesc from VDOX.DBO.TbAna wHERE AnaCod = @CLIENTE),''),
	   SCONTO = cast(0 as decimal(5,2))

FROM VArtCat INNER JOIN
     TbSlCli ON SlCli = @CLIENTE AND SlArtCod = ArtCod INNER JOIN
	 tBaTT ON AttCod = @GRUPPO 
WHERE Slno = 0 and SlStampa = 1 AND ArtCod not in (Select SlArtCod from TbSlAtt where SlAtt = @GRUPPO)

UPDATE #TMP SET SlAttPrezzo = SlCliPrezzo
FROM #tmp inner join
     TbSlCli on SlArtCod = ArtCod
WHERE SlCli = @CLIENTE AND sLnO = 0

DELETE FROM #TMP WHERE ArtCod in (select SlArtCod from TbSlCli where SlCli = @CLIENTE and SlNo = 1)



IF (SELECT COUNT(*) FROM TMPCAT WHERE TERM = @TERM) = 0
   BEGIN
	SELECT * FROM #TMP order by artdesc
   END
ELSE
   BEGIN
	SELECT * FROM #TMP 
	WHERE ArtCat IN (SELECT CATEGORIA FROM TMPCAT WHERE TERM = @TERM)
	order by artdesc
   END
GO
USE [master]
GO
ALTER DATABASE [GEVE] SET  READ_WRITE 
GO
