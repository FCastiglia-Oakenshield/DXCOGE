USE [master]
GO
/****** Object:  Database [COGE]    Script Date: 20/05/2026 10:24:42 ******/
CREATE DATABASE [COGE]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'COGE_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.RACCA\MSSQL\DATA\COGE.mdf' , SIZE = 1024000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'COGE_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.RACCA\MSSQL\DATA\COGE.ldf' , SIZE = 795328KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [COGE] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [COGE].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [COGE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [COGE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [COGE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [COGE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [COGE] SET ARITHABORT OFF 
GO
ALTER DATABASE [COGE] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [COGE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [COGE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [COGE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [COGE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [COGE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [COGE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [COGE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [COGE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [COGE] SET  DISABLE_BROKER 
GO
ALTER DATABASE [COGE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [COGE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [COGE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [COGE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [COGE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [COGE] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [COGE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [COGE] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [COGE] SET  MULTI_USER 
GO
ALTER DATABASE [COGE] SET PAGE_VERIFY TORN_PAGE_DETECTION  
GO
ALTER DATABASE [COGE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [COGE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [COGE] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [COGE] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [COGE] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [COGE] SET QUERY_STORE = OFF
GO
USE [COGE]
GO
/****** Object:  User [REDACTED_DB_USER_1]    Script Date: REDACTED ******/
-- REDACTED: database user omitted from documentation export
GO
/****** Object:  Schema [REDACTED_DB_SCHEMA_1]    Script Date: REDACTED ******/
-- REDACTED: database schema/user placeholder omitted from documentation export
GO
/****** Object:  UserDefinedFunction [dbo].[FnEtichetta]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[FnEtichetta](@DIBASE AS SMALLINT,@LINGUA AS SMALLINT)
returns varchar(1000)
as

BEGIN

declare @ETICHETTA AS VARCHAR(500), @ITALIANO VARCHAR(40), @CARATT as bit, @OLDCARATT as bit,@PARZIALE AS VARCHAR(200)
SET @ETICHETTA = ''
SET @PARZIALE = ''
SET @OLDCARATT = 0

IF @LINGUA = 1
   BEGIN
   DECLARE CURSORE CURSOR FOR
   select DISTINCT ITALIANO, CARATTERIZZANTE
   from VDISBASE WHERE  DIBASE = @DIBASE
   order by CARATTERIZZANTE Desc, ITALIANO
   END
ELSE IF @LINGUA = 2
   BEGIN
   DECLARE CURSORE CURSOR FOR
   select DISTINCT FRANCESE, CARATTERIZZANTE
   from VDISBASE WHERE  DIBASE = @DIBASE
   order by CARATTERIZZANTE Desc, FRANCESE
   END
ELSE IF @LINGUA = 3
   BEGIN
   DECLARE CURSORE CURSOR FOR
   select DISTINCT INGLESE, CARATTERIZZANTE
   from VDISBASE WHERE  DIBASE = @DIBASE
   order by CARATTERIZZANTE Desc, INGLESE
   END
ELSE IF @LINGUA = 4
   BEGIN
   DECLARE CURSORE CURSOR FOR
   select DISTINCT TEDESCO, CARATTERIZZANTE
   from VDISBASE WHERE  DIBASE = @DIBASE
   order by CARATTERIZZANTE Desc, TEDESCO
   END
ELSE IF @LINGUA = 5
   BEGIN
   DECLARE CURSORE CURSOR FOR
   select DISTINCT SPAGNOLO, CARATTERIZZANTE
   from VDISBASE WHERE  DIBASE = @DIBASE
   order by CARATTERIZZANTE Desc, SPAGNOLO
   END

OPEN CURSORE
FETCH NEXT FROM CURSORE
INTO @ITALIANO,@CARATT

WHILE @@FETCH_STATUS = 0

BEGIN
IF LEN(@ETICHETTA) > 1000
   GOTO ESCI
IF LEN(@PARZIALE) >80
BEGIN
   SET @ETICHETTA = @ETICHETTA + @PARZIALE-- + CHAR(13) + CHAR(10)
   SET @PARZIALE = ''
   END
if @OLDCARATT <> @CARATT
   BEGIN
   IF @CARATT = 1
      SET @OLDCARATT = @CARATT
   ELSE
      BEGIN
      SET @OLDCARATT = @CARATT
      SET @ETICHETTA = @ETICHETTA + @PARZIALE + CHAR(13) + CHAR(10)
      SET @PARZIALE = ''
      END
   END
SET @PARZIALE = @PARZIALE + lower(RTRIM(@ITALIANO)) + ',' 

      

FETCH NEXT FROM CURSORE
INTO @ITALIANO,@CARATT
end

SET @ETICHETTA = @ETICHETTA + SUBSTRING(@PARZIALE,1,LEN(@PARZIALE) -1)

ESCI:

CLOSE CURSORE
DEALLOCATE CURSORE

RETURN (@ETICHETTA)

end



GO
/****** Object:  UserDefinedFunction [dbo].[FnPrezzoLis]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE function [dbo].[FnPrezzoLis] (@ArtCod as smallint, @Numero as smallint, @Validita as smalldatetime)
returns decimal(7,2)
as

BEGIN

DECLARE @PREZZO as decimal(7,2)

SET @PREZZO = (select TOP 1 case 
                when @Numero = 1 then LisPrezzo1 
                when @Numero = 2 then LisPrezzo2 
                when @Numero = 3 then LisPrezzo3 
                when @Numero = 4 then LisPrezzo4 
                when @Numero = 5 then LisPrezzo5 
                when @Numero = 6 then LisPrezzo6 
                when @Numero = 7 then LisPrezzo7 
                when @Numero = 8 then LisPrezzo8 
                when @Numero = 9 then LisPrezzo9 
                when @Numero = 10 then LisPrezzo10 
                when @Numero = 11 then LisPrezzo11 
                when @Numero = 12 then LisPrezzo12 
                when @Numero = 13 then LisPrezzo13 
                when @Numero = 14 then LisPrezzo14 
                when @Numero = 15 then LisPrezzo15 
                when @Numero = 16 then LisPrezzo16 
                when @Numero = 17 then LisPrezzo17 
                when @Numero = 18 then LisPrezzo18 
                when @Numero = 19 then LisPrezzo19 
                when @Numero = 20 then LisPrezzo20 end
from TbLis where LisId = @artCod and LisValiditaDal <= @Validita order by LisValiditaDal desc)

return (@PREZZO)

END


GO
/****** Object:  UserDefinedFunction [dbo].[FnSPEZZA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FnSPEZZA](@STRINGA AS VARCHAR(1000))
returns varchar(1000)
as

begin
declare @STRINGAOUT AS VARCHAR(1000), @ARRIVO AS INT, @PARTENZA AS INT, @STRING VARCHAR(200)


SET @ARRIVO=0
SET @PARTENZA = 1
SET @STRINGAOUT = ''

LOOP1:

SET @ARRIVO = CHARINDEX(',',@STRINGA, @ARRIVO + 85)
IF @ARRIVO < 1
   BEGIN
   SET @STRING = SUBSTRING(@STRINGA,@PARTENZA,LEN(@STRINGA) - @PARTENZA + 1 )
   SET @STRINGAOUT = @STRINGAOUT + @STRING
   GOTO ESCI
   END
SET @STRING = SUBSTRING(@STRINGA,@PARTENZA,(@ARRIVO - @PARTENZA + 1))
SET @STRINGAOUT =@STRINGAOUT + @STRING  + CHAR(13) + CHAR(10)
SET @PARTENZA = @ARRIVO + 1
GOTO LOOP1

ESCI:


RETURN (@STRINGAOUT)

end






GO
/****** Object:  UserDefinedFunction [dbo].[NUMBER_TO_STR_BASE]    Script Date: 20/05/2026 10:24:42 ******/
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
/****** Object:  Table [dbo].[TbPri]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPri](
	[PriId] [int] NOT NULL,
	[PriProg] [smallint] NOT NULL,
	[PriDataGio] [smalldatetime] NOT NULL,
	[PriCausale] [smallint] NOT NULL,
	[PriCoDare] [varchar](5) NOT NULL,
	[PriCoAvere] [varchar](5) NOT NULL,
	[PriNumProt] [int] NOT NULL,
	[PriBisRet] [varchar](1) NOT NULL,
	[PriCodIva] [smallint] NOT NULL,
	[PriRegIva] [smallint] NOT NULL,
	[PriImpDare] [decimal](13, 2) NOT NULL,
	[PriImpAvere] [decimal](13, 2) NOT NULL,
	[PriDesc] [varchar](24) NOT NULL,
	[PriDocEst] [int] NOT NULL,
	[PriMeseSk] [varchar](1) NOT NULL,
	[PriDataEst] [smalldatetime] NOT NULL,
	[PriDescB] [varchar](32) NOT NULL,
	[PriFl04] [smallint] NOT NULL,
	[PriFl05] [smallint] NOT NULL,
	[PriFl06] [smallint] NOT NULL,
	[PriNsRif] [varchar](7) NOT NULL,
	[PriSos] [varchar](5) NOT NULL,
	[PriLinea] [varchar](3) NOT NULL,
	[PriDocAnn] [smallint] NOT NULL,
	[PriCodPag] [smallint] NOT NULL,
	[PriValuta] [decimal](13, 2) NOT NULL,
	[PriArtFisc] [int] NOT NULL,
	[PriIvaPrint] [bit] NOT NULL,
	[PriGStampa] [bit] NOT NULL,
 CONSTRAINT [PK_TbPri] PRIMARY KEY CLUSTERED 
(
	[PriId] ASC,
	[PriProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[fnControllo]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnControllo](@ANNO smallINT)
returns table
as
return 

select DISTINCT ANACOD,ANADESC ,ANAPIVA,' ' as ErrP,ANACFIS, ' ' as ErrC,ANAPIVAEST ,ANAGRP from vdox.dbo.tbana 
where anagrp = 'CL' AND ANACOD IN (SELECT PRICODARE FROM TBPRI WHERE DATEPART(YEAR,PRIDATAGIO) = @ANNO AND PRICAUSALE = 3)
union all
select DISTINCT ANACOD,ANADESC ,ANAPIVA,' ' as ErrP,ANACFIS, ' ' as ErrC,ANAPIVAEST ,ANAGRP from vdox.dbo.tbana 
where anagrp = 'FO' AND ANACOD IN (SELECT PRICOAVERE FROM TBPRI WHERE DATEPART(YEAR,PRIDATAGIO) = @ANNO AND PRICAUSALE = 3)


GO
/****** Object:  Table [dbo].[TbQuo]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbQuo](
	[QuoNum] [int] NOT NULL,
	[QuoAnno] [smallint] NOT NULL,
	[QuoCoStorIni] [decimal](13, 2) NOT NULL,
	[QuoCoAmmIni] [decimal](13, 2) NOT NULL,
	[QuoFondoIni] [decimal](13, 2) NOT NULL,
	[QuoResiduoIni] [decimal](13, 2) NOT NULL,
	[QuoNoDetraIni] [decimal](13, 2) NOT NULL,
	[QuoCoStor] [decimal](13, 2) NOT NULL,
	[QuoCoAmm] [decimal](13, 2) NOT NULL,
	[QuoTipoAmm] [smallint] NOT NULL,
	[QuoAli] [decimal](5, 2) NOT NULL,
	[QuoQuota] [decimal](13, 2) NOT NULL,
	[QuoFondo] [decimal](13, 2) NOT NULL,
	[QuoResiduo] [decimal](13, 2) NOT NULL,
	[QuoNoDetra] [decimal](13, 2) NOT NULL,
 CONSTRAINT [PK_TbQuo] PRIMARY KEY CLUSTERED 
(
	[QuoNum] ASC,
	[QuoAnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCsp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCsp](
	[CspGru] [varchar](3) NOT NULL,
	[CspSpe1] [varchar](2) NOT NULL,
	[CspSpe2] [varchar](1) NOT NULL,
	[CspNum] [varchar](2) NOT NULL,
	[CspDesc] [varchar](50) NOT NULL,
	[CspPerc] [decimal](5, 2) NOT NULL,
	[CspCp] [bit] NOT NULL,
	[CspCespiti] [varchar](5) NOT NULL,
	[CspFondoAmm] [varchar](5) NOT NULL,
	[CspQuotaNormale] [varchar](5) NOT NULL,
	[CspQuotaAnticipata] [varchar](5) NOT NULL,
 CONSTRAINT [PK_TbCsp] PRIMARY KEY CLUSTERED 
(
	[CspGru] ASC,
	[CspSpe1] ASC,
	[CspSpe2] ASC,
	[CspNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCesp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCesp](
	[CespNum] [int] IDENTITY(1,1) NOT NULL,
	[CespAnnoA] [smallint] NOT NULL,
	[CespCat] [varchar](2) NOT NULL,
	[CespContoStorico] [varchar](5) NOT NULL,
	[CespAliFis] [decimal](5, 2) NOT NULL,
	[CespDescr] [varchar](110) NOT NULL,
	[CespDataFat] [smalldatetime] NULL,
	[CespProtFat] [int] NOT NULL,
	[CespCostoStorico] [decimal](13, 2) NOT NULL,
	[CespContoQuota] [varchar](5) NOT NULL,
	[CespContoFondo] [varchar](5) NOT NULL,
	[CespContoQuotaAnt] [varchar](5) NOT NULL,
	[CespAliTab] [decimal](5, 2) NOT NULL,
	[CespDataCessione] [smalldatetime] NULL,
	[CespUltAnnoAmm] [smallint] NOT NULL,
	[CespCp] [bit] NOT NULL,
 CONSTRAINT [PK_TbCes] PRIMARY KEY CLUSTERED 
(
	[CespNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VCespCat]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VCespCat]
AS
SELECT     dbo.TbCesp.CespNum, dbo.TbCesp.CespAnnoA, dbo.TbCesp.CespCat, dbo.TbCesp.CespContoStorico, dbo.TbCesp.CespAliFis, dbo.TbCesp.CespDescr, 
                      dbo.TbCesp.CespDataFat, dbo.TbCesp.CespProtFat, dbo.TbCesp.CespCostoStorico, dbo.TbCesp.CespContoQuota, dbo.TbCesp.CespContoQuotaAnt, 
                      dbo.TbCesp.CespContoFondo, dbo.TbCesp.CespAliTab, dbo.TbCesp.CespDataCessione, dbo.TbCesp.CespUltAnnoAmm, dbo.TbCsp.CspDesc, 
                      dbo.TbCsp.CspGru, dbo.TbCsp.CspSpe1, dbo.TbCsp.CspSpe2, dbo.TbCesp.CespCp, CspCp, 
                      Cp = CASE WHEN dbo.TbCesp.CespCp = 0 THEN cast(0 as bit) ELSE cast(1 as bit) END,
                       ceduto = CASE WHEN isnull(cespdatacessione, '') > '' THEN cast(1 as bit) ELSE cast(0 as bit) END
FROM         dbo.TbCesp INNER JOIN
                      dbo.TbCsp ON dbo.TbCesp.CespCat = dbo.TbCsp.CspNum


GO
/****** Object:  Table [dbo].[TbVCesp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbVCesp](
	[VCespNum] [int] NOT NULL,
	[VCespAnno] [smallint] NOT NULL,
	[VCespProg] [int] NOT NULL,
	[VCespGgMm] [varchar](4) NOT NULL,
	[VCespProt] [int] NOT NULL,
	[VCespCaus] [smallint] NOT NULL,
	[VCespVariazioni] [decimal](13, 2) NOT NULL,
	[VCespCessioni] [decimal](13, 2) NOT NULL,
	[VCespPlusMinus] [decimal](13, 2) NOT NULL,
	[VCespAnnota] [varchar](25) NOT NULL,
 CONSTRAINT [PK_TbVCes] PRIMARY KEY CLUSTERED 
(
	[VCespNum] ASC,
	[VCespAnno] ASC,
	[VCespProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VCesp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create View [dbo].[VCesp]
as
Select *,Causale = 
      CASE VCespCaus 
	WHEN '1' THEN '1 - Variazione Cespite'
        WHEN '2' THEN '2 - Cessione Cespite'
        WHEN '3' THEN '3 - Variazione Fondo'
       END,
substring(vcespggmm,1,2) + '/' + substring(vcespggmm,3,2) as GgMm from TbVCesp
GO
/****** Object:  Table [dbo].[TbAzi]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAzi](
	[AziAnnoLavoro] [smallint] NOT NULL,
	[AziCod] [varchar](5) NOT NULL,
	[AziRegimeIva] [varchar](1) NOT NULL,
	[AziEsente] [decimal](5, 2) NOT NULL,
	[AziGruppoCesp] [varchar](3) NOT NULL,
	[AziSpecieCesp] [varchar](2) NOT NULL,
	[AziSottosCesp] [varchar](1) NOT NULL,
	[AziUfficioIva] [varchar](25) NULL,
	[AziCodIstat] [varchar](5) NOT NULL,
	[AziCodAttivita] [varchar](4) NULL,
	[AziDescAttivita] [varchar](35) NULL,
	[AziCognome] [varchar](25) NULL,
	[AziNome] [varchar](25) NULL,
	[AziDataNascita] [smalldatetime] NULL,
	[AziSesso] [varchar](1) NULL,
	[AziComuneNascita] [varchar](25) NULL,
	[AziProvNascita] [varchar](2) NULL,
	[AziNaturaIva] [varchar](2) NULL,
	[AziNaturaRedditi] [varchar](2) NULL,
	[AziScritture] [bit] NULL,
	[AziLuoghiAttivita] [bit] NULL,
	[AziLeaCap] [varchar](5) NULL,
	[AziLeaComune] [varchar](25) NULL,
	[AziLeaProv] [varchar](2) NULL,
	[AziLeaIndirizzo] [varchar](25) NULL,
	[AziDrrCodiceFisc] [varchar](16) NULL,
	[AziDrrCognome] [varchar](25) NULL,
	[AziDrrNome] [varchar](25) NULL,
	[AziDrrSesso] [varchar](1) NULL,
	[AziDrrDataNascita] [smalldatetime] NULL,
	[AziDrrCarica770] [varchar](1) NULL,
	[AziDrrCapNascita] [varchar](5) NULL,
	[AziDrrComuneNascita] [varchar](25) NULL,
	[AziDrrProvNascita] [varchar](2) NULL,
	[AziDrrCapRes] [varchar](5) NULL,
	[AziDrrComuneRes] [varchar](25) NULL,
	[AziDrrProvRes] [varchar](2) NULL,
	[AziDrrIndirizzo] [varchar](25) NULL,
	[AziDscCodiceFisc] [varchar](16) NULL,
	[AziDscCognome] [varchar](25) NULL,
	[AziDscNome] [varchar](25) NULL,
	[AziDscCap] [varchar](5) NULL,
	[AziDscComune] [varchar](25) NULL,
	[AziDscProv] [varchar](2) NULL,
	[AziDscIndirizzo] [varchar](25) NULL,
	[AziCodAteco] [varchar](8) NULL,
 CONSTRAINT [PK_TbAzi] PRIMARY KEY CLUSTERED 
(
	[AziAnnoLavoro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VLibroCesp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[VLibroCesp]
as
SELECT top 100 percent *,
	Caus = CASE VCespCaus 
	WHEN '1' THEN 'VARIAZIONE' 
	WHEN '2' THEN 'CESSIONE' 
	WHEN '3' THEN 'VARIAZIONE'
        END,
	Segno = CASE 
	WHEN VCespVariazioni < 0 THEN '-' 
	WHEN VCespVariazioni >= 0 THEN '+' 
	END 
FROM VCespCat
inner join TbQuo on QuoNum=CespNum
left outer JOIN VCesp on QuoNum=VCespNum and QuoAnno=VCespAnno
where CspGru=(select Top 1 AziGruppoCesp from TbAzi order by AziAnnoLavoro Desc) and 
	CspSpe1=(select Top 1 AzispecieCesp from TbAzi order by AziAnnoLavoro Desc) and 
	CspSpe2=(select Top 1 AziSottosCesp from TbAzi order by AziAnnoLavoro Desc)
order by CespCat,CespNum


GO
/****** Object:  Table [dbo].[TMPXMLREGIVA_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPXMLREGIVA_EST](
	[PRegAnno] [smallint] NOT NULL,
	[PRegPeriodo] [smallint] NOT NULL,
	[PRegTipo] [varchar](2) NOT NULL,
	[PRegId] [int] NOT NULL,
	[PRegDataG] [smalldatetime] NOT NULL,
	[PRegDataE] [smalldatetime] NOT NULL,
	[PRegNumProt] [int] NOT NULL,
	[PRegProtBis] [varchar](1) NOT NULL,
	[PRegNumDoc] [int] NOT NULL,
	[PRegCliFor] [varchar](5) NOT NULL,
	[PRegAnaGraf] [varchar](60) NOT NULL,
	[PRegImpon] [decimal](13, 2) NOT NULL,
	[PRegCodIva] [smallint] NOT NULL,
	[PRegAliq] [varchar](12) NOT NULL,
	[PRegImpIva] [decimal](13, 2) NOT NULL,
	[PRegCpt] [varchar](5) NOT NULL,
	[PRegValuta] [decimal](13, 2) NOT NULL,
	[PRegPND] [smallint] NOT NULL,
	[PRegMerce] [smallint] NOT NULL,
	[PRegNumReg] [smallint] NOT NULL,
	[PRegPriId] [int] NOT NULL,
	[PRegPriProg] [smallint] NOT NULL,
	[PRegOK] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbErrP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbErrP](
	[PErrAnno] [smallint] NOT NULL,
	[PErrPeriodo] [tinyint] NOT NULL,
	[PErrTipo] [varchar](2) NOT NULL,
	[PErrClifor] [varchar](5) NOT NULL,
	[PErrAnaDesc] [varchar](60) NOT NULL,
	[PErrPiva] [varchar](11) NOT NULL,
	[PerrCFis] [varchar](16) NOT NULL,
	[PErrPivaEst] [varchar](20) NOT NULL,
	[PErrEscludi] [bit] NOT NULL,
	[PErrMsg] [varchar](200) NOT NULL,
	[PErrErrore] [bit] NOT NULL,
 CONSTRAINT [PK_TbErrP_1] PRIMARY KEY CLUSTERED 
(
	[PErrAnno] ASC,
	[PErrPeriodo] ASC,
	[PErrTipo] ASC,
	[PErrClifor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbRegIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRegIva](
	[RIvaAnno] [smallint] NOT NULL,
	[RIvaNReg] [smallint] NOT NULL,
	[RIvaSL] [varchar](5) NULL,
	[RIvaNumFog] [int] NOT NULL,
	[RIvaTipo] [smallint] NOT NULL,
	[RIvaPRata] [bit] NOT NULL,
	[RIvaDesc] [varchar](35) NULL,
	[RIvaCpt] [varchar](5) NULL,
	[RIvaAutoFCee] [smallint] NULL,
	[RIvaCptCee] [varchar](5) NULL,
	[RIvaCptSosp] [varchar](5) NULL,
	[RIvaCliCee] [varchar](5) NULL,
	[RIvaPrintIniziale] [bit] NOT NULL,
	[RIvaProtCar] [int] NULL,
	[RIvaProtCarRBS] [varchar](1) NULL,
	[RIvaGData] [smalldatetime] NULL,
	[RIvaProtSta] [int] NULL,
	[RIvaProtStaRBS] [varchar](1) NULL,
	[RIvaUData] [smalldatetime] NULL,
	[RIvaArt] [smallint] NOT NULL,
	[RivaInt] [bit] NOT NULL,
	[RivaCh] [bit] NOT NULL,
	[RivaRCharge] [bit] NULL,
	[RIvaFteP] [bit] NOT NULL,
	[RivaTipoDoc] [varchar](4) NOT NULL,
 CONSTRAINT [PK_TbRegIva] PRIMARY KEY CLUSTERED 
(
	[RIvaAnno] ASC,
	[RIvaNReg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VELEXMLFATTURE_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VELEXMLFATTURE_EST]
AS
select  EleCfAnno= PRegAnno,
        EleCfPeriodo = PRegPeriodo,
        EleCfTipo = PRegTipo,
        EleCfCodice = PRegCliFor , 
        EleCfAnaDesc = PRegAnaGraf,
		EleCfPriRegIva =PRegNumReg,
        EleCfDataDoc = PRegDataE,
        EleCfNumDoc = PRegNumDoc,
        EleCfImponibile=SUM(PRegImpon), 
        EleCfIva=sum(PRegImpIva),
        Totale= SUM(PRegImpon) +  sum(PRegImpIva),
		DescRegistro = (select top 1 RIvaDesc from TbRegiva where RIvaNReg = PRegNumReg order by RIvaAnno desc),
		EleCfNumProt=PRegNumProt,
		EleCfRegistrazione = PRegDataG,
		EleCfOK = PRegOK,
		EleId = PRegPriId
from TMPXMLREGIVA_EST INNER JOIN
     TbErrP on	PREGANNO = PErranno and PRegPeriodo = PErrPeriodo and PRegCliFor = PErrClifor
WHERE PErrEscludi = 0
group by PRegAnno,PRegPeriodo,PRegTipo,PRegCliFor,PRegAnaGraf,PRegNumReg,PRegDataE,PRegNumDoc,PRegNumProt,PRegDataG,PRegOK,PRegPriId



GO
/****** Object:  Table [dbo].[TbPia]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPia](
	[PiaCodCo] [varchar](5) NOT NULL,
	[PiaAnaCo] [varchar](32) NOT NULL,
	[PiaFl01] [smallint] NOT NULL,
	[PiaFl02] [smallint] NOT NULL,
	[PiaFl03] [smallint] NOT NULL,
	[PiaFl04] [smallint] NOT NULL,
	[PiaFl05] [smallint] NOT NULL,
	[PiaFl06] [smallint] NOT NULL,
	[PiaFl07] [smallint] NOT NULL,
	[PiaFl08] [bit] NOT NULL,
	[PiaFl09] [bit] NOT NULL,
	[PiaFl10] [smallint] NOT NULL,
	[PiaFl11] [smallint] NOT NULL,
	[PiaFl12] [smallint] NOT NULL,
 CONSTRAINT [PK_TbPia] PRIMARY KEY CLUSTERED 
(
	[PiaCodCo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPrk]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPrk](
	[PrkId] [int] NOT NULL,
	[PrkProg] [smallint] NOT NULL,
	[PrkDa] [varchar](1) NOT NULL,
	[PrkTipoCo] [varchar](1) NOT NULL,
	[PrkConto] [varchar](5) NOT NULL,
	[PrkAammgg] [smalldatetime] NOT NULL,
	[PrkDocAnn] [smallint] NOT NULL,
	[PrkDocEst] [int] NOT NULL,
	[PrkPAperta] [bit] NOT NULL,
 CONSTRAINT [PK_TbPrk_1] PRIMARY KEY CLUSTERED 
(
	[PrkId] ASC,
	[PrkProg] ASC,
	[PrkDa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCii]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCii](
	[CiiCod] [smallint] NOT NULL,
	[CiiAli] [smallint] NOT NULL,
	[CiiInd] [smallint] NOT NULL,
	[CiiTp] [smallint] NOT NULL,
	[CiiCmp] [smallint] NOT NULL,
	[CiiDes] [varchar](12) NOT NULL,
	[CiiCau] [varchar](12) NOT NULL,
	[CiiNatura] [varchar](4) NOT NULL,
	[CiiSoggBollo] [bit] NOT NULL,
 CONSTRAINT [PK_TbCii] PRIMARY KEY CLUSTERED 
(
	[CiiCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VH8EX]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VH8EX]
AS
Select top 100 percent priartfisc,prkconto,prkTipoCo,
PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PRKCONTO AND ANAGRP = 'CL' OR ANACOD = PRKCONTO AND ANAGRP = 'FO') END,
pridatagio,pridataest,cau.CiiCau as Causale,PriNumProt,PriregIva,PriDocEst,
CPT = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end,
CPTDESC = ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end),
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'CL' OR ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'FO')), 
DARE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then 0 when pricausale = 2 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when prkda = 0 then priimpdare else 0 end),
AVERE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when pricausale = 2 then 0 when prkda = 0 then 0 else  priimpavere end),
pridesc+pridescb as descriz,PrkAammgg,
PIAFL = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE 0 END,
PRICAUSALE
 from Tbpri 
inner join Tbprk on prkId = priid and PrkProg = priProg
left outer join tbcii as cau on pricausale = cau.ciicod
left outer join tbcii as cii on pricodiva = cii.ciicod
order by prkTipoCo,PrkConto,PridataGio,PriregIva,PriNumProt
GO
/****** Object:  Table [dbo].[TMPBILS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPBILS](
	[TMSBLOCK] [int] NOT NULL,
	[TMSCONTO] [varchar](5) NULL,
	[TMSDESC] [varchar](90) NULL,
	[TMSFLAG] [smallint] NULL,
	[TMSDARE] [decimal](13, 2) NULL,
	[TMSAVERE] [decimal](13, 2) NULL,
	[TMSSALDO] [decimal](13, 2) NULL,
	[TMSDAREP] [decimal](13, 2) NULL,
	[TMSAVEREP] [decimal](13, 2) NULL,
	[TMSSALDOP] [decimal](13, 2) NULL,
	[TMSMASTRO] [varchar](2) NULL,
	[TMSCAUS] [smallint] NULL,
	[TMSDEMAS] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRBILPROG]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRBILPROG]
AS
select distinct TOP 100 PERCENT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,sum(TMSDARE) as TMSDARE,SUM(TMSAVERE) AS TMSAVERE,SUM(TMSSALDO) AS TMSSALDO,
SUM(TMSDAREP) AS TMSDAREP,SUM(TMSAVEREP) AS TMSAVEREP,SUM(TMSSALDOP) AS TMSSALDOP,TMSMASTRO,
TMSSPPP = CASE WHEN TMSFLAG <> 6 AND TMSFLAG <> 7 then 0 else 1 end,TMSDEMAS
FROM TMPBILS
where SUBSTRING(TMSCONTO,4,2) <> '00'
group by TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSMASTRO,TMSDEMAS
ORDER BY TMSSPPP,TMSMASTRO,TMSCONTO

GO
/****** Object:  View [dbo].[VCHIUSURA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VCHIUSURA]
AS
select TOP 100 PERCENT TMSCONTO,TMSDESC,(TMSSALDO + TMSSALDOP) as SALDO,TMSFLAG ,TMSBLOCK FROM CRBILPROG where (TMSSALDO + TMSSALDOP) <>0

GO
/****** Object:  Table [dbo].[TbRegioni]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRegioni](
	[RegCod] [smallint] NOT NULL,
	[RegDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbReg] PRIMARY KEY CLUSTERED 
(
	[RegCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbProv]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbProv](
	[ProvCod] [varchar](2) NOT NULL,
	[ProvDesc] [varchar](50) NOT NULL,
	[ProvReg] [smallint] NOT NULL,
 CONSTRAINT [PK_TbProv] PRIMARY KEY CLUSTERED 
(
	[ProvCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCom]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCom](
	[ComCod] [varchar](4) NOT NULL,
	[ComCap] [varchar](5) NOT NULL,
	[ComDesc] [varchar](25) NOT NULL,
	[ComProv] [varchar](2) NOT NULL,
 CONSTRAINT [PK_TbCom] PRIMARY KEY CLUSTERED 
(
	[ComCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VComuni]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VComuni]
AS
SELECT     dbo.TbCom.ComCod, dbo.TbCom.ComCap, dbo.TbCom.ComDesc, dbo.TbCom.ComProv, dbo.TbProv.ProvDesc, dbo.TbProv.ProvReg, 
                      dbo.TbRegioni.RegDesc
FROM         dbo.TbCom LEFT OUTER JOIN
                      dbo.TbProv ON dbo.TbCom.ComProv = dbo.TbProv.ProvCod LEFT OUTER JOIN
                      dbo.TbRegioni ON dbo.TbProv.ProvReg = dbo.TbRegioni.RegCod


GO
/****** Object:  View [dbo].[VH8NEW]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VH8NEW]
AS
Select top 100 percent priartfisc,prkconto,prkTipoCo,
PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PRKCONTO AND ANAGRP = 'CL' OR ANACOD = PRKCONTO AND ANAGRP = 'FO') END,
pridatagio,pridataest,cau.CiiCau as Causale,PriNumProt,PriregIva,PriDocEst,
CPT = CASE WHEN PRICAUSALE = 3 THEN (SELECT CASE WHEN PRICODARE <> PRKCONTO THEN PRICODARE ELSE PRICOAVERE END FROM TBPRI AS Q WHERE Q.PRIID = PRKID AND Q.PRIPROG = (PRKPROG - 1) )WHEN PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end,
CPTDESC = ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = CASE WHEN PRICAUSALE = 3 THEN (SELECT CASE WHEN PRICODARE <> PRKCONTO THEN PRICODARE ELSE PRICOAVERE END FROM TBPRI AS Q WHERE Q.PRIID = PRKID AND Q.PRIPROG = (PRKPROG - 1) ) when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end),
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'CL' OR ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'FO')), 
DARE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then 0 when pricausale = 2 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when prkda = 0 then priimpdare else 0 end),
AVERE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when pricausale = 2 then 0 when prkda = 0 then 0 else  priimpavere end),
pridesc+pridescb as descriz,PrkAammgg,
PIAFL = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE 0 END,
PRICAUSALE from Tbpri 
inner join Tbprk on prkId = priid and PrkProg = priProg
left outer join tbcii as cau on pricausale = cau.ciicod
left outer join tbcii as cii on pricodiva = cii.ciicod
--order by prkTipoCo,PrkConto,PridataGio,PriregIva,PriNumProt
GO
/****** Object:  Table [dbo].[TMPBILC]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPBILC](
	[TMCBLOCK] [int] NOT NULL,
	[TMCCONTO] [varchar](5) NULL,
	[TMCDESC] [varchar](90) NULL,
	[TMCFLAG] [smallint] NULL,
	[TMCDARE] [decimal](13, 2) NULL,
	[TMCAVERE] [decimal](13, 2) NULL,
	[TMCSALDO] [decimal](13, 2) NULL,
	[TMCDAREP] [decimal](13, 2) NULL,
	[TMCAVEREP] [decimal](13, 2) NULL,
	[TMCSALDOP] [decimal](13, 2) NULL,
	[TMCCPT] [varchar](5) NULL,
	[TMCCAUS] [smallint] NULL,
	[TMCTIPO] [smallint] NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRBILCLFO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRBILCLFO]
AS
select distinct TOP 100 PERCENT TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,sum(TMCDARE) as TMCDARE,SUM(TMCAVERE) AS TMCAVERE,SUM(TMCSALDO) AS TMCSALDO,
SUM(TMCDAREP) AS TMCDAREP,SUM(TMCAVEREP) AS TMCAVEREP,SUM(TMCSALDOP) AS TMCSALDOP,TMCTIPO
FROM TMPBILC
group by TMCBLOCK,TMCTIPO,TMCCONTO,TMCDESC,TMCFLAG
ORDER BY TMCTIPO,TMCCONTO,TMCDESC

GO
/****** Object:  View [dbo].[VCHIUCLFO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VCHIUCLFO]
AS
select TOP 100 PERCENT TMCCONTO,TMCDESC,(TMCSALDO + TMCSALDOP) as SALDO,TMCBLOCK FROM CRBILCLFO where (TMCSALDO + TMCSALDOP) <>0
GO
/****** Object:  Table [dbo].[TbIvaP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbIvaP](
	[IvaPAnno] [smallint] NOT NULL,
	[IvaPRegIva] [smallint] NOT NULL,
	[IvaPMese] [smallint] NOT NULL,
	[IvaPCodIva] [smallint] NOT NULL,
	[IvaPImpon] [decimal](13, 2) NOT NULL,
	[IvaPIvaDE] [decimal](13, 2) NOT NULL,
	[IvaPIvaND] [decimal](13, 2) NOT NULL,
	[IvaPImpMerce] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRTOTIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRTOTIVA]
AS
SELECT     TOP 100 PERCENT dbo.TbIvaP.IvaPCodIva AS ASCODIVA, dbo.TbCii.CiiDes AS DESIVA, dbo.TbIvaP.*, dbo.TbRegIva.RIvaTipo AS TIPOREG, 
                      dbo.TbRegIva.RIvaDesc AS DESCREG
FROM         dbo.TbIvaP LEFT OUTER JOIN
                      dbo.TbRegIva ON dbo.TbIvaP.IvaPRegIva = dbo.TbRegIva.RIvaNReg AND dbo.TbIvaP.IvaPAnno = dbo.TbRegIva.RIvaAnno LEFT OUTER JOIN
                      dbo.TbCii ON dbo.TbIvaP.IvaPCodIva = dbo.TbCii.CiiCod
ORDER BY dbo.TbIvaP.IvaPAnno, dbo.TbIvaP.IvaPRegIva, dbo.TbIvaP.IvaPMese
GO
/****** Object:  Table [dbo].[TMPXMLINVIATE_ELT_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPXMLINVIATE_ELT_EST](
	[PRegAnno] [smallint] NOT NULL,
	[PRegPeriodo] [smallint] NOT NULL,
	[PRegTipo] [varchar](2) NOT NULL,
	[PRegId] [int] NOT NULL,
	[PRegDataG] [smalldatetime] NOT NULL,
	[PRegDataE] [smalldatetime] NOT NULL,
	[PRegNumProt] [int] NOT NULL,
	[PRegProtBis] [varchar](1) NOT NULL,
	[PRegNumDoc] [int] NOT NULL,
	[PRegCliFor] [varchar](5) NOT NULL,
	[PRegAnaGraf] [varchar](60) NOT NULL,
	[PRegImpon] [decimal](13, 2) NOT NULL,
	[PRegCodIva] [smallint] NOT NULL,
	[PRegAliq] [varchar](12) NOT NULL,
	[PRegImpIva] [decimal](13, 2) NOT NULL,
	[PRegCpt] [varchar](5) NOT NULL,
	[PRegValuta] [decimal](13, 2) NOT NULL,
	[PRegPND] [smallint] NOT NULL,
	[PRegMerce] [smallint] NOT NULL,
	[PRegNumReg] [smallint] NOT NULL,
	[PRegPriId] [int] NOT NULL,
	[PRegPriProg] [smallint] NOT NULL,
	[PRegOK] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VELEXMLFATTURE_ELT_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[VELEXMLFATTURE_ELT_EST]
AS
select  EleCfAnno= PRegAnno,
        EleCfPeriodo = PRegPeriodo,
        EleCfTipo = PRegTipo,
        EleCfCodice = PRegCliFor , 
        EleCfAnaDesc = PRegAnaGraf,
		EleCfPriRegIva =PRegNumReg,
        EleCfDataDoc = PRegDataE,
        EleCfNumDoc = PRegNumDoc,
        EleCfImponibile=SUM(PRegImpon), 
        EleCfIva=sum(PRegImpIva),
        Totale= SUM(PRegImpon) +  sum(PRegImpIva),
		DescRegistro = (select top 1 RIvaDesc from TbRegiva where RIvaNReg = PRegNumReg order by RIvaAnno desc),
		EleCfNumProt=PRegNumProt,
		EleCfRegistrazione = PRegDataG,
		EleId = PRegPriId
from TMPXMLINVIATE_ELT_EST 
group by PRegAnno,PRegPeriodo,PRegTipo,PRegCliFor,PRegAnaGraf,PRegNumReg,PRegDataE,PRegNumDoc,PRegNumProt,PRegDataG,PRegOK,PRegPriId






GO
/****** Object:  View [dbo].[CRACQUANN]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRACQUANN]
AS
SELECT DISTINCT 
                      IvaPAnno AS ANNO, ASCODIVA, DESIVA, SUM(IvaPImpon) AS IMPO, SUM(IvaPIvaDE) AS IVADED, SUM(IvaPIvaND) AS IVAND, SUM(IvaPImpMerce) 
                      AS MERCE
FROM         dbo.CRTOTIVA
WHERE     (TIPOREG = 2) OR
                      (TIPOREG = 4)
GROUP BY IvaPAnno, ASCODIVA, DESIVA
GO
/****** Object:  View [dbo].[VB8]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VB8]
AS
Select top 100 percent prkconto,prkTipoCo,
PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PRKCONTO AND ANAGRP = 'CL' OR ANACOD = PRKCONTO AND ANAGRP = 'FO') END,
pridataest,cau.CiiCau as Causale,PriNumProt,PriregIva,PrkDocEst,
DARE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then 0 when pricausale = 2 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when prkda = 0 then priimpdare else 0 end),
AVERE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when pricausale = 2 then 0 when prkda = 0 then 0 else  priimpavere end),
pridesc+pridescb as descriz,PRICAUSALE,PrkDocAnn,PRKAAMMGG,
PARTITARIO = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAFL09 FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE 1 END,
PrkPAperta,PRIID,
PRKAST = CASE WHEN prkpAperta = 1 then '*' else ' ' end,
FORMULA = PRKDOCANN * 1000000 + prkdocest,
PRKINDI = CASE WHEN PRKTIPOCO = 0 THEN '' ELSE
(SELECT ANAINDIRIZZO+' '+ANACAP+' '+ANACITTA+' '+ANAPROV FROM VDOX.DBO.TBANA WHERE ANACOD = PRKCONTO AND ANAGRP = 'CL' OR ANACOD = PRKCONTO AND ANAGRP = 'FO') END,
PRIPROG,PRKDA
 from Tbprk 
inner join Tbpri on prkId = priid and PrkProg = PriProg
left outer join tbcii as cau on pricausale = cau.ciicod
left outer join tbcii as cii on pricodiva = cii.ciicod

GO
/****** Object:  View [dbo].[VB8SC]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VB8SC]
AS
SELECT DISTINCT PrkConto,PrkDocAnn,PrkDocEst,sum(DARE) - SUM(AVERE) AS SCOPERTO,PrkPAperta,AnaDesc
 FROM vb8 left outer join 
vdox.dbo.TbAna on PrkConto = anacod
where PrkPaperta = 0
GROUP BY PRKCONTO,AnaDesc,PrkDocAnn,PrkDocEst,PrkPAperta

GO
/****** Object:  View [dbo].[CRVENDANN]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRVENDANN]
AS
SELECT DISTINCT 
                      IvaPAnno AS ANNO, ASCODIVA, DESIVA, SUM(IvaPImpon) AS IMPO, SUM(IvaPIvaDE) AS IVADED, SUM(IvaPIvaND) AS IVAND, SUM(IvaPImpMerce) 
                      AS MERCE
FROM         dbo.CRTOTIVA
WHERE     (TIPOREG = 1) OR
                      (TIPOREG = 3) OR
                      (TIPOREG = 5)
GROUP BY IvaPAnno, ASCODIVA, DESIVA
GO
/****** Object:  Table [dbo].[TbRegXML]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRegXML](
	[RxAnno] [smallint] NOT NULL,
	[RxTipo] [varchar](1) NOT NULL,
	[RxRegistro] [smallint] NOT NULL,
	[RxPeriodo] [tinyint] NOT NULL,
	[RxDal] [smalldatetime] NULL,
	[RxAl] [smalldatetime] NULL,
	[RxProgInvio] [int] NOT NULL,
	[RxSel] [bit] NOT NULL,
	[RxInviato] [bit] NOT NULL,
	[RxNoControl] [bit] NOT NULL,
 CONSTRAINT [PK_TbRegXML] PRIMARY KEY CLUSTERED 
(
	[RxAnno] ASC,
	[RxTipo] ASC,
	[RxRegistro] ASC,
	[RxPeriodo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VREGXML]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VREGXML]
AS

SELECT  RIvaAnno, RIvaNReg, RIvaNumFog,RivaTipo,RIvaDesc, 
        RIvaTipoDesc = CASE RIvaTipo WHEN '1' THEN cast(rivatipo AS varchar) + ' - Vendite' 
		                             WHEN '2' THEN cast(rivatipo AS varchar) + ' - Acquisti' 
									 WHEN '3' THEN cast(rivatipo AS varchar) + ' - Rett. Vendite' 
									 WHEN '4' THEN cast(rivatipo AS varchar) + ' - Rett. Acquisti' 
									 WHEN '5' THEN cast(rivatipo AS varchar) + ' - Corrispettivi' 
									 WHEN '6' THEN cast(rivatipo AS varchar) + ' - In sospensione' 
									 WHEN '7' THEN cast(rivatipo AS varchar) + ' - Acquisti Attivit Agricoltura' 
									 WHEN '8' THEN cast(rivatipo AS varchar) + ' - Autofatture Agricoltura' 
									 WHEN '9' THEN cast(rivatipo AS varchar) + ' - Riepilogativo' END,
		SEL = ISNULL(RxSel,CAST(0 AS BIT)),
		RxPeriodo= isnull(rxperiodo,0),
		RxTipo = CASE WHEN isnull(rxperiodo,0) = 0 THEN (CASE WHEN (RIvaTipo = 1 Or RIvaTipo = 3) THEN 'V'  WHEN (RIvaTipo = 2 Or RIvaTipo = 4) THEN 'A' ELSE '' END) ELSE RxTipo END
from TbRegiva left outer join
     TbRegXml on RxAnno = RivaAnno and RxRegistro = RivaNreg

 
GO
/****** Object:  View [dbo].[CRGRUCESP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRGRUCESP]
AS
SELECT DISTINCT 
                      dbo.TbAzi.AziGruppoCesp, dbo.TbAzi.AziSpecieCesp, dbo.TbAzi.AziSottosCesp, dbo.TbCsp.CspNum, dbo.TbCsp.CspDesc, dbo.TbCsp.CspCp
FROM         dbo.TbAzi INNER JOIN
                      dbo.TbCsp ON dbo.TbAzi.AziGruppoCesp = dbo.TbCsp.CspGru AND dbo.TbAzi.AziSpecieCesp = dbo.TbCsp.CspSpe1 AND 
                      dbo.TbAzi.AziSottosCesp = dbo.TbCsp.CspSpe2
WHERE     (dbo.TbCsp.CspNum <> '00')
GO
/****** Object:  Table [dbo].[TbCgc]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCgc](
	[CgcCod] [varchar](3) NOT NULL,
	[CgcDesc] [varchar](45) NOT NULL,
	[CgcLivello] [smallint] NOT NULL,
 CONSTRAINT [PK_TbCgc] PRIMARY KEY CLUSTERED 
(
	[CgcCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRCGC]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRCGC]
AS
SELECT     TOP 100 PERCENT *
FROM         dbo.TbCgc
ORDER BY CgcCod, CgcLivello
GO
/****** Object:  Table [dbo].[TMPXMLREGIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPXMLREGIVA](
	[PRegAnno] [smallint] NOT NULL,
	[PRegPeriodo] [tinyint] NOT NULL,
	[PRegTipo] [varchar](2) NOT NULL,
	[PRegId] [int] NOT NULL,
	[PRegDataG] [smalldatetime] NOT NULL,
	[PRegDataE] [smalldatetime] NOT NULL,
	[PRegNumProt] [int] NOT NULL,
	[PRegProtBis] [varchar](1) NOT NULL,
	[PRegNumDoc] [int] NOT NULL,
	[PRegCliFor] [varchar](5) NOT NULL,
	[PRegAnaGraf] [varchar](60) NOT NULL,
	[PRegImpon] [decimal](13, 2) NOT NULL,
	[PRegCodIva] [smallint] NOT NULL,
	[PRegAliq] [varchar](12) NOT NULL,
	[PRegImpIva] [decimal](13, 2) NOT NULL,
	[PRegCpt] [varchar](5) NOT NULL,
	[PRegValuta] [decimal](13, 2) NOT NULL,
	[PRegPND] [smallint] NOT NULL,
	[PRegMerce] [smallint] NOT NULL,
	[PRegNumReg] [smallint] NOT NULL,
	[PRegPriId] [int] NOT NULL,
	[PRegPriProg] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VELEXMLFATTURE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VELEXMLFATTURE]
AS
select  EleCfAnno= PRegAnno,
        EleCfPeriodo = PRegPeriodo,
        EleCfTipo = PRegTipo,
        EleCfCodice = PRegCliFor , 
        EleCfAnaDesc = PRegAnaGraf,
		EleCfPriRegIva =PRegNumReg,
        EleCfDataDoc = PRegDataE,
        EleCfNumDoc = PRegNumDoc,
        EleCfImponibile=SUM(PRegImpon), 
        EleCfIva=sum(PRegImpIva),
        Totale= SUM(PRegImpon) +  sum(PRegImpIva),
		DescRegistro = (select top 1 RIvaDesc from TbRegiva where RIvaNReg = PRegNumReg order by RIvaAnno desc),
		EleCfNumProt=PRegNumProt,
		EleCfRegistrazione = PRegDataG
from TMPXMLREGIVA INNER JOIN
     TbErrP on	PREGANNO = PErranno and PRegPeriodo = PErrPeriodo and PRegCliFor = PErrClifor
WHERE PErrEscludi = 0
group by PRegAnno,PRegPeriodo,PRegTipo,PRegCliFor,PRegAnaGraf,PRegNumReg,PRegDataE,PRegNumDoc,PRegNumProt,PRegDataG


GO
/****** Object:  View [dbo].[VCspAzi]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[VCspAzi]
as
select * from TbCsp 
inner join TbAzi on AziGruppoCesp=CspGru and AziSpecieCesp=CspSpe1 and AziSottosCesp=CspSpe2 
where AziAnnoLavoro=datepart(year,getdate())
GO
/****** Object:  View [dbo].[CRTOTCESP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRTOTCESP]
AS
SELECT DISTINCT 
                      TOP 100 PERCENT dbo.TbQuo.QuoAnno, SUM(dbo.TbQuo.QuoCoAmm) AS AMM, SUM(dbo.TbQuo.QuoFondo) AS FONDO, SUM(dbo.TbQuo.QuoResiduo) 
                      AS RESIDUO, SUM(dbo.TbQuo.QuoNoDetra) AS NODETRA, dbo.TbCesp.CespCat, dbo.VCspAzi.CspDesc
FROM         dbo.VCspAzi RIGHT OUTER JOIN
                      dbo.TbCesp ON dbo.VCspAzi.CspNum = dbo.TbCesp.CespCat LEFT OUTER JOIN
                      dbo.TbQuo ON dbo.TbCesp.CespNum = dbo.TbQuo.QuoNum
GROUP BY dbo.TbQuo.QuoAnno, dbo.TbCesp.CespCat, dbo.VCspAzi.CspDesc
ORDER BY dbo.TbQuo.QuoAnno, dbo.TbCesp.CespCat
GO
/****** Object:  View [dbo].[CRPIA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[CRPIA]
AS
SELECT     TOP 100 PERCENT dbo.TbPia.PiaCodCo AS CodiceSottoconto, dbo.TbPia.PiaAnaCo AS Descrizione, SUBSTRING(dbo.TbPia.PiaCodCo, 1, 2) 
                      AS MASTRO, TbPia_1.PiaCodCo, TbPia_1.PiaAnaCo, TbPia_1.PiaFl01, TbPia_1.PiaFl02, TbPia_1.PiaFl03, TbPia_1.PiaFl04, TbPia_1.PiaFl05, 
                      TbPia_1.PiaFl06, TbPia_1.PiaFl07, PiaFl08 = cast(TbPia_1.PiaFl08 as smallint), PiaFl09=cast(TbPia_1.PiaFl09 as smallint), TbPia_1.PiaFl10, TbPia_1.PiaFl11, TbPia_1.PiaFl12
FROM         dbo.TbPia LEFT OUTER JOIN
                      dbo.TbPia TbPia_1 ON SUBSTRING(dbo.TbPia.PiaCodCo, 1, 2) = SUBSTRING(TbPia_1.PiaCodCo, 1, 2) AND SUBSTRING(TbPia_1.PiaCodCo, 4, 2) 
                      <> '00'
WHERE     (SUBSTRING(dbo.TbPia.PiaCodCo, 4, 2) = '00')
ORDER BY dbo.TbPia.PiaCodCo


GO
/****** Object:  Table [dbo].[TMPFTERRORE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPFTERRORE](
	[FeId] [bigint] IDENTITY(1,1) NOT NULL,
	[FeTipo] [varchar](2) NOT NULL,
	[FeRegistro] [smallint] NOT NULL,
	[FeNumProt] [int] NOT NULL,
	[FeCliFor] [varchar](5) NOT NULL,
	[FeNumdoc] [int] NOT NULL,
	[FeDataDoc] [smalldatetime] NOT NULL,
	[FeImponibileImporto] [decimal](13, 2) NOT NULL,
	[FeImposta] [decimal](13, 2) NOT NULL,
	[FeCodiva] [smallint] NOT NULL,
	[FeNatura] [varchar](4) NOT NULL,
	[FeErrore] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TMPFTERRORE_1] PRIMARY KEY CLUSTERED 
(
	[FeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VTMPFTERRORE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VTMPFTERRORE]
AS

SELECT FeTipo, 
       FeRegistro, 
	   FeNumProt, 
	   FeCliFor, 
	   FeNumdoc, 
	   FeDataDoc, 
	   FeImponibileImporto, 
	   FeImposta, 
	   FeCodiva, 
	   FeNatura, 
	   FeErrore,
	   ANAGRAFICA = (Select AnaDesc from vdox.dbo.TbAna where AnaCod = FeCliFor) 	   
FROM TMPFTERRORE
GO
/****** Object:  Table [dbo].[TbGrv]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbGrv](
	[GrvCliCons] [varchar](5) NOT NULL,
	[GrvLisTest] [smallint] NOT NULL,
	[GrvCliFatt] [varchar](5) NOT NULL,
 CONSTRAINT [PK_TbGrv] PRIMARY KEY CLUSTERED 
(
	[GrvCliCons] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VGrv]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[VGrv]
as

select
GrvCliCons,
RGCLICONS = (SELECT rtrim(AnaDesc) + ' - ' + rtrim(AnaIndirizzo) + ' - ' + rtrim(AnaCap) + ' - ' + rtrim(AnaCitta)  from Vdox.dbo.TbAna where anagrp = 'CL' AND AnaCod = GrvCliCons),
GrvCliFatt,
RGCLIFATT = (SELECT AnaDesc from Vdox.dbo.TbAna where anagrp = 'CL' AND AnaCod = GrvCliFatt),
GrvLisTest
from TbGrv


GO
/****** Object:  Table [dbo].[TbGds]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbGds](
	[GdsIdText] [smallint] NOT NULL,
	[GdsTESTI] [varchar](1000) NOT NULL,
	[GdsDataOra] [datetime] NOT NULL,
	[GdsAutorizza] [bit] NOT NULL,
	[GdsArtCod] [smallint] NOT NULL,
	[GdsLingua] [smallint] NOT NULL,
 CONSTRAINT [PK_TbGds] PRIMARY KEY CLUSTERED 
(
	[GdsIdText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VGESTBILANCE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VGESTBILANCE]
AS

select 
CODICE = GdsArtCod,
GdsIdText,
TESTO = GdsTESTI,
TESTOBILANCE = ISNULL(TESTO,''),
GdsAutorizza,
GdsLingua,
GdsDataOra

from TbGds LEFT OUTER join
     GDS...TESTI on Gdsidtext = IDTEXT and centra1 = 1








GO
/****** Object:  View [dbo].[VIvaP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[VIvaP]
as
select ivapanno,ivapregiva,ivapmese,ivapcodiva,ivapimpon,ivapivade 
from tbivap inner join tbregiva on rivaanno=ivapanno and ivapregiva=rivanreg where rivatipo=5
GO
/****** Object:  Table [dbo].[TMPOTTOBRE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPOTTOBRE](
	[A] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VH8OTTOBRE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VH8OTTOBRE]
AS
SELECT     TOP 100 PERCENT priartfisc, prkconto, prkTipoCo, PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN
                          (SELECT     PIAANACO
                            FROM          TBPIA
                            WHERE      PIACODCO = PRKCONTO) ELSE
                          (SELECT     ANADESC
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = PRKCONTO AND ANAGRP = 'CL' OR
                                                   ANACOD = PRKCONTO AND ANAGRP = 'FO') END, pridatagio, pridataest, cau.CiiCau AS Causale, PriNumProt, PriregIva, PriDocEst, 
                      CPT = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare END, 
                      CPTDESC = ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE
                                                     pricodare END),
                          (SELECT     ANADESC
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare
                                                    END AND ANAGRP = 'CL' OR
                                                   ANACOD = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare
                                                    END AND ANAGRP = 'FO')), DARE = CONVERT(DECIMAL(13, 2), 
                      CASE WHEN PriCausale = 1 THEN 0 WHEN pricausale = 2 THEN PriImpDare + (CASE WHEN pricodiva = 0 THEN 0 ELSE priimpavere * cii.ciiInd / 100 END)
                       WHEN prkda = 0 THEN priimpdare ELSE 0 END), AVERE = CONVERT(DECIMAL(13, 2), 
                      CASE WHEN PriCausale = 1 THEN PriImpDare + (CASE WHEN pricodiva = 0 THEN 0 ELSE priimpavere * cii.ciiInd / 100 END) 
                      WHEN pricausale = 2 THEN 0 WHEN prkda = 0 THEN 0 ELSE priimpavere END), pridesc + pridescb AS descriz, PrkAammgg, 
                      PIAFL = CASE WHEN PRKTIPOCO = 0 THEN
                          (SELECT     PIAFL01
                            FROM          TBPIA
                            WHERE      PIACODCO = PRKCONTO) ELSE 0 END, PRICAUSALE, PriSos, MDESC = CASE WHEN Prisos <> '' THEN ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = Prisos), isnull
                          ((SELECT     ANADESC
                              FROM         vdox.dbo.tbana
                              WHERE     ANACOD = Prisos AND ANAGRP = 'CL'), isnull
                          ((SELECT     ANADESC
                              FROM         vdox.dbo.tbana
                              WHERE     ANACOD = Prisos AND ANAGRP = 'FO'), ''))) ELSE '' END
FROM         Tbpri INNER JOIN
                      Tbprk ON prkId = priid AND PrkProg = priProg LEFT OUTER JOIN
                      tbcii AS cau ON pricausale = cau.ciicod LEFT OUTER JOIN
                      tbcii AS cii ON pricodiva = cii.ciicod
					  WHERE PRKID IN(SELECT A FROM TMPOTTOBRE)






GO
/****** Object:  Table [dbo].[TRPRI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRPRI](
	[PriId] [int] NOT NULL,
	[PriProg] [smallint] NOT NULL,
	[PriDataGio] [smalldatetime] NOT NULL,
	[PriCausale] [smallint] NOT NULL,
	[PriCoDare] [varchar](5) NOT NULL,
	[PriCoAvere] [varchar](5) NOT NULL,
	[PriNumProt] [int] NOT NULL,
	[PriBisRet] [varchar](1) NOT NULL,
	[PriCodIva] [smallint] NOT NULL,
	[PriRegIva] [smallint] NOT NULL,
	[PriImpDare] [decimal](13, 2) NOT NULL,
	[PriImpAvere] [decimal](13, 2) NOT NULL,
	[PriDesc] [varchar](24) NOT NULL,
	[PriDocEst] [int] NOT NULL,
	[PriMeseSk] [varchar](1) NOT NULL,
	[PriDataEst] [smalldatetime] NOT NULL,
	[PriDescB] [varchar](32) NOT NULL,
	[PriFl04] [smallint] NOT NULL,
	[PriFl05] [smallint] NOT NULL,
	[PriFl06] [smallint] NOT NULL,
	[PriNsRif] [varchar](7) NOT NULL,
	[PriSos] [varchar](5) NOT NULL,
	[PriLinea] [varchar](3) NOT NULL,
	[PriDocAnn] [smallint] NOT NULL,
	[PriCodPag] [smallint] NOT NULL,
	[PriValuta] [decimal](13, 2) NOT NULL,
	[PriArtFisc] [int] NOT NULL,
	[PriIvaPrint] [bit] NOT NULL,
	[PriGStampa] [bit] NOT NULL,
 CONSTRAINT [PK_TRPRI] PRIMARY KEY CLUSTERED 
(
	[PriId] ASC,
	[PriProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRPRK]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRPRK](
	[PrkId] [int] NOT NULL,
	[PrkProg] [smallint] NOT NULL,
	[PrkDa] [varchar](1) NOT NULL,
	[PrkTipoCo] [varchar](1) NOT NULL,
	[PrkConto] [varchar](5) NOT NULL,
	[PrkAammgg] [smalldatetime] NOT NULL,
	[PrkDocAnn] [smallint] NOT NULL,
	[PrkDocEst] [int] NOT NULL,
	[PrkPAperta] [bit] NOT NULL,
 CONSTRAINT [PK_TRPRK_1] PRIMARY KEY CLUSTERED 
(
	[PrkId] ASC,
	[PrkProg] ASC,
	[PrkDa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[RH8OTTOBRE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[RH8OTTOBRE]
AS
Select top 100 percent priartfisc,prkconto,prkTipoCo,
PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PRKCONTO AND ANAGRP = 'CL' OR ANACOD = PRKCONTO AND ANAGRP = 'FO') END,
pridatagio,pridataest,cau.CiiCau as Causale,PriNumProt,PriregIva,PriDocEst,
CPT = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end,
CPTDESC = ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end),
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'CL' OR ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'FO')), 
DARE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then 0 when pricausale = 2 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when prkda = 0 then priimpdare else 0 end),
AVERE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when pricausale = 2 then 0 when prkda = 0 then 0 else  priimpavere end),
pridesc+pridescb as descriz,PrkAammgg,PRKID,
PIAFL = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE 0 END,
PRICAUSALE,MCPT = CASE WHEN PRICAUSALE = 3 THEN (SELECT CASE WHEN PRICODARE <> PRKCONTO THEN PRICODARE ELSE PRICOAVERE END FROM TRPRI AS Q WHERE Q.PRIID = PRKID AND Q.PRIPROG = (PRKPROG - 1) ) ELSE '*' END 
 from TRPRI 
inner join TRPRK on prkId = priid and PrkProg = priProg
left outer join tbcii as cau on pricausale = cau.ciicod
left outer join tbcii as cii on pricodiva = cii.ciicod
where priid in (select a from tmpottobre)
--order by prkTipoCo,PrkConto,PridataGio,PriregIva,PriNumProt




GO
/****** Object:  UserDefinedFunction [dbo].[fnSCONTIABBUONI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[fnSCONTIABBUONI](@MIN AS DECIMAL(5,2),@MAX AS DECIMAL(5,2),@MIGLIO as INT)
returns table
as
return
SELECT distinct PRKCONTO,PRKDOCANN,PRKDOCEST,(sum(dare)-sum(avere)) AS DIFF 
FROM vb8
where partitario = 1 and PrkPAperta = 0  AND PrkTipoCo = 1 and prkconto < @MIGLIO
group by PRKCONTO,PRKDOCANN,PRKDOCEST
HAVING (sum(dare)-sum(avere))<> 0 AND (sum(dare)-sum(avere)) BETWEEN @MIN AND @MAX 

GO
/****** Object:  Table [dbo].[TbRit]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRit](
	[RitNum] [int] IDENTITY(1,1) NOT NULL,
	[RitCodFor] [varchar](5) NOT NULL,
	[RitDataFat] [smalldatetime] NOT NULL,
	[RitProtFat] [int] NOT NULL,
	[RitDataPag] [smalldatetime] NULL,
	[RitTributo] [varchar](4) NOT NULL,
	[RitCompenso] [decimal](13, 2) NOT NULL,
	[RitPerc] [decimal](4, 2) NOT NULL,
	[RitRitenuta] [decimal](13, 2) NOT NULL,
	[RitPrevPerc] [decimal](13, 2) NOT NULL,
	[RitRimborsi] [decimal](13, 2) NOT NULL,
	[RitRivalsa] [decimal](13, 2) NOT NULL,
	[RitRimbConv] [decimal](13, 2) NOT NULL,
	[RitIva] [decimal](13, 2) NOT NULL,
	[RitDataVer] [smalldatetime] NULL,
	[RitEsCc] [varchar](1) NOT NULL,
	[RitNumQB] [varchar](12) NOT NULL,
	[RitImpVersam] [decimal](13, 2) NOT NULL,
	[RitContrInps] [decimal](13, 2) NOT NULL,
	[RitLetCont10] [varchar](1) NOT NULL,
	[RitSospesa] [decimal](13, 2) NOT NULL,
	[RitPerAtDa] [smalldatetime] NULL,
	[RitPerAtAa] [smalldatetime] NULL,
	[RitRifId] [int] NOT NULL,
 CONSTRAINT [PK_TbRit] PRIMARY KEY CLUSTERED 
(
	[RitNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTrib]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTrib](
	[TribCod] [varchar](4) NOT NULL,
	[TribDesc] [varchar](50) NOT NULL,
	[TribRit] [decimal](5, 2) NOT NULL,
	[TribCaus] [varchar](10) NULL,
 CONSTRAINT [PK_TbTrib] PRIMARY KEY CLUSTERED 
(
	[TribCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRCERTIFIC]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRCERTIFIC]
AS
SELECT     TOP 100 PERCENT RitCodFor, RitDataFat, RitProtFat, RitDataPag, RitTributo, RitSospesa, RitCompenso, RitPerc, RitRitenuta, RitContrInps, 
                      dbo.TbRit.RitRimborsi, dbo.TbRit.RitRivalsa, RitRimbConv, RitIva, RitPrevPerc, RitDataVer, RitEsCc, RitNumQB, RitImpVersam, RitPerAtDa, RitPerAtAa, 
                      TribDesc, TribCaus, AnaCfis, AnaDesc, AnaIndirizzo, AnaCap, AnaCitta, AnaProv, AnaRag1, AnaRag2, DAY(dbo.TbRit.RitDataPag) AS DAYCOMP, 
                      MONTH(dbo.TbRit.RitDataPag) AS MESECOMP, YEAR(dbo.TbRit.RitDataPag) AS ANNOCOMP, YEAR(dbo.TbRit.RitDataVer) AS ANNOVER, 
                      ANNOC = CASE WHEN month(RitDataPag) = 1 AND DAY(RitDataPag) < 13 AND month(RitDataVer) = month(RitDataPag) AND (year(RitDataVer) 
                      = year(dbo.TbRit.RitDataPag)) THEN YEAR(dbo.TbRit.RitDataPag) - 1 ELSE YEAR(dbo.TbRit.RitDataPag) END, COMUNE = isnull
                          ((SELECT     COMDESC
                              FROM         tbcom
                              WHERE     comcod = substring(anacfis, 12, 4)), ''), PROV = isnull
                          ((SELECT     COMPROV
                              FROM         tbcom
                              WHERE     comcod = substring(anacfis, 12, 4)), ''), GG = CASE WHEN CONVERT(SMALLINT, SUBSTRING(ANACFIS, 10, 2)) 
                      > 40 THEN CONVERT(SMALLINT, SUBSTRING(ANACFIS, 10, 2)) - 40 ELSE CONVERT(SMALLINT, SUBSTRING(ANACFIS, 10, 2)) END, 
                      MM = CASE WHEN SUBSTRING(ANACFIS, 9, 1) BETWEEN 'A' AND 'E' THEN ascii(SUBSTRING(ANACFIS, 9, 1)) - 64 WHEN SUBSTRING(ANACFIS, 9, 1) 
                      BETWEEN 'H' AND 'H' THEN ascii(SUBSTRING(ANACFIS, 9, 1)) - 66 WHEN SUBSTRING(ANACFIS, 9, 1) BETWEEN 'L' AND 
                      'M' THEN ascii(SUBSTRING(ANACFIS, 9, 1)) - 69 WHEN SUBSTRING(ANACFIS, 9, 1) BETWEEN 'P' AND 'P' THEN ascii(SUBSTRING(ANACFIS, 9, 1)) 
                      - 71 WHEN SUBSTRING(ANACFIS, 9, 1) BETWEEN 'R' AND 'T' THEN ascii(SUBSTRING(ANACFIS, 9, 1)) - 72 ELSE 99 END, 
                      AA = '19' + SUBSTRING(ANACFIS, 7, 2),
                      ENASARCO = CASE WHEN ISNULL(YEAR(dbo.TbRit.RitDataPag),0) > 0 AND ISNULL(dbo.TbRit.RitContrInps,0) > 0 AND ISNULL(dbo.TbRit.RitPrevPerc,0) > 0 THEN YEAR(dbo.TbRit.RitDataFat) ELSE 0 END
FROM         dbo.TbRit LEFT OUTER JOIN
                      VDOX.DBO.TBANA ON dbo.TbRit.RitCodFor = vdox.dbo.Tbana.AnaCod AND vdox.dbo.Tbana.anagrp = 'FO' LEFT OUTER JOIN
                      dbo.TbTrib ON dbo.TbRit.RitTributo = dbo.TbTrib.TribCod
ORDER BY AnaDesc,RitCodFor,RitDataFat,RitProtFat
GO
/****** Object:  View [dbo].[CRUTE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRUTE]
AS
SELECT     dbo.TbAzi.AziAnnoLavoro, dbo.TbAzi.AziCod, VDOX.dbo.TbAna.AnaDesc AS UTEDESC, VDOX.dbo.TbAna.AnaIndirizzo AS UTEIND, 
                      VDOX.dbo.TbAna.AnaCap AS UTECAP, VDOX.dbo.TbAna.AnaCitta AS UTECITTA, VDOX.dbo.TbAna.AnaProv AS UTEPROV, VDOX.dbo.TbAna.AnaPiva, 
                      VDOX.dbo.TbAna.AnaCfis
FROM         dbo.TbAzi LEFT OUTER JOIN
                      VDOX.dbo.TbAna ON dbo.TbAzi.AziCod = VDOX.dbo.TbAna.AnaCod AND VDOX.dbo.TbAna.AnaGrp = 'AZ'
GO
/****** Object:  Table [dbo].[TbEff]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEff](
	[RicProg] [int] IDENTITY(1,1) NOT NULL,
	[RicClie] [varchar](5) NOT NULL,
	[RicNfat] [int] NOT NULL,
	[RicImpFatt] [decimal](13, 2) NOT NULL,
	[RicAbi] [int] NOT NULL,
	[RicCab] [int] NOT NULL,
	[RicImpRata] [decimal](13, 2) NOT NULL,
	[RicTPag] [smallint] NOT NULL,
	[RicSeff] [smallint] NOT NULL,
	[RicNRata] [smallint] NOT NULL,
	[RicBan] [smallint] NOT NULL,
	[RicFlagCoge] [bit] NOT NULL,
	[RicDfat] [smalldatetime] NOT NULL,
	[RicDsca] [smalldatetime] NOT NULL,
	[RicDDis] [smalldatetime] NOT NULL,
	[RicSomma] [varchar](1) NOT NULL,
	[RicRifFat] [int] NULL,
	[RicNumDis] [int] NULL,
 CONSTRAINT [PK_TbEff] PRIMARY KEY CLUSTERED 
(
	[RicProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VDISTRB2]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VDISTRB2]
AS
SELECT     TOP 100 PERCENT dbo.TbEff.RicDsca AS SCADENZA, dbo.TbEff.RicClie AS CLIENTE, Vdox.dbo.TbAna.AnaDesc AS EMITTENTE, 
                      Vdox.dbo.TbAna.AnaCitta AS PIAZZA, dbo.TbEff.RicImpRata AS IMPRATA, dbo.TbEff.RicImpFatt AS IMPFATT, dbo.TbEff.RicBan AS BANCA, 'E' AS ESPOS, 
                      'NS. RIF. FATT. N.' + CAST(dbo.TbEff.RicNfat AS varchar) + ' DEL ' + CONVERT(char, dbo.TbEff.RicDfat, 103) AS docum, dbo.TbEff.RicAbi AS ABI, 
                      dbo.TbEff.RicCab AS CAB, Vdox.dbo.TbAna.AnaRag1, Vdox.dbo.TbAna.AnaRag2, Vdox.dbo.TbAna.AnaCfis, Vdox.dbo.TbAna.AnaPiva, 
                      Vdox.dbo.TbAna.AnaIndirizzo, Vdox.dbo.TbAna.AnaCap, Vdox.dbo.TbAna.AnaProv, dbo.TbEff.RicProg, DATEPART(yy, dbo.TbEff.RicDfat) AS AAFatt, 
                      DATEPART(mm, dbo.TbEff.RicDfat) AS MMFatt, dbo.TbEff.RicTPag,ClCntRid ,clCC
FROM         dbo.TbEff INNER JOIN
                      Vdox.dbo.TbAna ON dbo.TbEff.RicClie = Vdox.dbo.TbAna.AnaCod
                      INNER JOIN
                      Geve.dbo.TbCli ON dbo.TbEff.RicClie = Geve.dbo.TbCli.ClCod
WHERE     (dbo.TbEff.RicSeff = '2')
ORDER BY dbo.TbEff.RicDsca, Vdox.dbo.TbAna.AnaDesc

GO
/****** Object:  View [dbo].[VDISTRB3]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  VIEW [dbo].[VDISTRB3]
AS
SELECT     TOP 100 PERCENT dbo.TbEff.RicDsca AS SCADENZA, dbo.TbEff.RicClie AS CLIENTE, Vdox.dbo.TbAna.AnaDesc AS EMITTENTE, 
                      Vdox.dbo.TbAna.AnaCitta AS PIAZZA, SUM(dbo.TbEff.RicImpRata) AS IMPRATA, SUM(dbo.TbEff.RicImpFatt) AS IMPFATT, dbo.TbEff.RicTpag,
                      dbo.TbEff.RicBan AS BANCA, ESPOS = CASE WHEN
                          (SELECT     COUNT(*)
                            FROM          TbEff AS ef1
                            WHERE      ef1.RicClie = dbo.TbEff.RicClie AND ef1.RicDsca = dbo.TbEff.RicDsca AND datepart(yy, dbo.TbEff.RicDfat) * 100 + datepart(mm, 
                                                   dbo.TbEff.RicDfat) = datepart(yy, ef1.RicDfat) * 100 + datepart(mm, ef1.RicDfat) AND dbo.TbEff.RicSeff = '2') > 1 THEN 'R' ELSE 'E' END, 
                      docum = CASE WHEN
                          (SELECT     COUNT(*)
                            FROM          TbEff AS ef1
                            WHERE      ef1.RicClie = dbo.TbEff.RicClie AND ef1.RicDsca = dbo.TbEff.RicDsca AND datepart(yy, dbo.TbEff.RicDfat) * 100 + datepart(mm, 
                                                   dbo.TbEff.RicDfat) = datepart(yy, ef1.RicDfat) * 100 + datepart(mm, ef1.RicDfat) AND dbo.TbEff.RicSeff = '2') 
                      > 1 THEN  (SELECT  DISTINCT   'NS. ESTRATTO CONTO DEL MESE DI '  + UPPER(dateNAME(mm, RicDfat)) + ' ' + dateNAME(YY, RicDfat)
                            FROM          TbEff AS ef1
                            WHERE      ef1.RicClie = dbo.TbEff.RicClie AND ef1.RicDsca = dbo.TbEff.RicDsca AND dbo.TbEff.RicSeff = '2' AND datepart(yy, dbo.TbEff.RicDfat) 
                                                   * 100 + datepart(mm, dbo.TbEff.RicDfat) = datepart(yy, ef1.RicDfat) * 100 + datepart(mm, ef1.RicDfat))
ELSE
                          (SELECT     'NS. RIF. FATT. N.' + cast(Ricnfat AS varchar) + ' DEL ' + CONVERT(char, RicDfat, 103)
                            FROM          TbEff AS ef1
                            WHERE      ef1.RicClie = dbo.TbEff.RicClie AND ef1.RicDsca = dbo.TbEff.RicDsca AND dbo.TbEff.RicSeff = '2' AND datepart(yy, dbo.TbEff.RicDfat) 
                                                   * 100 + datepart(mm, dbo.TbEff.RicDfat) = datepart(yy, ef1.RicDfat) * 100 + datepart(mm, ef1.RicDfat)) END, RicAbi AS ABI, 
                      RicCab AS CAB,AnaRag1,Anarag2,AnaCfis,AnaPiva,AnaIndirizzo,AnaCap,AnaProv,
(SELECT top 1 RicProg
                            FROM          TbEff AS ef1
                            WHERE      ef1.RicClie = dbo.TbEff.RicClie AND ef1.RicDsca = dbo.TbEff.RicDsca AND dbo.TbEff.RicSeff = '2' AND datepart(yy, dbo.TbEff.RicDfat) 
                                                   * 100 + datepart(mm, dbo.TbEff.RicDfat) = datepart(yy, ef1.RicDfat) * 100 + datepart(mm, ef1.RicDfat)) as RicProg,
datepart(yy, dbo.TbEff.RicDfat)as AAFatt,datepart(mm, dbo.TbEff.RicDfat)as MMFatt ,ClCntRid,ClCC
FROM         dbo.TbEff INNER JOIN
                      Vdox.dbo.TbAna ON dbo.TbEff.RicClie = Vdox.dbo.TbAna.AnaCod
                       INNER JOIN
                      Geve.dbo.TbCli ON dbo.TbEff.RicClie = Geve.dbo.TbCli.ClCod
WHERE     dbo.TbEff.RicSeff = '2'  
GROUP BY dbo.TbEff.RicDsca, dbo.TbEff.RicClie, Vdox.dbo.TbAna.AnaDesc, datepart(yy, dbo.TbEff.RicDfat) * 100 + datepart(mm, dbo.TbEff.RicDfat), 
                      dbo.TbEff.RicSeff, dbo.TbEff.RicBan, RicAbi, RicCab, Vdox.dbo.TbAna.AnaCitta, dbo.TbEff.RicSeff,AnaRag1,Anarag2,AnaCFis,AnaPiva,AnaIndirizzo,AnaCap,AnaProv, datepart(yy, dbo.TbEff.RicDfat),datepart(mm, dbo.TbEff.RicDfat),
                      dbo.TbEff.RicTpag,ClCntRid,ClCC
ORDER BY dbo.TbEff.RicDsca, Vdox.dbo.TbAna.AnaDesc
GO
/****** Object:  Table [dbo].[TbCee]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCee](
	[CeeCod] [varchar](5) NOT NULL,
	[CeeDesc] [varchar](100) NOT NULL,
	[CeeAlt] [varchar](5) NULL,
	[CeeSt] [varchar](1) NULL,
 CONSTRAINT [PK_TbCee] PRIMARY KEY CLUSTERED 
(
	[CeeCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPANNS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPANNS](
	[TMSBLOCK] [int] NOT NULL,
	[TMSCONTO] [varchar](5) NULL,
	[TMSDESC] [varchar](90) NULL,
	[TMSFLAG] [smallint] NULL,
	[TMSDARE] [decimal](13, 2) NULL,
	[TMSAVERE] [decimal](13, 2) NULL,
	[TMSSALDO] [decimal](13, 2) NULL,
	[TMSDAREP] [decimal](13, 2) NULL,
	[TMSAVEREP] [decimal](13, 2) NULL,
	[TMSSALDOP] [decimal](13, 2) NULL,
	[TMSPDARE] [decimal](13, 2) NULL,
	[TMSPAVERE] [decimal](13, 2) NULL,
	[TMSPSALDO] [decimal](13, 2) NULL,
	[TMSPDAREP] [decimal](13, 2) NULL,
	[TMSPAVEREP] [decimal](13, 2) NULL,
	[TMSPSALDOP] [decimal](13, 2) NULL,
	[TMSMASTRO] [varchar](2) NULL,
	[TMSCAUS] [smallint] NULL,
	[TMSDEMAS] [varchar](60) NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRSOLE24ORE2ANNI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[CRSOLE24ORE2ANNI]
AS
SELECT PIACODCO,PIAANACO,PIAFL11,CEEDESC,CEEALT,CEEST ,(SUM(TMSSALDO)+ SUM(TMSSALDOP))as SALDO,
RIGO = CASE WHEN (SUM(TMSSALDO)+SUM(TMSSALDOP)) < 0 AND CEEALT > 0 THEN CEEALT ELSE PIAFL11 END,TMSBLOCK 
FROM TBPIA
INNER JOIN TBCEE ON PIAFL11 = CEECOD
INNER JOIN TMPANNS on PIACODCO = TMSCONTO
GROUP BY TMSBLOCK,PIACODCO,PIAANACO,PIAFL11,CEEDESC,CEEALT,CEEST
HAVING (SUM(TMSSALDO)+SUM(TMSSALDOP))<>0 
UNION ALL
SELECT PIACODCO,PIAANACO,PIAFL11,CEEDESC,CEEALT,CEEST ,(SUM(TMSPSALDO)+ SUM(TMSPSALDOP)),
RIGO = CASE WHEN (SUM(TMSSALDO)+SUM(TMSSALDOP)) < 0 AND CEEALT > 0 THEN CEEALT ELSE PIAFL11 END,TMSBLOCK 
FROM TBPIA
INNER JOIN TBCEE ON PIAFL11 = CEECOD
INNER JOIN TMPANNS on PIACODCO = TMSCONTO
GROUP BY TMSBLOCK,PIACODCO,PIAANACO,PIAFL11,CEEDESC,CEEALT,CEEST
HAVING (SUM(TMSPSALDO)+SUM(TMSPSALDOP))<>0




GO
/****** Object:  View [dbo].[VCspCpt]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[VCspCpt]
as
select *,CptCespiti=isnull((select PiaAnaCo from tbPia where CspCespiti=PiaCodCo),'')
	,CptFondoAmm=isnull((select PiaAnaCo from tbPia where CspFondoAmm=PiaCodCo),'')
	,CptQuotaNormale=isnull((select PiaAnaCo from tbPia where CspQuotaNormale=PiaCodCo),'')
	,CptQuotaAnticipata=isnull((select PiaAnaCo from tbPia where CspQuotaAnticipata=PiaCodCo),'')
from tbcsp


GO
/****** Object:  View [dbo].[RH4]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[RH4]
AS
select DISTINCT TOP 100 PERCENT  priid,PriProg,PridataGio,pricausale,PriCodare, 
DAREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoDare ),'***ERRATO***'))),PriCoAvere,
AVEREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoAvere ),'***ERRATO***'))),
IMPORTO = case when PriCoDare = '00.10' then PriImpAvere else PriImpDare end,
PriImpDare,PriImpAvere,priDesc,priGstampa,PriCodPag,PriIvaPrint,
PriNumProt,PriBisRet,PriCodIva,PriRegIva,PriDocEst,PriDataEst,PriDescB,PriNsRif,PriArtFisc,PriDocAnn
from TRPRI 
INNER JOIN TRPRK 
ON PRIID = PRKID
WHERE PriCausale > 3 AND PRIREGIVA = 0
ORDER BY PriNumProt,PRIID,PRIPROG


GO
/****** Object:  Table [dbo].[TbArtP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbArtP](
	[ArtPId] [int] NOT NULL,
	[ArtPSigla] [varchar](30) NOT NULL,
	[ArtPProg] [smallint] NOT NULL,
	[ArtPCausale] [smallint] NOT NULL,
	[ArtPDare] [varchar](5) NOT NULL,
	[ArtPAvere] [varchar](5) NOT NULL,
	[ArtPDesc1] [varchar](24) NOT NULL,
	[ArtPDesc2] [varchar](32) NOT NULL,
 CONSTRAINT [PK_TbArtP] PRIMARY KEY CLUSTERED 
(
	[ArtPId] ASC,
	[ArtPProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VArtP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VArtP]
AS
SELECT ArtPId, ArtPSigla, ArtPProg, ArtPCausale, CiiCau, ArtPDare,
DescDare = case when ArtPDare = 'F' then 'Fornitori'
		when ArtPDare = 'C' then 'Clienti'
		when ArtPDare = 'S' then 'Sottoconto'
		when not substring(ArtPDare,3,1) = '.' then (SELECT AnaDesc FROM vdox.dbo.TbAna WHERE vdox.dbo.TbAna.AnaCod = ArtPDare)
		else (SELECT PiaAnaCo FROM TbPia WHERE PiaCodCo = ArtPDare) 
	   end, ArtPAvere,
DescAvere = case when ArtPAvere = 'F' then 'Fornitori'
		 when ArtPAvere = 'C' then 'Clienti'
		 when ArtPAvere = 'S' then 'Sottoconto'
		 when not substring(ArtPAvere,3,1) = '.' then (SELECT AnaDesc FROM vdox.dbo.TbAna WHERE vdox.dbo.TbAna.AnaCod = ArtPAvere)
		 else (SELECT PiaAnaCo FROM TbPia WHERE PiaCodCo = ArtPAvere) 
	   end, ArtPDesc1, ArtPDesc2
FROM TbArtP 
INNER JOIN TbCii ON ArtPCausale = CiiCod
GO
/****** Object:  Table [dbo].[TbMenuGrup]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbMenuGrup](
	[MenuGrupId] [int] NOT NULL,
	[MenuGrupInd] [varchar](5) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VGrupMenu]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VGrupMenu]
AS
SELECT     VDOX.dbo.TbGrupLav.*, dbo.TbMenuGrup.MenuGrupInd AS Indice
FROM         VDOX.dbo.TbGrupLav LEFT OUTER JOIN
                      dbo.TbMenuGrup ON VDOX.dbo.TbGrupLav.GrupLavId = dbo.TbMenuGrup.MenuGrupId


GO
/****** Object:  View [dbo].[RH8]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[RH8]
AS
Select top 100 percent priartfisc,prkconto,prkTipoCo,
PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PRKCONTO AND ANAGRP = 'CL' OR ANACOD = PRKCONTO AND ANAGRP = 'FO') END,
pridatagio,pridataest,cau.CiiCau as Causale,PriNumProt,PriregIva,PriDocEst,
CPT = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end,
CPTDESC = ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end),
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'CL' OR ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'FO')), 
DARE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then 0 when pricausale = 2 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when prkda = 0 then priimpdare else 0 end),
AVERE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when pricausale = 2 then 0 when prkda = 0 then 0 else  priimpavere end),
pridesc+pridescb as descriz,PrkAammgg,
PIAFL = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE 0 END,
PRICAUSALE,MCPT = CASE WHEN PRICAUSALE = 3 THEN (SELECT CASE WHEN PRICODARE <> PRKCONTO THEN PRICODARE ELSE PRICOAVERE END FROM TRPRI AS Q WHERE Q.PRIID = PRKID AND Q.PRIPROG = (PRKPROG - 1) ) ELSE '*' END 
 from TRPRI 
inner join TRPRK on prkId = priid and PrkProg = priProg
left outer join tbcii as cau on pricausale = cau.ciicod
left outer join tbcii as cii on pricodiva = cii.ciicod
--order by prkTipoCo,PrkConto,PridataGio,PriregIva,PriNumProt


GO
/****** Object:  Table [dbo].[TbPag]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPag](
	[PagCod] [smallint] NOT NULL,
	[PagDesc] [varchar](40) NOT NULL,
	[PagTipo] [smallint] NOT NULL,
	[PagNrate] [smallint] NOT NULL,
	[PagTestR1] [smallint] NOT NULL,
	[PagTestS1] [smallint] NOT NULL,
	[PagTestGM] [smallint] NOT NULL,
	[PagMeseE1] [smallint] NOT NULL,
	[PagMeseE2] [smallint] NOT NULL,
	[PagGgmmE1] [smalldatetime] NOT NULL,
	[PagGgmmE2] [smalldatetime] NOT NULL,
	[PagSlitE1] [varchar](1) NULL,
	[PagSlitE2] [varchar](1) NULL,
	[PagRata1] [smallint] NULL,
	[PagData1] [smallint] NULL,
	[PagRata2] [smallint] NULL,
	[PagData2] [smallint] NULL,
	[PagRata3] [smallint] NULL,
	[PagData3] [smallint] NULL,
	[PagRata4] [smallint] NULL,
	[PagData4] [smallint] NULL,
	[PagRata5] [smallint] NULL,
	[PagData5] [smallint] NULL,
	[PagRata6] [smallint] NULL,
	[PagData6] [smallint] NULL,
	[PagRata7] [smallint] NULL,
	[PagData7] [smallint] NULL,
	[PagRata8] [smallint] NULL,
	[PagData8] [smallint] NULL,
	[PagRata9] [smallint] NULL,
	[PagData9] [smallint] NULL,
	[PagRata10] [smallint] NULL,
	[PagData10] [smallint] NULL,
	[PagRata11] [smallint] NULL,
	[PagData11] [smallint] NULL,
	[PagRata12] [smallint] NULL,
	[PagData12] [smallint] NULL,
	[PagRata13] [smallint] NULL,
	[PagData13] [smallint] NULL,
	[PagRata14] [smallint] NULL,
	[PagData14] [smallint] NULL,
	[PagRata15] [smallint] NULL,
	[PagData15] [smallint] NULL,
	[PagRata16] [smallint] NULL,
	[PagData16] [smallint] NULL,
	[PagRata17] [smallint] NULL,
	[PagData17] [smallint] NULL,
	[PagRata18] [smallint] NULL,
	[PagData18] [smallint] NULL,
	[PagRata19] [smallint] NULL,
	[PagData19] [smallint] NULL,
	[PagRata20] [smallint] NULL,
	[PagData20] [smallint] NULL,
	[PagRata21] [smallint] NULL,
	[PagData21] [smallint] NULL,
	[PagRata22] [smallint] NULL,
	[PagData22] [smallint] NULL,
	[PagRata23] [smallint] NULL,
	[PagData23] [smallint] NULL,
	[PagRata24] [smallint] NULL,
	[PagData24] [smallint] NULL,
	[PagCodFE] [varchar](5) NOT NULL,
 CONSTRAINT [PK_TbPag] PRIMARY KEY CLUSTERED 
(
	[PagCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRH7IVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[CRH7IVA]
AS
SELECT     TOP 100 PERCENT *, ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICODARE AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICODARE AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PRICODARE), '***ERRATO***'))) AS DAREDESC, ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PRICOAVERE), '***ERRATO***'))) AS AVEREDESC, YEAR(A.PriDataGio) AS ANNOIVA, 
                      dbo.TbCii.CiiDes AS DESIVA,
					  DescrPag = CASE WHEN A.PRICAUSALE < 3 THEN 
					  CAST((SELECT PAGDESC FROM TBPAG WHERE PAGCOD = (select  B.Pricodpag from TBPRI AS B WHERE A.PRIID=B.PRIID AND B.PRICAUSALE = 3)) AS VARCHAR) ELSE CAST(PRICODPAG AS VARCHAR) END
FROM         dbo.TbPri AS A LEFT OUTER JOIN
                      dbo.TbCii ON A.PriCodIva = dbo.TbCii.CiiCod
WHERE     (A.PriRegIva > 0)
ORDER BY ANNOIVA, A.PriRegIva, A.PriNumProt, A.PriId, A.PriProg


GO
/****** Object:  UserDefinedFunction [dbo].[FnContaRicevute]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FnContaRicevute] (@CLIE as varchar(5), @DSCA as smalldatetime, @DFAT as smalldatetime, @TIPO AS SMALLINT)
returns table
as
return

SELECT COUNT(*) as Quante FROM TbEff
WHERE  RicClie = @CLIE  AND RicDsca = @DSCA
AND datepart(yy,RicDfat) * 100 + datepart(mm,RicDfat) = datepart(yy,@DFAT) * 100 + datepart(mm,@DFAT) AND RicSeff = '2' AND RicTPag = @TIPO

GO
/****** Object:  View [dbo].[VRITNETTO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VRITNETTO]
AS
SELECT RITPROTFAT,RITDATAFAT,RITCODFOR,NETTO =((RitCompenso - RitRitenuta - RitPrevPerc) + (RitRimborsi+ritRivalsa+ritrimbconv+ritIva)) 
FROM tbrit

GO
/****** Object:  UserDefinedFunction [dbo].[fnFotoChIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[fnFotoChIva](@MAX smalldatetime,@FAL as smalldatetime)
returns table
as
return
SELECT top 100 percent RivaNReg,RivaTipo,RivaDesc,Max(PriNumProt) as ProtCar,Max(PridataGio) as DataCar,
(SELECT isnull(Max(PriNumProt),0) FROM tbPri
where  datepart(year,pridatagio) = datepart(year,@max) and  pridatagio <= @max  and PriIvaPrint = 1 AND RivaNReg = PriRegIva) as ProtSta,
(SELECT isnull(Max(PriDataGio),@MAX) FROM tbPri
where  datepart(year,pridatagio) = datepart(year,@max) and pridatagio <=@max  and PriIvaPrint = 1 AND RivaNReg = PriRegIva) as DataSta,RivaPrintIniziale,RivaCh,RivaInt
FROM TbRegIva
left outer join TbPri on RivaNReg = PriRegIva
where (RivaAnno =datepart(year,@MAX) and datepart(year,pridatagio)= datepart(year,@MAX) and  pridatagio <= @max and pridataest <=@FAL)
group by RivaNReg,RivaTipo,RivaDesc,RivaPrintIniziale,RivaCh,RivaInt
Order by RivaNreg

GO
/****** Object:  View [dbo].[CRE14]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRE14]
AS
SELECT     TOP 100 PERCENT dbo.TbRit.RitCodFor, dbo.TbRit.RitDataFat, dbo.TbRit.RitProtFat, dbo.TbRit.RitDataPag, dbo.TbRit.RitTributo, dbo.TbRit.RitSospesa, 
                      dbo.TbRit.RitCompenso, dbo.TbRit.RitPerc, dbo.TbRit.RitRitenuta, dbo.TbRit.RitContrInps, dbo.TbRit.RitRimborsi, dbo.TbRit.RitRivalsa, 
                      dbo.TbRit.RitRimbConv, dbo.TbRit.RitIva, dbo.TbRit.RitPrevPerc, dbo.TbRit.RitDataVer, dbo.TbRit.RitEsCc, dbo.TbRit.RitNumQB, 
                      dbo.TbRit.RitImpVersam, dbo.TbRit.RitPerAtDa, dbo.TbRit.RitPerAtAa, dbo.TbTrib.TribDesc, dbo.TbTrib.TribCaus, VDOX.dbo.TbAna.AnaDesc, 
                      MONTH(dbo.TbRit.RitDataPag) AS MESECOMP, YEAR(dbo.TbRit.RitDataPag) AS ANNOCOMP
FROM         dbo.TbRit LEFT OUTER JOIN
                      VDOX.dbo.TbAna ON dbo.TbRit.RitCodFor = VDOX.dbo.TbAna.AnaCod AND VDOX.dbo.TbAna.AnaGrp = 'FO' LEFT OUTER JOIN
                      dbo.TbTrib ON dbo.TbRit.RitTributo = dbo.TbTrib.TribCod
ORDER BY ANNOCOMP, MESECOMP, dbo.TbRit.RitTributo, VDOX.dbo.TbAna.AnaDesc
GO
/****** Object:  Table [dbo].[TbEnasarco]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEnasarco](
	[EnaCod] [smallint] NOT NULL,
	[EnaFinoA] [decimal](13, 2) NULL,
 CONSTRAINT [PK_TbEnasarco] PRIMARY KEY CLUSTERED 
(
	[EnaCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRCONAGE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRCONAGE]
AS
SELECT     TOP 100 PERCENT YEAR(dbo.TbRit.RitDataFat) AS ANNOENA, dbo.TbRit.RitCodFor, SUM(dbo.TbRit.RitCompenso) AS COMPENSO, 
                      VDOX.dbo.TbAna.AnaDesc AS RAGIONESOC, GEVE.dbo.TbFor.FoEnasarco, dbo.TbEnasarco.EnaFinoA
FROM         dbo.TbRit LEFT OUTER JOIN
                      VDOX.dbo.TbAna ON dbo.TbRit.RitCodFor = VDOX.dbo.TbAna.AnaCod INNER JOIN
                      GEVE.dbo.TbFor ON dbo.TbRit.RitCodFor = GEVE.dbo.TbFor.FoCod INNER JOIN
                      dbo.TbEnasarco ON GEVE.dbo.TbFor.FoEnasarco = dbo.TbEnasarco.EnaCod
GROUP BY YEAR(dbo.TbRit.RitDataFat), VDOX.dbo.TbAna.AnaDesc, dbo.TbRit.RitCodFor, GEVE.dbo.TbFor.FoEnasarco, dbo.TbEnasarco.EnaFinoA
ORDER BY ANNOENA, VDOX.dbo.TbAna.AnaDesc

GO
/****** Object:  View [dbo].[CRBILCONF]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRBILCONF]
AS
select distinct TOP 100 PERCENT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,
sum(TMSDARE) as TMSDARE,SUM(TMSAVERE) AS TMSAVERE,SUM(TMSSALDO) AS TMSSALDO,
SUM(TMSDAREP) AS TMSDAREP,SUM(TMSAVEREP) AS TMSAVEREP,SUM(TMSSALDOP) AS TMSSALDOP,
sum(TMSPDARE) as TMSPDARE,SUM(TMSPAVERE) AS TMSPAVERE,SUM(TMSPSALDO) AS TMSPSALDO,
SUM(TMSPDAREP) AS TMSPDAREP,SUM(TMSPAVEREP) AS TMSPAVEREP,SUM(TMSPSALDOP) AS TMSPSALDOP,
TMSMASTRO,TMSSPPP = CASE WHEN TMSFLAG <> 6 AND TMSFLAG <> 7 then 0 else 1 end,TMSDEMAS
FROM TMPANNS
where SUBSTRING(TMSCONTO,4,2) <> '00'
group by TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSMASTRO,TMSDEMAS
ORDER BY TMSSPPP,TMSMASTRO,TMSCONTO

GO
/****** Object:  View [dbo].[CRSKENASARCO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRSKENASARCO]
AS
SELECT     TOP 100 PERCENT YEAR(dbo.TbRit.RitDataFat) AS ANNOENA, dbo.TbRit.RitCodFor, dbo.TbRit.RitDataFat, dbo.TbRit.RitProtFat, dbo.TbRit.RitCompenso, 
                      dbo.TbRit.RitRitenuta, dbo.TbRit.RitPrevPerc, dbo.TbRit.RitRimborsi, dbo.TbRit.RitRivalsa, dbo.TbRit.RitIva, dbo.TbRit.RitContrInps, 
                      VDOX.dbo.TbAna.AnaDesc AS RAGIONESOC, GEVE.dbo.TbFor.FoEnasarco, dbo.TbEnasarco.EnaFinoA, VDOX.dbo.TbAna.AnaPiva, 
                      VDOX.dbo.TbAna.AnaCfis, VDOX.dbo.TbAna.AnaIndirizzo, VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, 
                      VDOX.dbo.TbAna.AnaRag1, VDOX.dbo.TbAna.AnaRag2
FROM         dbo.TbRit LEFT OUTER JOIN
                      VDOX.dbo.TbAna ON dbo.TbRit.RitCodFor = VDOX.dbo.TbAna.AnaCod INNER JOIN
                      GEVE.dbo.TbFor ON dbo.TbRit.RitCodFor = GEVE.dbo.TbFor.FoCod INNER JOIN
                      dbo.TbEnasarco ON GEVE.dbo.TbFor.FoEnasarco = dbo.TbEnasarco.EnaCod
ORDER BY ANNOENA, VDOX.dbo.TbAna.AnaDesc, dbo.TbRit.RitProtFat

GO
/****** Object:  Table [dbo].[TbCod_Fe]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCod_Fe](
	[TIPO] [varchar](1) NOT NULL,
	[COD] [varchar](6) NOT NULL,
	[DESCRIZIONE] [varchar](150) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSca]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSca](
	[ScaId] [int] IDENTITY(1,1) NOT NULL,
	[ScaTipoCo] [varchar](1) NOT NULL,
	[ScaConto] [varchar](5) NOT NULL,
	[ScaNdoc] [int] NOT NULL,
	[ScaImpDoc] [decimal](13, 2) NOT NULL,
	[ScaIvaSpe] [decimal](13, 2) NOT NULL,
	[ScaAbi] [int] NOT NULL,
	[ScaCab] [int] NOT NULL,
	[ScaImpRata] [decimal](13, 2) NOT NULL,
	[ScaTPag] [smallint] NOT NULL,
	[ScaCodPag] [smallint] NOT NULL,
	[ScaNRata] [smallint] NOT NULL,
	[ScaBan] [smallint] NOT NULL,
	[ScaDdoc] [smalldatetime] NOT NULL,
	[ScaDsca] [smalldatetime] NOT NULL,
	[ScaRifId] [int] NOT NULL,
	[ScaRifProg] [smallint] NOT NULL,
	[ScaRifDA] [varchar](1) NOT NULL,
	[ScaRAperta] [varchar](1) NOT NULL,
	[ScaImpPagato] [decimal](13, 2) NOT NULL,
 CONSTRAINT [PK_TbSca] PRIMARY KEY CLUSTERED 
(
	[ScaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRG2]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE   VIEW [dbo].[CRG2]
AS

SELECT CONTO=ScaConto,NDOC=ScaNdoc,DATADOC=ScaDdoc,TOTALEDOC= ScaImpDoc,ACCONTI=SUM(SCAIMPPAGATO),RESIDUO= (ScaImpDoc -SUM(SCAIMPPAGATO)) from tbsca --where ScaConto='50178' and ScaNdoc=351 and ScaDdoc='31/01/2025'
GROUP BY ScaConto,ScaNdoc,ScaDdoc,ScaImpDoc
GO
/****** Object:  Table [dbo].[TbAbi]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAbi](
	[AbAbi] [int] NOT NULL,
	[AbBanca] [varchar](60) NOT NULL,
	[AbStato] [varchar](1) NULL,
	[AbSabi] [varchar](5) NULL,
 CONSTRAINT [PK_TbAbi] PRIMARY KEY CLUSTERED 
(
	[AbAbi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCab]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCab](
	[CaAbi] [int] NOT NULL,
	[CaCab] [int] NOT NULL,
	[CaIndi] [varchar](30) NOT NULL,
	[CaCap] [varchar](5) NOT NULL,
	[CaCitta] [varchar](25) NOT NULL,
	[CaProv] [varchar](2) NOT NULL,
	[CaFia] [varchar](40) NOT NULL,
	[CaFib] [varchar](40) NOT NULL,
	[CaFic] [varchar](40) NOT NULL,
	[CaDescFt] [varchar](40) NOT NULL,
	[CaPaese] [varchar](2) NOT NULL,
	[CaBic] [varchar](15) NOT NULL,
 CONSTRAINT [PK_TbCab] PRIMARY KEY CLUSTERED 
(
	[CaAbi] ASC,
	[CaCab] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbBan]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbBan](
	[BanCod] [smallint] NOT NULL,
	[BanAbi] [int] NOT NULL,
	[BanCab] [int] NOT NULL,
	[BanDes] [varchar](40) NOT NULL,
	[BanCc] [varchar](12) NOT NULL,
	[BanSia] [varchar](5) NOT NULL,
	[BanFir] [varchar](20) NOT NULL,
	[BanRb] [varchar](5) NOT NULL,
	[BanPaese] [varchar](2) NULL,
	[BanCinEur] [varchar](2) NULL,
	[BanCin] [varchar](1) NULL,
	[banBic] [varchar](20) NULL,
	[BanAttivo] [bit] NULL,
	[BanCreditorId] [varchar](23) NULL,
 CONSTRAINT [PK_TbBan] PRIMARY KEY CLUSTERED 
(
	[BanCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRG1]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[CRG1]
AS
SELECT top 100 percent ScaTipoCo,a.ScaConto,SCADESC=ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.ScaConto AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.ScaConto AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = a.ScaConto ),'***ERRATO***'))),
a.ScaNdoc,ScaImpDoc,ScaIvaSpe,ScaAbi,ScaCab,ISNULL(ABBANCA,' ')as ABBANCA,ISNULL(CAFIA,' ') as CAFIA,ScaImpRata,ScaTPag,ScaCodPag,
ScaNRata,ScaBan,ISNULL(BANDES,' ')as BANDES,a.ScaDdoc,ScaDSca,ScaRifId,ScaRifProg,ScaRifDA,PagDesc= CAST(ScaCodPag AS varchar(3)) + ' ' + PagDesc,ScaId,PrkPaperta,ScaRAperta,
ScaImpPagato,CLFOPI = CASE WHEN ScaTipoCo = '0' THEN 'SO' 
ELSE ISNULL((SELECT ANAGRP FROM VDOX.DBO.TBANA WHERE ANACOD = a.ScaConto) ,'F0') END,
TIPA = case when ScaTPag = 1 then 'TRATTA' when ScaTPag = 2 then ' R.B.' when ScaTPag = 3 then ' R.D.'
when ScaTPag = 4 then 'CONTANTI' when ScaTPag = 5 then 'BONIFICO' when ScaTPag = 6 then 'C/ASS'
when ScaTPag = 7 then 'R.I.D' when ScaTPag = 8 then 'C.C.P' ELSE ' ' END,(ScaImpRata - ScaImpPagato) as ScaScopRata,FatPagCodFE=COD + ' ' + DESCRIZIONE,
TOTALEDOC,ACCONTI,RESIDUO,SD = CAST('0' AS varchar(1))
FROM TbSca as a
INNER JOIN CRG2 as b on a.ScaConto=CONTO and a.ScaNdoc=NDOC and a.ScaDdoc=DATADOC and a.ScaImpDoc=TOTALEDOC
LEFT OUTER JOIN TBPAG ON a.ScaCodPag = PagCod
LEFT OUTER JOIN TBBAN ON a.ScaBAN = BANCOD
LEFT OUTER JOIN TBABI ON a.ScaAbi = AbAbi
LEFT OUTER JOIN TBCAB ON a.ScaAbi = CAAbi AND SCACAB = CACAB
LEFT OUTER JOIN TBPRK on a.SCARIFID = PRKID AND a.SCARIFPROG = PRKPROG AND a.SCARIFDA = PRKDA 
LEFT OUTER JOIN TbCod_FE on PagCodFE = COD and TIPO='P'
--WHERE a.ScaConto=50199
GO
/****** Object:  View [dbo].[CRH7PN]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[CRH7PN]
AS
SELECT     TOP 100 PERCENT *, ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICODARE AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICODARE AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PRICODARE), '***ERRATO***'))) AS DAREDESC, ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PRICOAVERE), '***ERRATO***'))) AS AVEREDESC, dbo.TbCii.CiiCau AS Expr1
FROM         dbo.TbPri INNER JOIN
                      dbo.TbCii ON dbo.TbCii.CiiCod = dbo.TbPri.PriCausale
WHERE     (dbo.TbPri.PriRegIva = 0)
ORDER BY dbo.TbPri.PriNumProt, dbo.TbPri.PriId, dbo.TbPri.PriProg


GO
/****** Object:  Table [dbo].[TMPIVAP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPIVAP](
	[TivaPid] [int] NOT NULL,
	[TIvaPAnno] [smallint] NOT NULL,
	[TIvaPRegIva] [smallint] NOT NULL,
	[TIvaPMese] [smallint] NOT NULL,
	[TIvaPCodIva] [smallint] NOT NULL,
	[TIvaPImpon] [decimal](13, 2) NOT NULL,
	[TIvaPIvaDE] [decimal](13, 2) NOT NULL,
	[TIvaPIvaND] [decimal](13, 2) NOT NULL,
	[TIvaPImpMerce] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[crriepiva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[crriepiva]
as
SELECT DISTINCT 
                      dbo.TMPIVAP.TIvaPAnno, dbo.TMPIVAP.TIvaPMese,dbo.TMPIVAP.TIvaPRegIva, dbo.TMPIVAP.TIvaPCodIva, SUM(dbo.TMPIVAP.TIvaPImpon) AS Timpon,
                       SUM(dbo.TMPIVAP.TIvaPIvaDE) AS IvaDe, SUM(dbo.TMPIVAP.TIvaPIvaND) AS IvaNd, SUM(dbo.TMPIVAP.TIvaPImpMerce) AS Merce, dbo.TbCii.CiiDes, 
                      dbo.TMPIVAP.TivaPid
FROM         dbo.TMPIVAP LEFT OUTER JOIN
                      dbo.TbCii ON dbo.TMPIVAP.TIvaPCodIva = dbo.TbCii.CiiCod
GROUP BY dbo.TMPIVAP.TIvaPAnno,  dbo.TMPIVAP.TIvaPMese,dbo.TMPIVAP.TIvaPRegIva, dbo.TMPIVAP.TIvaPCodIva, dbo.TbCii.CiiDes, dbo.TMPIVAP.TivaPid

GO
/****** Object:  View [dbo].[VH3]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VH3]
AS
select DISTINCT TOP 100 PERCENT  priid,PriProg,PridataGio,pricausale,PriCodare as CONTO,
DAREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoDare ),'***ERRATO***'))),
PriCoAvere as CPT,
AVEREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoAvere ),'***ERRATO***'))),
PriImpDare,PriImpAvere,priDesc,priGstampa,PriCodPag,PriIvaPrint,
PriNumProt,PriBisRet,PriCodIva,isnull(CiiDes,'') as CiiDes,Isnull(CiiAli,0) as CiiAli,PriRegIva,PriDocEst,PriDataEst,PriDescB,PriNsRif,PriArtFisc,
TIPOREG = isnull((select RIvaTipo from TbRegIva where PriRegIva = RIvaNReg and datePart(year,pridatagio) = RIvaAnno),0),PriDocAnn
from TBPRI 
INNER JOIN TBPRK 
ON PRIID = PRKID
left outer join tbcii as cii on pricodiva = cii.ciicod
ORDER BY PRIregIva,PriNumProt,PriBisRet,PRIID,PRIPROG

GO
/****** Object:  View [dbo].[VDISTRBINS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE VIEW [dbo].[VDISTRBINS]
AS
SELECT     TOP 100 PERCENT RicDsca AS SCADENZA, RicClie AS CLIENTE, AnaDesc AS EMITTENTE, 
                      AnaCitta AS PIAZZA, RicImpRata AS IMPRATA, RicImpFatt AS IMPFATT, 'E' AS ESPOS, 
					                        'FATT. N.' + CAST(RicNfat AS varchar) + ' DEL ' + CONVERT(char, RicDfat, 103) AS DOCUM, RicAbi AS ABI, 
											                      RicCab AS CAB,RicProg,RicTPag,RicBan,
																					  RicNfat,RicDfat,RicNRata,ANNO=datepart(year,RicDfat),PrkPAperta=cast(0 as bit)
																					  FROM         TbEff INNER JOIN
																					                        Vdox.dbo.TbAna ON RicClie = AnaCod and anagrp='CL'
																											WHERE     (RicSeff = '3' and RicTpag =2)
																											ORDER BY AnaDesc,RicDsca






GO
/****** Object:  View [dbo].[wwwVH8]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wwwVH8]
AS
Select top 100 percent priartfisc,prkconto,prkTipoCo,
PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PRKCONTO AND ANAGRP = 'CL' OR ANACOD = PRKCONTO AND ANAGRP = 'FO') END,
pridatagio,pridataest,cau.CiiCau as Causale,PriNumProt,PriregIva,PriDocEst,
CPT = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end,
CPTDESC = ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end),
(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'CL' OR ANACOD = Case when PriCausale = 1 then PriCoDare when pricausale = 2 then PriCoAvere when prkda = 0 then priCoAvere else pricodare end AND ANAGRP = 'FO')), 
DARE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then 0 when pricausale = 2 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when prkda = 0 then priimpdare else 0 end),
AVERE = CONVERT(DECIMAL(13,2),case when PriCausale = 1 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) when pricausale = 2 then 0 when prkda = 0 then 0 else  priimpavere end),
pridesc+pridescb as descriz,PrkAammgg,
PIAFL = CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE 0 END,
PRICAUSALE,PriSos,MDESC = CASE WHEN Prisos <>'' then ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = Prisos),'') ELSE '' END 
 from Tbpri 
inner join Tbprk on prkId = priid and PrkProg = priProg
left outer join tbcii as cau on pricausale = cau.ciicod
left outer join tbcii as cii on pricodiva = cii.ciicod


GO
/****** Object:  Table [dbo].[TMPPROQUO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPPROQUO](
	[ProQuoNum] [int] NOT NULL,
	[ProQuoCat] [varchar](2) NOT NULL,
	[ProQuoAnnoA] [smallint] NOT NULL,
	[ProQuoAliTab] [decimal](5, 2) NOT NULL,
	[ProQuoAliFis] [decimal](5, 2) NOT NULL,
	[ProQuoAliLib] [decimal](5, 2) NOT NULL,
	[ProContoQuo] [varchar](5) NOT NULL,
	[ProContoFondo] [varchar](5) NOT NULL,
	[ProQuoCoAmm] [decimal](13, 2) NOT NULL,
	[ProQuoAmmortam] [decimal](13, 2) NOT NULL,
	[ProQuoAnticip] [decimal](13, 2) NOT NULL,
	[ProQuoMeta] [decimal](13, 2) NOT NULL,
	[ProQuoAmmLib] [decimal](13, 2) NOT NULL,
	[ProQuoDescr] [varchar](110) NOT NULL,
	[ProQuoFondo] [decimal](13, 2) NOT NULL,
 CONSTRAINT [PK_TMPPROQUO] PRIMARY KEY CLUSTERED 
(
	[ProQuoNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRPROGQUO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[CRPROGQUO]
AS
SELECT     TOP 100 PERCENT *
FROM         dbo.TMPPROQUO left outer JOIN
                      dbo.VCespCat ON dbo.VCespCat.CespNum = dbo.TMPPROQUO.ProQuoNum left outer  JOIN
                      dbo.TbQuo ON dbo.TbQuo.QuoNum = dbo.VCespCat.CespNum






GO
/****** Object:  View [dbo].[VINCCOR]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[VINCCOR]
AS
SELECT distinct PriDataGio,PriNumProt,sum(PriImpDare) as IncassoLordo,PriRegIva,PriId 
FROM TBPRI 
group by PriDataGio,PriNumProt,PriregIva,PriId 


GO
/****** Object:  View [dbo].[VCiiIvaP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[VCiiIvaP]
as
select IvaPAnno,IvaPRegIva,IvaPmese,IvaPCodIva,CiiDes,IvaPImpon,IvaPIvaDE,IvaPIvaND,IvaPImpMerce
from tbivap inner join tbcii on ciicod=ivapcodiva

GO
/****** Object:  View [dbo].[vistacespiti]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vistacespiti]
AS
SELECT     TOP 100 PERCENT dbo.TbCesp.CespNum AS Expr1, dbo.TbCesp.CespAnnoA AS Expr2, dbo.TbCesp.CespCat AS Expr3, 
                      dbo.TbCesp.CespContoStorico AS Expr4, dbo.TbCesp.CespDataCessione AS Expr5, dbo.TbQuo.QuoAnno, dbo.TbQuo.QuoCoAmmIni, 
                      dbo.TbQuo.QuoFondoIni, dbo.TbQuo.QuoResiduoIni, dbo.TbQuo.QuoNoDetraIni, dbo.TbQuo.QuoCoStor, dbo.TbQuo.QuoCoAmm, dbo.TbQuo.QuoQuota, 
                      dbo.TbQuo.QuoFondo, dbo.TbQuo.QuoResiduo, dbo.TbQuo.QuoNoDetra
FROM         dbo.TbCesp LEFT OUTER JOIN
                      dbo.TbQuo ON dbo.TbCesp.CespNum = dbo.TbQuo.QuoNum
WHERE     (dbo.TbCesp.CespAnnoA < 2004) AND (dbo.TbCesp.CespDataCessione < CONVERT(DATETIME, '2004-01-01 00:00:00', 102) AND 
                      dbo.TbCesp.CespDataCessione > CONVERT(DATETIME, '2003-01-01 00:00:00', 102)) OR
                      (dbo.TbCesp.CespDataCessione IS NULL)
ORDER BY dbo.TbCesp.CespCat

GO
/****** Object:  View [dbo].[VRIEPIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VRIEPIVA]
AS
SELECT DISTINCT TOP 100 PERCENT 
                      dbo.TBIVAP.IvaPAnno, dbo.TBIVAP.IvaPMese, dbo.TBIVAP.IvaPRegIva, dbo.TBIVAP.IvaPCodIva, dbo.TBIVAP.IvaPImpon AS Timpon,
                       dbo.TBIVAP.IvaPIvaDE AS IvaDe, dbo.TBIVAP.IvaPIvaND AS IvaNd, dbo.TBIVAP.IvaPImpMerce AS Merce, dbo.TbCii.CiiDes,dbo.TbCii.CiiAli
FROM         dbo.TBIVAP LEFT OUTER JOIN
                      dbo.TbCii ON dbo.TBIVAP.IvaPCodIva = dbo.TbCii.CiiCod
ORDER BY dbo.TBIVAP.IvaPAnno, dbo.TBIVAP.IvaPMese, dbo.TBIVAP.IvaPRegIva, dbo.TBIVAP.IvaPCodIva, dbo.TbCii.CiiDes,dbo.TbCii.CiiAli

GO
/****** Object:  Table [dbo].[TMPIVAS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPIVAS](
	[IvaPAnno] [smallint] NOT NULL,
	[IvaPRegIva] [smallint] NOT NULL,
	[IvaPMese] [smallint] NOT NULL,
	[IvaPCodIva] [smallint] NOT NULL,
	[IvaPImpon] [decimal](13, 2) NOT NULL,
	[IvaPIvaDE] [decimal](13, 2) NOT NULL,
	[IvaPIvaND] [decimal](13, 2) NOT NULL,
	[IvaPImpMerce] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[wwwVTMPRIEPIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wwwVTMPRIEPIVA]
AS
SELECT DISTINCT TOP 100 PERCENT 
                      dbo.TMPIVAS.IvaPAnno, dbo.TMPIVAS.IvaPMese, dbo.TMPIVAS.IvaPRegIva, dbo.TMPIVAS.IvaPCodIva, dbo.TMPIVAS.IvaPImpon AS Timpon,
                       dbo.TMPIVAS.IvaPIvaDE AS IvaDe, dbo.TMPIVAS.IvaPIvaND AS IvaNd, dbo.TMPIVAS.IvaPImpMerce AS Merce, dbo.TbCii.CiiDes,dbo.TbCii.CiiAli
FROM         dbo.TMPIVAS LEFT OUTER JOIN
                      dbo.TbCii ON dbo.TMPIVAS.IvaPCodIva = dbo.TbCii.CiiCod
ORDER BY dbo.TMPIVAS.IvaPAnno, dbo.TMPIVAS.IvaPMese, dbo.TMPIVAS.IvaPRegIva, dbo.TMPIVAS.IvaPCodIva, dbo.TbCii.CiiDes,dbo.TbCii.CiiAli
GO
/****** Object:  View [dbo].[VRitTrib]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VRitTrib]
AS
SELECT     TOP 100 PERCENT dbo.TbRit.RitNum, dbo.TbRit.RitCodFor, dbo.TbRit.RitDataFat, dbo.TbRit.RitProtFat, dbo.TbRit.RitDataPag, dbo.TbRit.RitTributo, 
                      dbo.TbRit.RitCompenso, dbo.TbRit.RitPerc, dbo.TbRit.RitRitenuta, dbo.TbRit.RitContrInps, dbo.TbRit.RitRimborsi, dbo.TbRit.RitRivalsa, 
                      dbo.TbRit.RitRimbConv, dbo.TbRit.RitIva, dbo.TbRit.RitDataVer, dbo.TbRit.RitEsCc, dbo.TbRit.RitNumQB, dbo.TbRit.RitImpVersam, 
                      dbo.TbRit.RitPrevPerc, dbo.TbRit.RitLetCont10, dbo.TbRit.RitSospesa, dbo.TbRit.RitPerAtDa, dbo.TbRit.RitPerAtAa, dbo.TbTrib.TribDesc AS TribDesc, 
                      dbo.TbTrib.TribRit AS TribRit, vdox.dbo.TbAna.AnaDesc AS AnaDesc
FROM         dbo.TbTrib RIGHT OUTER JOIN
                      dbo.TbRit ON dbo.TbTrib.TribCod = dbo.TbRit.RitTributo LEFT OUTER JOIN
                      vdox.dbo.TbAna ON vdox.dbo.TbAna.AnaCod = dbo.TbRit.RitCodFor
ORDER BY dbo.TbRit.RitCodFor, dbo.TbRit.RitDataFat DESC

GO
/****** Object:  View [dbo].[VTotRit]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[VTotRit]
as
select distinct rittributo,tribdesc,sum(ritcompenso)as ritcompenso,sum(ritritenuta)as ritritenuta from tbrit
inner join tbtrib on rittributo=tribcod
group by rittributo,tribdesc

GO
/****** Object:  View [dbo].[VVERSIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VVERSIVA]
AS
SELECT DISTINCT 
                      dbo.TBIVAP.IvaPAnno, dbo.TBIVAP.IvaPMese, dbo.TBIVAP.IvaPRegIva, dbo.TBIVAP.IvaPCodIva, SUM(dbo.TBIVAP.IvaPImpon) AS Timpon,
                       SUM(dbo.TBIVAP.IvaPIvaDE) AS IvaDe, SUM(dbo.TBIVAP.IvaPIvaND) AS IvaNd, SUM(dbo.TBIVAP.IvaPImpMerce) AS Merce, dbo.TbCii.CiiDes
FROM         dbo.TBIVAP LEFT OUTER JOIN
                      dbo.TbCii ON dbo.TBIVAP.IvaPCodIva = dbo.TbCii.CiiCod
GROUP BY dbo.TBIVAP.IvaPAnno, dbo.TBIVAP.IvaPMese, dbo.TBIVAP.IvaPRegIva, dbo.TBIVAP.IvaPCodIva, dbo.TbCii.CiiDes

GO
/****** Object:  View [dbo].[VXriepIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VXriepIva]
as
SELECT DISTINCT 
                      dbo.TMPIVAP.TIvaPAnno,  dbo.TMPIVAP.TIvaPRegIva, dbo.TMPIVAP.TIvaPCodIva, SUM(dbo.TMPIVAP.TIvaPImpon) AS Timpon,
                       SUM(dbo.TMPIVAP.TIvaPIvaDE) AS IvaDe, SUM(dbo.TMPIVAP.TIvaPIvaND) AS IvaNd, SUM(dbo.TMPIVAP.TIvaPImpMerce) AS Merce, dbo.TbCii.CiiDes, 
                      dbo.TMPIVAP.TivaPid,dbo.TbCii.CiiAli
FROM         dbo.TMPIVAP LEFT OUTER JOIN
                      dbo.TbCii ON dbo.TMPIVAP.TIvaPCodIva = dbo.TbCii.CiiCod
GROUP BY dbo.TMPIVAP.TIvaPAnno, dbo.TMPIVAP.TIvaPRegIva, dbo.TMPIVAP.TIvaPCodIva, dbo.TbCii.CiiDes, dbo.TbCii.CiiAli,dbo.TMPIVAP.TivaPid
GO
/****** Object:  Table [dbo].[TbEleCf]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEleCf](
	[EleCfAnno] [smallint] NOT NULL,
	[EleCfTipo] [nvarchar](2) NOT NULL,
	[EleCfCodice] [nvarchar](5) NOT NULL,
	[EleCfAnaDesc] [nvarchar](60) NOT NULL,
	[EleCfCpt] [nvarchar](5) NOT NULL,
	[EleCfPiaDesc] [nvarchar](35) NOT NULL,
	[EleCfDataDoc] [smalldatetime] NOT NULL,
	[EleCfNumDoc] [int] NOT NULL,
	[EleCfImponibile] [decimal](13, 2) NOT NULL,
	[EleCfIva] [decimal](13, 2) NOT NULL,
	[EleCfCodiceIva] [smallint] NOT NULL,
	[EleCfDescIva] [nvarchar](15) NOT NULL,
	[EleCfPriCodIva] [smallint] NOT NULL,
	[EleCfPriId] [int] NOT NULL,
	[EleCfPriProg] [int] NOT NULL,
	[EleCfP] [smallint] NOT NULL,
	[EleCfPriRegIva] [smallint] NOT NULL,
	[EleCfPerc] [decimal](5, 2) NOT NULL,
	[EleCfTipImp] [smallint] NOT NULL,
	[EleCfUnion] [int] NOT NULL,
	[EleCfRegistrazione] [smalldatetime] NOT NULL,
	[EleCfModPag] [varchar](1) NOT NULL,
	[EleCfVariazione] [bit] NOT NULL,
 CONSTRAINT [PK_TbEleCf] PRIMARY KEY CLUSTERED 
(
	[EleCfPriId] ASC,
	[EleCfPriProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[XVERIFICASPE2010]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[XVERIFICASPE2010]
AS
SELECT DISTINCT 
                      EleCfUnion AS UNIONE, EleCfPriId AS RECREG, EleCfAnno AS ANNO, EleCfTipo AS TIPO, EleCfCodice AS CODICE, EleCfAnaDesc AS DENOMINAZIONE, 
                      SUM(EleCfImponibile) AS IMPONIBILE, SUM(EleCfIva) AS IVA, EleCfDataDoc AS DATAREG, EleCfNumDoc AS NUMDOC, EleCfP AS FLAG, EleCfModPag as PAG, 
                      CASE WHEN (SUM(EleCfImponibile)) < 25000 AND EleCfModPag = 'N' THEN 'MINORE DI € 25.000!!!!' ELSE '' END AS ERRORE
FROM         dbo.TbEleCf
WHERE     (elecfanno = 2010 AND EleCfP > 1 AND EleCfUnion = 0)
GROUP BY EleCfPriId, EleCfAnno, EleCfTipo, EleCfCodice, EleCfAnaDesc, EleCfUnion, EleCfDataDoc, EleCfNumDoc, EleCfP,EleCfModPag
UNION
SELECT DISTINCT 
                      EleCfUnion AS UNIONE, 0 AS RECREG, EleCfAnno AS ANNO, EleCfTipo AS TIPO, EleCfCodice AS CODICE, EleCfAnaDesc AS DENOMINAZIONE, SUM(EleCfImponibile) 
                      AS IMPONIBILE, SUM(EleCfIva) AS IVA, '31/12/2010' AS DATAREG, 0 AS NUMDOC, EleCfP AS FLAG, EleCfModPag as PAG,
                      CASE WHEN (SUM(EleCfImponibile)) < 25000 AND EleCfModPag = 'N' THEN 'MINORE DI € 25.000!!!!' ELSE '' END AS ERRORE

FROM         dbo.TbEleCf
WHERE     (elecfanno = 2010 AND EleCfP > 1 AND EleCfUnion > 0)
GROUP BY EleCfUnion, EleCfAnno, EleCfTipo, EleCfCodice, EleCfAnaDesc, EleCfP,EleCfModPag

GO
/****** Object:  View [dbo].[VELEDA2010]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VELEDA2010]
AS
select EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice, EleCfAnaDesc,EleCfNumDoc, EleCfDataDoc,EleCfImponibile=SUM(EleCfImponibile) ,EleCfIva=sum(EleCfIva),Totale= SUM(EleCfImponibile) +  sum(EleCfIva), EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione from tbelecf
group by  EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc, EleCfNumDoc, EleCfDataDoc, EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione




GO
/****** Object:  Table [dbo].[TbVers]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbVers](
	[IvaVAnno] [smallint] NOT NULL,
	[IvaVMese] [smallint] NOT NULL,
	[IvaVVersam] [decimal](13, 2) NOT NULL,
	[IvaVLiquida] [decimal](13, 2) NOT NULL,
	[IvaVData] [smalldatetime] NULL,
	[IvaVAbi] [int] NOT NULL,
	[IvaVCab] [int] NOT NULL,
	[IvaVDelega] [bit] NOT NULL,
	[IvaVChiusura] [bit] NOT NULL,
	[IvaVCrImUt] [decimal](13, 2) NOT NULL,
	[IvaVInteressi] [decimal](13, 2) NOT NULL,
	[IvaVImpDaVers] [decimal](13, 2) NOT NULL,
	[IvaVVersato] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[DXVERSAM]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DXVERSAM]
AS
SELECT     TOP 100 PERCENT IvaVAnno, IvaVMese, IvaVVersam, IvaVLiquida, IvaVData, IvaVAbi, IvaVCab, IvaVDelega, IvaVChiusura, IvaVCrImUt, IvaVInteressi, IvaVImpDaVers, 
                      IvaVVersato, 
                      CASE WHEN IvaVMese = 0 THEN 'Anno Precedente' WHEN IvaVMese = 1 THEN 'Gennaio' WHEN IvaVMese = 2 THEN 'Febbraio' WHEN IvaVMese = 3 THEN 'Marzo' WHEN
                       IvaVMese = 4 THEN 'Aprile' WHEN IvaVMese = 5 THEN 'Maggio' WHEN IvaVMese = 6 THEN 'Giugno' WHEN IvaVMese = 7 THEN 'Luglio' WHEN IvaVMese = 8 THEN 'Agosto'
                       WHEN IvaVMese = 9 THEN 'Settembre' WHEN IvaVMese = 10 THEN 'Ottobre' WHEN IvaVMese = 11 THEN 'Novembre' WHEN IvaVMese = 12 THEN 'Dicembre' ELSE 'Acconto'
                       END AS MESE, CASE WHEN IvaVVersam > 0 THEN IvaVVersam ELSE 0 END AS PAGATO
FROM         dbo.TbVers
ORDER BY IvaVAnno, IvaVData, IvaVMese

GO
/****** Object:  Table [dbo].[TbDDiCl]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbDDiCl](
	[DDiCId] [int] IDENTITY(1,1) NOT NULL,
	[DDiCAnno] [smallint] NOT NULL,
	[DDiCNProgInterno] [smallint] NOT NULL,
	[DDiCNProgDichiar] [smallint] NOT NULL,
	[DDiCCli] [varchar](5) NOT NULL,
	[DDiCCodEse] [tinyint] NOT NULL,
	[DDiCOpDa3] [smalldatetime] NULL,
	[DDiCOpAa3] [smalldatetime] NULL,
	[DDiCDataDoc] [smalldatetime] NULL,
	[DDiCRicev] [smalldatetime] NULL,
	[DDiCRegBollo] [bit] NOT NULL,
	[DDiCUPag] [smallint] NOT NULL,
	[DDiImporto] [decimal](12, 2) NOT NULL,
	[DDiProtocollo] [varchar](24) NOT NULL,
 CONSTRAINT [PK_TbDDiCl] PRIMARY KEY CLUSTERED 
(
	[DDiCId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VDichCli]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE View [dbo].[VDichCli]
as
select *, Anagrafica = AnaDesc + ' ' + AnaIndirizzo + ' ' + Anacap + ' ' + AnaCitta + '(' + AnaProv + ') - P.I. ' + AnaPiva from TbDDiCl
inner join VDOX.dbo.TbAna on DDiCCli =AnaCod and AnaGrp = 'CL'
inner join TbCii on DDiCCodEse = CiiCod


GO
/****** Object:  View [dbo].[CRSOLE24ORE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRSOLE24ORE]
AS
SELECT PIACODCO,PIAANACO,PIAFL11,CEEDESC,CEEALT,CEEST ,(SUM(TMSSALDO)+ SUM(TMSSALDOP))as SALDO,
RIGO = CASE WHEN (SUM(TMSSALDO)+SUM(TMSSALDOP)) < 0 AND CEEALT > 0 THEN CEEALT ELSE PIAFL11 END,TMSBLOCK 
FROM TBPIA
INNER JOIN TBCEE ON PIAFL11 = CEECOD
INNER JOIN TMPBILS on PIACODCO = TMSCONTO
GROUP BY TMSBLOCK,PIACODCO,PIAANACO,PIAFL11,CEEDESC,CEEALT,CEEST
HAVING (SUM(TMSSALDO)+SUM(TMSSALDOP))<>0
GO
/****** Object:  View [dbo].[wwwVH4]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[wwwVH4]
AS
select DISTINCT TOP 100 PERCENT  priid,PriProg,PridataGio,pricausale,PriCodare, 
DAREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoDare ),'***ERRATO***'))),PriCoAvere,
AVEREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoAvere ),'***ERRATO***'))),
IMPORTO = case when PriCoDare = '00.10' then PriImpAvere else PriImpDare end,
PriImpDare,PriImpAvere,priDesc,priGstampa,PriCodPag,PriIvaPrint,
PriNumProt,PriBisRet,PriCodIva,PriRegIva,PriDocEst,PriDataEst,PriDescB,PriNsRif,PriArtFisc,PriDocAnn
from TBPRI 
INNER JOIN TBPRK 
ON PRIID = PRKID
WHERE PriCausale > 3 AND PRIREGIVA = 0
ORDER BY PriNumProt,PRIID,PRIPROG
GO
/****** Object:  View [dbo].[VSCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VSCADENZE]
AS
SELECT     TOP (100) PERCENT dbo.TbEff.RicProg AS RECORD, dbo.TbEff.RicDsca AS SCADENZA, dbo.TbEff.RicImpRata AS [IMPORTO RATA], dbo.TbEff.RicClie AS CODCLI, 
                      VDOX.dbo.TbAna.AnaDesc AS ANAGRAFICA, dbo.TbEff.RicNfat AS [NR FATTURA], dbo.TbEff.RicDfat AS [DATA FATTURA], dbo.TbEff.RicImpFatt AS [IMPORTO FATTURA], 
                      CASE WHEN dbo.TbEff.RicSeff = '2' THEN 'IN PORTAFOGLIO' WHEN dbo.TbEff.RicSeff = '3' THEN 'PRESENTATO' ELSE 'DA EMETTERE' END AS [STATO EFFETTO], 
                      dbo.TbEff.RicDDis AS [DATA OPER/DIST], dbo.TbEff.RicBan AS BANCA, 
                      CASE WHEN dbo.TbEff.RicSomma = 'S' THEN 'SOMMA' WHEN dbo.TbEff.RicSomma = 'R' THEN 'RIEPILOGO' ELSE 'EFFETTO' END AS ESPOS, 
                      dbo.TbEff.RicFlagCoge AS [IN COGE], dbo.TbEff.RicTPag AS CodPag, 
                      CASE WHEN dbo.TbEff.RicTPag = 2 THEN 'Ricevuta Bancaria' WHEN dbo.TbEff.RicTPag = 3 THEN 'Rimessa Diretta' ELSE 'R.i.d.' END AS TIPOPAG, 
                      CAST('01/' + REPLICATE('0', 2 - LEN(CAST(MONTH(dbo.TbEff.RicDsca) AS varchar))) + CAST(MONTH(dbo.TbEff.RicDsca) AS varchar) 
                      + '/' + CAST(YEAR(dbo.TbEff.RicDsca) AS varchar) AS smalldatetime) AS GrupScad,RicNumDis
FROM         dbo.TbEff INNER JOIN
                      VDOX.dbo.TbAna ON dbo.TbEff.RicClie = VDOX.dbo.TbAna.AnaCod
ORDER BY SCADENZA, ANAGRAFICA

GO
/****** Object:  Table [dbo].[TbIstat]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbIstat](
	[IstatCod] [varchar](5) NOT NULL,
	[IstatDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TbIstat] PRIMARY KEY CLUSTERED 
(
	[IstatCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbAteco]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAteco](
	[AtecoCod] [varchar](8) NOT NULL,
	[AtecoDesc] [varchar](255) NOT NULL,
 CONSTRAINT [PK_TbAteco] PRIMARY KEY CLUSTERED 
(
	[AtecoCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VAziCsp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VAziCsp]
AS
SELECT     dbo.TbAzi.AziAnnoLavoro, dbo.TbAzi.AziCod, dbo.TbAzi.AziRegimeIva, dbo.TbAzi.AziEsente, dbo.TbAzi.AziGruppoCesp, dbo.TbAzi.AziSpecieCesp, 
                      dbo.TbAzi.AziSottosCesp, dbo.TbAzi.AziUfficioIva, dbo.TbAzi.AziCodIstat, dbo.TbAzi.AziCodAttivita, dbo.TbAzi.AziDescAttivita, dbo.TbAzi.AziCognome, 
                      dbo.TbAzi.AziNome, dbo.TbAzi.AziDataNascita, dbo.TbAzi.AziSesso, dbo.TbAzi.AziComuneNascita, dbo.TbAzi.AziProvNascita, dbo.TbAzi.AziNaturaIva, 
                      dbo.TbAzi.AziNaturaRedditi, dbo.TbAzi.AziScritture, dbo.TbAzi.AziLuoghiAttivita, dbo.TbAzi.AziLeaCap, dbo.TbAzi.AziLeaComune, 
                      dbo.TbAzi.AziLeaProv, dbo.TbAzi.AziLeaIndirizzo, dbo.TbAzi.AziDrrCodiceFisc, dbo.TbAzi.AziDrrCognome, dbo.TbAzi.AziDrrNome, 
                      dbo.TbAzi.AziDrrSesso, dbo.TbAzi.AziDrrDataNascita, dbo.TbAzi.AziDrrCarica770, dbo.TbAzi.AziDrrCapNascita, dbo.TbAzi.AziDrrComuneNascita, 
                      dbo.TbAzi.AziDrrProvNascita, dbo.TbAzi.AziDrrCapRes, dbo.TbAzi.AziDrrComuneRes, dbo.TbAzi.AziDrrProvRes, dbo.TbAzi.AziDrrIndirizzo, 
                      dbo.TbAzi.AziDscCodiceFisc, dbo.TbAzi.AziDscCognome, dbo.TbAzi.AziDscNome, dbo.TbAzi.AziDscCap, dbo.TbAzi.AziDscComune, 
                      dbo.TbAzi.AziDscProv, dbo.TbAzi.AziDscIndirizzo, ISNULL(dbo.TbCsp.CspDesc, '') AS CspDesc, ISNULL(dbo.TbIstat.IstatDesc, '') AS IstatDesc, 
                      dbo.TbAzi.AziCodAteco, ISNULL(dbo.TbAteco.AtecoDesc, '') AS AtecoDesc
FROM         dbo.TbAzi LEFT OUTER JOIN
                      dbo.TbAteco ON dbo.TbAzi.AziCodAteco = dbo.TbAteco.AtecoCod LEFT OUTER JOIN
                      dbo.TbIstat ON dbo.TbAzi.AziCodIstat = dbo.TbIstat.IstatCod LEFT OUTER JOIN
                      dbo.TbCsp ON dbo.TbAzi.AziGruppoCesp = dbo.TbCsp.CspGru AND dbo.TbAzi.AziSpecieCesp = dbo.TbCsp.CspSpe1 AND 
                      dbo.TbAzi.AziSottosCesp = dbo.TbCsp.CspSpe2 AND dbo.TbCsp.CspNum = 0

GO
/****** Object:  View [dbo].[CReleiva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CReleiva]
AS
SELECT     TOP 100 PERCENT MIN(dbo.TbPri.PriNumProt) AS MINIMO, MAX(dbo.TbPri.PriNumProt) AS MASSIMO, dbo.TbPri.PriRegIva AS REGIVA, 
                      YEAR(dbo.TbPri.PriDataGio) AS ANNO, dbo.TbRegIva.RIvaDesc
FROM         dbo.TbPri INNER JOIN
                      dbo.TbRegIva ON dbo.TbPri.PriRegIva = dbo.TbRegIva.RIvaNReg
WHERE     (dbo.TbPri.PriRegIva <> 0)
GROUP BY YEAR(dbo.TbPri.PriDataGio), dbo.TbPri.PriRegIva, dbo.TbRegIva.RIvaAnno, dbo.TbRegIva.RIvaDesc
HAVING      (dbo.TbRegIva.RIvaAnno = YEAR(dbo.TbPri.PriDataGio))
ORDER BY anno, regiva

GO
/****** Object:  View [dbo].[VRegIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VRegIva]
AS
SELECT     RIvaAnno, RIvaNReg, ISNULL(RIvaSL, '') AS RIvaSL, RIvaNumFog, RIvaTipoDesc = CASE RIvaTipo WHEN '1' THEN cast(rivatipo AS varchar) 
                      + ' - Vendite' WHEN '2' THEN cast(rivatipo AS varchar) + ' - Acquisti' WHEN '3' THEN cast(rivatipo AS varchar) 
                      + ' - Rett. Vendite' WHEN '4' THEN cast(rivatipo AS varchar) + ' - Rett. Acquisti' WHEN '5' THEN cast(rivatipo AS varchar) 
                      + ' - Corrispettivi' WHEN '6' THEN cast(rivatipo AS varchar) + ' - In sospensione' WHEN '7' THEN cast(rivatipo AS varchar) 
                      + ' - Acquisti Attivit Agricoltura' WHEN '8' THEN cast(rivatipo AS varchar) + ' - Autofatture Agricoltura' WHEN '9' THEN cast(rivatipo AS varchar) 
                      + ' - Riepilogativo' END, RIvaPRata, RIvaDesc, RIvaCpt, isnull(TbPia.PiaAnaCo, '') AS RIvaCptDesc, RIvaAutoFCee, ISNULL(RIvaCptCee, '') 
                      AS RIvaCptCee, ISNULL(TbPia_1.PiaAnaCo, '') AS RIvaCptCeeDesc, ISNULL(RIvaCptSosp, '') AS RIvaCptSosp, ISNULL(TbPia_2.PiaAnaCo, '') 
                      AS RIvaCptSospDesc, ISNULL(RIvaCliCee, '') AS RIvaCliCee, isnull(AnaDesc, '') AS RivaCliDesc, RivaPrintIniziale, RivaArt, RivaInt, RIvaCh, 
                      ISNULL(RIvaRCharge, '0') AS RIvaRCharge,RIvaFteP,RivaTipoDoc,RIvaTipo
FROM         TbPia RIGHT OUTER JOIN
                      TbPia TbPia_2 RIGHT OUTER JOIN
                      TbRegIva LEFT OUTER JOIN
                      TbPia TbPia_1 ON RIvaCptCee = TbPia_1.PiaCodCo ON TbPia_2.PiaCodCo = RIvaCptSosp ON TbPia.PiaCodCo = RIvaCpt LEFT OUTER JOIN
                      vdox.dbo.tbana ON anacod = rivaclicee AND AnaGrp = 'CL'




GO
/****** Object:  View [dbo].[CRQUADIVAVEND]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRQUADIVAVEND]
AS
SELECT DISTINCT PriCoAvere, YEAR(PriDataGio) AS ANNO, SUM(PriImpDare) AS imponibile, SUM(PriImpAvere) AS iva, ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PRICOAVERE), '***ERRATO***'))) AS AVEREDESC
FROM         dbo.TbPri
WHERE     (PriCausale = 1)
GROUP BY YEAR(PriDataGio), PriCoAvere
GO
/****** Object:  View [dbo].[CRQUADIVACQ]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRQUADIVACQ]
AS
SELECT DISTINCT PriCoDare, YEAR(PriDataGio) AS ANNO, SUM(PriImpAvere) AS imponibile, SUM(PriImpDare) AS iva, ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICODARE AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICODARE AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PRICODARE), '***ERRATO***'))) AS DAREDESC
FROM         dbo.TbPri
WHERE     (PriCausale = 2)
GROUP BY YEAR(PriDataGio), PriCoDare
GO
/****** Object:  View [dbo].[CRVERSAM]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRVERSAM]
AS
SELECT     TOP 100 PERCENT dbo.TbVers.*
FROM         dbo.TbVers
ORDER BY IvaVAnno, IvaVData, IvaVMese
GO
/****** Object:  View [dbo].[VDISTRB0]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VDISTRB0]
AS
SELECT     TOP 100 PERCENT dbo.TbEff.RicDsca AS SCADENZA, dbo.TbEff.RicClie AS CLIENTE, Vdox.dbo.TbAna.AnaDesc AS EMITTENTE, 
                      Vdox.dbo.TbAna.AnaCitta AS PIAZZA, dbo.TbEff.RicImpRata AS IMPRATA, dbo.TbEff.RicImpFatt AS IMPFATT, 'E' AS ESPOS, 
                      'NS. RIF. FATT. N.' + CAST(dbo.TbEff.RicNfat AS varchar) + ' DEL ' + CONVERT(char, dbo.TbEff.RicDfat, 103) AS docum, dbo.TbEff.RicAbi AS ABI, 
                      dbo.TbEff.RicCab AS CAB, Vdox.dbo.TbAna.AnaRag1, Vdox.dbo.TbAna.AnaRag2, Vdox.dbo.TbAna.AnaCfis, Vdox.dbo.TbAna.AnaPiva, 
                      Vdox.dbo.TbAna.AnaIndirizzo, Vdox.dbo.TbAna.AnaCap, Vdox.dbo.TbAna.AnaProv, dbo.TbEff.RicProg,dbo.TbEff.RicTPag
FROM         dbo.TbEff INNER JOIN
                      Vdox.dbo.TbAna ON dbo.TbEff.RicClie = Vdox.dbo.TbAna.AnaCod
WHERE     (dbo.TbEff.RicSeff = '2' and dbo.TbEff.RicTpag =2)
ORDER BY dbo.TbEff.RicDsca, Vdox.dbo.TbAna.AnaDesc





GO
/****** Object:  UserDefinedFunction [dbo].[fnFotoRIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE function [dbo].[fnFotoRIva](@Anno smallint)
returns table
as
return
SELECT top 100 percent RivaNreg,RivaTipo,
TipoDesc = case when RivaTipo = 1 then 'Vendite' 
when RivaTipo = 2 then 'Acquisti' when RivaTipo = 3 then 'Rett.Vendite'
when RivaTipo = 4 then 'Rett.Acquisti' when RivaTipo = 5 then 'Corrispettivi' when RivaTipo = 6 then 'In sospensione'
when RivaTipo = 7 then 'Acquisti Att. agricoltura' when RivaTipo = 8 then 'Autofatture Agricoltura' 
when RivaTipo = 9 then 'Riepilogativo' else ' ' end,RivaDesc,
ProtCar=(SELECT isnull(Max(PriNumProt),0) FROM tbPri where datepart(year,pridatagio) = @ANNO AND RivaNreg = PriRegIva),
DataCar=(SELECT isnull(Max(PriDataGio),'01/01/'+ CONVERT(VARCHAR(4),@ANNO)) FROM tbPri where  datepart(year,pridatagio) = @ANNO  AND RivaNreg = PriRegIva),
(SELECT isnull(Max(PriNumProt),0) FROM tbPri where  datepart(year,pridatagio) = @ANNO  and PriIvaPrint = 1 AND RivaNreg = PriRegIva) as ProtSta,
(SELECT isnull(Max(PriDataGio),'31/12/'+ CONVERT(VARCHAR(4),@ANNO - 1)) FROM tbPri where  datepart(year,pridatagio) = @ANNO  and PriIvaPrint = 1 AND RivaNreg = PriRegIva) as DataSta,RivaPrintIniziale,RivaCpt,isnull(RivaArt,0) as RivaArt,
RivaAutoFcee,RivaCptCee,RivaCliCee,RivaInt,RivaNumFog,RivaSL, isnull(RivaRCharge,0) as RivaRCharge,RivaFteP,RivaTipoDoc
FROM TbRegIva
where RivaAnno = @ANNO 
group by RivaNreg,RivaTipo,RivaDesc,RivaPrintIniziale,RivaCpt,RivaArt,RivaAutoFcee,RivaCptCee,RivaCliCee,RivaNumFog,RivaInt,RivaSl,RivaRCharge,RivaFteP,RivaTipoDoc
Order by RivaNreg




GO
/****** Object:  View [dbo].[CRQUADCORRISP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRQUADCORRISP]
AS
SELECT DISTINCT PriCoAvere, YEAR(PriDataGio) AS ANNO, SUM(PriImpDare) AS imponibile, ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PRICOAVERE AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PRICOAVERE), '***ERRATO***'))) AS AVEREDESC
FROM         dbo.TbPri
WHERE     (PriCausale > 3) AND (PriRegIva > 0)
GROUP BY YEAR(PriDataGio), PriCoAvere
GO
/****** Object:  View [dbo].[CRIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRIVA]
AS
SELECT     CiiCod AS CODIIVA, CiiDes AS DESCIVA, CiiAli AS PERCE, CiiInd, CiiTp, CiiCmp
FROM         dbo.TbCii
GO
/****** Object:  View [dbo].[CRL8]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[CRL8]
AS
SELECT     TOP 100 PERCENT 
CodiceIva =case when a.PriCodIva = 0 and a.PriCausale < 3 then (select top 1 b.PriCodIva from TbPri as b where b.PriId = a.Priid and b.PriProg > a.priprog and b.PriCodIva > 0) else a.pricodiva end ,
a.PriDataGio AS Datagio, a.PriCoAvere AS COAVERE, a.PriCoDare AS CODARE, a.PriNumProt AS numpro, 
                      a.PriBisRet AS bis, a.PriRegIva AS RegIva, a.PriImpDare AS Imponibile, a.PriDocEst AS Ndoc, a.PriDataEst AS DataDoc,
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.PRICOAVERE AND ANAGRP = 'FO'),
 ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.PRICOAVERE AND ANAGRP = 'CL'),
  ISNULL((SELECT PIAANACO FROM TbPia WHERE PIACODCO = a.PRICOAVERE), '***ERRATO***'))) AS DESCAVERE,
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.PRICODARE AND ANAGRP = 'FO'),
 ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.PRICODARE AND ANAGRP = 'CL'),
  ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = a.PRICODARE), '***ERRATO***'))) AS DESCDARE,
   a.PriCausale, a.PriImpAvere, a.PriFl04, a.PriFl06,
ISNULL((SELECT DESCIVA FROM CRIVA WHERE (case when a.PriCodIva = 0 and a.PriCausale < 3 then (select top 1 b.PriCodIva from TbPri as b where b.PriId = a.Priid and b.PriProg > a.priprog and b.PriCodIva > 0) else a.pricodiva end) = CRIVA.CODIIVA), '') AS desiva,
ISNULL((SELECT CIICAU FROM dbo.TbCii WHERE a.PriCAUSALE = TbCii.CiiCod), '') AS desCAU,
a.PriCodPag
FROM  dbo.TbPri as a
ORDER BY a.PriDataGio, a.PriRegIva, a.PriNumProt, a.PriCoAvere




GO
/****** Object:  View [dbo].[VSOLE24ORE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VSOLE24ORE]
AS
SELECT     PIACODCO, PIAANACO, PIAFL11, CEEDESC, CEEALT, CAST(CEEST AS SMALLINT) AS CEEST, (SUM(TMSSALDO) + SUM(TMSSALDOP)) AS SALDO, 
                      RIGO = CASE WHEN (SUM(TMSSALDO) + SUM(TMSSALDOP)) < 0 AND CEEALT > 0 THEN CEEALT ELSE PIAFL11 END, TMSBLOCK
FROM         TBPIA INNER JOIN
                      TBCEE ON PIAFL11 = CEECOD INNER JOIN
                      TMPBILS ON PIACODCO = TMSCONTO
WHERE     PIAFL11 > 0
GROUP BY TMSBLOCK, PIACODCO, PIAANACO, PIAFL11, CEEDESC, CEEALT, CEEST
HAVING      (SUM(TMSSALDO) + SUM(TMSSALDOP)) <> 0
GO
/****** Object:  View [dbo].[VSCAGRUP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VSCAGRUP]
AS
SELECT DISTINCT 
                      dbo.TbEff.RicDsca, SUM(dbo.TbEff.RicImpRata) AS rata, Vdox.dbo.TbAna.AnaDesc AS ANAGRAFICA, dbo.TbEff.RicClie AS CODCLI, 
                      dbo.TbEff.RicAbi AS ABI, dbo.TbEff.RicCab AS CAB, compensazione = CASE WHEN SUM(dbo.TbEff.RicImpRata) > 0 THEN SUM(dbo.TbEff.RicImpRata) 
                      ELSE 0 END, dbo.TbEff.RicSeff AS STATO, dbo.TbEff.RicTpag AS TIPAG
FROM         dbo.TbEff INNER JOIN
                      Vdox.dbo.TbAna ON dbo.TbEff.RicClie = Vdox.dbo.TbAna.AnaCod
GROUP BY dbo.TbEff.RicDsca, Vdox.dbo.TbAna.AnaDesc, dbo.TbEff.RicClie, dbo.TbEff.RicSeff, dbo.TbEff.RicAbi, dbo.TbEff.RicCab, dbo.TbEff.RicTpag
GO
/****** Object:  Table [dbo].[TMPREGIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPREGIVA](
	[PRegId] [int] NOT NULL,
	[PRegDataG] [smalldatetime] NOT NULL,
	[PRegDataE] [smalldatetime] NOT NULL,
	[PRegNumProt] [int] NOT NULL,
	[PRegProtBis] [varchar](1) NOT NULL,
	[PRegNumDoc] [varchar](20) NOT NULL,
	[PRegCliFor] [varchar](5) NOT NULL,
	[PRegAnaGraf] [varchar](60) NOT NULL,
	[PRegImpon] [decimal](13, 2) NOT NULL,
	[PRegCodIva] [smallint] NOT NULL,
	[PRegAliq] [varchar](12) NOT NULL,
	[PRegImpIva] [decimal](13, 2) NOT NULL,
	[PRegCpt] [varchar](5) NOT NULL,
	[PRegValuta] [decimal](13, 2) NOT NULL,
	[PRegPND] [smallint] NOT NULL,
	[PRegMerce] [smallint] NOT NULL,
	[PRegNumReg] [smallint] NOT NULL,
	[PRegPriId] [int] NOT NULL,
	[PRegPriProg] [smallint] NOT NULL,
	[PRegFinoAl] [smalldatetime] NULL,
 CONSTRAINT [PK_TMPREGIVA] PRIMARY KEY CLUSTERED 
(
	[PRegId] ASC,
	[PRegNumProt] ASC,
	[PRegProtBis] ASC,
	[PRegNumReg] ASC,
	[PRegPriId] ASC,
	[PRegPriProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRREGIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[CRREGIVA]
AS
SELECT     TOP 100 PERCENT PRegId, PRegDataG, PRegDataE, PRegNumProt, PRegProtBis, PRegNumDoc,PRegCliFor, PRegAnaGraf, PRegImpon, PRegCodIva, PRegAliq, 
                      PRegImpIva, PRegCpt, PRegValuta,PRegPND, PRegMerce, PRegNumReg, PRegPriId,PRegPriProg,PRegFinoAl
FROM         dbo.TMPREGIVA
ORDER BY PRegNumReg, PRegNumProt, PRegProtBis, PRegPriId,PRegPriProg





GO
/****** Object:  View [dbo].[DXProgCF]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DXProgCF]
AS
SELECT DISTINCT 
                      TOP (100) PERCENT YEAR(dbo.TbPri.PriDataGio) AS Anno, dbo.TbPri.PriCoDare AS Codice, SUM(dbo.TbPri.PriImpDare) AS Imponibile, SUM(dbo.TbPri.PriImpAvere) 
                      AS Iva, SUM(dbo.TbPri.PriImpDare + dbo.TbPri.PriImpAvere) AS TFAT, VDOX.dbo.TbAna.AnaDesc,  VDOX.dbo.TbAna.AnaCfis, 
                      VDOX.dbo.TbAna.AnaIndirizzo, VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, 
                      VDOX.dbo.TbAna.AnaGrp,
                      CASE WHEN VDOX.dbo.TbAna.AnaPivaEst > '' then VDOX.dbo.TbAna.AnaPivaEst else   VDOX.dbo.TbAna.AnaPiva END as PIva
FROM         dbo.TbPri INNER JOIN
                      VDOX.dbo.TbAna ON VDOX.dbo.TbAna.AnaCod = dbo.TbPri.PriCoDare
WHERE     (dbo.TbPri.PriCausale = 1)
GROUP BY YEAR(dbo.TbPri.PriDataGio), dbo.TbPri.PriCoDare, VDOX.dbo.TbAna.AnaDesc, VDOX.dbo.TbAna.AnaPiva, VDOX.dbo.TbAna.AnaCfis, VDOX.dbo.TbAna.AnaIndirizzo, 
                      VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, VDOX.dbo.TbAna.AnaPivaEst, VDOX.dbo.TbAna.AnaGrp
UNION
SELECT DISTINCT 
                      TOP (100) PERCENT YEAR(dbo.TbPri.PriDataGio) AS Anno, dbo.TbPri.PriCoAvere AS Codice, SUM(dbo.TbPri.PriImpDare) AS Imponibile, SUM(dbo.TbPri.PriImpAvere) 
                      AS Iva, SUM(dbo.TbPri.PriImpDare + dbo.TbPri.PriImpAvere) AS TFAT, VDOX.dbo.TbAna.AnaDesc,  VDOX.dbo.TbAna.AnaCfis, 
                      VDOX.dbo.TbAna.AnaIndirizzo, VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, 
                      VDOX.dbo.TbAna.AnaGrp,
                      CASE WHEN VDOX.dbo.TbAna.AnaPivaEst > '' then VDOX.dbo.TbAna.AnaPivaEst else   VDOX.dbo.TbAna.AnaPiva END as PIva
FROM         dbo.TbPri INNER JOIN
                      VDOX.dbo.TbAna ON VDOX.dbo.TbAna.AnaCod = dbo.TbPri.PriCoAvere
WHERE     (dbo.TbPri.PriCausale = 2)
GROUP BY YEAR(dbo.TbPri.PriDataGio), dbo.TbPri.PriCoAvere, VDOX.dbo.TbAna.AnaDesc, VDOX.dbo.TbAna.AnaPiva, VDOX.dbo.TbAna.AnaCfis, VDOX.dbo.TbAna.AnaIndirizzo, 
                      VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, VDOX.dbo.TbAna.AnaPivaEst, VDOX.dbo.TbAna.AnaGrp
ORDER BY Anno, VDOX.dbo.TbAna.AnaGrp, Codice

GO
/****** Object:  View [dbo].[CRB4CLI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRB4CLI]
AS
SELECT DISTINCT 
                      TOP 100 PERCENT YEAR(dbo.TbPri.PriDataGio) AS Anno, dbo.TbPri.PriCoDare AS Cliente, SUM(dbo.TbPri.PriImpDare) AS Imponibile, 
                      SUM(dbo.TbPri.PriImpAvere) AS Iva, VDOX.dbo.TbAna.AnaDesc, VDOX.dbo.TbAna.AnaPiva, VDOX.dbo.TbAna.AnaCfis, VDOX.dbo.TbAna.AnaIndirizzo, 
                      VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, VDOX.dbo.TbAna.AnaPivaEst
FROM         dbo.TbPri INNER JOIN
                      VDOX.dbo.TbAna ON VDOX.dbo.TbAna.AnaCod = dbo.TbPri.PriCoDare
WHERE     (dbo.TbPri.PriCausale = 1)
GROUP BY YEAR(dbo.TbPri.PriDataGio), dbo.TbPri.PriCoDare, VDOX.dbo.TbAna.AnaDesc, VDOX.dbo.TbAna.AnaPiva, VDOX.dbo.TbAna.AnaCfis, 
                      VDOX.dbo.TbAna.AnaIndirizzo, VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, VDOX.dbo.TbAna.AnaPivaEst
ORDER BY YEAR(dbo.TbPri.PriDataGio), dbo.TbPri.PriCoDare

GO
/****** Object:  View [dbo].[CRB4FOR]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRB4FOR]
AS
SELECT DISTINCT 
                      TOP 100 PERCENT YEAR(dbo.TbPri.PriDataGio) AS Anno, dbo.TbPri.PriCoAvere AS Fornitore, SUM(dbo.TbPri.PriImpDare) AS Imponibile, 
                      SUM(dbo.TbPri.PriImpAvere) AS Iva, VDOX.dbo.TbAna.AnaDesc, VDOX.dbo.TbAna.AnaPiva, VDOX.dbo.TbAna.AnaCfis, VDOX.dbo.TbAna.AnaIndirizzo, 
                      VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, VDOX.dbo.TbAna.AnaPivaEst
FROM         dbo.TbPri INNER JOIN
                      VDOX.dbo.TbAna ON VDOX.dbo.TbAna.AnaCod = dbo.TbPri.PriCoAvere
WHERE     (dbo.TbPri.PriCausale = 2)
GROUP BY YEAR(dbo.TbPri.PriDataGio), dbo.TbPri.PriCoAvere, VDOX.dbo.TbAna.AnaDesc, VDOX.dbo.TbAna.AnaPiva, VDOX.dbo.TbAna.AnaCfis, 
                      VDOX.dbo.TbAna.AnaIndirizzo, VDOX.dbo.TbAna.AnaCap, VDOX.dbo.TbAna.AnaCitta, VDOX.dbo.TbAna.AnaProv, VDOX.dbo.TbAna.AnaPivaEst
ORDER BY YEAR(dbo.TbPri.PriDataGio), dbo.TbPri.PriCoAvere

GO
/****** Object:  View [dbo].[VDISTRB1]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VDISTRB1]
AS
SELECT     TOP 100 PERCENT RicDsca AS SCADENZA, dbo.TbEff.RicClie AS CLIENTE, Vdox.dbo.TbAna.AnaDesc AS EMITTENTE, 
                      Vdox.dbo.TbAna.AnaCitta AS PIAZZA, SUM(dbo.TbEff.RicImpRata) AS IMPRATA, SUM(dbo.TbEff.RicImpFatt) AS IMPFATT, dbo.TbEff.RicTpag,
           ESPOS = CASE WHEN COUNT(*) > 1 THEN 'R' ELSE 'E' END, 
           DOCUM = CASE WHEN COUNT(*) > 1 THEN 'NS. ESTRATTO CONTO MESE ' +  SUBSTRING(CONVERT(char, max(RicDfat), 103),4,7)
                                          ELSE 'NS. RIF. FATT. N.' + cast(max(Ricnfat) AS varchar) + ' DEL ' + CONVERT(char, max(RicDfat), 103)END,
           RicAbi AS ABI,RicCab AS CAB,AnaRag1,Anarag2,AnaCfis,AnaPiva,AnaIndirizzo,AnaCap,AnaProv,RICPROG = max(ricprog)
FROM       dbo.TbEff INNER JOIN
                      Vdox.dbo.TbAna ON dbo.TbEff.RicClie = Vdox.dbo.TbAna.AnaCod
WHERE     dbo.TbEff.RicSeff = '2' and RicTPag = 2 
GROUP BY dbo.TbEff.RicDsca, dbo.TbEff.RicClie, Vdox.dbo.TbAna.AnaDesc,
                      dbo.TbEff.RicSeff, RicAbi, RicCab, Vdox.dbo.TbAna.AnaCitta, dbo.TbEff.RicSeff,AnaRag1,Anarag2,AnaCFis,AnaPiva,AnaIndirizzo,AnaCap,AnaProv,
                     dbo.TbEff.RicTpag
ORDER BY dbo.TbEff.RicDsca, Vdox.dbo.TbAna.AnaDesc

GO
/****** Object:  View [dbo].[VH1H2]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VH1H2]
AS
select DISTINCT TOP 100 PERCENT  priid,PriProg,PridataGio,pricausale,
CONTO = case when pricausale = 1 THEN PriCodare ELSE PriCoAvere end,
DAREDESC =case when pricausale = 1 THEN ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoDare ),'***ERRATO***'))) ELSE
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoAvere ),'***ERRATO***'))) end,
CPT = case when pricausale = 1 THEN PriCoAvere ELSE PriCoDare end,
AVEREDESC =case when pricausale = 1 THEN ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoAvere ),'***ERRATO***'))) ELSE
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoDare ),'***ERRATO***'))) end,
PriImpDare,PriImpAvere,priDesc,priGstampa,PriCodPag,PriIvaPrint,
PriNumProt,PriBisRet,PriCodIva,isnull(CiiDes,'') as CiiDes,Isnull(CiiAli,0) as CiiAli,PriRegIva,PriDocEst,PriDataEst,PriDescB,PriNsRif,PriArtFisc,
TIPOREG = isnull((select RIvaTipo from TbRegIva where PriRegIva = RIvaNReg and datePart(year,pridatagio) = RIvaAnno),0),PriDocAnn,PriValuta,PriLinea,PriMeseSk
from TBPRI 
INNER JOIN TBPRK 
ON PRIID = PRKID
left outer join tbcii as cii on pricodiva = cii.ciicod
WHERE PriCausale < 3
ORDER BY PRIregIva,PriNumProt,PriBisRet,PRIID,PRIPROG



GO
/****** Object:  View [dbo].[VESTRATTO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VESTRATTO]
AS

SELECT     ScaImpRata = ISNULL(ScaImpRata, CASE WHEN PRKDA = 0 THEN DARE ELSE AVERE END), ScaNrata = isnull(ScaNrata, 1), 
                      ScaDsca = Isnull(ScaDsca, Pridataest), PrkConto, PrkTipoCo, PrkDesc, Pridataest, Causale, PriNumProt, Priregiva, PrkDocEst, DARE, AVERE, Descriz, 
                      PriCausale, PrkDocAnn, Prkaammgg, Partitario, PrkPaperta, PrIID, PRKAST, FORMULA, PRKINDI = CASE WHEN PRKTIPOCO = 0 THEN '' ELSE
                          (SELECT     ANAINDIRIZZO
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = PRKCONTO AND ANAGRP = 'CL') END, PRKCITTA = CASE WHEN PRKTIPOCO = 0 THEN '' ELSE
                          (SELECT     ANACAP + ' ' + ANACITTA + ' ' + ANAPROV
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = PRKCONTO AND ANAGRP = 'CL') END, PRIPROG, PRKDA
FROM         tbsca RIGHT OUTER JOIN
                      vb8 ON SCARIFID = PRIID AND SCARIFPROG = PRIPROG AND ScaRifDa = PRKDA INNER JOIN
                      GEVE.DBO.TbCli ON prkconto = ClCod 
WHERE     PARTITARIO = 1 AND PRKPAPERTA = 0

GO
/****** Object:  View [dbo].[VH4]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VH4]
AS
SELECT DISTINCT TOP 100 PERCENT priid, PriProg, PridataGio, pricausale, PriCodare, DAREDESC = ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PriCoDare AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PriCoDare AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PriCoDare), '***ERRATO***'))), PriCoAvere, AVEREDESC = ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PriCoAvere AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = PriCoAvere AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = PriCoAvere), '***ERRATO***'))), IMPORTO = CASE WHEN PriCoDare = '00.10' THEN PriImpAvere ELSE PriImpDare END, 
                      PriImpDare, PriImpAvere, priDesc, priGstampa, PriCodPag, PriIvaPrint, PriNumProt, PriBisRet, PriCodIva, PriRegIva, PriDocEst, PriDataEst, PriDescB, 
                      PriNsRif, PriArtFisc, PriDocAnn, PriSos
FROM         TBPRI INNER JOIN
                      TBPRK ON PRIID = PRKID
WHERE     PriCausale > 3 AND PRIREGIVA = 0
ORDER BY PriNumProt, PRIID, PRIPROG


GO
/****** Object:  View [dbo].[VH8]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VH8]
AS
SELECT     TOP 100 PERCENT priartfisc, prkconto, prkTipoCo, PRKDESC = CASE WHEN PRKTIPOCO = 0 THEN
                          (SELECT     PIAANACO
                            FROM          TBPIA
                            WHERE      PIACODCO = PRKCONTO) ELSE
                          (SELECT     ANADESC
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = PRKCONTO AND ANAGRP = 'CL' OR
                                                   ANACOD = PRKCONTO AND ANAGRP = 'FO') END, pridatagio, pridataest, cau.CiiCau AS Causale, PriNumProt, PriregIva, PriDocEst, 
                      CPT = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare END, 
                      CPTDESC = ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE
                                                     pricodare END),
                          (SELECT     ANADESC
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare
                                                    END AND ANAGRP = 'CL' OR
                                                   ANACOD = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare
                                                    END AND ANAGRP = 'FO')), DARE = CONVERT(DECIMAL(13, 2), 
                      CASE WHEN PriCausale = 1 THEN 0 WHEN pricausale = 2 THEN PriImpDare + (CASE WHEN pricodiva = 0 THEN 0 ELSE priimpavere * cii.ciiInd / 100 END)
                       WHEN prkda = 0 THEN priimpdare ELSE 0 END), AVERE = CONVERT(DECIMAL(13, 2), 
                      CASE WHEN PriCausale = 1 THEN PriImpDare + (CASE WHEN pricodiva = 0 THEN 0 ELSE priimpavere * cii.ciiInd / 100 END) 
                      WHEN pricausale = 2 THEN 0 WHEN prkda = 0 THEN 0 ELSE priimpavere END), pridesc + pridescb AS descriz, PrkAammgg, 
                      PIAFL = CASE WHEN PRKTIPOCO = 0 THEN
                          (SELECT     PIAFL01
                            FROM          TBPIA
                            WHERE      PIACODCO = PRKCONTO) ELSE 0 END, PRICAUSALE, PriSos, MDESC = CASE WHEN Prisos <> '' THEN ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = Prisos), isnull
                          ((SELECT     ANADESC
                              FROM         vdox.dbo.tbana
                              WHERE     ANACOD = Prisos AND ANAGRP = 'CL'), isnull
                          ((SELECT     ANADESC
                              FROM         vdox.dbo.tbana
                              WHERE     ANACOD = Prisos AND ANAGRP = 'FO'), ''))) ELSE '' END
FROM         Tbpri INNER JOIN
                      Tbprk ON prkId = priid AND PrkProg = priProg LEFT OUTER JOIN
                      tbcii AS cau ON pricausale = cau.ciicod LEFT OUTER JOIN
                      tbcii AS cii ON pricodiva = cii.ciicod





GO
/****** Object:  View [dbo].[VTMPRIEPIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VTMPRIEPIVA]
AS
SELECT DISTINCT 
                      TOP 100 PERCENT dbo.TMPIVAS.IvaPAnno, MAX(dbo.TMPIVAS.IvaPMese) AS IvaPMese, dbo.TMPIVAS.IvaPRegIva, dbo.TMPIVAS.IvaPCodIva, 
                      SUM(dbo.TMPIVAS.IvaPImpon) AS Timpon, SUM(dbo.TMPIVAS.IvaPIvaDE) AS IvaDe, SUM(dbo.TMPIVAS.IvaPIvaND) AS IvaNd, 
                      SUM(dbo.TMPIVAS.IvaPImpMerce) AS Merce, dbo.TbCii.CiiDes, dbo.TbCii.CiiAli
FROM         dbo.TMPIVAS LEFT OUTER JOIN
                      dbo.TbCii ON dbo.TMPIVAS.IvaPCodIva = dbo.TbCii.CiiCod
GROUP BY dbo.TMPIVAS.IvaPAnno, dbo.TMPIVAS.IvaPRegIva, dbo.TMPIVAS.IvaPCodIva, dbo.TbCii.CiiDes, dbo.TbCii.CiiAli
ORDER BY dbo.TMPIVAS.IvaPAnno, IvaPMese, dbo.TMPIVAS.IvaPRegIva, dbo.TMPIVAS.IvaPCodIva, dbo.TbCii.CiiDes, dbo.TbCii.CiiAli
GO
/****** Object:  View [dbo].[CRCESPITI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRCESPITI]
AS
SELECT     TOP 100 PERCENT *, ISNULL(YEAR(CespDataCessione), 9999) AS ceduto
FROM         dbo.TbCesp
ORDER BY CespCat, CespNum
GO
/****** Object:  View [dbo].[CRESTRATTO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CRESTRATTO]
AS
SELECT     TOP 100 PERCENT ScaTipoCo, ScaConto, SCADESC = ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANADESC
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = ScaConto), '***ERRATO***'))), SCAINDI = ISNULL
                          ((SELECT     ANAINDIRIZZO
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANAINDIRIZZO
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = ScaConto), '***ERRATO***'))), SCACITTA = ISNULL
                          ((SELECT     ANACITTA
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANACITTA
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = ScaConto), '***ERRATO***'))), SCACAP = ISNULL
                          ((SELECT     ANACAP
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANACAP
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = ScaConto), '***ERRATO***'))), SCAPROV = ISNULL
                          ((SELECT     ANAPROV
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANAPROV
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = ScaConto), '***ERRATO***'))), SCATEL = ISNULL
                          ((SELECT     ANATEL1
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'CL'), ISNULL
                          ((SELECT     ANATEL1
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto AND ANAGRP = 'FO'), ISNULL
                          ((SELECT     PIAANACO
                              FROM         TBPIA
                              WHERE     PIACODCO = ScaConto), '***ERRATO***'))), ScaNdoc, ScaImpDoc, ScaIvaSpe, ScaAbi, ScaCab, ScaImpRata, ScaTPag, ScaCodPag, 
                      ScaNRata, ScaBan, ScaDdoc, ScaDSca, ScaRifId, ScaRifProg, ScaRifDA, PagDesc, ScaId, PrkPaperta, ScaRAperta, ScaImpPagato, 
                      CLFOPI = CASE WHEN ScaTipoCo = '0' THEN 'SO' ELSE ISNULL
                          ((SELECT     ANAGRP
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto), 'F0') END, 
                      TIPA = CASE WHEN ScaTPag = 1 THEN 'TRATTA' WHEN ScaTPag = 2 THEN ' R.B.' WHEN ScaTPag = 3 THEN ' R.D.' WHEN ScaTPag = 4 THEN 'CONTA' WHEN
                       ScaTPag = 5 THEN 'BONIF' WHEN ScaTPag = 6 THEN 'C/ASS' WHEN ScaTPag = 7 THEN 'R.I.D' WHEN ScaTPag = 8 THEN 'C.C.P' ELSE ' ' END, 
                      (ScaImpRata - ScaImpPagato) AS ScaScopRata
FROM         TbSca LEFT OUTER JOIN
                      TBPAG ON ScaCodPag = PagCod LEFT OUTER JOIN
                      TBPRK ON SCARIFID = PRKID AND SCARIFPROG = PRKPROG AND SCARIFDA = PRKDA
WHERE     PRKPAPERTA = 0
ORDER BY SCATIPOCO, SCACONTO, SCADSCA

GO
/****** Object:  View [dbo].[VVERIFICASPE2011]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[VVERIFICASPE2011]
AS


SELECT DISTINCT 
                      EleCfUnion AS UNIONE, EleCfPriId AS RECREG, EleCfAnno AS ANNO, EleCfTipo AS TIPO, EleCfCodice AS CODICE, EleCfAnaDesc AS DENOMINAZIONE, 
                      SUM(EleCfImponibile) AS IMPONIBILE, SUM(EleCfIva) AS IVA, EleCfDataDoc AS DATAREG, EleCfNumDoc AS NUMDOC, EleCfP AS FLAG, EleCfModPag as PAG, 
                      CASE WHEN (SUM(EleCfImponibile)) < 3000 AND EleCfModPag = 'N' THEN 'MINORE DI € 3.000!!!!' ELSE '' END AS ERRORE
FROM         dbo.TbEleCf
WHERE     (elecfanno = 2011 AND EleCfP > 1 AND EleCfUnion = 0)
GROUP BY EleCfPriId, EleCfAnno, EleCfTipo, EleCfCodice, EleCfAnaDesc, EleCfUnion, EleCfDataDoc, EleCfNumDoc, EleCfP,EleCfModPag
UNION
SELECT DISTINCT 
                      EleCfUnion AS UNIONE, 0 AS RECREG, EleCfAnno AS ANNO, EleCfTipo AS TIPO, EleCfCodice AS CODICE, EleCfAnaDesc AS DENOMINAZIONE, SUM(EleCfImponibile) 
                      AS IMPONIBILE, SUM(EleCfIva) AS IVA, '31/12/2011' AS DATAREG, 0 AS NUMDOC, EleCfP AS FLAG, EleCfModPag as PAG,
                      CASE WHEN (SUM(EleCfImponibile)) < 3000 AND EleCfModPag = 'N' THEN 'MINORE DI € 3.000!!!!' ELSE '' END AS ERRORE

FROM         dbo.TbEleCf
WHERE     (elecfanno = 2011 AND EleCfP > 1 AND EleCfUnion > 0)
GROUP BY EleCfUnion, EleCfAnno, EleCfTipo, EleCfCodice, EleCfAnaDesc, EleCfP,EleCfModPag


GO
/****** Object:  Table [dbo].[TBDXMENU]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBDXMENU](
	[BARGROUP] [smallint] NOT NULL,
	[BARID] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VDXMenu]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




create VIEW [dbo].[VDXMenu]
AS
SELECT     VDOX.dbo.TbGrupLav.*, dbo.TbDXMenu.BARID AS Indice
FROM         VDOX.dbo.TbGrupLav LEFT OUTER JOIN
                      dbo.TbDXMenu ON VDOX.dbo.TbGrupLav.GrupLavId = dbo.TbDXMenu.BARGROUP




GO
/****** Object:  View [dbo].[DXSCACLF]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[DXSCACLF]
AS
SELECT     dbo.TbSca.ScaTipoCo, dbo.TbSca.ScaConto, ISNULL
                          ((SELECT     AnaDesc
                              FROM         VDOX.dbo.TbAna
                              WHERE     (AnaCod = dbo.TbSca.ScaConto) AND (AnaGrp = 'CL')), ISNULL
                          ((SELECT     AnaDesc
                              FROM         VDOX.dbo.TbAna AS TbAna_1
                              WHERE     (AnaCod = dbo.TbSca.ScaConto) AND (AnaGrp = 'FO')), ISNULL
                          ((SELECT     PiaAnaCo
                              FROM         dbo.TbPia
                              WHERE     (PiaCodCo = dbo.TbSca.ScaConto)), '***ERRATO***'))) AS SCADESC, dbo.TbSca.ScaNdoc, dbo.TbSca.ScaImpDoc, dbo.TbSca.ScaImpRata, 
                      dbo.TbSca.ScaTPag, dbo.TbSca.ScaDdoc, dbo.TbSca.ScaDsca, dbo.TbPrk.PrkPAperta, dbo.TbSca.ScaImpRata - dbo.TbSca.ScaImpPagato AS ScaScopRata, 
                      CASE WHEN ScaTipoCo = '0' THEN 'SO' ELSE ISNULL
                          ((SELECT     ANAGRP
                              FROM         VDOX.DBO.TBANA
                              WHERE     ANACOD = ScaConto), 'FO') END AS CLFOPI,
                              
          CASE WHEN ScaTPag = 1 THEN (TbSca.ScaImpRata - dbo.TbSca.ScaImpPagato) ELSE 0 END AS ScaSca1, 
          CASE WHEN ScaTPag = 2 THEN (TbSca.ScaImpRata - dbo.TbSca.ScaImpPagato) ELSE 0 END AS ScaSca2,
          CASE WHEN ScaTPag = 3 THEN (TbSca.ScaImpRata - TbSca.ScaImpPagato)ELSE 0 END AS ScaSca3,
          CASE WHEN ScaTPag = 4 THEN (TbSca.ScaImpRata - dbo.TbSca.ScaImpPagato) ELSE 0 END AS ScaSca4, 
          CASE WHEN ScaTPag = 5 THEN (TbSca.ScaImpRata - TbSca.ScaImpPagato) ELSE 0 END AS ScaSca5, 
          CASE WHEN ScaTPag = 6 THEN (TbSca.ScaImpRata - dbo.TbSca.ScaImpPagato) ELSE 0 END AS ScaSca6,
          CASE WHEN ScaTPag = 7 THEN (TbSca.ScaImpRata - TbSca.ScaImpPagato) ELSE 0 END AS ScaSca7,
          CASE WHEN ScaTPag = 8 THEN (TbSca.ScaImpRata - dbo.TbSca.ScaImpPagato) ELSE 0 END AS ScaSca8, 
           dbo.TbSca.ScaDsca AS Periodo, ISNULL(dbo.TbBan.BanDes, 
                      'DA ASSEGNARE') AS NrBanca
FROM         dbo.TbSca LEFT OUTER JOIN
                      dbo.TbBan ON dbo.TbSca.ScaBan = dbo.TbBan.BanCod LEFT OUTER JOIN
                      dbo.TbPag ON dbo.TbSca.ScaCodPag = dbo.TbPag.PagCod LEFT OUTER JOIN
                      dbo.TbPrk ON dbo.TbSca.ScaRifId = dbo.TbPrk.PrkId AND dbo.TbSca.ScaRifProg = dbo.TbPrk.PrkProg AND dbo.TbSca.ScaRifDA = dbo.TbPrk.PrkDa
                      
                      


GO
/****** Object:  View [dbo].[VDatabase]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[VDatabase]
as
select  distinct s.table_catalog,table_type = case when t.table_type='BASE TABLE' then 'T' else 'V' end,
		s.table_name,s.ordinal_position, s.column_name, s.data_type,sys.length, Chiave.constraint_name,
		Prec = case when s.data_type='Decimal' then s.Numeric_precision else ' ' end, 
		Scala = case when s.data_type='Decimal' then s.Numeric_scale else ' ' end,
		Nullo = case when s.is_nullable='yes' then 'Null' else '' end,
		Replica = case when k.replinfo='1' then 'R' else '' end,
		Id = case when sys.status=128 then 'X' else '' end
		from INFORMATION_SCHEMA.COLUMNS as s  inner join information_schema.tables t on s.table_name=t.table_name
left outer join INFORMATION_SCHEMA.KEY_COLUMN_USAGE as chiave on s.column_name=chiave.column_name 
			and s.table_name=chiave.table_name 
inner join coge.DBO.sysobjects as k on s.table_name=k.name
inner join coge.DBO.syscolumns as sys on s.column_name=sys.name and k.id=sys.id
where s.table_name not like 'sy%' and s.table_name not like 'dt%' and s.table_name not like 'ms%' and s.table_name <> 'TbTabelle'
		and s.table_name <> 'TbTrig' and s.table_name <> 'TbNote' and s.table_name <> 'tmpTbTabelle'
		and isnull(constraint_name,'') not like 'FK_%'

GO
/****** Object:  View [dbo].[VRoutine]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[VRoutine]
as
select text,specific_name,type= case when routine_type='PROCEDURE' then 'P' else 'F' end, specific_catalog,colid
 from INFORMATION_SCHEMA.ROUTINES as inf
		inner join coge.dbo.sysobjects as obj on obj.name=inf.specific_name
		inner join coge.dbo.syscomments as com on com.id=obj.id
where specific_name not like 'dt%'
GO
/****** Object:  View [dbo].[VTrigger]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[VTrigger]
as
select inf.table_catalog,sys.name,nome_trig=obj.name,com.text from sysobjects sys
inner join coge.dbo.sysobjects obj on sys.id=obj.parent_obj
inner join coge.dbo.syscomments com on obj.id=com.id
inner join information_schema.tables inf on inf.table_name=sys.name
where obj.xtype='tr'
GO
/****** Object:  Table [dbo].[ABICABTXT]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ABICABTXT](
	[Col001] [char](1) NULL,
	[Col002] [char](1) NULL,
	[Col003] [int] NULL,
	[Col004] [int] NULL,
	[Col005] [char](179) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BULKPiano]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BULKPiano](
	[NRATA] [float] NULL,
	[DataInizio] [datetime] NULL,
	[rata_sett] [float] NULL,
	[Valore residuo] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CRDistinta]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CRDistinta](
	[Scadenza] [smalldatetime] NOT NULL,
	[Cliente] [varchar](5) NOT NULL,
	[RagioneSociale] [varbinary](60) NOT NULL,
	[Abi] [varchar](5) NOT NULL,
	[Cab] [varchar](5) NOT NULL,
	[Documento] [varchar](35) NOT NULL,
	[Rata] [decimal](11, 2) NOT NULL,
	[Banca] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NEWNEWSCA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NEWNEWSCA](
	[ScaId] [int] NOT NULL,
	[ScaTipoCo] [varchar](1) NOT NULL,
	[ScaConto] [varchar](5) NOT NULL,
	[ScaNdoc] [int] NOT NULL,
	[ScaImpDoc] [decimal](13, 2) NOT NULL,
	[ScaIvaSpe] [decimal](13, 2) NOT NULL,
	[ScaAbi] [int] NOT NULL,
	[ScaCab] [int] NOT NULL,
	[ScaImpRata] [decimal](13, 2) NOT NULL,
	[ScaTPag] [smallint] NOT NULL,
	[ScaCodPag] [smallint] NOT NULL,
	[ScaNRata] [smallint] NOT NULL,
	[ScaBan] [smallint] NOT NULL,
	[ScaDdoc] [smalldatetime] NOT NULL,
	[ScaDsca] [smalldatetime] NOT NULL,
	[ScaRifId] [int] NOT NULL,
	[ScaRifProg] [smallint] NOT NULL,
	[ScaRifDA] [varchar](1) NOT NULL,
	[ScaRAperta] [varchar](1) NOT NULL,
	[ScaImpPagato] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NEWSCA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NEWSCA](
	[ScaId] [int] NOT NULL,
	[ScaTipoCo] [varchar](1) NOT NULL,
	[ScaConto] [varchar](5) NOT NULL,
	[ScaNdoc] [int] NOT NULL,
	[ScaImpDoc] [decimal](13, 2) NOT NULL,
	[ScaIvaSpe] [decimal](13, 2) NOT NULL,
	[ScaAbi] [int] NOT NULL,
	[ScaCab] [int] NOT NULL,
	[ScaImpRata] [decimal](13, 2) NOT NULL,
	[ScaTPag] [smallint] NOT NULL,
	[ScaCodPag] [smallint] NOT NULL,
	[ScaNRata] [smallint] NOT NULL,
	[ScaBan] [smallint] NOT NULL,
	[ScaDdoc] [smalldatetime] NOT NULL,
	[ScaDsca] [smalldatetime] NOT NULL,
	[ScaRifId] [int] NOT NULL,
	[ScaRifProg] [smallint] NOT NULL,
	[ScaRifDA] [varchar](1) NOT NULL,
	[ScaRAperta] [varchar](1) NOT NULL,
	[ScaImpPagato] [decimal](13, 2) NOT NULL,
	[Scadenza] [datetime] NULL,
	[scoperto] [decimal](10, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Piano]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Piano](
	[NRATA] [float] NULL,
	[DataInizio] [datetime] NULL,
	[rata_sett] [decimal](10, 2) NULL,
	[Valore residuo] [decimal](10, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PIANO_CONTI_SAS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PIANO_CONTI_SAS](
	[Colonna 0] [nvarchar](50) NULL,
	[Colonna 1] [nvarchar](255) NULL,
	[Colonna 2] [nvarchar](50) NULL,
	[Colonna 3] [nvarchar](50) NULL,
	[Colonna 4] [nvarchar](50) NULL,
	[Colonna 5] [nvarchar](50) NULL,
	[Colonna 6] [nvarchar](50) NULL,
	[Colonna 7] [nvarchar](50) NULL,
	[Colonna 8] [nvarchar](50) NULL,
	[Colonna 9] [nvarchar](50) NULL,
	[Colonna 10] [nvarchar](50) NULL,
	[Colonna 11] [nvarchar](50) NULL,
	[Colonna 12] [nvarchar](50) NULL,
	[Colonna 13] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PROVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PROVA](
	[CliFor] [varchar](5) NOT NULL,
	[Registro] [smallint] NOT NULL,
	[NumProt] [int] NOT NULL,
	[Codiva] [smallint] NOT NULL,
	[IdPaese] [varchar](2) NULL,
	[AnaPivaEst] [varchar](20) NULL,
	[AnaPiva] [varchar](11) NOT NULL,
	[CodiceFiscale] [varchar](16) NOT NULL,
	[Denominazione] [varchar](60) NOT NULL,
	[Nome] [varchar](30) NOT NULL,
	[Cognome] [varchar](30) NOT NULL,
	[Indirizzo] [varchar](50) NOT NULL,
	[Comune] [varchar](50) NOT NULL,
	[Provincia] [varchar](2) NULL,
	[Nazione] [varchar](2) NULL,
	[TipoDocumento] [varchar](4) NOT NULL,
	[Data] [varchar](10) NOT NULL,
	[Numero] [varchar](61) NULL,
	[ImponibileImporto] [decimal](38, 2) NULL,
	[Imposta] [decimal](38, 2) NULL,
	[Aliquota] [decimal](5, 2) NULL,
	[Natura] [varchar](2) NOT NULL,
	[Detraibile] [decimal](5, 2) NULL,
	[Deducibile] [varchar](1) NULL,
	[EsigibilitaIva] [varchar](1) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rientro]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rientro](
	[IDRIENTRO] [float] NULL,
	[Data_Fattura] [datetime] NULL,
	[Documento] [float] NULL,
	[Importo] [decimal](10, 2) NULL,
	[CCli] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SCOPERTI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCOPERTI](
	[Codice] [nvarchar](255) NULL,
	[Ragione Sociale] [nvarchar](255) NULL,
	[NUMERO] [int] NULL,
	[DATA] [datetime] NULL,
	[IMPORTO] [float] NULL,
	[TipoPagamento] [nvarchar](255) NULL,
	[Scadenza] [datetime] NULL,
	[Cliente di] [nvarchar](255) NULL,
	[Scoperto] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SCOPERTI2022]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCOPERTI2022](
	[Codice] [nvarchar](255) NULL,
	[Ragione Sociale] [nvarchar](255) NULL,
	[NrDoC] [int] NULL,
	[DataDoc] [datetime] NULL,
	[Importo] [float] NULL,
	[TipoPagamento] [nvarchar](255) NULL,
	[Scadenza] [datetime] NULL,
	[Cliente di] [nvarchar](255) NULL,
	[Scoperto] [decimal](10, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SCOPERTI2022_1]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCOPERTI2022_1](
	[Codice] [nvarchar](255) NULL,
	[Ragione Sociale] [nvarchar](255) NULL,
	[NrDoC] [int] NULL,
	[DataDoc] [datetime] NULL,
	[Importo] [decimal](10, 2) NULL,
	[TipoPagamento] [nvarchar](255) NULL,
	[Scadenza] [datetime] NULL,
	[Cliente di] [nvarchar](255) NULL,
	[Scoperto] [decimal](10, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SEMAFORO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SEMAFORO](
	[SEMID] [int] IDENTITY(1,1) NOT NULL,
	[SEMDESC] [varchar](60) NOT NULL,
 CONSTRAINT [PK_SEMAFORO] PRIMARY KEY CLUSTERED 
(
	[SEMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbBilCee]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbBilCee](
	[BILCEECOD] [int] NOT NULL,
	[BILCEEDES] [varchar](100) NOT NULL,
	[BILCEEST] [smallint] NOT NULL,
	[BILCODICE] [varchar](5) NOT NULL,
	[BILDESCR] [varchar](100) NOT NULL,
	[BILCEEDESCEE] [varchar](100) NOT NULL,
	[BILSALDO] [decimal](12, 2) NOT NULL,
	[BILSALDOA] [decimal](12, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBCMVP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBCMVP](
	[VpAnno] [smallint] NOT NULL,
	[VpMese] [smallint] NOT NULL,
	[Vp1] [smallint] NOT NULL,
	[Vp2] [decimal](12, 2) NOT NULL,
	[Vp3] [decimal](12, 2) NOT NULL,
	[Vp4] [decimal](12, 2) NOT NULL,
	[Vp5] [decimal](12, 2) NOT NULL,
	[Vp6] [decimal](12, 2) NOT NULL,
	[Vp7] [decimal](12, 2) NOT NULL,
	[Vp8] [decimal](12, 2) NOT NULL,
	[Vp9] [decimal](12, 2) NOT NULL,
	[Vp10] [decimal](12, 2) NOT NULL,
	[Vp11] [decimal](12, 2) NOT NULL,
	[Vp12] [decimal](12, 2) NOT NULL,
	[Vp13] [decimal](12, 2) NOT NULL,
	[Vp14] [decimal](12, 2) NOT NULL,
	[VpSub] [bit] NOT NULL,
	[VpEventi] [varchar](1) NOT NULL,
	[VpOperazioni] [bit] NOT NULL,
	[VpMetodo] [smallint] NOT NULL,
 CONSTRAINT [PK_TbCmVp] PRIMARY KEY CLUSTERED 
(
	[VpAnno] ASC,
	[VpMese] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCod_FeP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCod_FeP](
	[TIPO] [varchar](2) NOT NULL,
	[COD] [varchar](10) NOT NULL,
	[DESCRIZIONE] [varchar](150) NOT NULL,
	[CODFE] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbCorr]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbCorr](
	[CorrAnno] [smallint] NOT NULL,
	[CorrRegIva] [smallint] NOT NULL,
	[CorrMese] [smallint] NOT NULL,
	[CorrCodIva] [smallint] NOT NULL,
	[CorrConto] [varchar](5) NOT NULL,
	[CorrLordo] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbEleCfXML]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEleCfXML](
	[EleCfAnno] [smallint] NOT NULL,
	[EleCfPeriodo] [tinyint] NOT NULL,
	[EleCfNumProt] [int] NOT NULL,
	[EleCfTipo] [varchar](2) NOT NULL,
	[EleCfCodice] [varchar](5) NOT NULL,
	[EleCfAnaDesc] [varchar](60) NOT NULL,
	[EleCfPriRegIva] [smallint] NOT NULL,
	[EleCfDataDoc] [smalldatetime] NOT NULL,
	[EleCfNumDoc] [int] NOT NULL,
	[EleCfImponibile] [decimal](13, 2) NOT NULL,
	[EleCfIva] [decimal](13, 2) NOT NULL,
	[Totale] [decimal](13, 2) NOT NULL,
	[EleCfRegistrazione] [smalldatetime] NOT NULL,
	[EleCfStato] [varchar](2) NOT NULL,
	[EleCfPartIva] [varchar](16) NOT NULL,
	[EleCfCodFisc] [varchar](16) NOT NULL,
	[EleCfPartIvaE] [bit] NOT NULL,
	[EleCfCodFiscE] [bit] NOT NULL,
	[EleCfEscludi] [bit] NOT NULL,
 CONSTRAINT [PK_TbEleCfXML] PRIMARY KEY CLUSTERED 
(
	[EleCfAnno] ASC,
	[EleCfPeriodo] ASC,
	[EleCfNumProt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbElVar]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbElVar](
	[VaCfPriId] [int] NOT NULL,
	[VaCfDataDoc] [smalldatetime] NULL,
	[VaCfNumDoc] [int] NOT NULL,
	[VaCfImponibile] [nvarchar](1) NOT NULL,
	[VaCfIva] [nvarchar](1) NOT NULL,
	[VaCfAnno] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbEPoE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEPoE](
	[DiCfAnno] [smallint] NOT NULL,
	[DiCfTipo] [nvarchar](2) NOT NULL,
	[DiCfCodice] [nvarchar](5) NOT NULL,
	[DiCognome] [nvarchar](24) NULL,
	[DiNome] [nvarchar](20) NULL,
	[DiDataNasc] [smalldatetime] NULL,
	[DiComune] [nvarchar](40) NULL,
	[DiProv] [nvarchar](2) NULL,
	[DiStato] [nvarchar](3) NULL,
	[DiDenomina] [nvarchar](60) NULL,
	[DiECitta] [nvarchar](40) NULL,
	[DiEStato] [nvarchar](3) NULL,
	[DiEIndiri] [nvarchar](40) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbEse]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEse](
	[EseAnno] [smallint] NOT NULL,
	[EseDal] [smalldatetime] NOT NULL,
	[EseAl] [smalldatetime] NOT NULL,
	[EseBilAp] [varchar](5) NOT NULL,
	[EseBilChi] [varchar](5) NOT NULL,
	[EsePP] [varchar](5) NOT NULL,
	[EseUti] [varchar](5) NOT NULL,
	[EsePer] [varchar](5) NOT NULL,
	[EseProg] [bit] NOT NULL,
	[EseSppp] [bit] NOT NULL,
	[EseClFo] [bit] NOT NULL,
	[EseSdo] [bit] NOT NULL,
	[EseQuote] [bit] NOT NULL,
	[EseGioInt] [bit] NOT NULL,
	[EseGioDesc] [varchar](35) NOT NULL,
	[EseGioNFog] [int] NOT NULL,
	[EseGioProg] [decimal](18, 2) NOT NULL,
	[EseGioArt] [int] NOT NULL,
	[EseFormato] [smallint] NOT NULL,
	[EseCausaleChiusuraConti] [smallint] NOT NULL,
	[EseArtGiroconto] [int] NOT NULL,
	[EseSaDa] [int] NOT NULL,
	[EseSaA] [int] NOT NULL,
	[EseSpDa] [int] NOT NULL,
	[EseSpA] [int] NOT NULL,
	[EseCeDa] [int] NOT NULL,
	[EseCeA] [int] NOT NULL,
	[EseInvNFog] [int] NOT NULL,
	[EseInvDesc] [varchar](35) NOT NULL,
	[EseGContoRCee] [int] NOT NULL,
 CONSTRAINT [PK_TbEse] PRIMARY KEY CLUSTERED 
(
	[EseAnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbEstCf]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbEstCf](
	[EstCfAnno] [smallint] NOT NULL,
	[EstCfProg] [int] NOT NULL,
	[EstCfCodice] [nvarchar](5) NOT NULL,
	[EstCfT3] [int] NOT NULL,
	[EstCfT4] [int] NOT NULL,
	[EstCfT6] [int] NOT NULL,
	[EstCfT7] [int] NOT NULL,
 CONSTRAINT [PK_TbEstCf] PRIMARY KEY CLUSTERED 
(
	[EstCfAnno] ASC,
	[EstCfProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFte_Natura]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFte_Natura](
	[FteNcod] [varchar](4) NOT NULL,
	[FteNdesc] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_TbFte_Natura] PRIMARY KEY CLUSTERED 
(
	[FteNcod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbFte_TipoDoc]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbFte_TipoDoc](
	[FteTdCod] [varchar](4) NOT NULL,
	[FteTdDesc] [varchar](200) NOT NULL,
	[FteTd_Differita] [bit] NOT NULL,
	[FteTd_Libera] [bit] NOT NULL,
	[FteTd_Ncr] [bit] NOT NULL,
 CONSTRAINT [PK_TbFte_TipoDoc] PRIMARY KEY CLUSTERED 
(
	[FteTdCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbGrp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbGrp](
	[GrpCod] [varchar](2) NOT NULL,
	[GrpDesc] [varchar](25) NULL,
	[GrpMigl] [int] NOT NULL,
	[GrpAl1] [int] NOT NULL,
	[GrpAl2] [int] NOT NULL,
	[GrpAl3] [int] NOT NULL,
	[GrpAl4] [int] NOT NULL,
	[GrpAl5] [int] NOT NULL,
	[GrpAl6] [int] NOT NULL,
	[GrpAl7] [int] NOT NULL,
	[GrpAl8] [int] NOT NULL,
	[GrpAl9] [int] NOT NULL,
	[GrpUl1] [int] NOT NULL,
	[GrpUl2] [int] NOT NULL,
	[GrpUl3] [int] NOT NULL,
	[GrpUl4] [int] NOT NULL,
	[GrpUl5] [int] NOT NULL,
	[GrpUl6] [int] NOT NULL,
	[GrpUl7] [int] NOT NULL,
	[GrpUl8] [int] NOT NULL,
	[GrpUl9] [int] NOT NULL,
	[GrpCpt1] [varchar](5) NOT NULL,
	[GrpCpt2] [varchar](5) NOT NULL,
	[GrpCpt3] [varchar](5) NOT NULL,
	[GrpCpt4] [varchar](5) NOT NULL,
	[GrpCpt5] [varchar](5) NOT NULL,
	[GrpCpt6] [varchar](5) NOT NULL,
	[GrpCpt7] [varchar](5) NOT NULL,
	[GrpCpt8] [varchar](5) NOT NULL,
	[GrpCpt9] [varchar](5) NOT NULL,
	[GrpBloccato] [bit] NOT NULL,
 CONSTRAINT [PK_TbGrp] PRIMARY KEY CLUSTERED 
(
	[GrpCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBIDP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBIDP](
	[IDPRI] [int] IDENTITY(1,1) NOT NULL,
	[IDDATA] [smalldatetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBINCMI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBINCMI](
	[IcmAnno] [smallint] NOT NULL,
	[IcmTrim] [smallint] NOT NULL,
	[IcmLock] [bit] NOT NULL,
	[IcmDrCodFisc] [nvarchar](16) NULL,
	[IcmDrCaf] [bit] NOT NULL,
	[IcmDrImpegno] [smalldatetime] NULL,
	[IcmProg] [smallint] NOT NULL,
 CONSTRAINT [PK_TbInCmi] PRIMARY KEY CLUSTERED 
(
	[IcmAnno] ASC,
	[IcmTrim] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbInEle]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbInEle](
	[IeAnno] [smallint] NOT NULL,
	[IeTipo] [nvarchar](50) NOT NULL,
	[IeLock] [bit] NOT NULL,
	[IeDrCodFisc] [nvarchar](16) NULL,
	[IeDrCaf] [nvarchar](5) NULL,
	[IeDrImpegno] [smalldatetime] NULL,
 CONSTRAINT [PK_TbInEle_1] PRIMARY KEY CLUSTERED 
(
	[IeAnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbInP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbInP](
	[InPCliCons] [varchar](5) NOT NULL,
	[InPData] [smalldatetime] NOT NULL,
	[InPCodArt] [smallint] NOT NULL,
	[InPImballo] [smallint] NOT NULL,
	[InPUm] [varchar](2) NOT NULL,
	[InPQta] [decimal](9, 3) NOT NULL,
 CONSTRAINT [PK_TbInP] PRIMARY KEY CLUSTERED 
(
	[InPCliCons] ASC,
	[InPData] ASC,
	[InPCodArt] ASC,
	[InPImballo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbintPF]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbintPF](
	[PfClifor] [varchar](5) NOT NULL,
	[PfAnaDesc] [varchar](60) NOT NULL,
	[PfCognome] [varchar](30) NOT NULL,
	[PfNome] [varchar](30) NOT NULL,
	[PfTipo] [varchar](2) NOT NULL,
	[PFPiva] [varchar](20) NOT NULL,
	[PFCFis] [varchar](16) NOT NULL,
 CONSTRAINT [PK_TbintPF] PRIMARY KEY CLUSTERED 
(
	[PfClifor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbIvaV]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbIvaV](
	[IvaVAnno] [smallint] NOT NULL,
	[IvaVRegIva] [smallint] NOT NULL,
	[IvaVMese] [smallint] NOT NULL,
	[IvaVCodIva] [smallint] NOT NULL,
	[IvaVAcLordi] [decimal](13, 2) NOT NULL,
	[IvaVPerComp] [decimal](13, 10) NOT NULL,
	[IvaVLordi] [decimal](13, 2) NOT NULL,
	[IvaVCMPIva] [smallint] NOT NULL,
	[IvaVNetti] [decimal](13, 2) NOT NULL,
	[IvaVIva] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPaCii]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPaCii](
	[PaTipo] [varchar](3) NOT NULL,
	[PaCodiva] [smallint] NOT NULL,
	[PaDesciva] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPaesi]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPaesi](
	[PaCod] [varchar](3) NOT NULL,
	[PaSigla] [varchar](2) NOT NULL,
	[PaDesc] [varchar](50) NOT NULL,
	[PaIntra] [bit] NOT NULL,
 CONSTRAINT [PK_TbPaesi] PRIMARY KEY CLUSTERED 
(
	[PaCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBPIR]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBPIR](
	[IDPIR] [int] IDENTITY(1,1) NOT NULL,
	[IDDATA] [smalldatetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPos]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPos](
	[PosId] [tinyint] NOT NULL,
	[PosCauInsoluti] [smallint] NOT NULL,
	[PosNomeModulo] [nvarchar](30) NOT NULL,
	[PosTesto] [nvarchar](400) NOT NULL,
	[PosPiede] [nvarchar](200) NOT NULL,
	[PosUltima] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPrintIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPrintIva](
	[PRINTIVAPriId] [int] NOT NULL,
	[PRINTIVAPriRegIva] [smallint] NOT NULL,
	[PRINTIVAFinoAl] [smalldatetime] NULL,
 CONSTRAINT [PK_TbPrintIva] PRIMARY KEY CLUSTERED 
(
	[PRINTIVAPriId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPrkCoge]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPrkCoge](
	[PrkRiga] [int] NOT NULL,
	[PrkDa] [varchar](1) NOT NULL,
	[PrkAggCon] [varchar](1) NOT NULL,
	[PrkLinea] [varchar](3) NOT NULL,
	[PrkTipoConto] [varchar](1) NOT NULL,
	[PrkConto] [varchar](5) NOT NULL,
	[PrkAammgg] [smalldatetime] NOT NULL,
	[PrkGStampa] [varchar](1) NOT NULL,
	[PrkGaammgg] [smalldatetime] NOT NULL,
	[PrkGArtProt] [varchar](6) NOT NULL,
	[PrkGBis] [varchar](1) NOT NULL,
	[PrkGRegIva] [varchar](2) NOT NULL,
	[PrkGda] [varchar](1) NOT NULL,
	[PrkNRegIva] [varchar](2) NOT NULL,
	[PrkNprot] [varchar](6) NOT NULL,
	[PrkNBr] [varchar](1) NOT NULL,
	[PrkPlinea] [varchar](3) NOT NULL,
	[PrkPTipoConto] [varchar](1) NOT NULL,
	[PrkPConto] [varchar](5) NOT NULL,
	[PrkPApCh] [varchar](1) NOT NULL,
	[PrkPDocEst] [varchar](6) NOT NULL,
	[PrkPDocAnn] [varchar](4) NOT NULL,
	[PrkArtFisc] [int] NOT NULL,
 CONSTRAINT [PK_TbPrk] PRIMARY KEY CLUSTERED 
(
	[PrkRiga] ASC,
	[PrkDa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbRegXML_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRegXML_EST](
	[RxAnno] [smallint] NOT NULL,
	[RxTipo] [varchar](1) NOT NULL,
	[RxRegistro] [smallint] NOT NULL,
	[RxPeriodo] [smallint] NOT NULL,
	[RxDal] [smalldatetime] NULL,
	[RxAl] [smalldatetime] NULL,
	[RxProgInvio] [int] NOT NULL,
	[RxSel] [bit] NOT NULL,
	[RxInviato] [bit] NOT NULL,
	[RxNoControl] [bit] NOT NULL,
 CONSTRAINT [PK_TbRegXML_EST] PRIMARY KEY CLUSTERED 
(
	[RxAnno] ASC,
	[RxTipo] ASC,
	[RxRegistro] ASC,
	[RxPeriodo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbRieCf]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbRieCf](
	[RieCfAnno] [smallint] NOT NULL,
	[RieCfTipo] [nvarchar](2) NOT NULL,
	[RieCfCodice] [nvarchar](5) NOT NULL,
	[RieCfPiva] [nvarchar](16) NOT NULL,
	[RieCfCFis] [nvarchar](16) NOT NULL,
	[RieCfImponibile] [decimal](13, 2) NOT NULL,
	[RieCfIva] [decimal](13, 2) NOT NULL,
	[RieCfNonImp] [decimal](13, 2) NOT NULL,
	[RieCfEsente] [decimal](13, 2) NOT NULL,
	[RieCfAltri] [decimal](13, 2) NOT NULL,
	[RieCfAnadesc] [nvarchar](60) NOT NULL,
	[RieCfTotale] [decimal](13, 2) NOT NULL,
	[RieCfNCredito] [bit] NOT NULL,
	[RieCfNFatture] [smallint] NOT NULL,
	[RieCfEscludi] [bit] NOT NULL,
 CONSTRAINT [PK_TbRieCf] PRIMARY KEY CLUSTERED 
(
	[RieCfAnno] ASC,
	[RieCfTipo] ASC,
	[RieCfCodice] ASC,
	[RieCfNCredito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbScaDUP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbScaDUP](
	[ScaId] [int] NOT NULL,
	[ScaTipoCo] [varchar](1) NOT NULL,
	[ScaConto] [varchar](5) NOT NULL,
	[ScaNdoc] [int] NOT NULL,
	[ScaImpDoc] [decimal](13, 2) NOT NULL,
	[ScaIvaSpe] [decimal](13, 2) NOT NULL,
	[ScaAbi] [int] NOT NULL,
	[ScaCab] [int] NOT NULL,
	[ScaImpRata] [decimal](13, 2) NOT NULL,
	[ScaTPag] [smallint] NOT NULL,
	[ScaCodPag] [smallint] NOT NULL,
	[ScaNRata] [smallint] NOT NULL,
	[ScaBan] [smallint] NOT NULL,
	[ScaDdoc] [smalldatetime] NOT NULL,
	[ScaDsca] [smalldatetime] NOT NULL,
	[ScaRifId] [int] NOT NULL,
	[ScaRifProg] [smallint] NOT NULL,
	[ScaRifDA] [varchar](1) NOT NULL,
	[ScaRAperta] [varchar](1) NOT NULL,
	[ScaImpPagato] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSpeCf]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSpeCf](
	[SpeCfAnno] [smallint] NOT NULL,
	[SpeCfProg] [int] NOT NULL,
	[SpeCfPiva] [nvarchar](16) NOT NULL,
	[SpeCfCFis] [nvarchar](16) NOT NULL,
	[SpeCfRie] [nvarchar](1) NOT NULL,
	[SpeCfNoa] [int] NOT NULL,
	[SpeCfNop] [int] NOT NULL,
	[SpeCfT7] [int] NOT NULL,
	[SpeCfT8] [int] NOT NULL,
	[SpeCfT9] [int] NOT NULL,
	[SpeCfT10] [int] NOT NULL,
	[SpeCfT11] [int] NOT NULL,
	[SpeCfT12] [int] NOT NULL,
	[SpeCfT13] [int] NOT NULL,
	[SpeCfT14] [int] NOT NULL,
	[SpeCfT15] [int] NOT NULL,
	[SpeCfT16] [int] NOT NULL,
 CONSTRAINT [PK_TbSpeCf] PRIMARY KEY CLUSTERED 
(
	[SpeCfAnno] ASC,
	[SpeCfProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbSup]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSup](
	[SupCod] [smallint] NOT NULL,
	[SupDesc] [varchar](40) NOT NULL,
	[SupDesc2] [varchar](40) NOT NULL,
	[SupDesc3] [varchar](40) NOT NULL,
	[SupDesc4] [varchar](40) NOT NULL,
	[SupDesc5] [varchar](40) NOT NULL,
 CONSTRAINT [PK_TbSup] PRIMARY KEY CLUSTERED 
(
	[SupCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TM]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TM](
	[Codice] [nvarchar](255) NULL,
	[Ragione Sociale] [nvarchar](255) NULL,
	[NrDoc] [float] NULL,
	[DataDoc] [datetime] NULL,
	[ImportoDoc] [money] NULL,
	[TipoPagamento] [nvarchar](255) NULL,
	[Scadenza] [datetime] NULL,
	[Cliente di] [nvarchar](255) NULL,
	[Scoperto] [money] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmp3]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmp3](
	[RowNumber] [bigint] NULL,
	[codice] [nvarchar](255) NULL,
	[Ragione Sociale] [nvarchar](255) NULL,
	[NrDoC] [int] NULL,
	[DataDoc] [datetime] NULL,
	[Importo] [decimal](10, 2) NULL,
	[TIPOPAGAMENTO] [nvarchar](255) NULL,
	[SCOP] [decimal](38, 2) NULL,
	[CodPag] [smallint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPAMMLIB]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPAMMLIB](
	[AmmLibBlock] [int] NOT NULL,
	[AmmLibCat] [varchar](2) NOT NULL,
	[AmmLibPerc] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_TMPAMMLIB] PRIMARY KEY CLUSTERED 
(
	[AmmLibCat] ASC,
	[AmmLibBlock] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPANNC]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPANNC](
	[TMCBLOCK] [int] NOT NULL,
	[TMCCONTO] [varchar](5) NULL,
	[TMCDESC] [varchar](90) NULL,
	[TMCFLAG] [smallint] NULL,
	[TMCSALDO] [decimal](13, 2) NULL,
	[TMCCPT] [varchar](5) NULL,
	[TMCCAUS] [smallint] NULL,
	[TMCTIPO] [smallint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPBINV]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPBINV](
	[TMSBLOCK] [int] NOT NULL,
	[TMSCLASSE] [smallint] NOT NULL,
	[TMSCONTO] [varchar](5) NOT NULL,
	[TMSDCLFO] [varchar](60) NOT NULL,
	[TMSTIPOCO] [smallint] NOT NULL,
	[TMSCODCO] [varchar](5) NOT NULL,
	[TMSDESC] [varchar](60) NOT NULL,
	[TMSDARE] [decimal](13, 2) NOT NULL,
	[TMSAVERE] [decimal](13, 2) NOT NULL,
	[TMSMASTRO] [varchar](2) NOT NULL,
	[TMSDEMAS] [varchar](60) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPBPART]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPBPART](
	[TMBCONTO] [varchar](5) NOT NULL,
	[TMBDESC] [varchar](60) NOT NULL,
	[TMBSALDO] [decimal](13, 2) NOT NULL,
	[TMBDOCAN] [smallint] NOT NULL,
	[TMBDOCEST] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPCEE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPCEE](
	[TMPSID] [int] IDENTITY(1,1) NOT NULL,
	[TMPS1CODICE] [varchar](5) NOT NULL,
	[TMPS1DESC] [varchar](100) NOT NULL,
	[TMPS1SALDO] [decimal](12, 2) NOT NULL,
	[TMPS2CODICE] [varchar](5) NOT NULL,
	[TMPS2DESC] [varchar](100) NOT NULL,
	[TMPS2SALDO] [decimal](12, 2) NOT NULL,
	[TMPS3CODICE] [varchar](5) NOT NULL,
	[TMPS3DESC] [varchar](100) NOT NULL,
	[TMPS3SALDO] [decimal](12, 2) NOT NULL,
	[TMPS4CODICE] [varchar](5) NOT NULL,
	[TMPS4DESC] [varchar](100) NOT NULL,
	[TMPS4SALDO] [decimal](12, 2) NOT NULL,
	[TMPSPPP] [smallint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPCORR]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPCORR](
	[TCorrId] [int] NOT NULL,
	[TCorrAnno] [smallint] NOT NULL,
	[TCorrRegIva] [smallint] NOT NULL,
	[TCorrMese] [smallint] NOT NULL,
	[TCorrCodIva] [smallint] NOT NULL,
	[TCorrConto] [varchar](5) NOT NULL,
	[TCorrLordo] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPCORS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPCORS](
	[CorrAnno] [smallint] NOT NULL,
	[CorrRegIva] [smallint] NOT NULL,
	[CorrMese] [smallint] NOT NULL,
	[CorrCodIva] [smallint] NOT NULL,
	[CorrConto] [varchar](5) NOT NULL,
	[CorrLordo] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPDICIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPDICIVA](
	[BSCESSIONI] [decimal](10, 2) NULL,
	[BSACQUISTI] [decimal](10, 2) NULL,
	[LEASING] [decimal](10, 2) NULL,
	[MERCI] [decimal](10, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPDIFF]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPDIFF](
	[TMPCONTO] [varchar](5) NOT NULL,
	[TMPANAG] [varchar](60) NOT NULL,
	[TMPDIFFC] [decimal](10, 2) NOT NULL,
	[TMPDIFFP] [decimal](10, 2) NOT NULL,
	[TMPDIFFS] [decimal](10, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPGIO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPGIO](
	[PriId] [int] NOT NULL,
	[PriProg] [smallint] NOT NULL,
	[PriDataGio] [smalldatetime] NOT NULL,
	[PriCausale] [smallint] NOT NULL,
	[PriCoDare] [varchar](5) NOT NULL,
	[DAREDESC] [varchar](60) NOT NULL,
	[PriCoAvere] [varchar](5) NOT NULL,
	[AVEREDESC] [varchar](60) NOT NULL,
	[PriImpDare] [decimal](13, 2) NOT NULL,
	[PriImpAvere] [decimal](13, 2) NOT NULL,
	[PriDesc] [varchar](24) NOT NULL,
	[PriNumProt] [int] NOT NULL,
	[PriBisRet] [varchar](1) NOT NULL,
	[PriCodIva] [smallint] NOT NULL,
	[PriRegIva] [smallint] NOT NULL,
	[PriDocEst] [int] NOT NULL,
	[PriDataEst] [smalldatetime] NOT NULL,
	[PriDescB] [varchar](32) NOT NULL,
	[PriNsRif] [varchar](7) NOT NULL,
	[PriArtFisc] [int] NOT NULL,
	[PriGStampa] [bit] NOT NULL,
	[RigheD] [smallint] NOT NULL,
	[RigheA] [smallint] NOT NULL,
	[TIPOREG] [smallint] NOT NULL,
	[RETTIF] [bit] NOT NULL,
 CONSTRAINT [PK_TMPGIO] PRIMARY KEY CLUSTERED 
(
	[PriId] ASC,
	[PriProg] ASC,
	[PriDataGio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPINTRA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPINTRA](
	[PROG] [int] IDENTITY(1,1) NOT NULL,
	[PRICODARE] [varchar](5) NOT NULL,
	[ANADESC] [varchar](60) NOT NULL,
	[PAESE] [varchar](2) NOT NULL,
	[PIVA] [varchar](18) NOT NULL,
	[IMPONIBILE] [decimal](13, 2) NOT NULL,
	[TOTALP] [decimal](13, 2) NOT NULL,
	[TOTALT] [decimal](13, 2) NOT NULL,
 CONSTRAINT [PK_TMPINTRA] PRIMARY KEY CLUSTERED 
(
	[PROG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPLOCK]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPLOCK](
	[IdPrNota] [int] NOT NULL,
 CONSTRAINT [PK_tmplock] PRIMARY KEY CLUSTERED 
(
	[IdPrNota] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPPF]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPPF](
	[PfAnno] [smallint] NOT NULL,
	[PfPeriodo] [tinyint] NOT NULL,
	[PfClifor] [varchar](5) NOT NULL,
	[PfAnaDesc] [varchar](60) NOT NULL,
	[PfCognome] [varchar](30) NOT NULL,
	[PfNome] [varchar](30) NOT NULL,
	[PfTipo] [varchar](2) NOT NULL,
	[PFPiva] [varchar](20) NOT NULL,
	[PFCFis] [varchar](16) NOT NULL,
 CONSTRAINT [PK_TMPPF] PRIMARY KEY CLUSTERED 
(
	[PfAnno] ASC,
	[PfPeriodo] ASC,
	[PfClifor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPPIANO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPPIANO](
	[IDGRUPPO] [int] NOT NULL,
	[IDRATA] [int] NOT NULL,
	[IDDATAFAT] [smalldatetime] NOT NULL,
	[IDNRFAT] [int] NOT NULL,
	[IDRATACLI] [int] NOT NULL,
	[IDIMPORTORATA] [decimal](10, 2) NOT NULL,
	[IDIMPORTOFAT] [decimal](10, 2) NOT NULL,
	[IDRESIDUOCLI] [decimal](10, 2) NOT NULL,
	[IDCCLIE] [varchar](5) NOT NULL,
	[DATASCADENZA] [smalldatetime] NULL,
	[RATACONCORDATA] [decimal](10, 2) NOT NULL,
	[DATACREAZIONE] [smalldatetime] NULL,
	[DATACREASCADENZE] [smalldatetime] NULL,
	[IDANAGRAFICA] [varchar](80) NULL,
	[IDSEQUENZA] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmppri2019]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmppri2019](
	[PriId] [int] NOT NULL,
	[PriProg] [smallint] NOT NULL,
	[PriDataGio] [smalldatetime] NOT NULL,
	[PriCausale] [smallint] NOT NULL,
	[PriCoDare] [varchar](5) NOT NULL,
	[PriCoAvere] [varchar](5) NOT NULL,
	[PriNumProt] [int] NOT NULL,
	[PriBisRet] [varchar](1) NOT NULL,
	[PriCodIva] [smallint] NOT NULL,
	[PriRegIva] [smallint] NOT NULL,
	[PriImpDare] [decimal](13, 2) NOT NULL,
	[PriImpAvere] [decimal](13, 2) NOT NULL,
	[PriDesc] [varchar](24) NOT NULL,
	[PriDocEst] [int] NOT NULL,
	[PriMeseSk] [varchar](1) NOT NULL,
	[PriDataEst] [smalldatetime] NOT NULL,
	[PriDescB] [varchar](32) NOT NULL,
	[PriFl04] [smallint] NOT NULL,
	[PriFl05] [smallint] NOT NULL,
	[PriFl06] [smallint] NOT NULL,
	[PriNsRif] [varchar](7) NOT NULL,
	[PriSos] [varchar](5) NOT NULL,
	[PriLinea] [varchar](3) NOT NULL,
	[PriDocAnn] [smallint] NOT NULL,
	[PriCodPag] [smallint] NOT NULL,
	[PriValuta] [decimal](13, 2) NOT NULL,
	[PriArtFisc] [int] NOT NULL,
	[PriIvaPrint] [bit] NOT NULL,
	[PriGStampa] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPREGQUO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPREGQUO](
	[RegQuoNum] [int] NOT NULL,
	[RegQuoCat] [varchar](2) NOT NULL,
	[RegQuoAnnoA] [smallint] NOT NULL,
	[RegQuoTipoAmm] [smallint] NOT NULL,
	[RegQuoAliTab] [decimal](5, 2) NOT NULL,
	[RegQuoCoAmm] [decimal](13, 2) NOT NULL,
	[RegQuoQuota] [decimal](13, 2) NOT NULL,
	[RegQuoFondo] [decimal](13, 2) NOT NULL,
	[RegQuoResiduo] [decimal](13, 2) NOT NULL,
	[RegQuoQuotaND] [decimal](13, 2) NOT NULL,
	[RegQuoDescr] [varchar](110) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPSCAPIANO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPSCAPIANO](
	[IDGRUPPO] [int] NOT NULL,
	[IDRATA] [int] NOT NULL,
	[IDDATAFAT] [smalldatetime] NOT NULL,
	[IDNRFAT] [int] NOT NULL,
	[IDRATACLI] [int] NOT NULL,
	[IDIMPORTORATA] [decimal](10, 2) NOT NULL,
	[IDIMPORTOFAT] [decimal](10, 2) NOT NULL,
	[IDRESIDUOCLI] [decimal](10, 2) NOT NULL,
	[IDCCLIE] [varchar](5) NOT NULL,
	[DATASCADENZA] [smalldatetime] NULL,
	[RATACONCORDATA] [decimal](10, 2) NOT NULL,
	[DATACREAZIONE] [smalldatetime] NULL,
	[DATACREASCADENZE] [smalldatetime] NULL,
	[IDANAGRAFICA] [varchar](80) NULL,
	[IDSEQUENZA] [smallint] NOT NULL,
	[ScaId] [int] NOT NULL,
	[ScaTipoCo] [varchar](1) NOT NULL,
	[ScaConto] [varchar](5) NOT NULL,
	[ScaNdoc] [int] NOT NULL,
	[ScaImpDoc] [decimal](13, 2) NOT NULL,
	[ScaIvaSpe] [decimal](13, 2) NOT NULL,
	[ScaAbi] [int] NOT NULL,
	[ScaCab] [int] NOT NULL,
	[ScaImpRata] [decimal](13, 2) NOT NULL,
	[ScaTPag] [smallint] NOT NULL,
	[ScaCodPag] [smallint] NOT NULL,
	[ScaNRata] [smallint] NOT NULL,
	[ScaBan] [smallint] NOT NULL,
	[ScaDdoc] [smalldatetime] NOT NULL,
	[ScaDsca] [smalldatetime] NOT NULL,
	[ScaRifId] [int] NOT NULL,
	[ScaRifProg] [smallint] NOT NULL,
	[ScaRifDA] [varchar](1) NOT NULL,
	[ScaRAperta] [varchar](1) NOT NULL,
	[ScaImpPagato] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPSEZ]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPSEZ](
	[TMPSID] [int] IDENTITY(1,1) NOT NULL,
	[TMPS1CODICE] [varchar](5) NOT NULL,
	[TMPS1DESC] [varchar](50) NOT NULL,
	[TMPS1SALDO] [decimal](12, 2) NOT NULL,
	[TMPS2CODICE] [varchar](50) NOT NULL,
	[TMPS2DESC] [varchar](50) NOT NULL,
	[TMPS2SALDO] [decimal](12, 2) NOT NULL,
	[TMPSPPP] [smallint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPTBPRI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPTBPRI](
	[PriTemp] [int] NOT NULL,
	[PriProg] [smallint] NOT NULL,
	[PriDataGio] [smalldatetime] NOT NULL,
	[PriCausale] [smallint] NOT NULL,
	[PriCoDare] [varchar](5) NOT NULL,
	[PriCoAvere] [varchar](5) NOT NULL,
	[PriNumProt] [int] NOT NULL,
	[PriBisRet] [varchar](1) NOT NULL,
	[PriCodIva] [smallint] NOT NULL,
	[PriRegIva] [smallint] NOT NULL,
	[PriImpDare] [decimal](13, 2) NOT NULL,
	[PriImpAvere] [decimal](13, 2) NOT NULL,
	[PriDesc] [varchar](24) NOT NULL,
	[PriDocEst] [int] NOT NULL,
	[PriMeseSk] [varchar](1) NOT NULL,
	[PriDataEst] [smalldatetime] NOT NULL,
	[PriDescB] [varchar](32) NOT NULL,
	[PriFl04] [smallint] NOT NULL,
	[PriFl05] [smallint] NOT NULL,
	[PriFl06] [smallint] NOT NULL,
	[PriNsRif] [varchar](7) NOT NULL,
	[PriSos] [varchar](1) NOT NULL,
	[PriLinea] [varchar](3) NOT NULL,
	[PriDocAnn] [smallint] NOT NULL,
	[PriCodPag] [smallint] NOT NULL,
	[PriValuta] [varchar](1) NOT NULL,
	[PriArtFisc] [int] NOT NULL,
	[PriGStampa] [bit] NOT NULL,
	[PriPAperta] [bit] NOT NULL,
 CONSTRAINT [PK_TMPTbPri] PRIMARY KEY CLUSTERED 
(
	[PriTemp] ASC,
	[PriProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmptotale]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmptotale](
	[A] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPVENTILA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPVENTILA](
	[IvaVAnno] [smallint] NOT NULL,
	[IvaVRegIva] [smallint] NOT NULL,
	[IvaVCodIva] [smallint] NOT NULL,
	[IvaVAcLordi] [decimal](13, 2) NOT NULL,
	[IvaVPerComp] [decimal](13, 10) NOT NULL,
	[IvaVLordi] [decimal](13, 2) NOT NULL,
	[IvaVCMPIva] [smallint] NOT NULL,
	[IvaVNetti] [decimal](13, 2) NOT NULL,
	[IvaVIva] [decimal](13, 2) NOT NULL,
	[IvaDes] [varchar](12) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMRLOCK]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMRLOCK](
	[IdPrNota] [int] NOT NULL,
 CONSTRAINT [PK_tmRlock] PRIMARY KEY CLUSTERED 
(
	[IdPrNota] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMSCOP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMSCOP](
	[Codice] [nvarchar](255) NULL,
	[Ragione Sociale] [nvarchar](255) NULL,
	[NrDoc] [int] NULL,
	[DataDoc] [datetime] NULL,
	[ImportoDoc] [money] NULL,
	[TipoPagamento] [nvarchar](255) NULL,
	[Scadenza] [datetime] NULL,
	[Cliente di] [nvarchar](255) NULL,
	[Scoperto] [money] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRIDP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRIDP](
	[IDPRI] [int] IDENTITY(1,1) NOT NULL,
	[IDDATA] [smalldatetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRRET]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRRET](
	[RETID] [int] NOT NULL,
	[RETARTICOLO] [int] NOT NULL,
	[RETDATA] [smalldatetime] NOT NULL,
	[RETDESCR] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_TRRET] PRIMARY KEY CLUSTERED 
(
	[RETID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_TbEff]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbEff] ON [dbo].[TbEff]
(
	[RicRifFat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TbPri]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbPri] ON [dbo].[TbPri]
(
	[PriDocAnn] ASC,
	[PriDocEst] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TbPri_1]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbPri_1] ON [dbo].[TbPri]
(
	[PriNumProt] ASC,
	[PriBisRet] ASC,
	[PriRegIva] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TbPrk]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbPrk] ON [dbo].[TbPrk]
(
	[PrkTipoCo] ASC,
	[PrkConto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TbPrk_1]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbPrk_1] ON [dbo].[TbPrk]
(
	[PrkConto] ASC,
	[PrkDocAnn] ASC,
	[PrkDocEst] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TbSca]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbSca] ON [dbo].[TbSca]
(
	[ScaRifId] ASC,
	[ScaRifProg] ASC,
	[ScaRifDA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TbSca_1]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbSca_1] ON [dbo].[TbSca]
(
	[ScaDsca] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TbSca_2]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TbSca_2] ON [dbo].[TbSca]
(
	[ScaConto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TMPREGIVA]    Script Date: 20/05/2026 10:24:42 ******/
CREATE NONCLUSTERED INDEX [IX_TMPREGIVA] ON [dbo].[TMPREGIVA]
(
	[PRegId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SEMAFORO] ADD  CONSTRAINT [DF_SEMAFORO_SEMDESC]  DEFAULT ('?') FOR [SEMDESC]
GO
ALTER TABLE [dbo].[TbArtP] ADD  CONSTRAINT [DF_TbArtP_ArtPDesc2]  DEFAULT ('') FOR [ArtPDesc2]
GO
ALTER TABLE [dbo].[TbCab] ADD  CONSTRAINT [DF_TbCab_CaPaese]  DEFAULT ('IT') FOR [CaPaese]
GO
ALTER TABLE [dbo].[TbCab] ADD  CONSTRAINT [DF_TbCab_CaBic]  DEFAULT ('') FOR [CaBic]
GO
ALTER TABLE [dbo].[TbCesp] ADD  CONSTRAINT [DF_TbCesp_CespContoQuotaAnt]  DEFAULT ('00.00') FOR [CespContoQuotaAnt]
GO
ALTER TABLE [dbo].[TbCesp] ADD  CONSTRAINT [DF_TbCesp_CespCp]  DEFAULT (0) FOR [CespCp]
GO
ALTER TABLE [dbo].[TbCii] ADD  CONSTRAINT [DF_TbCii_TbAli]  DEFAULT (0) FOR [CiiAli]
GO
ALTER TABLE [dbo].[TbCii] ADD  CONSTRAINT [DF_TbCii_TbInd]  DEFAULT (0) FOR [CiiInd]
GO
ALTER TABLE [dbo].[TbCii] ADD  CONSTRAINT [DF_TbCii_TbTp]  DEFAULT (0) FOR [CiiTp]
GO
ALTER TABLE [dbo].[TbCii] ADD  CONSTRAINT [DF_TbCii_TbCmp]  DEFAULT (0) FOR [CiiCmp]
GO
ALTER TABLE [dbo].[TbCii] ADD  DEFAULT ('') FOR [CiiNatura]
GO
ALTER TABLE [dbo].[TbCii] ADD  DEFAULT ((0)) FOR [CiiSoggBollo]
GO
ALTER TABLE [dbo].[TBCMVP] ADD  DEFAULT ((0)) FOR [VpOperazioni]
GO
ALTER TABLE [dbo].[TBCMVP] ADD  DEFAULT ((0)) FOR [VpMetodo]
GO
ALTER TABLE [dbo].[TbCsp] ADD  CONSTRAINT [DF_TbCsp_CspCp]  DEFAULT (0) FOR [CspCp]
GO
ALTER TABLE [dbo].[TbCsp] ADD  CONSTRAINT [DF_TbCsp_CspCespiti]  DEFAULT ('00.00') FOR [CspCespiti]
GO
ALTER TABLE [dbo].[TbCsp] ADD  CONSTRAINT [DF_TbCsp_CspFondoAmm]  DEFAULT ('00.00') FOR [CspFondoAmm]
GO
ALTER TABLE [dbo].[TbCsp] ADD  CONSTRAINT [DF_TbCsp_CspQuotaNormale]  DEFAULT ('00.00') FOR [CspQuotaNormale]
GO
ALTER TABLE [dbo].[TbCsp] ADD  CONSTRAINT [DF_TbCsp_CspQuotaAnticipata]  DEFAULT ('00.00') FOR [CspQuotaAnticipata]
GO
ALTER TABLE [dbo].[TbDDiCl] ADD  CONSTRAINT [DF_TbDDiCl_DDiCNProgDichiar]  DEFAULT ((0)) FOR [DDiCNProgDichiar]
GO
ALTER TABLE [dbo].[TbDDiCl] ADD  CONSTRAINT [DF_TbDDiCl_DDiCCodEse]  DEFAULT ((0)) FOR [DDiCCodEse]
GO
ALTER TABLE [dbo].[TbDDiCl] ADD  CONSTRAINT [DF_TbDDiCl_DDiCRegBollo]  DEFAULT ((0)) FOR [DDiCRegBollo]
GO
ALTER TABLE [dbo].[TbDDiCl] ADD  CONSTRAINT [DF_TbDDiCl_DDiCUPag]  DEFAULT ((0)) FOR [DDiCUPag]
GO
ALTER TABLE [dbo].[TbDDiCl] ADD  DEFAULT ((0)) FOR [DDiImporto]
GO
ALTER TABLE [dbo].[TbDDiCl] ADD  DEFAULT ('') FOR [DDiProtocollo]
GO
ALTER TABLE [dbo].[TbEleCf] ADD  CONSTRAINT [DF_TbEleCf_EleCfUnion]  DEFAULT (0) FOR [EleCfUnion]
GO
ALTER TABLE [dbo].[TbEleCf] ADD  CONSTRAINT [DF_TbEleCf_EleCfModPag]  DEFAULT ('N') FOR [EleCfModPag]
GO
ALTER TABLE [dbo].[TbEleCf] ADD  CONSTRAINT [DF_TbEleCf_EleCfVariazione]  DEFAULT (0) FOR [EleCfVariazione]
GO
ALTER TABLE [dbo].[TbElVar] ADD  CONSTRAINT [DF_TbElVar_VaCfNumDoc]  DEFAULT (0) FOR [VaCfNumDoc]
GO
ALTER TABLE [dbo].[TbElVar] ADD  CONSTRAINT [DF_TbElVar_VaCfImponibile]  DEFAULT ('') FOR [VaCfImponibile]
GO
ALTER TABLE [dbo].[TbElVar] ADD  CONSTRAINT [DF_TbElVar_VaCfIva]  DEFAULT ('') FOR [VaCfIva]
GO
ALTER TABLE [dbo].[TbElVar] ADD  CONSTRAINT [DF_TbElVar_VaCfAnno]  DEFAULT ((2010)) FOR [VaCfAnno]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseProg]  DEFAULT (1) FOR [EseProg]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseSppp]  DEFAULT (1) FOR [EseSppp]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseClFo]  DEFAULT (1) FOR [EseClFo]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseSdo]  DEFAULT (1) FOR [EseSdo]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseQuote]  DEFAULT (1) FOR [EseQuote]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseGioInt]  DEFAULT (0) FOR [EseGioInt]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseGioDesc]  DEFAULT ('') FOR [EseGioDesc]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseGioNFog]  DEFAULT (0) FOR [EseGioNFog]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseGioProg]  DEFAULT (0) FOR [EseGioProg]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseGioArt]  DEFAULT (0) FOR [EseGioArt]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseFormato]  DEFAULT (0) FOR [EseFormato]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseCausaleChiusuraConti]  DEFAULT (0) FOR [EseCausaleChiusuraConti]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseArtGiroconto]  DEFAULT (0) FOR [EseArtGiroconto]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseSaDa]  DEFAULT (0) FOR [EseSaDa]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseSaA]  DEFAULT (0) FOR [EseSaA]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseSpDa]  DEFAULT (0) FOR [EseSpDa]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseSpA]  DEFAULT (0) FOR [EseSpA]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseCeDa]  DEFAULT (0) FOR [EseCeDa]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseCeA]  DEFAULT (0) FOR [EseCeA]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseInvNFog]  DEFAULT (0) FOR [EseInvNFog]
GO
ALTER TABLE [dbo].[TbEse] ADD  CONSTRAINT [DF_TbEse_EseInvDesc]  DEFAULT ('') FOR [EseInvDesc]
GO
ALTER TABLE [dbo].[TbEse] ADD  DEFAULT ((0)) FOR [EseGContoRCee]
GO
ALTER TABLE [dbo].[TbFte_TipoDoc] ADD  DEFAULT ((0)) FOR [FteTd_Differita]
GO
ALTER TABLE [dbo].[TbFte_TipoDoc] ADD  DEFAULT ((0)) FOR [FteTd_Libera]
GO
ALTER TABLE [dbo].[TbFte_TipoDoc] ADD  DEFAULT ((0)) FOR [FteTd_Ncr]
GO
ALTER TABLE [dbo].[TbGds] ADD  CONSTRAINT [DF_TbGds_GdsDataOra]  DEFAULT (getdate()) FOR [GdsDataOra]
GO
ALTER TABLE [dbo].[TbGds] ADD  CONSTRAINT [DF_TbGds_GdsAutorizza]  DEFAULT (0) FOR [GdsAutorizza]
GO
ALTER TABLE [dbo].[TbGds] ADD  CONSTRAINT [DF_TbGds_GdsArtCod]  DEFAULT (0) FOR [GdsArtCod]
GO
ALTER TABLE [dbo].[TbGds] ADD  CONSTRAINT [DF_TbGds_GdsLingua]  DEFAULT (0) FOR [GdsLingua]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpMigl]  DEFAULT (0) FOR [GrpMigl]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA1]  DEFAULT (0) FOR [GrpAl1]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl1]  DEFAULT (0) FOR [GrpAl2]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA2]  DEFAULT (0) FOR [GrpAl3]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl2]  DEFAULT (0) FOR [GrpAl4]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA3]  DEFAULT (0) FOR [GrpAl5]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl3]  DEFAULT (0) FOR [GrpAl6]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA4]  DEFAULT (0) FOR [GrpAl7]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl40]  DEFAULT (0) FOR [GrpAl8]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA5]  DEFAULT (0) FOR [GrpAl9]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl5]  DEFAULT (0) FOR [GrpUl1]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA6]  DEFAULT (0) FOR [GrpUl2]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl6]  DEFAULT (0) FOR [GrpUl3]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA7]  DEFAULT (0) FOR [GrpUl4]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl7]  DEFAULT (0) FOR [GrpUl5]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpA8]  DEFAULT (0) FOR [GrpUl6]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl8]  DEFAULT (0) FOR [GrpUl7]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_Grpa9]  DEFAULT (0) FOR [GrpUl8]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpUl9]  DEFAULT (0) FOR [GrpUl9]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt1]  DEFAULT (0.00) FOR [GrpCpt1]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt2]  DEFAULT (0.00) FOR [GrpCpt2]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt3]  DEFAULT (0.00) FOR [GrpCpt3]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt4]  DEFAULT (0.00) FOR [GrpCpt4]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt5]  DEFAULT (0.00) FOR [GrpCpt5]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt6]  DEFAULT (0.00) FOR [GrpCpt6]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt7]  DEFAULT (0.00) FOR [GrpCpt7]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt8]  DEFAULT (0.00) FOR [GrpCpt8]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpCpt9]  DEFAULT (0.00) FOR [GrpCpt9]
GO
ALTER TABLE [dbo].[TbGrp] ADD  CONSTRAINT [DF_TbGrp_GrpBloccato]  DEFAULT (0) FOR [GrpBloccato]
GO
ALTER TABLE [dbo].[TbGrv] ADD  CONSTRAINT [DF_TbGrv_GrvCliFatt]  DEFAULT ('') FOR [GrvCliFatt]
GO
ALTER TABLE [dbo].[TBINCMI] ADD  CONSTRAINT [DF_TbInCmi_IcmDrCaf]  DEFAULT ((1)) FOR [IcmDrCaf]
GO
ALTER TABLE [dbo].[TbInEle] ADD  CONSTRAINT [DF_TbInEle_IeLock]  DEFAULT (0) FOR [IeLock]
GO
ALTER TABLE [dbo].[TbInP] ADD  CONSTRAINT [DF_TbInP_InPUm]  DEFAULT ('') FOR [InPUm]
GO
ALTER TABLE [dbo].[TbInP] ADD  CONSTRAINT [DF_TbInP_InPQta]  DEFAULT (0) FOR [InPQta]
GO
ALTER TABLE [dbo].[TbPaesi] ADD  DEFAULT ((0)) FOR [PaIntra]
GO
ALTER TABLE [dbo].[TbPag] ADD  DEFAULT ('') FOR [PagCodFE]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl01]  DEFAULT (0) FOR [PiaFl01]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl02]  DEFAULT (0) FOR [PiaFl02]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl03]  DEFAULT (0) FOR [PiaFl03]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl04]  DEFAULT (0) FOR [PiaFl04]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl05]  DEFAULT (0) FOR [PiaFl05]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl06]  DEFAULT (0) FOR [PiaFl06]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl07]  DEFAULT (0) FOR [PiaFl07]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl08]  DEFAULT (0) FOR [PiaFl08]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl09]  DEFAULT (0) FOR [PiaFl09]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl10]  DEFAULT (0) FOR [PiaFl10]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl11]  DEFAULT (0) FOR [PiaFl11]
GO
ALTER TABLE [dbo].[TbPia] ADD  CONSTRAINT [DF_TbPia_PiaFl12]  DEFAULT (0) FOR [PiaFl12]
GO
ALTER TABLE [dbo].[TbPos] ADD  CONSTRAINT [DF_TbPos_PosCauInsoluti]  DEFAULT ((16)) FOR [PosCauInsoluti]
GO
ALTER TABLE [dbo].[TbPos] ADD  CONSTRAINT [DF_TbPos_PosNomeModulo]  DEFAULT ('') FOR [PosNomeModulo]
GO
ALTER TABLE [dbo].[TbPos] ADD  CONSTRAINT [DF_TbPos_PosTesto]  DEFAULT ('') FOR [PosTesto]
GO
ALTER TABLE [dbo].[TbPos] ADD  CONSTRAINT [DF_TbPos_PosPiede]  DEFAULT ('') FOR [PosPiede]
GO
ALTER TABLE [dbo].[TbPos] ADD  CONSTRAINT [DF_TbPos_PosUltima]  DEFAULT ((0)) FOR [PosUltima]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriDocEst]  DEFAULT (0) FOR [PriDocEst]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriFl04]  DEFAULT (0) FOR [PriFl04]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriFl05]  DEFAULT (0) FOR [PriFl05]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriFl06]  DEFAULT (0) FOR [PriFl06]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriSos]  DEFAULT ('') FOR [PriSos]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriLinea]  DEFAULT (0) FOR [PriLinea]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriCodPag]  DEFAULT (0) FOR [PriCodPag]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriValuta]  DEFAULT (0) FOR [PriValuta]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriArtFisc]  DEFAULT (0) FOR [PriArtFisc]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriIvaPrint]  DEFAULT (0) FOR [PriIvaPrint]
GO
ALTER TABLE [dbo].[TbPri] ADD  CONSTRAINT [DF_TbPri_PriGStampa]  DEFAULT (0) FOR [PriGStampa]
GO
ALTER TABLE [dbo].[TbPrk] ADD  CONSTRAINT [DF_TbPrk_PrkDocAnn]  DEFAULT (0) FOR [PrkDocAnn]
GO
ALTER TABLE [dbo].[TbPrk] ADD  CONSTRAINT [DF_TbPrk_PrkDocest]  DEFAULT (0) FOR [PrkDocEst]
GO
ALTER TABLE [dbo].[TbPrk] ADD  CONSTRAINT [DF_TbPrk_PrkPAperta]  DEFAULT (0) FOR [PrkPAperta]
GO
ALTER TABLE [dbo].[TbPrkCoge] ADD  CONSTRAINT [DF_TbPrk_PrkArtFisc]  DEFAULT (0) FOR [PrkArtFisc]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RIvaNumFog]  DEFAULT (0) FOR [RIvaNumFog]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RIvaTipo]  DEFAULT (1) FOR [RIvaTipo]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RIvaPRata]  DEFAULT (0) FOR [RIvaPRata]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RivaPrintIniziale]  DEFAULT (1) FOR [RIvaPrintIniziale]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RIvaArt]  DEFAULT (0) FOR [RIvaArt]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RivaInt]  DEFAULT (0) FOR [RivaInt]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RivaCh]  DEFAULT (0) FOR [RivaCh]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  CONSTRAINT [DF_TbRegIva_RivaRCharge]  DEFAULT (0) FOR [RivaRCharge]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  DEFAULT ((0)) FOR [RIvaFteP]
GO
ALTER TABLE [dbo].[TbRegIva] ADD  DEFAULT ('') FOR [RivaTipoDoc]
GO
ALTER TABLE [dbo].[TbRegXML] ADD  CONSTRAINT [DF_TbRegXML_RxPeriodo]  DEFAULT ((0)) FOR [RxPeriodo]
GO
ALTER TABLE [dbo].[TbRegXML] ADD  CONSTRAINT [DF_TbRegXML_RxProgInvio]  DEFAULT ((0)) FOR [RxProgInvio]
GO
ALTER TABLE [dbo].[TbRegXML] ADD  DEFAULT ((0)) FOR [RxSel]
GO
ALTER TABLE [dbo].[TbRegXML] ADD  DEFAULT ((0)) FOR [RxInviato]
GO
ALTER TABLE [dbo].[TbRegXML] ADD  DEFAULT ((0)) FOR [RxNoControl]
GO
ALTER TABLE [dbo].[TbRegXML_EST] ADD  DEFAULT ((0)) FOR [RxPeriodo]
GO
ALTER TABLE [dbo].[TbRegXML_EST] ADD  DEFAULT ((0)) FOR [RxProgInvio]
GO
ALTER TABLE [dbo].[TbRegXML_EST] ADD  DEFAULT ((0)) FOR [RxSel]
GO
ALTER TABLE [dbo].[TbRegXML_EST] ADD  DEFAULT ((0)) FOR [RxInviato]
GO
ALTER TABLE [dbo].[TbRegXML_EST] ADD  DEFAULT ((0)) FOR [RxNoControl]
GO
ALTER TABLE [dbo].[TbRieCf] ADD  CONSTRAINT [DF_TbRieCf_RieCfEscludi]  DEFAULT ((0)) FOR [RieCfEscludi]
GO
ALTER TABLE [dbo].[TbRit] ADD  CONSTRAINT [DF_TbRit_RitRifId]  DEFAULT (0) FOR [RitRifId]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaTipoCo]  DEFAULT ((1)) FOR [ScaTipoCo]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaIvaSpe]  DEFAULT ((0)) FOR [ScaIvaSpe]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaBan]  DEFAULT ((0)) FOR [ScaBan]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaRifId]  DEFAULT ((0)) FOR [ScaRifId]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaRifProg]  DEFAULT ((0)) FOR [ScaRifProg]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaRifDA]  DEFAULT ('') FOR [ScaRifDA]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaRaperta]  DEFAULT ('') FOR [ScaRAperta]
GO
ALTER TABLE [dbo].[TbSca] ADD  CONSTRAINT [DF_TbSca_ScaImpPagato]  DEFAULT ((0.0)) FOR [ScaImpPagato]
GO
ALTER TABLE [dbo].[TbSup] ADD  CONSTRAINT [DF_TbSup_SupDesc]  DEFAULT ('') FOR [SupDesc]
GO
ALTER TABLE [dbo].[TbSup] ADD  CONSTRAINT [DF_TbSup_SupDesc2]  DEFAULT ('') FOR [SupDesc2]
GO
ALTER TABLE [dbo].[TbSup] ADD  CONSTRAINT [DF_TbSup_SupDesc3]  DEFAULT ('') FOR [SupDesc3]
GO
ALTER TABLE [dbo].[TbSup] ADD  CONSTRAINT [DF_TbSup_SupDesc4]  DEFAULT ('') FOR [SupDesc4]
GO
ALTER TABLE [dbo].[TbSup] ADD  CONSTRAINT [DF_TbSup_SupDesc5]  DEFAULT ('') FOR [SupDesc5]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVVersam]  DEFAULT (0) FOR [IvaVVersam]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVLiquida]  DEFAULT (0) FOR [IvaVLiquida]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVAbi]  DEFAULT (0) FOR [IvaVAbi]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVCab]  DEFAULT (0) FOR [IvaVCab]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVer_IvaVDelega]  DEFAULT (0) FOR [IvaVDelega]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVChiusura]  DEFAULT (0) FOR [IvaVChiusura]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVCrImUt]  DEFAULT (0) FOR [IvaVCrImUt]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVInteressi]  DEFAULT (0) FOR [IvaVInteressi]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVImpDaVers]  DEFAULT (0) FOR [IvaVImpDaVers]
GO
ALTER TABLE [dbo].[TbVers] ADD  CONSTRAINT [DF_TbVers_IvaVVersato]  DEFAULT (0) FOR [IvaVVersato]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCFLAG]  DEFAULT ((0)) FOR [TMCFLAG]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCDARE]  DEFAULT ((0)) FOR [TMCDARE]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCAVERE]  DEFAULT ((0)) FOR [TMCAVERE]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCSALDO]  DEFAULT ((0)) FOR [TMCSALDO]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCDAREP]  DEFAULT ((0)) FOR [TMCDAREP]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCAVEREP]  DEFAULT ((0)) FOR [TMCAVEREP]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCSALDOP]  DEFAULT ((0)) FOR [TMCSALDOP]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCCAUS]  DEFAULT ((0)) FOR [TMCCAUS]
GO
ALTER TABLE [dbo].[TMPBILC] ADD  CONSTRAINT [DF_TMPBILC_TMCTIPO]  DEFAULT ((0)) FOR [TMCTIPO]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSDARE]  DEFAULT ((0)) FOR [TMSDARE]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSAVERE]  DEFAULT ((0)) FOR [TMSAVERE]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSSALDO]  DEFAULT ((0)) FOR [TMSSALDO]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSDAREP]  DEFAULT ((0)) FOR [TMSDAREP]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSAVEREP]  DEFAULT ((0)) FOR [TMSAVEREP]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSSALDOP]  DEFAULT ((0)) FOR [TMSSALDOP]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSMASTRO]  DEFAULT ('00') FOR [TMSMASTRO]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSCAUS]  DEFAULT ((0)) FOR [TMSCAUS]
GO
ALTER TABLE [dbo].[TMPBILS] ADD  CONSTRAINT [DF_TMPBILS_TMSDESMA]  DEFAULT (' ') FOR [TMSDEMAS]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS1CODICE]  DEFAULT ('') FOR [TMPS1CODICE]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS1DESC]  DEFAULT ('') FOR [TMPS1DESC]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS1SALDO]  DEFAULT ((0)) FOR [TMPS1SALDO]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS2CODICE]  DEFAULT ('') FOR [TMPS2CODICE]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS2DESC]  DEFAULT ('') FOR [TMPS2DESC]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS2SALDO]  DEFAULT ((0)) FOR [TMPS2SALDO]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS3CODICE]  DEFAULT ('00.00') FOR [TMPS3CODICE]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS3DESC]  DEFAULT ('') FOR [TMPS3DESC]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS3SALDO]  DEFAULT ((0.00)) FOR [TMPS3SALDO]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS4CODICE]  DEFAULT ('00.00') FOR [TMPS4CODICE]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS4DESC]  DEFAULT ('') FOR [TMPS4DESC]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPS4SALDO]  DEFAULT ((0.00)) FOR [TMPS4SALDO]
GO
ALTER TABLE [dbo].[TMPCEE] ADD  CONSTRAINT [DF_TMPCEE_TMPSPPP]  DEFAULT ((0)) FOR [TMPSPPP]
GO
ALTER TABLE [dbo].[TMPDICIVA] ADD  CONSTRAINT [DF_TMPDICIVA_BSCESSIONI]  DEFAULT (0) FOR [BSCESSIONI]
GO
ALTER TABLE [dbo].[TMPDICIVA] ADD  CONSTRAINT [DF_TMPDICIVA_BSACQUISTI]  DEFAULT (0) FOR [BSACQUISTI]
GO
ALTER TABLE [dbo].[TMPDICIVA] ADD  CONSTRAINT [DF_TMPDICIVA_MERCI]  DEFAULT (0) FOR [LEASING]
GO
ALTER TABLE [dbo].[TMPGIO] ADD  CONSTRAINT [DF_TMPGIO_TIPOREG]  DEFAULT (0) FOR [TIPOREG]
GO
ALTER TABLE [dbo].[TMPGIO] ADD  CONSTRAINT [DF_TMPGIO_RETTIF]  DEFAULT (0) FOR [RETTIF]
GO
ALTER TABLE [dbo].[TMPLOCK] ADD  CONSTRAINT [DF_tmplock_IdPrNota]  DEFAULT (0) FOR [IdPrNota]
GO
ALTER TABLE [dbo].[TMPPIANO] ADD  CONSTRAINT [DF_TMPPIANO_RATACONCORDATA]  DEFAULT ((0.00)) FOR [RATACONCORDATA]
GO
ALTER TABLE [dbo].[TMPPIANO] ADD  CONSTRAINT [DF_TMPPIANO_IDSEQUENZA]  DEFAULT ((0)) FOR [IDSEQUENZA]
GO
ALTER TABLE [dbo].[TMPPROQUO] ADD  CONSTRAINT [DF_TMPPROQUO_ProQuoFondo]  DEFAULT (0) FOR [ProQuoFondo]
GO
ALTER TABLE [dbo].[TMPREGIVA] ADD  CONSTRAINT [DF_TMPREGIVA_PRegPND]  DEFAULT (0) FOR [PRegPND]
GO
ALTER TABLE [dbo].[TMPREGIVA] ADD  CONSTRAINT [DF_TMPREGIVA_PRegMerce]  DEFAULT (0) FOR [PRegMerce]
GO
ALTER TABLE [dbo].[TMPSEZ] ADD  CONSTRAINT [DF_TMPSEZ_TMPS1CODICE]  DEFAULT ('') FOR [TMPS1CODICE]
GO
ALTER TABLE [dbo].[TMPSEZ] ADD  CONSTRAINT [DF_TMPSEZ_TMPS1DESC]  DEFAULT ('') FOR [TMPS1DESC]
GO
ALTER TABLE [dbo].[TMPSEZ] ADD  CONSTRAINT [DF_TMPSEZ_TMPS1SALDO]  DEFAULT ((0)) FOR [TMPS1SALDO]
GO
ALTER TABLE [dbo].[TMPSEZ] ADD  CONSTRAINT [DF_TMPSEZ_TMPS2CODICE]  DEFAULT ('') FOR [TMPS2CODICE]
GO
ALTER TABLE [dbo].[TMPSEZ] ADD  CONSTRAINT [DF_TMPSEZ_TMPS2DESC]  DEFAULT ('') FOR [TMPS2DESC]
GO
ALTER TABLE [dbo].[TMPSEZ] ADD  CONSTRAINT [DF_TMPSEZ_TMPS2SALDO]  DEFAULT ((0)) FOR [TMPS2SALDO]
GO
ALTER TABLE [dbo].[TMPSEZ] ADD  CONSTRAINT [DF_TMPSEZ_TMPSPPP]  DEFAULT ((0)) FOR [TMPSPPP]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriDocEst]  DEFAULT (0) FOR [PriDocEst]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriFl04]  DEFAULT (0) FOR [PriFl04]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriFl05]  DEFAULT (0) FOR [PriFl05]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriFl06]  DEFAULT (0) FOR [PriFl06]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriCodPag]  DEFAULT (0) FOR [PriCodPag]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriArtFisc]  DEFAULT (0) FOR [PriArtFisc]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriIvaPrint]  DEFAULT (0) FOR [PriGStampa]
GO
ALTER TABLE [dbo].[TMPTBPRI] ADD  CONSTRAINT [DF_TMPTbPri_PriPAperta]  DEFAULT (0) FOR [PriPAperta]
GO
ALTER TABLE [dbo].[TMPXMLINVIATE_ELT_EST] ADD  DEFAULT ((0)) FOR [PRegPND]
GO
ALTER TABLE [dbo].[TMPXMLINVIATE_ELT_EST] ADD  DEFAULT ((0)) FOR [PRegMerce]
GO
ALTER TABLE [dbo].[TMPXMLINVIATE_ELT_EST] ADD  DEFAULT ((1)) FOR [PRegOK]
GO
ALTER TABLE [dbo].[TMPXMLREGIVA] ADD  CONSTRAINT [DF_TMPXMLREGIVA_PRegPND]  DEFAULT ((0)) FOR [PRegPND]
GO
ALTER TABLE [dbo].[TMPXMLREGIVA] ADD  CONSTRAINT [DF_TMPXMLREGIVA_PRegMerce]  DEFAULT ((0)) FOR [PRegMerce]
GO
ALTER TABLE [dbo].[TMPXMLREGIVA_EST] ADD  DEFAULT ((0)) FOR [PRegPND]
GO
ALTER TABLE [dbo].[TMPXMLREGIVA_EST] ADD  DEFAULT ((0)) FOR [PRegMerce]
GO
ALTER TABLE [dbo].[TMPXMLREGIVA_EST] ADD  CONSTRAINT [DF_TMPXMLREGIVA_EST_PRegOK]  DEFAULT ((1)) FOR [PRegOK]
GO
ALTER TABLE [dbo].[TMRLOCK] ADD  CONSTRAINT [DF_tmRlock_IdPrNota]  DEFAULT (0) FOR [IdPrNota]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriDocEst]  DEFAULT (0) FOR [PriDocEst]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriFl04]  DEFAULT (0) FOR [PriFl04]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriFl05]  DEFAULT (0) FOR [PriFl05]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriFl06]  DEFAULT (0) FOR [PriFl06]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriCodPag]  DEFAULT (0) FOR [PriCodPag]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriValuta]  DEFAULT (0) FOR [PriValuta]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriArtFisc]  DEFAULT (0) FOR [PriArtFisc]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriIvaPrint]  DEFAULT (0) FOR [PriIvaPrint]
GO
ALTER TABLE [dbo].[TRPRI] ADD  CONSTRAINT [DF_TRPRI_PriGStampa]  DEFAULT (0) FOR [PriGStampa]
GO
ALTER TABLE [dbo].[TRPRK] ADD  CONSTRAINT [DF_TRPRK_PrkDocAnn]  DEFAULT (0) FOR [PrkDocAnn]
GO
ALTER TABLE [dbo].[TRPRK] ADD  CONSTRAINT [DF_TRPRK_PrkDocest]  DEFAULT (0) FOR [PrkDocEst]
GO
ALTER TABLE [dbo].[TRPRK] ADD  CONSTRAINT [DF_TRPRK_PrkPAperta]  DEFAULT (0) FOR [PrkPAperta]
GO
ALTER TABLE [dbo].[TRRET] ADD  CONSTRAINT [DF_TRRET_RETARTICOLO]  DEFAULT (0) FOR [RETARTICOLO]
GO
ALTER TABLE [dbo].[TRRET] ADD  CONSTRAINT [DF_TRRET_RETDESCR]  DEFAULT ('') FOR [RETDESCR]
GO
ALTER TABLE [dbo].[TbCom]  WITH CHECK ADD  CONSTRAINT [FK_TbCom_TbProv] FOREIGN KEY([ComProv])
REFERENCES [dbo].[TbProv] ([ProvCod])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[TbCom] CHECK CONSTRAINT [FK_TbCom_TbProv]
GO
ALTER TABLE [dbo].[TbProv]  WITH CHECK ADD  CONSTRAINT [FK_TbProv_TbRegioni] FOREIGN KEY([ProvReg])
REFERENCES [dbo].[TbRegioni] ([RegCod])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[TbProv] CHECK CONSTRAINT [FK_TbProv_TbRegioni]
GO
ALTER TABLE [dbo].[TbQuo]  WITH NOCHECK ADD  CONSTRAINT [FK_TbQuo_TbCesp] FOREIGN KEY([QuoNum])
REFERENCES [dbo].[TbCesp] ([CespNum])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TbQuo] CHECK CONSTRAINT [FK_TbQuo_TbCesp]
GO
ALTER TABLE [dbo].[TbVCesp]  WITH NOCHECK ADD  CONSTRAINT [FK_TbVCesp_TbQuo] FOREIGN KEY([VCespNum], [VCespAnno])
REFERENCES [dbo].[TbQuo] ([QuoNum], [QuoAnno])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TbVCesp] CHECK CONSTRAINT [FK_TbVCesp_TbQuo]
GO
/****** Object:  StoredProcedure [dbo].[ADDCRSP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ADDCRSP] @ANNO AS SMALLINT, @REG AS SMALLINT,@DM as SMALLINT,@AM as SMALLINT ,@BLOCK AS INT
AS 
INSERT INTO TMPCORR (TCorrAnno,TCorrRegIva,TCorrMese,TCorrCodIva,TCorrConto,TCorrLordo,TCorrId)
SELECT CorrAnno,CorrRegIva,CorrMese,CorrCodIva,CorrConto,CorrLordo,@BLOCK
FROM TbCorr where CorrAnno = @anno and CorrRegIva = @REG and CorrMese between @DM and @AM

GO
/****** Object:  StoredProcedure [dbo].[ADDIVAP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ADDIVAP] @ANNO AS SMALLINT, @REG AS SMALLINT,@DM as SMALLINT,@AM as SMALLINT ,@BLOCK AS INT
AS 
INSERT INTO TMPIVAP (TIvaPAnno, TIvaPMese, TIvaPRegIva,TIvaPCodIva,TIvaPImpon,TIvaPIvaDE, TIvaPIvaND,TIvaPImpMerce,TivaPid)
SELECT IvaPAnno, IvaPMese, IvaPRegIva,IvaPCodIva,IvaPImpon,IvaPIvaDE,IvaPIvaND,IvaPImpMerce,@BLOCK
FROM tbivap where IvaPAnno = @anno and IvaPRegIva = @REG and IvaPmese between @DM and @AM

GO
/****** Object:  StoredProcedure [dbo].[AggiornaBilance]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE Proc [dbo].[AggiornaBilance] @Lingua smallint
as

declare @MaxData as datetime, @DataPart as datetime

set @DataPart = (select top 1 TaiAggGds from TbTai Order by TaiAnno desc)

set @MaxData = (select Max(GdsDataOra) from TbGds)

if @MaxData > @DataPart
   BEGIN
   UPDATE GDS...TESTI
   SET TESTO = GdsTESTI
   from GDS...TESTI INNER JOIN
        TbGds on IDTEXT = GdsIdText
   WHERE GdsDataOra >= @DataPart and GdsDataOra <= @MaxData and GdsAutorizza = 1 and GdsLingua = @Lingua

   update TbTai set TaiAggGds = @MaxData
   end






GO
/****** Object:  StoredProcedure [dbo].[AggiornaRicette]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


Create proc [dbo].[AggiornaRicette] @DIBASE AS SMALLINT
AS

Update TbTDis set
TdisItaliano = (select DBO.FNETICHETTA(@DIBASE,1)),
TdisFrancese = (select DBO.FNETICHETTA(@DIBASE,2)),
TdisInglese = (select DBO.FNETICHETTA(@DIBASE,3)),
TdisTedesco = (select DBO.FNETICHETTA(@DIBASE,4)),
TdisSpagnolo = (select DBO.FNETICHETTA(@DIBASE,5))
WHERE TDisCod = @DIBASE

GO
/****** Object:  StoredProcedure [dbo].[AggiornaTutteRicette]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create proc [dbo].[AggiornaTutteRicette]
as

declare @DIBASE AS SMALLINT

DECLARE CURSORE
CURSOR FOR
SELECT TdisCod from tbTdis

OPEN CURSORE
FETCH NEXT FROM CURSORE
INTO @DIBASE

WHILE @@FETCH_STATUS = 0

BEGIN

EXEC AGGIORNARICETTE @DIBASE   

FETCH NEXT FROM CURSORE
INTO @DIBASE

end

GO
/****** Object:  StoredProcedure [dbo].[AprePartita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[AprePartita] @DbWhere AS VARCHAR(200) 
as
declare @Cmd varchar(2000)
---- passa direttamente la where iniziando con AND
---- ( ES. AND PRKCONTO = '01001' AND PRKDOCANN = 2004 AND PRKDOCEST = 10) per la singola partita oppure
---- ( ES. AND PRKCONTO = '01001' ) per tutte le partite di quel conto
set @cmd = 'SELECT distinct PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST,
SUM(dare)AS DARE,sum(avere)AS AVERE ,sum(dare)-sum(avere) AS SALDO 
into #tmp
FROM vb8
where partitario = 1'+@DbWhere+' 
group by PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST
HAVING (sum(dare)-sum(avere)) <> 0
order by PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST
UPDATE TBPRK SET PRKpAPERTA = 0 FROM TBPRK T, #TMP S WHERE T.PRKCONTO =S.PRKCONTO AND T.PRKDOCANN = S.PRKDOCANN AND T.PRKDOCEST = S.PRKDOCEST  
drop table #tmp'
exec (@cmd)


GO
/****** Object:  StoredProcedure [dbo].[AssegnaPartita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[AssegnaPartita] @CONTO as VARCHAR(5),@ANNO as SMALLInt,@NDOC as int,@IMPORTO as DECIMAL(13,2)
as
CREATE TABLE [dbo].[#TASSP] (
	[AspId] [int] NOT NULL ,
	[AspProg] [smallint] NOT NULL ,
	[AspDa] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL 
       ) ON [PRIMARY]
INSERT INTO [dbo].[#TASSP] ([AspId],[AspProg],[AspDa])
select PriId,PriProg,PrkDa from tbprk 
inner join tbpri on prkid = priid and PrkProg = priprog 
where prktipoco = 1 and prkdocest = 0 and prkpaperta = 0 and 
prkconto = @CONTO and pricausale > 3 and 
( PriImpDare = @IMPORTO OR PriImpAvere = @IMPORTO) 
DECLARE @P AS SMALLINT
SET @P = (SELECT COUNT(*) FROM #TASSP)
IF @P <> 1 goto FINECICLO
UPDATE TBPRK SET PRKDOCANN = @ANNO,PRKDOCEST = @NDOC FROM TBPRK , #TASSP 
WHERE PRKCONTO =@CONTO AND PRKID =AspId AND PrkProg = AspProg  and PrkDa = AspDa  

UPDATE TBPRI SET PRIDOCANN = @ANNO,PRIDOCEST = @NDOC FROM TBPRI , #TASSP 
WHERE PRIID =AspId AND PrIProg = AspProg  

FINECICLO:
DROP TABLE #TASSP


GO
/****** Object:  StoredProcedure [dbo].[ChiudeContoZero]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[ChiudeContoZero] @CONTO AS VARCHAR(5) 
as
declare @Cmd varchar(2000),@DbWhere varchar(200)
IF @CONTO > '0'
   begin
 SET @DbWhere = ' And PrkConto = '''+@conto+''''
   end
else
   begin
 SET @DbWhere = ''
   end
set @cmd = 'SELECT distinct PRKTIPOCO,PRKCONTO,
SUM(dare)AS DARE,sum(avere)AS AVERE ,sum(dare)-sum(avere) AS SALDO 
into #tmp
FROM vb8
where partitario = 1'+@DbWhere+' 
group by PRKTIPOCO,PRKCONTO
HAVING (sum(dare)-sum(avere)) = 0
order by PRKTIPOCO,PRKCONTO
UPDATE TBPRK SET PRKpAPERTA = 1 FROM TBPRK T, #TMP S WHERE T.PRKCONTO =S.PRKCONTO   
drop table #tmp'
exec (@cmd)

GO
/****** Object:  StoredProcedure [dbo].[ChiudePartita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ChiudePartita] 
as
SELECT distinct PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST,
SUM(dare)AS DARE,sum(avere)AS AVERE ,sum(dare)-sum(avere) AS SALDO 
into #tmp
FROM vb8
where partitario = 1 AND PRKPAPERTA = 0 AND PRKDOCEST > 0
group by PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST
HAVING (sum(dare)-sum(avere)) = 0
order by PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST
UPDATE TBPRK SET PRKpAPERTA = 1 FROM TBPRK T, #TMP S WHERE T.PRKCONTO =S.PRKCONTO AND T.PRKDOCANN = S.PRKDOCANN AND T.PRKDOCEST = S.PRKDOCEST  
drop table #tmp
GO
/****** Object:  StoredProcedure [dbo].[ChiudePiuContiZero]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[ChiudePiuContiZero] @CONTOD AS VARCHAR(6),@CONTOA AS VARCHAR(6)
as
SELECT distinct PRKTIPOCO,PRKCONTO,
SUM(dare)AS DARE,sum(avere)AS AVERE ,sum(dare)-sum(avere) AS SALDO 
into #tmp
FROM vb8
where partitario = 1 AND PRKTIPOCO+PRKCONTO BETWEEN @CONTOD and @CONTOA
group by PRKTIPOCO,PRKCONTO
HAVING (sum(dare)-sum(avere)) = 0
order by PRKTIPOCO,PRKCONTO
UPDATE TBPRK SET PRKpAPERTA = 1 FROM TBPRK T, #TMP S WHERE T.PRKCONTO =S.PRKCONTO   
drop table #tmp

GO
/****** Object:  StoredProcedure [dbo].[ChiudePiuPartite]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[ChiudePiuPartite] @CONTOD AS VARCHAR(6),@CONTOA AS VARCHAR(6)
as
SELECT distinct PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST,
SUM(dare)AS DARE,sum(avere)AS AVERE ,sum(dare)-sum(avere) AS SALDO 
into #tmp
FROM vb8
where partitario = 1 AND PRKTIPOCO+PRKCONTO BETWEEN @CONTOD and @CONTOA
group by PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST
HAVING (sum(dare)-sum(avere)) = 0
order by PRKTIPOCO,PRKCONTO,PRKDOCANN,PRKDOCEST
UPDATE TBPRK SET PRKpAPERTA = 1 FROM TBPRK T, #TMP S WHERE T.PRKCONTO =S.PRKCONTO  AND T.PRKDOCANN = S.PRKDOCANN AND T.PRKDOCEST = S.PRKDOCEST  
drop table #tmp


GO
/****** Object:  StoredProcedure [dbo].[ChiudePiuRate]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[ChiudePiuRate] @CONTOD AS VARCHAR(6),@CONTOA AS VARCHAR(6)
as
select ScaTipoCo,ScaConto,ScaId,ScaNRata,ScaNDoc,ScaDDoc,ScaImpDoc,ScaImprata,ScaImpPagato
into #tmp
 from tbsca  
where SCATIPOCO+SCACONTO BETWEEN @CONTOD and @CONTOA
group by scatipoco,scaconto,scaid,ScaNRata,ScaNDoc,ScaDDoc,ScaImpDoc,scaImpRata,ScaImpPagato
having ScaImpPagato = ScaImpRata
order by scatipoco,scaconto,scaid

UPDATE TBSCA SET SCARAPERTA = '*' FROM TBSCA T, #TMP S WHERE T.SCAID =S.SCAID   
drop table #tmp



GO
/****** Object:  StoredProcedure [dbo].[CModIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[CModIva] @anno AS int
as


DELETE FROM TMPDICIVA

--- FASE 1

select ciicod into #CII from tbcii where ciitp > 0 and ciitp < 4
---SELECT * FROM #CII
SELECT (select sum(priimpdare)from tbpri where pricausale = 1 and prifl04 >0 and prifl04 <> 88 and datepart(year,pridatagio) = @anno and PRICODIVA IN (select ciicod from #cii)) as VenditeBs,
(select sum(priimpdare)from tbpri where pricausale = 2 and prifl04 >0 and prifl04 <> 88 and datepart(year,pridatagio) = @anno and PRICODIVA IN (select ciicod from #cii)) as AcquistiBs,
(select sum(priimpdare)from tbpri where pricausale = 2 and prifl04 = 88 and datepart(year,pridatagio) = @anno and PRICODIVA IN (select ciicod from #cii)) as LeasingBs,
(select sum(priimpdare)from tbpri where pricausale = 2 and prifl06 >0 and datepart(year,pridatagio) = @anno and PRICODIVA IN (select ciicod from #cii)) as Merce into #TMP
--select * from #tmp


--- FASE 2
select(select isNull(sum(priimpdare),0) from tbpri as T where pricausale = 1 and prifl04 >0 and prifl04 <> 88 and datepart(year,pridatagio) = @anno and pricodiva = 0
and (select top 1 pricodiva from tbpri as b where b.priid = t.priid and b.priprog > t.priprog and b.pricodiva > 0 ) IN (select ciicod from #cii)) as VenditeBsZ, 
(select IsNull(sum(priimpdare),0) from tbpri as T where pricausale = 2 and prifl04 >0 and prifl04 <> 88 and datepart(year,pridatagio) = @anno and pricodiva = 0
and (select top 1 pricodiva from tbpri as b where b.priid = t.priid and b.priprog > t.priprog and b.pricodiva > 0 ) IN (select ciicod from #cii)) as AcquistiBsZ, 
(select IsNull(sum(priimpdare),0) from tbpri as T where pricausale = 2 and prifl04 = 88 and datepart(year,pridatagio) = @anno and pricodiva = 0
and (select top 1 pricodiva from tbpri as b where b.priid = t.priid and b.priprog > t.priprog and b.pricodiva > 0 ) IN (select ciicod from #cii)) as LeasingBsZ,
(select IsNull(sum(priimpdare),0) from tbpri as T where pricausale = 2 and prifl06 >0 and datepart(year,pridatagio) = @anno and pricodiva = 0
and (select top 1 pricodiva from tbpri as b where b.priid = t.priid and b.priprog > t.priprog and b.pricodiva > 0 ) IN (select ciicod from #cii)) as MerceZ into #TMP2
--select * from #tmp2

INSERT INTO TMPDICIVA(BSCESSIONI,BSACQUISTI,LEASING,MERCI)
select isnull((select VenditeBs from #tmp),0) + isnull((select VenditeBsZ from #tmp2),0) as BSCESSIONI,
isnull((select AcquistiBs from #tmp),0) + isnull((select AcquistiBsZ from #tmp2),0) as BSACQUISTI,
isnull((select LeasingBs from #tmp),0) + isnull((select LeasingBsZ from #tmp2),0) as LEASING,
isnull((select Merce from #tmp),0) + isnull((select MerceZ from #tmp2),0) as MercI

---INSERT INTO TMPDICIVA(BSCESSIONE,BSACQUISTI,LEASING,MERCI)

drop table #tmp2
drop table #tmp
drop table #CII




GO
/****** Object:  StoredProcedure [dbo].[ControlloPVendita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE proc [dbo].[ControlloPVendita] @GRUPPO as varchar(5), @DaData as smalldatetime, @AData as smalldatetime
as

select 
PVENDITA = FatCliCons,
GRUPPO = FatCliFat,
CODICE = CorCodArt,
CODIMBALLO = CorCodImballo,
UMACQ = CorUMisVen,
QACQ = sum(CorQuaVen),
VACQ = sum(CorImporto),
QVEN = sum(CorQuaVen),
VVEN = sum(CorQuaven) 
into #TMP
from TbCor inner join
     TbFat on Cortipodoc = FatTipoDoc 
where corcau = 5 and corcodart  > 0 AND FatCliFat = @GRUPPO and FatData between @DADATA and @ADATA 
group by FatCliCons,FatCliFat,CorCodArt,CorCodImballo,CorUMisVen


SELECT
GRUPPO,PVENDITA,
RAGSOC = (Select AnaDesc from Vdox.dbo.TbAna where AnaGrp = 'CL' AND AnaCod = PVENDITA),
FAMIGLIA = ArtFamiglia,
DESCFAMIGLIA = ISNULL((select FamDesc from TbFam where FamCod = ArtFamiglia),''),
CODICE,
DESCRIZIONE = ArtDesc,
IMBALLO = isnull((select Matdesc from TbMat where MatCod = CODIMBALLO),''),
UMACQ,QACQ,VACQ,
PMACQ = CAST((VACQ/QACQ) AS DECIMAL(12,2)),
QGIACE = ISNULL((select InPQta FROM TbInP where InPCliCons = PVENDITA and InPData = @ADATA and InPCodArt = CODICE and InpImballo = CODIMBALLO),0),
QVEN, 
VVEN = cast((qven - ISNULL((select InPQta FROM TbInP where InPCliCons = PVENDITA and InPData = @ADATA and InPCodArt = CODICE and InpImballo = CODIMBALLO),0)) * ISNULL(dbo.fnprezzoLis(CODICE,GrvLisTest,@ADATA),0) as decimal(12,2))
from #TMP inner join
     TbArt on ArtCod = CODICE INNER JOIN
     TbGrv on GrvCliCons = PVENDITA 
order by GRUPPO,PVENDITA,FAMIGLIA,CODICE



     



GO
/****** Object:  StoredProcedure [dbo].[CopiaAnnoLavoro]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[CopiaAnnoLavoro]
--Declare
@Da as smallint, @A as smallint
as
--set @Da=2004
--set @A=2001
select * into #tmp
from tbAzi
where aziannolavoro=@Da
update #tmp set AziAnnoLavoro=@A
INSERT INTO TbAzi
   SELECT * FROM #tmp
drop table #tmp

select * into #tmp1
from tbEse
where Eseanno=@Da
update #tmp1 set EseAnno=@A, eseDal= cast(day(eseDal)as varchar)+ '/' + cast(month(eseDal) as varchar)+ '/' + cast(@A as varchar),
EseAl= cast(day(eseAl)as varchar)+ '/' + cast(month(eseAl) as varchar) + '/' + cast(@A as varchar),EseGioNFog = 0, EseGioProg = 0, EseInvNFog=0
INSERT INTO TbEse
   SELECT * FROM #tmp1
drop table #tmp1

select * into #tmp2
from tbRegIva
where RIvaAnno=@Da
update #tmp2 set RIvaAnno=@A, RivaNumFog=0,RIvaPrintIniziale=1,RivaProtCar = null,RivaProtCarRBS = null,RivaGData = null,
RIvaProtSta = null,RivaProtStaRBS = null,RIvaUData = null
INSERT INTO Tbregiva
   SELECT * FROM #tmp2
drop table #tmp2




GO
/****** Object:  StoredProcedure [dbo].[CORRISP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CORRISP]
AS
------DIMEZZA GLI ERRORI DA COBOL PER CORRISPETTIVI-----
DECLARE @P AS INT,@IIDD AS INT,@PRID AS SMALLINT
DECLARE DATISCA CURSOR FOR
SELECT distinct priid,max(priprog)as PriProg FROM tbpri where priregiva = 6 group by priid 
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @IIDD,@PRID
WHILE @@FETCH_STATUS = 0
BEGIN
set @P = @prid/2 
IF @p = 0 goto FINE_LOOP
LOOPLOOP:
SET @P = @P + 1
DELETE FROM tbpri where priid = @iidd and priprog = @p
if @p = @prid goto DOPO_LOOP 
goto LOOPLOOP
DOPO_LOOP:
exec InitPrk @Id = @IIDD
FINE_LOOP:
   FETCH NEXT FROM DATISCA
   INTO @IIDD,@PRID
END
CLOSE DATISCA
DEALLOCATE DATISCA

GO
/****** Object:  StoredProcedure [dbo].[CreaPrk]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[CreaPrk]
as
delete from TBPrk
insert into TbPrk(PrkId,PrkProg,PrkDa,PrkTipoCo,PrkConto,PrkAammgg,PrkDocAnn,PrkDocEst,PrkPAperta)
select priid,PriProg,'0',case when substring(priCoDare,3,1) = '.' then '0' else '1' end,PriCoDare,
case when PRICAUSALE < 4 then PriDataGio else PridataEst end,
PriDocAnn,PriDocEst,0
from TbPri where PriCodare <> '00.10' AND PRICAUSALE <> 1 ---- prima i dare
insert into TbPrk(PrkId,PrkProg,PrkDa,PrkTipoCo,PrkConto,PrkAammgg,PrkDocAnn,PrkDocEst,PrkPAperta)
select priid,PriProg,'1',case when substring(priCoAvere,3,1) = '.' then '0' else '1' end,PriCoAvere,
case when PRICAUSALE < 4 then PriDataGio else PridataEst end,
PriDocAnn,PriDocEst,0
from TbPri where PriCoAvere <> '00.10' AND PRICAUSALE <> 2 ---- poi Avere

UPDATE TBPRK SET PRKPAPERTA = 1 FROM TBPRK , TMPTBPRI 
       WHERE PRKID = PRITEMP  AND PRKPROG = PRIPROG  AND PRIPAPERTA = 1
GO
/****** Object:  StoredProcedure [dbo].[DaEliminaPartita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DaEliminaPartita] 
as
CREATE TABLE [dbo].[#RAPCH] (
	[RpcConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[RpcNdoc] [int] NOT NULL ,
	[RpcAnno] [smallint] 
) ON [PRIMARY]
DECLARE @CONTO as VARCHAR(5),@ANNO as SMALLInt,@NDOC as int,@RIF as int, @MIGLIO AS INT

SET @MIGLIO = (SELECT GrpMigl FROM TBGRP WHERE GRPCOD='CL')

DECLARE SERGENTE CURSOR FOR
SELECT distinct ApcConto,ApcAnno ,ApcNdoc from ##TAPCH
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @CONTO,@ANNO,@NDOC
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO [dbo].[#RAPCH] ([RpcConto] ,[RpcAnno],[RpcNdoc]) 
SELECT distinct PRKCONTO,PRKDOCANN,PRKDOCEST
FROM vb8
where partitario = 1 and PRKCONTO = @CONTO AND PRKDOCANN = @ANNO AND PRKDOCEST = @NDOC AND PRKDOCEST > 0
group by PRKCONTO,PRKDOCANN,PRKDOCEST
HAVING (sum(dare)-sum(avere)) = 0
order by PRKCONTO,PRKDOCANN,PRKDOCEST
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @CONTO,@ANNO,@NDOC
END
CLOSE SERGENTE
DEALLOCATE SERGENTE
-- IN PARTE NUOVA
--if (select count(*) from #RAPCH ) > 0
--begin
UPDATE TBPRK SET PRKpAPERTA = 1 FROM TBPRK T, #RAPCH S WHERE T.PRKCONTO =S.RpcConto AND T.PRKDOCANN = S.RpcAnno AND T.PRKDOCEST = S.RpcNdoc  
--UPDATE TBSCA SET SCArAPERTA = 1,SCAIMPPAGATO = SCAIMPRATA FROM TBSCA Q, #RAPCH S WHERE Q.SCACONTO =S.RpcConto AND DATEPART(YEAR,Q.SCADDOC) = S.RpcAnno AND Q.SCANDOC = S.RpcNdoc
DECLARE XERGENTE CURSOR FOR
SELECT distinct ApcConto,ApcAnno ,ApcNdoc from ##TAPCH
OPEN XERGENTE
FETCH NEXT FROM XERGENTE
INTO @CONTO,@ANNO,@NDOC
WHILE @@FETCH_STATUS = 0
BEGIN
IF ISNUMERIC(@CONTO)=1
BEGIN
SET @RIF = (Select DISTINCT ScaRifId from TbSca where ScaConto = @CONTO AND DATEPART(YEAR,SCADDOC) = @ANNO AND SCANDOC = @NDOC AND ISNUMERIC(@CONTO)=1)
IF @RIF > 0  
BEGIN
EXEC RiScadenza @id = @RIF, @az = 0,@miglio =@miglio
END
END
XFINE_LOOP:
   FETCH NEXT FROM XERGENTE
   INTO @CONTO,@ANNO,@NDOC
END
CLOSE XERGENTE
DEALLOCATE XERGENTE
--SET @CONTO=(SELECT TOP 1 ApcConto from ##TAPCH)
--SET @ANNO=(SELECT TOP 1 ApcAnno from ##TAPCH)
--set @NDOC=(SELECT TOP 1 ApcNdoc from ##TAPCH)
USCITA:
drop table #RAPCH
drop table ##TAPCH

GO
/****** Object:  StoredProcedure [dbo].[datecontrollo]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc [dbo].[datecontrollo] @GRUPPO AS VARCHAR(5)
AS


select distinct InPData as FatData, 1 as Inv
into #TMP
from TbInP
where InPCliCons in (select GrvCliCons from TbGrv where GrvCliFatt = @GRUPPO)
UNION
select distinct fatdata,0 AS INV
from TbFat
where FatCliFat = @gruppo AND FATCAU = 5
AND FATDATA NOT IN ( SELECT DISTINCT InPdata from tbInP where InPCliCons in (select GrvCliCons from TbGrv where GrvCliFatt = @GRUPPO))


SELECT * FROM #TMP ORDER BY FATDATA DESC

GO
/****** Object:  StoredProcedure [dbo].[DXRIESCA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DXRIESCA] @AL as smalldatetime,@CLFO as varchar(2),@TIPO as varchar(1)
AS
declare @M30 as smalldatetime,@M60 as smalldatetime,@M90 as smalldatetime
DECLARE @P30 as smalldatetime,@P60 as smalldatetime,@P90 as smalldatetime

SET @M30 = DATEADD(MONTH,-1,@AL)
SET @M60 = DATEADD(MONTH,-2,@AL)
SET @M90 = DATEADD(MONTH,-3,@AL)

SET @P30 = DATEADD(MONTH,1,@AL)
SET @P60 = DATEADD(MONTH,2,@AL)
SET @P90 = DATEADD(MONTH,3,@AL)


SELECT ScaTipoCo,ScaConto,
SCADESC=ISNULL((SELECT AnaDesc FROM VDOX.dbo.TbAna WHERE (AnaCod = ScaConto) AND (AnaGrp = 'CL')),
ISNULL((SELECT AnaDesc FROM VDOX.dbo.TbAna WHERE (AnaCod = ScaConto) AND (AnaGrp = 'FO')),
ISNULL((SELECT PiaAnaCo FROM TbPia WHERE (PiaCodCo = ScaConto)), '***ERRATO***'))),
ScaNdoc,ScaImpDoc,ScaImpRata,ScaTPag,ScaDdoc,ScaDsca,PrkPAperta,ScaScopRata=(ScaImpRata - ScaImpPagato) ,ScaImpPagato,ScaNRata,
CLFOPI=CASE WHEN ScaTipoCo = '0' THEN 'SO' ELSE ISNULL((SELECT ANAGRP FROM  VDOX.DBO.TBANA WHERE ANACOD = ScaConto), 'FO') END,
ScaSca1 = case when ScaDsca BETWEEN @M30 and DATEADD(DAY,-1,@AL) then (ScaImpRata - ScaImpPagato) else 0 end,
ScaSca2 = case when ScaDsca BETWEEN @M60 and DATEADD(DAY,-1,@M30)then (ScaImpRata - ScaImpPagato) else 0 end,
ScaSca3 = case when ScaDsca BETWEEN @M90 AND DATEADD(DAY,-1,@M60)then (ScaImpRata - ScaImpPagato) else 0 end,
ScaSca4 = case when ScaDsca < @M90 then (ScaImpRata - ScaImpPagato) else 0 end,
ScaSca5 = case when ScaDsca BETWEEN @AL AND @P30 then (ScaImpRata - ScaImpPagato) else 0 end,
ScaSca6 = case when ScaDsca BETWEEN DATEADD(DAY,1,@P30) AND @P60 then (ScaImpRata - ScaImpPagato) else 0 end,
ScaSca7 = case when ScaDsca BETWEEN DATEADD(DAY,1,@P60) AND @P90 then (ScaImpRata - ScaImpPagato) else 0 end,
ScaSca8 = case when ScaDsca >@P90  then (ScaImpRata - ScaImpPagato) else 0 end,
Periodo = ScaDsca,NrBanca=IsNull(BanDes,'DA ASSEGNARE')
INTO #TMPRIE FROM dbo.TbSca 
LEFT OUTER JOIN TBBAN ON ScaBAN = BANCOD
LEFT OUTER JOIN dbo.TbPag ON dbo.TbSca.ScaCodPag = dbo.TbPag.PagCod 
LEFT OUTER JOIN dbo.TbPrk ON dbo.TbSca.ScaRifId = dbo.TbPrk.PrkId AND dbo.TbSca.ScaRifProg = dbo.TbPrk.PrkProg AND dbo.TbSca.ScaRifDA = dbo.TbPrk.PrkDa
WHERE PrkPAperta = 0 and (ScaImpRata - ScaImpPagato) <> 0

IF @TIPO = 'A'
BEGIN
SELECT * FROM #TMPRIE WHERE CLFOPI = @CLFO Order by SCACONTO,ScaDsca
goto USCITA
END 

SELECT ScaConto,SCADESC,CLFOPI,Scoperto=SUM(ScaSca1)+SUM(ScaSca2)+SUM(ScaSca3)+SUM(ScaSca4)+SUM(ScaSca5)+SUM(ScaSca6)+SUM(ScaSca7)+SUM(ScaSca8),
ScaSca1 =SUM(ScaSca1),ScaSca2 =SUM(ScaSca2),ScaSca3 =SUM(ScaSca3),ScaSca4 =SUM(ScaSca4),
ScaSca5 =SUM(ScaSca5),ScaSca6 =SUM(ScaSca6),ScaSca7 =SUM(ScaSca7),ScaSca8 =SUM(ScaSca8)
FROM #TMPRIE WHERE CLFOPI = @CLFO
GROUP BY ScaConto,SCADESC,CLFOPI
 Order by SCACONTO


USCITA:
GO
/****** Object:  StoredProcedure [dbo].[GeneraGruppo]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc [dbo].[GeneraGruppo] @Cliente as varchar(5)
as

declare @result as smallint

set @result = 0

select * into #tmp from TbCli where ClCliFatt = @Cliente

if (select count(*) from #tmp) < 2
   begin
   set @result = 1
   end
   
if (select count(*) from #tmp) > 1 
   begin
   insert into TbGrv (GrvCliCons,GrvLisTest,GrvCliFatt)
   select ClCod , 0, @Cliente from #tmp
   where ClCod not in (select GrvCliCons from TbGrv)
   end
   
   
   
select @result


GO
/****** Object:  StoredProcedure [dbo].[GeneraPvendita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create Proc [dbo].[GeneraPvendita] @Cliente as varchar(5)
as

declare @result as smallint

set @result = 0

insert into TbGrv (GrvCliCons,GrvLisTest,GrvCliFatt)
select ClCod , 0, ClCliFatt
from TbCli
where ClCod = @Cliente and ClCod not in (select GrvCliCons from TbGrv)

   
   
select @result


GO
/****** Object:  StoredProcedure [dbo].[InitPrk]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[InitPrk] @Id as int
as
delete from TBPrk where PrkId = @id
insert into TbPrk(PrkId,PrkProg,PrkDa,PrkTipoCo,PrkConto,PrkAammgg,PrkDocAnn,PrkDocEst,PrkPAperta)
select priid,PriProg,'0',case when substring(priCoDare,3,1) = '.' then '0' else '1' end,PriCoDare,
case when PRICAUSALE < 4 then PriDataGio else PridataEst end,
PriDocAnn,PriDocEst,0
from TbPri where PriiD = @id AND PriCodare <> '00.10' AND PRICAUSALE <> 1 --- prima i dare
insert into TbPrk(PrkId,PrkProg,PrkDa,PrkTipoCo,PrkConto,PrkAammgg,PrkDocAnn,PrkDocEst,PrkPAperta)
select priid,PriProg,'1',case when substring(priCoAvere,3,1) = '.' then '0' else '1' end,PriCoAvere,
case when PRICAUSALE < 4 then PriDataGio else PridataEst end,
PriDocAnn,PriDocEst,0
from TbPri where PriiD = @id AND PriCoAvere <> '00.10' AND PRICAUSALE <> 2 ---- poi Avere


GO
/****** Object:  StoredProcedure [dbo].[MULTIPARTITA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[MULTIPARTITA] @Id AS int ,@miglio as int
AS
CREATE TABLE [dbo].[#MAPCH] (
	[ApcConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ApcNdoc] [int] NOT NULL ,
	[ApcAnno] [smallint] 
) ON [PRIMARY]
DECLARE @CONTO as VARCHAR(5),@ANNO as SMALLInt,@NDOC as int,@RIF AS INT

insert into #MAPCH(ApcConto,ApcAnno,ApcNdoc)
SELECT distinct PRKCONTO,PRKDOCANN,PRKDOCEST FROM TbPrk WHERE PrkId=@Id AND PRKTIPOCO=1

DECLARE XERGENTE CURSOR FOR
SELECT distinct ApcConto,ApcAnno ,ApcNdoc from [#MAPCH]
OPEN XERGENTE
FETCH NEXT FROM XERGENTE
INTO @CONTO,@ANNO,@NDOC
WHILE @@FETCH_STATUS = 0
BEGIN
IF ISNUMERIC(@CONTO)=1 and @CONTO not in (select RIvaCliCee from tbregiva where RIvaAnno=@ANNO AND RIvaCliCee > 0)
BEGIN
SET @RIF = (Select DISTINCT ScaRifId from TbSca where ScaConto = @CONTO AND DATEPART(YEAR,SCADDOC) = @ANNO AND SCANDOC = @NDOC AND ISNUMERIC(@CONTO)=1)
IF @RIF > 0  
BEGIN
EXEC RiScadenza @id = @RIF, @az = 0,@miglio =@miglio
END
END
XFINE_LOOP:
   FETCH NEXT FROM XERGENTE
   INTO @CONTO,@ANNO,@NDOC
END
CLOSE XERGENTE
DEALLOCATE XERGENTE

GO
/****** Object:  StoredProcedure [dbo].[OldControlloPVendita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[OldControlloPVendita] @GRUPPO as varchar(5), @DaData as smalldatetime, @AData as smalldatetime
as

select 
DATA = Fatdata,
PVENDITA = FatCliCons,
RAGSOC = (Select AnaDesc from Vdox.dbo.TbAna where AnaGrp = 'CL' AND AnaCod = FatCliCons),
GRUPPO = FatCliFat,
FAMIGLIA = ArtFamiglia,
DESCFAMIGLIA = ISNULL((select FamDesc from TbFam where FamCod = ArtFamiglia),''),
CODICE = CorCodArt,
DESCRIZIONE = ArtDesc,
IMBALLO = isnull((select Matdesc from TbMat where MatCod = CorCodImballo),''),
UMACQ = CorUMisVen,
QACQ = CorQuaVen,
VACQ = CorImporto,
QVEN = CorQuaVen,
VVEN = CorQuaven * ISNULL(dbo.fnprezzoLis(CorCodArt,GrvLisTest,FatData),0)
into #TMP
from TbCor inner join
     TbFat on Cortipodoc = FatTipoDoc inner join
     TbArt on ArtCod = CorCodArt inner join
     TbGrv on GrvCliCons = fatCliCons
where corcau = 5  AND FatCliFat = @GRUPPO and FatData between @DaData and @AData

SELECT
GRUPPO,PVENDITA,RAGSOC,DESCFAMIGLIA,CODICE,DESCRIZIONE,IMBALLO,UMACQ,
QACQ = SUM(QACQ),
VACQ =  SUM(VACQ),
PMACQ =CAST( (SUM(VACQ) / SUM(QACQ)) AS DECIMAL(7,2)),
QGIACE = 0,
QVEN =  SUM(QVEN),
VVEN =CAST( SUM(VVEN) AS DECIMAL(12,2))
from #TMP
group by GRUPPO,PVENDITA,RAGSOC,FAMIGLIA,DESCFAMIGLIA,CODICE,DESCRIZIONE,IMBALLO,UMACQ
order by GRUPPO,PVENDITA,FAMIGLIA,CODICE


     


GO
/****** Object:  StoredProcedure [dbo].[PCREASCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[PCREASCADENZE] @IDP as int
AS
CREATE TABLE [dbo].[#TSca] (
[ScaId] [int] IDENTITY (1, 1) NOT NULL ,
	[ScaTipoCo] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaNdoc] [int] NOT NULL ,
	[ScaImpDoc] [decimal](13, 2) NOT NULL ,
        [ScaIvaSpe] [decimal](13, 2) NOT NULL ,
	[ScaAbi] [int] NOT NULL ,
	[ScaCab] [int] NOT NULL ,
	[ScaImpRata] [decimal](13, 2) NOT NULL ,
	[ScaTPag] [smallint] NOT NULL ,
	[ScaCodPag] [smallint] NOT NULL ,
	[ScaNRata] [smallint] NOT NULL ,
	[ScaBan] [smallint] NOT NULL ,
	[ScaDdoc] [smalldatetime] NOT NULL ,
	[ScaDsca] [smalldatetime] NOT NULL ,
	[ScaRifId] [int] NOT NULL ,
	[ScaRifProg] [smallint] NOT NULL ,
	[ScaRifDA] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaRAperta] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaImpPagato] [decimal](13, 2) NOT NULL 
) ON [PRIMARY]

declare @Cmd varchar(2000),@DbWhere varchar(200)
IF @IDP > 0
   begin
 SET @DbWhere = ' And PriiD = '+CAST(@IDP AS VARCHAR(10))
   end
else
   begin
 SET @DbWhere = ''
 delete from TBSCA
 DBCC CHECKIDENT (TBSCA, RESEED, 0)
   end


set @cmd ='INSERT INTO [dbo].[#TSca] ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]) 
SELECT PRKTIPOCO,PRKCONTO,PRKDOCEST,CASE WHEN PRKDA = 0 THEN PRIIMPDARE ELSE PRIIMPAVERE END,CASE WHEN PRKDA = 0 THEN PRIIMPAVERE ELSE PRIIMPDARE END,0,0,CASE WHEN PRKDA = 0 THEN PRIIMPDARE ELSE PRIIMPAVERE END,
ISNULL(PAGTIPO,0),PRICODPAG,0,CAST(PRILINEA AS SMALLINT),PRIDATAEST,PRIDATAGIO,PRIID,PRIPROG,PRKDA,'''',0 FROM tbpri 
INNER JOIN TBPRK ON PRIID = PRKID AND PRIPROG = PRKPROG
INNER JOIN TBPAG ON PRICODPAG = PAGCOD 
WHERE PRICAUSALE = 3 AND PRKTIPOCO = 1'+@DbWhere
EXEC (@cmd)

DECLARE @Fatt as decimal(12,2),@IvaSp as decimal(12,2),@DataFatt as smalldatetime,@CPag as smallint,@ID as int,@NR as smallint,@P AS SMALLINT,@RRATA AS DECIMAL(12,2),@SSCAD AS SMALLDATETIME,
@PID AS INT
DECLARE SERGENTE CURSOR FOR
SELECT SCAID,SCAIMPDOC,SCAIVASPE,SCADDOC,SCACODPAG ,SCARIFID FROM #TSCA --where SCAID = 834
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @id,@fatt,@IvaSp,@datafatt,@cpag,@pid
WHILE @@FETCH_STATUS = 0
BEGIN
SET @P = 0

DECLARE @FattURA as decimal(12,2),@SPESE as decimal(12,2),@DataFattura as smalldatetime,@CodPag as smallint

set @Fattura=@fatt
set @Spese=@IvaSp
set @DataFattura=@DataFatt
set @CodPag=@Cpag


if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '#tmp'))
   begin
   drop table #tmp
  end 
CREATE TABLE #tmp (
	[NR] [smallint] NOT NULL ,
	[RATA] [decimal](12,2),
	[SCADENZA] [smalldatetime] NOT NULL 
) ON [PRIMARY]
declare @PagMeseE1 as smallint,@PagMeseE2 as smallint,@PagGgmmE1 as smalldatetime,@PagGgmmE2 as smalldatetime,
@PagSlitE1 as varchar(1),@PagSlitE2 as varchar(2),@PagNRate as smallint,@PagTestGM as smallint,@PagTestS1 as smallint,
@PagTestR1 as smallint
declare @Imponibile as decimal(12,2),@ImpRata as Decimal(12,2),@Resto as Decimal(12,2),@Molti as smallint,@NRA as smallint
declare @VetIm as decimal(12,2),@VetDt as smalldatetime,@PlusDate as smallint,@DayGG as smallint, @DbRate as varchar(50),@DbDate as varchar(50)
declare @current as smalldatetime,@dada as varchar(20)
set @PagMeseE1 = (select PagMeseE1 from TbPag Where @CodPag = PagCod)
set @PagMeseE2 = (select PagMeseE2 from TbPag Where @CodPag = PagCod)
set @PagGgmmE1 = (select PagGgmmE1 from TbPag Where @CodPag = PagCod)
set @PagGgmmE2 = (select PagGgmmE2 from TbPag Where @CodPag = PagCod)
set @PagSlitE1 = (select PagSlitE1 from TbPag Where @CodPag = PagCod)
set @PagSlitE2 = (select PagSlitE2 from TbPag Where @CodPag = PagCod)
set @PagNRate  = (select PagNRate  from TbPag Where @CodPag = PagCod)
set @PagTestGM = (select PagTestGM from TbPag Where @CodPag = PagCod)
set @PagTestS1 = (select PagTestS1 from TbPag Where @CodPag = PagCod)
set @PagTestR1 = (select PagTestR1 from TbPag Where @CodPag = PagCod)
set @molti = 0
if @PagTestR1 = 1
   begin
   SET @Imponibile = @Fattura
   set @Spese = 0
   end
else
   begin
   SET @Imponibile = @Fattura - @Spese
end
set @ImpRata = round(@Imponibile / @PagNRate,2)
set @resto = 0
set @nra = 0
Loop_Rate:
begin
set @Nra = @Nra +1
set @vetim = @ImpRata
if @nra = 1 set @vetim = @vetim + @spese
set @Resto = @resto + @vetim
--Set @DbRate = 'PagRata'+cast(@Nra as varchar(2))
--Set @DbDate = 'PagData'+cast(@Nra as varchar(2))
if @nra = 1 
begin
SET @PlusDate =(select PagRata1 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData1 from TbPag Where @CodPag = PagCod)
end
if @nra = 2
begin 
SET @PlusDate =(select PagRata2 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData2 from TbPag Where @CodPag = PagCod)
end
if @nra = 3 
begin
SET @PlusDate =(select PagRata3 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData3 from TbPag Where @CodPag = PagCod)
end
if @nra = 4 
begin
SET @PlusDate =(select PagRata4 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData4 from TbPag Where @CodPag = PagCod)
end
if @nra = 5 
begin
SET @PlusDate =(select PagRata5 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData5 from TbPag Where @CodPag = PagCod)
end
if @nra = 6 
begin
SET @PlusDate =(select PagRata6 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData6 from TbPag Where @CodPag = PagCod)
end
if @nra = 7 
begin
SET @PlusDate =(select PagRata7 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData7 from TbPag Where @CodPag = PagCod)
end
if @nra = 8 
begin
SET @PlusDate =(select PagRata8 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData8 from TbPag Where @CodPag = PagCod)
end
if @nra = 9 
begin
SET @PlusDate =(select PagRata9 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData9 from TbPag Where @CodPag = PagCod)
end
if @nra = 10 
begin
SET @PlusDate =(select PagRata10 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData10 from TbPag Where @CodPag = PagCod)
end
if @nra = 11 
begin
SET @PlusDate =(select PagRata11 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData11 from TbPag Where @CodPag = PagCod)
end
if @nra = 12 
begin
SET @PlusDate =(select PagRata12 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData12 from TbPag Where @CodPag = PagCod)
end
if @nra = 13 
begin
SET @PlusDate =(select PagRata13 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData13 from TbPag Where @CodPag = PagCod)
end
if @nra = 14 
begin
SET @PlusDate =(select PagRata14 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData14 from TbPag Where @CodPag = PagCod)
end
if @nra = 15 
begin
SET @PlusDate =(select PagRata15 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData15 from TbPag Where @CodPag = PagCod)
end
if @nra = 16 
begin
SET @PlusDate =(select PagRata16 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData16 from TbPag Where @CodPag = PagCod)
end
if @nra = 17 
begin
SET @PlusDate =(select PagRata17 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData17 from TbPag Where @CodPag = PagCod)
end
if @nra = 18 
begin
SET @PlusDate =(select PagRata18 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData18 from TbPag Where @CodPag = PagCod)
end
if @nra = 19 
begin
SET @PlusDate =(select PagRata19 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData19 from TbPag Where @CodPag = PagCod)
end
if @nra = 20 
begin
SET @PlusDate =(select PagRata20 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData20 from TbPag Where @CodPag = PagCod)
end
if @nra = 21 
begin
SET @PlusDate =(select PagRata21 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData21 from TbPag Where @CodPag = PagCod)
end
if @nra = 22 
begin
SET @PlusDate =(select PagRata22 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData22 from TbPag Where @CodPag = PagCod)
end
if @nra = 23 
begin
SET @PlusDate =(select PagRata23 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData23 from TbPag Where @CodPag = PagCod)
end
if @nra = 24 
begin
SET @PlusDate =(select PagRata24 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData24 from TbPag Where @CodPag = PagCod)
end
set @PlusDate = @plusDate + @molti
if @PagTestGm = 1 
begin
set @vetdt = dateadd(day,@Plusdate,@DataFattura)
end
else
begin
set @vetdt = dateadd(Month,@Plusdate,@DataFattura)
end
if @PagTestS1 = 2 
begin
set @current = @vetdt
set @dada ='01/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @current = CONVERT(VARCHAR(20),@Dada,103)
set @current = dateadd(Month,1,@current)
set @Vetdt = dateadd(day,-1,@current)
end
if @DayGG > 0 
begin
set @current = @vetdt
set @dada =cast(@Daygg as varchar(2))+'/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)
end
set @current = @vetdt
if datepart(month,@Current) = @PagMeseE1
begin
set @vetdt = @PagGgmmE1
  IF @PagSlitE1 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end
if datepart(month,@Current) = @PagMeseE2
begin
set @vetdt = @PagGgmmE2
  IF @PagSlitE2 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end
if @nra = @PagNRate set @vetim = @vetim + @Fattura - @Resto 
--SET @CMD = 'RATA N.'+CAst(@nra as varchar(2)) +' IMPORTO: '+cast(@Vetim as varchar(15))+ ' SCADE AL '+ CONVERT(VARCHAR(20),@VetDt,103) +' IMPORTO FATTURA: '+cast(@FATTURA as varchar(15))+ ' RESTO RATA: ' +cast(@RESTO as varchar(15))
--print @CMD
INSERT INTO #TMP (nr,rata,scadenza)
SELECT @NRA ,@VETIM ,@VETDT 
end
IF @Nra < @PagNRate GOTO Loop_Rate

SET @NR = (SELECT COUNT(*) FROM #tmp)
IF @NR = 0 GOTO FINE_LOOP
lOOP_SCADE:
SET @P = @P + 1
SET @RRATA = (SELECT RATA FROM #TMP WHERE NR = @P)
SET @SSCAD = (SELECT SCADENZA FROM #TMP WHERE NR = @P)

INSERT INTO [dbo].[TbSca] ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato])
SELECT                     [ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],@RRATA,[ScaTPag],[ScaCodPag],
	                   @P,[ScaBan],[ScaDdoc],@SSCAD,[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]
FROM [dbo].[#TSca] WHERE SCAID = @ID
IF @P < @NR GOTO LOOP_SCADE
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @id,@fatt,@IvaSp,@datafatt,@cpag,@pid
END
CLOSE SERGENTE
DEALLOCATE SERGENTE

DROP TABLE [dbo].[#TSca]

GO
/****** Object:  StoredProcedure [dbo].[PNOTACREASCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[PNOTACREASCADENZE] 
AS

CREATE TABLE [dbo].[#TSca] (
[ScaId] [int] IDENTITY (1, 1) NOT NULL ,
	[ScaTipoCo] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaNdoc] [int] NOT NULL ,
	[ScaImpDoc] [decimal](13, 2) NOT NULL ,
    [ScaIvaSpe] [decimal](13, 2) NOT NULL ,
	[ScaAbi] [int] NOT NULL ,
	[ScaCab] [int] NOT NULL ,
	[ScaImpRata] [decimal](13, 2) NOT NULL ,
	[ScaTPag] [smallint] NOT NULL ,
	[ScaCodPag] [smallint] NOT NULL ,
	[ScaNRata] [smallint] NOT NULL ,
	[ScaBan] [smallint] NOT NULL ,
	[ScaDdoc] [smalldatetime] NOT NULL ,
	[ScaDsca] [smalldatetime] NOT NULL ,
	[ScaRifId] [int] NOT NULL ,
	[ScaRifProg] [smallint] NOT NULL ,
	[ScaRifDA] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaRAperta] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaImpPagato] [decimal](13, 2) NOT NULL 
) ON [PRIMARY]
CREATE TABLE #tmp (
	[NR] [smallint] NOT NULL ,
	[RATA] [decimal](12,2),
	[SCADENZA] [smalldatetime] NOT NULL 
) ON [PRIMARY]

select * into #BASE from TbPri where PriDataEst ='23/12/2013' and PriCausale = 33 and substring(pricodare,3,1)<>'.' and PriCoDare < (select grpmigl from TbGrp where grpcod ='CL')


INSERT INTO [dbo].[#TSca] ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]) 

SELECT PRKTIPOCO,PRKCONTO,PRKDOCEST,CASE WHEN PRKDA = 0 THEN B.PRIIMPDARE ELSE B.PRIIMPAVERE END,CASE WHEN PRKDA = 0 THEN B.PRIIMPAVERE ELSE B.PRIIMPDARE END,0,0,
CASE WHEN PRKDA = 0 THEN a.PRIIMPDARE ELSE a.PRIIMPAVERE END,
ISNULL(PAGTIPO,0),b.PRICODPAG,0,CAST(b.PRILINEA AS SMALLINT),B.PRIDATAEST,A.PRIDATAGIO,A.PRIID,A.PRIPROG,PRKDA,'',0
 from #BASE as a 
inner join pastaeco.coge.dbo.TbPri as b on A.pridocest = b.PriDocEst and A.pricodare = b.PriCoDare and b.pridocann = DATEPART(year,a.pridataest)
INNER JOIN pastaeco.coge.dbo.TBPRK ON B.PRIID = PRKID AND B.PRIPROG = PRKPROG AND PrkDa = '0'
INNER JOIN TBPAG ON B.PRICODPAG = PAGCOD 
where b.PriCausale = 3 

--DROP TABLE #TSCA
--DROP TABLE #BASE
--DROP TABLE #TMP





DECLARE @Fatt as decimal(12,2),@IvaSp as decimal(12,2),@DataFatt as smalldatetime,@CPag as smallint,@ID as int,@NR as smallint,@P AS SMALLINT,@RRATA AS DECIMAL(12,2),@SSCAD AS SMALLDATETIME,
@PID AS INT
DECLARE SERGENTE CURSOR FOR
SELECT SCAID,SCAIMPRATA,SCAIVASPE,SCADDOC,SCACODPAG ,SCARIFID FROM #TSCA --where SCAID = 834
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @id,@fatt,@IvaSp,@datafatt,@cpag,@pid
WHILE @@FETCH_STATUS = 0
BEGIN
SET @P = 0

DECLARE @FattURA as decimal(12,2),@SPESE as decimal(12,2),@DataFattura as smalldatetime,@CodPag as smallint

set @Fattura=@fatt
set @Spese=@IvaSp
set @DataFattura=@DataFatt
set @CodPag=@Cpag


DELETE FROM #TMP

declare @PagMeseE1 as smallint,@PagMeseE2 as smallint,@PagGgmmE1 as smalldatetime,@PagGgmmE2 as smalldatetime,
@PagSlitE1 as varchar(1),@PagSlitE2 as varchar(2),@PagNRate as smallint,@PagTestGM as smallint,@PagTestS1 as smallint,
@PagTestR1 as smallint
declare @Imponibile as decimal(12,2),@ImpRata as Decimal(12,2),@Resto as Decimal(12,2),@Molti as smallint,@NRA as smallint
declare @VetIm as decimal(12,2),@VetDt as smalldatetime,@PlusDate as smallint,@DayGG as smallint, @DbRate as varchar(50),@DbDate as varchar(50)
declare @current as smalldatetime,@dada as varchar(20)
set @PagMeseE1 = (select PagMeseE1 from TbPag Where @CodPag = PagCod)
set @PagMeseE2 = (select PagMeseE2 from TbPag Where @CodPag = PagCod)
set @PagGgmmE1 = (select PagGgmmE1 from TbPag Where @CodPag = PagCod)
set @PagGgmmE2 = (select PagGgmmE2 from TbPag Where @CodPag = PagCod)
set @PagSlitE1 = (select PagSlitE1 from TbPag Where @CodPag = PagCod)
set @PagSlitE2 = (select PagSlitE2 from TbPag Where @CodPag = PagCod)
set @PagNRate  = (select PagNRate  from TbPag Where @CodPag = PagCod)
set @PagTestGM = (select PagTestGM from TbPag Where @CodPag = PagCod)
set @PagTestS1 = (select PagTestS1 from TbPag Where @CodPag = PagCod)
set @PagTestR1 = (select PagTestR1 from TbPag Where @CodPag = PagCod)
set @molti = 0
if @PagTestR1 = 1
   begin
   SET @Imponibile = @Fattura
   set @Spese = 0
   end
else
   begin
   SET @Imponibile = @Fattura - @Spese
end
set @ImpRata = round(@Imponibile / @PagNRate,2)
set @resto = 0
set @nra = 0
Loop_Rate:
begin
set @Nra = @Nra +1
set @vetim = @ImpRata
if @nra = 1 set @vetim = @vetim + @spese
set @Resto = @resto + @vetim
--Set @DbRate = 'PagRata'+cast(@Nra as varchar(2))
--Set @DbDate = 'PagData'+cast(@Nra as varchar(2))
if @nra = 1 
begin
SET @PlusDate =(select PagRata1 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData1 from TbPag Where @CodPag = PagCod)
end
if @nra = 2
begin 
SET @PlusDate =(select PagRata2 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData2 from TbPag Where @CodPag = PagCod)
end
if @nra = 3 
begin
SET @PlusDate =(select PagRata3 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData3 from TbPag Where @CodPag = PagCod)
end
if @nra = 4 
begin
SET @PlusDate =(select PagRata4 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData4 from TbPag Where @CodPag = PagCod)
end
if @nra = 5 
begin
SET @PlusDate =(select PagRata5 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData5 from TbPag Where @CodPag = PagCod)
end
if @nra = 6 
begin
SET @PlusDate =(select PagRata6 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData6 from TbPag Where @CodPag = PagCod)
end
if @nra = 7 
begin
SET @PlusDate =(select PagRata7 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData7 from TbPag Where @CodPag = PagCod)
end
if @nra = 8 
begin
SET @PlusDate =(select PagRata8 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData8 from TbPag Where @CodPag = PagCod)
end
if @nra = 9 
begin
SET @PlusDate =(select PagRata9 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData9 from TbPag Where @CodPag = PagCod)
end
if @nra = 10 
begin
SET @PlusDate =(select PagRata10 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData10 from TbPag Where @CodPag = PagCod)
end
if @nra = 11 
begin
SET @PlusDate =(select PagRata11 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData11 from TbPag Where @CodPag = PagCod)
end
if @nra = 12 
begin
SET @PlusDate =(select PagRata12 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData12 from TbPag Where @CodPag = PagCod)
end
if @nra = 13 
begin
SET @PlusDate =(select PagRata13 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData13 from TbPag Where @CodPag = PagCod)
end
if @nra = 14 
begin
SET @PlusDate =(select PagRata14 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData14 from TbPag Where @CodPag = PagCod)
end
if @nra = 15 
begin
SET @PlusDate =(select PagRata15 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData15 from TbPag Where @CodPag = PagCod)
end
if @nra = 16 
begin
SET @PlusDate =(select PagRata16 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData16 from TbPag Where @CodPag = PagCod)
end
if @nra = 17 
begin
SET @PlusDate =(select PagRata17 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData17 from TbPag Where @CodPag = PagCod)
end
if @nra = 18 
begin
SET @PlusDate =(select PagRata18 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData18 from TbPag Where @CodPag = PagCod)
end
if @nra = 19 
begin
SET @PlusDate =(select PagRata19 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData19 from TbPag Where @CodPag = PagCod)
end
if @nra = 20 
begin
SET @PlusDate =(select PagRata20 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData20 from TbPag Where @CodPag = PagCod)
end
if @nra = 21 
begin
SET @PlusDate =(select PagRata21 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData21 from TbPag Where @CodPag = PagCod)
end
if @nra = 22 
begin
SET @PlusDate =(select PagRata22 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData22 from TbPag Where @CodPag = PagCod)
end
if @nra = 23 
begin
SET @PlusDate =(select PagRata23 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData23 from TbPag Where @CodPag = PagCod)
end
if @nra = 24 
begin
SET @PlusDate =(select PagRata24 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData24 from TbPag Where @CodPag = PagCod)
end
set @PlusDate = @plusDate + @molti
if @PagTestGm = 1 
begin
set @vetdt = dateadd(day,@Plusdate,@DataFattura)
end
else
begin
set @vetdt = dateadd(Month,@Plusdate,@DataFattura)
end
if @PagTestS1 = 2 
begin
set @current = @vetdt
set @dada ='01/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @current = CONVERT(VARCHAR(20),@Dada,103)
set @current = dateadd(Month,1,@current)
set @Vetdt = dateadd(day,-1,@current)
end
if @DayGG > 0 
begin
set @current = @vetdt
set @dada =cast(@Daygg as varchar(2))+'/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)
end
set @current = @vetdt
if datepart(month,@Current) = @PagMeseE1
begin
set @vetdt = @PagGgmmE1
  IF @PagSlitE1 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end
if datepart(month,@Current) = @PagMeseE2
begin
set @vetdt = @PagGgmmE2
  IF @PagSlitE2 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end
if @nra = @PagNRate set @vetim = @vetim + @Fattura - @Resto 
--SET @CMD = 'RATA N.'+CAst(@nra as varchar(2)) +' IMPORTO: '+cast(@Vetim as varchar(15))+ ' SCADE AL '+ CONVERT(VARCHAR(20),@VetDt,103) +' IMPORTO FATTURA: '+cast(@FATTURA as varchar(15))+ ' RESTO RATA: ' +cast(@RESTO as varchar(15))
--print @CMD
INSERT INTO #TMP (nr,rata,scadenza)
SELECT @NRA ,@VETIM ,@VETDT 
end
IF @Nra < @PagNRate GOTO Loop_Rate

SET @NR = (SELECT COUNT(*) FROM #tmp)
IF @NR = 0 GOTO FINE_LOOP
lOOP_SCADE:
SET @P = @P + 1
SET @RRATA = (SELECT RATA FROM #TMP WHERE NR = @P)
SET @SSCAD = (SELECT SCADENZA FROM #TMP WHERE NR = @P)

--INSERT INTO [dbo].[TbSca] ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
--	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato])
--SELECT                     [ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],@RRATA,[ScaTPag],[ScaCodPag],
--	                   @P,[ScaBan],[ScaDdoc],@SSCAD,[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]
	                   
UPDATE [dbo].[#TSca] SET  ScaNRata =@P,ScaDsca=@SSCAD  WHERE SCAID = @ID         
	                   
--FROM [dbo].[#TSca] WHERE SCAID = @ID
IF @P < @NR GOTO LOOP_SCADE
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @id,@fatt,@IvaSp,@datafatt,@cpag,@pid
END
CLOSE SERGENTE
DEALLOCATE SERGENTE

INSERT INTO [dbo].[TbSca] ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato])
SELECT                     [ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]
	                   FROM [dbo].[#TSca]
GO
/****** Object:  StoredProcedure [dbo].[POINTSCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[POINTSCADENZE]
AS
declare @p as int
set @p = (SELECT ISNULL(MAX(SCAID),0) FROM TBsca)
dbcc checkident ('TBSCA',RESEED,@p)

GO
/****** Object:  StoredProcedure [dbo].[ProdVenduti]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc [dbo].[ProdVenduti] @PVENDITA as varchar(5), @DaData as smalldatetime, @AData as smalldatetime, @DataInv as smalldatetime
as

select distinct
CODICE = CorCodArt,
CODIMBALLO = CorCodImballo,
UMACQ = CorUMisVen
into #tmp
from TbCor inner join
     TbFat on Cortipodoc = FatTipoDoc 
where corcau = 5 and corcodart  > 0 AND FatCliCons = @PVENDITA and FatData between @DADATA and @ADATA 
group by CorCodArt,CorCodImballo,CorUMisVen

delete from #tmp
from #tmp inner join
     (select * from TbInP where InPCliCons = @PVENDITA and InPData = @DataInv) a on a.InPCodArt = CODICE and a.InPImballo = CODIMBALLO

SELECT
CODICE,
DESCRIZIONE = ArtDesc,
CODIMBALLO,
IMBALLO = isnull((select Matdesc from TbMat where MatCod = CODIMBALLO),''),
UMACQ 
from #TMP inner join
     TbArt on ArtCod = CODICE 
order by CODICE

GO
/****** Object:  StoredProcedure [dbo].[RATEESCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[RATEESCADENZE] @Fattura as decimal(12,2),@Spese as decimal(12,2),@DataFattura as smalldatetime,@CodPag as smallint
as

--DECLARE @Fattura as decimal(12,2),@Spese as decimal(12,2),@DataFattura as smalldatetime,@CodPag as smallint

--SET @FATTURA=1200.00
--SET @SPESE=200.00
--SET @DATAFATTURA=GETDATE()
--SET @CODPAG=200

if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##tmp'))
   begin
   drop table ##tmp
  end 
CREATE TABLE ##tmp (
	[NR] [smallint] NOT NULL ,
	[RATA] [decimal](12,2),
	[SCADENZA] [smalldatetime] NOT NULL 
) ON [PRIMARY]

if @codpag = 0 
begin
INSERT INTO ##TMP (nr,rata,scadenza)
SELECT 1 ,@Fattura ,@DataFattura 
GOTO USCITA
end

declare @PagMeseE1 as smallint,@PagMeseE2 as smallint,@PagGgmmE1 as smalldatetime,@PagGgmmE2 as smalldatetime,
@PagSlitE1 as varchar(1),@PagSlitE2 as varchar(2),@PagNRate as smallint,@PagTestGM as smallint,@PagTestS1 as smallint,
@PagTestR1 as smallint
declare @Imponibile as decimal(12,2),@ImpRata as Decimal(12,2),@Resto as Decimal(12,2),@Molti as smallint,@NRA as smallint,@PA as tinyint
declare @VetIm as decimal(12,2),@VetDt as smalldatetime,@PlusDate as smallint,@DayGG as smallint, @DbRate as varchar(50),@DbDate as varchar(50),@CMD AS VARCHAR (1000)
declare @current as smalldatetime,@dada as varchar(20)
set @PagMeseE1 = (select PagMeseE1 from TbPag Where @CodPag = PagCod)
set @PagMeseE2 = (select PagMeseE2 from TbPag Where @CodPag = PagCod)
set @PagGgmmE1 = (select PagGgmmE1 from TbPag Where @CodPag = PagCod)
set @PagGgmmE2 = (select PagGgmmE2 from TbPag Where @CodPag = PagCod)
set @PagSlitE1 = (select PagSlitE1 from TbPag Where @CodPag = PagCod)
set @PagSlitE2 = (select PagSlitE2 from TbPag Where @CodPag = PagCod)
set @PagNRate  = (select PagNRate  from TbPag Where @CodPag = PagCod)
set @PagTestGM = (select PagTestGM from TbPag Where @CodPag = PagCod)
set @PagTestS1 = (select PagTestS1 from TbPag Where @CodPag = PagCod)
set @PagTestR1 = (select PagTestR1 from TbPag Where @CodPag = PagCod)
set @molti = 0
set @resto = 0
set @nra = 0
set @PA = 0

if @PagTestR1 = 1
   begin
   SET @Imponibile = @Fattura
   set @Spese = 0
   set @ImpRata = round(@Imponibile / @PagNRate,2)
   end
   if @PagTestR1 = 2
   begin
   SET @Imponibile = @Fattura - @Spese
   set @ImpRata = round(@Imponibile / @PagNRate,2)
   end
   if @PagTestR1 = 3
   begin
   SET @Imponibile = @Fattura - @Spese
   set @ImpRata = round(@Imponibile / (@PagNRate -1),2)
   set @Nra = 1
   set @vetim = @Spese
   set @Resto = @Spese
   SET @PlusDate =(select PagRata1 from TbPag Where @CodPag = PagCod)
   SET @DayGG = (select PagData1 from TbPag Where @CodPag = PagCod)

   if @PagTestGm = 1 
   begin
   set @vetdt = dateadd(day,@Plusdate,@DataFattura)
   end
   else
   begin
   set @vetdt = dateadd(Month,@Plusdate,@DataFattura)
   end

   if @PagTestS1 = 2 
   begin
   set @current = @vetdt
   set @dada ='01/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
   set @current = CONVERT(VARCHAR(20),@Dada,103)
   set @current = dateadd(Month,1,@current)
   set @Vetdt = dateadd(day,-1,@current)
   end

   if @DayGG > 0 
   begin
   set @current = @vetdt
   set @dada =cast(@Daygg as varchar(2))+'/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
   set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)
   end

   set @current = @vetdt

   if datepart(month,@Current) = @PagMeseE1
   begin
   set @vetdt = @PagGgmmE1
   IF @PagSlitE1 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
    end

    if datepart(month,@Current) = @PagMeseE2
    begin
    set @vetdt = @PagGgmmE2
    IF @PagSlitE2 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
    end
	INSERT INTO ##TMP (nr,rata,scadenza)
    SELECT @NRA ,@VETIM ,@VETDT 
 end
   	  

Loop_Rate:
begin
set @Nra = @Nra +1
set @vetim = @ImpRata
if @nra = 1 set @vetim = @vetim + @spese
set @Resto = @resto + @vetim
--Set @DbRate = 'PagRata'+cast(@Nra as varchar(2))
--Set @DbDate = 'PagData'+cast(@Nra as varchar(2))
if @nra = 1 
begin
SET @PlusDate =(select PagRata1 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData1 from TbPag Where @CodPag = PagCod)
end
if @nra = 2
begin 
SET @PlusDate =(select PagRata2 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData2 from TbPag Where @CodPag = PagCod)
end
if @nra = 3 
begin
SET @PlusDate =(select PagRata3 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData3 from TbPag Where @CodPag = PagCod)
end
if @nra = 4 
begin
SET @PlusDate =(select PagRata4 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData4 from TbPag Where @CodPag = PagCod)
end
if @nra = 5 
begin
SET @PlusDate =(select PagRata5 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData5 from TbPag Where @CodPag = PagCod)
end
if @nra = 6 
begin
SET @PlusDate =(select PagRata6 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData6 from TbPag Where @CodPag = PagCod)
end
if @nra = 7 
begin
SET @PlusDate =(select PagRata7 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData7 from TbPag Where @CodPag = PagCod)
end
if @nra = 8 
begin
SET @PlusDate =(select PagRata8 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData8 from TbPag Where @CodPag = PagCod)
end
if @nra = 9 
begin
SET @PlusDate =(select PagRata9 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData9 from TbPag Where @CodPag = PagCod)
end
if @nra = 10 
begin
SET @PlusDate =(select PagRata10 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData10 from TbPag Where @CodPag = PagCod)
end
if @nra = 11 
begin
SET @PlusDate =(select PagRata11 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData11 from TbPag Where @CodPag = PagCod)
end
if @nra = 12 
begin
SET @PlusDate =(select PagRata12 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData12 from TbPag Where @CodPag = PagCod)
end
if @nra = 13 
begin
SET @PlusDate =(select PagRata13 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData13 from TbPag Where @CodPag = PagCod)
end
if @nra = 14 
begin
SET @PlusDate =(select PagRata14 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData14 from TbPag Where @CodPag = PagCod)
end
if @nra = 15 
begin
SET @PlusDate =(select PagRata15 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData15 from TbPag Where @CodPag = PagCod)
end
if @nra = 16 
begin
SET @PlusDate =(select PagRata16 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData16 from TbPag Where @CodPag = PagCod)
end
if @nra = 17 
begin
SET @PlusDate =(select PagRata17 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData17 from TbPag Where @CodPag = PagCod)
end
if @nra = 18 
begin
SET @PlusDate =(select PagRata18 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData18 from TbPag Where @CodPag = PagCod)
end
if @nra = 19 
begin
SET @PlusDate =(select PagRata19 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData19 from TbPag Where @CodPag = PagCod)
end
if @nra = 20 
begin
SET @PlusDate =(select PagRata20 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData20 from TbPag Where @CodPag = PagCod)
end
if @nra = 21 
begin
SET @PlusDate =(select PagRata21 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData21 from TbPag Where @CodPag = PagCod)
end
if @nra = 22 
begin
SET @PlusDate =(select PagRata22 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData22 from TbPag Where @CodPag = PagCod)
end
if @nra = 23 
begin
SET @PlusDate =(select PagRata23 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData23 from TbPag Where @CodPag = PagCod)
end
if @nra = 24 
begin
SET @PlusDate =(select PagRata24 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData24 from TbPag Where @CodPag = PagCod)
end
set @PlusDate = @plusDate + @molti
if @PagTestGm = 1 
begin
set @vetdt = dateadd(day,@Plusdate,@DataFattura)
end
else
begin
set @vetdt = dateadd(Month,@Plusdate,@DataFattura)
end
if @PagTestS1 = 2 
begin
set @current = @vetdt
set @dada ='01/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @current = CONVERT(VARCHAR(20),@Dada,103)
set @current = dateadd(Month,1,@current)
set @Vetdt = dateadd(day,-1,@current)
end
if @DayGG > 0 
begin
set @current = @vetdt
set @dada =cast(@Daygg as varchar(2))+'/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)
end

set @current = @vetdt
SET @PA=0
if datepart(month,@Current) = @PagMeseE1
begin
if @PagMeseE1 = 12 
begin
SET @PA = 1
end

set @dada =cast(datepart(DAY,@PagGgmmE1) as varchar(2))+'/'+cast(datepart(month,@PagGgmmE1) as varchar(2))+'/'+cast(datepart(year ,@current) + @PA as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)

--set @vetdt = @PagGgmmE1
  IF @PagSlitE1 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end

if datepart(month,@Current) = @PagMeseE2
begin
if @PagMeseE2 = 12 
begin
SET @PA = 1
end

--set @vetdt = @PagGgmmE2

set @dada =cast(datepart(DAY,@PagGgmmE2) as varchar(2))+'/'+cast(datepart(month,@PagGgmmE2) as varchar(2))+'/'+cast(datepart(year,@current) + @PA as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)

  IF @PagSlitE2 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end
if @nra = @PagNRate set @vetim = @vetim + @Fattura - @Resto 
--SET @CMD = 'RATA N.'+CAst(@nra as varchar(2)) +' IMPORTO: '+cast(@Vetim as varchar(15))+ ' SCADE AL '+ CONVERT(VARCHAR(20),@VetDt,103) +' IMPORTO FATTURA: '+cast(@FATTURA as varchar(15))+ ' RESTO RATA: ' +cast(@RESTO as varchar(15))
--print @CMD
INSERT INTO ##TMP (nr,rata,scadenza)
SELECT @NRA ,@VETIM ,@VETDT 
end
IF @Nra < @PagNRate GOTO Loop_Rate

select * from ##tmp

USCITA:


GO
/****** Object:  StoredProcedure [dbo].[RDUPLICA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[RDUPLICA] @DAL as smalldatetime,@AL AS SMALLDATETIME
AS
------DUPLICA ARTICOLO RETTIFICA-----
DECLARE @P AS INT,@IIDD AS INT,@NP AS INT,@MC as INT
SET @NP =(SELECT ISNULL(MAX(PRINUMPROT),0) FROM TRPRI)  
DECLARE DATIRET CURSOR FOR
SELECT RETID from TRRET where RETDATA = @DAL ORDER BY RETID
OPEN DATIRET
FETCH NEXT FROM DATIRET
INTO @IIDD
WHILE @@FETCH_STATUS = 0
BEGIN

INSERT INTO TRIDP (IDdata) values(@AL)
SET @P = (SELECT @@IDENTITY)
SET @NP = @NP + 1
INSERT INTO TRPRI(PriId,PriProg,PriDataGio,PriCausale,PriCoDare,PriCoAvere,PriNumProt,PriBisRet,PriCodIva,
       PriRegIva,PriImpDare,PriImpAvere,PriDesc,PriDocEst,PriMeseSk,PriDataEst,PriDescB,PriFl04,
       PriFl05,PriFl06,PriNsRif,PriSos,PriLinea,PriDocAnn,PriCodPag,PriValuta,PriArtFisc,
       PriIvaPrint,PriGStampa)
SELECT @P,PriProg,@AL,PriCausale,PriCoDare,PriCoAvere,@NP,PriBisRet,PriCodIva,
       PriRegIva,PriImpDare,PriImpAvere,PriDesc,PriDocEst,PriMeseSk,@AL,PriDescB,PriFl04,
       PriFl05,PriFl06,PriNsRif,PriSos,PriLinea,PriDocAnn,PriCodPag,PriValuta,PriArtFisc,
       PriIvaPrint,PriGStampa FROM TRPRI WHERE PRIID = @IIDD ORDER BY PRIID,PRIPROG

INSERT INTO TRRET(RETID,RETARTICOLO,RETDATA,RETDESCR)
SELECT @P,@NP,@AL,(SELECT PRIDESC+PRIDESCB FROM TRPRI WHERE PRIID = @P AND PRIPROG = 1)

EXEC RInitPrk @Id = @P
if exists (select * from MASTER.dbo.SYSDATABASES where NAME = 'DCDC')
begin
INSERT INTO DCDC.dbo.TRIMC (IDMCDT) values(@AL)
SET @MC = (SELECT @@IDENTITY)

INSERT INTO DCDC.dbo.TRMCC (MCCID,MCCPROG,MCCPrkAammgg,MCCPrkId,MCCPrkProg,MCCPrkDa,
       MCCCogLdp,MCCCogCdc,MCCCogRep,MCCCogConto,MCCIMPORTO)
SELECT @MC,MCCPROG,@AL,@P,MCCPrkProg,MCCPrkDa,
       MCCCogLdp,MCCCogCdc,MCCCogRep,MCCCogConto,MCCIMPORTO FROM DCDC.dbo.TRMCC 
                 WHERE MCCPrkId = @IIDD ORDER BY MCCPrkId,MCCPrkProg
END
FINE_LOOP:
   FETCH NEXT FROM DATIRET
   INTO @IIDD
END
CLOSE DATIRET
DEALLOCATE DATIRET

DELETE FROM TRIDP WHERE IDDATA = @AL
if exists (select * from MASTER.dbo.SYSDATABASES where NAME = 'DCDC')
BEGIN
DELETE FROM DCDC.dbo.TRIMC WHERE IDMCDT = @AL
END


GO
/****** Object:  StoredProcedure [dbo].[RECUPEROSCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[RECUPEROSCADENZE]  @MIGLIAIO as int
AS
DECLARE @IIDD AS INT
DECLARE DATISCA CURSOR FOR
SELECT priid from Tbpri where pricausale = 3 and Pricodpag > 0
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @IIDD
WHILE @@FETCH_STATUS = 0
BEGIN

 EXEC XCREASCADENZE  @IDP = @IIDD
 EXEC RiChiudePartita  @Id = @IIDD ,@Miglio= @MIGLIAIO

FINE_LOOP:
   FETCH NEXT FROM DATISCA
   INTO @IIDD
END
CLOSE DATISCA
DEALLOCATE DATISCA

UPDATE TBSCA SET SCAABI = clabi,SCACAB = clcab FROM TBSCA Q, GEVE.DBO.TBCLI S WHERE Q.SCACONTO =S.clCod
UPDATE TBSCA SET SCAABI = foabi,SCACAB = focab FROM TBSCA Q, GEVE.DBO.TBFOR S WHERE Q.SCACONTO =S.FOCod


GO
/****** Object:  StoredProcedure [dbo].[RF1]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[RF1] @DAL as smalldatetime,@AL as smalldatetime,@BLOCK as int,@CAUS as smallint,@SW as smallint,@CAUCH as smallint,@FAL as smalldatetime
 as
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
SUBSTRING(prkconto,1,2),PRICAUSALE
from VH8
where PrkTipoCo = 0 and Prkaammgg between @DAL and @AL and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl,PRICAUSALE
UNION
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
SUBSTRING(prkconto,1,2),PRICAUSALE
from RH8
where PrkTipoCo = 0 and Prkaammgg = @FAL and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl,PRICAUSALE
----
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end, 
PRICAUSALE,0
from VH8 inner join TbGrp on GrpCod = 'CL' 
where PrkTipoCo = 1  and Prkaammgg between @DAL and @AL and PrkConto < GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,PRICAUSALE
UNION
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end, 
PRICAUSALE,0
from RH8 inner join TbGrp on GrpCod = 'CL' 
where PrkTipoCo = 1  and Prkaammgg =@FAL and PrkConto < GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,PRICAUSALE
---
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end ,
PRICAUSALE,1
from VH8 inner join TbGrp on GrpCod = 'FO' 
where PrkTipoCo = 1  and Prkaammgg between @DAL and @AL and PrkConto > GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,pricausale 
UNION
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end ,
PRICAUSALE,1
from RH8 inner join TbGrp on GrpCod = 'FO' 
where PrkTipoCo = 1  and Prkaammgg =@FAL and PrkConto > GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,pricausale 
---
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select distinct TMCBLOCK,TMCCPT,(SELECT PIAANACO FROM TBPIA WHERE PIACODCO = TMCCPT),(SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = TMCCPT),
SUM(TMCDARE),SUM(TMCAVERE),SUM(TMCSALDO),SUM(TMCDAREP),SUM(TMCAVEREP),SUM(TMCSALDOP),SUBSTRING(TMCCPT,1,2),0
from TMPBILC where TMCBLOCK = @BLOCK
GROUP BY TMCCPT,TMCBLOCK
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select @BLOCK,PIACODCO ,PIAANACO,0,0,0,0,0,0,0,SUBSTRING(PIACODCO,1,2),0 
FROM TBPIA WHERE SUBSTRING(PIACODCO,4,2) = '00' 
DECLARE @APERTURA AS DECIMAL(13,2),@CONTO AS VARCHAR(5)
SET @APERTURA = 0.0
SET @CONTO = (SELECT ESEBILAP FROM TBESE WHERE ESEANNO = DATEPART(YEAR,@DAL))
SET @APERTURA =(SELECT ISNULL(SUM(TMSSALDOP),0) FROM TMPBILS WHERE NOT EXISTS (SELECT * FROM tmpbils where TMSCONTO = @CONTO AND TMSCAUS = 45 and tmsblock = @block))
IF @APERTURA <> 0 
BEGIN
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select @BLOCK,@CONTO ,(SELECT PIAANACO FROM TBPIA WHERE PIACODCO = @CONTO),(SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = @CONTO),0,0,0,0,0,(@APERTURA * -1),SUBSTRING(@CONTO,1,2),45 
DELETE FROM TMPBILS WHERE TMSCONTO = @CONTO AND TMSCAUS <> 45 and tmsblock = @block
END
UPDATE TMPBILS SET TMSDEMAS = (select ISNULL(PIAANACO,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSMASTRO+'.00' ) WHERE TMSBLOCK = @BLOCK
IF @SW = 1
BEGIN
INSERT INTO TMPANNS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSPDARE,TMSPAVERE,TMSPSALDO,TMSPDAREP,TMSPAVEREP,TMSPSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT DISTINCT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,0,0,0,0,0,0,TMSMASTRO,TMSCAUS,TMSDEMAS
from TMPBILS where TMSBLOCK = @BLOCK
DELETE FROM TMPBILS WHERE TMSBLOCK = @BLOCK 
---DELETE FROM TMPBILC WHERE TMCBLOCK = @BLOCK 
END
IF @SW = 2
BEGIN
INSERT INTO TMPANNS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSPDARE,TMSPAVERE,TMSPSALDO,TMSPDAREP,TMSPAVEREP,TMSPSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT DISTINCT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,0,0,0,0,0,0,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS
from TMPBILS where TMSBLOCK = @BLOCK
DELETE FROM TMPBILS WHERE TMSBLOCK = @BLOCK 
---DELETE FROM TMPBILC WHERE TMCBLOCK = @BLOCK 
END
GO
/****** Object:  StoredProcedure [dbo].[RF1C]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[RF1C] @DAL as smalldatetime,@AL as smalldatetime,@BLOCK as int,@CAUS as smallint,@SW as smallint,@CAUCH as smallint,@FAL as smalldatetime
 as
--- REM BILANCIO PER COMPETENZA SOSTITUISCO PRKAAMMGG CON PRIDATAEST
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
SUBSTRING(prkconto,1,2),PRICAUSALE
from VH8
where PrkTipoCo = 0 and PriDataEst between @DAL and @AL and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl,PRICAUSALE
UNION
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
SUBSTRING(prkconto,1,2),PRICAUSALE
from RH8
where PrkTipoCo = 0 and PriDataEst = @FAL and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl,PRICAUSALE
----
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end, 
PRICAUSALE,0
from VH8 inner join TbGrp on GrpCod = 'CL' 
where PrkTipoCo = 1  and PriDataEst between @DAL and @AL and PrkConto < GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,PRICAUSALE
UNION
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end, 
PRICAUSALE,0
from RH8 inner join TbGrp on GrpCod = 'CL' 
where PrkTipoCo = 1  and PriDataEst =@FAL and PrkConto < GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,PRICAUSALE
---
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end ,
PRICAUSALE,1
from VH8 inner join TbGrp on GrpCod = 'FO' 
where PrkTipoCo = 1  and PriDataEst between @DAL and @AL and PrkConto > GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,pricausale 
UNION
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end ,
PRICAUSALE,1
from RH8 inner join TbGrp on GrpCod = 'FO' 
where PrkTipoCo = 1  and PriDataEst =@FAL and PrkConto > GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,pricausale 
---
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select distinct TMCBLOCK,TMCCPT,(SELECT PIAANACO FROM TBPIA WHERE PIACODCO = TMCCPT),(SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = TMCCPT),
SUM(TMCDARE),SUM(TMCAVERE),SUM(TMCSALDO),SUM(TMCDAREP),SUM(TMCAVEREP),SUM(TMCSALDOP),SUBSTRING(TMCCPT,1,2),0
from TMPBILC where TMCBLOCK = @BLOCK
GROUP BY TMCCPT,TMCBLOCK
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select @BLOCK,PIACODCO ,PIAANACO,0,0,0,0,0,0,0,SUBSTRING(PIACODCO,1,2),0 
FROM TBPIA WHERE SUBSTRING(PIACODCO,4,2) = '00' 
DECLARE @APERTURA AS DECIMAL(13,2),@CONTO AS VARCHAR(5)
SET @APERTURA = 0.0
SET @CONTO = (SELECT ESEBILAP FROM TBESE WHERE ESEANNO = DATEPART(YEAR,@DAL))
SET @APERTURA =(SELECT ISNULL(SUM(TMSSALDOP),0) FROM TMPBILS WHERE NOT EXISTS (SELECT * FROM tmpbils where TMSCONTO = @CONTO AND TMSCAUS = 45 and tmsblock = @block))
IF @APERTURA <> 0 
BEGIN
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select @BLOCK,@CONTO ,(SELECT PIAANACO FROM TBPIA WHERE PIACODCO = @CONTO),(SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = @CONTO),0,0,0,0,0,(@APERTURA * -1),SUBSTRING(@CONTO,1,2),45 
DELETE FROM TMPBILS WHERE TMSCONTO = @CONTO AND TMSCAUS <> 45 and tmsblock = @block
END
UPDATE TMPBILS SET TMSDEMAS = (select ISNULL(PIAANACO,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSMASTRO+'.00' ) WHERE TMSBLOCK = @BLOCK
IF @SW = 1
BEGIN
INSERT INTO TMPANNS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSPDARE,TMSPAVERE,TMSPSALDO,TMSPDAREP,TMSPAVEREP,TMSPSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT DISTINCT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,0,0,0,0,0,0,TMSMASTRO,TMSCAUS,TMSDEMAS
from TMPBILS where TMSBLOCK = @BLOCK
DELETE FROM TMPBILS WHERE TMSBLOCK = @BLOCK 
---DELETE FROM TMPBILC WHERE TMCBLOCK = @BLOCK 
END
IF @SW = 2
BEGIN
INSERT INTO TMPANNS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSPDARE,TMSPAVERE,TMSPSALDO,TMSPDAREP,TMSPAVEREP,TMSPSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT DISTINCT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,0,0,0,0,0,0,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS
from TMPBILS where TMSBLOCK = @BLOCK
DELETE FROM TMPBILS WHERE TMSBLOCK = @BLOCK 
---DELETE FROM TMPBILC WHERE TMCBLOCK = @BLOCK 
END
GO
/****** Object:  StoredProcedure [dbo].[RiAprePartita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[RiAprePartita] @Id AS int ,@az as int,@MIGLIO as int
as
if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TAPCH'))
   begin
   drop table ##TAPCH
  end   
CREATE TABLE [##TAPCH] (
	[ApcConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ApcNdoc] [int] NOT NULL ,
	[ApcAnno] [smallint],
        [ApcImpo] [decimal](13,2) NOT NULL 
) ON [PRIMARY]
DECLARE @CONTO as VARCHAR(5),@ANNO as SMALLInt,@NDOC as int,@SCOPERTO AS DECIMAL(13,2)
DECLARE SERGENTE CURSOR FOR
SELECT distinct PrkConto,PrkDocAnn,PrkDocEst from TbPrk where PrkId =  @id  AND PRKDOCEST > 0 AND CASE WHEN PRKTIPOCO = 0 THEN (SELECT PIAFL09 FROM TBPIA WHERE PIACODCO = PRKCONTO) ELSE 1 END = 1 
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @CONTO,@ANNO,@NDOC
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO [##TAPCH] ([ApcConto] ,[ApcAnno],[ApcNdoc], [ApcImpo]) 
SELECT distinct PRKCONTO,PRKDOCANN,PRKDOCEST,
(SELECT isnull(SUM(DARE)-SUM(AVERE),0) FROM VB8  WHERE PRKCONTO = @CONTO and PRKDOCEST = @NDOC and PRKDOCANN = @ANNO)
FROM TBPRK
where PRKCONTO = @CONTO AND PRKDOCANN = @ANNO AND PRKDOCEST = @NDOC
group by PRKCONTO,PRKDOCANN,PRKDOCEST
order by PRKCONTO,PRKDOCANN,PRKDOCEST
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @CONTO,@ANNO,@NDOC
END
CLOSE SERGENTE
DEALLOCATE SERGENTE
UPDATE TBPRK SET PRKpAPERTA = 0 FROM TBPRK T, ##TAPCH S WHERE T.PRKCONTO =S.ApcConto AND T.PRKDOCANN = S.ApcAnno AND T.PRKDOCEST = S.ApcNdoc
UPDATE TBSCA SET SCArAPERTA = 0,SCAIMPPAGATO = 0 FROM TBSCA Q, ##TAPCH S WHERE Q.SCACONTO =S.ApcConto AND DATEPART(YEAR,Q.SCADDOC) = S.ApcAnno AND Q.SCANDOC = S.ApcNdoc
--EXEC Riscadenza @Id =@id, @az=@az, @miglio=@miglio
GO
/****** Object:  StoredProcedure [dbo].[RiChiudePartita]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Batch submitted through debugger: SQLQuery1.sql|7|0|C:\Users\ALDO\AppData\Local\Temp\~vsA01D.sql
CREATE procedure [dbo].[RiChiudePartita] @Id AS int ,@miglio as int
as
CREATE TABLE [dbo].[#TAPCH] (
	[ApcConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ApcNdoc] [int] NOT NULL ,
	[ApcAnno] [smallint] 
) ON [PRIMARY]
DECLARE @CONTO as VARCHAR(5),@ANNO as SMALLInt,@NDOC as int
DECLARE SERGENTE CURSOR FOR
SELECT distinct PrkConto,PrkDocAnn,PrkDocEst from TbPrk where PrkId =  @id
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @CONTO,@ANNO,@NDOC
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO [dbo].[#TAPCH] ([ApcConto] ,[ApcAnno],[ApcNdoc]) 
SELECT distinct PRKCONTO,PRKDOCANN,PRKDOCEST
FROM vb8
where partitario = 1 and PRKCONTO = @CONTO AND PRKDOCANN = @ANNO AND PRKDOCEST = @NDOC AND PRKDOCEST > 0
group by PRKCONTO,PRKDOCANN,PRKDOCEST
HAVING (sum(dare)-sum(avere)) = 0
order by PRKCONTO,PRKDOCANN,PRKDOCEST
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @CONTO,@ANNO,@NDOC
END
CLOSE SERGENTE
DEALLOCATE SERGENTE
UPDATE TBPRK SET PRKpAPERTA = 1 FROM TBPRK T, #TAPCH S WHERE T.PRKCONTO =S.ApcConto AND T.PRKDOCANN = S.ApcAnno AND T.PRKDOCEST = S.ApcNdoc  
--UPDATE TBSCA SET SCArAPERTA = 1,SCAIMPPAGATO = SCAIMPRATA FROM TBSCA Q, #TAPCH S WHERE Q.SCACONTO =S.ApcConto AND DATEPART(YEAR,Q.SCADDOC) = S.ApcAnno AND Q.SCANDOC = S.ApcNdoc
--SELECT * FROM  #TAPCH
drop table #TAPCH
EXEC MultiPartita @id = @id, @miglio =@miglio

GO
/****** Object:  StoredProcedure [dbo].[RinitPrk]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[RinitPrk] @Id as int
as
delete from TRPrk where PrkId = @id
insert into TRPrk(PrkId,PrkProg,PrkDa,PrkTipoCo,PrkConto,PrkAammgg,PrkDocAnn,PrkDocEst,PrkPAperta)
select priid,PriProg,'0',case when substring(priCoDare,3,1) = '.' then '0' else '1' end,PriCoDare,
case when PRICAUSALE < 4 then PriDataGio else PridataEst end,
PriDocAnn,PriDocEst,0
from TRPri where PriiD = @id AND PriCodare <> '00.10' AND PRICAUSALE <> 1 --- prima i dare
insert into TRPrk(PrkId,PrkProg,PrkDa,PrkTipoCo,PrkConto,PrkAammgg,PrkDocAnn,PrkDocEst,PrkPAperta)
select priid,PriProg,'1',case when substring(priCoAvere,3,1) = '.' then '0' else '1' end,PriCoAvere,
case when PRICAUSALE < 4 then PriDataGio else PridataEst end,
PriDocAnn,PriDocEst,0
from TRPri where PriiD = @id AND PriCoAvere <> '00.10' AND PRICAUSALE <> 2 ---- poi Aver


GO
/****** Object:  StoredProcedure [dbo].[RiScadenza]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE procedure [dbo].[RiScadenza] @Id AS int,@Az AS int,@MIGLIO AS INT
as
CREATE TABLE [dbo].[#TAPCH] (
	[ApcConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ApcNdoc] [int] NOT NULL ,
	[ApcAnno] [smallint],
        [ApcImpo] [decimal](13,2) NOT NULL 
) ON [PRIMARY]
DECLARE @CONTO as VARCHAR(5),@ANNO as SMALLInt,@NDOC as int,@TOTINSOLUTI AS DECIMAL(12,2),@CAUINS as smallint
SET @CAUINS=(SELECT PosCauInsoluti from TbPos where PosId=0)

DECLARE SERGENTE CURSOR FOR
SELECT distinct PrkConto,PrkDocAnn,PrkDocEst from TbPrk where PrkId =  @id AND prktipoco = 1 AND PRKDOCEST > 0
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @CONTO,@ANNO,@NDOC
WHILE @@FETCH_STATUS = 0
BEGIN
INSERT INTO [dbo].[#TAPCH] ([ApcConto] ,[ApcAnno],[ApcNdoc], [ApcImpo]) 
SELECT distinct PRKCONTO,PRKDOCANN,PRKDOCEST,
(SELECT SALDO = CASE WHEN @CONTO < @MIGLIO THEN isnull(SUM(DARE)-SUM(AVERE),0) ELSE  isnull(SUM(AVERE)-SUM(DARE),0) END FROM VB8  WHERE PRKCONTO = @CONTO and PRKDOCEST = @NDOC and PRKDOCANN = @ANNO and priid <> @AZ and pricausale <> @CAUINS)
FROM TBPRK inner join tbpri on priid=prkid
where PRKCONTO = @CONTO AND PRKDOCANN = @ANNO AND PRKDOCEST = @NDOC AND prkid <> @az
group by PRKCONTO,PRKDOCANN,PRKDOCEST
order by PRKCONTO,PRKDOCANN,PRKDOCEST
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @CONTO,@ANNO,@NDOC
END
CLOSE SERGENTE
DEALLOCATE SERGENTE

--select * from #tapch

IF @AZ > 0 
BEGIN
UPDATE TBSCA SET SCArAPERTA = 0,SCAIMPPAGATO = 0 FROM TBSCA Q, #TAPCH S WHERE Q.SCACONTO =S.ApcConto AND DATEPART(YEAR,Q.SCADDOC) = S.ApcAnno AND Q.SCANDOC = S.ApcNdoc
END 


DECLARE @RESIDUO AS DECIMAL(13,2),@SCAID AS INT,@NR AS SMALLINT,@RL AS SMALLINT,@DIFF AS DECIMAL(13,2),@SOMMA AS DECIMAL(13,2),@RATA AS DECIMAL(13,2),@TOTALE AS DECIMAL(13,2),@AP AS VARCHAR(1),@RESTO AS DECIMAL(13,2)
DECLARE @XSOMMA AS DECIMAL(13,2),@XRESTO AS DECIMAL(13,2),@XRESIDUO AS DECIMAL(13,2),@XPAGATO as decimal(13,2),@SEGNO AS SMALLINT
DECLARE PARTITA CURSOR FOR
SELECT distinct ApcConto ,ApcAnno,ApcNdoc, ApcImpo from #TAPCH 
OPEN PARTITA
FETCH NEXT FROM PARTITA
INTO @CONTO,@ANNO,@NDOC,@RESIDUO
WHILE @@FETCH_STATUS = 0
BEGIN
SET @NR =(SELECT ISNULL(MAX(SCANRATA),0)from TBSCA 
              WHERE SCACONTO =@CONTO AND DATEPART(YEAR,SCADDOC) = @ANNO AND SCANDOC = @NDOC ) 
SET @SOMMA = 0
SET @RESTO = 0
SET @SEGNO= 1
IF @RESIDUO < 0
BEGIN 
IF (@CONTO > @MIGLIO  and @RESIDUO > 0)-- OR (@CONTO < @MIGLIO  and @RESIDUO < 0)
BEGIN
SET @SEGNO = -1
END
SET @RESTO =ABS(@RESIDUO)
SET @RESIDUO = 0
END 
       DECLARE RATE CURSOR FOR
       SELECT SCAID,SCAIMPRATA,SCAIMPDOC,SCArAPERTA,SCANRATA from TBSCA 
              WHERE SCACONTO =@CONTO AND DATEPART(YEAR,SCADDOC) = @ANNO AND SCANDOC = @NDOC 
              ORDER BY SCANRATA 
       OPEN RATE
       FETCH NEXT FROM RATE
       INTO @SCAID,@RATA,@TOTALE,@AP,@RL
       WHILE @@FETCH_STATUS = 0
       BEGIN
	   print @TOTALE;
	   PRINT @RESIDUO;
	   PRINT @SOMMA;
       SET @DIFF = @TOTALE - (@RESIDUO + @SOMMA)
	   PRINT @DIFF;
	   PRINT @RATA;
       IF @DIFF >= @RATA 
        BEGIN       
        SET @DIFF = @RATA
        SET @AP = 1
        GOTO ALTRA_RATA
        END
       IF @DIFF < @RATA AND @DIFF > 0 
        BEGIN
        SET @AP = 0
        GOTO ALTRA_RATA
        END
        GOTO FINE_RATE 
ALTRA_RATA:
      IF @NR =  @RL --and @CONTO > @MIGLIO
      BEGIN
	  PRINT @DIFF;
	  PRINT @RESTO;
	  PRINT @SEGNO;
      SET @DIFF = @DIFF + (@RESTO  * @SEGNO)
       IF @DIFF = 0
        BEGIN
        SET @AP = 0
        END
      END  
	  PRINT @NR;
	  print @DIFF;
	  UPDATE TBSCA SET SCARAPERTA =@AP,SCAIMPPAGATO = case when ABS(@DIFF) > abs(SCAIMPPAGATO) and SCAIMPPAGATO <> 0 THEN ScaImpRata  ELSE @DIFF END WHERE SCAID = @SCAID
	   UPDATE TBSCA SET SCARAPERTA = case when SCAIMPPAGATO=SCAIMPRATA THEN 1 ELSE 0 END WHERE SCAID = @SCAID
      SET @SOMMA = @SOMMA + @DIFF
	  PRINT @SOMMA;
      FETCH NEXT FROM RATE
      INTO @SCAID,@RATA,@TOTALE,@AP,@RL
      END
FINE_RATE:
     CLOSE RATE
     DEALLOCATE RATE
FINE_PARTITA:
   FETCH NEXT FROM PARTITA
   INTO @CONTO,@ANNO,@NDOC,@RESIDUO
END
PRINT @SEGNO;
print @diff;
print @somma;
print @resto;
CLOSE PARTITA
DEALLOCATE PARTITA

--select * from tbsca
IF @CONTO > @MIGLIO GOTO USCITA_FORNITORI
print @nr;


-- 1a parte
if @NR = 1
BEGIN
SET @TOTINSOLUTI=(SELECT ISNULL(sum(PRIIMPDARE),0) from tbsca 
inner join tbpri 
on scaconto=pricodare and scandoc=PriDocEst and PriDocAnn = datepart(year,ScaDdoc)
where scaconto=@CONTO and  scandoc = @NDOC and datepart(year,ScaDdoc) = @ANNO and pricausale=@CAUINS and pridataest >=ScaDsca  )

update tbsca set scaraperta=0,scaimppagato=scaimppagato- @TOTINSOLUTI from tbsca 
where scaconto=@CONTO and  scandoc = @NDOC and datepart(year,ScaDdoc) = @ANNO  

UPDATE TBSCA SET scaimppagato=ScaImpRata where ScaRaperta=0 and abs(ScaImpPagato) > abs(scaimprata)  
AND scaconto=@CONTO and  scandoc = @NDOC and datepart(year,ScaDdoc) = @ANNO 

UPDATE TBSCA SET SCARAPERTA=1 from tbsca  WHERE scaimppagato=ScaImpRata
AND scaconto=@CONTO and  scandoc = @NDOC and datepart(year,ScaDdoc) = @ANNO  



GOTO USCITA_FORNITORI
END


-- 2A PARTE
SELECT PriCodare,PriDocAnn,PriDataEst,PriDocEst,PriImpDare INTO #TMP2 FROM TBPRI WHERE PriCoDare=@CONTO and  PriDocEst = @NDOC and PriDocAnn = @ANNO and pricausale=@CAUINS

SELECT distinct TBSCA.*,Dopo=ScaDsca into #tmp from #TMP2
inner join tbSca 
on scaconto=pricodare and scandoc=PriDocEst and PriDocAnn = datepart(year,ScaDdoc)
where scaconto=@CONTO and  scandoc = @NDOC and datepart(year,ScaDdoc) = @ANNO and pridataest >=ScaDsca 
ORDER BY ScaNRata



DECLARE @P as int,@N AS INT,@D as smalldatetime,@G AS SMALLDATETIME, @DSca as smalldatetime,@R as int

set @N=(select count(*) from #tmp)
DECLARE SERGENTE CURSOR FOR
SELECT distinct ScaId ,ScaDsca,ScaNRata,Dopo from #TMP order by ScaNrata 
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @Scaid,@DSca,@R,@D
WHILE @@FETCH_STATUS = 0
BEGIN
SET  @G= ISNULL((select TOP 1 ScaDsca=DATEADD(DAY,16,@D) from #tmp where @D = ScaDsca ORDER BY ScaDsca),DATEADD(DAY,16,@D))
UPDATE #TMP SET DOPO=@G WHERE SCAID=@SCAID
XFINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO  @Scaid,@DSca,@R,@D
END
CLOSE SERGENTE
DEALLOCATE SERGENTE



SELECT * FROM #TMP2
SELECT * FROM #TMP
--uscita:
--uscita_fornitori:

SELECT DISTINCT * into #TMP3 from #TMP inner join #TMP2 on scaconto=@CONTO and scandoc = @NDOC and datepart(year,ScaDdoc) = @ANNO AND  pridataest>= ScaDsca AND pridataest < Dopo
--where scaconto=@CONTO and  scandoc = @NDOC and datepart(year,ScaDdoc) = @ANNO AND  pridataest>= ScaDsca AND pridataest <= Dopo

select * from #tmp3

update tbsca set scaraperta=0,scaimppagato=(b.ScaImpPagato - priimpdare) from #TMP3 as a,TbSca as b
where  pridataest>= b.ScaDsca AND pridataest < Dopo  and b.scaid=a.scaid

Uscita:
PRINT @RESTO;


IF @RESTO > 0
BEGIN
SET @XSOMMA = 0
SET @XRESIDUO =@RESTO
SET @XRESTO = 0
SET @NR =(SELECT ISNULL(MAX(SCANRATA),0)from TBSCA 
              WHERE SCACONTO =@CONTO AND DATEPART(YEAR,SCADDOC) = @ANNO AND SCANDOC = @NDOC and ScaImpPagato < ScaImpRata) 
			  PRINT @NR;
       DECLARE XRATE CURSOR FOR
       SELECT SCAID,SCAIMPRATA,SCAIMPDOC,SCArAPERTA,SCANRATA,SCAIMPPAGATO from TBSCA 
              WHERE SCACONTO =@CONTO AND DATEPART(YEAR,SCADDOC) = @ANNO AND SCANDOC = @NDOC and ScaImpPagato < ScaImpRata
              ORDER BY SCANRATA 
       OPEN XRATE
       FETCH NEXT FROM XRATE
       INTO @SCAID,@RATA,@TOTALE,@AP,@RL,@XPAGATO
       WHILE @@FETCH_STATUS = 0
       BEGIN
	   IF @XRESIDUO = 0 GOTO XFINE_RATE
	   if @TOTALE=@XRESIDUO
	   BEGIN
	     UPDATE TBSCA SET SCARAPERTA =1,SCAIMPPAGATO = ScaImpRata  WHERE SCAID = @SCAID
		 GOTO XOLTRE_RATA
	   END
	   if @XPAGATO > 0 
	   BEGIN
	   SET @XSOMMA = @XSOMMA + @XPAGATO
	   END
	   SET @DIFF = @TOTALE - (@XRESIDUO + @XSOMMA)
	   If @DIFF=0
		BEGIN
		UPDATE TBSCA SET SCARAPERTA =1,SCAIMPPAGATO = ScaImpRata  WHERE SCAID = @SCAID
		GOTO XOLTRE_RATA
		END
       IF @DIFF >= @RATA 
        BEGIN       
        SET @DIFF = @RATA
        SET @AP = 1
        GOTO XALTRA_RATA
        END
       IF @DIFF < @RATA AND @DIFF > 0 
        BEGIN
        SET @AP = 0
        GOTO XALTRA_RATA
        END
        GOTO XFINE_RATE 
XALTRA_RATA:
      IF @NR =  @RL
      BEGIN
      SET @DIFF = @DIFF + @XRESTO   
       IF @DIFF = 0
        BEGIN
        SET @AP = 0
        END
      END  
     -- UPDATE TBSCA SET SCARAPERTA =@AP,SCAIMPPAGATO = case when @DIFF > SCAIMPPAGATO THEN ScaImpRata  ELSE @DIFF END WHERE SCAID = @SCAID
	    UPDATE TBSCA SET SCARAPERTA =@AP,SCAIMPPAGATO = case when ABS(@DIFF) > ABS(SCAIMPPAGATO) and SCAIMPPAGATO <> 0 THEN ScaImpRata  ELSE @DIFF END WHERE SCAID = @SCAID
	  UPDATE TBSCA SET SCARAPERTA = case when SCAIMPPAGATO=SCAIMPRATA THEN 1 ELSE 0 END WHERE SCAID = @SCAID
      SET @XSOMMA = @XSOMMA + @DIFF
	  SET  @XRESIDUO=@XRESIDUO - @DIFF
PRINT @SCAID
print @diff;
print @XSOMMA;
print @XRESTO;
XOLTRE_RATA:
			        FETCH NEXT FROM XRATE
      INTO @SCAID,@RATA,@TOTALE,@AP,@RL,@XPAGATO
      END
XFINE_RATE:
     CLOSE XRATE
     DEALLOCATE XRATE
	 END

USCITA_FORNITORI:
GO
/****** Object:  StoredProcedure [dbo].[saXSCORRIS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[saXSCORRIS] @DAL as smalldatetime,@al as smalldatetime,@NREG AS SMALLINT
as
if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##tmp'))
   begin
   drop table ##tmp
  end   
CREATE TABLE ##TMP (
       	[CONTO] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[IVA] [decimal](13, 2) NOT NULL,
        [TOTALEIVA] [decimal](13, 2) NOT NULL,
	) ON [PRIMARY]
DECLARE @TOTIVA AS DECIMAL(13,2)
select DISTINCT CORRANNO,CORRREGIVA,CORRMESE,CORRCONTO,SUM(CORRLORDO) as CORRLORDO,IVAPIMPON,IVAPIVADE,CORRCODIVA,
 (SUM(CORRLORDO) * 100 /(IVAPIMPON+IVAPIVADE))as PERC,(ivapivade * (SUM(CORRLORDO) * 100 /(IVAPIMPON+IVAPIVADE))) / 100 as IVA
INTO #TP
from TBCORR inner join TBIVAP 
ON CORRANNO = IVAPANNO AND CORRREGIVA = IVAPREGIVA AND CORRMESE = IVAPMESE AND CORRCODIVA = IVAPCODIVA
WHERE CORRANNO = DATEPART(YEAR,@AL) AND CORRREGIVA = @NREG AND CORRMESE BETWEEN  DATEPART(month,@DAL) AND DATEPART(month,@AL)
GROUP BY CORRANNO,CORRREGIVA,CORRMESE,CORRCONTO,CORRCODIVA,IVAPIMPON,IVAPIVADE
SET @TOTIVA = (SELECT SUM(DISTINCT IVAPIVADE) FROM #TP)
INSERT INTO ##TMP ([CONTO],[IVA],[TOTALEIVA])
SELECT DISTINCT CORRCONTO,CAST(SUM(IVA) AS DECIMAL(12,2)),@TOTIVA FROM #TP GROUP BY CORRCONTO
DROP TABLE #TP






GO
/****** Object:  StoredProcedure [dbo].[SUMCRSP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SUMCRSP] @ANNO AS SMALLINT, @REG AS SMALLINT,@DM as SMALLINT,@AM as SMALLINT , @BLOCK AS INT
AS
DELETE FROM TBCORR WHERE CORRANNO = @ANNO AND CORRREGIVA = @REG  and CORRmese between @DM and @AM
INSERT INTO TBCORR (CorrAnno,CorrRegIva,CorrMese,CorrCodIva,CorrConto,CorrLordo)
SELECT TCorrAnno,TCorrRegIva,TCorrMese,TCorrCodIva,TCorrConto,TCorrLordo 
FROM TMPCORR where TCORRAnno = @anno and TCORRRegIva = @REG AND TCORRid = @BLOCK

GO
/****** Object:  StoredProcedure [dbo].[SUMCRSS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SUMCRSS] @ANNO AS SMALLINT, @REG AS SMALLINT,@DM as SMALLINT,@AM as SMALLINT , @BLOCK AS INT
AS
DELETE FROM TMPCORS WHERE CORRANNO = @ANNO AND CORRREGIVA = @REG  and CORRmese between @DM and @AM
INSERT INTO TMPCORS (CorrAnno,CorrRegIva,CorrMese,CorrCodIva,CorrConto,CorrLordo)
SELECT TCorrAnno,TCorrRegIva,TCorrMese,TCorrCodIva,TCorrConto,TCorrLordo 
FROM TMPCORR where TCORRAnno = @anno and TCORRRegIva = @REG AND TCORRid = @BLOCK
GO
/****** Object:  StoredProcedure [dbo].[SUMIVAP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SUMIVAP] @ANNO AS SMALLINT, @REG AS SMALLINT,@DM as SMALLINT,@AM as SMALLINT , @BLOCK AS INT
AS
DELETE FROM TBIVAP WHERE IVAPANNO = @ANNO AND IVAPREGIVA = @REG  and IvaPmese between @DM and @AM
INSERT INTO TBIVAP (IvaPAnno, IvaPMese, IvaPRegIva,IvaPCodIva,IvaPImpon,IvaPIvaDE,IvaPIvaND,IvaPImpMerce)
SELECT TIvaPAnno, TIvaPMese, TIvaPRegIva,TIvaPCodIva,TImpon,IvaDE, IvaND,Merce 
FROM crriepiva where TIvaPAnno = @anno and TIvaPRegIva = @REG AND TivaPid = @BLOCK
UPDATE TBPRI SET PRIIVAPRINT = 1 WHERE PRIID in (SELECT PREGPRIID from TMPREGIVA where PREGNUMREG = @REG AND PregId = @BLOCK ) 
--UPDATE GEVE.DBO.TbFte_Passiva SET FteFinoAl= (SELECT PRegFinoAl from TMPREGIVA where PREGNUMREG = @REG AND PregId = @BLOCK ) 
UPDATE GEVE.DBO.TbFte_Passiva SET FteFinoAl= (SELECT distinct PRegFinoAl from TMPREGIVA where PREGNUMREG = @REG AND PregId = @BLOCK ) where
FteRifPri in(select distinct PREGPRIID from TMPREGIVA where PREGNUMREG = @REG AND PregId = @BLOCK) 
-- AGGIORNO REGISTRI STAMPA
insert into TbPrintIva(PRINTIVAPriId,PRINTIVAPriRegIva,PRINTIVAFinoAl)
SELECT distinct PREGPRIID,PREGNUMREG,PRegFinoAl from TMPREGIVA where PREGNUMREG = @REG AND PregId = @BLOCK GROUP BY PREGPRIID,PREGNUMREG,PRegFinoAl


GO
/****** Object:  StoredProcedure [dbo].[SUMIVAS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SUMIVAS] @ANNO AS SMALLINT, @REG AS SMALLINT,@DM as SMALLINT,@AM as SMALLINT , @BLOCK AS INT
AS
DELETE FROM TMPIVAS WHERE IVAPANNO = @ANNO AND IVAPREGIVA = @REG  and IvaPmese between @DM and @AM
INSERT INTO TMPIVAS (IvaPAnno, IvaPMese, IvaPRegIva,IvaPCodIva,IvaPImpon,IvaPIvaDE,IvaPIvaND,IvaPImpMerce)
SELECT TIvaPAnno, TIvaPMese, TIvaPRegIva,TIvaPCodIva,TImpon,IvaDE, IvaND,Merce 
FROM crriepiva where TIvaPAnno = @anno and TIvaPRegIva = @REG AND TivaPid = @BLOCK
GO
/****** Object:  StoredProcedure [dbo].[ToGDS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE proc [dbo].[ToGDS] @Dibase as smallint
as


DECLARE  @IDTEXT as smallint, @ARTCOD AS SMALLINT

SELECT  top 1 
ArtCod,
ITALIANO = cast(artcod as varchar(3)) + ' ' + RTRIM(ArtDesc)  + CHAR(13) + CHAR(10) + 'Ingredienti: ' + Dbo.FnSpezza(TDisItaliano) + CHAR(13) + CHAR(10) + 'SPECIALITA'' GASTRONOMICA',
FRANCESE = cast(artcod as varchar(3)) + ' ' + RTRIM(ArtDesc2) + CHAR(13) + CHAR(10) + 'Ingrédients: ' + Dbo.FnSpezza(TDisFrancese) + CHAR(13) + CHAR(10) + upper('Specialite'' gastronomique italien'),
INGLESE  = cast(artcod as varchar(3)) + ' ' + RTRIM(ArtDesc3) + CHAR(13) + CHAR(10) + 'Ingredients: ' + Dbo.FnSpezza(TDisInglese) + CHAR(13) + CHAR(10) + upper('Fine Italy foods'),
TEDESCO  = cast(artcod as varchar(3)) + ' ' + RTRIM(ArtDesc4) + CHAR(13) + CHAR(10) + 'Zutaten: ' + Dbo.FnSpezza(TDisTedesco) + CHAR(13) + CHAR(10) + upper('Italienische gastronomiche Spezialgebiete'),
SPAGNOLO = cast(artcod as varchar(3)) + ' ' + RTRIM(ArtDesc5) + CHAR(13) + CHAR(10) + 'Ingredientes: ' + Dbo.FnSpezza(TDisSpagnolo)
INTO #TMP
from TbArt inner join
     TbTDis on tdisCod = ArtCod
where ArtDibase = @Dibase and ArtCod between 100 and 999


SET @ARTCOD = ( SELECT ArtCod FROM #TMP)

-- ITALIANO

set @idtext = @ARTCOD

if (Select count(*) from TbGds where GdsIdText = @Idtext) = 0
    BEGIN
    insert into TbGds (GdsIdText,GdsTesti,GdsArtCod,GdsLingua) select @idtext, ITALIANO,@artcod,1 from #TMP
    END
else
    BEGIN
    update TbGds set GdsTesti = (select ITALIANO from #TMP) , GdsDataOra = (select getdate()) where GdsIdText = @Idtext
    END

-- FRANCESE

set @idtext = @ARTCOD + 1000
if (Select count(*) from TbGds where GdsIdText = @Idtext) = 0
    BEGIN
    insert into TbGds (GdsIdText,GdsTesti,GdsArtCod,GdsLingua) select @idtext, FRANCESE,@artcod,2 from #TMP
    END
else
    BEGIN
    update TbGds set GdsTesti = (select FRANCESE from #TMP) , GdsDataOra = (select getdate()) where GdsIdText = @Idtext
    END

-- INGLESE

set @idtext = @ARTCOD + 2000
if (Select count(*) from TbGds where GdsIdText = @Idtext) = 0
    BEGIN
    insert into TbGds (GdsIdText,GdsTesti,GdsArtCod,GdsLingua) select @idtext, INGLESE,@artcod,3 from #TMP
    END
else
    BEGIN
    update TbGds set GdsTesti = (select INGLESE from #TMP) , GdsDataOra = (select getdate()) where GdsIdText = @Idtext
    END


-- TEDESCO

set @idtext = @ARTCOD + 3000
if (Select count(*) from TbGds where GdsIdText = @Idtext) = 0
    BEGIN
    insert into TbGds (GdsIdText,GdsTesti,GdsArtCod,GdsLingua) select @idtext, TEDESCO,@artcod,4 from #TMP
    END
else
    BEGIN
    update TbGds set GdsTesti = (select TEDESCO from #TMP) , GdsDataOra = (select getdate()) where GdsIdText = @Idtext
    END

-- SPAGNOLO

set @idtext = @ARTCOD + 4000
if (Select count(*) from TbGds where GdsIdText = @Idtext) = 0
    BEGIN
    insert into TbGds (GdsIdText,GdsTesti,GdsArtCod,GdsLingua) select @idtext, SPAGNOLO, @artcod, 5 from #TMP
    END
else
    BEGIN
    update TbGds set GdsTesti = (select SPAGNOLO from #TMP) , GdsDataOra = (select getdate()) where GdsIdText = @Idtext
    END


GO
/****** Object:  StoredProcedure [dbo].[WWVENTILA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE procedure [dbo].[WWVENTILA] @ANNO as SMALLINT
AS

if NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMPVENTILA]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN

CREATE TABLE [dbo].[TMPVENTILA] (
	[IvaVAnno] [smallint] NOT NULL ,
	[IvaVRegIva] [smallint] NOT NULL ,
	[IvaVCodIva] [smallint] NOT NULL ,
	[IvaVAcLordi] [decimal](13, 2) NOT NULL ,
	[IvaVPerComp] [decimal](13, 10) NOT NULL ,
	[IvaVLordi] [decimal](13, 2) NOT NULL ,
	[IvaVCMPIva] [smallint] NOT NULL ,
	[IvaVNetti] [decimal](13, 2) NOT NULL ,
	[IvaVIva] [decimal](13, 2) NOT NULL,
	[IvaDes] [varchar](12) NOT NULL 
) ON [PRIMARY]
END

DELETE FROM TMPVENTILA

DECLARE @TM AS DECIMAL(13,2),@PS AS DECIMAL(13,10),@AA AS DECIMAL(13,2),@LORDOC AS DECIMAL(13,2),@MERCE AS DECIMAL(13,2)
DECLARE @MESE AS SMALLINT,@REGIVA AS SMALLINT,@PE AS SMALLINT,@IM as SMALLINT,@ALIQ as smallint
DECLARE @CORRN as decimal(13,2),@CORRI as decimal(13,2),@DESC AS VARCHAR(12)

SET @MESE = (SELECT Max(IvaVmese) FROM TbIvaV where IvaVAnno = @ANNO AND IvaVmese < 13)

SET @MERCE  =(SELECT SUM(IvaVAcLordi) FROM TbIvaV where IvaVAnno = @ANNO and IvaVMese =@MESE)
SET @LORDOC =(SELECT SUM(IvaVLordi) FROM TbIvaV where IvaVAnno = @ANNO )

DECLARE SERGENTE CURSOR FOR
SELECT distinct IvaVCodIva,Sum(IvaVAcLordi),IvaVRegiva, IvaVCmpIva from TbIvaV where IvaVAnno = @ANNO and IvaVMese =@MESE group by IvaVCmpIva,IvaVRegiva,IvaVCodIva
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @IM,@TM,@REGIVA,@PE
WHILE @@FETCH_STATUS = 0
BEGIN
SET @PS = ( @TM / @MERCE ) * 100
SET @AA = @LORDOC * @PS / 100
SET @ALIQ = (select CiiAli from Tbcii where CiiCod = @PE)
SET @DESC = (select CiiDes from Tbcii where CiiCod = @PE)

set @CORRN = @AA / (100 + @ALIQ) * 100
SET @CORRI = @AA - @CORRN
INSERT INTO [dbo].[TMPVENTILA] (IvaVAnno,IvaVRegIva,IvaVCodIva,IvaVAcLordi,IvaVPerComp,IvaVLordi,
IvaVCMPIva,IvaVNetti,IvaVIva,IvaDes)
SELECT @ANNO,@REGIVA,@IM,@TM,@PS,@AA,@ALIQ,@CORRN,@CORRI,@DESC
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @IM,@TM,@REGIVA,@PE
END
CLOSE SERGENTE
DEALLOCATE SERGENTE
select * from TMPVENTILA
GO
/****** Object:  StoredProcedure [dbo].[X_FATTXML_DTE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[X_FATTXML_DTE] @ANNO AS SMALLINT, @PERIODO AS TINYINT, @NO_CONTROL AS BIT 
AS

DELETE FROM TMPFTERRORE WHERE FeTipo = 'CL'

select PaCodiva 
into #split
from TbPaCii 
where PATIPO='IVA'

SELECT * 
INTO #REGISTRI
from TbRegIva
WHERE RIvaAnno = @ANNO

select CliFor=PRegCliFor,
       Anagrafica=PRegAnaGraf,
       Registro=PRegNumReg,
	   NumDoc=PRegNumDoc,
	   DataDoc=PRegDataE,
	   Codiva=PregCodiva,
	   ImponibileImporto=SUM(PRegImpon), 
       Imposta=sum(PRegImpIva), 
	   TipoReg = (Select RivaTipo from #registri where RIvaNreg = PRegNumReg),
	   Dataregistrazione = PRegDataG,
	   NumProt = PRegNumProt,
	   TOTALE = (SELECT SUM(PRegImpon) + sum(PRegImpIva) FROM TMPXMLREGIVA b where b.PRegPriId = a.PRegPriId group by  b.PRegPriId) 
into #TMP
from TMPXMLREGIVA a INNER JOIN
     TbErrP on	PREGANNO = PErranno and PRegPeriodo = PErrPeriodo and PRegCliFor = PErrClifor
where pregtipo = 'CL' AND PRegAnno =@ANNO and PRegPeriodo = @PERIODO and PErrEscludi = 0
GROUP BY PRegCliFor,PRegAnaGraf,PRegNumReg,PRegNumDoc,PRegDataE,PregCodiva,PRegDataG,PRegNumProt,PRegPriId
ORDER BY PRegNumReg,PRegNumDoc,PRegDataE

----------------CONTROLLO ERRORI NATURA
insert into TMPFTERRORE (FeTipo, FeRegistro, FeNumProt, FeCliFor, FeNumdoc, FeDataDoc, FeImponibileImporto, FeImposta, FeCodiva, FeNatura, FeErrore)

SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'MANCA NATURA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (CiiAli = 0 AND ImponibileImporto <> 0 AND CiiNatura = '') 
union
SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA DA TOGLIERE !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE ((CiiNatura <>'' and Imposta <> 0) and CiiNatura <> 'N6')

union
SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA ERRATA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (Imposta = 0 AND ImponibileImporto <> 0 and CiiNatura = 'N6')

union
SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA PROVINCIA !!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and rtrim(Anaprov) = '' AND @NO_CONTROL = 0

union

SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA C.A.P. !!, C.A.P. ERRATO !!!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and ( rtrim(AnacAP) = '00000' OR  LEN(rtrim(AnacAP)) < 5 OR ISNUMERIC(rtrim(AnacAP)) = 0) AND @NO_CONTROL = 0

---------------------------------------------------------------------------
select  CliFor,
        Registro,
        NumProt,
		Codiva,
        IdPaese = CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END ,
        idCodice =CASE WHEN rtrim(AnaPivaEst) > '' THEN substring(AnaPivaEst,3,LEN(ANAPIVAEST) - 2) ELSE (CASE WHEN LTRIM(AnaPiva) > '' THEN rtrim(AnaPiva) ELSE '' END) END ,
		CodiceFiscale = case when AnaCFis <> AnaPiva THEN AnaCFis else '' end,
		Denominazione =case when  isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),'') = '' THEN Anagrafica ELSE '' END,
		Nome = isnull((SELECT PFNome from tbIntPf where PfCliFor = CliFor),'') ,
		Cognome = isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),''),
		Indirizzo = CASE WHEN rtrim(AnaIndirizzo) = '' THEN 'DATI MANCANTI' ELSE  AnaIndirizzo END,
		Comune= CASE WHEN rtrim(AnaCitta) = '' THEN 'DATI MANCANTI' ELSE  AnaCitta END,
		CAP = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE rtrim(AnaCap) END,
		Provincia = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE rtrim(AnaProv) END,
		Nazione= CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END,
		--TipoDocumento = CASE WHEN TipoReg = 3 then 'TD04' else 'TD01' END ,
		TipoDocumento = CASE WHEN TOTALE < 0 then 'TD04' else 'TD01' END ,
		Data= ISNULL(convert(varchar(10),DataDoc,126),''),
		Numero=CAST(NumDoc as varchar) + '/' + cast(Registro as varchar),
		--ImponibileImporto = CASE WHEN TIPOREG = 3 THEN ImponibileImporto * -1 ELSE ImponibileImporto END ,
		ImponibileImporto = CASE WHEN TOTALE < 0 then ImponibileImporto * -1 else ImponibileImporto END ,
		--Imposta = CASE WHEN TIPOREG = 3 THEN Imposta * -1 ELSE Imposta END ,
		Imposta = CASE WHEN TOTALE < 0 then Imposta * -1 else Imposta END ,
		Aliquota=cast(CiiAli as decimal(5,2)),
		Natura = CiiNatura,
		Detraibile = cast(100 - CiiInd as decimal(5,2)),
		Deducibile = cast('' as varchar(1)),
		EsigibilitaIva =CASE WHEN cast(CiiAli as decimal(5,2)) <> 0 THEN (CASE WHEN Codiva in (select distinct PaCodiva FROM #SPLIT) THEN 'S' ELSE 'I' END) ELSE '' END


FROM #TMP A INNER JOIN
     VDOX.DBO.TbAna B ON AnaCod = CliFor AND  anaGrp = 'CL' inner join
	 TbCii on CiiCod = Codiva
ORDER BY Clifor,Registro,NumDoc,Data




GO
/****** Object:  StoredProcedure [dbo].[X_FATTXML_DTE_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROC [dbo].[X_FATTXML_DTE_EST] @ANNO AS SMALLINT, @PERIODO AS TINYINT, @NO_CONTROL AS BIT 
AS

DELETE FROM TMPFTERRORE WHERE FeTipo = 'CL'

select PaCodiva 
into #split
from TbPaCii 
where PATIPO='IVA'

SELECT * 
INTO #REGISTRI
from TbRegIva
WHERE RIvaAnno = @ANNO

select CliFor=PRegCliFor,
       Anagrafica=PRegAnaGraf,
       Registro=PRegNumReg,
	   NumDoc=PRegNumDoc,
	   DataDoc=PRegDataE,
	   Codiva=PregCodiva,
	   ImponibileImporto=SUM(PRegImpon), 
       Imposta=sum(PRegImpIva), 
	   TipoReg = (Select RivaTipo from #registri where RIvaNreg = PRegNumReg),
	   Dataregistrazione = PRegDataG,
	   NumProt = PRegNumProt,
	   TOTALE = (SELECT SUM(PRegImpon) + sum(PRegImpIva) FROM TMPXMLREGIVA_EST b where b.PRegPriId = a.PRegPriId group by  b.PRegPriId) 
into #TMP
from TMPXMLREGIVA_EST a INNER JOIN
     TbErrP on	PREGANNO = PErranno and PRegPeriodo = PErrPeriodo and PRegCliFor = PErrClifor
where pregtipo = 'CL' AND PRegAnno =@ANNO and PRegPeriodo = @PERIODO and PRegOK = 1 AND PErrEscludi = 0  
GROUP BY PRegCliFor,PRegAnaGraf,PRegNumReg,PRegNumDoc,PRegDataE,PregCodiva,PRegDataG,PRegNumProt,PRegPriId
ORDER BY PRegNumReg,PRegNumDoc,PRegDataE

----------------CONTROLLO ERRORI NATURA
insert into TMPFTERRORE (FeTipo, FeRegistro, FeNumProt, FeCliFor, FeNumdoc, FeDataDoc, FeImponibileImporto, FeImposta, FeCodiva, FeNatura, FeErrore)

SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'MANCA NATURA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (CiiAli = 0 AND ImponibileImporto <> 0 AND CiiNatura = '') 
union
SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA DA TOGLIERE !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE ((CiiNatura <>'' and Imposta <> 0) and SUBSTRING(CiiNatura,1,2) <> 'N6')

union
SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA ERRATA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (Imposta = 0 AND ImponibileImporto <> 0 and (CiiNatura = 'N2' Or CiiNatura='N3'))

union
SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA PROVINCIA !!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and rtrim(Anaprov) = '' AND @NO_CONTROL = 0

union

SELECT 'CL',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA C.A.P. !!, C.A.P. ERRATO !!!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and ( rtrim(AnacAP) = '00000' OR  LEN(rtrim(AnacAP)) < 5 OR ISNUMERIC(rtrim(AnacAP)) = 0) AND @NO_CONTROL = 0

---------------------------------------------------------------------------
select  CliFor,
        Registro,
        NumProt,
		Codiva,
        IdPaese = CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END ,
        idCodice =CASE WHEN rtrim(AnaPivaEst) > '' THEN substring(AnaPivaEst,3,LEN(ANAPIVAEST) - 2) ELSE (CASE WHEN LTRIM(AnaPiva) > '' THEN rtrim(AnaPiva) ELSE '' END) END ,
		CodiceFiscale = case when AnaCFis <> AnaPiva THEN AnaCFis else '' end,
		Denominazione =case when  isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),'') = '' THEN Anagrafica ELSE '' END,
		Nome = isnull((SELECT PFNome from tbIntPf where PfCliFor = CliFor),'') ,
		Cognome = isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),''),
		Indirizzo = CASE WHEN rtrim(AnaIndirizzo) = '' THEN 'DATI MANCANTI' ELSE  AnaIndirizzo END,
		Comune= CASE WHEN rtrim(AnaCitta) = '' THEN 'DATI MANCANTI' ELSE  AnaCitta END,
		CAP = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE rtrim(AnaCap) END,
		Provincia = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE rtrim(AnaProv) END,
		Nazione= CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END,
		--TipoDocumento = CASE WHEN TipoReg = 3 then 'TD04' else 'TD01' END ,
		TipoDocumento = CASE WHEN TOTALE < 0 then 'TD04' else 'TD01' END ,
		Data= ISNULL(convert(varchar(10),DataDoc,126),''),
		Numero=CAST(NumDoc as varchar) + '/' + cast(Registro as varchar),
		--ImponibileImporto = CASE WHEN TIPOREG = 3 THEN ImponibileImporto * -1 ELSE ImponibileImporto END ,
		ImponibileImporto = CASE WHEN TOTALE < 0 then ImponibileImporto * -1 else ImponibileImporto END ,
		--Imposta = CASE WHEN TIPOREG = 3 THEN Imposta * -1 ELSE Imposta END ,
		Imposta = CASE WHEN TOTALE < 0 then Imposta * -1 else Imposta END ,
		Aliquota=cast(CiiAli as decimal(5,2)),
		Natura = CiiNatura,
		Detraibile = cast(100 - CiiInd as decimal(5,2)),
		Deducibile = cast('' as varchar(1)),
		EsigibilitaIva =CASE WHEN cast(CiiAli as decimal(5,2)) <> 0 THEN (CASE WHEN Codiva in (select distinct PaCodiva FROM #SPLIT) THEN 'S' ELSE 'I' END) ELSE '' END


FROM #TMP A INNER JOIN
     VDOX.DBO.TbAna B ON AnaCod = CliFor AND  anaGrp = 'CL' inner join
	 TbCii on CiiCod = Codiva
WHERE TOTALE <> 0
ORDER BY Clifor,Registro,NumDoc,Data



GO
/****** Object:  StoredProcedure [dbo].[X_FATTXML_DTR]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[X_FATTXML_DTR] @ANNO AS SMALLINT, @PERIODO AS TINYINT , @NO_CONTROL AS BIT 
AS

DELETE FROM TMPFTERRORE WHERE FeTipo = 'FO'

select PaCodiva 
into #split
from TbPaCii 
where PATIPO='IVA'

SELECT * 
INTO #REGISTRI
from TbRegIva
WHERE RIvaAnno = @ANNO

select CliFor=PRegCliFor,
       Anagrafica=PRegAnaGraf,
       Registro=PRegNumReg,
	   NumDoc=PRegNumDoc,
	   DataDoc=PRegDataE,
	   Codiva=PregCodiva,
	   ImponibileImporto=SUM(PRegImpon), 
       Imposta=sum(PRegImpIva), 
	   TipoReg = (Select RivaTipo from #registri where RIvaNreg = PRegNumReg),
	   Dataregistrazione = PRegDataG,
	   NumProt = PRegNumProt,
	   PrId = PRegPriId,
	   TOTALE = (SELECT SUM(PRegImpon) + sum(PRegImpIva) FROM TMPXMLREGIVA b where b.PRegPriId = a.PRegPriId group by  b.PRegPriId),
	   Merce=MAX(PRegMerce)
into #TMP
from TMPXMLREGIVA a INNER JOIN
     TbErrP on	PREGANNO = PErranno and PRegPeriodo = PErrPeriodo and PRegCliFor = PErrClifor
where pregtipo = 'FO' AND PRegAnno =@ANNO and PRegPeriodo = @PERIODO and PErrEscludi = 0
GROUP BY PRegCliFor,PRegAnaGraf,PRegNumReg,PRegNumDoc,PRegDataE,PregCodiva,PRegDataG,PRegNumProt,PRegPriId
ORDER BY PRegNumReg,PRegNumDoc,PRegDataE


----------------CONTROLLO ERRORI NATURA
insert into TMPFTERRORE (FeTipo, FeRegistro, FeNumProt, FeCliFor, FeNumdoc, FeDataDoc, FeImponibileImporto, FeImposta, FeCodiva, FeNatura, FeErrore)

SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'MANCA NATURA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (CiiAli = 0 AND ImponibileImporto <> 0 AND CiiNatura = '')
union
SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA DA TOGLIERE !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE ((CiiNatura <>'' and Imposta <> 0) and CiiNatura <> 'N6')

union
SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA ERRATA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (Imposta = 0 AND ImponibileImporto <> 0 and CiiNatura = 'N6')

union
SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA PROVINCIA !!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and rtrim(Anaprov) = '' AND @NO_CONTROL = 0

union

SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA C.A.P. !!, C.A.P. ERRATO !!!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and ( rtrim(AnacAP) = '00000' OR  LEN(rtrim(AnacAP)) < 5 OR ISNUMERIC(rtrim(AnacAP)) = 0) AND @NO_CONTROL = 0

union

SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA PARTITA IVA !!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and rtrim(AnaPiva) = ''
---------------------------------------------------------------------------

select  PrId,
        CliFor,
        Registro,
        NumProt,
		Codiva,
        IdPaese = CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END ,
        idCodice =CASE WHEN rtrim(AnaPivaEst) > '' THEN substring(AnaPivaEst,3,LEN(ANAPIVAEST) - 2) ELSE (CASE WHEN LTRIM(AnaPiva) > '' THEN rtrim(AnaPiva) ELSE '' END) END ,
		CodiceFiscale = case when AnaCFis <> AnaPiva THEN AnaCFis else '' end,
		Denominazione =case when  isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),'') = '' THEN Anagrafica ELSE '' END,
		Nome = isnull((SELECT PFNome from tbIntPf where PfCliFor = CliFor),'') ,
		Cognome = isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),''),
		Indirizzo = CASE WHEN rtrim(AnaIndirizzo) = '' THEN 'DATI MANCANTI' ELSE  AnaIndirizzo END,
		Comune= CASE WHEN rtrim(AnaCitta) = '' THEN 'DATI MANCANTI' ELSE  AnaCitta END,
		CAP = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE rtrim(AnaCap) END,
		Provincia = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE UPPER(rtrim(AnaProv)) END, 
		Nazione= CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END,
		TipoDocumento =CASE WHEN (rtrim(AnaPivaEst) > '' and substring(rtrim(AnaPivaEst),1,2) in (select distinct PaSigla from TbPaesi where PaIntra = 1)) THEN (CASE WHEN MERCE = 1 THEN 'TD10' else 'TD11' END) ELSE ( CASE WHEN TOTALE < 0 then 'TD04' else 'TD01' END ) END,
		Data= ISNULL(convert(varchar(10),DataDoc,126),''),
		DataRegistrazione= ISNULL(convert(varchar(10),Dataregistrazione,126),''),
		Numero=CAST(NumDoc as varchar),
		ImponibileImporto = CASE WHEN TOTALE < 0 then ImponibileImporto * -1 else ImponibileImporto END ,
		Imposta  = CASE WHEN TOTALE < 0 then Imposta * -1 else Imposta END ,
		TOTALE,
		Aliquota=cast(CiiAli as decimal(5,2)),
		Natura = CiiNatura,
		Detraibile = cast(100 - CiiInd as decimal(5,2)),
		Deducibile = cast('' as varchar(1)),
		EsigibilitaIva =CASE WHEN cast(CiiAli as decimal(5,2)) <> 0 THEN (CASE WHEN Codiva in (select distinct PaCodiva FROM #SPLIT) THEN 'S' ELSE 'I' END) ELSE '' END


FROM #TMP A INNER JOIN
     VDOX.DBO.TbAna B ON AnaCod = CliFor AND AnaGrp = 'FO' inner join
	 TbCii on CiiCod = Codiva
--WHERE aNAcfIS <> 'R' or rtrim(AnaPivaEst) <> ''
--WHERE LEN(AnaCFis) > 10 or Len(anaPiva) = 11 or rtrim(AnaPivaEst) <> ''
ORDER BY Clifor,data,NumDoc 

GO
/****** Object:  StoredProcedure [dbo].[X_FATTXML_DTR_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROC [dbo].[X_FATTXML_DTR_EST] @ANNO AS SMALLINT, @PERIODO AS TINYINT , @NO_CONTROL AS BIT 
AS

DELETE FROM TMPFTERRORE WHERE FeTipo = 'FO'

select PaCodiva 
into #split
from TbPaCii 
where PATIPO='IVA'

SELECT * 
INTO #REGISTRI
from TbRegIva
WHERE RIvaAnno = @ANNO

select CliFor=PRegCliFor,
       Anagrafica=PRegAnaGraf,
       Registro=PRegNumReg,
	   NumDoc=PRegNumDoc,
	   DataDoc=PRegDataE,
	   Codiva=PregCodiva,
	   ImponibileImporto=SUM(PRegImpon), 
       Imposta=sum(PRegImpIva), 
	   TipoReg = (Select RivaTipo from #registri where RIvaNreg = PRegNumReg),
	   Dataregistrazione = PRegDataG,
	   NumProt = PRegNumProt,
	   PrId = PRegPriId,
	   TOTALE = (SELECT SUM(PRegImpon) + sum(PRegImpIva) FROM TMPXMLREGIVA_EST b where b.PRegPriId = a.PRegPriId group by  b.PRegPriId),
	   Merce=MAX(PRegMerce)
into #TMP
from TMPXMLREGIVA_EST a INNER JOIN
     TbErrP on	PREGANNO = PErranno and PRegPeriodo = PErrPeriodo and PRegCliFor = PErrClifor
where pregtipo = 'FO' AND PRegAnno =@ANNO and PRegPeriodo = @PERIODO and PRegOK = 1  and PErrEscludi = 0
GROUP BY PRegCliFor,PRegAnaGraf,PRegNumReg,PRegNumDoc,PRegDataE,PregCodiva,PRegDataG,PRegNumProt,PRegPriId
ORDER BY PRegNumReg,PRegNumDoc,PRegDataE


----------------CONTROLLO ERRORI NATURA
insert into TMPFTERRORE (FeTipo, FeRegistro, FeNumProt, FeCliFor, FeNumdoc, FeDataDoc, FeImponibileImporto, FeImposta, FeCodiva, FeNatura, FeErrore)

SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'MANCA NATURA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (CiiAli = 0 AND ImponibileImporto <> 0 AND CiiNatura = '')
union
SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA DA TOGLIERE !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE ((CiiNatura <>'' and Imposta <> 0) and SUBSTRING(CiiNatura,1,2) <> 'N6')

union
SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = CiiNatura,
		'NATURA ERRATA !!'
from #tmp inner join
     TbCii on CiiCod = Codiva
WHERE (Imposta = 0 AND ImponibileImporto <> 0 and (CiiNatura = 'N2' Or CiiNatura='N3'))

union
SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA PROVINCIA !!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and rtrim(Anaprov) = '' AND @NO_CONTROL = 0

union

SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA C.A.P. !!, C.A.P. ERRATO !!!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and ( rtrim(AnacAP) = '00000' OR  LEN(rtrim(AnacAP)) < 5 OR ISNUMERIC(rtrim(AnacAP)) = 0) AND @NO_CONTROL = 0

union

SELECT 'FO',Registro,NumProt,CliFor,NumDoc,DataDoc,ImponibileImporto,Imposta,Codiva,
        Natura = '',
		'MANCA PARTITA IVA !!'
from #tmp inner join
      VDOX.DBO.TbAna on ANACOD = CLIFOR
WHERE RTRIM(AnaPivaEst) = '' and rtrim(AnaPiva) = ''
---------------------------------------------------------------------------

select  PrId,
        CliFor,
        Registro,
        NumProt,
		Codiva,
        IdPaese = CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END ,
        idCodice =CASE WHEN rtrim(AnaPivaEst) > '' THEN substring(AnaPivaEst,3,LEN(ANAPIVAEST) - 2) ELSE (CASE WHEN LTRIM(AnaPiva) > '' THEN rtrim(AnaPiva) ELSE '' END) END ,
		CodiceFiscale = case when AnaCFis <> AnaPiva THEN AnaCFis else '' end,
		Denominazione =case when  isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),'') = '' THEN Anagrafica ELSE '' END,
		Nome = isnull((SELECT PFNome from tbIntPf where PfCliFor = CliFor),'') ,
		Cognome = isnull((SELECT PFCognome from tbIntPf where PfCliFor = CliFor),''),
		Indirizzo = CASE WHEN rtrim(AnaIndirizzo) = '' THEN 'DATI MANCANTI' ELSE  AnaIndirizzo END,
		Comune= CASE WHEN rtrim(AnaCitta) = '' THEN 'DATI MANCANTI' ELSE  AnaCitta END,
		CAP = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE rtrim(AnaCap) END,
		Provincia = CASE WHEN rtrim(AnaPivaEst) > '' THEN '' ELSE UPPER(rtrim(AnaProv)) END, 
		Nazione= CASE WHEN rtrim(AnaPivaEst) > '' THEN SUBSTRING(AnaPivaEst,1,2) ELSE 'IT' END,
		TipoDocumento =CASE WHEN TOTALE < 0 THEN 'TD04' WHEN (rtrim(AnaPivaEst) > '' and substring(rtrim(AnaPivaEst),1,2) in (select distinct PaSigla from TbPaesi where PaIntra = 1)) THEN (CASE WHEN MERCE = 1 THEN 'TD10' else 'TD11' END) ELSE 'TD01' END,
		Data= ISNULL(convert(varchar(10),DataDoc,126),''),
		DataRegistrazione= ISNULL(convert(varchar(10),Dataregistrazione,126),''),
		Numero=CAST(NumDoc as varchar),
		ImponibileImporto = CASE WHEN TOTALE < 0 then ImponibileImporto * -1 else ImponibileImporto END ,
		Imposta  = CASE WHEN TOTALE < 0 then Imposta * -1 else Imposta END ,
		TOTALE,
		Aliquota=cast(CiiAli as decimal(5,2)),
		Natura = CiiNatura,
		Detraibile = cast(100 - CiiInd as decimal(5,2)),
		Deducibile = cast('' as varchar(1)),
		EsigibilitaIva =CASE WHEN cast(CiiAli as decimal(5,2)) <> 0 THEN (CASE WHEN Codiva in (select distinct PaCodiva FROM #SPLIT) THEN 'S' ELSE 'I' END) ELSE '' END


FROM #TMP A INNER JOIN
     VDOX.DBO.TbAna B ON AnaCod = CliFor AND AnaGrp = 'FO' inner join
	 TbCii on CiiCod = Codiva
--WHERE aNAcfIS <> 'R' or rtrim(AnaPivaEst) <> ''
--WHERE LEN(AnaCFis) > 10 or Len(anaPiva) = 11 or rtrim(AnaPivaEst) <> ''
ORDER BY Clifor,data,NumDoc 




GO
/****** Object:  StoredProcedure [dbo].[X_GeneraPoE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[X_GeneraPoE] @ANNO AS SMALLINT, @TIPO AS VARCHAR(2)
AS

SELECT distinct PRegAnno,PRegTipo,PRegCliFor,AnaDesc,DiCognome,DiNome,DiDataNasc,DiComune,DiProv,DiStato,DiDenomina,DiECitta,DiEStato,DiEIndiri
into #tmp 
from TMPXMLREGIVA 
inner join VDOX.dbo.TbAna on AnaCod = PRegCliFor and AnaGrp = PRegTipo
LEFT OUTER JOIN TbEPoE ON DiCfTipo = PRegTipo and DiCfCodice=PRegCliFor and DiCfAnno = PRegAnno
where PRegAnno = @ANNO and AnaPiva='' and AnaCfis='' and (AnaPivaEst='' or  AnaPivaEst='.')

DELETE FROM TbEPoe WHERE DiCfCodice NOT IN (select PRegCliFor from #TMP where PRegAnno = DiCfAnno and PRegTipo= DiCfTipo) AND DiCfAnno = @ANNO

INSERT INTO TbEPoE(DiCfAnno,DiCfTipo,DiCfCodice)
select PRegAnno,PRegTipo,PRegCliFor from #tmp where PRegCliFor not in( select DiCfCodice from TbEPoe where PRegAnno = DiCfAnno and PRegTipo= DiCfTipo)

select * from #tmp 
WHERE PRegTipo = @TIPO
Order By PRegTipo,PRegCliFor


GO
/****** Object:  StoredProcedure [dbo].[X_LEGGI_REGXML]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[X_LEGGI_REGXML] @ANNO AS SMALLINT, @TIPO AS VARCHAR(1), @PERIODO AS TINYINT
AS


select distinct Escludi=priregiva, pa=CAST(1 AS BIT) 
INTO #escludi
from tbpri 
where pricodiva =(select PaCodiva from TbPaCii where PATIPO='IVA')  and DATEPART(YEAR,PridataGio) = @ANNO 
UNION

select distinct Escludi=RIvaAutoFCee,pa=Cast(0 as bit) 
from tbregiva 
where rivaanno = @ANNO and rivaautoFcee > 0

---- ELIMINO EVENTUALI REGISTRI NON APPARTENENTI A PA.
select DISTINCT PRIREGIVA 
into #noescludi
from TbPri  
where PRICODIVA <> (select pacodiva from TbPaCii where PaTipo = 'IVA')  and PriCodIva <> 0
AND DATEPART(YEAR,PridataGio) = @ANNO  AND PriRegIva IN (SELECT Escludi FROM #escludi WHERE PA = 1)
AND PriCodIva NOT IN (SELECT cIICOD FROM TbCii WHERE CiiNatura <> '')

delete from #escludi where escludi in (select priregiva from #noescludi)

----------------------------------------------------------------------------

SELECT  RIvaAnno, RIvaNReg, RIvaNumFog,RivaTipo,RIvaDesc, 
        RIvaTipoDesc = CASE RIvaTipo WHEN '1' THEN cast(rivatipo AS varchar) + ' - Vendite' 
		                             WHEN '2' THEN cast(rivatipo AS varchar) + ' - Acquisti' 
									 WHEN '3' THEN cast(rivatipo AS varchar) + ' - Rett. Vendite' 
									 WHEN '4' THEN cast(rivatipo AS varchar) + ' - Rett. Acquisti' 
									 WHEN '5' THEN cast(rivatipo AS varchar) + ' - Corrispettivi' 
									 WHEN '6' THEN cast(rivatipo AS varchar) + ' - In sospensione' 
									 WHEN '7' THEN cast(rivatipo AS varchar) + ' - Acquisti Attivit Agricoltura' 
									 WHEN '8' THEN cast(rivatipo AS varchar) + ' - Autofatture Agricoltura' 
									 WHEN '9' THEN cast(rivatipo AS varchar) + ' - Riepilogativo' END,
		SEL = CASE WHEN @PERIODO = 0 THEN CAST(0 AS BIT) ELSE ISNULL(RxSel,CAST(0 AS BIT)) END,
		RxPeriodo= isnull(rxperiodo,0),
		RxTipo = CASE WHEN isnull(rxperiodo,0) = 0 THEN (CASE WHEN (RIvaTipo = 1 Or RIvaTipo = 3) THEN 'V'  WHEN (RIvaTipo = 2 Or RIvaTipo = 4) THEN 'A' ELSE '' END) ELSE RxTipo END,
		NUOVO = CASE WHEN  isnull(rxperiodo,0) = 0 THEN cast(1 as bit) else cast(0 as bit) end,
		ABILITATO = CASE WHEN (SELECT count(*) from #escludi where Escludi = RivaNreg) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
into #tmp
from TbRegiva left outer join
     TbRegXml on RxAnno = RivaAnno and RxRegistro = RivaNreg and RxPeriodo = @PERIODO

IF @PERIODO = 0
   BEGIN
   GOTO ESCI
   END

update #tmp
       set SEL = CASE WHEN (SELECT count(*) from #escludi where Escludi = RivaNreg) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,
	       ABILITATO = CASE WHEN (SELECT count(*) from #escludi where Escludi = RivaNreg) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
WHERE NUOVO = 1



ESCI:
select * FROM #TMP WHERE RIvaAnno = @anno and RxTipo = @TIPO




GO
/****** Object:  StoredProcedure [dbo].[X_LEGGI_REGXML_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROC [dbo].[X_LEGGI_REGXML_EST] @ANNO AS SMALLINT, @TIPO AS VARCHAR(1), @PERIODO AS smallint
AS


select distinct Escludi=priregiva, pa=CAST(1 AS BIT) 
INTO #escludi
from tbpri 
where pricodiva =(select PaCodiva from TbPaCii where PATIPO='IVA')  and DATEPART(YEAR,PridataGio) = @ANNO 

UNION

select distinct Escludi=RIvaAutoFCee,pa=Cast(0 as bit) 
from tbregiva 
where rivaanno = @ANNO and rivaautoFcee > 0

--UNION

--select distinct Escludi = FteRegistro,pa=Cast(0 as bit) 
--from GEVE.dbo.TbFte
--where FteAnno = @ANNO


---- ELIMINO EVENTUALI REGISTRI NON APPARTENENTI A PA.
select DISTINCT PRIREGIVA 
into #noescludi
from TbPri  
where PRICODIVA <> (select pacodiva from TbPaCii where PaTipo = 'IVA')  and PriCodIva <> 0
AND DATEPART(YEAR,PridataGio) = @ANNO  AND PriRegIva IN (SELECT Escludi FROM #escludi WHERE PA = 1)
AND PriCodIva NOT IN (SELECT cIICOD FROM TbCii WHERE CiiNatura <> '')

delete from #escludi where escludi in (select priregiva from #noescludi)

----------------------------------------------------------------------------

SELECT  RIvaAnno, RIvaNReg, RIvaNumFog,RivaTipo,RIvaDesc, 
        RIvaTipoDesc = CASE RIvaTipo WHEN '1' THEN cast(rivatipo AS varchar) + ' - Vendite' 
		                             WHEN '2' THEN cast(rivatipo AS varchar) + ' - Acquisti' 
									 WHEN '3' THEN cast(rivatipo AS varchar) + ' - Rett. Vendite' 
									 WHEN '4' THEN cast(rivatipo AS varchar) + ' - Rett. Acquisti' 
									 WHEN '5' THEN cast(rivatipo AS varchar) + ' - Corrispettivi' 
									 WHEN '6' THEN cast(rivatipo AS varchar) + ' - In sospensione' 
									 WHEN '7' THEN cast(rivatipo AS varchar) + ' - Acquisti Attivit Agricoltura' 
									 WHEN '8' THEN cast(rivatipo AS varchar) + ' - Autofatture Agricoltura' 
									 WHEN '9' THEN cast(rivatipo AS varchar) + ' - Riepilogativo' END,
		SEL = CASE WHEN @PERIODO = 0 THEN CAST(0 AS BIT) ELSE ISNULL(RxSel,CAST(0 AS BIT)) END,
		RxPeriodo= isnull(rxperiodo,0),
		RxTipo = CASE WHEN isnull(rxperiodo,0) = 0 THEN (CASE WHEN (RIvaTipo = 1 Or RIvaTipo = 3) THEN 'V'  WHEN (RIvaTipo = 2 Or RIvaTipo = 4) THEN 'A' ELSE '' END) ELSE RxTipo END,
		NUOVO = CASE WHEN  isnull(rxperiodo,0) = 0 THEN cast(1 as bit) else cast(0 as bit) end,
		ABILITATO = CASE WHEN (SELECT count(*) from #escludi where Escludi = RivaNreg) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
into #tmp
from TbRegiva left outer join
     TbRegXml_EST on RxAnno = RivaAnno and RxRegistro = RivaNreg and RxPeriodo = @PERIODO

IF @PERIODO = 0
   BEGIN
   GOTO ESCI
   END

update #tmp
       set SEL = CASE WHEN (SELECT count(*) from #escludi where Escludi = RivaNreg) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,
	       ABILITATO = CASE WHEN (SELECT count(*) from #escludi where Escludi = RivaNreg) = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
WHERE NUOVO = 1



ESCI:
select * FROM #TMP WHERE RIvaAnno = @anno and RxTipo = @TIPO



GO
/****** Object:  StoredProcedure [dbo].[XArtPSucc]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[XArtPSucc] @ArtPProg as int, @ArtPId as int,@ud as smallint
as
update tbartp set artpprog=0 where artpprog=@ArtPProg and ArtPId=@ArtPId and (not artpprog>=(select max(ArtPProg) from TbArtp where ArtPId=@ArtPId) or @ud<0)
update tbartp set artpprog=@ArtPProg where artpprog=@ArtPProg+@ud and ArtPId=@ArtPId
update tbartp set artpprog=@ArtPProg+@ud where artpprog=0 and ArtPId=@ArtPId
GO
/****** Object:  StoredProcedure [dbo].[XAZZGIO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XAZZGIO] @UDATA AS SMALLDATETIME
as
update TBPRI set PRIGSTAMPA = 0,PRIARTFISC = 0 WHERE PRIDATAGIO >= @Udata


GO
/****** Object:  StoredProcedure [dbo].[XAZZIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[XAZZIVA] @DAL AS SMALLDATETIME,@AL AS SMALLDATETIME,@NREG AS SMALLINT
as
DECLARE @DbWhere AS VARCHAR(30),@DbWherA AS VARCHAR(20),@DbWherB AS VARCHAR(20),@cmd as varchar(2000),@AG AS SMALLDATETIME
SET @AG = dateadd(DAY,15,@AL)
if @NREG > 0
  begin
   SET @DbWhere = ' AND PRINTIVAPriRegIva = '+CAST(@NREG AS VARCHAR(2)) + ')'
   SET @DbWherA = ' AND IVAPREGIVA = '+CAST(@NREG AS VARCHAR(2))
   SET @DbWherB = ' AND CORRREGIVA = '+CAST(@NREG AS VARCHAR(2))
   end
else
   begin
   SET @DbWhere = ' AND PRINTIVAPriRegIva <> 0 )'
   SET @DbWherA = ' AND IVAPREGIVA <> 0'
   SET @DbWherB = ' AND CORRREGIVA <> 0'
end
set @cmd = 'UPDATE TBPRI SET PRIIVAPRINT = 0 WHERE PRIID in (SELECT PRINTIVAPriid from TbPrintIva where (PRINTIVAFinoAl BETWEEN  '''+CONVERT(VARCHAR(20),@DAL,103)+''' AND '''+CONVERT(VARCHAR(20),@AL,103)+''')'+@DBWHERE
EXEC(@cmd)
set @cmd = 'DELETE TBIVAP WHERE IVAPANNO = datepart(year,'''+CONVERT(VARCHAR(20),@DAL,103)+''') AND IVAPMESE BETWEEN datepart(month,'''+CONVERT(VARCHAR(20),@DAL,103)+''') and datepart(month,'''+CONVERT(VARCHAR(20),@AL,103)+''')'+@DBWHERA
EXEC(@cmd)
set @cmd = 'DELETE TBCORR WHERE CORRANNO = datepart(year,'''+CONVERT(VARCHAR(20),@DAL,103)+''') AND CORRMESE BETWEEN datepart(month,'''+CONVERT(VARCHAR(20),@DAL,103)+''') and datepart(month,'''+CONVERT(VARCHAR(20),@AL,103)+''')'+@DBWHERB
EXEC(@cmd)
set @cmd = 'UPDATE GEVE.DBO.TbFte_Passiva SET FteFinoAl = NULL WHERE (FteFinoAl BETWEEN  '''+CONVERT(VARCHAR(20),@DAL,103)+''' AND '''+CONVERT(VARCHAR(20),@AL,103)+''')'
EXEC(@cmd)
set @cmd = 'DELETE FROM TbPrintIva where ((PRINTIVAFinoAl BETWEEN  '''+CONVERT(VARCHAR(20),@DAL,103)+''' AND '''+CONVERT(VARCHAR(20),@AL,103)+''')'+@DBWHERE
EXEC(@cmd)


GO
/****** Object:  StoredProcedure [dbo].[XAZZIVA2018]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[XAZZIVA2018] @DAL AS SMALLDATETIME,@AL AS SMALLDATETIME,@NREG AS SMALLINT
as
DECLARE @DbWhere AS VARCHAR(20),@DbWherA AS VARCHAR(20),@DbWherB AS VARCHAR(20),@cmd as varchar(2000)
if @NREG > 0
  begin
   SET @DbWhere = ' AND PRIREGIVA = '+CAST(@NREG AS VARCHAR(2))
   SET @DbWherA = ' AND IVAPREGIVA = '+CAST(@NREG AS VARCHAR(2))
   SET @DbWherB = ' AND CORRREGIVA = '+CAST(@NREG AS VARCHAR(2))
   end
else
   begin
   SET @DbWhere = ' AND PRIREGIVA <> 0'
   SET @DbWherA = ' AND IVAPREGIVA <> 0'
   SET @DbWherB = ' AND CORRREGIVA <> 0'
end
set @cmd = 'UPDATE TBPRI SET PRIIVAPRINT = 0 WHERE (PRIDATAGIO BETWEEN  '''+CONVERT(VARCHAR(20),@DAL,103)+''' AND '''+CONVERT(VARCHAR(20),@AL,103)+''')'+@DBWHERE
EXEC(@cmd)

set @cmd = 'DELETE TBIVAP WHERE IVAPANNO = datepart(year,'''+CONVERT(VARCHAR(20),@DAL,103)+''') AND IVAPMESE BETWEEN datepart(month,'''+CONVERT(VARCHAR(20),@DAL,103)+''') and datepart(month,'''+CONVERT(VARCHAR(20),@AL,103)+''')'+@DBWHERA
EXEC(@cmd)

set @cmd = 'DELETE TBCORR WHERE CORRANNO = datepart(year,'''+CONVERT(VARCHAR(20),@DAL,103)+''') AND CORRMESE BETWEEN datepart(month,'''+CONVERT(VARCHAR(20),@DAL,103)+''') and datepart(month,'''+CONVERT(VARCHAR(20),@AL,103)+''')'+@DBWHERB
EXEC(@cmd)

DELETE FROM TbPrintIva where PRINTIVAFinoAl <='31/12/2018'

GO
/****** Object:  StoredProcedure [dbo].[XBILCEE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[XBILCEE] @BLOCK as int
AS

-- IDENTICO A XBILSEZ CAMBIA SOLO LA TABELLA TMPCEE in Luogo di TMPSEZ -- 27/07/2018

DELETE FROM TMPCEE
DBCC checkident(TMPCEE,reseed,0)


select distinct TOP 100 PERCENT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,sum(TMSDARE) as TMSDARE,SUM(TMSAVERE) AS TMSAVERE,SUM(TMSSALDO) AS TMSSALDO,
SUM(TMSDAREP) AS TMSDAREP,SUM(TMSAVEREP) AS TMSAVEREP,SUM(TMSSALDOP) AS TMSSALDOP,TMSMASTRO,
TMSSPPP = CASE WHEN TMSFLAG <> 6 AND TMSFLAG <> 7 then 0 else 1 end,TMSDEMAS
into #tmp FROM TMPBILS
where SUBSTRING(TMSCONTO,4,2) <> '00' and TMSBLOCK = @BLOCK
group by TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSMASTRO,TMSDEMAS
ORDER BY TMSSPPP,TMSMASTRO,TMSCONTO


SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPA FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) > 0 AND TMSSPPP = 0
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPP FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) < 0 AND TMSSPPP = 0

SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPC FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) > 0 AND TMSSPPP = 1
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPR FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) < 0 AND TMSSPPP = 1

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS1 FROM #TMPA GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPA GROUP BY TMSCONTO,TMSDESC ORDER BY codice

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS2 FROM #TMPP GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPP GROUP BY TMSCONTO,TMSDESC ORDER BY codice

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS3 FROM #TMPC GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPC GROUP BY TMSCONTO,TMSDESC ORDER BY codice

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS4 FROM #TMPR GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPR GROUP BY TMSCONTO,TMSDESC ORDER BY codice

DECLARE @COD AS VARCHAR(5),@DESC AS VARCHAR(100),@IMP AS DECIMAL(12,2),@W AS VARCHAR(5)

if (SELECT COUNT(*) FROM #TMPS1) >= (SELECT COUNT(*) FROM #TMPS2) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT CODICE, descrizione,importo,'','',0,0 FROM #TMPS1

DECLARE DATISCA CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS2 ORDER BY CODICE
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS2CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS1CODICE)
 UPDATE TMPCEE SET TMPS2CODICE = @COD, TMPS2DESC=@DESC,TMPS2SALDO=@IMP WHERE TMPSID = @W
FINE_LOOP:
   FETCH NEXT FROM DATISCA
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCA
DEALLOCATE DATISCA
END

if (SELECT COUNT(*) FROM #TMPS1) < (SELECT COUNT(*) FROM #TMPS2) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT '','',0,CODICE, descrizione,importo,0 FROM #TMPS2 ORDER BY CODICE
DECLARE DATISCAX CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS1 ORDER BY CODICE
OPEN DATISCAX
FETCH NEXT FROM DATISCAX
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS1CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS2CODICE)
 UPDATE TMPCEE SET TMPS1CODICE = @COD, TMPS1DESC=@DESC,TMPS1SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPX:
   FETCH NEXT FROM DATISCAX
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAX
DEALLOCATE DATISCAX
END

if (SELECT COUNT(*) FROM #TMPS3) >= (SELECT COUNT(*) FROM #TMPS4) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT CODICE, descrizione,importo,'','',0,1 FROM #TMPS3

DECLARE DATISCAY CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS4 ORDER BY CODICE
OPEN DATISCAY
FETCH NEXT FROM DATISCAY
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS2CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS1CODICE)
 UPDATE TMPCEE SET TMPS2CODICE = @COD, TMPS2DESC=@DESC,TMPS2SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPY:
   FETCH NEXT FROM DATISCAY
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAY
DEALLOCATE DATISCAY
END

if (SELECT COUNT(*) FROM #TMPS3) < (SELECT COUNT(*) FROM #TMPS4) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT '','',0,CODICE, descrizione,importo,1 FROM #TMPS4 ORDER BY CODICE
DECLARE DATISCAZ CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS3 ORDER BY CODICE
OPEN DATISCAZ
FETCH NEXT FROM DATISCAZ
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS1CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS2CODICE)
 UPDATE TMPCEE SET TMPS1CODICE = @COD, TMPS1DESC=@DESC,TMPS1SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPZ:
   FETCH NEXT FROM DATISCAZ
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAZ
DEALLOCATE DATISCAZ
END

--- INIZIO BILANCIO FORMAZIONE BILANCIO CEE


select  TMPSID,TMPS1CODICE,TMPS1DESC,
TMPS1CEECOD=(select Ceecod from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod),
TMPS1CEEDESC=(select CEEDESC from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod),
TMPS1CEEFL=(select CeeSt from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod),
TMPS1SALDO,
TMPSPPP INTO #SEZ from TMPCEE where (select Ceecod from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod) > '00000'
union ALL
select  TMPSID,TMPS2CODICE,TMPS2DESC,
TMPS2CEECOD=(select Ceecod from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod),
TMPS2CEEDESC=(select CEEDESC from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod),
TMPS2CEEFL=(select CeeSt from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod),
TMPS2SALDO,TMPSPPP
from TMPCEE where (select Ceecod from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod) > '00000' ORDER BY TMPSPPP,tmps1ceecod,TMPS1CEEFL desc

SELECT * INTO #ALT FROM TbCee WHERE CeeAlt > 0

UPDATE #SEZ SET TMPS1CEECOD=CEEalt,TMPS1CEEDESC=CEEDESC,TMPS1CEEFL=CeeSt FROM #SEZ ,#ALT WHERE TMPS1CEECOD=CEECOD AND TMPS1SALDO < 0

SELECT CeeCod,CeeDesc,CeeSt,CODICE=CAST('' as VARCHAR(5)),DESCR=CAST('' AS VARCHAR(100)),DESCEE = CAST('' AS VARCHAR(120)),SALDO=cast(0.00 as decimal(12,2)) INTO #CEE FROM TBCEE WHERE CEECOD > '00000' AND cEEST BETWEEN 7 AND 9 ORDER BY CeeCod,CeeSt DESC

--select * from #CEE
--SELECT * FROM #SEZ

INSERT INTO #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO)
SELECT TMPS1CEECOD,TMPS1CEEDESC,TMPS1CEEFL,TMPS1CODICE,TMPS1DESC,TMPS1CEEDESC,TMPS1SALDO FROM #SEZ

select distinct P=Row_Number() OVER (ORDER BY CEECOD,CEEST,CODICE ),* into #LAVORO from #CEE order by CEECOD,CEEST,CODICE

declare @CCOD as smallint,@DDESC AS varchar,@ST as smallint,@SALDO as decimal(12,2),@P as int
declare @TOTALE9 as decimal(12,2),@TOTALE8 as decimal(12,2),@TOTALE7 as decimal(12,2)

SET @TOTALE9=0
SET @TOTALE8=0
SET @TOTALE7=0

DECLARE DATISCAW CURSOR FOR
SELECT DISTINCT P,CEECOD,CEEDESC,CEEST,SALDO from #LAVORO ORDER BY P DESC 
OPEN DATISCAW
FETCH NEXT FROM DATISCAW
INTO @P,@CCOD,@DDESC,@ST,@SALDO
WHILE @@FETCH_STATUS = 0
BEGIN
SET @TOTALE9=@TOTALE9 + @SALDO
SET @TOTALE8=@TOTALE8 + @SALDO
SET @TOTALE7=@TOTALE7 + @SALDO
IF @ST=9
BEGIN
 UPDATE #CEE SET SALDO=@TOTALE9 WHERE CEECOD=@CCOD
 SET @TOTALE9=0
 END
 IF @ST=8
BEGIN
 UPDATE #CEE SET SALDO=@TOTALE8 WHERE CEECOD=@CCOD
 SET @TOTALE8=0
 END
 IF @ST=7
BEGIN
 UPDATE #CEE SET SALDO=@TOTALE7 WHERE CEECOD=@CCOD
 SET @TOTALE7=0
 END

FINE_LOOPW:
   FETCH NEXT FROM DATISCAW
   INTO @P,@CCOD,@DDESC,@ST,@SALDO
END
CLOSE DATISCAW
DEALLOCATE DATISCAW

----RIGO UTILI o PERDITA
DECLARE @PPUTILA as decimal(12,2),@SPUTILA as decimal(12,2), @SPCHIUSURA as varchar(100),@PPCHIUSURA as varchar(100),@VP as varchar(100),@AI as varchar(100)
DECLARE @RIGOSP as smallint,@RIGOPP as smallint,@RIGOTT as smallint,@RIGVP as smallint,@RIGCP as smallint,@RIGAI as smallint,@RIG1T as smallint,@RIG2T as smallint,@VPDIFF as decimal(12,2),@AIDIFF as decimal(12,2)

set @RIGVP=3380
set @RIGCP=3664
set @RIGAI=4308
set @RIG1T=4304
set @RIG2T=4720

set @VP = 'Differenza tra valore e costi di produzione (A-B)'
set @AI = 'Risultato prima delle imposte(A-B+-C+-D)'

set @VPDIFF = (SELECT SUM(SALDO)  FROM #CEE WHERE CEECOD = @RIGVP OR CEECOD = @RIGCP) 
set @AIDIFF = (SELECT SUM(SALDO)  FROM #CEE WHERE CEECOD = @RIGVP OR CEECOD = @RIGCP OR CEECOD=@RIGAI) 

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO)
(select @RIG1T,@VP,7,'',@VP,@VP,@VPDIFF)

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO)
(select @RIG2T,@AI,7,'',@AI,@AI,@AIDIFF)


set @RIGOTT = 5050
set @RIGOSP = 5052
set @RIGOPP = 5054

set @SPCHIUSURA = '() Utile (Perdita) dell' + '''' + 'esercizio SITUAZIONE PATRIMONIALE'
set @PPCHIUSURA = '() Utile (Perdita) dell' + '''' + 'esercizio CONTO ECONOMICO'

SET @SPUTILA = (select sum(TMPS1SALDO)  from #SEZ where TMPSPPP = 0)--SITUAZIONE PATRIMONIALE )
SET @PPUTILA = (select sum(TMPS1SALDO)  from #SEZ where TMPSPPP = 1)--CONTO ECONOMICO )

--SET @PPUTILP = (select sum(TMPS1SALDOA) from #SEZ where TMPSPPP = 1)--2 anno )
--SET @CCUTILA =(CASE WHEN @PPUTILA <=0 THEN (select EseUti from tbese where EseAnno=@ANNO) ELSE (select EsePer from tbese where EseAnno=@ANNO) END)
--SET @CCUTILP =(CASE WHEN @PPUTILP <=0 THEN (select EseUti from tbese where EseAnno=@ANNO) ELSE (select EsePer from tbese where EseAnno=@ANNO) END)
--SET @FLUTILA = (SELECT PiaFl11 from tbpia where piacodco = @CCUTILA)
--SET @FLUTILP = (SELECT PiaFl11 from tbpia where piacodco = @CCUTILP)
--SET @DDUTILA = (SELECT Piaanaco from tbpia where piacodco = @CCUTILA)
--SET @DDUTILP = (SELECT Piaanaco from tbpia where piacodco = @CCUTILP)


-- rigo intestazione utili7perdite con totale ( 1 solo )
Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO)
(select @RIGOTT,(select Ceedesc from TbCee where CeeCod=@RIGOTT),
(select CeeSt from TbCee where CeeCod=@RIGOTT),'',(select Ceedesc from TbCee where CeeCod=@RIGOTT),(select Ceedesc from TbCee where CeeCod=@RIGOTT),@PPUTILA)


Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO)
(select @RIGOSP,@SPCHIUSURA,6,'',@SPCHIUSURA,@SPCHIUSURA,@SPUTILA)

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO)
(select @RIGOPP,@PPCHIUSURA,6,'',@PPCHIUSURA,@PPCHIUSURA,@PPUTILA)
-- --


DELETE FROM #CEE WHERE saldo = 0
update #CEE set saldo=0 where saldo is null
DELETE FROM TBBILCEE

insert into TbBilCee
SELECT *,0.00 FROM #CEE ORDER BY CeeCod

GO
/****** Object:  StoredProcedure [dbo].[XBILCEE2ANNI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XBILCEE2ANNI] @BLOCK as int,@ANNO as smallint
AS

DELETE FROM TMPCEE
DBCC checkident(TMPCEE,reseed,0)

select distinct TOP 100 PERCENT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,sum(TMSDARE) as TMSDARE,SUM(TMSAVERE) AS TMSAVERE,SUM(TMSSALDO) AS TMSSALDO,
SUM(TMSDAREP) AS TMSDAREP,SUM(TMSAVEREP) AS TMSAVEREP,SUM(TMSSALDOP) AS TMSSALDOP,
TMSPDARE=0.00,TMSPAVERE=0.00,TMSPSALDO=0.00,TMSPDAREP=0.00,TMSPAVEREP=0.00,TMSPSALDOP=0.00,
TMSMASTRO,
TMSSPPP = CASE WHEN TMSFLAG <> 6 AND TMSFLAG <> 7 then 0 else 1 end,TMSDEMAS
 INTO #TMP FROM TMPANNS
 where SUBSTRING(TMSCONTO,4,2) <> '00' and TMSBLOCK = @BLOCK
group by TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSMASTRO,TMSDEMAS
UNION ALL
select distinct TOP 100 PERCENT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE=0.00,TMSAVERE=0.00,TMSSALDO=0.00,TMSDAREP=0.00,TMSAVEREP=0.00,TMSSALDOP=0.00,
sum(TMSPDARE) as TMSPDARE,SUM(TMSPAVERE) AS TMSPAVERE,SUM(TMSPSALDO) AS TMSPSALDO,
SUM(TMSPDAREP) AS TMSPDAREP,SUM(TMSPAVEREP) AS TMSPAVEREP,SUM(TMSPSALDOP) AS TMSPSALDOP,TMSMASTRO,
TMSSPPP = CASE WHEN TMSFLAG <> 6 AND TMSFLAG <> 7 then 0 else 1 end,TMSDEMAS
 FROM TMPANNS
 where SUBSTRING(TMSCONTO,4,2) <> '00' and TMSBLOCK = @BLOCK
group by TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSMASTRO,TMSDEMAS
ORDER BY TMSSPPP,TMSMASTRO,TMSCONTO

SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPS1 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) > 0 AND TMSSPPP = 0
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPS2 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) < 0 AND TMSSPPP = 0

SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPS3 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) > 0 AND TMSSPPP = 1
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPS4 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) < 0 AND TMSSPPP = 1

SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSPSALDO + TMSPSALDOP) INTO #TMPS5 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSPSALDO + TMSPSALDOP) > 0 AND TMSSPPP = 0
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSPSALDO + TMSPSALDOP) INTO #TMPS6 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSPSALDO + TMSPSALDOP) < 0 AND TMSSPPP = 0

SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSPSALDO + TMSPSALDOP) INTO #TMPS7 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSPSALDO + TMSPSALDOP) > 0 AND TMSSPPP = 1
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSPSALDO + TMSPSALDOP) INTO #TMPS8 FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSPSALDO + TMSPSALDOP) < 0 AND TMSSPPP = 1

DECLARE @COD AS VARCHAR(5),@DESC AS VARCHAR(100),@IMP AS DECIMAL(12,2),@W AS VARCHAR(5)

if (SELECT COUNT(*) FROM #TMPS1) >= (SELECT COUNT(*) FROM #TMPS2) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT TMSCONTO, TMSDESC,SALDO,'','',0,'','',0,'','',0,0 FROM #TMPS1

DECLARE DATISCA CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS2 ORDER BY TMSCONTO
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS2CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS1CODICE)
 UPDATE TMPCEE SET TMPS2CODICE = @COD, TMPS2DESC=@DESC,TMPS2SALDO=@IMP WHERE TMPSID = @W
FINE_LOOP:
   FETCH NEXT FROM DATISCA
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCA
DEALLOCATE DATISCA
END

if (SELECT COUNT(*) FROM #TMPS1) < (SELECT COUNT(*) FROM #TMPS2) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT '','',0,TMSCONTO, TMSDESC,SALDO,'','',0,'','',0,0 FROM #TMPS2 ORDER BY TMSCONTO
DECLARE DATISCAX CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS1 ORDER BY TMSCONTO
OPEN DATISCAX
FETCH NEXT FROM DATISCAX
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS1CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS2CODICE)
 UPDATE TMPCEE SET TMPS1CODICE = @COD, TMPS1DESC=@DESC,TMPS1SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPX:
   FETCH NEXT FROM DATISCAX
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAX
DEALLOCATE DATISCAX
END

if (SELECT COUNT(*) FROM #TMPS3) >= (SELECT COUNT(*) FROM #TMPS4) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT TMSCONTO, TMSDESC,SALDO,'','',0,'','',0,'','',0,1 FROM #TMPS3 ORDER BY TMSCONTO

DECLARE DATISCAY CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS4 ORDER BY TMSCONTO
OPEN DATISCAY
FETCH NEXT FROM DATISCAY
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS2CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS1CODICE)
 UPDATE TMPCEE SET TMPS2CODICE = @COD, TMPS2DESC=@DESC,TMPS2SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPY:
   FETCH NEXT FROM DATISCAY
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAY
DEALLOCATE DATISCAY
END

if (SELECT COUNT(*) FROM #TMPS3) < (SELECT COUNT(*) FROM #TMPS4) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT '','',0,TMSCONTO, TMSDESC,SALDO,'','',0,'','',0,1 FROM #TMPS4 ORDER BY TMSCONTO
DECLARE DATISCAZ CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS3 ORDER BY TMSCONTO
OPEN DATISCAZ
FETCH NEXT FROM DATISCAZ
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS1CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS2CODICE)
 UPDATE TMPCEE SET TMPS1CODICE = @COD, TMPS1DESC=@DESC,TMPS1SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPZ:
   FETCH NEXT FROM DATISCAZ
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAZ
DEALLOCATE DATISCAZ
END

---- REM ANNO PRECEDENTE
if (SELECT COUNT(*) FROM #TMPS5) >= (SELECT COUNT(*) FROM #TMPS6) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT '','',0,'','',0,TMSCONTO, TMSDESC,SALDO,'','',0,0 FROM #TMPS5

DECLARE DATISCA5 CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS6 ORDER BY TMSCONTO
OPEN DATISCA5
FETCH NEXT FROM DATISCA5
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS4CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS3CODICE)
 UPDATE TMPCEE SET TMPS4CODICE = @COD, TMPS4DESC=@DESC,TMPS4SALDO=@IMP WHERE TMPSID = @W
FINE_LOOP5:
   FETCH NEXT FROM DATISCA5
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCA5
DEALLOCATE DATISCA5
END

if (SELECT COUNT(*) FROM #TMPS5) < (SELECT COUNT(*) FROM #TMPS6) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT '','',0,'','',0,'','',0,TMSCONTO, TMSDESC,SALDO,0 FROM #TMPS6 ORDER BY TMSCONTO
DECLARE DATISCA6 CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS5 ORDER BY TMSCONTO
OPEN DATISCA6
FETCH NEXT FROM DATISCA6
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS3CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS4CODICE)
 UPDATE TMPCEE SET TMPS3CODICE = @COD, TMPS3DESC=@DESC,TMPS3SALDO=@IMP WHERE TMPSID = @W
FINE_LOOP6:
   FETCH NEXT FROM DATISCA6
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCA6
DEALLOCATE DATISCA6
END

if (SELECT COUNT(*) FROM #TMPS7) >= (SELECT COUNT(*) FROM #TMPS8) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT '','',0,'','',0,TMSCONTO, TMSDESC,SALDO,'','',0,1 FROM #TMPS7 ORDER BY TMSCONTO

DECLARE DATISCA7 CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS8 ORDER BY TMSCONTO
OPEN DATISCA7
FETCH NEXT FROM DATISCA7
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS4CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS3CODICE)
 UPDATE TMPCEE SET TMPS4CODICE = @COD, TMPS4DESC=@DESC,TMPS4SALDO=@IMP WHERE TMPSID = @W
FINE_LOOP7:
   FETCH NEXT FROM DATISCA7
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCA7
DEALLOCATE DATISCA7
END

if (SELECT COUNT(*) FROM #TMPS7) < (SELECT COUNT(*) FROM #TMPS8) 
BEGIN
INSERT INTO TMPCEE(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPS3CODICE,TMPS3DESC,TMPS3SALDO,TMPS4CODICE,TMPS4DESC,TMPS4SALDO,TMPSPPP)
SELECT '','',0,'','',0,'','',0,TMSCONTO, TMSDESC,SALDO,1 FROM #TMPS8 ORDER BY TMSCONTO
DECLARE DATISCA8 CURSOR FOR
SELECT TMSCONTO, TMSDESC,SALDO FROM #TMPS7 ORDER BY TMSCONTO
OPEN DATISCA8
FETCH NEXT FROM DATISCA8
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPCEE WHERE TMPS3CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS4CODICE)
 UPDATE TMPCEE SET TMPS3CODICE = @COD, TMPS3DESC=@DESC,TMPS3SALDO=@IMP WHERE TMPSID = @W
FINE_LOOP8:
   FETCH NEXT FROM DATISCAZ
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCA8
DEALLOCATE DATISCA8
END

-- INIZIO BILANCIO FORMAZIONE BILANCIO CEE

select  TMPSID,TMPS1CODICE,TMPS1DESC,
TMPS1CEECOD=(select Ceecod from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod),
TMPS1CEEDESC=(select CEEDESC from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod),
TMPS1CEEFL=(select CeeSt from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod),
TMPS1SALDO,TMPS1SALDOA = 0.00,
TMPSPPP INTO #SEZ from TMPCEE where (select Ceecod from tbcee inner join tbpia on piacodco = TMPS1CODICE and PIAFL11 = CeeCod) > '00000'
union ALL
select  TMPSID,TMPS2CODICE,TMPS2DESC,
TMPS2CEECOD=(select Ceecod from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod),
TMPS2CEEDESC=(select CEEDESC from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod),
TMPS2CEEFL=(select CeeSt from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod),
TMPS2SALDO,0.00,TMPSPPP
from TMPCEE where (select Ceecod from tbcee inner join tbpia on piacodco = TMPS2CODICE and PIAFL11 = CeeCod) > '00000' 
union ALL
select  TMPSID,TMPS3CODICE,TMPS3DESC,
TMPS3CEECOD=(select Ceecod from tbcee inner join tbpia on piacodco = TMPS3CODICE and PIAFL11 = CeeCod),
TMPS3CEEDESC=(select CEEDESC from tbcee inner join tbpia on piacodco = TMPS3CODICE and PIAFL11 = CeeCod),
TMPS3CEEFL=(select CeeSt from tbcee inner join tbpia on piacodco = TMPS3CODICE and PIAFL11 = CeeCod),
0.00,TMPS3SALDO,TMPSPPP
from TMPCEE where (select Ceecod from tbcee inner join tbpia on piacodco = TMPS3CODICE and PIAFL11 = CeeCod) > '00000' 

union ALL
select  TMPSID,TMPS4CODICE,TMPS4DESC,
TMPS4CEECOD=(select Ceecod from tbcee inner join tbpia on piacodco = TMPS4CODICE and PIAFL11 = CeeCod),
TMPS4CEEDESC=(select CEEDESC from tbcee inner join tbpia on piacodco = TMPS4CODICE and PIAFL11 = CeeCod),
TMPS4CEEFL=(select CeeSt from tbcee inner join tbpia on piacodco = TMPS4CODICE and PIAFL11 = CeeCod),
0.00,TMPS4SALDO,TMPSPPP
from TMPCEE where (select Ceecod from tbcee inner join tbpia on piacodco = TMPS4CODICE and PIAFL11 = CeeCod) > '00000' ORDER BY TMPSPPP,tmps1ceecod,TMPS1CEEFL desc


SELECT * INTO #ALT FROM TbCee WHERE CeeAlt > 0

UPDATE #SEZ SET TMPS1CEECOD=CEEalt,TMPS1CEEDESC=CEEDESC,TMPS1CEEFL=CeeSt FROM #SEZ ,#ALT WHERE TMPS1CEECOD=CEECOD AND TMPS1SALDO < 0
UPDATE #SEZ SET TMPS1CEECOD=CEEalt,TMPS1CEEDESC=CEEDESC,TMPS1CEEFL=CeeSt FROM #SEZ ,#ALT WHERE TMPS1CEECOD=CEECOD AND TMPS1SALDOA < 0

SELECT CeeCod,CeeDesc,CeeSt,CODICE=CAST('' as VARCHAR(5)),DESCR=CAST('' AS VARCHAR(100)),DESCEE = CAST('' AS VARCHAR(120)),SALDO=cast(0.00 as decimal(12,2)) ,SALDOA=cast(0.00 as decimal(12,2)) INTO #CEE FROM TBCEE WHERE CEECOD > '00000' AND cEEST BETWEEN 7 AND 9 ORDER BY CeeCod,CeeSt DESC

INSERT INTO #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
SELECT TMPS1CEECOD,TMPS1CEEDESC,TMPS1CEEFL,TMPS1CODICE,TMPS1DESC,TMPS1CEEDESC,TMPS1SALDO,TMPS1SALDOA FROM #SEZ


----RIGO UTILI o PERDITA
--DECLARE @PPUTILA as decimal(12,2),@PPUTILP as decimal(12,2),@CCUTILA as varchar(5),@CCUTILP AS VARCHAR(5),@FLUTILA as smallint,@FLUTILP as smallint,@DDUTILA as varchar(100),@DDUTILP as varchar(100)


--SET @PPUTILA = (select sum(TMPS1SALDO)  from #SEZ where TMPSPPP = 1)--1 anno )
--SET @PPUTILP = (select sum(TMPS1SALDOA) from #SEZ where TMPSPPP = 1)--2 anno )
--SET @CCUTILA =(CASE WHEN @PPUTILA <=0 THEN (select EseUti from tbese where EseAnno=@ANNO) ELSE (select EsePer from tbese where EseAnno=@ANNO) END)
--SET @CCUTILP =(CASE WHEN @PPUTILP <=0 THEN (select EseUti from tbese where EseAnno=@ANNO) ELSE (select EsePer from tbese where EseAnno=@ANNO) END)
--SET @FLUTILA = (SELECT PiaFl11 from tbpia where piacodco = @CCUTILA)
--SET @FLUTILP = (SELECT PiaFl11 from tbpia where piacodco = @CCUTILP)
--SET @DDUTILA = (SELECT Piaanaco from tbpia where piacodco = @CCUTILA)
--SET @DDUTILP = (SELECT Piaanaco from tbpia where piacodco = @CCUTILP)

--Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
-- (select @FLUTILA,(select Ceedesc from TbCee where CeeCod=@FLUTILA),
-- (select CeeSt from TbCee where CeeCod=@FLUTILA),@CCUTILA,@DDUTILA,(select Ceedesc from TbCee where CeeCod=@FLUTILA),@PPUTILA,0)

-- Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
-- (select @FLUTILP,(select Ceedesc from TbCee where CeeCod=@FLUTILP),
-- (select CeeSt from TbCee where CeeCod=@FLUTILP),@CCUTILP,@DDUTILP,(select Ceedesc from TbCee where CeeCod=@FLUTILP),0,@PPUTILP)

-- --


select distinct P=Row_Number() OVER (ORDER BY CEECOD,CEEST,CODICE ),* into #LAVORO from #CEE order by CEECOD,CEEST,CODICE

declare @CCOD as smallint,@DDESC AS varchar,@ST as smallint,@SALDO as decimal(12,2),@P as int
declare @TOTALE9 as decimal(12,2),@TOTALE8 as decimal(12,2),@TOTALE7 as decimal(12,2)

SET @TOTALE9=0
SET @TOTALE8=0
SET @TOTALE7=0

DECLARE DATISCAW CURSOR FOR
SELECT DISTINCT P,CEECOD,CEEDESC,CEEST,SALDO from #LAVORO ORDER BY P DESC 
OPEN DATISCAW
FETCH NEXT FROM DATISCAW
INTO @P,@CCOD,@DDESC,@ST,@SALDO
WHILE @@FETCH_STATUS = 0
BEGIN
SET @TOTALE9=@TOTALE9 + @SALDO
SET @TOTALE8=@TOTALE8 + @SALDO
SET @TOTALE7=@TOTALE7 + @SALDO
IF @ST=9
BEGIN
 UPDATE #CEE SET SALDO=@TOTALE9 WHERE CEECOD=@CCOD AND CEEST = 9
 SET @TOTALE9=0
 END
 IF @ST=8
BEGIN
 UPDATE #CEE SET SALDO=@TOTALE8 WHERE CEECOD=@CCOD AND CEEST = 8
 SET @TOTALE8=0
 END
 IF @ST=7
BEGIN
 UPDATE #CEE SET SALDO=@TOTALE7 WHERE CEECOD=@CCOD AND CEEST = 7
 SET @TOTALE7=0
 END

FINE_LOOPW:
   FETCH NEXT FROM DATISCAW
   INTO @P,@CCOD,@DDESC,@ST,@SALDO
END
CLOSE DATISCAW
DEALLOCATE DATISCAW

--- SALDO COLONNA DI DESTRA

SET @TOTALE9=0
SET @TOTALE8=0
SET @TOTALE7=0

DECLARE DATISCAA CURSOR FOR
SELECT DISTINCT P,CEECOD,CEEDESC,CEEST,SALDOA from #LAVORO ORDER BY P DESC 
OPEN DATISCAA
FETCH NEXT FROM DATISCAA
INTO @P,@CCOD,@DDESC,@ST,@SALDO
WHILE @@FETCH_STATUS = 0
BEGIN
SET @TOTALE9=@TOTALE9 + @SALDO
SET @TOTALE8=@TOTALE8 + @SALDO
SET @TOTALE7=@TOTALE7 + @SALDO
IF @ST=9
BEGIN
 UPDATE #CEE SET SALDOA=@TOTALE9 WHERE CEECOD=@CCOD AND CEEST = 9
 SET @TOTALE9=0
 END
 IF @ST=8
BEGIN
 UPDATE #CEE SET SALDOA=@TOTALE8 WHERE CEECOD=@CCOD AND CEEST = 8
 SET @TOTALE8=0
 END
 IF @ST=7
BEGIN
 UPDATE #CEE SET SALDOA=@TOTALE7 WHERE CEECOD=@CCOD AND CEEST = 7
 SET @TOTALE7=0
 END

FINE_LOOPA:
   FETCH NEXT FROM DATISCAA
   INTO @P,@CCOD,@DDESC,@ST,@SALDO
END
CLOSE DATISCAA
DEALLOCATE DATISCAA

----RIGO UTILI o PERDITA
DECLARE @SPUTILA as decimal(12,2),@SPUTILP as decimal(12,2),@SPCHIUSURA as varchar(100),@VP as varchar(100),@AI as varchar(100)
DECLARE @PPUTILA as decimal(12,2),@PPUTILP as decimal(12,2),@PPCHIUSURA as varchar(100),@VPDIFF as decimal(12,2),@AIDIFF as decimal(12,2),@VPDIFA as decimal(12,2),@AIDIFA as decimal(12,2)

DECLARE @RIGOSP as smallint,@RIGOPP as smallint,@RIGOTT as smallint,@RIGVP as smallint,@RIGCP as smallint,@RIGAI as smallint,@RIG1T as smallint,@RIG2T as smallint

set @RIGVP=3380
set @RIGCP=3664
set @RIGAI=4308
set @RIG1T=4304
set @RIG2T=4720


set @VP = 'Differenza tra valore e costi di produzione (A-B)'
set @AI = 'Risultato prima delle imposte(A-B+-C+-D)'

set @VPDIFF = (SELECT SUM(SALDO)  FROM #CEE WHERE CEECOD = @RIGVP OR CEECOD = @RIGCP) 
set @VPDIFA = (SELECT SUM(SALDOA) FROM #CEE WHERE CEECOD = @RIGVP OR CEECOD = @RIGCP) 
set @AIDIFF = (SELECT SUM(SALDO)  FROM #CEE WHERE CEECOD = @RIGVP OR CEECOD = @RIGCP OR CEECOD=@RIGAI) 
set @AIDIFA = (SELECT SUM(SALDOA) FROM #CEE WHERE CEECOD = @RIGVP OR CEECOD = @RIGCP OR CEECOD=@RIGAI) 

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIG1T,@VP,7,'',@VP,@VP,@VPDIFF,@VPDIFA)

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIG2T,@AI,7,'',@AI,@AI,@AIDIFF,@AIDIFA)

set @RIGOTT = 5050
set @RIGOSP = 5052
set @RIGOPP = 5054

set @SPCHIUSURA = '() Utile (Perdita) dell' + '''' + 'esercizio SITUAZIONE PATRIMONIALE'
set @PPCHIUSURA = '() Utile (Perdita) dell' + '''' + 'esercizio CONTO ECONOMICO'


SET @SPUTILA = (select sum(TMPS1SALDO)  from #SEZ where TMPSPPP = 0)--SITUAZIONE PATRIMONIALE  -- 1 ANNO)
SET @PPUTILA = (select sum(TMPS1SALDO)  from #SEZ where TMPSPPP = 1)--CONTO ECONOMICO ) -- 1 ANNO

SET @SPUTILP = (select sum(TMPS1SALDOA) from #SEZ where TMPSPPP = 0)--SITUAZIONE PATRIMONIALE -- 2 ANNO )
SET @PPUTILP = (select sum(TMPS1SALDOA) from #SEZ where TMPSPPP = 1)--CONTO ECONOMICO ) -- 2 ANNO


-- rigo intestazione utili7perdite con totale ( 1 solo )
Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIGOTT,(select Ceedesc from TbCee where CeeCod=@RIGOTT),
(select CeeSt from TbCee where CeeCod=@RIGOTT),'',(select Ceedesc from TbCee where CeeCod=@RIGOTT),(select Ceedesc from TbCee where CeeCod=@RIGOTT),@PPUTILA,0)


Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIGOSP,@SPCHIUSURA,6,'',@SPCHIUSURA,@SPCHIUSURA,@SPUTILA,0)

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIGOPP,@PPCHIUSURA,6,'',@PPCHIUSURA,@PPCHIUSURA,@PPUTILA,0)

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIGOTT,(select Ceedesc from TbCee where CeeCod=@RIGOTT),
(select CeeSt from TbCee where CeeCod=@RIGOTT),'',(select Ceedesc from TbCee where CeeCod=@RIGOTT),(select Ceedesc from TbCee where CeeCod=@RIGOTT),0,@PPUTILP)


Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIGOSP,@SPCHIUSURA,6,'',@SPCHIUSURA,@SPCHIUSURA,0,@SPUTILP)

Insert into #CEE(CeeCod,CeeDesc,CeeSt,CODICE,DESCR,DESCEE,SALDO,SALDOA)
(select @RIGOPP,@PPCHIUSURA,6,'',@PPCHIUSURA,@PPCHIUSURA,0,@PPUTILP)


-- --
DELETE FROM #CEE WHERE saldo = 0 AND SALDOA = 0
update #CEE set saldo=0 where saldo is null
update #CEE set saldoa=0 where saldoa is null

DELETE FROM TBBILCEE
insert into TbBilCee
SELECT * FROM #CEE ORDER BY CeeCod



GO
/****** Object:  StoredProcedure [dbo].[XBILSEZ]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[XBILSEZ] @BLOCK as int
AS

DELETE FROM TMPSEZ
DBCC checkident(TMPSEZ,reseed,0)


select distinct TOP 100 PERCENT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,sum(TMSDARE) as TMSDARE,SUM(TMSAVERE) AS TMSAVERE,SUM(TMSSALDO) AS TMSSALDO,
SUM(TMSDAREP) AS TMSDAREP,SUM(TMSAVEREP) AS TMSAVEREP,SUM(TMSSALDOP) AS TMSSALDOP,TMSMASTRO,
TMSSPPP = CASE WHEN TMSFLAG <> 6 AND TMSFLAG <> 7 then 0 else 1 end,TMSDEMAS
into #tmp FROM TMPBILS
where SUBSTRING(TMSCONTO,4,2) <> '00' and TMSBLOCK = @BLOCK
group by TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSMASTRO,TMSDEMAS
ORDER BY TMSSPPP,TMSMASTRO,TMSCONTO


SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPA FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) > 0 AND TMSSPPP = 0
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPP FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) < 0 AND TMSSPPP = 0

SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPC FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) > 0 AND TMSSPPP = 1
SELECT TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC,SALDO=SUM(TMSSALDO + TMSSALDOP) INTO #TMPR FROM #TMP GROUP BY TMSBLOCK,TMSMASTRO,TMSDEMAS,TMSCONTO,TMSDESC, TMSSPPP HAVING SUM(TMSSALDO + TMSSALDOP) < 0 AND TMSSPPP = 1

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS1 FROM #TMPA GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPA GROUP BY TMSCONTO,TMSDESC ORDER BY codice

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS2 FROM #TMPP GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPP GROUP BY TMSCONTO,TMSDESC ORDER BY codice

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS3 FROM #TMPC GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPC GROUP BY TMSCONTO,TMSDESC ORDER BY codice

SELECT CODICE=tmsmastro+'.00',DESCRIZIONE=tmsdemas,IMPORTO=sum(saldo) into #TMPS4 FROM #TMPR GROUP BY tmsmastro,tmsdemas
union all
SELECT CODICE=TMSCONTO,TMSDESC,sum(saldo) FROM #TMPR GROUP BY TMSCONTO,TMSDESC ORDER BY codice

DECLARE @COD AS VARCHAR(5),@DESC AS VARCHAR(50),@IMP AS DECIMAL(12,2),@W AS VARCHAR(5)

if (SELECT COUNT(*) FROM #TMPS1) >= (SELECT COUNT(*) FROM #TMPS2) 
BEGIN
INSERT INTO TMPSEZ(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT CODICE, descrizione,importo,'','',0,0 FROM #TMPS1 ORDER BY CODICE

DECLARE DATISCA CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS2 ORDER BY CODICE
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPSEZ WHERE TMPS2CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS1CODICE)
 UPDATE TMPSEZ SET TMPS2CODICE = @COD, TMPS2DESC=@DESC,TMPS2SALDO=@IMP WHERE TMPSID = @W
FINE_LOOP:
   FETCH NEXT FROM DATISCA
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCA
DEALLOCATE DATISCA
END

if (SELECT COUNT(*) FROM #TMPS1) < (SELECT COUNT(*) FROM #TMPS2) 
BEGIN
INSERT INTO TMPSEZ(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT '','',0,CODICE, descrizione,importo,0 FROM #TMPS2 ORDER BY CODICE
DECLARE DATISCAX CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS1 ORDER BY CODICE
OPEN DATISCAX
FETCH NEXT FROM DATISCAX
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPSEZ WHERE TMPS1CODICE = '' AND TMPSPPP = 0 ORDER BY TMPS2CODICE)
 UPDATE TMPSEZ SET TMPS1CODICE = @COD, TMPS1DESC=@DESC,TMPS1SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPX:
   FETCH NEXT FROM DATISCAX
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAX
DEALLOCATE DATISCAX
END

if (SELECT COUNT(*) FROM #TMPS3) >= (SELECT COUNT(*) FROM #TMPS4) 
BEGIN
INSERT INTO TMPSEZ(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT CODICE, descrizione,importo,'','',0,1 FROM #TMPS3 ORDER BY CODICE

DECLARE DATISCAY CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS4 ORDER BY CODICE
OPEN DATISCAY
FETCH NEXT FROM DATISCAY
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPSEZ WHERE TMPS2CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS1CODICE)
 UPDATE TMPSEZ SET TMPS2CODICE = @COD, TMPS2DESC=@DESC,TMPS2SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPY:
   FETCH NEXT FROM DATISCAY
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAY
DEALLOCATE DATISCAY
END

if (SELECT COUNT(*) FROM #TMPS3) < (SELECT COUNT(*) FROM #TMPS4) 
BEGIN
INSERT INTO TMPSEZ(TMPS1CODICE,TMPS1DESC,TMPS1SALDO,TMPS2CODICE,TMPS2DESC,TMPS2SALDO,TMPSPPP)
SELECT '','',0,CODICE, descrizione,importo,1 FROM #TMPS4 ORDER BY CODICE
DECLARE DATISCAZ CURSOR FOR
SELECT CODICE, descrizione,importo FROM #TMPS3 ORDER BY CODICE
OPEN DATISCAZ
FETCH NEXT FROM DATISCAZ
INTO @COD,@DESC,@IMP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @W=(SELECT TOP 1 TMPSID FROM TMPSEZ WHERE TMPS1CODICE = '' AND TMPSPPP = 1 ORDER BY TMPS2CODICE)
 UPDATE TMPSEZ SET TMPS1CODICE = @COD, TMPS1DESC=@DESC,TMPS1SALDO=@IMP WHERE TMPSID = @W
FINE_LOOPZ:
   FETCH NEXT FROM DATISCAZ
   INTO @COD,@DESC,@IMP
END
CLOSE DATISCAZ
DEALLOCATE DATISCAZ
END



 
GO
/****** Object:  StoredProcedure [dbo].[XCALCOLASCAPIANO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[XCALCOLASCAPIANO] @CLIENTE varchar(5),@DATAOGGI AS SMALLDATETIME
as
--DROP TABLE  #TMP
--DROP TABLE  #TMPDUP
SELECT [IDGRUPPO],[IDRATA],[IDDATAFAT],[IDNRFAT],[IDRATACLI],[IDIMPORTORATA],[IDIMPORTOFAT],[IDRESIDUOCLI],[IDCCLIE],[DATASCADENZA],[RATACONCORDATA],[DATACREAZIONE],
[DATACREASCADENZE],[IDANAGRAFICA],[IDSEQUENZA], 
[ScaId],[ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]
				   into #TMP
				   FROM TMPPIANO 
INNER JOIN TBSCA ON SCACONTO= IDCCLIE AND ScaDdoc=IDDATAFAT AND ScaNdoc=IDNRFAT AND ScaImpDoc=IDIMPORTOFAT AND SCANRATA=IDRATACLI AND ScaDsca=DATASCADENZA
WHERE IDCCLIE = @CLIENTE AND SCARAPERTA <>1 AND ScaImpDoc <> 0 AND DATASCADENZA <=@DATAOGGI--<=GETDATE()
ORDER BY IDRATA,IDSEQUENZA

--SELECT * FROM #TMP

SELECT DISTINCT IDIMPORTORATA,DATASCADENZA,IDCCLIE,IDRATACLI into #TMPDUP FROM #TMP

--SELECT * FROM #TMPDUP
--INNER JOIN TBSCA ON IDCCLIE=ScaConto AND SCANRATA = IDRATACLI AND ScaImpRata=IDIMPORTORATA AND ScaDsca=DATASCADENZA

SELECT ISNULL(SUM(IDIMPORTORATA),0) FROM #TMPDUP
INNER JOIN TBSCA ON IDCCLIE=ScaConto AND SCANRATA = IDRATACLI AND ScaImpRata=IDIMPORTORATA AND ScaDsca=DATASCADENZA

GO
/****** Object:  StoredProcedure [dbo].[XCesp]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XCesp] @Data as smalldatetime, @idblk as int
as
--set @Data='31/01/2005'
--set @idblk=135
declare
@CespNum as smallint,@CespAnnoA as smallint,@CespCat as varchar(2),@CespAliFis as decimal(5,2),@CespAliTab as decimal(5,2),
@QuoCoAmm as decimal(13,2),@QuoTipoAmm as smallint,@QuoAli as decimal(5,2),@QuoQuota as decimal(13,2),@QuoFondo as decimal(13,2),
@QuoResiduo as decimal(13,2),@VCespCaus as smallint,@VCespVariazioni as decimal(13,2),@CspCp as smallint,@QuoCoStor as decimal(13,2),
@QuoNoDetra as decimal(13,2),@CespDescr as varchar(110),@CespContoQuota as varchar(5),@CespContoFondo as varchar(5), @Anno as smallint,
@PercLib as decimal(13,2),@Mesi as smallint,@InizioP as smalldatetime,@FineP as smalldatetime

--- NUOVA PER ESERCIZIO INFRANNUALE
SET @Anno =(select Eseanno from Tbese where @data between EseDal and EseAl)
SET @InizioP =(select EseDal from Tbese where EseAnno = @Anno)
SET @FineP  =(select EseAl from Tbese where EseAnno = @Anno)

set @mesi =datediff("mm",@inizioP,@Data) + 1

--set @Anno=datepart(year,@Data)
--set @Mesi=datepart(month,@Data)

if @idblk = 0
begin
insert into TmpAmmLib (AmmLibBlock,AmmLibCat,AmmLibPerc)
select distinct 0,cespcat,0 from tbcesp
END 


DECLARE Cespiti CURSOR FOR
select CespNum,CespDescr,CespAnnoA,CespCat,CespCp,CespAliFis,CespAliTab,CespContoQuota,CespContoFondo,QuoCoStor,QuoCoAmmIni,QuoTipoAmm,QuoAli,QuoQuota,
QuoFondoIni,QuoResiduoIni,QuoNoDetraIni,sum(isnull(VCespVariazioni,'0')) as VCespVariazioni, AmmLibPerc
from TbCesp
inner join tbquo on Quonum=cespnum 
left outer join TbVCesp on VCespNum=QuoNum and VCespAnno=QuoAnno and not cast(substring(VCespGgMm,1,2)+'/'+substring(VCespGgMm,3,2)+'/'+cast(VCespAnno as varchar(4)) as smalldatetime)>@Data
left outer join TMPAMMLIB on AmmLibCat=CespCat
where not CespDataFat > @Data and ISNULL(VCespCaus,'') <> 2 and ISNULL(VCespCaus,'') <> 3 
and QuotipoAmm<> 3 and QuoAnno=@Anno
and AmmLibBlock=@idblk
group by CespNum,CespDescr,CespAnnoA,CespCat,CespCp,CespAliFis,CespAliTab,CespContoQuota,CespContoFondo,QuoCoStor,QuoCoAmmIni,QuoTipoAmm,QuoAli,QuoQuota,QuoFondoIni,QuoResiduoIni,QuoNoDetraIni,AmmLibPerc
order by CespCat,CespAnnoA,CespNum

select top 1 CespNum,CespCat,CespAnnoA,CespAliTab,CespAliFis,cast(0 as decimal(13,2)) as AliLib,CespContoQuota,CespContoFondo,QuoCoAmm,
cast(0 as decimal(13,2)) as Ammortam, cast(0 as decimal(13,2)) as Anticip, cast(0 as decimal(13,2)) as Meta,cast(0 as decimal(13,2)) as AmmLib,CespDescr,QuoFondo
into #tmp from TbCesp inner join tbquo on Quonum=cespnum left outer join TbVCesp on VCespNum=QuoNum and VCespAnno=QuoAnno
where not CespDataFat > @Data and ISNULL(VCespCaus,'') <> 2 
and QuotipoAmm<> 3 order by CespNum

delete from #tmp

OPEN Cespiti
FETCH NEXT FROM Cespiti
INTO @CespNum,@CespDescr,@CespAnnoA,@CespCat,@CspCp,@CespAliFis,@CespAliTab,@CespContoQuota,@CespContoFondo,@QuoCoStor,@QuoCoAmm,@QuoTipoAmm,
	@QuoAli,@QuoQuota,@QuoFondo,@QuoResiduo,@QuoNoDetra,@VCespVariazioni,@PercLib
WHILE @@FETCH_STATUS = 0
BEGIN

Declare @PerAmm as decimal(13,2), @PerAnt as decimal(13,2),@Ammortam as decimal(13,2),
        @Anticip as decimal(13,2),@Meta as decimal(13,2),@WsSaldo as decimal(13,2),@AnnoDiff as smallint,@AmmLib as decimal(13,2)

if @CespAliFis>0
      begin
        set @PerAmm=@CespAliFis
      end
    else
      begin
        set @PerAmm=@CespAliTab
      end

set @QuoCoAmm = sum(@QuoCoAmm + isnull(@VCespVariazioni,'0'))

set @QuoFondo=@Quofondo+(select isnull(sum(VCespVariazioni),0) from TbVcesp where VCespNum=@CespNum and VCespCaus=3 and VCespAnno=@Anno and not cast(substring(VCespGgMm,1,2)+'/'+substring(VCespGgMm,3,2)+'/'+cast(VCespAnno as varchar(4)) as smalldatetime)>@Data)
set @QuoCoAmm=@QuoCoAmm+(select isnull(sum(VCespVariazioni),0) from TbVcesp where VCespNum=@CespNum and VCespCaus=2 and VCespAnno=@Anno and not cast(substring(VCespGgMm,1,2)+'/'+substring(VCespGgMm,3,2)+'/'+cast(VCespAnno as varchar(4)) as smalldatetime)>@Data)
set @QuoResiduo=@QuoCoAmm-@QuoFondo

if @CespAnnoA=@Anno
begin
    if @QuoTipoAmm<>4 and @CspCp=0 and @PerAmm<100
      begin
        set @PerAmm=sum(@PerAmm  / 2)
        set @PerAnt=@PerAmm
        set @PercLib=sum(@PercLib / 2)
      end
    
    set @Ammortam = sum(@QuoCoAmm * @PerAmm / 100)
    set @Anticip = sum(@QuoCoAmm * @PerAnt / 100)
    set @Meta = sum(@Ammortam / 2)
    set @AmmLib = sum(@QuoCoAmm * @PercLib / 100)

    if @QuoFondo<>0 and @Ammortam>(@QuoResiduo) and @CspCp=0
      begin
	set @Ammortam = @QuoResiduo
	set @Anticip = 0.00
	set @Meta = 0.00
	set @AmmLib = 0.00
      end

end

else

begin
    if not @quoResiduo > 0.00
      begin
	set @Ammortam = 0.00
	set @Anticip = 0.00
	set @Meta = 0.00
	set @AmmLib = 0.00
      end
    else
      begin
        set @AnnoDiff = sum(@Anno-@CespAnnoA)
        if @CespAnnoA<1988
          begin
	    if @AnnoDiff > 2
	      begin
	        set @PerAnt = 0.00
	      end
	    else
	      begin
	        set @PerAnt = 15
	      end

	  end
        else
          begin
            if @AnnoDiff > 2
	      begin
	        set @PerAnt = 0.00
	      end
	    else
	      begin
	        set @PerAnt = @PerAmm
	      end
	  end
            
        set @Ammortam = sum(@QuoCoAmm * @PerAmm / 100)

        if @QuoCoStor < @QuoCoAmm
          begin
	    set @WsSaldo = @QuoResiduo-- + isnull(@VCespVariazioni,'0')
          end
        else
          begin
	    set @WsSaldo = sum(@QuoCoAmm-@QuoFondo-@QuoNoDetra)
          end

	set @Anticip = sum(@QuoCoAmm * @PerAnt / 100)
	set @Meta = sum(@Ammortam / 2)
	set @AmmLib = sum(@QuoCoAmm * @PercLib / 100)

        if @Ammortam > @QuoResiduo
          begin
	    set @Ammortam  = @QuoResiduo
	    set @Anticip = 0.00
	  end

        if @Meta > @QuoResiduo
          begin
	    set @Meta  = @QuoResiduo
          end

	if @AmmLib > @QuoResiduo
          begin
	    set @AmmLib  = @QuoResiduo
          end
      
      set @WsSaldo = sum(@WsSaldo - @Ammortam)

      if @Anticip > @QuoResiduo
        begin
	  set @Anticip  = @QuoResiduo
        end
    end
end

if @CspCp=1 or @PerAmm=100
  begin
    set @Anticip = 0
    set @Meta = 0
  end

insert into #tmp (CespNum,CespCat,CespAnnoA,CespAliFis,CespAliTab,AliLib,CespContoQuota,CespContoFondo,QuoCoAmm,Ammortam,Anticip,Meta,AmmLib,CespDescr,QuoFondo) 
          values (@CespNum,@CespCat,@CespAnnoA,@CespAliFis,@CespAliTab,@PercLib,@CespContoQuota,@CespContoFondo,@QuoCoAmm,@Ammortam/12*@Mesi,@Anticip/12*@Mesi,@Meta/12*@Mesi,@AmmLib/12*@Mesi,@CespDescr,@QuoFondo)

   FETCH NEXT FROM Cespiti
   INTO @CespNum,@CespDescr,@CespAnnoA,@CespCat,@CspCp,@CespAliFis,@CespAliTab,@CespContoQuota,@CespContoFondo,@QuoCoStor,@QuoCoAmm,@QuoTipoAmm,
	@QuoAli,@QuoQuota,@QuoFondo,@QuoResiduo,@QuoNoDetra,@VCespVariazioni,@PercLib
END
CLOSE Cespiti
DEALLOCATE Cespiti

delete from TMPPROQUO
insert into TMPPROQUO
select * from #tmp WITH (TABLOCKX)
DELETE FROM TmpAmmLib where AmmLibBlock = @Idblk



GO
/****** Object:  StoredProcedure [dbo].[XCESPITIANNO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[XCESPITIANNO] @ANNO AS SMALLINT
AS


select AziAnnoLavoro ,AziGruppoCesp,AziSpecieCesp,AziSottosCesp,CspDesc,CspNum into #tmp from tbazi
 inner join TbCsp on CspGru = AziGruppoCesp and CspSpe1 =AziSpecieCesp and CspSpe2 =AziSottosCesp
 where CspNum > 0  group by AziAnnoLavoro ,AziGruppoCesp,AziSpecieCesp,AziSottosCesp,CspDesc,CspNum
 having AziAnnoLavoro = (select MAX(aziannoLavoro) from TbAzi)
 order by cspnum
 
 select * from TbCesp inner join #tmp on CespCat = cspnum where CespAnnoA = @anno
GO
/****** Object:  StoredProcedure [dbo].[XCOMIVA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE procedure [dbo].[XCOMIVA] @ANNO AS SMALLINT,@T AS SMALLINT
AS

--DECLARE @ANNO AS SMALLINT,@T AS SMALLINT

--SET @ANNO=2016
--SET @T=1

DECLARE @VP1 AS SMALLINT,@VP2 AS DECIMAL(12,2),@VP3 AS DECIMAL(12,2),@VP4 AS DECIMAL(12,2),@VP5 AS DECIMAL(12,2),@VP6 AS DECIMAL(12,2),@VP7 AS DECIMAL(12,2),@VP8 AS DECIMAL(12,2),@VP9 AS DECIMAL(12,2)
DECLARE @VP10 AS DECIMAL(12,2),@VP11 AS DECIMAL(12,2),@VP12 AS DECIMAL(12,2),@VP13 AS DECIMAL(12,2),@VP14 AS DECIMAL(12,2),@DAL AS SMALLINT,@AAL AS SMALLINT,@LIMITE AS DECIMAL (12,2),@LIMACC AS DECIMAL(12,2)
DECLARE @MESE AS smallint,@VPSUB AS BIT,@VPEVENTI AS VARCHAR(1),@VPOPERAZIONI AS BIT,@VPMETODO AS smallint,@BASEPRORATA AS DECIMAL(12,2),@VP5MENO as decimal(12,2),@PERCPRORATA as decimal(12,2)

-- CALCOLOPRORATA
SET @BASEPRORATA=0


-- LIMITE VERSAMENTO
SET @LIMITE = 25.82

-- LIMITE ACCONTO
SET @LIMACC=103.29

SET @MESE=0
SET @VP2=0
SET @VP3=0
SET @VP4=0
SET @VP5=0
SET @VP5MENO = 0
SET @VP6=0
SET @VP7=0
SET @VP8=0
SET @VP9=0
SET @VP10=0
SET @VP11=0
SET @VP12=0
SET @VP13=0
SET @VP14=0
SET @VPSUB=0
SET @VPEVENTI=''
SET @VPOPERAZIONI = 0
SET @VPMETODO=0



--MENSILE=0 o TRIMESTRALE=1
DECLARE @MT AS SMALLINT
SET @MT=(SELECT AZIREGIMEIVA FROM TBAZI WHERE AZIANNOLAVORO = @ANNO)

SET @PERCPRORATA=(SELECT AZIESENTE FROM TBAZI WHERE AZIANNOLAVORO = @ANNO)

SET @DAL=(@T - 1) * 3 + 1
SET @AAL = @DAL + 2


select Escludi=RIvaAutoFCee into #RIVAC from tbregiva where rivaanno = @ANNO and rivaautoFcee > 0


IF @MT = 0 GOTO MENSILE

SET @VP1=@T
IF @VP1 = 4
BEGIN
SET @VP1=5
END


SET @VP2 = isnull((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5) and IvaPmese between @DAL and @AAL and CiiTp between 1 and 3 and RivaNReg not in (select Escludi from #RIVAC)),0) 
    

SET @VP3 = isnull((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 2 or RivaTipo = 4 or RivaTipo = 7) and IvaPmese between @DAL and @AAL and CiiTp between 1 and 3),0)


SET @VP4 = isnull((SELECT SUM(IvaPIvaDe) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5) and IvaPmese between @DAL and @AAL and CiiTp between 1 and 3 and CiiCod not in (select PaCodiva from TbPaCii where PATIPO='IVA')),0)

SET @VP5 = isnull((SELECT SUM(IvaPIvaDe) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 2 or RivaTipo = 4 or RivaTipo = 7) and IvaPmese between @DAL and @AAL and CiiTp between 1 and 3),0)
  -- EVENTUALE PRORATA
  IF @PERCPRORATA > 0
  BEGIN
  SET @BASEPRORATA = isnull((SELECT SUM(IvaPIvaDe) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and  RivaPrata=1 and (RivaTipo = 2 or RivaTipo = 4 or RivaTipo = 7) and IvaPmese between @DAL and @AAL and CiiTp between 1 and 3),0)
  set @VP5MENO = (@BASEPRORATA * @PERCPRORATA) / 100 
  set @VP5 = @VP5 - @VP5MENO
   END


-- CREDITI D'IMPOSTA
SET @VP11=isnull((SELECT abs(IvaVCrImUt) from TbVers where IvaVanno=@ANNO and IvaVMese =@AAL),0) * -1

-- INTERESSI TRIMESTRALI
SET @VP12=isnull((SELECT IvaVInteressi from TbVers where IvaVanno=@ANNO and IvaVMese =@AAL),0)

SET @VP6 = (@VP4 - @VP5) --- @VP12

IF @DAL = 1 
BEGIN 
SET @VP7=0
SET @VP8=0
SET @VP9=isnull((SELECT IvaVVersam from TbVers where IvaVanno=@ANNO and IvaVMese = 0),0)

if @VP9 > 0 AND @VP9<=@LIMITE
BEGIN
SET @VP7=@VP9
SET @VP9=0
END
END

IF @DAL > 1
BEGIN
SET @VP7=0
SET @VP9=0
SET @VP8=isnull((SELECT IvaVVersam from TbVers where IvaVanno=@ANNO and IvaVMese = @AAL - 3),0)
IF @VP8 > @LIMITE 
BEGIN
SET @VP8=0
END
if @VP8 > 0 AND @VP8<=@LIMITE
BEGIN
SET @VP7=@VP8
SET @VP8=0
END
END

IF @AAL=12
BEGIN
SET @VP13=isnull((SELECT IvaVVersam from TbVers where IvaVanno=@ANNO and IvaVMese = 14),0)
END

--- CONTROLLO SE ESISTONO CAMPI IN PRECEDENZA

SET @VP10 = isnull(((SELECT abs(VP10) FROM TbCmVp where VpAnno=@ANNO and VpMese = @AAL) * -1),0)
SET @VPSUB = isnull((SELECT VPsub FROM TbCmVp where VpAnno=@ANNO and VpMese = @AAL),0)
SET @VPEVENTI = isnull((SELECT VPEVENTI FROM TbCmVp where VpAnno=@ANNO and VpMese = @AAL),'')
SET @VPOPERAZIONI = isnull((SELECT VPOperazioni FROM TbCmVp where VpAnno=@ANNO and VpMese = @AAL),0)
SET @VPMETODO = isnull((SELECT VPMetodo FROM TbCmVp where VpAnno=@ANNO and VpMese = @AAL),0)


SET @VP14 = @VP6 + @VP7 + @VP8 + @VP9 + @VP10 + @VP11 + @VP12 - @VP13 

delete from TbCmVp where VpAnno=@ANNO and VpMese = @AAL

insert into TbCmVp(VpAnno,VpMese,Vp1,Vp2,Vp3,Vp4,Vp5,Vp6,Vp7,Vp8,Vp9,Vp10,Vp11,Vp12,Vp13,Vp14,VpSub,VpEventi,VPOperazioni,VPMetodo)
select @ANNO,@AAL,@VP1,@VP2,@VP3,@VP4,@VP5,@VP6,@VP7,@VP8,@VP9,@VP10,@VP11,@VP12,@VP13,@VP14,@VPSUB,@VPEVENTI,@VPOPERAZIONI,@VPMETODO

GOTO FINE

MENSILE:

IF @MESE = 0 SET @MESE =@DAL
IF @MESE > @AAL GOTO FINE
SET @VP2=0
SET @VP3=0
SET @VP4=0
SET @VP5=0
SET @VP6=0
SET @VP7=0
SET @VP8=0
SET @VP9=0
SET @VP10=0
SET @VP11=0
SET @VP12=0
SET @VP13=0
SET @VP14=0
SET @VPSUB=0
SET @VPEVENTI=''
SET @VPOPERAZIONI = 0
SET @VPMETODO=0

SET @VP1=@MESE

-- MODIFICA PER BAGLIETTO 05/06/2017 ORE 15:00
if NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TbExCiva]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
   begin
SET @VP2 = isnull((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5) and IvaPmese = @MESE and CiiTp between 1 and 3 and RivaNReg not in (select Escludi from #RIVAC)),0)
   end
else
  begin
 SET @VP2 = isnull((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5) and IvaPmese = @MESE and CiiTp between 1 and 3 and RivaNReg not in (select Escludi from #RIVAC) and IvaPCodIva not in (select ExCiva from TbExCiva)),0)
  -- MODIFICA PER BAGLIETTO 12/02/2018 ORE 10:30 
 SET @VP2 = @VP2 + ISNULL((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and ivapmese =@MESE and IvaPCodIva = 63 AND  (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5)),0)
 SET @VP2 = @VP2 - ISNULL((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and ivapmese =@MESE and IvaPCodIva = 42 AND  (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5)),0)
 SET @VP2 = @VP2 + ISNULL((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and ivapmese =@MESE and IvaPCodIva = 64 AND  (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5)),0)
 SET @VP2 = @VP2 + ISNULL((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and ivapmese =@MESE and IvaPCodIva = 54 AND  (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5)),0)
 SET @VP2 = @VP2 + ISNULL((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and ivapmese =@MESE and IvaPCodIva = 43 AND  (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5)),0)
      end


SET @VP3 = isnull((SELECT SUM(IvaPImpon) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 2 or RivaTipo = 4 or RivaTipo = 7) and IvaPmese = @MESE and CiiTp between 1 and 3),0)


SET @VP4 = isnull((SELECT SUM(IvaPIvaDe) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 1 or RivaTipo = 3 or RivaTipo = 5) and IvaPmese = @MESE and CiiTp between 1 and 3 and CiiCod not in (select PaCodiva from TbPaCii where PATIPO='IVA')),0)

SET @VP5 = isnull((SELECT SUM(IvaPIvaDe) from tbivap inner join tbregiva on Rivaanno=ivapanno and RIvaNReg = IvaPRegIva inner join tbcii on IvaPCodIva = CiiCod
  where ivapanno = @anno and (RivaTipo = 2 or RivaTipo = 4 or RivaTipo = 7) and IvaPmese = @MESE and CiiTp between 1 and 3),0)

  -- CREDITI D'IMPOSTA
SET @VP11=isnull((SELECT abs(IvaVCrImUt) from TbVers where IvaVanno=@ANNO and IvaVMese =@MESE),0) * -1

-- interessi normalmente = 0
SET @VP12=isnull((SELECT IvaVInteressi from TbVers where IvaVanno=@ANNO and IvaVMese =@MESE),0)

SET @VP6 = (@VP4 - @VP5) --- @VP12

IF @MESE = 1 
BEGIN 
SET @VP7=0
SET @VP8=0
SET @VP9=isnull((SELECT IvaVVersam from TbVers where IvaVanno=@ANNO and IvaVMese = 0),0)

if @VP9 > 0 AND @VP9<=@LIMITE
BEGIN
SET @VP7=@VP9
SET @VP9=0
END
END

IF @MESE > 1
BEGIN
SET @VP7=0
SET @VP9=0
SET @VP8=isnull((SELECT IvaVVersam from TbVers where IvaVanno=@ANNO and IvaVMese = @MESE - 1 ),0)
IF @VP8 > @LIMITE 
BEGIN
SET @VP8=0
END
if @VP8 > 0 AND @VP8<=@LIMITE
BEGIN
SET @VP7=@VP8
SET @VP8=0
END
END

IF @MESE=12
BEGIN
SET @VP13=isnull((SELECT IvaVVersam from TbVers where IvaVanno=@ANNO and IvaVMese = 14),0)
END

-- CONTROLLO SE ESISTONO CAMPI IN PRECEDENZA

SET @VP10 = ISNULL(((SELECT abs(VP10) FROM TbCmVp where VpAnno=@ANNO and VpMese = @MESE) * -1),0)
SET @VPSUB = ISNULL((SELECT VPsub FROM TbCmVp where VpAnno=@ANNO and VpMese = @MESE),0)
SET @VPEVENTI = ISNULL((SELECT VPEVENTI FROM TbCmVp where VpAnno=@ANNO and VpMese = @MESE),'')
SET @VPOPERAZIONI = isnull((SELECT VPOperazioni FROM TbCmVp where VpAnno=@ANNO and VpMese = @MESE),0)
SET @VPMETODO = isnull((SELECT VPMetodo FROM TbCmVp where VpAnno=@ANNO and VpMese = @MESE),0)


SET @VP14 = @VP6 + @VP7 +@VP8 + @VP9 + @VP10 + @VP11 + @VP12 - @VP13 

delete from TbCmVp where VpAnno=@ANNO and VpMese = @MESE

insert into TbCmVp(VpAnno,VpMese,Vp1,Vp2,Vp3,Vp4,Vp5,Vp6,Vp7,Vp8,Vp9,Vp10,Vp11,Vp12,Vp13,Vp14,VpSub,VpEventi,VPOperazioni,VPMetodo)
select @ANNO,@MESE,@VP1,@VP2,@VP3,@VP4,@VP5,@VP6,@VP7,@VP8,@VP9,@VP10,@VP11,@VP12,@VP13,@VP14,@VPSUB,@VPEVENTI,@VPOPERAZIONI,@VPMETODO

SET @MESE = @MESE + 1
GOTO MENSILE

FINE:








GO
/****** Object:  StoredProcedure [dbo].[XControlloPI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[XControlloPI] @ANNO as smallINT
as

select DISTINCT ANACOD,ANADESC ,ANAPIVA,' ' as ErrP,ANACFIS, ' ' as ErrC,ANAPIVAEST ,' ' as ErrE, ANAGRP from vdox.dbo.tbana 
where anagrp = 'CL' AND ANACOD IN (SELECT PRICODARE FROM TBPRI WHERE DATEPART(YEAR,PRIDATAGIO) = @ANNO AND PRICAUSALE = 3)
union all
select DISTINCT ANACOD,ANADESC ,ANAPIVA,' ' as ErrP,ANACFIS, ' ' as ErrC,ANAPIVAEST,' ' as ErrE ,ANAGRP from vdox.dbo.tbana 
where anagrp = 'FO' AND ANACOD IN (SELECT PRICOAVERE FROM TBPRI WHERE DATEPART(YEAR,PRIDATAGIO) = @ANNO AND PRICAUSALE = 3)
GO
/****** Object:  StoredProcedure [dbo].[XCREASCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[XCREASCADENZE] @IDP as int
AS
CREATE TABLE [dbo].[#TSca] (
[ScaId] [int] IDENTITY (1, 1) NOT NULL ,
	[ScaTipoCo] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaNdoc] [int] NOT NULL ,
	[ScaImpDoc] [decimal](13, 2) NOT NULL ,
    [ScaIvaSpe] [decimal](13, 2) NOT NULL ,
	[ScaAbi] [int] NOT NULL ,
	[ScaCab] [int] NOT NULL ,
	[ScaImpRata] [decimal](13, 2) NOT NULL ,
	[ScaTPag] [smallint] NOT NULL ,
	[ScaCodPag] [smallint] NOT NULL ,
	[ScaNRata] [smallint] NOT NULL ,
	[ScaBan] [smallint] NOT NULL ,
	[ScaDdoc] [smalldatetime] NOT NULL ,
	[ScaDsca] [smalldatetime] NOT NULL ,
	[ScaRifId] [int] NOT NULL ,
	[ScaRifProg] [smallint] NOT NULL ,
	[ScaRifDA] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaRAperta] [varchar] (1) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ScaImpPagato] [decimal](13, 2) NOT NULL 
) ON [PRIMARY]

declare @Cmd varchar(2000),@DbWhere varchar(200)
IF @IDP > 0
   begin
 SET @DbWhere = ' And PriiD = '+CAST(@IDP AS VARCHAR(10))
   end
else
   begin
 SET @DbWhere = ''
 delete from TBSCA
 DBCC CHECKIDENT (TBSCA, RESEED, 0)
   end


set @cmd ='INSERT INTO [dbo].[#TSca] ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]) 
SELECT PRKTIPOCO,PRKCONTO,PRKDOCEST,CASE WHEN PRKDA = 0 THEN PRIIMPDARE ELSE PRIIMPAVERE END,CASE WHEN PRKDA = 0 THEN PRIIMPAVERE ELSE PRIIMPDARE END,0,0,CASE WHEN PRKDA = 0 THEN PRIIMPDARE ELSE PRIIMPAVERE END,
ISNULL(PAGTIPO,0),PRICODPAG,0,CAST(PRILINEA AS SMALLINT),PRIDATAEST,PRIDATAGIO,PRIID,PRIPROG,PRKDA,'''',0 FROM tbpri 
INNER JOIN TBPRK ON PRIID = PRKID AND PRIPROG = PRKPROG
INNER JOIN TBPAG ON PRICODPAG = PAGCOD 
WHERE PRICAUSALE = 3 AND PRKTIPOCO = 1'+@DbWhere
EXEC (@cmd)

DECLARE @CONTO AS INT,@ANNO as smallint
SET @CONTO=(SELECT top 1 ScaConto FROM #TSca WHERE SCARIFID =@IDP)
set @ANNO =(select top 1 datepart(YEAR,SCADDOC) FROM #TSca WHERE SCARIFID =@IDP)
IF ISNUMERIC(@CONTO)=1 and @CONTO IN (select RIvaCliCee from tbregiva where RIvaAnno=@ANNO and RIvaCliCee > 0)
BEGIN
DROP TABLE [dbo].#TSca
GOTO USCITA
END


DECLARE @Fatt as decimal(12,2),@IvaSp as decimal(12,2),@DataFatt as smalldatetime,@CPag as smallint,@ID as int,@NR as smallint,@P AS SMALLINT,@RRATA AS DECIMAL(12,2),@SSCAD AS SMALLDATETIME,
@PID AS INT
DECLARE SERGENTE CURSOR FOR
SELECT SCAID,SCAIMPDOC,SCAIVASPE,SCADDOC,SCACODPAG ,SCARIFID FROM #TSCA --where SCAID = 834
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @id,@fatt,@IvaSp,@datafatt,@cpag,@pid
WHILE @@FETCH_STATUS = 0
BEGIN

---- era fuori ----exec yRATEESCADENZE @Fattura=@fatt,@Spese=@IvaSp,@DataFattura=@DataFatt,@CodPag=@Cpag
DECLARE @Fattura as decimal(12,2),@Spese as decimal(12,2),@DataFattura as smalldatetime,@CodPag as smallint

set @Fattura=@fatt
set @Spese=@IvaSp 
set @DataFattura=@DataFatt
set @CodPag=@Cpag
CREATE TABLE #tmp (
	[NR] [smallint] NOT NULL ,
	[RATA] [decimal](12,2),
	[SCADENZA] [smalldatetime] NOT NULL 
) ON [PRIMARY]
if @codpag = 0 GOTO FINE_LOOP


declare @PagMeseE1 as smallint,@PagMeseE2 as smallint,@PagGgmmE1 as smalldatetime,@PagGgmmE2 as smalldatetime,
@PagSlitE1 as varchar(1),@PagSlitE2 as varchar(2),@PagNRate as smallint,@PagTestGM as smallint,@PagTestS1 as smallint,
@PagTestR1 as smallint
declare @Imponibile as decimal(12,2),@ImpRata as Decimal(12,2),@Resto as Decimal(12,2),@Molti as smallint,@NRA as smallint,@PA as tinyint
declare @VetIm as decimal(12,2),@VetDt as smalldatetime,@PlusDate as smallint,@DayGG as smallint, @DbRate as varchar(50),@DbDate as varchar(50)
declare @current as smalldatetime,@dada as varchar(20)
set @PagMeseE1 = (select PagMeseE1 from TbPag Where @CodPag = PagCod)
set @PagMeseE2 = (select PagMeseE2 from TbPag Where @CodPag = PagCod)
set @PagGgmmE1 = (select PagGgmmE1 from TbPag Where @CodPag = PagCod)
set @PagGgmmE2 = (select PagGgmmE2 from TbPag Where @CodPag = PagCod)
set @PagSlitE1 = (select PagSlitE1 from TbPag Where @CodPag = PagCod)
set @PagSlitE2 = (select PagSlitE2 from TbPag Where @CodPag = PagCod)
set @PagNRate  = (select PagNRate  from TbPag Where @CodPag = PagCod)
set @PagTestGM = (select PagTestGM from TbPag Where @CodPag = PagCod)
set @PagTestS1 = (select PagTestS1 from TbPag Where @CodPag = PagCod)
set @PagTestR1 = (select PagTestR1 from TbPag Where @CodPag = PagCod)
set @molti = 0
set @resto = 0
set @nra = 0
set @PA = 0

if @PagTestR1 = 1
   begin
   SET @Imponibile = @Fattura
   set @Spese = 0
   set @ImpRata = round(@Imponibile / @PagNRate,2)
   end
   if @PagTestR1 = 2
   begin
   SET @Imponibile = @Fattura - @Spese
   set @ImpRata = round(@Imponibile / @PagNRate,2)
   end
   if @PagTestR1 = 3
   begin
   SET @Imponibile = @Fattura - @Spese
   set @ImpRata = round(@Imponibile / (@PagNRate -1),2)
   set @Nra = 1
   set @vetim = @Spese
   set @Resto = @Spese
   SET @PlusDate =(select PagRata1 from TbPag Where @CodPag = PagCod)
   SET @DayGG = (select PagData1 from TbPag Where @CodPag = PagCod)

   if @PagTestGm = 1 
   begin
   set @vetdt = dateadd(day,@Plusdate,@DataFattura)
   end
   else
   begin
   set @vetdt = dateadd(Month,@Plusdate,@DataFattura)
   end

   if @PagTestS1 = 2 
   begin
   set @current = @vetdt
   set @dada ='01/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
   set @current = CONVERT(VARCHAR(20),@Dada,103)
   set @current = dateadd(Month,1,@current)
   set @Vetdt = dateadd(day,-1,@current)
   end

   if @DayGG > 0 
   begin
   set @current = @vetdt
   set @dada =cast(@Daygg as varchar(2))+'/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
   set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)
   end

   set @current = @vetdt

   if datepart(month,@Current) = @PagMeseE1
   begin
   set @vetdt = @PagGgmmE1
   IF @PagSlitE1 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
    end

    if datepart(month,@Current) = @PagMeseE2
    begin
    set @vetdt = @PagGgmmE2
    IF @PagSlitE2 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
    end
	INSERT INTO #TMP (nr,rata,scadenza)
    SELECT @NRA ,@VETIM ,@VETDT 
 end
   	  

Loop_Rate:
begin
set @Nra = @Nra +1
set @vetim = @ImpRata
if @nra = 1 set @vetim = @vetim + @spese
set @Resto = @resto + @vetim
--Set @DbRate = 'PagRata'+cast(@Nra as varchar(2))
--Set @DbDate = 'PagData'+cast(@Nra as varchar(2))
if @nra = 1 
begin
SET @PlusDate =(select PagRata1 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData1 from TbPag Where @CodPag = PagCod)
end
if @nra = 2
begin 
SET @PlusDate =(select PagRata2 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData2 from TbPag Where @CodPag = PagCod)
end
if @nra = 3 
begin
SET @PlusDate =(select PagRata3 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData3 from TbPag Where @CodPag = PagCod)
end
if @nra = 4 
begin
SET @PlusDate =(select PagRata4 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData4 from TbPag Where @CodPag = PagCod)
end
if @nra = 5 
begin
SET @PlusDate =(select PagRata5 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData5 from TbPag Where @CodPag = PagCod)
end
if @nra = 6 
begin
SET @PlusDate =(select PagRata6 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData6 from TbPag Where @CodPag = PagCod)
end
if @nra = 7 
begin
SET @PlusDate =(select PagRata7 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData7 from TbPag Where @CodPag = PagCod)
end
if @nra = 8 
begin
SET @PlusDate =(select PagRata8 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData8 from TbPag Where @CodPag = PagCod)
end
if @nra = 9 
begin
SET @PlusDate =(select PagRata9 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData9 from TbPag Where @CodPag = PagCod)
end
if @nra = 10 
begin
SET @PlusDate =(select PagRata10 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData10 from TbPag Where @CodPag = PagCod)
end
if @nra = 11 
begin
SET @PlusDate =(select PagRata11 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData11 from TbPag Where @CodPag = PagCod)
end
if @nra = 12 
begin
SET @PlusDate =(select PagRata12 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData12 from TbPag Where @CodPag = PagCod)
end
if @nra = 13 
begin
SET @PlusDate =(select PagRata13 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData13 from TbPag Where @CodPag = PagCod)
end
if @nra = 14 
begin
SET @PlusDate =(select PagRata14 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData14 from TbPag Where @CodPag = PagCod)
end
if @nra = 15 
begin
SET @PlusDate =(select PagRata15 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData15 from TbPag Where @CodPag = PagCod)
end
if @nra = 16 
begin
SET @PlusDate =(select PagRata16 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData16 from TbPag Where @CodPag = PagCod)
end
if @nra = 17 
begin
SET @PlusDate =(select PagRata17 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData17 from TbPag Where @CodPag = PagCod)
end
if @nra = 18 
begin
SET @PlusDate =(select PagRata18 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData18 from TbPag Where @CodPag = PagCod)
end
if @nra = 19 
begin
SET @PlusDate =(select PagRata19 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData19 from TbPag Where @CodPag = PagCod)
end
if @nra = 20 
begin
SET @PlusDate =(select PagRata20 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData20 from TbPag Where @CodPag = PagCod)
end
if @nra = 21 
begin
SET @PlusDate =(select PagRata21 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData21 from TbPag Where @CodPag = PagCod)
end
if @nra = 22 
begin
SET @PlusDate =(select PagRata22 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData22 from TbPag Where @CodPag = PagCod)
end
if @nra = 23 
begin
SET @PlusDate =(select PagRata23 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData23 from TbPag Where @CodPag = PagCod)
end
if @nra = 24 
begin
SET @PlusDate =(select PagRata24 from TbPag Where @CodPag = PagCod)
SET @DayGG = (select PagData24 from TbPag Where @CodPag = PagCod)
end
set @PlusDate = @plusDate + @molti
if @PagTestGm = 1 
begin
set @vetdt = dateadd(day,@Plusdate,@DataFattura)
end
else
begin
set @vetdt = dateadd(Month,@Plusdate,@DataFattura)
end
if @PagTestS1 = 2 
begin
set @current = @vetdt
set @dada ='01/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @current = CONVERT(VARCHAR(20),@Dada,103)
set @current = dateadd(Month,1,@current)
set @Vetdt = dateadd(day,-1,@current)
end
if @DayGG > 0 
begin
set @current = @vetdt
set @dada =cast(@Daygg as varchar(2))+'/'+cast(datepart(month,@current) as varchar(2))+'/'+cast(datepart(year,@current) as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)
end

set @current = @vetdt
SET @PA=0
if datepart(month,@Current) = @PagMeseE1
begin
if @PagMeseE1 = 12 
begin
SET @PA = 1
end

set @dada =cast(datepart(DAY,@PagGgmmE1) as varchar(2))+'/'+cast(datepart(month,@PagGgmmE1) as varchar(2))+'/'+cast(datepart(year ,@current) + @PA as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)

--set @vetdt = @PagGgmmE1
  IF @PagSlitE1 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end

if datepart(month,@Current) = @PagMeseE2
begin
if @PagMeseE2 = 12 
begin
SET @PA = 1
end

--set @vetdt = @PagGgmmE2

set @dada =cast(datepart(DAY,@PagGgmmE2) as varchar(2))+'/'+cast(datepart(month,@PagGgmmE2) as varchar(2))+'/'+cast(datepart(year,@current) + @PA as varchar(4))
set @Vetdt = CONVERT(VARCHAR(20),@Dada,103)

  IF @PagSlitE2 = '*' 
    begin 
    set @molti = @molti + @Plusdate / @NRA
    end
end
if @nra = @PagNRate set @vetim = @vetim + @Fattura - @Resto 
--SET @CMD = 'RATA N.'+CAst(@nra as varchar(2)) +' IMPORTO: '+cast(@Vetim as varchar(15))+ ' SCADE AL '+ CONVERT(VARCHAR(20),@VetDt,103) +' IMPORTO FATTURA: '+cast(@FATTURA as varchar(15))+ ' RESTO RATA: ' +cast(@RESTO as varchar(15))
--print @CMD
INSERT INTO #TMP (nr,rata,scadenza)
SELECT @NRA ,@VETIM ,@VETDT 
end
IF @Nra < @PagNRate GOTO Loop_Rate

SET @P = 0
SET @NR = (SELECT COUNT(*) FROM #tmp)
IF @NR = 0 GOTO FINE_LOOP
lOOP_SCADE:
SET @P = @P + 1
SET @RRATA = (SELECT RATA FROM #TMP WHERE NR = @P)
SET @SSCAD = (SELECT SCADENZA FROM #TMP WHERE NR = @P)

INSERT INTO [dbo].[TbSca] ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
	                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato])
SELECT                     [ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],@RRATA,[ScaTPag],[ScaCodPag],
	                   @P,[ScaBan],[ScaDdoc],@SSCAD,[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]
FROM [dbo].[#TSca] WHERE SCAID = @ID
IF @P < @NR GOTO LOOP_SCADE
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @id,@fatt,@IvaSp,@datafatt,@cpag,@pid
END
CLOSE SERGENTE
DEALLOCATE SERGENTE

DROP TABLE [dbo].[#TSca]
DROP TABLE [dbo].[#TMP]

USCITA:

GO
/****** Object:  StoredProcedure [dbo].[XCREASCPIANORIENTRO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[XCREASCPIANORIENTRO] @GRUPPO AS INT
AS
--SET @GRUPPO=10
delete TMPSCAPIANO WHERE IDGRUPPO=@GRUPPO


INSERT INTO [dbo].[TMPSCAPIANO]([IDGRUPPO],[IDRATA],[IDDATAFAT],[IDNRFAT],[IDRATACLI],[IDIMPORTORATA],[IDIMPORTOFAT],[IDRESIDUOCLI],[IDCCLIE],[DATASCADENZA],[RATACONCORDATA],[DATACREAZIONE],
[DATACREASCADENZE],[IDANAGRAFICA],[IDSEQUENZA], 
[ScaId],[ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato])

SELECT [IDGRUPPO],[IDRATA],[IDDATAFAT],[IDNRFAT],[IDRATACLI],[IDIMPORTORATA],[IDIMPORTOFAT],[IDRESIDUOCLI],[IDCCLIE],[DATASCADENZA],[RATACONCORDATA],[DATACREAZIONE],
[DATACREASCADENZE],[IDANAGRAFICA],[IDSEQUENZA], 
[ScaId],[ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato] FROM TMPPIANO 
INNER JOIN TBSCA ON SCACONTO= IDCCLIE AND ScaDdoc=IDDATAFAT AND ScaNdoc=IDNRFAT AND ScaImpDoc=IDIMPORTOFAT 
WHERE IDGRUPPO=@GRUPPO AND ScaImpPagato=0 AND ScaImpDoc > 0
ORDER BY IDRATA,IDSEQUENZA


SELECT DISTINCT ScaId INTO #TMP FROM [TMPSCAPIANO] where IDGRUPPO=@GRUPPO

INSERT INTO TbScaDUP ([ScaId],[ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]) 
SELECT [ScaId],[ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato] from TbSca where ScaId in (Select ScaId from #TMP)

DELETE FROM TbSca where ScaId in (Select ScaId from #TMP)


INSERT INTO TBSCA ([ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],[ScaImpRata],[ScaTPag],[ScaCodPag],
                   [ScaNRata],[ScaBan],[ScaDdoc],[ScaDsca],[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato]) 

select [ScaTipoCo],[ScaConto],[ScaNdoc],[ScaImpDoc],[ScaIvaSpe],[ScaAbi],[ScaCab],IDIMPORTORATA,[ScaTPag],[ScaCodPag],
                   IDRATACLI,[ScaBan],[ScaDdoc],DATASCADENZA,[ScaRifId],[ScaRifProg],[ScaRifDA],[ScaRAperta],[ScaImpPagato] from TMPSCAPIANO WHERE IDGRUPPO=@GRUPPO ORDER BY IDRATA,IDSEQUENZA

update TMPPIANO set DATACREASCADENZE=getdate() FROM TMPPIANO where IDGRUPPO=@GRUPPO

update TMPSCAPIANO set DATACREASCADENZE=getdate() FROM TMPSCAPIANO where IDGRUPPO=@GRUPPO





GO
/****** Object:  StoredProcedure [dbo].[XDettIva]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE procedure [dbo].[XDettIva] @ANNO AS smallint
as
select PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
CLoFO = case when pricausale = 2 then 'FO' else 'CL' end,
Codice = case when pricausale = 2 then Pricoavere else  PricoDare end,
PIVA = CASE WHEN pricausale = 1 then 
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL' ),'* ERRATO *')
else
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),'* ERRATO *')
END,
PriImpdare,PriCodIva,
CIITP=isnull((select CiiTp from TbCii where CiiCod = PriCodIva),''),
IVA=CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
PriRegIva,PriId,PriProg
into #tmp
 from Tbpri where 
PRIREGIVA >0 
AND datepart(year,pridatagio) = @ANNO
AND pricausale < 3 
ORDER BY PRIREGIVA,PRINUMPROT,PRIBISRET,PRIID,PRIPROG DESC
---
DECLARE @P AS SMALLINT
II:
select priid as IID,priprog as PP , ciitp as TP INTO #TMP2 from #tmp  WHERE ciitp = 0 order by PRIID, PRIPROG desc
set @P=(SELECT COUNT(*) FROM #TMP2)
if @P = 0 goto III
UPDATE #TMP2 SET TP = ISNULL((select CIITP FROM #TMP WHERE IID= PRIID AND PRIPROG = PP + 1 and CIITP > 0),0)
UPDATE #TMP SET ciitp = ISNULL((select TP FROM #TMP2 WHERE IID= PRIID AND PRIPROG = PP AND TP > 0),0) WHERE ciitp = 0
drop table #tmp2
GOTO II
III:
drop table #tmp2
---

select DISTINCT CLoFO,PIVA,Codice,
IMPONIBILE = CASE WHEN CIITP = 1 THEN SUM(PRIIMPDARE) ELSE 0 END,
IVA = CASE WHEN CIITP = 1 THEN SUM(IVA) ELSE 0 END,
NONIMP =  CASE WHEN CIITP = 2 THEN SUM(PRIIMPDARE) ELSE 0 END,
ESENTI= CASE WHEN CIITP = 3 THEN SUM(PRIIMPDARE) ELSE 0 END,
ALTRI = CASE WHEN CIITP > 3 THEN SUM(PRIIMPDARE) ELSE 0 END
	into #TMP3
FROM #TMP where CIITP BETWEEN 1 AND 9 ----AND ISNUMERIC(PIVA) = 1
GROUP BY CLoFO,Codice,Piva,CIITP

SELECT DISTINCT CLoFO,Codice,PIVA,
IMPONIBILE=SUM(IMPONIBILE),
IVA=SUM(IVA),
NONIMP=SUM(NONIMP),
ESENTI=SUM(ESENTI),
ALTRI=SUM(ALTRI)
INTO #TMP4
 FROM #TMP3 GROUP BY CLoFO,Codice,PIVA
ORDER BY CLoFO,Codice,PIVA

SELECT CLoFO,Codice,PIVA,IMPONIBILE,IVA,NONIMP,ESENTI,ALTRI,ANAG = (SELECT TOP 1 ANADESC FROM VDOX.DBO.TBANA WHERE Codice = anacod AND ANAGRP = CLoFO)
INTO #TMP5
FROM #TMP4

SELECT CLoFO,Codice,PIVA,IMPONIBILE,IVA,NONIMP,ESENTI,ALTRI,ANAG,TT1= (IMPONIBILE + IVA + NONIMP + ESENTI + ALTRI) FROM #TMP5 
where ((IMPONIBILE <> 0 ) OR 
           (NONIMP <> 0 ) OR 
           (ESENTI <> 0 ) OR 
           (ALTRI  <> 0 ) OR
              (IVA <> 0 ))
ORDER BY CLoFO, ANAG


drop table #tmp
drop table #tmp3
drop table #tmp4
drop table #tmp5

GO
/****** Object:  StoredProcedure [dbo].[XDETTQUO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[XDETTQUO] @BLOCK as int,@MESI as varchar(12)
as
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT @BLOCK,PiaCodCo,PiaAnaCo,PiaFl01,isnull(sum(PROQUOAMMORTAM),0),0,isnull(sum(PROQUOAMMORTAM),0),0,0,0,SUBSTRING(PiaCodCo,1,2),60,'*' FROM tmpproquo
inner join tbpia on procontoquo = PiaCodCo
where procontoquo <> '00.00' 
group by PiaCodCo,PiaAnaCo,PiaFl01
having isnull(sum(PROQUOAMMORTAM),0) > 0
UPDATE TMPBILS SET TMSDEMAS = (select ISNULL(PIAANACO+@MESI,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSMASTRO+'.00' ) WHERE TMSBLOCK = @BLOCK and TMSDEMAS = '*'

INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT @BLOCK,PiaCodCo,PiaAnaCo,PiaFl01,0,isnull(sum(PROQUOAMMORTAM),0),isnull(sum(PROQUOAMMORTAM)*-1,0),0,0,0,SUBSTRING(PiaCodCo,1,2),0,'§' FROM tmpproquo
inner join tbpia on procontoFondo = PiaCodCo
where procontoFondo <> '00.00' 
group by PiaCodCo,PiaAnaCo,PiaFl01
having isnull(sum(PROQUOAMMORTAM),0) > 0
UPDATE TMPBILS SET TMSDEMAS = (select ISNULL(PIAANACO,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSMASTRO+'.00' ) WHERE TMSBLOCK = @BLOCK and TMSDEMAS = '§'


GO
/****** Object:  StoredProcedure [dbo].[XDIFFERENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[XDIFFERENZE]
 as

if NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMPDIFF]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[TMPDIFF] (
	[TMPCONTO] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[TMPANAG] [varchar] (60) COLLATE Latin1_General_CI_AS NOT NULL ,
	[TMPDIFFC] [decimal](10, 2) NOT NULL ,
	[TMPDIFFP] [decimal](10, 2) NOT NULL ,
	[TMPDIFFS] [decimal](10, 2) NOT NULL 
) ON [PRIMARY]
END

DELETE FROM TMPDIFF
----- saldo conti
SELECT DISTINCT PRKCONTO,PRKDESC,ABS((SUM(DARE)-SUM(AVERE))) AS DIFFC,0 AS DIFFP,0 AS DIFFS
into #tmp
 FROM VH8 WHERE PRKTIPOCO = 1 AND prkaammgg >=isnull((SELECT MAX(PRIDATAEST) FROM TBPRI WHERE PRICAUSALE = 45),(SELECT min(PRIDATAEST) FROM TBPRI))
GROUP BY PRKCONTO,PRKDESC
HAVING (SUM(DARE)-SUM(AVERE)) <> 0
union all
---- partite Aperte
SELECT DISTINCT PRKCONTO,PRKDESC,0 AS DIFFC,ABS((SUM(DARE)-SUM(AVERE))) AS DIFFP,0 AS DIFFS
 FROM VB8 WHERE PRKTIPOCO = 1 AND PRKPAPERTA = 0 ---- AND PRIDATAEST >=(SELECT MAX(PRIDATAEST) FROM TBPRI WHERE PRICAUSALE = 45)
GROUP BY PRKCONTO,PRKDESC
union all
--- SCADENZIARIO
SELECT DISTINCT SCACONTO AS PRKCONTO ,
PRKDESC =(SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = SCACONTO AND ANAGRP = 'CL' OR ANACOD = SCACONTO AND ANAGRP = 'FO'),
0 AS DIFFC,0 AS DIFFP,abs(SUM(scaimprata) - sum(scaimppagato)) AS DIFFS
 FROM CRG1 WHERE PRKPAPERTA = 0
GROUP BY SCACONTO

INSERT INTO TMPDIFF(TMPCONTO,TMPANAG,TMPDIFFC,TMPDIFFP,TMPDIFFS)
select DISTINCT PRKCONTO,PRKDESC,DIFFC=SUM(DIFFC),DIFFP=SUM(DIFFP),DIFFs=SUM(DIFFs) from #tmp 
GROUP BY PRKCONTO,PRKDESC 
HAVING (SUM(DIFFC)<>SUM(DIFFP)) OR (SUM(DIFFP)<>SUM(DIFFS)) OR (SUM(DIFFC)<>SUM(DIFFS))
ORDER BY PRKCONTO,PRKDESC 

DROP TABLE #TMP

GO
/****** Object:  StoredProcedure [dbo].[XDIFFIVAPS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[XDIFFIVAPS] @ANNO AS smallint
AS
---- pridatagio 2005 pridataest <> 2005
select PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,YEAR(PRIDATAEST) AS ANNOIVA,
CLoFO = case when pricausale = 2 then 'FO' else 'CL' end,
Codice = case when pricausale = 2 then Pricoavere else  PricoDare end,
PIVA = CASE WHEN pricausale = 1 then 
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL' ),'* ERRATO *')
else
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),'* ERRATO *')
END,
PriImpdare,PriCodIva,
CIITP=isnull((select CiiTp from TbCii where CiiCod = PriCodIva),''),
IVA=CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
PriRegIva,PriId,PriProg
into #tmp
 from Tbpri where 
PRIREGIVA >0 
AND datepart(year,pridatagio) = @ANNO and datepart(year,pridataest) <> @ANNO
AND pricausale < 3 
ORDER BY PRIREGIVA,PRINUMPROT,PRIBISRET,PRIID,PRIPROG DESC
---
DECLARE @P AS SMALLINT
II:
select priid as IID,priprog as PP , ciitp as TP INTO #TMP2 from #tmp  WHERE ciitp = 0 order by PRIID, PRIPROG desc
set @P=(SELECT COUNT(*) FROM #TMP2)
if @P = 0 goto III
UPDATE #TMP2 SET TP = ISNULL((select CIITP FROM #TMP WHERE IID= PRIID AND PRIPROG = PP + 1 and CIITP > 0),0)
UPDATE #TMP SET ciitp = ISNULL((select TP FROM #TMP2 WHERE IID= PRIID AND PRIPROG = PP AND TP > 0),0) WHERE ciitp = 0
drop table #tmp2
GOTO II
III:
drop table #tmp2
---

---- pridatagio<> 2005 pridataest=2005

select PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,YEAR(PRIDATAEST) AS ANNOIVA,
CLoFO = case when pricausale = 2 then 'FO' else 'CL' end,
Codice = case when pricausale = 2 then Pricoavere else  PricoDare end,
PIVA = CASE WHEN pricausale = 1 then 
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL' ),'* ERRATO *')
else
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),'* ERRATO *')
END,
PriImpdare,PriCodIva,
CIITP=isnull((select CiiTp from TbCii where CiiCod = PriCodIva),''),
IVA=CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
PriRegIva,PriId,PriProg
into #tmpPP
 from Tbpri where 
PRIREGIVA >0 
AND datepart(year,pridatagio) <> @ANNO and datepart(year,pridataest) = @ANNO
AND pricausale < 3 
ORDER BY PRIREGIVA,PRINUMPROT,PRIBISRET,PRIID,PRIPROG DESC
---

XIII:
select priid as IID,priprog as PP , ciitp as TP INTO #TMP22 from #tmpPP  WHERE ciitp = 0 order by PRIID, PRIPROG desc
set @P=(SELECT COUNT(*) FROM #TMP22)
if @P = 0 goto IIII
UPDATE #TMP22 SET TP = ISNULL((select CIITP FROM #tmpPP WHERE IID= PRIID AND PRIPROG = PP + 1 and CIITP > 0),0)
UPDATE #tmpPP SET ciitp = ISNULL((select TP FROM #TMP22 WHERE IID= PRIID AND PRIPROG = PP AND TP > 0),0) WHERE ciitp = 0
drop table #tmp22
GOTO XIII
IIII:
drop table #tmp22
---

SELECT *,ANAG = (SELECT TOP 1 ANADESC FROM VDOX.DBO.TBANA WHERE Codice = anacod AND ANAGRP = CLoFO) FROM #TMP 
UNION 
SELECT *,ANAG = (SELECT TOP 1 ANADESC FROM VDOX.DBO.TBANA WHERE Codice = anacod AND ANAGRP = CLoFO) FROM #TMPPP 
ORDER BY CODICE
GO
/****** Object:  StoredProcedure [dbo].[XELE2012]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[XELE2012] @ANNO AS SMALLINT,@T AS SMALLINT
AS
select RieCfAnno,RieCfTipo,RieCfCodice,RieCfAnadesc,RieCfPiva,RieCfCFis,TotaleOp=SUM(RieCfImponibile)+ SUM(RieCfNonImp) +SUM(RieCfEsente),
TotaleIva=SUM(RieCfIva),NFatt=SUM(RieCfNFatture),RieCfNCredito,DocRie=CAST(0 as bit) into #RIEP from TbRieCf 
where RieCfEscludi = 0 AND RieCfCFis<>'R' and RieCfAnno=@ANNO
group by RieCfAnno,RieCfTipo,RieCfCodice,RieCfAnadesc,RieCfPiva,RieCfCFis,RieCfNCredito


select RieCfAnno,RieCfTipo,RieCfCodice='00000',RieCfAnadesc='DOCUMENTO RIEPILOGATIVO',RieCfPiva,RieCfCFis,TotaleOp=SUM(RieCfImponibile)+ SUM(RieCfNonImp) +SUM(RieCfEsente),
TotaleIva=SUM(RieCfIva),NFatt=SUM(RieCfNFatture),RieCfNCredito,DocRie=Cast(1 as bit) into #DOCR from TbRieCf 
where RieCfEscludi = 0 AND RieCfPiva='' AND RieCfCFis='R' and RieCfAnno=@ANNO
group by RieCfAnno,RieCfTipo,RieCfPiva,RieCfCFis,RieCfNCredito


select * into #TT from #RIEP
union
select * from #DOCR

IF @T = 0 
BEGIN
SELECT * FROM #TT 
order by RieCfPiva,RieCfCFis,RieCfTipo,RieCfNCredito
GOTO USCITA
END


-- CALCOLO MODELLO IVA
DECLARE @TIPO AS VARCHAR(2),@CODICE AS VARCHAR(5),@NOTAC AS BIT,@DOCRIE AS BIT,@NN AS INT,@TOTOP as decimal(12,2),@TOTIVA as decimal(12,2)
DECLARE @PI AS VARCHAR(11),@CF AS VARCHAR(16),@DR AS VARCHAR(1),@NOA AS INT,@NOP AS INT,@NL AS VARCHAR(1)
DECLARE @T7 AS DECIMAL(12,2),@T8 AS DECIMAL(12,2),@T9 AS DECIMAL(12,2),@T10 AS DECIMAL(12,2),@T11 AS DECIMAL(12,2)
DECLARE @T12 AS DECIMAL(12,2),@T13 AS DECIMAL(12,2),@T14 AS DECIMAL(12,2),@T15 AS DECIMAL(12,2),@T16 AS DECIMAL(12,2)

DECLARE @P as int,@PL AS VARCHAR(16),@CFL as varchar(16),@DOR as varchar(1),@NPROG AS INT
set @P =0
SET @DOR = '0'
set @NPROG = 0

delete from TbSpeCf where SpeCfAnno = @ANNO

DECLARE DATISCA CURSOR FOR
SELECT RieCfTipo,RieCfCodice,RieCfPiva,RieCfCFis,TotaleOp,ToTaleIva,NFatt,RieCfNcredito,DocRie from #TT 
where RieCfCodice not in(Select DiCfCodice from TbEPoE where DiCfAnno = @ANNO and RieCfTipo = DiCfTipo)
order by DocRie,RieCfPiva,RieCfCFis
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @TIPO,@CODICE,@PI,@CF,@TOTOP,@TOTIVA,@NN,@NOTAC,@DOCRIE
WHILE @@FETCH_STATUS = 0
BEGIN
INIZIOLETTURA:
 if @P = 0
 begin
 SET @PL=@PI
 set @CFL=@CF
 set @DOR =@DOCRIE
 SET @NOA=0
 SET @NOP=0
 SET @T7=0
 SET @T8=0
 SET @T9=0
 SET @T10=0
 SET @T11=0
 SET @T12=0
 SET @T13=0
 SET @T14=0
 SET @T15=0
 SET @T16=0 
 end
 if @PL <> @PI OR @CFL<>@CF GOTO SCRIVI
 
 set @P=@P + 1
  IF @TIPO = 'CL' AND @NOTAC = 0
 BEGIN
SET @T7=@T7 + @TOTOP
SET @T8=@T8 + @TOTIVA
SET @T9=0
SET @NOA = @NOA + @NN
 END
  IF @TIPO = 'CL' AND @NOTAC = 1
  BEGIN
SET @T15=@T15 + ABS(@TOTOP)
SET @T16=@T16 + ABS(@TOTIVA)
SET @NOP = @NOP + @NN
  END
  IF @TIPO = 'FO' AND @NOTAC = 0
 BEGIN
SET @T12=@T12 + @TOTOP
SET @T13=@T13 + @TOTIVA
SET @T14=0
SET @NOP = @NOP + @NN
 END
  IF @TIPO = 'FO' AND @NOTAC = 1
  BEGIN
SET @T10=@T10 + ABS(@TOTOP)
SET @T11=@T11 + ABS(@TOTIVA)
SET @NOA = @NOA + @NN
  END
  GOTO FINE_LOOP
SCRIVI:
set @NPROG = @NPROG +1
INSERT INTO [TbSpeCf]([SpeCfAnno],[SpeCfProg],[SpeCfPiva],[SpeCfCFis],[SpeCfRie],[SpeCfNoa],[SpeCfNop],[SpeCfT7],[SpeCfT8],[SpeCfT9],[SpeCfT10],
	                  [SpeCfT11],[SpeCfT12],[SpeCfT13],[SpeCfT14],[SpeCfT15],[SpeCfT16])
	         SELECT @ANNO,@NPROG,@PL,@CFL,@DOR,@NOA,@NOP,@T7,@T8,@T9,@T10,@T11,@T12,@T13,@T14,@T15,@T16         
 set @P=0
 goto INIZIOLETTURA
FINE_LOOP:
   FETCH NEXT FROM DATISCA
   INTO @TIPO,@CODICE,@PI,@CF,@TOTOP,@TOTIVA,@NN,@NOTAC,@DOCRIE
END
SET @NPROG = @NPROG +1 
INSERT INTO [TbSpeCf]([SpeCfAnno],[SpeCfProg],[SpeCfPiva],[SpeCfCFis],[SpeCfRie],[SpeCfNoa],[SpeCfNop],[SpeCfT7],[SpeCfT8],[SpeCfT9],[SpeCfT10],
	                  [SpeCfT11],[SpeCfT12],[SpeCfT13],[SpeCfT14],[SpeCfT15],[SpeCfT16])
	         SELECT @ANNO,@NPROG,@PL,@CFL,@DOR,@NOA,@NOP,@T7,@T8,@T9,@T10,@T11,@T12,@T13,@T14,@T15,@T16  
CLOSE DATISCA
DEALLOCATE DATISCA

-- REM TABELLA BL X ESTERI
declare @CODL as varchar(5),@NOTACL as bit
set @P =0
set @NPROG = 0

delete from TbEstCf where EstCfAnno = @ANNO

DECLARE DATISCA CURSOR FOR
SELECT RieCfTipo,RieCfCodice,TotaleOp,ToTaleIva,NFatt,RieCfNcredito from #TT 
where RieCfCodice in(Select DiCfCodice from TbEPoE where DiCfAnno = @ANNO and RieCfTipo = DiCfTipo)
order by RieCfCodice,RieCfNcredito
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @TIPO,@CODICE,@TOTOP,@TOTIVA,@NN,@NOTAC
WHILE @@FETCH_STATUS = 0
BEGIN
INIZIOLETTURABL:
 if @P = 0
 begin
 SET @CODL=@CODICE
 set @NOTACL=@NOTAC
  SET @T7=0
 SET @T8=0
 SET @T9=0
 SET @T10=0
 end
 if @CODL <> @CODICE GOTO SCRIVIBL
 
  set @P=@P + 1
  IF  @NOTAC = 0
 BEGIN
SET @T7=ABS(@TOTOP)
SET @T8=ABS(@TOTIVA)
 END
 
  IF  @NOTAC = 1
  BEGIN
SET @T9=ABS(@TOTOP)
SET @T10=ABS(@TOTIVA)
  END
   GOTO FINE_LOOPBL
SCRIVIBL:
set @NPROG = @NPROG +1
INSERT INTO TbEstCf ([EstCfAnno],[EstCfProg],[EstCfCodice],[EstCfT3],[EstCfT4],[EstCfT6],[EstCfT7])
	         SELECT @ANNO,@NPROG,@CODL,@T7,@T8,@T9,@T10         
 set @P=0
 goto INIZIOLETTURABL
FINE_LOOPBL:
   FETCH NEXT FROM DATISCA
   INTO @TIPO,@CODICE,@TOTOP,@TOTIVA,@NN,@NOTAC
END
IF @P>0
 BEGIN
SET @NPROG = @NPROG +1 
INSERT INTO TbEstCf ([EstCfAnno],[EstCfProg],[EstCfCodice],[EstCfT3],[EstCfT4],[EstCfT6],[EstCfT7])
	         SELECT @ANNO,@NPROG,@CODL,@T7,@T8,@T9,@T10
END
	             
CLOSE DATISCA
DEALLOCATE DATISCA

select * from TbSpeCf where SpeCfAnno = @ANNO

USCITA:

GO
/****** Object:  StoredProcedure [dbo].[XELEDA2010]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[XELEDA2010] @ANNO as smallint
AS

declare @LIM as decimal(12,2)
set @LIM=25000

IF @ANNO = 2011
BEGIN
set @LIM=3000
END



delete from [dbo].[TbEleCf] where EleCfAnno=@ANNO

Insert Into [dbo].[TbEleCf](EleCfAnno,EleCfTipo,EleCfCodice,EleCfAnaDesc,EleCfCpt,EleCfPiaDesc,EleCfDataDoc,EleCfNumDoc,
	EleCfImponibile,EleCfIva,EleCfCodiceIva,EleCfDescIva,EleCfPriCodIva,EleCfPriId,EleCfPriProg,EleCfP,EleCfPriRegIva,EleCfPerc,EleCfTipImp,EleCfRegistrazione)
select @ANNO,'CL',a.PriCoDare,ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.PriCoDare AND ANAGRP = 'CL'),'***ERRATO***'),
a.PriCoAvere,ISNULL((SELECT PIAANACO FROM TbPia WHERE PIACODCO = a.PriCoAvere), '***ERRATO***'),
a.PriDataEst,a.PriDocEst,a.PriImpDare,a.PriImpAvere,
case when a.PriCodIva = 0 then (select top 1 b.PriCodIva from TbPri as b where b.PriId = a.Priid and b.PriProg > a.priprog and b.PriCodIva > 0) else a.pricodiva end ,
'',a.pricodiva,a.Priid,a.PriProg,0,a.PriRegIva,0,0,PriDatagio
from tbpri as a where DatePart(YEAR,a.pridatagio)=@ANNO and a.PriCausale=1 AND a.PriRegIva IN(select RivaNREG from fnFotoriva(@ANNO) where RivaTipo between 1 and 4)
UNION
select @ANNO,'FO',a.PriCoAvere,ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = a.PriCoAvere AND ANAGRP = 'FO'),'***ERRATO***'),
a.PriCoDare,ISNULL((SELECT PIAANACO FROM TbPia WHERE PIACODCO = a.PriCoDare), '***ERRATO***'),
a.PriDataEst,a.PriDocEst,a.PriImpDare,a.PriImpAvere,
case when a.PriCodIva = 0 then (select top 1 b.PriCodIva from TbPri as b where b.PriId = a.Priid and b.PriProg > a.priprog and b.PriCodIva > 0) else a.pricodiva end ,
'',a.pricodiva,a.Priid,a.PriProg,0,a.PriRegIva,0,0,PriDatagio
from tbpri as a where DatePart(YEAR,a.pridatagio)=@ANNO and a.PriCausale=2 AND a.PriRegIva IN(select RivaNREG from fnFotoriva(@ANNO) where RivaTipo between 1 and 4)

DATI:

update [dbo].[TbEleCf] set EleCfPerc=(select ciiali from TbCii where CiiCod = EleCfCodiceIva),
EleCfTipImp =(select CiiTp from TbCii where CiiCod = EleCfCodiceIva),
EleCfDescIva =(select CiiDes from TbCii where CiiCod = EleCfCodiceIva) WHERE EleCfAnno = @ANNO


DELETE FROM [dbo].[TbEleCf] WHERE EleCfTipImp > 3 AND EleCfAnno = @ANNO

SELECT DISTINCT ANACOD INTO #PI FROM VDOX.dbo.TbAna WHERE AnaPivaEst>'' and AnaPivaEst<>'.' 

DELETE FROM [dbo].[TbEleCf] WHERE EleCfCodice IN (SELECT ANACOD FROM #PI) AND EleCfAnno = @ANNO

DELETE FROM [dbo].[TbEleCf] WHERE EleCfCodice IN (SELECT Clcod FROM geve.dbo.tbcli where ClBlackList = 1) AND EleCfAnno = @ANNO
DELETE FROM [dbo].[TbEleCf] WHERE EleCfCodice IN (SELECT Focod FROM geve.dbo.tbfor where FoBlackList = 1) AND EleCfAnno = @ANNO


UPDATE [dbo].[TbEleCf] SET EleCfIva = (EleCfImponibile * EleCfPerc / 100 ) WHERE EleCfAnno = @ANNO


UPDATE [dbo].[TbEleCf] SET EleCfP=1 WHERE EleCfCodice IN(select EleCfCodice from [dbo].[TbEleCf] WHERE EleCfAnno = @ANNO GROUP by EleCfCodice having SUM(EleCfImponibile) >= @LIM) AND EleCfAnno = @ANNO



GO
/****** Object:  StoredProcedure [dbo].[XEleDiE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XEleDiE] @ANNO as smallint
as
SELECT distinct EleCfAnno,EleCfTipo,EleCfCodice,EleCfAnaDesc,DiCognome,DiNome,DiDataNasc,DiComune,DiProv,DiStato,DiDenomina,DiECitta,DiEStato,DiEIndiri
into #tmp from TbEleCf 
inner join VDOX.dbo.TbAna on AnaCod = EleCfCodice and AnaGrp = EleCfTipo
LEFT OUTER JOIN TbEPoE ON DiCfTipo = EleCfTipo and DiCfCodice=EleCfCodice and DiCfAnno = EleCfAnno
where EleCfP > 1 and EleCfAnno = @ANNO and AnaPiva='' and AnaCfis='' and (AnaPivaEst='' or  AnaPivaEst='.')

DELETE FROM TbEPoe WHERE DiCfCodice NOT IN (select EleCfCodice from #TMP where EleCfanno = DiCfAnno and EleCfTipo= DiCfTipo) AND DiCfAnno = @ANNO

INSERT INTO TbEPoE(DiCfAnno,DiCfTipo,DiCfCodice)
select EleCfAnno,EleCfTipo,EleCfCodice from #tmp where EleCfCodice not in( select DiCfCodice from TbEPoe where EleCfanno = DiCfAnno and EleCfTipo= DiCfTipo)

select * from #tmp Order By EleCfTipo,EleCfAnaDesc

GO
/****** Object:  StoredProcedure [dbo].[XEleDiE2012]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[XEleDiE2012] @ANNO as smallint
as
SELECT distinct RieCfAnno,RieCfTipo,RieCfCodice,RieCfAnaDesc,DiCognome,DiNome,DiDataNasc,DiComune,DiProv,DiStato,DiDenomina,DiECitta,DiEStato,DiEIndiri
into #tmp from TbRieCf 
inner join VDOX.dbo.TbAna on AnaCod = RieCfCodice and AnaGrp = RieCfTipo
LEFT OUTER JOIN TbEPoE ON DiCfTipo = RieCfTipo and DiCfCodice=RieCfCodice and DiCfAnno = RieCfAnno
where RieCfEscludi = 0 and RieCfAnno = @ANNO and AnaPiva='' and AnaCfis='' and (AnaPivaEst='' or  AnaPivaEst='.')

DELETE FROM TbEPoe WHERE DiCfCodice NOT IN (select RieCfCodice from #TMP where RieCfanno = DiCfAnno and RieCfTipo= DiCfTipo) AND DiCfAnno = @ANNO

INSERT INTO TbEPoE(DiCfAnno,DiCfTipo,DiCfCodice)
select RieCfAnno,RieCfTipo,RieCfCodice from #tmp where RieCfCodice not in( select DiCfCodice from TbEPoe where RieCfanno = DiCfAnno and RieCfTipo= DiCfTipo)

select * from #tmp Order By RieCfTipo,RieCfAnaDesc

GO
/****** Object:  StoredProcedure [dbo].[XELEFILE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[XELEFILE]  @T AS SMALLINT,@ANNO as smallint
AS
IF @T = 1 
BEGIN
select EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaCfis,Totale= SUM(EleCfImponibile) +  sum(EleCfIva), EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione 
INTO #TMP from tbelecf
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
where EleCfP > 1 and EleCfAnno= @ANNO and AnaCfis>'' and AnaPiva='' AND EleCfVariazione = 0 AND EleCfUnion = 0
group by  EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc, AnaCfis, EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione

select EleCfPriId=MIN(EleCfPriId),EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaCfis,Totale= SUM(EleCfImponibile) +  sum(EleCfIva), EleCfUnion,EleCfRegistrazione=MIN(EleCfRegistrazione),EleCfModPag,EleCfVariazione 
INTO #TMP2 from tbelecf
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
where EleCfP > 1 and EleCfAnno= @ANNO and AnaCfis>'' and AnaPiva='' AND EleCfVariazione = 0 AND EleCfUnion > 0
group by  EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaCfis, EleCfUnion,EleCfModPag,EleCfVariazione

SELECT EleCfRegistrazione,AnaCfis,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END ,Totale=abs(CAST(totale as int)) FROM #TMP
UNION
SELECT EleCfRegistrazione,AnaCfis,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END,Totale=abs(CAST(totale as int)) FROM #TMP2
GOTO USCITA
END 
IF @T = 2 
BEGIN
select EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaPiva,EleCfNumDoc,EleCfImponibile= SUM(EleCfImponibile) , EleCfIva= sum(EleCfIva), EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione 
INTO #TMP3 from tbelecf
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
where EleCfP > 1 and EleCfAnno= @ANNO and AnaPiva>'' AND EleCfVariazione = 0 AND EleCfUnion = 0
group by  EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc, AnaPiva,EleCfNumDoc,EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione

select EleCfPriId=MIN(EleCfPriId),EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaPiva,EleCfNumDoc=MIN(EleCfNumDoc),EleCfImponibile= SUM(EleCfImponibile) , EleCfIva= sum(EleCfIva), EleCfUnion,EleCfRegistrazione=MIN(EleCfRegistrazione),EleCfModPag,EleCfVariazione 
 INTO #TMP4 from tbelecf
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
where EleCfP > 1 and EleCfAnno= @ANNO and AnaPiva>'' AND EleCfVariazione = 0 AND EleCfUnion > 0
group by  EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaPiva,EleCfUnion,EleCfModPag,EleCfVariazione

SELECT EleCfRegistrazione,AnaPiva,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END,ElecfNumDoc,EleCfImponibile=abs(CAST(EleCfImponibile as int)), EleCfIva=abs(cast(EleCfIva as int)), Op=case when EleCfTipo = 'FO' then 2 else 1 end FROM #TMP3
UNION
SELECT EleCfRegistrazione,AnaPiva,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END,ElecfNumDoc,EleCfImponibile=abs(CAST(EleCfImponibile as int)), EleCfIva=abs(cast(EleCfIva as int)), Op=case when EleCfTipo = 'FO' then 2 else 1 end FROM #TMP4
GOTO USCITA
END 
if @T = 3
BEGIN
SELECT * INTO #ESTERI FROM TbEPoe WHERE DiCfAnno =@ANNO
select EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,EleCfNumDoc,EleCfImponibile= SUM(EleCfImponibile) , EleCfIva= sum(EleCfIva), EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione 
INTO #TMP5 from tbelecf
INNER JOIN #ESTERI ON DiCfTipo = EleCfTipo and EleCfCodice = DiCfCodice 
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
where EleCfP > 1 and EleCfAnno= @ANNO AND EleCfVariazione = 0 AND EleCfUnion = 0
group by  EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,EleCfNumDoc,EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione

select EleCfPriId=MIN(EleCfPriId),EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,EleCfNumDoc=MIN(EleCfNumDoc),EleCfImponibile= SUM(EleCfImponibile) , EleCfIva= sum(EleCfIva), EleCfUnion,EleCfRegistrazione=MIN(EleCfRegistrazione),EleCfModPag,EleCfVariazione 
 INTO #TMP6 from tbelecf
 INNER JOIN #ESTERI ON DiCfTipo = EleCfTipo and EleCfCodice = DiCfCodice 
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
where EleCfP > 1 and EleCfAnno= @ANNO  AND EleCfVariazione = 0 AND EleCfUnion > 0
group by  EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,EleCfUnion,EleCfModPag,EleCfVariazione

SELECT EleCfRegistrazione,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END,ElecfNumDoc,EleCfImponibile=abs(CAST(EleCfImponibile as int)), EleCfIva=abs(cast(EleCfIva as int)), Op=case when EleCfTipo = 'FO' then 2 else 1 end,
 	DiCognome=isnull(DiCognome,''),DiNome=isnull(DiNome,''),DiDataNasc=convert(varchar(10),DiDataNasc,103),DiComune=isnull(DiComune,''),DiProv=isnull(DiProv,''),DiStato=isnull(DiStato,''),DiDenomina=isnull(DiDenomina,''),
	DiECitta=isnull(DiECitta,''),DiEStato=isnull(DiEStato,''),DiEIndiri=isnull(DiEIndiri,'')
 FROM #TMP5 INNER JOIN #ESTERI ON DiCfTipo = EleCfTipo and EleCfCodice = DiCfCodice 
 
UNION
SELECT EleCfRegistrazione,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END,ElecfNumDoc,EleCfImponibile=abs(CAST(EleCfImponibile as int)), EleCfIva=abs(cast(EleCfIva as int)), Op=case when EleCfTipo = 'FO' then 2 else 1 end,
	DiCognome=isnull(DiCognome,''),DiNome=isnull(DiNome,''),DiDataNasc=convert(varchar(10),DiDataNasc,103),DiComune=isnull(DiComune,''),DiProv=isnull(DiProv,''),DiStato=isnull(DiStato,''),DiDenomina=isnull(DiDenomina,''),
	DiECitta=isnull(DiECitta,''),DiEStato=isnull(DiEStato,''),DiEIndiri=isnull(DiEIndiri,'')
 FROM #TMP6 INNER JOIN #ESTERI ON DiCfTipo = EleCfTipo and EleCfCodice = DiCfCodice 
GOTO USCITA
END 
if @T=4
BEGIN
select EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaCfis,AnaPiva,EleCfNumDoc,EleCfImponibile= SUM(EleCfImponibile) , EleCfIva= sum(EleCfIva), EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione,
VaCfDataDoc,VaCfNumDoc,VaCfImponibile,VaCfIva 
INTO #TMP8 from tbelecf
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
inner join TbElVar on EleCfPriId = VaCfPriId 
where EleCfP > 1 and EleCfAnno= @ANNO AND EleCfVariazione > 0 AND EleCfUnion = 0 and (anacfis >'' or AnaPiva >'')
group by  EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,AnaCfis,AnaPiva,EleCfNumDoc,EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione,VaCfDataDoc,VaCfNumDoc,VaCfImponibile,VaCfIva
SELECT EleCfRegistrazione,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END,ElecfNumDoc,EleCfImponibile=abs(CAST(EleCfImponibile as int)), EleCfIva=abs(cast(EleCfIva as int)), Op=case when EleCfTipo = 'FO' then 2 else 1 end,
       AnaCfis= case when AnaPiva >'' then '' else AnaCfis end,AnaPiva,VaCfDataDoc,VaCfNumDoc,VaCfImponibile,VaCfIva
 	 FROM #TMP8 
 GOTO USCITA
END 
if @T = 5
BEGIN
SELECT * INTO #ESTERV FROM TbEPoe WHERE DiCfAnno =@ANNO
select EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,EleCfNumDoc,EleCfImponibile= SUM(EleCfImponibile) , EleCfIva= sum(EleCfIva), 
EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione,VaCfDataDoc,VaCfNumDoc,VaCfImponibile,VaCfIva
INTO #TMP7 from tbelecf
INNER JOIN #ESTERV ON DiCfTipo = EleCfTipo and EleCfCodice = DiCfCodice 
inner join VDOX.dbo.TbAna on AnaGrp = EleCfTipo and EleCfCodice = anacod
inner join TbElVar on EleCfPriId = VaCfPriId 
where EleCfP > 1 and EleCfAnno= @ANNO AND EleCfVariazione > 0 AND EleCfUnion = 0
group by  EleCfPriId,EleCfAnno,EleCfTipo,EleCfP,EleCfCodice,EleCfAnaDesc,EleCfNumDoc,EleCfUnion,EleCfRegistrazione,EleCfModPag,EleCfVariazione,VaCfDataDoc,VaCfNumDoc,VaCfImponibile,VaCfIva


SELECT EleCfRegistrazione,EleCfModPag = case when EleCfModPag ='C' THEN 3 WHEN EleCfModPag ='F' THEN 2 ELSE 1 END,ElecfNumDoc,EleCfImponibile=abs(CAST(EleCfImponibile as int)), EleCfIva=abs(cast(EleCfIva as int)), Op=case when EleCfTipo = 'FO' then 2 else 1 end,
 	DiCognome=isnull(DiCognome,''),DiNome=isnull(DiNome,''),DiDataNasc=convert(varchar(10),DiDataNasc,103),DiComune=isnull(DiComune,''),DiProv=isnull(DiProv,''),DiStato=isnull(DiStato,''),DiDenomina=isnull(DiDenomina,''),
	DiECitta=isnull(DiECitta,''),DiEStato=isnull(DiEStato,''),DiEIndiri=isnull(DiEIndiri,''),
	VaCfDataDoc,VaCfNumDoc,VaCfImponibile,VaCfIva
 FROM #TMP7 INNER JOIN #ESTERV ON DiCfTipo = EleCfTipo and EleCfCodice = DiCfCodice 
 
 GOTO USCITA
END 
USCITA:
GO
/****** Object:  StoredProcedure [dbo].[XELENCHI]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[XELENCHI] @ANNO AS smallint
as
select PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
CLoFO = case when pricausale = 2 then 'FO' else 'CL' end,
PIVA = CASE WHEN pricausale = 1 then 
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL' ),'* ERRATO *')
else
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),'* ERRATO *')
END,
PriImpdare,PriCodIva,
CIITP=isnull((select CiiTp from TbCii where CiiCod = PriCodIva),''),
IVA=CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
PriRegIva,PriId,PriProg
into #tmp
 from Tbpri where 
PRIREGIVA >0 
AND datepart(year,pridataest) = @ANNO
AND pricausale < 3 
ORDER BY PRIREGIVA,PRINUMPROT,PRIBISRET,PRIID,PRIPROG DESC
---
DECLARE @P AS SMALLINT
II:
select priid as IID,priprog as PP , ciitp as TP INTO #TMP2 from #tmp  WHERE ciitp = 0 order by PRIID, PRIPROG desc
set @P=(SELECT COUNT(*) FROM #TMP2)
if @P = 0 goto III
UPDATE #TMP2 SET TP = ISNULL((select CIITP FROM #TMP WHERE IID= PRIID AND PRIPROG = PP + 1 and CIITP > 0),0)
UPDATE #TMP SET ciitp = ISNULL((select TP FROM #TMP2 WHERE IID= PRIID AND PRIPROG = PP AND TP > 0),0) WHERE ciitp = 0
drop table #tmp2
GOTO II
III:
drop table #tmp2
---

select DISTINCT CLoFO,PIVA,
IMPONIBILE = CASE WHEN CIITP = 1 THEN SUM(PRIIMPDARE) ELSE 0 END,
IVA = CASE WHEN CIITP = 1 THEN SUM(IVA) ELSE 0 END,
NONIMP =  CASE WHEN CIITP = 2 THEN SUM(PRIIMPDARE) ELSE 0 END,
ESENTI= CASE WHEN CIITP = 3 THEN SUM(PRIIMPDARE) ELSE 0 END
into #TMP3
FROM #TMP where CIITP BETWEEN 1 AND 3 AND ISNUMERIC(PIVA) = 1
GROUP BY CLoFO,PIVA,CIITP

SELECT DISTINCT CLoFO,PIVA,
IMPONIBILE=SUM(IMPONIBILE),
IVA=SUM(IVA),
NONIMP=SUM(NONIMP),
ESENTI=SUM(ESENTI)
INTO #TMP4
 FROM #TMP3 GROUP BY CLoFO,PIVA
ORDER BY CLoFO,PIVA

SELECT CLoFO,PIVA,IMPONIBILE,IVA,NONIMP,ESENTI,ANAG = (SELECT TOP 1 ANADESC FROM VDOX.DBO.TBANA WHERE ANAPIVA = PIVA AND ANAGRP = CLoFO)
INTO #TMP5
FROM #TMP4

SELECT CLoFO,PIVA,IMPONIBILE,IVA,NONIMP,ESENTI,ANAG FROM #TMP5 
where ((IMPONIBILE <> 0 AND (IMPONIBILE > 0.49 OR IMPONIBILE < -0.49)) OR 
           (NONIMP <> 0 AND (NONIMP > 0.49 OR NONIMP < -0.49)) OR 
           (ESENTI <> 0 AND (ESENTI > 0.49 OR ESENTI < -0.49)) OR 
              (IVA <> 0 AND (IVA > 0.49 OR IVA < -0.49)))
ORDER BY CLoFO, ANAG
drop table #tmp
drop table #tmp3
drop table #tmp4
drop table #tmp5

GO
/****** Object:  StoredProcedure [dbo].[XEleVar]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XEleVar] @ANNO as smallint
as
SELECT  * INTO #ELENCHI FROM VELEDA2010 where EleCfP > 1 and EleCfAnno = @ANNO and EleCfUnion =0 and EleCfVariazione = 1

SELECT EleCfPriId,EleCfAnno,EleCfTipo,EleCfCodice,EleCfAnaDesc,EleCfNumDoc,EleCfDataDoc,EleCfImponibile,EleCfIva,Totale,
VaCfPriId,VaCfDataDoc,VaCfNumDoc,
VaCfImponibile= case 
when ElecfTipo ='CL' and totale < 0 and isnull(VaCfImponibile,'') = '' then 'D' 
when ElecfTipo ='CL' and totale >=0 and isnull(VaCfImponibile,'') = '' then 'C'
when EleCfTipo ='FO' AND totale < 0 and isnull(VaCfImponibile,'') = '' then 'C'
when EleCfTipo ='FO' AND totale >=0 and isnull(VaCfImponibile,'') = '' then 'D' else VaCfImponibile end,
VaCfIva= case 
when ElecfTipo ='CL' and totale < 0 and isnull(VaCfIva,'') = '' then 'D' 
when ElecfTipo ='CL' and totale >=0 and isnull(VaCfIva,'') = '' then 'C'
when EleCfTipo ='FO' AND totale < 0 and isnull(VaCfIva,'') = '' then 'C'
when EleCfTipo ='FO' AND totale >=0 and isnull(VaCfIva,'') = '' then 'D' else VaCfIva end
 into #tmp from #ELENCHI LEFT OUTER JOIN TbElVar ON EleCfPriId = VaCfPriId

DELETE FROM TbElVar WHERE VaCfPriId NOT IN (select EleCfPriId from #TMP ) and VaCfAnno = @ANNO

INSERT INTO TbElVar(VaCfPriId,VaCfAnno)
select EleCfPriId,@ANNO from #tmp where EleCfPriId not in( select VaCfPriId from TbElVar)

select * from #tmp Order By EleCfTipo,EleCfAnaDesc


GO
/****** Object:  StoredProcedure [dbo].[XESTRATTO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XESTRATTO] @CLIE AS VARCHAR(5), @AL as smalldatetime
as
SELECT     ScaImpRata = ISNULL(ScaImpRata, CASE WHEN PRKDA = 0 THEN DARE ELSE AVERE END), ScaNrata = isnull(ScaNrata, 1), 
                      ScaDsca = Isnull(ScaDsca, Pridataest), PrkConto, PrkTipoCo, PrkDesc, Pridataest, Causale, PriNumProt, Priregiva, PrkDocEst, DARE, AVERE, Descriz, 
                      PriCausale, PrkDocAnn, Prkaammgg, Partitario, PrkPaperta, PrIID, PRKAST, FORMULA, PRKINDI = CASE WHEN PRKTIPOCO = 0 THEN '' ELSE
                          (SELECT     ANAINDIRIZZO
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = PRKCONTO AND ANAGRP = 'CL') END, PRKCITTA = CASE WHEN PRKTIPOCO = 0 THEN '' ELSE
                          (SELECT     ANACAP + ' ' + ANACITTA + ' ' + ANAPROV
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = PRKCONTO AND ANAGRP = 'CL') END, PRIPROG, PRKDA,TotaleFattura=case when Pricausale=3 then DARE else cast(0.00 as decimal) end
into #TMP FROM         tbsca RIGHT OUTER JOIN
                      vb8 ON SCARIFID = PRIID AND SCARIFPROG = PRIPROG AND ScaRifDa = PRKDA INNER JOIN
                      GEVE.DBO.TbCli ON prkconto = ClCod 
WHERE     PARTITARIO = 1 AND PRKPAPERTA = 0 AND CLCOD=@CLIE 

UPDATE #TMP SET DARE=SCAIMPRATA WHERE PRICAUSALE = 3

SELECT DISTINCT A=FORMULA,B=ScaDsca,C=ScaNrata INTO #TMP2 FROM #TMP WHERE PRICAUSALE = 3 

UPDATE #TMP SET ScaDsca=ISNULL((SELECT B FROM #TMP2 WHERE A=FORMULA AND C=ScaNrata ),ScadSca)


DELETE FROM #TMP WHERE ScaDsca> @AL

declare @P as decimal(12,2)

set @P=(select sum(DARE) - SUM(AVERE) FROM #TMP)

if @P = 0
BEGIN
DELETE FROM #TMP
END
SELECT * FROM #TMP


GO
/****** Object:  StoredProcedure [dbo].[XF1]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XF1] @DAL as smalldatetime,@AL as smalldatetime,@BLOCK as int,@CAUS as smallint,@SW as smallint,@CAUCH as smallint
 as
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
SUBSTRING(prkconto,1,2),PRICAUSALE
from VH8
where PrkTipoCo = 0 and Prkaammgg between @DAL and @AL and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl,PRICAUSALE
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end, 
PRICAUSALE,0
from VH8 inner join TbGrp on GrpCod = 'CL' 
where PrkTipoCo = 1  and Prkaammgg between @DAL and @AL and PrkConto < GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,PRICAUSALE
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,
case when @caus <> pricausale then sum(DARE) else 0 end,
case when @caus <> pricausale then sum(AVERE)else 0 end,
case when @caus <> pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when @caus = pricausale then sum(DARE) else 0 end,
case when @caus = pricausale then sum(AVERE)else 0 end,
case when @caus = pricausale then (sum(DARE)-sum(AVERE)) else 0 end,
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end ,
PRICAUSALE,1
from VH8 inner join TbGrp on GrpCod = 'FO' 
where PrkTipoCo = 1  and Prkaammgg between @DAL and @AL and PrkConto > GrpMigl and PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9,pricausale 
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select distinct TMCBLOCK,TMCCPT,(SELECT PIAANACO FROM TBPIA WHERE PIACODCO = TMCCPT),(SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = TMCCPT),
SUM(TMCDARE),SUM(TMCAVERE),SUM(TMCSALDO),SUM(TMCDAREP),SUM(TMCAVEREP),SUM(TMCSALDOP),SUBSTRING(TMCCPT,1,2),0
from TMPBILC where TMCBLOCK = @BLOCK
GROUP BY TMCCPT,TMCBLOCK
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select @BLOCK,PIACODCO ,PIAANACO,0,0,0,0,0,0,0,SUBSTRING(PIACODCO,1,2),0 
FROM TBPIA WHERE SUBSTRING(PIACODCO,4,2) = '00' 
DECLARE @APERTURA AS DECIMAL(13,2),@CONTO AS VARCHAR(5)
SET @APERTURA = 0.0
SET @CONTO = (SELECT ESEBILAP FROM TBESE WHERE ESEANNO = DATEPART(YEAR,@DAL))
SET @APERTURA =(SELECT ISNULL(SUM(TMSSALDOP),0) FROM TMPBILS WHERE NOT EXISTS (SELECT * FROM tmpbils where TMSCONTO = @CONTO AND TMSCAUS = 45 and tmsblock = @block))
IF @APERTURA <> 0 
BEGIN
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select @BLOCK,@CONTO ,(SELECT PIAANACO FROM TBPIA WHERE PIACODCO = @CONTO),(SELECT PIAFL01 FROM TBPIA WHERE PIACODCO = @CONTO),0,0,0,0,0,(@APERTURA * -1),SUBSTRING(@CONTO,1,2),45 
DELETE FROM TMPBILS WHERE TMSCONTO = @CONTO AND TMSCAUS <> 45 and tmsblock = @block
END
UPDATE TMPBILS SET TMSDEMAS = (select ISNULL(PIAANACO,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSMASTRO+'.00' ) WHERE TMSBLOCK = @BLOCK
IF @SW = 1
BEGIN
INSERT INTO TMPANNS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSPDARE,TMSPAVERE,TMSPSALDO,TMSPDAREP,TMSPAVEREP,TMSPSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT DISTINCT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,0,0,0,0,0,0,TMSMASTRO,TMSCAUS,TMSDEMAS
from TMPBILS where TMSBLOCK = @BLOCK
DELETE FROM TMPBILS WHERE TMSBLOCK = @BLOCK 
DELETE FROM TMPBILC WHERE TMCBLOCK = @BLOCK 
END
IF @SW = 2
BEGIN
INSERT INTO TMPANNS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSPDARE,TMSPAVERE,TMSPSALDO,TMSPDAREP,TMSPAVEREP,TMSPSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS)
SELECT DISTINCT TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,0,0,0,0,0,0,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS,TMSDEMAS
from TMPBILS where TMSBLOCK = @BLOCK
DELETE FROM TMPBILS WHERE TMSBLOCK = @BLOCK 
DELETE FROM TMPBILC WHERE TMCBLOCK = @BLOCK 
END
GO
/****** Object:  StoredProcedure [dbo].[XF1P]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XF1P] @DAL as smalldatetime,@AL as smalldatetime,@BLOCK as int,@CAUS as smallint,@SW as smallint,@CAUCH as smallint
 as
-- LA CAUSALE SULL'ANNO PRECEDENTE NON SERVE ----
INSERT INTO TMPBILS(TMSBLOCK,TMSCONTO,TMSDESC,TMSFLAG,TMSDARE,TMSAVERE,TMSSALDO,TMSDAREP,TMSAVEREP,TMSSALDOP,TMSMASTRO,TMSCAUS)
select distinct @BLOCK,prkconto,prkdesc,piafl,0,0,0,sum(DARE),sum(AVERE) ,(sum(DARE)-sum(AVERE)),SUBSTRING(prkconto,1,2),0 
from VH8
where PrkTipoCo = 0 AND PIAFL <> 6 AND PIAFL <> 7 and Prkaammgg between @DAL and @AL
group by prkconto,prkdesc,piafl
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,0,0,0,sum(DARE) ,sum(AVERE),(sum(DARE)-sum(AVERE)),
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end ,0,0
from VH8 inner join TbGrp on GrpCod = 'CL' 
where PrkTipoCo = 1  and Prkaammgg between @DAL and @AL and PrkConto < GrpMigl AND PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9 
INSERT INTO TMPBILC(TMCBLOCK,TMCCONTO,TMCDESC,TMCFLAG,TMCDARE,TMCAVERE,TMCSALDO,TMCDAREP,TMCAVEREP,TMCSALDOP,TMCCPT,TMCCAUS,TMCTIPO)
select distinct @BLOCK,prkconto,prkdesc,piafl,0,0,0,sum(DARE) ,sum(AVERE),(sum(DARE)-sum(AVERE)),
case when GrpAl1 >=prkconto then GrpCpt1
when GrpAl2 >=prkconto then GrpCpt2 
when GrpAl3 >=prkconto then GrpCpt3  
when GrpAl4 >=prkconto then GrpCpt4 
when GrpAl5 >=prkconto then GrpCpt5 
when GrpAl6 >=prkconto then GrpCpt6 
when GrpAl7 >=prkconto then GrpCpt7 
when GrpAl8 >=prkconto then GrpCpt8 else
GrpCpt9 end , 0,1
from VH8 inner join TbGrp on GrpCod = 'FO' 
where PrkTipoCo = 1  and Prkaammgg between @DAL and @AL and PrkConto > GrpMigl AND PRICAUSALE <> @CAUCH
group by prkconto,prkdesc,piafl, GrpAl1,GrpCpt1,GrpAl2,GrpCpt2,GrpAl3,GrpCpt3,GrpAl4,GrpCpt4,GrpAl5,GrpCpt5,
GrpAl6,GrpCpt6,GrpAl7,GrpCpt7,GrpAl8,GrpCpt8,GrpCpt9

GO
/****** Object:  StoredProcedure [dbo].[XF5]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XF5] @ANNO AS SMALLINT,@CAUCH as smallint
AS
--SET @ANNO = 2004
--SET @CAUCH = 44
--SET @BLoCK = 1

delete from TMPBINV
declare @CHI as varchar(5),@PEP as varchar(5),@UTI as varchar(5),@PER as varchar(5)

set @CHI = (select esebilchi from tbese where eseanno = @ANNO)
SET @PEP = (select esepp from tbese where eseanno = @ANNO)
SET @UTI = (select eseuti from tbese where eseanno = @ANNO)
SET @PER = (select eseper from tbese where eseanno = @ANNO)
INSERT INTO tmpbinv (TMSBLOCK,TMSCLASSE,TMSCONTO,TMSDCLFO,TMSTIPOCO,TMSCODCO,TMSDESC,TMSDARE,TMSAVERE,TMSMASTRO,TMSDEMAS)
select 0,0,pricoavere,'',PrkTipoCo,pricoavere,'',priimpavere,0,'','' from tbpri 
INNER JOIN TBPRK ON PRIID = PRKID AND PRKCONTO = PRICOAVERE 

where pricausale = @CAUCH and datepart(year,pridataest) = @ANNO AND pricodare = @CHI
INSERT INTO tmpbinv (TMSBLOCK,TMSCLASSE,TMSCONTO,TMSDCLFO,TMSTIPOCO,TMSCODCO,TMSDESC,TMSDARE,TMSAVERE,TMSMASTRO,TMSDEMAS)
select 0,0,pricodare,'',PrkTipoCo,pricodare,'',0,priimpdare,'','' from tbpri
INNER JOIN TBPRK ON PRIID = PRKID AND PRKCONTO = PRICODARE
where pricausale = @CAUCH and datepart(year,pridataest) = @ANNO AND pricoavere = @CHI

INSERT INTO tmpbinv (TMSBLOCK,TMSCLASSE,TMSCONTO,TMSDCLFO,TMSTIPOCO,TMSCODCO,TMSDESC,TMSDARE,TMSAVERE,TMSMASTRO,TMSDEMAS)
select 0,0,pricoavere,'',PrkTipoCo,pricoavere,'',priimpavere,0,'','' from tbpri
INNER JOIN TBPRK ON PRIID = PRKID AND PRKCONTO = PRICOAVERE 
 where pricausale = @CAUCH and datepart(year,pridataest) = @ANNO AND pricodare = @PEP
INSERT INTO tmpbinv (TMSBLOCK,TMSCLASSE,TMSCONTO,TMSDCLFO,TMSTIPOCO,TMSCODCO,TMSDESC,TMSDARE,TMSAVERE,TMSMASTRO,TMSDEMAS)
select 0,0,pricodare,'',PrkTipoCo,pricodare,'',0,priimpdare,'','' from tbpri
INNER JOIN TBPRK ON PRIID = PRKID AND PRKCONTO = PRICODARE 
 where pricausale = @CAUCH and datepart(year,pridataest) = @ANNO AND pricoavere = @PEP

DELETE TMPBINV WHERE TMSCONTO = @UTI
 
DELETE TMPBINV WHERE TMSCONTO = @PER
 
DECLARE @MIGL AS INT

SET @MIGL = (SELECT GRPMIGL FROM TBGRP WHERE GRPCOD = 'FO')

update TMPBINV SET TMSTIPOCO = 2 WHERE TMSTIPOCO = 1 AND TMSCONTO > @MIGL 

update  TMPBINV SET TMSCODCO = case when GrpAl1 >=TMSCONTO then GrpCpt1
when GrpAl2 >=TMSCONTO then GrpCpt2 
when GrpAl3 >=TMSCONTO then GrpCpt3  
when GrpAl4 >=TMSCONTO then GrpCpt4 
when GrpAl5 >=TMSCONTO then GrpCpt5 
when GrpAl6 >=TMSCONTO then GrpCpt6 
when GrpAl7 >=TMSCONTO then GrpCpt7 
when GrpAl8 >=TMSCONTO then GrpCpt8 else
TMSCONTO end ,
TMSDCLFO = (select anadesc from vdox.dbo.tbana where anacod = TMSCONTO AND ANAGRP = 'cl')
FROM tmpbinv,tbgrp where grpcod = 'CL' AND TMSTIPOCO = 1

update  TMPBINV SET TMSCODCO = case when GrpAl1 >=TMSCONTO then GrpCpt1
when GrpAl2 >=TMSCONTO then GrpCpt2 
when GrpAl3 >=TMSCONTO then GrpCpt3  
when GrpAl4 >=TMSCONTO then GrpCpt4 
when GrpAl5 >=TMSCONTO then GrpCpt5 
when GrpAl6 >=TMSCONTO then GrpCpt6 
when GrpAl7 >=TMSCONTO then GrpCpt7 
when GrpAl8 >=TMSCONTO then GrpCpt8 else
TMSCONTO end ,
TMSDCLFO = (select anadesc from vdox.dbo.tbana where anacod = TMSCONTO AND ANAGRP = 'fo')
FROM tmpbinv,tbgrp where grpcod = 'FO' AND TMSTIPOCO = 2

update TMPBINV SET TMSMASTRO = SUBSTRING(TMSCODCO,1,2)

UPDATE TMPBINV SET TMSDEMAS = (select ISNULL(PIAANACO,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSMASTRO+'.00' )

UPDATE TMPBINV SET TMSDCLFO = (select ISNULL(PIAANACO,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSCONTO ) WHERE tmstipoco = 0
update TMPBINV SET TMSDESC = (select ISNULL(PIAANACO,'ERRATO') FROM TBPIA WHERE PIACODCO = TMSCODCO )
update TMPBINV SET TMSCLASSE = (select ISNULL(PIAFL01,0) FROM TBPIA WHERE PIACODCO = TMSCODCO )

update TMPBINV SET TMSBLOCK = case 
when TMSCLASSE < 6 and TMSCONTO = @UTI THEN 10 
when TMSCLASSE < 6 and TMSCONTO = @PER THEN 10 
when TMSCLASSE < 6 AND TMSDARE  > 0 then 0 
when TMSCLASSE < 6 AND TMSAVERE > 0 then 1 
when TMSCLASSE > 7 AND TMSDARE > 0 then 0
when TMSCLASSE > 7 AND TMSAVERE > 0 then 1
when TMSCLASSE = 6 AND TMSDARE > 0 then 6 
when TMSCLASSE = 7 AND TMSAVERE > 0 then 7 
when TMSCLASSE = 6 AND TMSAVERE > 0 then 7 
when TMSCLASSE = 7 AND TMSDARE > 0 then 6 

end


SELECT     TOP 100 PERCENT  year(dbo.TbPrk.PrkAammgg) as annoinv, dbo.TbPrk.PrkConto,dbo.TbPrk.PrkDa, dbo.TbPrk.PrkDocAnn, dbo.TbPrk.PrkDocEst,
impriga = case when prkda = 0 then dbo.TbPri.PriImpDare else priimpavere *-1 end,
            pridesc, pricausale, piaanaco    into #TMP    
FROM         dbo.TbPrk LEFT OUTER JOIN
                      dbo.TbPri ON dbo.TbPrk.PrkId = dbo.TbPri.PriId right OUTER JOIN
                      dbo.TbPia ON dbo.TbPrk.Prkconto  = dbo.TbPia.Piacodco and tbpia.piafl08 = 1 and piafl01 < 6
WHERE  (dbo.TbPrk.prktipoco = '0') and pricausale <> @CAUCH AND year(dbo.TbPrk.PrkAammgg) = @ANNO
ORDER BY annoinv,dbo.TbPrk.PrkConto, dbo.TbPrk.PrkDocAnn, dbo.TbPrk.PrkDocEst

DELETE FROM TMPBPART

INSERT INTO TMPBPART (TMBCONTO,TMBDESC,TMBSALDO,TMBDOCAN,TMBDOCEST)
select prkconto,pridesc,sum(impriga),prkdocann,prkdocest from #TMP
 group by  prkconto,prkdocann,prkdocest,pridesc
having sum(impriga) <> 0 AND PRKCONTO IN(SELECT DISTINCT tmsconto from tmpbinv)
drop table #tmp
--select * from tmpBINV where TMSCLASSE <> 6 AND TMSCLASSE <> 7 AND (tmsdare >0 or TMSAVERE < 0)
--ORDER BY tmscodco
--select * from tmpBINV where TMSCLASSE <> 6 AND TMSCLASSE <> 7 AND (TMSAVERE >0 or TMSDARE < 0)
--ORDER BY tmscodco

--select * from tmpBINV where (TMSCLASSE =6  OR TMSCLASSE= 7)  AND (tmsdare >0 or TMSAVERE < 0)
--ORDER BY tmscodco
--select * from tmpBINV where (TMSCLASSE =6  OR TMSCLASSE= 7)  AND (TMSAVERE >0 or TMSDARE < 0)
--ORDER BY tmscodco




GO
/****** Object:  StoredProcedure [dbo].[XH11]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XH11] @FAL as smalldatetime
 as
select distinct  priid,PriProg,PridataGio,pricausale,PriCodare,
DAREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoDare ),'***ERRATO***'))),PriCoAvere,
AVEREDESC =ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'CL'),
ISNULL((SELECT ANADESC FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),
ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PriCoAvere ),'***ERRATO***'))),
DARE = CONVERT(DECIMAL(13,2),case when PriCausale < 3 then PriImpDare +(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) else PriImpDare end),
PriImpAvere,priDesc,priGstampa,
PriNumProt,PriBisRet,PriCodIva,PriRegIva,PriDocEst,PriDataEst,PriDescB,PriNsRif,PriArtFisc,
TIPOREG = isnull((select RIvaTipo from TbRegIva where PriRegIva = RIvaNReg and datePart(year,pridatagio) = RIvaAnno),0),
RETTIF = CASE WHEN DATEPART(YEAR,PRIDATAGIO) <> DATEPART(YEAR,PRIDATAEST) AND PRICAUSALE > 3 THEN 1 ELSE 0 END
INTO #TMP
from TBPRI 
INNER JOIN TBPRK 
ON PRIID = PRKID
left outer join tbcii as cii on pricodiva = cii.ciicod
WHERE PRIgSTAMPA = 0
AND PRIDATAGIO  <= @FAL
ORDER BY PRIDATAGIO,PRIID,PRIPROG
SELECT DISTINCT PRIID,PRIDATAGIO,(SELECT COUNT(PrkDa) from Tbprk where prkid = priId and PrkDa = 0 )as righeD,
(SELECT COUNT(PrkDa) from Tbprk where prkid = priId and PrkDa = 1 ) AS RIGHEA
INTO #TMP1 
FROM #TMP inner join tbprk on prkid = priId 
GROUP BY PRIID,PRIDATAGIO
ORDER BY PRIDATAGIO,PRIID
DELETE FROM coge.dbo.tmpgio
INSERT INTO COGE.DBO.TMPGIO (priid,PriProg,PridataGio,pricausale,PriCodare,DAREDESC,PriCoAvere,AVEREDESC,
PriImpdare,PriImpavere,priDesc,priGstampa,RigheD,RigheA,PriNumProt,PriBisRet,PriCodIva,PriRegIva,PriDocEst,PriDataEst,PriDescB,PriNsRif,PriArtFisc,TIPOREG,RETTIF) 
select A.priid,PriProg,A.PridataGio,pricausale,PriCodare,DAREDESC ,PriCoAvere,AVEREDESC,
DARE,PriImpAvere,priDesc,prIGstampa ,RIGHED,RIGHEA,
PriNumProt,PriBisRet,PriCodIva,PriRegIva,PriDocEst,PriDataEst,PriDescB,PriNsRif,PriArtFisc,TIPOREG,RETTIF 
FROM #TMP AS A INNER JOIN #TMP1 AS B ON A.PRIID =B.PRIID
DROP TABLE #TMP
DROP TABLE #TMP1

GO
/****** Object:  StoredProcedure [dbo].[XH8]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XH8] @MINCONTO AS VARCHAR(6),@MAXCONTO AS VARCHAR(6), @DAL as smalldatetime,@AL as smalldatetime,@DAP as smalldatetime,@ALP as smalldatetime
 as
SELECT *
INTO #TMP
FROM VH8 WHERE PRKTIPOCO+PRKCONTO BETWEEN @MINCONTO AND @MAXCONTO AND PRIDATAGIO BETWEEN @DAL AND @AL
SELECT DISTINCT prkconto , SUM(DARE) as TDARE,sum(AVERE) as TAVERE ,(SUM(DARE)-SUM(AVERE)) AS TSALDO 
into #tmp1
from Vh8 where PRIDATAGIO BETWEEN @DAP AND @ALP
GROUP BY PRKCONTO

GO
/****** Object:  StoredProcedure [dbo].[XI1]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XI1] @FAL as SMALLDATETIME,@REG as SMALLINT,@DBVDOX as VARCHAR(20),@BLOCK AS INT
as
---SET @FAL = '31/12/2003'
---SET @REG = 2
---SET @DBVDOX = 'VDOX'
declare @cmd as varchar(2000)
SET @CMD ='INSERT INTO TMPREGIVA ([PRegId],[PRegDataG],[PRegDataE],[PRegNumProt],[PRegProtBis],[PRegNumDoc],[PRegCliFor],
[PRegAnaGraf],[PRegImpon] ,[PRegCodIva],[PRegAliq],[PRegImpIva],[PRegCpt],[PRegValuta],[PRegPND],[PRegMerce],[PRegNumReg],[PRegPriId],[PRegPriProg]) 
select '+CAST(@BLOCK AS VARCHAR(10))+',PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
case when pricausale = 2 then PriCoAvere else PriCoDare end,
CASE WHEN pricausale = 1 then 
ISNULL((SELECT RTRIM(ANARAG1)+replicate('' '',28-len(RTRIM(ANARAG1)))+ANARAG2 FROM '+@DBVDOX+'.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = ''CL'' OR ANACOD = PriCoDare AND ANAGRP = ''FO''),''* ERRATO *'')
WHEN pricausale = 2 then
ISNULL((SELECT RTRIM(ANARAG1)+replicate('' '',28-len(RTRIM(ANARAG1)))+ANARAG2 FROM '+@DBVDOX+'.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = ''CL'' OR ANACOD = PriCoAvere AND ANAGRP = ''FO''),''* ERRATO *'')
WHEN PriDesc > '' '' then
PriDesc
else ''CORRISPETTIVI DEL GIORNO'' 
END,
PriImpdare,PriCodIva,
isnull((select CiiDes from TbCii where CiiCod = PriCodIva),''''),
CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
case when pricausale = 2 then  PriCoDare else PricoAvere end,PriValuta,
isnull((select CiiInd from TbCii where CiiCod = PriCodIva),0),Prifl06,PriRegIva,PriId,PriProg
 from Tbpri where 
PRIREGIVA = '+CAST(@REG AS VARCHAR(10))+'
AND PRIDATAGIO <='''+CONVERT(VARCHAR(20),@FAL,103)+'''
AND datepart(year,pridatagio) = datepart(year,'''+CONVERT(VARCHAR(20),@FAL,103)+''')
AND pricausale <> 3 AND PriIvaPrint = 0
ORDER BY PRINUMPROT,PRIBISRET,PRIID,PRIPROG'
EXEC(@CMD)

UPDATE tmpregiva set PRegAnaGraf = substring((PRegAnaGraf) + (select Pridesc from tbpri where priid = PRegPriid and PriProg = PRegPriProg),1,60)
 where PRegNumReg in (select DISTINCT RivaautofCee from TBREGIVA WHERE RIvaAutoFCee > 0 AND RIvaAnno = datepart(year,CONVERT(VARCHAR(20),@FAL,103))) and PRegId=@BLOCK AND PRegNumReg =@REG

 UPDATE TMPREGIVA SET PRegFinoAl=@FAL WHERE PRegId=@BLOCK AND PRegNumReg =@REG


GO
/****** Object:  StoredProcedure [dbo].[XI12019]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[XI12019] @FAL as SMALLDATETIME,@REG as SMALLINT,@DBVDOX as VARCHAR(20),@BLOCK AS INT
as
--SET @FAL = '31/01/2019'
---SET @REG = 2
---SET @DBVDOX = 'VDOX'

DECLARE @AG AS SMALLDATETIME,@REGIMEIVA AS VARCHAR(1)
--SET @REGIMEIVA = (SELECT AZIREGIMEIVA FROM TBAZI WHERE AZIANNOLAVORO = DATEPART(YEAR,@FAL))
 SET @AG = dateadd(DAY,15,@FAL)
 --If @REGIMEIVA = 1 
 --BEGIN
 --SET @AG = DATEADD(Month,1,@AG)
 --END
declare @cmd as varchar(2000)
SET @CMD ='INSERT INTO TMPREGIVA ([PRegId],[PRegDataG],[PRegDataE],[PRegNumProt],[PRegProtBis],[PRegNumDoc],[PRegCliFor],
[PRegAnaGraf],[PRegImpon] ,[PRegCodIva],[PRegAliq],[PRegImpIva],[PRegCpt],[PRegValuta],[PRegPND],[PRegMerce],[PRegNumReg],[PRegPriId],[PRegPriProg]) 
select '+CAST(@BLOCK AS VARCHAR(10))+',PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
case when pricausale = 2 then PriCoAvere else PriCoDare end,
CASE WHEN pricausale = 1 then LTRIM(substring(
ISNULL((select AnaPiva from '+@DBVDOX+'.dbo.tbana where anacod=PriCoDare AND (ANAGRP=''CL'' OR ANAGRP=''FO'')),
ISNULL((select AnaCfis from '+@DBVDOX+'.dbo.tbana where anacod=PriCoDare AND (ANAGRP=''CL'' OR ANAGRP=''FO'')),''''))+'' ''+
ISNULL((SELECT LTRIM(RTRIM(ANADESC)) FROM '+@DBVDOX+'.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = ''CL'' OR ANACOD = PriCoDare AND ANAGRP = ''FO''),''* ERRATO *''),1,60))
WHEN pricausale = 2 then LTRIM(substring(
ISNULL((select AnaPiva from '+@DBVDOX+'.dbo.tbana where anacod=PriCoAvere AND (ANAGRP=''CL'' OR ANAGRP=''FO'')),
ISNULL((select AnaCfis from '+@DBVDOX+'.dbo.tbana where anacod=PriCoAvere AND (ANAGRP=''CL'' OR ANAGRP=''FO'')),''''))+'' ''+
ISNULL((SELECT LTRIM(RTRIM(ANADESC)) FROM '+@DBVDOX+'.DBO.TBANA WHERE ANACOD = PriCoavere AND ANAGRP = ''CL'' OR ANACOD = PriCoavere AND ANAGRP = ''FO''),''* ERRATO *''),1,60))
WHEN PriDesc > '' '' then
PriDesc
else ''CORRISPETTIVI DEL GIORNO'' 
END,
PriImpdare,PriCodIva,
isnull((select CiiDes from TbCii where CiiCod = PriCodIva),''''),
CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
case when pricausale = 2 then  PriCoDare else PricoAvere end,PriValuta,
isnull((select CiiInd from TbCii where CiiCod = PriCodIva),0),Prifl06,PriRegIva,PriId,PriProg
 from Tbpri where 
PRIREGIVA = '+CAST(@REG AS VARCHAR(10))+'
AND PRIDATAEST <='''+CONVERT(VARCHAR(20),@FAL,103)+'''
AND PRIDATAGIO <='''+CONVERT(VARCHAR(20),@AG,103)+'''
AND datepart(year,pridatagio) = datepart(year,'''+CONVERT(VARCHAR(20),@FAL,103)+''')
AND pricausale <> 3 AND PriIvaPrint = 0
ORDER BY PRINUMPROT,PRIBISRET,PRIID,PRIPROG'
EXEC(@CMD)

UPDATE TMPREGIVA SET PRegFinoAl=@FAL WHERE PRegId=@BLOCK AND PRegNumReg =@REG


UPDATE tmpregiva set PRegAnaGraf = substring((PRegAnaGraf) + ' ' +(select Pridesc from tbpri where priid = PRegPriid and PriProg = PRegPriProg),1,60)
 where PRegNumReg in (select DISTINCT RivaautofCee from TBREGIVA WHERE RIvaAutoFCee > 0 AND RIvaAnno = datepart(year,CONVERT(VARCHAR(20),@FAL,103))) and PRegId=@BLOCK AND PRegNumReg =@REG
 
 UPDATE tmpregiva set PRegNumDoc=FteNumero from geve.dbo.tbfte_Passiva,Tmpregiva where PregPriid=FteRifPri and PRegId=@BLOCK AND PRegNumReg =@REG 

 UPDATE tmpregiva set PRegNumDoc=FteAlfanum from geve.dbo.tbfte,Tmpregiva where FteRegistro=@REG and PRegCliFor=FteCliente and cast(FteNumero as varchar(6))=PRegNumDoc and PRegId=@BLOCK AND PRegNumReg =@REG 
GO
/****** Object:  StoredProcedure [dbo].[XI1XML]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[XI1XML] @ANNO as INT,@PERIODO AS INT
AS
----
DECLARE @DAL as smalldatetime,@FAL as SMALLDATETIME,@BLOCK AS INT
SET @BLOCK = 1
SET @DAL = (select top 1 RxDal From TbRegXml where RxAnno =@ANNO and RxPeriodo =@PERIODO)
SET @FAL = (select top 1 Rxal From TbRegXml where RxAnno =@ANNO and RxPeriodo =@PERIODO)

TRUNCATE TABLE TMPFTERRORE

DELETE FROM TMPXMLREGIVA where PRegAnno = @ANNO AND PRegPeriodo = @PERIODO

INSERT INTO TMPXMLREGIVA (PRegAnno,PRegPeriodo,PRegId, PRegDataG, PRegDataE, PRegNumProt, PRegProtBis, PRegNumDoc, PRegCliFor,
PRegAnaGraf, PRegImpon, PRegCodIva, PRegAliq, PRegImpIva, PRegCpt, PRegValuta, PRegPND, PRegMerce, PRegNumReg, PRegPriId, PRegPriProg, PRegTipo) 


select @ANNO,@PERIODO,@BLOCK,PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
CliFor=CASE WHEN pricausale = 2 THEN PriCoAvere else PriCoDare end,
AnaGraf= CASE WHEN pricausale = 1 THEN ISNULL((SELECT RTRIM(ANARAG1)+ ' '+ANARAG2 FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL' OR ANACOD = PriCoDare AND ANAGRP = 'FO'),'* ERRATO *')
              WHEN pricausale = 2 THEN ISNULL((SELECT RTRIM(ANARAG1)+ ' '+ANARAG2 FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'CL' OR ANACOD = PriCoAvere AND ANAGRP = 'FO'),'* ERRATO *')
              WHEN PriDesc > ' ' THEN PriDesc
              ELSE 'CORRISPETTIVI DEL GIORNO' END,
PriImpdare,PriCodIva,
Aliq=isnull((select CiiDes from TbCii where CiiCod = PriCodIva),''),
Impiva = CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
Cpt=CASE WHEN pricausale = 2 then  PriCoDare else PricoAvere end,
PriValuta,
PRegPND= isnull((select CiiInd from TbCii where CiiCod = PriCodIva),0),
Prifl06,PriRegIva,PriId,PriProg,
Tipo = CASE WHEN (Select RxTipo from TbRegXml where RxAnno =@ANNO and RxPeriodo = @PERIODO and RxRegistro = PRIREGIVA) = 'V' THEN 'CL' ELSE 'FO' END

from Tbpri where 
PRIREGIVA in (select RxRegistro From TbRegXml where RxAnno =@ANNO and RxPeriodo = @PERIODO and RxSel = 1)
AND PRIDATAGIO BETWEEN @DAL AND @FAL
AND pricausale <> 3 
ORDER BY PRIREGIVA,PRINUMPROT,PRIBISRET,PRIID,PRIPROG

update a set PREGCODIVA = (SELECT TOP 1 PRegcodiva  from TMPXMLREGIVA b where b.PregPriId = a.PregPriId and b.PregPriProg > a.pregpriprog and b.PregCodiva > 0 order by PregPriProg)
from TMPXMLREGIVA a
where a.PRegcodiva = 0



---------preparazione dati integrativi persono fisiche
select AnaCod into #no from  vdox.dbo.TbAna where LEN(Anacfis)<> 16

INSERT INTO TbIntPF(PfClifor,PfAnadesc,PfCognome,PfNome,PfTipo,PFPiva,PFCFis)
SELECT DISTINCT AnaCod,AnaDesc, '','',AnaGrp,CASE WHEN AnaPivaEst <> '' THEN AnaPivaEst ELSE AnaPiva END, AnaCfis
FROM vdox.dbo.TbAna 
where LEN(Anacfis)= 16
      AND AnaCod not in (SELECT PfClifor from TbIntPF )
	  AND ANACOD IN (SELECT PRegCliFor FROM TMPXMLREGIVA WHERE PRegAnno = @ANNO AND PRegPeriodo = @PERIODO)

DELETE FROM TbIntPf where pfclifor in (select anacod from #no)

--------preparazione Tabella Partite Errate
DELETE FROM TbErrP WHERE PErrAnno=@ANNO AND PErrPeriodo=@PERIODO

INSERT INTO TbErrP(PErrAnno, PErrPeriodo, PErrTipo, PErrClifor, PErrAnaDesc, PErrPiva, PerrCFis, PErrPivaEst, PErrEscludi, PErrMsg, PErrErrore)
SELECT DISTINCT @ANNO,@PERIODO,AnaGrp,AnaCod,AnaDesc,AnaPiva,AnaCfis,AnaPivaEst,0,'', 0
FROM vdox.dbo.TbAna 
where AnaCod in (SELECT PRegCliFor FROM TMPXMLREGIVA WHERE PRegAnno = @ANNO AND PRegPeriodo = @PERIODO)
 
-------Controllo Partite Iva (VIENE SOLAMENTE COSIDARATO IL CASO DI MANCANTE)
update TbErrP
set PErrEscludi = 1,PErrErrore = 1,
    PErrMsg = 'MANCA PARTITA IVA !!'
WHERE PErrAnno=@ANNO AND PErrPeriodo=@PERIODO AND  rtrim(PErrPiva)='' and RTRIM(PerrCFis) = '' and  RTRIM(PErrPivaEst) = ''

-------Controllo Partite Iva ESTERE
update TbErrP
set PErrEscludi = 1,PErrErrore = 1,
    PErrMsg = 'PARTITA IVA ESTERA ERRATA !!'
WHERE PErrAnno=@ANNO AND PErrPeriodo=@PERIODO AND RTRIM(PErrPivaEst) <> '' AND  RTRIM(PErrPivaEst) <> 'OO99999999999' 
      AND (SUBSTRING(RTRIM(PErrPivaEst),1,2) NOT IN (SELECT DISTINCT PaSigla from TbPaesi)) 


GO
/****** Object:  StoredProcedure [dbo].[XI1XML_EST]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[XI1XML_EST] @ANNO as INT,@PERIODO AS INT
AS
----
DECLARE @DAL as smalldatetime,@FAL as SMALLDATETIME,@BLOCK AS INT
SET @BLOCK = 1
SET @DAL = (select top 1 RxDal From TbRegXml_EST where RxAnno =@ANNO and RxPeriodo =@PERIODO)
SET @FAL = (select top 1 Rxal From TbRegXml_EST where RxAnno =@ANNO and RxPeriodo =@PERIODO)

TRUNCATE TABLE TMPFTERRORE

DELETE FROM TMPXMLREGIVA_EST where PRegAnno = @ANNO AND PRegPeriodo = @PERIODO

DELETE FROM TMPXMLINVIATE_ELT_EST where PRegAnno = @ANNO AND PRegPeriodo = @PERIODO


-------SO CONSIDERANO SOLAMENTE I NOMINATIVI CON PARTITA IVA ESTERA > ''

INSERT INTO TMPXMLREGIVA_EST (PRegAnno,PRegPeriodo,PRegId, PRegDataG, PRegDataE, PRegNumProt, PRegProtBis, PRegNumDoc, PRegCliFor,
PRegAnaGraf, PRegImpon, PRegCodIva, PRegAliq, PRegImpIva, PRegCpt, PRegValuta, PRegPND, PRegMerce, PRegNumReg, PRegPriId, PRegPriProg, PRegTipo, PRegOk) 


select @ANNO,@PERIODO,@BLOCK,PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
CliFor=CASE WHEN pricausale = 2 THEN PriCoAvere else PriCoDare end,
AnaGraf= CASE WHEN pricausale = 1 THEN ISNULL((SELECT RTRIM(ANARAG1)+ ' '+ANARAG2 FROM VDOX.DBO.TBANA WHERE (ANACOD = PriCoDare AND ANAGRP = 'CL') OR (ANACOD = PriCoDare AND ANAGRP = 'FO')),'* ERRATO *')
              WHEN pricausale = 2 THEN ISNULL((SELECT RTRIM(ANARAG1)+ ' '+ANARAG2 FROM VDOX.DBO.TBANA WHERE (ANACOD = PriCoAvere AND ANAGRP = 'CL') OR (ANACOD = PriCoAvere AND ANAGRP = 'FO')),'* ERRATO *')
              WHEN PriDesc > ' ' THEN PriDesc
              ELSE 'CORRISPETTIVI DEL GIORNO' END,
PriImpdare,PriCodIva,
Aliq=isnull((select CiiDes from TbCii where CiiCod = PriCodIva),''),
Impiva = CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
Cpt=CASE WHEN pricausale = 2 then  PriCoDare else PricoAvere end,
PriValuta,
PRegPND= isnull((select CiiInd from TbCii where CiiCod = PriCodIva),0),
Prifl06,PriRegIva,PriId,PriProg,
Tipo = CASE WHEN (Select RxTipo from TbRegXml_EST where RxAnno =@ANNO and RxPeriodo = @PERIODO and RxRegistro = PRIREGIVA) = 'V' THEN 'CL' ELSE 'FO' END,
cast(1 as bit)
from Tbpri where 
PRIREGIVA in (select RxRegistro From TbRegXml_EST where RxAnno =@ANNO and RxPeriodo = @PERIODO and RxSel = 1)
AND PRIDATAGIO BETWEEN @DAL AND @FAL
AND pricausale <> 3
and ISNULL((SELECT AnaPivaest FROM VDOX.DBO.TBANA WHERE (ANACOD = PriCoDare AND ANAGRP = 'CL') OR (ANACOD = PriCoAvere AND ANAGRP = 'FO')),'') > '' 
ORDER BY PRIREGIVA,PRINUMPROT,PRIBISRET,PRIID,PRIPROG

INSERT INTO TMPXMLINVIATE_ELT_EST
SELECT *
FROM TMPXMLREGIVA_EST
WHERE PRegAnno = @ANNO AND PRegPeriodo = @PERIODO

------- RICAVO FATTURE NON SPEDITE O RICEVUTE ELETTRONICAMENTE

DELETE FROM TMPXMLREGIVA_EST
from TMPXMLREGIVA_EST  inner join
     GEVE.DBO.tBFte on PRegAnno = FteAnno and PRegNumReg = FteRegistro and PregNumdoc = FteNumero
where pregtipo = 'CL' and (FteProgSia > 0 or fteIdSdi > '')
AND PRegAnno = @ANNO AND PRegPeriodo = @PERIODO

DELETE FROM TMPXMLREGIVA_EST
from TMPXMLREGIVA_EST  inner join
     GEVE.DBO.tBFte_Passiva on FteRifPri = PRegPriId  
where pregtipo = 'FO'
AND PRegAnno = @ANNO AND PRegPeriodo = @PERIODO

DELETE FROM TMPXMLREGIVA_EST
from TMPXMLREGIVA_EST  inner join
     GEVE.DBO.tBFte on PRegAnno = datepart(year,fteDataOra) and PRegNumReg = FteRegistro and PregNumpROT = FteNumero
where pregtipo = 'FO' and (FteProgSia > 0 or fteIdSdi > '')
AND PRegAnno = @ANNO AND PRegPeriodo = @PERIODO AND FTERIF > 30000000


--------- ricavo  PER DIFFERENZA FATTURE SPEDITE E RICEVUTE ELETTRONICAMENTE

DELETE FROM TMPXMLINVIATE_ELT_EST
FROM TMPXMLINVIATE_ELT_EST A  INNER JOIN
     TMPXMLREGIVA_EST B ON B.PRegAnno = A.PRegAnno AND B.PRegPeriodo = A.PRegPeriodo AND B.PRegPriId = A.PRegPriId AND B.PRegPriProg = A.PRegPriProg
WHERE A.PRegAnno = @ANNO AND A.PRegPeriodo = @PERIODO


---------------------------------------------------------
update a set PREGCODIVA = (SELECT TOP 1 PRegcodiva  from TMPXMLREGIVA_EST b where b.PregPriId = a.PregPriId and b.PregPriProg > a.pregpriprog and b.PregCodiva > 0 order by PregPriProg)
from TMPXMLREGIVA_EST a
where a.PRegcodiva = 0



---------preparazione dati integrativi persono fisiche
select AnaCod into #no from  vdox.dbo.TbAna where LEN(Anacfis)<> 16

INSERT INTO TbIntPF(PfClifor,PfAnadesc,PfCognome,PfNome,PfTipo,PFPiva,PFCFis)
SELECT DISTINCT AnaCod,AnaDesc, '','',AnaGrp,CASE WHEN AnaPivaEst <> '' THEN AnaPivaEst ELSE AnaPiva END, AnaCfis
FROM vdox.dbo.TbAna 
where LEN(Anacfis)= 16
      AND AnaCod not in (SELECT PfClifor from TbIntPF )
	  AND ANACOD IN (SELECT PRegCliFor FROM TMPXMLREGIVA_EST WHERE PRegAnno = @ANNO AND PRegPeriodo = @PERIODO)

DELETE FROM TbIntPf where pfclifor in (select anacod from #no)

--------preparazione Tabella Partite Errate
DELETE FROM TbErrP WHERE PErrAnno=@ANNO AND PErrPeriodo=@PERIODO

INSERT INTO TbErrP(PErrAnno, PErrPeriodo, PErrTipo, PErrClifor, PErrAnaDesc, PErrPiva, PerrCFis, PErrPivaEst, PErrEscludi, PErrMsg, PErrErrore)
SELECT DISTINCT @ANNO,@PERIODO,AnaGrp,AnaCod,AnaDesc,AnaPiva,AnaCfis,AnaPivaEst,0,'', 0
FROM vdox.dbo.TbAna 
where AnaCod in (SELECT PRegCliFor FROM TMPXMLREGIVA_EST WHERE PRegAnno = @ANNO AND PRegPeriodo = @PERIODO)
 
-------Controllo Partite Iva (VIENE SOLAMENTE COSIDARATO IL CASO DI MANCANTE)
update TbErrP
set PErrEscludi = 1,PErrErrore = 1,
    PErrMsg = 'MANCA PARTITA IVA !!'
WHERE PErrAnno=@ANNO AND PErrPeriodo=@PERIODO AND  rtrim(PErrPiva)='' and RTRIM(PerrCFis) = '' and  RTRIM(PErrPivaEst) = ''

-------Controllo Partite Iva ESTERE
update TbErrP
set PErrEscludi = 1,PErrErrore = 1,
    PErrMsg = 'PARTITA IVA ESTERA ERRATA !!'
WHERE PErrAnno=@ANNO AND PErrPeriodo=@PERIODO AND RTRIM(PErrPivaEst) <> '' AND  RTRIM(PErrPivaEst) <> 'OO99999999999' 
      AND (SUBSTRING(RTRIM(PErrPivaEst),1,2) NOT IN (SELECT DISTINCT PaSigla from TbPaesi)) 

GO
/****** Object:  StoredProcedure [dbo].[XI1xxx]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[XI1xxx] @FAL as SMALLDATETIME,@REG as SMALLINT,@DBVDOX as VARCHAR(20),@BLOCK AS INT
as
---SET @FAL = '31/12/2003'
---SET @REG = 2
---SET @DBVDOX = 'VDOX'
declare @cmd as varchar(2000)
SET @CMD ='INSERT INTO TMPREGIVA ([PRegId],[PRegDataG],[PRegDataE],[PRegNumProt],[PRegProtBis],[PRegNumDoc],[PRegCliFor],
[PRegAnaGraf],[PRegImpon] ,[PRegCodIva],[PRegAliq],[PRegImpIva],[PRegCpt],[PRegValuta],[PRegPND],[PRegMerce],[PRegNumReg],[PRegPriId],[PRegPriProg]) 
select '+CAST(@BLOCK AS VARCHAR(10))+',PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
case when pricausale = 2 then PriCoAvere else PriCoDare end,
CASE WHEN pricausale = 1 then 
ISNULL((SELECT ANARAG1+ANARAG2 FROM '+@DBVDOX+'.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = ''CL'' OR ANACOD = PriCoDare AND ANAGRP = ''FO''),''* ERRATO *'')
WHEN pricausale = 2 then
ISNULL((SELECT ANARAG1+ANARAG2 FROM '+@DBVDOX+'.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = ''CL'' OR ANACOD = PriCoAvere AND ANAGRP = ''FO''),''* ERRATO *'')
WHEN PriDesc > '' '' then
PriDesc
else ''CORRISPETTIVI DEL GIORNO'' 
END,
PriImpdare,PriCodIva,
isnull((select CiiDes from TbCii where CiiCod = PriCodIva),''''),
CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
case when pricausale = 2 then  PriCoDare else PricoAvere end,PriValuta,
isnull((select CiiInd from TbCii where CiiCod = PriCodIva),0),Prifl06,PriRegIva,PriId,PriProg
 from Tbpri where 
PRIREGIVA = '+CAST(@REG AS VARCHAR(10))+'
AND PRIDATAGIO <='''+CONVERT(VARCHAR(20),@FAL,103)+'''
AND datepart(year,pridatagio) = datepart(year,'''+CONVERT(VARCHAR(20),@FAL,103)+''')
AND pricausale <> 3 AND PriIvaPrint = 0
ORDER BY PRINUMPROT,PRIBISRET,PRIID,PRIPROG'
EXEC(@CMD)
GO
/****** Object:  StoredProcedure [dbo].[XINGRIVEND]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc [dbo].[XINGRIVEND] @LINGUA AS SMALLINT
AS



select * into #TMP
from TbArt INNER JOIN
     TbTdis on TdisCod = ArtDibase
WHERE ArtDibase > 0 and ArtCod between 100 and 999


IF @LINGUA = 1
   BEGIN
   SELECT ArtCod,DESCRIZIONE = ArtDesc,INGREDIENTI = TDisItaliano
   FROM #TMP
   WHERE TDisItaliano > ''
   ORDER BY ArtCod
   END
ELSE IF @LINGUA = 2 
   BEGIN
   SELECT ArtCod,DESCRIZIONE = ArtDesc2,INGREDIENTI = TDisFrancese
   FROM #TMP
   WHERE TDisFrancese > ''
   ORDER BY ArtCod
   END 
ELSE IF @LINGUA = 3
   BEGIN
   SELECT ArtCod,DESCRIZIONE = ArtDesc3,INGREDIENTI = TDisInglese
   FROM #TMP
   WHERE TDisInglese > ''
   ORDER BY ArtCod
   END 
ELSE IF @LINGUA = 4
   BEGIN
   SELECT ArtCod,DESCRIZIONE = ArtDesc4,INGREDIENTI = TDisTedesco
   FROM #TMP
   WHERE TDisTedesco > ''
   ORDER BY ArtCod
   END 
ELSE IF @LINGUA = 5
   BEGIN
   SELECT ArtCod,DESCRIZIONE = ArtDesc4,INGREDIENTI = TDisSpagnolo
   FROM #TMP
   WHERE TDisSpagnolo > ''
   ORDER BY ArtCod
   END 



GO
/****** Object:  StoredProcedure [dbo].[XINTRACEE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE procedure [dbo].[XINTRACEE] @codiva as smallint,@dal as smalldatetime,@al as smalldatetime 
AS
if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##INTRA'))
   begin
   drop table ##INTRA
  end   
CREATE TABLE [##INTRA] (
	[INTRID] [int] NOT NULL ,
	[INTRPROG] [int] NOT NULL 
      ) ON [PRIMARY]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMPINTRA]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMPINTRA]

CREATE TABLE [dbo].[TMPINTRA] (
	[PROG] [int] IDENTITY (1, 1) NOT NULL ,
	[PRICODARE] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[ANADESC] [varchar] (60) COLLATE Latin1_General_CI_AS NOT NULL ,
	[PAESE] [varchar] (2) COLLATE Latin1_General_CI_AS NOT NULL ,
	[PIVA] [varchar] (18) COLLATE Latin1_General_CI_AS NOT NULL ,
	[IMPONIBILE] [decimal](13, 2) NOT NULL, 
        [TOTALP] [decimal](13, 2) NOT NULL,
        [TOTALT] [decimal](13, 2) NOT NULL
) ON [PRIMARY]


ALTER TABLE [dbo].[TMPINTRA] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMPINTRA] PRIMARY KEY  CLUSTERED 
	(
		[PROG]
	)  ON [PRIMARY] 


DBCC CHECKIDENT (TMPINTRA, RESEED)

DECLARE @IIDD AS INT,@P AS INT
DECLARE @IIPP AS INT,@CI AS SMALLINT

DECLARE DATIINTRA CURSOR FOR
select priid,priprog from tbpri where pricausale = 1 and pricodiva = @codiva and (pridatagio between @dal and @al) ORDER BY priid,priprog
OPEN DATIINTRA
FETCH NEXT FROM DATIINTRA
INTO @IIDD,@IIPP
WHILE @@FETCH_STATUS = 0
BEGIN
SET @P = 0
SET @CI = 0
INSERT INTO [##INTRA] ([INTRID] ,[INTRPROG]) 
SELECT @IIDD,@IIPP
IF @IIPP = 1 GOTO FINE_LOOP
LOOP_LOOP:
set @P = @IIPP -1
if @P < 1 GOTO FINE_LOOP
SET @CI = (select ISNULL(PRICODIVA,2) from tbpri where priid = @IIDD and priprog =@P)
IF @CI > 0 GOTO FINE_LOOP
INSERT INTO [##INTRA] ([INTRID] ,[INTRPROG]) 
SELECT @IIDD,@P
SET @IIPP = @P
GOTO LOOP_LOOP
FINE_LOOP:
   FETCH NEXT FROM DATIINTRA
   INTO @IIDD,@IIPP
END
CLOSE DATIINTRA
DEALLOCATE DATIINTRA
INSERT INTO [TMPINTRA] ([PRICODARE],[ANADESC],[PAESE],[PIVA],[IMPONIBILE],[TOTALP],[TOTALT]) 
SELECT Pricodare,AnaDESC,SUBSTRING(isnull(AnaPivaEst,' '),1,2) AS PAESE,
SUBSTRING(isnull(AnaPivaEst,' '),3,18) AS PIVA,IMPONIBILE=SUM(PRIIMPDARE),0,0
 FROM ##INTRA LEFT OUTER JOIN TBPRI ON INTRID = PRIID AND INTRPROG = PRIPROG
INNER JOIN VDOX.DBO.TBANA ON ANACOD = PRICODARE AND ANAGRP = 'CL'
GROUP BY PRICODARE,AnaDESC,AnaPivaEst
--- TOTALIZZO
DECLARE @IIDDP AS INT,@IMPONIBILE AS DECIMAL(13,2),@TOTALP AS DECIMAL(13,2),@PP AS SMALLINT,@TOTALT AS DECIMAL(13,2)
DECLARE DATIPROG CURSOR FOR
select PROG,IMPONIBILE from TMPINTRA  ORDER BY PROG
SET @PP = 0
SET @TOTALP = 0
SET @TOTALT = 0
OPEN DATIPROG
FETCH NEXT FROM DATIPROG
INTO @IIDDP,@IMPONIBILE
WHILE @@FETCH_STATUS = 0
BEGIN
set @PP = @PP + 1
set @TOTALT = (@TOTALT + @IMPONIBILE)
UPDATE TMPINTRA set TOTALT = @TOTALT ,TOTALP = 0 WHERE PROG = @IIDDP
set @TOTALP = (@TOTALP + @IMPONIBILE)
if @PP < 10 GOTO XFINE_LOOP
set @TOTALP = (@TOTALT - @TOTALP)
UPDATE TMPINTRA set TOTALP = @TOTALP WHERE PROG = @IIDDP
SET @TOTALP = 0
SET @PP = 0
XFINE_LOOP:
   FETCH NEXT FROM DATIPROG
   INTO @IIDDP,@IMPONIBILE
END
set @TOTALP = (@TOTALT - @TOTALP)
UPDATE TMPINTRA set TOTALP = @TOTALP WHERE PROG = @IIDDP
CLOSE DATIPROG
DEALLOCATE DATIPROG

GO
/****** Object:  StoredProcedure [dbo].[XIvaP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[XIvaP] @IvaPAnno as smallint,@IvaPMese as smallint
as
---SET @IvaPAnno = 2003
---SET @IvaPMese = 3
Update TbIvaP set IvaPImpon=IvaPImpon+IvaPIvaDe,IvaPivaDe=0 
where IvaPAnno=@IvaPAnno and IvaPMese=@IvaPMese and IvaPREgIva in (select rivanreg from tbregiva where rivatipo=5 AND RIvaAnno =@IvaPAnno)







GO
/****** Object:  StoredProcedure [dbo].[XLEGGIFTEREG]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[XLEGGIFTEREG] @ANNO as smallint
as
declare @DAL as smalldatetime, @AL as smalldatetime,@DM as smallint,@AM as smallint,@P as varchar(2),@REGIMEIVA AS VARCHAR(1),@PERIODO as varchar(50),@AG AS SMALLDATETIME,@AP AS SMALLDATETIME,@NMESE as smallint
DECLARE @IN as smalldatetime, @FI as smalldatetime

SET @REGIMEIVA = (SELECT AZIREGIMEIVA FROM TBAZI WHERE AZIANNOLAVORO = @ANNO)

DECLARE @MeseRisc TABLE(Mn Tinyint,OkMese varchar(3))
insert into @MeseRisc (Mn,OkMese) values (1,'GEN')
insert into @MeseRisc (Mn,OkMese) values (2,'FEB')
insert into @MeseRisc (Mn,OkMese) values (3,'MAR')
insert into @MeseRisc (Mn,OkMese) values (4,'APR')
insert into @MeseRisc (Mn,OkMese) values (5,'MAG')
insert into @MeseRisc (Mn,OkMese) values (6,'GIU')
insert into @MeseRisc (Mn,OkMese) values (7,'LUG')
insert into @MeseRisc (Mn,OkMese) values (8,'AGO')
insert into @MeseRisc (Mn,OkMese) values (9,'SET')
insert into @MeseRisc (Mn,OkMese) values (10,'OTT')
insert into @MeseRisc (Mn,OkMese) values (11,'NOV')
insert into @MeseRisc (Mn,OkMese) values (12,'DIC')

CREATE TABLE [dbo].[#TFTE] (
        [FePTipo] [tinyint] NOT NULL,
		[FePDal] [smalldatetime] NOT NULL,
        [FePAl]  [smalldatetime] NOT NULL,
        [FePCh]  [smallint] NOT NULL ,
        [FePPeriodo] [varchar] (50) NOT NULL, 
		[FePImage] [smallint] NOT NULL,
		[FePMese] [varchar] (3) NULL 
	) ON [PRIMARY]
	
SET @AG=(Select MAX(FteDataRicezione) FROM GEVE.DBO.TBFTE_PASSIVA WHERE FTEANNO = @ANNO AND FteRifPri > 0 ) 
SET @AP=(Select MIN(FteDataRicezione) FROM GEVE.DBO.TBFTE_PASSIVA WHERE FTEANNO = @ANNO AND FteRifPri > 0 )

IF @AG IS NULL 
BEGIN
SET @AP='01/01/'+CAST(@ANNO as varchar(4))
SET @AG =GETDATE()
INSERT INTO [#TFTE](FePTipo,FePDal,FePAl,FePCh,FePPeriodo,FePImage,FePMese)
select CAST(1 AS tinyint),@AP, @AG,0, CONVERT(VARCHAR(20),@AP,103) +' AL '+ CONVERT(VARCHAR(20),@AG,103),-1,'' 
goto II
END

INSERT INTO [#TFTE](FePTipo,FePDal,FePAl,FePCh,FePPeriodo,FePImage,FePMese)
select CAST(1 AS tinyint),@AP, @AG,0, CONVERT(VARCHAR(20),@AP,103) +' AL '+ CONVERT(VARCHAR(20),@AG,103),-1,'' 
 
II:

SET @AG=(Select MAX(FteDataRicezione) FROM GEVE.DBO.TBFTE_PASSIVA WHERE FTEANNO = @ANNO AND FteRifPri = 0 ) 
SET @AP=(Select MIN(FteDataRicezione) FROM GEVE.DBO.TBFTE_PASSIVA WHERE FTEANNO = @ANNO AND FteRifPri = 0 )

IF @AG is NULL
BEGIN
SET @AP='01/01/'+CAST(@ANNO as varchar(4))
SET @AG =GETDATE()
INSERT INTO [#TFTE](FePTipo,FePDal,FePAl,FePCh,FePPeriodo,FePImage,FePMese)
select CAST(0 AS tinyint),@AP, @AG,0, CONVERT(VARCHAR(20),@AP,103) +' AL '+ CONVERT(VARCHAR(20),@AG,103),-1,'' 
GOTO III
END

INSERT INTO [#TFTE](FePTipo,FePDal,FePAl,FePCh,FePPeriodo,FePImage,FePMese)
select CAST(0 AS tinyint),@AP, @AG,0, CONVERT(VARCHAR(20),@AP,103) +' AL '+ CONVERT(VARCHAR(20),@AG,103),-1,'' 


--if DATEPART(MONTH,@AG) = DATEPART(MONTH,@AP) GOTO III

----- rem lettura diretta parziale per mese


--set @NMESE = DATEPART(MONTH,@AP)

--LOOPMESE:
--if @NMESE > DATEPART(MONTH,@AG) GOTO III


--SET @IN=(Select Min(FteDataRicezione) FROM GEVE.DBO.TBFTE_PASSIVA WHERE FTEANNO = @ANNO AND FteRifPri = 0  and datepart(month,FteData)=@NMESE )
--SET @FI=(Select Max(FteDataRicezione) FROM GEVE.DBO.TBFTE_PASSIVA WHERE FTEANNO = @ANNO AND FteRifPri = 0  and datepart(month,FteData)=@NMESE )

--REG:
--INSERT INTO [#TFTE](FePTipo,FePDal,FePAl,FePCh,FePPeriodo,FePImage,FePMese)
--select CAST(0 AS tinyint),@IN, @FI,DATEPART(MONTH,@IN), CONVERT(VARCHAR(20),@IN,103) +' AL '+ CONVERT(VARCHAR(20),@FI,103) ,-1,(select Okmese from @MeseRisc where Mn = DATEPART(MONTH,@IN))

--SET @NMESE = @NMESE + 1
--GOTO LOOPMESE

--- aggiorno date da registrare da registrare

III:
SELECT * FROM #TFTE


GO
/****** Object:  StoredProcedure [dbo].[XMERCEV]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[XMERCEV] @ANNO as SMALLINT, @MESE AS SMALLINT
as
SELECT IvaPcodIva,SUM(merce) as merce  INTO #TMP FROM vriepiva WHERE IvaPAnno =  @ANNO  AND merce > 0 AND IvaPMese < @MESE group by IvaPCodIva
UNION
SELECT IvaPcodIva,SUM(merce) as merce FROM vtmpriepiva WHERE IvaPAnno =  @ANNO  AND merce > 0  AND IvaPMese >= @MESE group by IvaPCodIva

select IvaPcodIva,SUM(merce) as merce  FROM #TMP  group by IvaPCodIva



GO
/****** Object:  StoredProcedure [dbo].[XMERCEVSIM]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[XMERCEVSIM] @ANNO as SMALLINT, @MESE AS SMALLINT
as
SELECT IvaPcodIva,SUM(merce) as merce  INTO #TMP FROM vriepiva WHERE IvaPAnno =  @ANNO  AND merce <> 0 AND IvaPMese < @MESE group by IvaPCodIva
UNION
SELECT IvaPcodIva,SUM(merce) as merce FROM vtmpriepiva WHERE IvaPAnno =  @ANNO  AND merce <> 0  AND IvaPMese >= @MESE group by IvaPCodIva

select IvaPcodIva,SUM(merce) as merce  FROM #TMP  group by IvaPCodIva




GO
/****** Object:  StoredProcedure [dbo].[XPIANORIENTRO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[XPIANORIENTRO]
AS
declare @NRATA AS INT,@DATA AS SMALLDATETIME,@RATASETT as decimal(10,2),@VALORERESIDUO DECIMAL(10,2),@TOTFAT as decimal(10,2),@TOTNC as decimal(10,2),@BLOCK as int,@DATACREA AS DATETIME
SET @TOTFAT=(select SUM(importo) from Rientro WHERE Importo>0)
SET @TOTNC=(select SUM(importo) from Rientro WHERE Importo<0)

INSERT INTO PIANO
SELECT * FROM BULKPiano

SET @DATACREA=GETDATE()

INSERT INTO TBPIR (IDDATA)
SELECT @DATACREA

set @BLOCK=(select @@IDENTITY)




DECLARE @DATAFATTURA AS SMALLDATETIME,@NFAT AS INT,@IMPORTOFAT AS DECIMAL(10,2),@P as int,@RESIDUOSUCC AS DECIMAL(10,2),@RESIDUO AS DECIMAL(10,2),@SW AS INT,@MAXRATA AS INT,@ID AS INT,@CCLI as varchar(5)
SET @P=0
SET @RESIDUOSUCC=0.00
SET @RESIDUO=0.00
SET @SW=0
SET @NRATA=0
SET @MAXRATA= (select MAX(NRATA) from Piano WHERE NRATA IS NOT NULL)
DELETE FROM TMPPIANO WHERE IDGRUPPO=@BLOCK

DECLARE DETTAGLIO CURSOR FOR
--select DISTINCT Data_fattura,fatture_note_credito,Importo from Rientro where Importo>0 order by Data_fattura
select Data_fattura,Documento,Importo,IDRIENTRO,CCli from Rientro WHERE Importo IS NOT NULL order by idrientro

OPEN DETTAGLIO
FETCH NEXT FROM DETTAGLIO
INTO @DATAFATTURA,@NFAT,@IMPORTOFAT,@ID,@CCLI
WHILE @@FETCH_STATUS = 0
BEGIN
SALTACURSORE:
IF @SW=0
BEGIN
SET @NRATA=@NRATA +1
IF @NRATA > @MAXRATA GOTO USCITA
SET @DATA =(select DataInizio from Piano where NRATA =@NRATA)
SET @RATASETT =(select rata_sett from Piano where NRATA =@NRATA)
END

IF @RESIDUOSUCC= 0 SET @RESIDUOSUCC=@RATASETT
IF @RESIDUO=0 SET @RESIDUO=@IMPORTOFAT
IF @RESIDUOSUCC > @RESIDUO
--
BEGIN
SET @P=@P+1
INSERT INTO TMPPIANO([IDGRUPPO],[IDRATA],[IDDATAFAT],[IDNRFAT],[IDRATACLI],[IDIMPORTORATA],[IDIMPORTOFAT],[IDRESIDUOCLI],IDCCLIE,DATACREAZIONE,IDSEQUENZA)
SELECT @BLOCK,@NRATA,@DATAFATTURA,@NFAT,@P,@RESIDUO,@IMPORTOFAT,0.00,@CCLI,@DATACREA,@ID

set @RESIDUOSUCC=@RESIDUOSUCC - @RESIDUO
set @P=0
SET @RESIDUO=0
IF @RESIDUOSUCC > 0 SET @SW=1
GOTO FINE_DETTAGLIO
END
--
IF @P=0 SET @RESIDUO=@IMPORTOFAT
IF @RESIDUOSUCC <= @RESIDUO
SET @RESIDUO= @RESIDUO - @RESIDUOSUCC
SET @P=@P+1
INSERT INTO TMPPIANO([IDGRUPPO],[IDRATA],[IDDATAFAT],[IDNRFAT],[IDRATACLI],[IDIMPORTORATA],[IDIMPORTOFAT],[IDRESIDUOCLI],IDCCLIE,DATACREAZIONE,IDSEQUENZA)
SELECT @BLOCK,@NRATA,@DATAFATTURA,@NFAT,@P,@RESIDUOSUCC,@IMPORTOFAT,@RESIDUO,@CCLI,@DATACREA,@ID
SET @RESIDUOSUCC=0
SET @SW=0
GOTO SALTACURSORE

FINE_DETTAGLIO:
   FETCH NEXT FROM DETTAGLIO
   INTO @DATAFATTURA,@NFAT,@IMPORTOFAT,@ID,@CCLI
END
USCITA:
CLOSE DETTAGLIO
DEALLOCATE DETTAGLIO

update TMPPIANO set DATASCADENZA=(select datainizio from piano where nrata=idrata),RATACONCORDATA=(select rata_sett from piano where nrata=idrata),
IDANAGRAFICA=(select AnaDesc from vdox.dbo.tbana where anacod=IDCCLIE) WHERE IDGRUPPO=@BLOCK 

--select *, DataScadenza=(select datainizio from piano where nrata=idrata),select datainizio from piano where nrata=idrata) 
--select SUM(idimportorata) from TMPPIANO
--SELECT * FROM PIANO
--select * from rientro
GO
/****** Object:  StoredProcedure [dbo].[XPRIQUO]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[XPRIQUO] @ANNO AS SMALLINT,@DATAOP as smalldatetime,@DATAGIO as smalldatetime,@CAUS as smallint,@DESCR as varchar(56),@NDOC as varchar(7)
AS

SELECT * into #TMPQUO FROM TBQUO inner join tbcesp ON QUONUM = cespNUM WHERE QUOANNO = @ANNO AND quoquota > 0

SELECT CESPCONTOQUOTA,QUOTE=SUM(QUOQUOTA),CESPCONTOFONDO,FONDO=SUM(QUOFONDO) INTO #TMP FROM #TMPQUO
group by CESPCONTOQUOTA,CESPCONTOFONDO

DECLARE @UA as int,@NEWID as int
DECLARE @CONTOQUO as VARCHAR(5),@CONTOFOND as VARCHAR(5),@QUOTA as DECIMAL(12,2),@FONDO as DECIMAL(12,2),@P AS SMALLINT,@PROG as int

set @UA = (select max(prinumprot) from tbpri) + 1

INSERT INTO TbIDP (IDdata) 
SELECT GETDATE()

SET @NEWID=(SELECT  @@IDENTITY)

SET @P = 0
SET @PROG = 0

SET @UA = @UA + 1

INIZIO_REG:
SET @P = @P + 1
IF @P > 2 GOTO FINE_REG

DECLARE SERGENTE CURSOR FOR
SELECT CESPCONTOQUOTA,QUOTE,CESPCONTOFONDO,FONDO from #tmp 
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @CONTOQUO,@QUOTA,@CONTOFOND,@FONDO
WHILE @@FETCH_STATUS = 0
BEGIN

SET @PROG = @PROG + 1

IF @P = 1
BEGIN

insert into tbpri (Priid,PriProg,PriDataGio,PriCausale,PriCoDare,PriCoAvere,PriNumProt,PriBisRet,PriCodIva,PriRegIva,PriImpDare,PriImpAvere,
	PriDesc,PriDocEst,PriMeseSk,PriDataEst,PriDescB,PriFl04,PriFl05,PriFl06,PriNsRif,PriSos,PriLinea,PriDocAnn,PriCodPag,PriValuta,PriArtFisc,
	PriIvaPrint,PriGStampa)

 select @NEWID,@PROG,@DATAGIO,@CAUS,@CONTOQUO,'00.10',@UA,'',0,0,@QUOTA,0,substring(@DESCR,1,24),0,'',@DATAOP,substring(@DESCR,25,32),0,0,0,@NDOC,'',0,
        @ANNO,0,0,0,0,0
	
END

IF @P = 2
BEGIN

insert into tbpri (Priid,PriProg,PriDataGio,PriCausale,PriCoDare,PriCoAvere,PriNumProt,PriBisRet,PriCodIva,PriRegIva,PriImpDare,PriImpAvere,
	PriDesc,PriDocEst,PriMeseSk,PriDataEst,PriDescB,PriFl04,PriFl05,PriFl06,PriNsRif,PriSos,PriLinea,PriDocAnn,PriCodPag,PriValuta,PriArtFisc,
	PriIvaPrint,PriGStampa)

 select @NEWID,@PROG,@DATAGIO,@CAUS,'00.10',@CONTOFOND,@UA,'',0,0,0,@QUOTA,substring(@DESCR,1,24),0,'',@DATAOP,substring(@DESCR,25,32),0,0,0,@NDOC,'',0,
        @ANNO,0,0,0,0,0
	
END
		 

FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @CONTOQUO,@QUOTA,@CONTOFOND,@FONDO
END
CLOSE SERGENTE
DEALLOCATE SERGENTE

GOTO INIZIO_REG

FINE_REG:

EXEC InitPrk  @NEWID


SELECT *,DAREDESC=ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRICODARE), '***ERRATO***'),
         AVEREDESC=ISNULL((SELECT PIAANACO FROM TBPIA WHERE PIACODCO = PRICOAVERE), '***ERRATO***'),
         Expr1=dbo.TbCii.CiiCau
FROM  TBPRI INNER JOIN TbCii ON dbo.TbCii.CiiCod = dbo.TbPri.PriCausale
WHERE    PRIID = @NEWID
ORDER BY PriId,PriProg



GO
/****** Object:  StoredProcedure [dbo].[XRECUPEROSCADENZE]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[XRECUPEROSCADENZE]  @MIGLIAIO as int
AS
DECLARE @IIDD AS INT
DECLARE DATISCA CURSOR FOR
SELECT priid from Tbpri where pricausale = 3 and Pricodpag > 0 AND PRIID > 2000
OPEN DATISCA
FETCH NEXT FROM DATISCA
INTO @IIDD
WHILE @@FETCH_STATUS = 0
BEGIN
 EXEC XCREASCADENZE  @IDP = @IIDD
-- EXEC RiChiudePartita  @Id = @IIDD ,@Miglio= @MIGLIAIO
FINE_LOOP:
   FETCH NEXT FROM DATISCA
   INTO @IIDD
END
CLOSE DATISCA
DEALLOCATE DATISCA
UPDATE TBSCA SET SCAABI = clabi,SCACAB = clcab FROM TBSCA Q, GEVE.DBO.TBCLI S WHERE Q.SCACONTO =S.clCod
UPDATE TBSCA SET SCAABI = foabi,SCACAB = focab FROM TBSCA Q, GEVE.DBO.TBFOR S WHERE Q.SCACONTO =S.FOCod
GO
/****** Object:  StoredProcedure [dbo].[XRegQuo]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE procedure [dbo].[XRegQuo]
@Data as smalldatetime,@TipoAmm as smallint,@AmmLib as decimal(13,2),
@DaCat as smallint, @ACat as smallint, @Num as int
as
declare
@CespNum as smallint,@CespAnnoA as smallint,@CespCat as smallint,@CespAliFis as decimal(5,2),@CespAliTab as decimal(5,2),
@QuoCoAmm as decimal(13,2),@QuoTipoAmm as smallint,@QuoAli as decimal(5,2),@QuoQuota as decimal(13,2),@QuoFondo as decimal(13,2),
@QuoResiduo as decimal(13,2),@VCespCaus as smallint,@VCespVariazioni as decimal(13,2),@CspCp as smallint,@QuoCoStor as decimal(13,2),
@QuoNoDetra as decimal(13,2),@CespDescr as varchar(31),
@Anno as smallint

set @Anno = datepart(year,@Data)
if @Num=0
begin
DECLARE Cespiti CURSOR FOR
select CespNum,CespDescr,CespAnnoA,CespCat,CespCp,CespAliFis,CespAliTab,CespCostoStorico,QuoCoAmmIni,QuoTipoAmm,QuoAli,QuoQuota,
QuoFondo,QuoResiduo,QuoNoDetra,VCespCaus,sum(isnull(VCespVariazioni,'0')) as VCespVariazioni 
from TbCesp inner join tbquo on Quonum=cespnum left outer join TbVCesp on VCespNum=QuoNum and VCespAnno=QuoAnno
where not CespDataFat > @Data and ISNULL(VCespCaus,'') <> 2  and ISNULL(VCespCaus,'') <> 3 
--and QuotipoAmm<> 3
 and QuoAnno=@Anno and CespCat >= @DaCat and CespCat <= @ACat and CespUltAnnoAmm<@Anno
group by CespNum,CespDescr,CespAnnoA,CespCat,CespCp,CespAliFis,CespAliTab,CespCostoStorico,QuoCoAmmIni,QuoTipoAmm,QuoAli,QuoQuota,QuoFondo,QuoResiduo,QuoNoDetra,VCespCaus
order by CespCat,CespAnnoA,CespNum
end
else
begin
DECLARE Cespiti CURSOR FOR
select CespNum,CespDescr,CespAnnoA,CespCat,CespCp,CespAliFis,CespAliTab,CespCostoStorico,QuoCoAmmIni,QuoTipoAmm,QuoAli,QuoQuota,
QuoFondo,QuoResiduo,QuoNoDetra,VCespCaus,sum(isnull(VCespVariazioni,'0')) as VCespVariazioni 
from TbCesp inner join tbquo on Quonum=cespnum left outer join TbVCesp on VCespNum=QuoNum and VCespAnno=QuoAnno
where not CespDataFat > @Data and ISNULL(VCespCaus,'') <> 2 and ISNULL(VCespCaus,'') <> 3 
--and QuotipoAmm<> 3
 and QuoAnno=@Anno and CespCat >= @DaCat and CespCat <= @ACat and CespUltAnnoAmm<@Anno and CespNum=@Num
group by CespNum,CespDescr,CespAnnoA,CespCat,CespCp,CespAliFis,CespAliTab,CespCostoStorico,QuoCoAmmIni,QuoTipoAmm,QuoAli,QuoQuota,QuoFondo,QuoResiduo,QuoNoDetra,VCespCaus
order by CespCat,CespAnnoA,CespNum
end

select top 1 CespNum,CespCat,CespAnnoA,QuoTipoAmm,CespAliTab,QuoCoAmm,
cast(0 as decimal(13,2)) as Quota, cast(0 as decimal(13,2)) as Fondo, cast(0 as decimal(13,2)) as Residuo,cast(0 as decimal(13,2)) as QuotaND,CespDescr
into #tmp from TbCesp inner join tbquo on Quonum=cespnum left outer join TbVCesp on VCespNum=QuoNum and VCespAnno=QuoAnno
where not CespDataFat > @Data and ISNULL(VCespCaus,'') <> 2 
and QuotipoAmm<> 3 order by CespNum

delete from #tmp

OPEN Cespiti
FETCH NEXT FROM Cespiti
INTO @CespNum,@CespDescr,@CespAnnoA,@CespCat,@CspCp,@CespAliFis,@CespAliTab,@QuoCoStor,@QuoCoAmm,@QuoTipoAmm,@QuoAli,@QuoQuota,@QuoFondo,@QuoResiduo,@QuoNoDetra,@VCespCaus,@VCespVariazioni
WHILE @@FETCH_STATUS = 0
BEGIN

Declare @PerAmm as decimal(13,2), @PerAnt as decimal(13,2),@Ammortam as decimal(13,2),@PerND as decimal(13,2),
        @Anticip as decimal(13,2),@Meta as decimal(13,2),@WsSaldo as decimal(13,2),@AnnoDiff as smallint,@PerLim as decimal(13,2),
	@AmmND as decimal(13,2),@UltND as decimal(13,2),@UltFondo as decimal(13,2),@Annopi as smallint

set @Annopi=sum(@Anno +1)

if @CespAliFis>0
      begin
        set @PerAmm=@CespAliFis
      end
    else
      begin
        set @PerAmm=@CespAliTab
      end

set @PerAnt=0
set @UltFondo = @QuoFondo
set @UltND = @QuoNoDetra
set @Anticip=0
--set @QuoTipoAmm=@TipoAmm
set @QuoCoAmm = sum(@QuoCoAmm + isnull(@VCespVariazioni,'0'))
--set @QuoResiduo=sum(@QuoCoAmm - @QuoFondo - @QuoNoDetra)
set @QuoResiduo=sum(@QuoCoAmm - @QuoFondo )


if @QuoTipoAmm=3
   begin
     set @Ammortam = 0
     set @Anticip=0
     set @QuoNoDetra=0
     set @QuoQuota=0
     set @QuoResiduo=@QuoCoAmm
   end
else
   begin
   set @QuoTipoAmm=@TipoAmm
   if @QuoTipoAmm=3
     begin
       set @Ammortam = 0
       set @Anticip=0
       set @QuoNoDetra=0
       set @QuoQuota=0
       set @QuoResiduo=@QuoCoAmm
     end
   if @QuoTipoAmm = 4 or @CspCp=1
      begin 
	set @ammortam=0
              if @CespAnnoA=@Anno
	  begin
	    set @Ammortam=@QuoCoAmm
	  end
	if @CspCp=0
	  begin
	    set @ammortam=@Quocoamm
	  end
	else
	  begin
	    if @quoResiduo>0 or @CespAnnoA=@Anno
	      begin
		set @ammortam=sum(@QuoCoAmm * @PerAmm / 100)
	      end
	  end
	if @Ammortam>@QuoResiduo
	  begin
	   set @Ammortam=@QuoResiduo
	  end
	set @QuoTipoAmm=0
      end 
   else
      begin 
	if @CespAnnoA=@Anno
	  begin 
    	      if @QuoFondo=@QuoCoamm
                begin
                  set @PerAmm=0
		  set @PerAnt=0
		  set @Ammortam=0
		  set @Anticip=0
		  SET @QuoTipoAmm=0
	        end
	      else
		begin
		  set @PerAmm=sum(@PerAmm/2)
		  set @PerAnt=@PerAmm
		  set @Ammortam=sum(@QuoCoAmm * @PerAmm / 100)
		  if @QuoTipoAmm=1
		    begin
		      set @Anticip=sum(@QuoCoAmm * @PerAnt / 100)
		    end
		  else
		    begin
		      set @anticip=0
		      set @PerAnt=0
		    end
		  if @QuoTipoAmm=2
		    begin
		      set @PerND=@PerAmm/2
		      set @PerLim=@AmmLib--sum(@PerAmm * @AmmLib / 100)
		      set @PerAmm=@AmmLib
		      set @ammortam=sum(@PerLim * @QuoCoAmm / 100)
		      set @QuoNoDetra=0
		      if @PerLim < @PerND
			begin
			  set @AmmND=sum(@PerNd - @PerLim)
			  set @QuoNoDetra=sum(@UltND+@AmmND*@QuoCoAmm/100)
			end
		    end
		end
	   end
        else

        begin
            if not @quoResiduo > 0.00
              begin
	        set @Ammortam = 0.00
	        set @Anticip = 0.00
	        set @PerAmm = 0.00
		set @PerAnt = 0.00
              end
            else
              begin
                set @AnnoDiff = sum(@Anno-@CespAnnoA)
                if @CespAnnoA<1988
                  begin
	            if @AnnoDiff > 2
	              begin
	                set @PerAnt = 0.00
			set @QuoTipoAmm=0
	              end
	            else
	              begin
	                set @PerAnt = 15
			set @QuoTipoAmm=1
	              end
	          end
                else
                  begin
                    if @AnnoDiff > 2
	              begin
	                set @PerAnt = 0.00
	              end
	            else
	              begin
	                set @PerAnt = @PerAmm
	              end
	          end
            
        set @Ammortam = sum(@QuoCoAmm * @PerAmm / 100)
	--set @WsSaldo = sum(@QuoCoAmm-@QuoFondo-@QuoNoDetra)
	set @WsSaldo = sum(@QuoCoAmm-@QuoFondo)

	if @QuoTipoAmm=1
	  begin
	    set @Anticip=sum(@QuoCoAmm * @PerAnt / 100)
	       if @AnnoDiff > 2
		begin
		  set @QuoTipoAmm=0
		end
	  end
	else
	  begin
	    set @Anticip=0
	    set @PerAnt=0
	  end

	if @QuoTipoAmm=2
	  begin
	    set @PerND=sum(@PerAmm / 2)
	    set @PerLim=@AmmLib--sum(@PerAmm * @AmmLib / 100)
	    set @PerAmm=@AmmLib
	    set @Ammortam=sum(@QuoCoAmm * @PerLim / 100)
	  end
	                
        if @Ammortam > @WsSaldo
          begin
	    set @Ammortam  = @WsSaldo
	    set @Anticip = 0.00
          end
        
      set @WsSaldo = sum(@WsSaldo - @Ammortam)

      if @Anticip > @WsSaldo
        begin
	  set @Anticip  = @WsSaldo
        end

        end
    end
  end
  set @QuoAli=sum(@PerAmm+@PerAnt)
  set @QuoQuota=sum(@Ammortam+@Anticip)
  set @QuoFondo=sum(@QuoQuota+@UltFondo)
  --set @QuoResiduo=sum(@QuoCoAmm - @QuoFondo - @QuoNoDetra)
    set @QuoResiduo=sum(@QuoCoAmm - @QuoFondo )

end

if @QuoQuota=0
  begin
    set @QuoAli=0
    set @PerAmm=0
end

if @QuoQuota = 0 and @QuoTipoAmm = 2
  begin
   set @QuoTipoAmm = 0 
end
   

insert into #tmp (CespNum,CespCat,CespAnnoA,QuoTipoAmm,CespAliTab,QuoCoAmm,Quota,Fondo,Residuo,QuotaND,CespDescr) 
          values (@CespNum,@CespCat,@CespAnnoA,@QuoTipoAmm,@QuoAli,@QuoCoAmm,@QuoQuota,@QuoFondo,@QuoResiduo,@QuoNoDetra,@CespDescr)

Update TbQuo set QuocoStor=@QuoCoStor,QuoCoamm=@QuoCoAmm,QuoTipoamm=@QuoTipoAmm,QuoAli=@QuoAli,QuoQuota=@QuoQuota,
		QuoFondo=@QuoFondo,QuoResiduo=@QuoResiduo,QuoNoDetra=@QuoNoDetra
where QuoNum=@CespNum and QuoAnno=@Anno

delete from TbQuo where QuoAnno=@Annopi and QuoNum=@CespNum
insert into TbQuo (QuoNum,QuoAnno,QuoCoStorIni,QuocoammIni,QuofondoIni,QuoResiduoIni,QuoNoDetraIni,
			QuocoStor,QuoCoamm,QuoTipoamm,QuoAli,QuoQuota,QuoFondo,QuoResiduo,QuoNoDetra)
		values(@CespNum,@Annopi,@QuoCoStor,@QuoCoAmm,@QuoFondo,@QuoResiduo,@QuoNoDetra,
			@QuoCoStor,@QuoCoAmm,'0','0','0',@QuoFondo,@QuoResiduo,@QuoNoDetra)

Update TbCesp set CespUltAnnoAmm=@Anno where CespNum=@CespNum

   FETCH NEXT FROM Cespiti
   INTO @CespNum,@CespDescr,@CespAnnoA,@CespCat,@CspCp,@CespAliFis,@CespAliTab,@QuoCoStor,@QuoCoAmm,@QuoTipoAmm,@QuoAli,@QuoQuota,@QuoFondo,@QuoResiduo,@QuoNoDetra,@VCespCaus,@VCespVariazioni
END
CLOSE Cespiti
DEALLOCATE Cespiti


GO
/****** Object:  StoredProcedure [dbo].[XSCORRIS]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[XSCORRIS] @DAL as smalldatetime,@al as smalldatetime,@NREG AS SMALLINT
as
if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##XSCO'))
   begin
   drop table ##XSCO
  end   
CREATE TABLE ##XSCO (
       	[CONTO] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[IVA] [decimal](13, 2) NOT NULL,
        [TOTALEIVA] [decimal](13, 2) NOT NULL,
	) ON [PRIMARY]
DECLARE @TOTIVA AS DECIMAL(13,2)
DECLARE @FINEMESE AS SMALLINT


SET @FINEMESE = (select MAX(CORRMESE)
from TBCORR WHERE CORRANNO = DATEPART(YEAR,@AL) AND CORRREGIVA = @NREG AND CORRMESE BETWEEN  DATEPART(month,@DAL) AND DATEPART(month,@AL))

IF DATEPART(month,@AL)>@FINEMESE
BEGIN
select DISTINCT CORRANNO,CORRREGIVA,CORRMESE,CORRCODIVA,CORRCONTO,CORRLORDO
INTO #TMP 
from TBCORR WHERE CORRANNO = DATEPART(YEAR,@AL) AND CORRREGIVA = @NREG AND CORRMESE =@FINEMESE 

INSERT INTO tbcorr(corranno,corrregiva,corrmese,corrcodiva,corrconto,corrlordo)
select corranno,corrregiva,DATEPART(month,@AL),corrcodiva,corrconto,0 FROM #TMP
END 



select DISTINCT CORRANNO,CORRREGIVA,CORRMESE,CORRCONTO,SUM(CORRLORDO) as CORRLORDO,IVAPIMPON,IVAPIVADE,CORRCODIVA,
 (SUM(CORRLORDO) * 100 /(IVAPIMPON+IVAPIVADE))as PERC,(ivapivade * (SUM(CORRLORDO) * 100 /(IVAPIMPON+IVAPIVADE))) / 100 as IVA
INTO #TP
from TBCORR inner join TBIVAP 
ON CORRANNO = IVAPANNO AND CORRREGIVA = IVAPREGIVA AND CORRMESE = IVAPMESE AND CORRCODIVA = IVAPCODIVA
WHERE CORRANNO = DATEPART(YEAR,@AL) AND CORRREGIVA = @NREG AND CORRMESE BETWEEN  DATEPART(month,@DAL) AND DATEPART(month,@AL)
GROUP BY CORRANNO,CORRREGIVA,CORRMESE,CORRCONTO,CORRCODIVA,IVAPIMPON,IVAPIVADE
union
select DISTINCT CORRANNO,CORRREGIVA,CORRMESE,CORRCONTO,SUM(CORRLORDO) as CORRLORDO,IVAVNETTI,IVAVIVA,CORRCODIVA,
 (SUM(CORRLORDO) * 100 /(IVAVNETTI+IVAVIVA))as PERC,IVAVIVA 
from TBCORR inner join TBIVAV 
ON CORRANNO = IVAVANNO AND CORRREGIVA = IVAVREGIVA AND CORRMESE = IVAVMESE
WHERE CORRANNO = DATEPART(YEAR,@AL) AND CORRREGIVA = @NREG AND CORRMESE BETWEEN  DATEPART(month,@DAL) AND DATEPART(month,@AL) 
GROUP BY CORRANNO,CORRREGIVA,CORRMESE,CORRCONTO,CORRCODIVA,IVAVNETTI,IVAVIVA

SET @TOTIVA = (SELECT SUM(DISTINCT IVAPIVADE) FROM #TP)
INSERT INTO ##XSCO ([CONTO],[IVA],[TOTALEIVA])
SELECT DISTINCT CORRCONTO,CAST(SUM(IVA) AS DECIMAL(12,2)),@TOTIVA FROM #TP GROUP BY CORRCONTO
DROP TABLE #TP







GO
/****** Object:  StoredProcedure [dbo].[XSPESEM2012]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO





CREATE procedure [dbo].[XSPESEM2012] @ANNO AS smallint
as

select PriDataGio,PriDataEst,PriNumProt,PriBisRet,PriDocest,
CLoFO = case when pricausale = 2 then 'FO' else 'CL' end,
Codice = case when pricausale = 2 then Pricoavere else  PricoDare end,
PIVA = CASE WHEN pricausale = 1 then 
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL' ),'* ERRATO *')
else
ISNULL((SELECT ANAPIVA FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),'* ERRATO *')
END,
CFISC=CASE WHEN pricausale = 1 then 
ISNULL((SELECT ANACFIS FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoDare AND ANAGRP = 'CL' ),'* ERRATO *')
else
ISNULL((SELECT ANACFIS FROM VDOX.DBO.TBANA WHERE ANACOD = PriCoAvere AND ANAGRP = 'FO'),'* ERRATO *')
END,
PriImpdare,PriCodIva,
CIITP=isnull((select CiiTp from TbCii where CiiCod = PriCodIva),''),
IVA=CASE WHEN pricausale < 3 then PriImpAvere else 0 end,
PriRegIva,PriId,PriProg,Nc= cast(0 as bit),Nf =cast(0 as smallint)
into #tmp
 from Tbpri where 
PRIREGIVA >0 
AND datepart(year,pridatagio) = @ANNO
AND pricausale < 3 
ORDER BY PRIREGIVA,PRINUMPROT,PRIBISRET,PRIID,PRIPROG DESC

DELETE FROM [TbEleCf] WHERE EleCfAnno = @ANNO

INSERT INTO[dbo].[TbEleCf]([EleCfAnno],[EleCfTipo],[EleCfCodice],[EleCfAnaDesc],[EleCfCpt],[EleCfPiaDesc],[EleCfDataDoc],[EleCfNumDoc],
	[EleCfImponibile],[EleCfIva],[EleCfCodiceIva],[EleCfDescIva],[EleCfPriCodIva],[EleCfPriId],[EleCfPriProg],[EleCfP],[EleCfPriRegIva],
	[EleCfPerc],[EleCfTipImp],[EleCfUnion],[EleCfRegistrazione],[EleCfModPag],[EleCfVariazione])
	
	select @ANNO,CLoFO,Codice,(select Anadesc from vdox.dbo.tbana Where anacod = Codice and Anagrp = CLoFO),'00.00','',PriDataEst,PriDocEst,
	PriImpDare,IVA,PRICODIVA,ISNULL((select CiiDes from TbCii where Pricodiva = CiiCod),''),PriCodIva,PriId,PriProg,0,PriRegIva,ISNULL((select CiiAli from TbCii where Pricodiva = CiiCod),''),
	CiiTp,0,pridatagio,0,0 FROM #TMP

select PriId into #NC from tbpri where pricausale = 3 and datepart(year,pridatagio) = @ANNO AND PriBisRet ='' and (priimpdare < 0 or PriImpAvere < 0)
select PriIdX = PRIID into #NF from tbpri where pricausale = 3 and datepart(year,pridatagio) = @ANNO AND PriBisRet ='' 

update #tmp set nc = 1 where priid in(select priid from #NC)

select DISTINCT clofoX=CLOFO,codicex=CODICE,NFx=COUNT(DISTINCT PRIID),NCX=nc into #PPX FROM #tmp INNER JOIN #NF ON priid = priidx group by clofo,codice,NC

---
DECLARE @P AS SMALLINT
II:
select priid as IID,priprog as PP , ciitp as TP INTO #TMP2 from #tmp  WHERE ciitp = 0 order by PRIID, PRIPROG desc
set @P=(SELECT COUNT(*) FROM #TMP2)
if @P = 0 goto III
UPDATE #TMP2 SET TP = ISNULL((select CIITP FROM #TMP WHERE IID= PRIID AND PRIPROG = PP + 1 and CIITP > 0),0)
UPDATE #TMP SET ciitp = ISNULL((select TP FROM #TMP2 WHERE IID= PRIID AND PRIPROG = PP AND TP > 0),0) WHERE ciitp = 0
drop table #tmp2
GOTO II
III:
drop table #tmp2
---


select DISTINCT CLoFO,PIVA,CFISC,Codice,
IMPONIBILE = CASE WHEN CIITP = 1 THEN SUM(PRIIMPDARE) ELSE 0 END,
IVA = CASE WHEN CIITP = 1 THEN SUM(IVA) ELSE 0 END,
NONIMP =  CASE WHEN CIITP = 2 THEN SUM(PRIIMPDARE) ELSE 0 END,
ESENTI= CASE WHEN CIITP = 3 THEN SUM(PRIIMPDARE) ELSE 0 END,
ALTRI = CASE WHEN CIITP > 3 THEN SUM(PRIIMPDARE) ELSE 0 END,
NC,NF
	into #TMP3
FROM #TMP where CIITP BETWEEN 1 AND 9 ----AND ISNUMERIC(PIVA) = 1
GROUP BY CLoFO,Codice,Piva,CFISC,CIITP,NC,NF



SELECT DISTINCT CLoFO,Codice,PIVA,CFISC,
IMPONIBILE=SUM(IMPONIBILE),
IVA=SUM(IVA),
NONIMP=SUM(NONIMP),
ESENTI=SUM(ESENTI),
ALTRI=SUM(ALTRI),NC,NF
INTO #TMP4
 FROM #TMP3 GROUP BY CLoFO,Codice,PIVA,CFISC,NC,NF
ORDER BY CLoFO,Codice,PIVA,CFISC,NC,NF

UPDATE #TMP4 SET NF = NFX  FROM #TMP4,#PPX where clofox=CLoFO and CodiceX=codice and ncX = nc

SELECT CLoFO,Codice,PIVA,CFISC,IMPONIBILE,IVA,NONIMP,ESENTI,ALTRI,ANAG = (SELECT TOP 1 ANADESC FROM VDOX.DBO.TBANA WHERE Codice = anacod AND ANAGRP = CLoFO),
NC,NF
INTO #TMP5
FROM #TMP4

DELETE FROM TbRieCf WHERE RieCfAnno = @ANNO

INSERT INTO [dbo].[TbRieCf]([RieCfAnno],[RieCfTipo],[RieCfCodice],[RieCfPiva],[RieCfCFis],[RieCfImponibile],[RieCfIva],
	[RieCfNonImp],[RieCfEsente],[RieCfAltri],[RieCfAnadesc],[RieCfTotale],[RieCfNCredito],[RieCfNFatture])
SELECT @ANNO,CLoFO,Codice,PIVA,CFISC,IMPONIBILE,IVA,NONIMP,ESENTI,ALTRI,ANAG,(IMPONIBILE + IVA + NONIMP + ESENTI + ALTRI),NC,NF FROM #TMP5 
where ((IMPONIBILE <> 0 ) OR 
           (NONIMP <> 0 ) OR 
           (ESENTI <> 0 ) OR 
           (ALTRI  <> 0 ) OR
              (IVA <> 0 ))
 ORDER BY CLoFO, ANAG


drop table #tmp
drop table #tmp3
drop table #tmp4
drop table #tmp5


-- ELIMINO INTRACEE E BLACK LIST
SELECT DISTINCT ANACOD INTO #PI FROM VDOX.dbo.TbAna WHERE AnaPivaEst>'' and AnaPivaEst<>'.' 

DELETE FROM [dbo].[TbEleCf] WHERE EleCfCodice IN (SELECT ANACOD FROM #PI) AND EleCfAnno = @ANNO
DELETE FROM [dbo].[TbEleCf] WHERE EleCfCodice IN (SELECT Clcod FROM geve.dbo.tbcli where ClBlackList = 1) AND EleCfAnno = @ANNO
DELETE FROM [dbo].[TbEleCf] WHERE EleCfCodice IN (SELECT Focod FROM geve.dbo.tbfor where FoBlackList = 1) AND EleCfAnno = @ANNO

DELETE FROM [dbo].[TbRieCf] WHERE RieCfCodice IN (SELECT ANACOD FROM #PI) AND RieCfAnno = @ANNO
DELETE FROM [dbo].[TbRieCf] WHERE RieCfCodice IN (SELECT Clcod FROM geve.dbo.tbcli where ClBlackList = 1) AND RieCfAnno = @ANNO
DELETE FROM [dbo].[TbRieCf] WHERE RieCfCodice IN (SELECT Focod FROM geve.dbo.tbfor where FoBlackList = 1) AND RieCfAnno = @ANNO

DELETE FROM [dbo].[TbRieCf] WHERE (RieCfImponibile = 0 and RieCfNonImp = 0 and RieCfEsente = 0) AND RieCfAnno = @ANNO

--SELECT RieCfCodice INTO #AA FROM TbRieCf WHERE RieCfAltri <> 0 and (RieCfImponibile = 0 and RieCfNonImp = 0 and RieCfEsente = 0) AND RieCfAnno = @ANNO

DELETE FROM [dbo].[TbEleCf] WHERE EleCfCodice NOT IN (SELECT RieCfCodice FROM [TbRieCf] WHERE RieCfAnno = @ANNO) AND EleCfAnno = @ANNO
--DELETE FROM [dbo].[TbRieCf] WHERE RieCfCodice IN (SELECT RieCfCodice FROM #AA) AND RieCfAnno = @ANNO
GO
/****** Object:  StoredProcedure [dbo].[XSUMP]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XSUMP] @ANNO as SMALLINT, @DMESE AS SMALLINT,@AMESE as smallint,@REG as smallint
as
select * into #tmp from VRiepiva  WHERE IvaPAnno =  @ANNO  AND IvaPMese between @DMESE and @AMESE 
SELECT  distinct top 100 percent IvaPAnno, max(IvaPMese) as IvaPMese,IvaPRegIva,IvaPCodIva, SUM(Timpon) AS Timpon,
                       SUM(IvaDe) AS IvaDe, SUM(IvaNd) AS IvaNd, SUM(Merce) AS Merce, CiiDes,CiiAli
 FROM #tmp                    
WHERE IvaPAnno=@ANNO AND IvaPRegIva = @REG and IvaPMese between @DMESE and @AMESE 
group by IvaPAnno,IvaPRegIva,IvaPCodIva,CiiDes,CiiAli
ORDER BY IvaPAnno,IvaPRegIva,IVAPmese,IvaPCodIva,CiiDes,CiiAli
GO
/****** Object:  StoredProcedure [dbo].[XVENTILA]    Script Date: 20/05/2026 10:24:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[XVENTILA] @ANNO as SMALLINT
AS

CREATE TABLE [dbo].[#TbIvaV] (
	[IvaVAnno] [smallint] NOT NULL ,
	[IvaVRegIva] [smallint] NOT NULL ,
	[IvaVCodIva] [smallint] NOT NULL ,
	[IvaVAcLordi] [decimal](13, 2) NOT NULL ,
	[IvaVPerComp] [decimal](13, 10) NOT NULL ,
	[IvaVLordi] [decimal](13, 2) NOT NULL ,
	[IvaVCMPIva] [smallint] NOT NULL ,
	[IvaVNetti] [decimal](13, 2) NOT NULL ,
	[IvaVIva] [decimal](13, 2) NOT NULL 
) ON [PRIMARY]

DECLARE @TM AS DECIMAL(13,2),@PS AS DECIMAL(13,10),@AA AS DECIMAL(13,2),@LORDOC AS DECIMAL(13,2),@MERCE AS DECIMAL(13,2)
DECLARE @MESE AS SMALLINT,@REGIVA AS SMALLINT,@PE AS SMALLINT,@IM as SMALLINT,@ALIQ as smallint
DECLARE @CORRN as decimal(13,2),@CORRI as decimal(13,2)

SET @MESE = (SELECT Max(IvaVmese) FROM TbIvaV where IvaVAnno = @ANNO AND IvaVmese < 13)

SET @MERCE  =(SELECT SUM(IvaVAcLordi) FROM TbIvaV where IvaVAnno = @ANNO and IvaVMese =@MESE)
SET @LORDOC =(SELECT SUM(IvaVLordi) FROM TbIvaV where IvaVAnno = @ANNO )

DECLARE SERGENTE CURSOR FOR
SELECT distinct IvaVCodIva,Sum(IvaVAcLordi),IvaVRegiva, IvaVCmpIva from TbIvaV where IvaVAnno = @ANNO and IvaVMese =@MESE group by IvaVCmpIva,IvaVRegiva,IvaVCodIva
OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @IM,@TM,@REGIVA,@PE
WHILE @@FETCH_STATUS = 0
BEGIN
SET @PS = ( @TM / @MERCE ) * 100
SET @AA = @LORDOC * @PS / 100
SET @ALIQ = (select CiiAli from Tbcii where CiiCod = @PE)

set @CORRN = @AA / (100 + @ALIQ) * 100
SET @CORRI = @AA - @CORRN
INSERT INTO [dbo].[#TbIvaV] (IvaVAnno,IvaVRegIva,IvaVCodIva,IvaVAcLordi,IvaVPerComp,IvaVLordi,
IvaVCMPIva,IvaVNetti,IvaVIva)
SELECT @ANNO,@REGIVA,@IM,@TM,@PS,@AA,@ALIQ,@CORRN,@CORRI
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @IM,@TM,@REGIVA,@PE
END
CLOSE SERGENTE
DEALLOCATE SERGENTE
select * from #TbIvaV




GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0= Anno Prec; 13=Annuale; 14=Acconto; <n>=mese' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TbVers', @level2type=N'COLUMN',@level2name=N'IvaVMese'
GO
USE [master]
GO
ALTER DATABASE [COGE] SET  READ_WRITE 
GO
