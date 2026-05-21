USE [master]
GO
/****** Object:  Database [VDOX]    Script Date: 20/05/2026 15:38:17 ******/
CREATE DATABASE [VDOX]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VDOX_Data', FILENAME = N'C:\DATI\DBCLIENTI\RACCA\VDOX.mdf' , SIZE = 4096000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'VDOX_Log', FILENAME = N'C:\DATI\DBCLIENTI\RACCA\VDOX_1.ldf' , SIZE = 1024000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [VDOX] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VDOX].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [VDOX] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VDOX] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VDOX] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VDOX] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VDOX] SET ARITHABORT OFF 
GO
ALTER DATABASE [VDOX] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [VDOX] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VDOX] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VDOX] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VDOX] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VDOX] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VDOX] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VDOX] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VDOX] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VDOX] SET  DISABLE_BROKER 
GO
ALTER DATABASE [VDOX] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VDOX] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VDOX] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VDOX] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VDOX] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VDOX] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [VDOX] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VDOX] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [VDOX] SET  MULTI_USER 
GO
ALTER DATABASE [VDOX] SET PAGE_VERIFY TORN_PAGE_DETECTION  
GO
ALTER DATABASE [VDOX] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VDOX] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VDOX] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [VDOX] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [VDOX] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [VDOX] SET QUERY_STORE = OFF
GO
USE [VDOX]
GO
/****** Object:  User [REDACTED_DB_USER_1]    Script Date: REDACTED ******/
-- REDACTED: database user omitted from documentation export
GO
/****** Object:  Schema [REDACTED_DB_SCHEMA_1]    Script Date: REDACTED ******/
-- REDACTED: database schema/user placeholder omitted from documentation export
GO
/****** Object:  UserDefinedFunction [dbo].[FnHHMMSS]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[FnHHMMSS] (@V AS INT)
returns VARCHAR(12)
as
BEGIN
DECLARE @H AS INT,@M AS INT,@S AS INT
DECLARE @TEMPO AS VARCHAR(12)
SET @H = @V/3600
SET @M = (@V/60)- (@H*60)
SET @S = @V -(@H*3600 + @M*60)
SET @TEMPO = CAST(@H AS VARCHAR(6))+':'+REPLICATE('0', 2 - DATALENGTH(CAST(@M AS VARCHAR(2))))+CAST(@M AS VARCHAR(2))+':'+REPLICATE('0', 2 - DATALENGTH(CAST(@S AS VARCHAR(2))))+CAST(@S AS VARCHAR(2))

RETURN (@TEMPO)
END


GO
/****** Object:  UserDefinedFunction [dbo].[FnMINUTI]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[FnMINUTI] (@V AS VARCHAR(12))
returns INT
as
BEGIN
IF DATALENGTH(@v) = 0 RETURN 0
DECLARE @H AS INT,@M AS INT,@S AS INT
DECLARE @TEMPO AS INT
SET @H = CAST(SUBSTRING(@V,1,(DATALENGTH(@V)-6)) AS INT)
SET @M = CAST(SUBSTRING(@V,(DATALENGTH(@V)-4),2) AS INT)
SET @S = CAST(SUBSTRING(@V,(DATALENGTH(@V)-1),2) AS INT)
SET @TEMPO = @H*60+@M
RETURN (@TEMPO)
END

GO
/****** Object:  UserDefinedFunction [dbo].[FnSECONDI]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[FnSECONDI] (@V AS VARCHAR(12))
returns INT
as
BEGIN
IF DATALENGTH(@v) = 0 RETURN 0
DECLARE @H AS INT,@M AS INT,@S AS INT
DECLARE @TEMPO AS INT
SET @H = CAST(SUBSTRING(@V,1,(DATALENGTH(@V)-6)) AS INT)
SET @M = CAST(SUBSTRING(@V,(DATALENGTH(@V)-4),2) AS INT)
SET @S = CAST(SUBSTRING(@V,(DATALENGTH(@V)-1),2) AS INT)
SET @TEMPO = @H*3600+@M*60+@S
RETURN (@TEMPO)
END

GO
/****** Object:  Table [dbo].[TbAna]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAna](
	[AnaProg] [int] IDENTITY(1,1) NOT NULL,
	[AnaDesc] [varchar](90) NOT NULL,
	[AnaPiva] [varchar](11) NOT NULL,
	[AnaCfis] [varchar](16) NOT NULL,
	[AnaIndirizzo] [varchar](50) NOT NULL,
	[AnaCap] [varchar](5) NOT NULL,
	[AnaCitta] [varchar](50) NOT NULL,
	[AnaProv] [varchar](2) NOT NULL,
	[AnaTel1] [varchar](20) NOT NULL,
	[AnaTel2] [varchar](20) NOT NULL,
	[AnaTel3] [varchar](20) NOT NULL,
	[AnaWww] [varchar](50) NOT NULL,
	[AnaEmail] [varchar](50) NOT NULL,
	[AnaGrp] [varchar](2) NOT NULL,
	[AnaCod] [varchar](5) NOT NULL,
	[AnaResp] [varchar](50) NOT NULL,
	[AnaNote] [varchar](3000) NOT NULL,
	[AnaFax] [varchar](20) NOT NULL,
	[AnaRag1] [varchar](45) NOT NULL,
	[AnaRag2] [varchar](45) NOT NULL,
	[AnaPivaEst] [varchar](20) NULL,
	[AnaNoRubrica] [bit] NOT NULL,
 CONSTRAINT [PK_TbAna] PRIMARY KEY CLUSTERED 
(
	[AnaProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vrubrica]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[vrubrica] 
as

select AnaProg,AnaDesc,AnaTel1,AnaTel2,AnaTel3,AnaFax,AnaEmail,ID=0,AnaDesc as Azienda,OS=0
from TbAna
where AnaNoRubrica = 0

UNION ALL

select PhProg,PhDesc,PhTel1,PhTel2,Phtel3,PhFax,PhEmail,ID=PhId,PhAzienda,OS=1
from NewPhone






GO
/****** Object:  Table [dbo].[TbAnn]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAnn](
	[AnnAnno] [char](4) NOT NULL,
	[AnnProg] [int] NOT NULL,
	[AnnStor] [char](1) NULL,
 CONSTRAINT [PK_TbAnn] PRIMARY KEY CLUSTERED 
(
	[AnnAnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPTbDoc]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPTbDoc](
	[TMPDocBLOCK] [int] NOT NULL,
	[TMPDocAnno] [char](4) NOT NULL,
	[TMPDocTipo] [char](2) NOT NULL,
	[TMPDocRifInterno] [char](6) NOT NULL,
	[TMPDocEst] [char](3) NOT NULL,
	[TMPDocAnaProg] [int] NOT NULL,
	[TMPDocData] [smalldatetime] NOT NULL,
	[TMPDocDesc] [varchar](80) NOT NULL,
	[TMPDocNote] [varchar](16) NOT NULL,
	[TMPDocNumRif] [int] NULL,
	[TMPDocProt] [varchar](7) NOT NULL,
	[TMPDocReg] [smallint] NOT NULL,
	[TMPDocAnnoCoge] [smallint] NOT NULL,
	[TMPDocKeyCm] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VMONDODETT]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create VIEW [dbo].[VMONDODETT] 
AS
SELECT *,
DocPath=TMPDocAnno + '\' + TMPDocTipo + TMPDocRifInterno + '.' + TMPDocEst,
TMPAnnStor=ISNULL(AnnStor, '')
 FROM TMPTBDOC 
 INNER JOIN TBTDO ON TMPDocTipo = TdoCod
 INNER JOIN TbAnn ON TMPDocAnno = AnnAnno

GO
/****** Object:  Table [dbo].[TbGrupLav]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbGrupLav](
	[GrupLavId] [int] IDENTITY(1,1) NOT NULL,
	[GrupLavNome] [varchar](30) NOT NULL,
	[GrupLavDesc] [varchar](50) NOT NULL,
	[GrupLavGest] [varchar](5) NULL,
 CONSTRAINT [PK_TbGrupLav] PRIMARY KEY CLUSTERED 
(
	[GrupLavId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VTbRisorse]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[VTbRisorse]
as

SELECT [RIS-ID],
       [RIS-ANAGRAFICA],
       [RIS-USERW],
	   [RIS-GRUPLAVID],
	   [RIS-GEVE],
	   [RIS-COGE],
	   [RIS-VDOX],
	   [RIS-AMMIN],
	   [RIS-UTIL],
	   [RIS-NOIP],
	   GRUPPOLAVORO = isnull((Select GrupLavNome from TbGrupLav where GrupLavId =  [RIS-GRUPLAVID]),'')

from TbRisorse
GO
/****** Object:  Table [dbo].[TbNote]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbNote](
	[NoteDb] [varchar](50) NOT NULL,
	[NoteTipo] [varchar](1) NOT NULL,
	[NoteTab] [varchar](50) NOT NULL,
	[NoteNote] [varchar](4000) NOT NULL,
	[NoteAzzer] [bit] NOT NULL,
	[NoteReplica] [bit] NOT NULL,
 CONSTRAINT [PK_TbNote] PRIMARY KEY CLUSTERED 
(
	[NoteDb] ASC,
	[NoteTipo] ASC,
	[NoteTab] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbTabelle]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbTabelle](
	[TabDataBase] [varchar](50) NOT NULL,
	[TabTipo] [varchar](50) NOT NULL,
	[TabTabella] [varchar](50) NOT NULL,
	[TabPosizione] [smallint] NOT NULL,
	[TabColonna] [varchar](50) NOT NULL,
	[TabTipoDato] [varchar](50) NOT NULL,
	[TabLungh] [smallint] NOT NULL,
	[TabChiave] [varchar](50) NULL,
	[TabIntero] [smallint] NULL,
	[TabDecimale] [smallint] NULL,
	[TabCheck] [varchar](2) NOT NULL,
	[TabReplica] [varchar](1) NULL,
	[TabText] [varchar](4000) NULL,
	[TabId] [varchar](10) NULL,
	[TabNull] [varchar](4) NULL,
 CONSTRAINT [PK_TbTabelle] PRIMARY KEY CLUSTERED 
(
	[TabDataBase] ASC,
	[TabTipo] ASC,
	[TabTabella] ASC,
	[TabPosizione] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CRTRACCIATI]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CRTRACCIATI]
AS
SELECT     dbo.TbTabelle.TabDataBase AS Expr1, dbo.TbTabelle.TabTipo, dbo.TbTabelle.TabTabella, dbo.TbTabelle.TabPosizione, dbo.TbTabelle.TabColonna, 
                      dbo.TbTabelle.TabTipoDato, dbo.TbTabelle.TabLungh, dbo.TbTabelle.TabChiave, dbo.TbTabelle.TabIntero, dbo.TbTabelle.TabDecimale, 
                      dbo.TbTabelle.TabNull, dbo.TbTabelle.TabId
FROM         dbo.TbTabelle INNER JOIN
                      dbo.TbNote ON dbo.TbTabelle.TabDataBase = dbo.TbNote.NoteDb AND dbo.TbTabelle.TabTabella = dbo.TbNote.NoteTab AND 
                      dbo.TbTabelle.TabTipo = dbo.TbNote.NoteTipo
WHERE     (dbo.TbTabelle.TabTipo = 'T')
GO
/****** Object:  Table [dbo].[TbAccessi]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbAccessi](
	[Entrata] [datetime] NULL,
	[Uscita] [datetime] NULL,
	[Workstation] [varchar](40) NULL,
	[Utente] [varchar](20) NULL,
	[Programma] [varchar](80) NULL,
	[Errore] [varchar](1) NOT NULL,
	[DescErrore] [varchar](200) NOT NULL,
	[InfoErrore] [varchar](4000) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VAccessi]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[VAccessi]
as
SELECT     CONVERT(varchar(10), Entrata, 103) AS Data, CONVERT(varchar(10), Entrata, 108) AS OraEntrata, CONVERT(varchar(10), Uscita, 108) AS OraUscita, 
                      Workstation, Programma, Utente, CONVERT(varchar(50), CONVERT(datetime, Uscita, 108) - CONVERT(datetime, Entrata, 108) + 0.00001, 108) AS Ora, 
                      'Errore' = case when Errore='E' then 'E' else '' end, 
		      'Interrotto' = case when Errore='I' then 'I' else '' end, DescErrore, InfoErrore
FROM         dbo.TbAccessi
GO
/****** Object:  Table [dbo].[TbDoc]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbDoc](
	[DocAnno] [char](4) NOT NULL,
	[DocTipo] [char](2) NOT NULL,
	[DocRifInterno] [char](6) NOT NULL,
	[DocEst] [char](3) NOT NULL,
	[DocAnaProg] [int] NOT NULL,
	[DocData] [smalldatetime] NOT NULL,
	[DocDesc] [varchar](80) NOT NULL,
	[DocNote] [varchar](16) NOT NULL,
	[DocNumRif] [int] NULL,
	[DocProt] [varchar](7) NOT NULL,
	[DocReg] [smallint] NOT NULL,
	[DocAnnoCoge] [smallint] NOT NULL,
	[DocKeyCm] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TbDoc] PRIMARY KEY CLUSTERED 
(
	[DocAnno] ASC,
	[DocTipo] ASC,
	[DocRifInterno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VAnaTDoc]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VAnaTDoc]
AS
SELECT     TOP 100 PERCENT dbo.TbDoc.DocAnno, dbo.TbAna.AnaDesc, dbo.TbTdo.TdoDesc, dbo.TbDoc.DocData, dbo.TbDoc.DocTipo, dbo.TbDoc.DocNumero, 
                      dbo.TbDoc.DocEst
FROM         dbo.TbDoc INNER JOIN
                      dbo.TbTdo ON dbo.TbDoc.DocTipo = dbo.TbTdo.TdoCod INNER JOIN
                      dbo.TbAna ON dbo.TbDoc.DocAnaCod = dbo.TbAna.AnaProg
ORDER BY dbo.TbAna.AnaDesc, dbo.TbTdo.TdoDesc, dbo.TbDoc.DocData
GO
/****** Object:  View [dbo].[VRdocum]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VRdocum]
AS
SELECT     TOP 100 PERCENT dbo.TbDoc.DocAnno, dbo.TbTdo.TdoDesc, dbo.TbAna.AnaDesc, dbo.TbDoc.DocTipo, dbo.TbDoc.DocNumero, 
                      dbo.TbDoc.DocEst
FROM         dbo.TbTdo INNER JOIN
                      dbo.TbDoc ON dbo.TbTdo.TdoCod = dbo.TbDoc.DocTipo INNER JOIN
                      dbo.TbAna ON dbo.TbDoc.DocAnaCod = dbo.TbAna.AnaProg
ORDER BY dbo.TbTdo.TdoDesc, dbo.TbAna.AnaDesc
GO
/****** Object:  View [dbo].[VRicAna]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VRicAna]
AS
SELECT     AnaProg, AnaDesc, AnaPiva, AnaIndirizzo, AnaCitta, AnaCod, AnaProv
FROM         dbo.TbAna
GO
/****** Object:  Table [dbo].[TbSel]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbSel](
	[SelId] [int] NOT NULL,
	[Sel1] [varchar](100) NULL,
	[Sel2] [varchar](100) NULL,
	[Sel3] [varchar](100) NULL,
	[Sel4] [varchar](100) NULL,
	[sel5] [varchar](100) NULL,
	[sel6] [varchar](100) NULL,
	[sel7] [varchar](100) NULL,
	[sel8] [varchar](100) NULL,
	[sel9] [bit] NULL,
	[sel10] [int] NULL,
	[sel11] [varchar](100) NULL,
	[sel12] [varchar](100) NULL,
	[sel13] [smallint] NULL,
	[sel14] [varchar](50) NOT NULL,
	[sel15] [bit] NOT NULL,
	[sel16] [varchar](50) NOT NULL,
	[SelDimArea] [decimal](5, 2) NOT NULL,
	[SelLicenza] [binary](50) NOT NULL,
 CONSTRAINT [PK_TbSel] PRIMARY KEY CLUSTERED 
(
	[SelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VLICENZA]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[VLICENZA]
AS

SELECT AnaDesc,AnaIndirizzo,AnaCap,AnaCitta,AnaProv,AnaPiva,AnaCfis,Sel1,SelLicenza =convert(varchar(11),SelLicenza) 
FROM TbAna inner join TbSel on SelId = 400
 WHERE AnaGrp = 'AZ' AND AnaCod = '00001'


GO
/****** Object:  View [dbo].[VRicDoc]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VRicDoc]
AS
SELECT     TOP 100 PERCENT dbo.TbTdo.TdoDesc, dbo.TbDoc.DocData, dbo.TbDoc.DocDesc, dbo.TbAna.AnaDesc, dbo.TbDoc.DocAnno, dbo.TbDoc.DocTipo, 
                      dbo.TbDoc.DocNumero, dbo.TbDoc.DocEst, dbo.TbDoc.DocAnaCod
FROM         dbo.TbTdo INNER JOIN
                      dbo.TbDoc ON dbo.TbTdo.TdoCod = dbo.TbDoc.DocTipo INNER JOIN
                      dbo.TbAna ON dbo.TbDoc.DocAnaCod = dbo.TbAna.AnaProg
ORDER BY dbo.TbDoc.DocData, dbo.TbDoc.DocNumero
GO
/****** Object:  View [dbo].[Vtreeview]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Vtreeview]
AS
SELECT     TOP 100 PERCENT dbo.TbAna.AnaDesc, dbo.TbDoc.DocAnno, dbo.TbTdo.TdoDesc, dbo.TbDoc.DocDesc, 
                      dbo.TbDoc.DocAnno + '\' + dbo.TbDoc.DocTipo + dbo.TbDoc.DocNumero + '.' + dbo.TbDoc.DocEst AS DocPath, ISNULL(dbo.TbAnn.AnnStor, ' ') 
                      AS AnnStor, ISNULL(dbo.TbDoc.DocNumRif, 0) AS DocNumRif, dbo.TbAna.AnaProg, dbo.TbAna.AnaGrp, COGE.dbo.TbGrp.GrpDesc, dbo.TbDoc.DocNote, 
                      dbo.TbDoc.DocProt, dbo.TbDoc.DocReg
FROM         dbo.TbAna INNER JOIN
                      dbo.TbDoc ON dbo.TbAna.AnaProg = dbo.TbDoc.DocAnaCod INNER JOIN
                      dbo.TbTdo ON dbo.TbDoc.DocTipo = dbo.TbTdo.TdoCod INNER JOIN
                      dbo.TbAnn ON dbo.TbDoc.DocAnno = dbo.TbAnn.AnnAnno INNER JOIN
                      COGE.dbo.TbGrp ON dbo.TbAna.AnaGrp = COGE.dbo.TbGrp.GrpCod
ORDER BY dbo.TbDoc.DocAnno, dbo.TbTdo.TdoDesc, dbo.TbDoc.DocReg, dbo.TbDoc.DocProt, dbo.TbDoc.DocDesc
GO
/****** Object:  Table [dbo].[TbUtenti]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbUtenti](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Utente] [varchar](20) NOT NULL,
	[Password] [varchar](10) NOT NULL,
	[GrupLavId] [int] NOT NULL,
 CONSTRAINT [PK_TbUtenti] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VUteGrup]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE VIEW [dbo].[VUteGrup]
AS
SELECT     dbo.TbUtenti.*, dbo.TbGrupLav.GrupLavNome AS GrupLavNome, dbo.TbGrupLav.GrupLavGest AS GrupLavGest
FROM         dbo.TbUtenti LEFT OUTER JOIN
                      dbo.TbGrupLav ON dbo.TbUtenti.GrupLavId = dbo.TbGrupLav.GrupLavId
GO
/****** Object:  View [dbo].[VANAGROUP]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VANAGROUP]
AS
select AnaProg,AnaDesc,AnaIndirizzo,AnaCap,AnaCitta,AnaProv,AnaGrp,AnaCod,
GRUPPO = GRPCOD + ' ' + GRPDESC from TBANA LEFT OUTER JOIN COGE.DBO.TBGRP ON ANAGRP = GRPCOD


GO
/****** Object:  View [dbo].[DxVAccessi]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE view [dbo].[DxVAccessi]
as
SELECT     Data = cast(Entrata AS smalldatetime), CONVERT(varchar(10), Entrata, 108) AS OraEntrata, CONVERT(varchar(10), Uscita, 108) AS OraUscita, 
                      Workstation, Programma, Utente, CONVERT(varchar(50), CONVERT(datetime, Uscita, 108) - CONVERT(datetime, Entrata, 108) + 0.00001, 108) AS Ora, 
                      Errore, DescErrore, InfoErrore,
		      Lancio = SUBSTRING(programma,1,PATINDEX('%(%', Programma) -1) ,
		      ProgId = SUBSTRING(programma,PATINDEX('%(%', Programma) + 1, PATINDEX('%)%', Programma)- PATINDEX('%(%', Programma) -1)
FROM         dbo.TbAccessi





GO
/****** Object:  View [dbo].[VListe]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[VListe]
as

select CLisLista,ClisAnaProg,ClisAnaDesc,ClisCellulare,Listadesc,ClisEmail,
       OK = Case when (substring(ClisCellulare,1,1) <> '+' OR substring(ClisCellulare,4,1) <> '3') THEN 0 ELSE 1 END,
       DITTA = AnaDesc
from TbCListe inner join
     TbAna ON AnaProg = ClisAnaProg inner join
     TbListe on ListaNum = CLisLista
GO
/****** Object:  View [dbo].[ArcTfa]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ArcTfa]
AS
SELECT     *
FROM         dbo.TbFat INNER JOIN
                      Coge.dbo.TbPag ON dbo.TbFat.FatPagCod = Coge.dbo.TbPag.PagCod
GO
/****** Object:  View [dbo].[CFAFRAS]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE view [dbo].[CFAFRAS]
as
SELECT    CorRif, CorAge, CorSiglaSconto, CorCodMer, CorCodart ,CorProg, CorOmaggi,CorDesc, CorUmisura ,CorConf = case when CorFattConv = 1 then 0 else CorQuaCon  end, (CorQuaCon * CorFattConv) 
                      AS CorQuantita,CorPrezzo = CASE WHEN CorOmaggi = 'E' THEN 'Omaggio ' WHEN CorOmaggi = 'S' THEN 'Om. sogg.' ELSE STR(CorPrezzo, 8, 5) 
                      END, CorSconto1, CorSconto2, CorImporto ,CorCiva
FROM        tbCor
WHERE    CorTipoDoc = 'F'
GO
/****** Object:  View [dbo].[VDatabase]    Script Date: 20/05/2026 15:38:18 ******/
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
inner join vdox.DBO.sysobjects as k on s.table_name=k.name
inner join vdox.DBO.syscolumns as sys on s.column_name=sys.name and k.id=sys.id
where s.table_name not like 'sy%' and s.table_name not like 'dt%' and s.table_name not like 'ms%' and s.table_name <> 'TbTabelle'
		and s.table_name <> 'TbTrig' and s.table_name <> 'TbNote' and s.table_name <> 'tmpTbTabelle'
		and isnull(constraint_name,'') not like 'FK_%'
GO
/****** Object:  View [dbo].[VPBXDET]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VPBXDET]
AS
SELECT 	PbxID,PbxData,PbxTipoLEntrata,PbxTipoLUscita,PbxIDChiamante,PbxIDChiamato,PbxDataOra,
        PbxDurata,PbxNumChiamante,PbxNumChiamato,PbxProvider,PbxEsito,PbxProg,PbxTemp,PbxSecondi,
	    PbxOpChiamante,PbxOpChiamato,PbxCliChiamante,PbxCliChiamato,PbxCosto,
        IDINTERNO = CASE WHEN len(Pbxidchiamante) > 4 THEN PbxIdChiamato else PbxIdChiamante end,
        IDINOUT = CASE WHEN len(Pbxidchiamante) > 4 THEN CAST(0 as integer) else cast(1 as integer) end,
        NUMEROTEL = CASE WHEN len(Pbxidchiamante) > 4 THEN PbxNumChiamante else PbxNumChiamato end,
        OPERATORE = CASE WHEN len(Pbxidchiamante) > 4 THEN PbxOpChiamante else PbxOpChiamato end,
        ANAGRAFICA = CASE WHEN len(Pbxidchiamante) > 4 THEN PbxCliChiamante else PbxCliChiamato end
      FROM TBPBX  


GO
/****** Object:  View [dbo].[VRoutine]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[VRoutine]
as
select text,specific_name,type= case when routine_type='PROCEDURE' then 'P' else 'F' end, specific_catalog,colid
 from INFORMATION_SCHEMA.ROUTINES as inf
		inner join vdox.dbo.sysobjects as obj on obj.name=inf.specific_name
		inner join vdox.dbo.syscomments as com on com.id=obj.id
where specific_name not like 'dt%'
GO
/****** Object:  View [dbo].[VTrigger]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[VTrigger]
as
select inf.table_catalog,sys.name,nome_trig=obj.name,com.text from sysobjects sys
inner join vdox.dbo.sysobjects obj on sys.id=obj.parent_obj
inner join vdox.dbo.syscomments com on obj.id=com.id
inner join information_schema.tables inf on inf.table_name=sys.name
where obj.xtype = 'tr'
GO
/****** Object:  Table [dbo].[Tblaser]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tblaser](
	[LasId] [varchar](6) NOT NULL,
	[LasFile] [varchar](40) NOT NULL,
	[LasStampante] [varchar](50) NULL,
	[LasTipo] [varchar](5) NULL,
	[LasDefault] [varchar](1) NOT NULL,
	[LasReset] [varchar](19) NULL,
	[LasCarCompr] [varchar](19) NULL,
	[LasCarNorm] [varchar](19) NULL,
	[LasScambioImporti] [bit] NOT NULL,
 CONSTRAINT [PK_Tblaser] PRIMARY KEY CLUSTERED 
(
	[LasId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbLogo]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbLogo](
	[IDLOGO] [smallint] NOT NULL,
	[TOPLOGO] [image] NULL,
	[TOPLOGOD] [tinyint] NULL,
	[TOPLOGOA] [tinyint] NULL,
	[TOPLOGOP] [varchar](500) NULL,
	[TOPLOGOW] [smallint] NULL,
	[TOPLOGOH] [smallint] NULL,
	[BOTLOGO] [image] NULL,
	[BOTLOGOD] [tinyint] NULL,
	[BOTLOGOA] [tinyint] NULL,
	[BOTLOGOP] [varchar](500) NULL,
	[BOTLOGOW] [smallint] NULL,
	[BOTLOGOH] [smallint] NULL,
	[TOPPERC] [decimal](5, 2) NULL,
	[BOTPERC] [decimal](5, 2) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbPwd]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbPwd](
	[PwdId] [varchar](8) NOT NULL,
	[PwdPwd] [varchar](8) NOT NULL,
	[PwdOperatore] [varchar](30) NULL,
 CONSTRAINT [PK_TbPwd] PRIMARY KEY CLUSTERED 
(
	[PwdId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TbXml]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbXml](
	[IdProgramma] [nvarchar](30) NOT NULL,
	[IdNomeStampa] [nvarchar](30) NOT NULL,
	[IdA4A3] [bit] NOT NULL,
	[IdOrizVert] [bit] NOT NULL,
	[IdUltima] [bit] NOT NULL,
	[IdFabbrica] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmpTbTabelle]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmpTbTabelle](
	[tmpTabDataBase] [varchar](50) NOT NULL,
	[tmpTabTipo] [varchar](50) NOT NULL,
	[tmpTabTabella] [varchar](50) NOT NULL,
	[tmpTabPosizione] [smallint] NOT NULL,
	[tmpTabColonna] [varchar](50) NOT NULL,
	[tmpTabTipodato] [varchar](50) NOT NULL,
	[tmpTabLungh] [smallint] NOT NULL,
	[tmpTabChiave] [varchar](50) NULL,
	[tmpTabIntero] [smallint] NULL,
	[tmpTabDecimale] [smallint] NULL,
	[tmpTabCheck] [varchar](2) NOT NULL,
	[tmpTabReplica] [varchar](1) NULL,
	[tmpTabText] [varchar](4000) NULL,
	[tmpTabId] [varchar](10) NULL,
	[tmpTabNull] [varchar](4) NULL,
 CONSTRAINT [PK_tmpTbTabelle] PRIMARY KEY CLUSTERED 
(
	[tmpTabDataBase] ASC,
	[tmpTabTipo] ASC,
	[tmpTabTabella] ASC,
	[tmpTabPosizione] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UtentiWidow]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UtentiWidow](
	[ANAGRAFICA] [varchar](60) NOT NULL,
	[UserWindows] [varchar](20) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [<Name of Missing Index, sysname,>]    Script Date: 20/05/2026 15:38:18 ******/
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>] ON [dbo].[TbAna]
(
	[AnaGrp] ASC,
	[AnaCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocNumRif]    Script Date: 20/05/2026 15:38:18 ******/
CREATE NONCLUSTERED INDEX [IX_DocNumRif] ON [dbo].[TbDoc]
(
	[DocNumRif] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TbAccessi] ADD  CONSTRAINT [DF_TbAccessi_Utente]  DEFAULT ('') FOR [Utente]
GO
ALTER TABLE [dbo].[TbAccessi] ADD  CONSTRAINT [DF_TbAccessi_Errore]  DEFAULT ('') FOR [Errore]
GO
ALTER TABLE [dbo].[TbAccessi] ADD  CONSTRAINT [DF_TbAccessi_DescErrore]  DEFAULT ('') FOR [DescErrore]
GO
ALTER TABLE [dbo].[TbAccessi] ADD  CONSTRAINT [DF_TbAccessi_InfoErrore]  DEFAULT ('') FOR [InfoErrore]
GO
ALTER TABLE [dbo].[TbAna] ADD  CONSTRAINT [DF_TbAna_AnaNoRubrica]  DEFAULT ((0)) FOR [AnaNoRubrica]
GO
ALTER TABLE [dbo].[TbDoc] ADD  CONSTRAINT [DF__TbDoc__DocProt__7A3223E8]  DEFAULT ('') FOR [DocProt]
GO
ALTER TABLE [dbo].[TbDoc] ADD  CONSTRAINT [DF__TbDoc__DocReg__7B264821]  DEFAULT ((0)) FOR [DocReg]
GO
ALTER TABLE [dbo].[TbDoc] ADD  CONSTRAINT [DF_TbDoc_DocAnnoCoge]  DEFAULT ((0)) FOR [DocAnnoCoge]
GO
ALTER TABLE [dbo].[TbGrupLav] ADD  CONSTRAINT [DF_TbGrupLav_GrupLavGest]  DEFAULT ('000') FOR [GrupLavGest]
GO
ALTER TABLE [dbo].[Tblaser] ADD  CONSTRAINT [DF_Tblaser_LasDefault]  DEFAULT ('') FOR [LasDefault]
GO
ALTER TABLE [dbo].[Tblaser] ADD  CONSTRAINT [DF_Tblaser_LasScamioImporti]  DEFAULT ((0)) FOR [LasScambioImporti]
GO
ALTER TABLE [dbo].[TbLogo] ADD  CONSTRAINT [DF_TbLogo_IDLOGO]  DEFAULT ((1)) FOR [IDLOGO]
GO
ALTER TABLE [dbo].[TbLogo] ADD  CONSTRAINT [DF_TbLogo_TOPLOGOP]  DEFAULT ('') FOR [TOPLOGOP]
GO
ALTER TABLE [dbo].[TbLogo] ADD  CONSTRAINT [DF_TbLogo_BOTLOGOH]  DEFAULT ('') FOR [BOTLOGOH]
GO
ALTER TABLE [dbo].[TbLogo] ADD  CONSTRAINT [DF_TbLogo_TOPPERC]  DEFAULT ((0)) FOR [TOPPERC]
GO
ALTER TABLE [dbo].[TbLogo] ADD  CONSTRAINT [DF_TbLogo_BOTPERC]  DEFAULT ((0)) FOR [BOTPERC]
GO
ALTER TABLE [dbo].[TbSel] ADD  CONSTRAINT [DF__TbSel__sel14__37703C52]  DEFAULT ('') FOR [sel14]
GO
ALTER TABLE [dbo].[TbSel] ADD  CONSTRAINT [DF__TbSel__sel15__3864608B]  DEFAULT (0) FOR [sel15]
GO
ALTER TABLE [dbo].[TbSel] ADD  CONSTRAINT [DF__TbSel__sel16__395884C4]  DEFAULT ('') FOR [sel16]
GO
ALTER TABLE [dbo].[TbSel] ADD  CONSTRAINT [DF_TbSel_SelDimArea]  DEFAULT (10) FOR [SelDimArea]
GO
ALTER TABLE [dbo].[TbXml] ADD  CONSTRAINT [DF_TbXml_IdFabbrica]  DEFAULT ((0)) FOR [IdFabbrica]
GO
ALTER TABLE [dbo].[TbUtenti]  WITH CHECK ADD  CONSTRAINT [FK_TbUtenti_TbGrupLav] FOREIGN KEY([GrupLavId])
REFERENCES [dbo].[TbGrupLav] ([GrupLavId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TbUtenti] CHECK CONSTRAINT [FK_TbUtenti_TbGrupLav]
GO
/****** Object:  StoredProcedure [dbo].[AggiornaPbx]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[AggiornaPbx]
as

---SELECT * INTO #TMP FROM TBANA WHERE ANATEL1>'' OR ANATEL2>'' OR ANATEL3>'' OR ANAFAX >'' ORDER BY ANAPROG DESC

SELECT AnaDesc,AnaTel1,AnaTel2,AnaTel3,AnaFax INTO #TMP1 FROM TBANA WHERE ANATEL1>'' OR ANATEL2>'' OR ANATEL3>'' OR ANAFAX >''
union
select PhAzienda,PhTel1,PhTel2,PhTel3,PhFax from NewPhone WHERE PHTEL1>'' OR PHTEL2>'' OR PHTEL3>'' OR PHFAX >''

select #TMP1.*,identity(int,1,1) as AnaProg into #TMP from #tmp1 order by anaprog desc

DECLARE @TEL1 as VARCHAR(20),@TEL2 as VARCHAR(20),@TEL3 as VARCHAR(20),@TELF as VARCHAR(20),@IIDD AS INT
DECLARE @NTEL as varchar(20), @I as smallint

DECLARE SERGENTE CURSOR FOR
SELECT ANAPROG,ANATEL1,ANATEL2,ANATEL3,ANAFAX from #TMP 

OPEN SERGENTE
FETCH NEXT FROM SERGENTE
INTO @IIDD,@TEL1,@TEL2,@TEL3,@TELF

WHILE @@FETCH_STATUS = 0
BEGIN
SET @NTEL = @TEL1
IF ISNUMERIC(@TEL1) = 0
BEGIN
SET @I = 0
SET @NTEL = ''
LOOP1:
SET @I = @I + 1
IF @I > 20 GOTO ATEL2
IF ISNUMERIC(SUBSTRING(@TEL1,@I,1)) = 1 SET @NTEL = @NTEL+SUBSTRING(@TEL1,@I,1)
GOTO LOOP1
ATEL2:
SET @NTEL = REPLACE (@NTEL,'-','')
SET @NTEL = REPLACE (@NTEL,'.','')
SET @NTEL = REPLACE (@NTEL,'/','')
UPDATE #TMP SET ANATEL1=@NTEL WHERE @IIDD = ANAPROG
END

SET @NTEL = @TEL2
IF ISNUMERIC(@TEL2) = 0
BEGIN
SET @I = 0
SET @NTEL = ''
LOOP2:
SET @I = @I + 1
IF @I > 20 GOTO ATEL3
IF ISNUMERIC(SUBSTRING(@TEL2,@I,1)) = 1 SET @NTEL = @NTEL+SUBSTRING(@TEL2,@I,1)
GOTO LOOP2
ATEL3:
SET @NTEL = REPLACE (@NTEL,'-','')
SET @NTEL = REPLACE (@NTEL,'.','')
SET @NTEL = REPLACE (@NTEL,'/','')
UPDATE #TMP SET ANATEL2=@NTEL WHERE @IIDD = ANAPROG
END

SET @NTEL = @TEL3
IF ISNUMERIC(@TEL3) = 0
BEGIN
SET @I = 0
SET @NTEL = ''
LOOP3:
SET @I = @I + 1
IF @I > 20 GOTO ATEL4
IF ISNUMERIC(SUBSTRING(@TEL3,@I,1)) = 1 SET @NTEL = @NTEL+SUBSTRING(@TEL3,@I,1)
GOTO LOOP3
ATEL4:
SET @NTEL = REPLACE (@NTEL,'-','')
SET @NTEL = REPLACE (@NTEL,'.','')
SET @NTEL = REPLACE (@NTEL,'/','')
UPDATE #TMP SET ANATEL3=@NTEL WHERE @IIDD = ANAPROG
END

SET @NTEL = @TELF
IF ISNUMERIC(@TELF) = 0
BEGIN
SET @I = 0
SET @NTEL = ''
LOOPF:
SET @I = @I + 1
IF @I > 20 GOTO ATELF
IF ISNUMERIC(SUBSTRING(@TELF,@I,1)) = 1 SET @NTEL = @NTEL+SUBSTRING(@TELF,@I,1)
GOTO LOOPF
ATELF:
SET @NTEL = REPLACE (@NTEL,'-','')
SET @NTEL = REPLACE (@NTEL,'.','')
SET @NTEL = REPLACE (@NTEL,'/','')
UPDATE #TMP SET ANAFAX=@NTEL WHERE @IIDD = ANAPROG
END
FINE_LOOP:
   FETCH NEXT FROM SERGENTE
   INTO @IIDD,@TEL1,@TEL2,@TEL3,@TELF
END
CLOSE SERGENTE
DEALLOCATE SERGENTE

Update TbPbx set PbxNumChiamante = pbxIDChiamante where PbxNumChiamante = ''

Update TbPbx
set PbxCliChiamante = CASE WHEN LEN(PbxNumChiamante) < 4
                           THEN
                           isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end from TbUpBx where UpBxext = PbxNumChiamante),'')
                           ELSE
                           isnull((Select top 1 substring(AnaDesc,1,50) from #TMP 
                           where (AnaTel1 = PbxNumChiamante or anatel2 = PbxNumChiamante or AnaTel3 = PbxNumChiamante or AnaFax = PbxNumChiamante)),'')
                           END
where pbxCliChiamante = ''

Update TbPbx set PbxOpChiamante = isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end
from TbUpBx where UpBxext = PbxIdChiamante),'')
where PbxOpChiamante = ''

Update TbPbx
set PbxCliChiamato = CASE WHEN LEN(PbxNumChiamato) < 4
                          THEN
                          isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end from TbUpBx where UpBxext = PbxNumChiamato),'')
                          ELSE
                          isnull((Select top 1 substring(AnaDesc,1,50) from #TMP
                          Where ( AnaTel1 = PbxNumChiamato or anatel2 = PbxNumChiamato or AnaTel3 = PbxNumChiamato or AnaFax = PbxNumChiamato)),'')
                          END

Update TbPbx set PbxOpChiamato = isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end
from TbUpBx where UpBxext = PbxIdChiamato),'')
where PbxOpChiamato = ''

UPDATE TbPbx set PbxCosto = CONVERT( DECIMAL(6,3),cast(LCRTariffa as decimal(9,3)) / 1000)  * pbxsecondi / 6000
from TbPbx
inner join TbLcr on PbxProvider = LCRProvider and SUBSTRING(PbxIDcHIAMATO,1,1) = lcrPrefisso 
where PbxopChiamante <> '' AND PbxCosto = 0


GO
/****** Object:  StoredProcedure [dbo].[ConfrontaTabelle]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ConfrontaTabelle]
as
update vdox.dbo.tmptbtabelle set tmptabcheck='1i' 
Update vdox.dbo.tmptbtabelle set tmptabcheck='0' 
	FROM tmptbtabelle 
	inner join vdox.dbo.tbtabelle 
	on (tmptabdatabase=tabdatabase and tmptabtipo=tabtipo 
		and tmptabposizione=tabposizione and tmptabtabella=tabtabella)
	where tmptabcolonna=tabcolonna and tmpTabtipodato=tabtipodato and tmpTabLungh=TabLungh 
		and TmpTabChiave=Tabchiave and tmpTabIntero=TabIntero 
		and tmpTabDecimale=TabDecimale and isnull(TmpTabtext,'')=isnull(Tabtext,'')
		and isnull(tmpTabId,'')=isnull(TabId,'') and isnull(tmpTabNull,'')=isnull(TabNull,'')
update vdox.dbo.tbtabelle set tabcheck='1c' 
Update vdox.dbo.tbtabelle set tabcheck='0' 
	FROM tbtabelle 
	inner join vdox.dbo.tmptbtabelle 
	on (tmptabdatabase=tabdatabase and tmptabtipo=tabtipo 
		and tmptabposizione=tabposizione and tmptabtabella=tabtabella)
	where tmptabcolonna=tabcolonna and tmpTabtipodato=tabtipodato and tmpTabLungh=TabLungh 
		and TmpTabChiave=Tabchiave and tmpTabIntero=TabIntero 
		and tmpTabDecimale=TabDecimale and isnull(TmpTabtext,'')=isnull(Tabtext,'')
		and isnull(tmpTabId,'')=isnull(TabId,'') and isnull(tmpTabNull,'')=isnull(TabNull,'')
Update vdox.dbo.tmptbtabelle set tmptabcheck='1m' 
	FROM tmptbtabelle 
	inner join vdox.dbo.tbtabelle 
	on (tmptabdatabase=tabdatabase and tmptabtipo=tabtipo 
		and tmptabposizione=tabposizione and tmptabtabella=tabtabella)
	where tmptabcheck='1i'
Update vdox.dbo.tbtabelle set tabcheck='1m' 
	FROM tbtabelle 
	inner join vdox.dbo.tmptbtabelle 
	on (tmptabdatabase=tabdatabase and tmptabtipo=tabtipo 
		and tmptabposizione=tabposizione and tmptabtabella=tabtabella)
	where tabcheck='1c'
GO
/****** Object:  StoredProcedure [dbo].[CreaDest]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc [dbo].[CreaDest] @Term as varchar(20), @Lista as smallint
as

DELETE FROM TMPDEST WHERE TMPTERM = @TERM

insert into TMPDEST (TMPTERM,TMPTEL)
select @term,ClisCellulare
from TbCliste where ClisLista = @Lista


GO
/****** Object:  StoredProcedure [dbo].[CreaDestEmail]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Proc [dbo].[CreaDestEmail] @Term as varchar(20), @Lista as smallint
as

DELETE FROM TMPDESTEMAIL WHERE TMPTERM = @TERM

insert into TMPDESTEMAIL (TMPTERM,TMPEMAIL)
select @term,ClisEmail
from TbCliste where ClisLista = @Lista



GO
/****** Object:  StoredProcedure [dbo].[CreatmpTabelle]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CreatmpTabelle]
as
delete from vdox.dbo.tmpTbTabelle
delete from vdox.dbo.tbtrig
insert into vdox.dbo.tmpTbTabelle (tmpTabDatabase,tmpTabTipo,tmpTabTabella,tmpTabPosizione,
			tmpTabColonna,tmpTabTipoDato,tmpTabLungh,tmpTabChiave,tmpTabIntero,
			tmpTabDecimale,tmpTabNull,tmpTabCheck,tmpTabId)
select table_catalog,table_type,table_name,ordinal_position,column_name,data_type,length,isnull(constraint_name,''),prec,
	scala,Nullo, Flag='0',Id from vdox.dbo.VDataBase
insert into vdox.dbo.tmpTbTabelle (tmpTabDatabase,tmpTabTipo,tmpTabTabella,tmpTabPosizione,
			tmpTabColonna,tmpTabTipoDato,tmpTabLungh,tmpTabChiave,tmpTabIntero,
			tmpTabDecimale,tmpTabNull,tmpTabCheck,tmpTabId)
select table_catalog,table_type,table_name,ordinal_position,column_name,data_type,length,isnull(constraint_name,''),prec,
	scala,Nullo, Flag='0',Id from COGE.dbo.VDataBase
insert into vdox.dbo.tmpTbTabelle (tmpTabDatabase,tmpTabTipo,tmpTabTabella,tmpTabPosizione,
			tmpTabColonna,tmpTabTipoDato,tmpTabLungh,tmpTabChiave,tmpTabIntero,
			tmpTabDecimale,tmpTabCheck,tmptabtext)
select specific_catalog,type,specific_name,colid,'','','','','','','',text from Vdox.dbo.Vroutine
insert into vdox.dbo.tmpTbTabelle (tmpTabDatabase,tmpTabTipo,tmpTabTabella,tmpTabPosizione,
			tmpTabColonna,tmpTabTipoDato,tmpTabLungh,tmpTabChiave,tmpTabIntero,
			tmpTabDecimale,tmpTabCheck,tmptabtext)
select specific_catalog,type,specific_name,colid,'','','','','','','',text from coge.dbo.Vroutine
insert into vdox.dbo.tmptbtabelle (tmpTabDatabase,tmpTabTipo,tmpTabTabella,tmpTabPosizione,
			tmpTabColonna,tmpTabTipoDato,tmpTabLungh,tmpTabChiave,tmpTabIntero,
			tmpTabDecimale,tmpTabCheck,tmptabtext)
select table_catalog,'Tr',nome_trig,'',name,'','','','','','',text from vdox.dbo.VTrigger
insert into vdox.dbo.tmptbtabelle (tmpTabDatabase,tmpTabTipo,tmpTabTabella,tmpTabPosizione,
			tmpTabColonna,tmpTabTipoDato,tmpTabLungh,tmpTabChiave,tmpTabIntero,
			tmpTabDecimale,tmpTabCheck,tmptabtext)
select table_catalog,'Tr',nome_trig,'',name,'','','','','','',text from coge.dbo.VTrigger
insert into vdox.dbo.TbTrig (TrigDb,trigtab,trignome,trigtext)
select table_catalog,name,nome_trig,text from vdox.dbo.VTrigger
insert into vdox.dbo.TbTrig (TrigDb,trigtab,trignome,trigtext)
select table_catalog,name,nome_trig,text from coge.dbo.VTrigger
GO
/****** Object:  StoredProcedure [dbo].[DXVISUAL]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DXVISUAL] @ANNO AS INT
AS

DECLARE @TABELLA as varchar(25),@CMD AS VARCHAR(1000),@NOME as varchar(6)

set @TABELLA = '[VDOX].[dbo].[Tb' + CAST(@ANNO as varchar(4))+ ']'
set @NOME = 'Tb' + CAST(@ANNO as varchar(4))+ ''

if not exists (SELECT * FROM VDOX.dbo.sysobjects WHERE (name = @NOME ))
begin
set @CMD = ' CREATE TABLE ' + @tabella + '(
	[IDANNO] [int] IDENTITY(1,1) NOT NULL,
	[IDDOC] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_' + @NOME + '] PRIMARY KEY CLUSTERED 
(
	[IDANNO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]'
EXEC  (@CMD)

INSERT INTO TbAnn(AnnAnno,AnnProg,AnnStor)
select @ANNO,0,''
end

GO
/****** Object:  StoredProcedure [dbo].[NApp]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[NApp]  @RS smallint,@IIDD as int
 as
INSERT INTO Tbscheduling ([SCH-IDRIS],[SCH-STATO],[SCH-DESCRIZIONE],[SCH-ETICHETTA],[SCH-INIZIO],
	[SCH-FINE],[SCH-LUOGO],[SCH-GINTERA],[SCH-EVENTO],[SCH-RICORRENZA],[SCH-TRICORRENZ],
        [SCH-OGGETTO],[SCH-ANACOD])
SELECT @RS,[SCH-STATO],[SCH-DESCRIZIONE],[SCH-ETICHETTA],[SCH-INIZIO],
	[SCH-FINE],[SCH-LUOGO],[SCH-GINTERA],[SCH-EVENTO],[SCH-RICORRENZA],[SCH-TRICORRENZ],
        [SCH-OGGETTO],[SCH-ANACOD]
FROM        Tbscheduling WHERE [SCH-ID] = @IIDD
GO
/****** Object:  StoredProcedure [dbo].[NuovaLista]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[NuovaLista] @Tipo as varchar(1)
as

declare @Codice as smallint

set @codice = ((select isnull(max(ListaNum),0) from TbListe) + 1)

insert into TbListe (ListaNum,ListaDesc,ListaTipo) values (@codice,'LISTA NUMERO ' + cast(@Codice as varchar),@Tipo)

select @Codice

GO
/****** Object:  StoredProcedure [dbo].[OLDXVISDOC]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OLDXVISDOC] @ID as int
AS
SELECT  AnaDesc,DocAnno,TdoDesc,DocDesc =REPLACE(docdesC,TdoInizialeDesc + ' NR. ' ,'') , 
DocPath=DocAnno + '\' + DocTipo + DocRifInterno + '.' + DocEst,
AnnStor=ISNULL(AnnStor, ''),
DocNumrif=ISNULL(DocNumRif, 0),
DocRifInterno,
AnaProg,AnaGrp,GrpDesc,DocNote,DocProt,DocReg,TdoCod,DocAnnoCoge,DocKeyCm,
DocNum = cast(0 as int),
DocData = cast(getdate() as smalldatetime),
DocP = REPLACE(REPLACE(docdesC,TdoInizialeDesc + ' NR. ' ,''),'DEL ','@')
INTO #TMP FROM TbDoc
INNER JOIN TbAna ON AnaProg = DocAnaProg
INNER JOIN TbTdo ON DocTipo = TdoCod
INNER JOIN TbAnn ON DocAnno = AnnAnno
INNER JOIN COGE.dbo.TbGrp ON AnaGrp = GrpCod
WHERE DocAnno IN(SELECT QA FROM tBqANN)

DELETE FROM #TMP WHERE TDOCOD IN (SELECT TdoCodGrup from TbTdoGr where TdoGrupId = @ID)

SELECT * from #TMP ORDER BY AnaGrp,AnaDesc,DocAnno,TdoCod,DocReg,DocProt,DocDesc
GO
/****** Object:  StoredProcedure [dbo].[procAggTabelle]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[procAggTabelle]
as
delete from vdox.dbo.tbtabelle
insert into vdox.dbo.tbtabelle (TabDatabase,TabTipo,TabTabella,TabPosizione,
			TabColonna,TabTipoDato,TabLungh,TabChiave,TabIntero,
			TabDecimale,TabCheck,tabtext,TabReplica,tabid,tabnull)
select tmpTabDatabase,tmpTabTipo,tmpTabTabella,tmpTabPosizione,
			tmpTabColonna,tmpTabTipoDato,tmpTabLungh,tmpTabChiave,tmpTabIntero,
			tmpTabDecimale,tmpTabCheck,tmptabtext,tmpTabReplica,tmptabid,tmptabnull 
from vdox.dbo.tmpTbTabelle
GO
/****** Object:  StoredProcedure [dbo].[X_PASSIVE_DA_IMPORTARE]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[X_PASSIVE_DA_IMPORTARE]
AS


---- CARICO IN UN TEMPORANEO I DOCUMENTI GIA' IMPORTATI
select * 
INTO #TMP
from TbDoc inner join
     coge.dbo.Tbpri on PridocAnn = DocAnno and Prinumprot = DocProt and priRegiva = DocReg inner join
	 geve.dbo.tbfte_passiva on fterifpri = priid
WHERE doctipo = 'FF'

----CONSIDERO SOLO QUELLE CONTABILIZZATE
SELECT DOCANNO = DATEPART(YEAR,FteData),
       DOCREG = PriRegIva,
	   DOCDESC = 'FATTURA NR. ' + CAST(PriDocEst as VARCHAR(6)) + ' DEL ' + CONVERT(VARCHAR(10), FteData, 103),
	   DOCDATA = FteData,
	   DOCANAPROG = AnaProg,
	   DOCPROT = PriNumProt,
	   DOCANNOCOGE = PriDocAnn

FROM  geve.dbo.tbfte_passiva inner join
      coge.dbo.Tbpri on priid = fterifpri and priprog = 1 inner join
	  TbAna on AnaCod = PricoAvere and AnaGrp = 'FO'

WHERE FTERIFPRI <> 0 AND  FTERIF NOT IN (SELECT fterif from #TMP) AND PRIPROG = 1
GO
/****** Object:  StoredProcedure [dbo].[XPHONE]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[XPHONE] @DAL AS SMALLDATETIME,@AL AS SMALLDATETIME
AS 

----Aggiornamento Pbx
declare @OGGI as smalldatetime
SET @OGGI = convert(smalldatetime,convert(varchar(10),getdate(),103),103)

select * INTO #TMPPBX from vdox.dbo.TbPbx where PbxData = @OGGI and PbxElab = 0

Update #TMPPBX  set PbxNumChiamante = pbxIDChiamante where PbxNumChiamante = '' 

Update #TMPPBX
set PbxCliChiamante = CASE WHEN LEN(PbxNumChiamante) < 4
                           THEN
                           isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end from vdox.dbo.TbUpBx where UpBxext = PbxNumChiamante),'')
                           ELSE
                           isnull((Select top 1 substring(AnaDesc,1,50) from Vdox.Dbo.TbAna
                           where (AnaTel1 = PbxNumChiamante or anatel2 = PbxNumChiamante or AnaTel3 = PbxNumChiamante or AnaFax = PbxNumChiamante)),'')
                           END
where pbxCliChiamante = ''

Update #TMPPBX set PbxOpChiamante = isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end
from vdox.dbo.TbUPbx where UpBxext = PbxIdChiamante),'')
where PbxOpChiamante = '' 

Update #TMPPBX
set PbxCliChiamato = CASE WHEN LEN(PbxNumChiamato) < 4
                          THEN
                          isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end from vdox.dbo.TbUpBx where UpBxext = PbxNumChiamato),'')
                          ELSE
                          isnull((Select top 1 substring(AnaDesc,1,50) from Vdox.Dbo.TbAna
                          Where ( AnaTel1 = PbxNumChiamato or anatel2 = PbxNumChiamato or AnaTel3 = PbxNumChiamato or AnaFax = PbxNumChiamato)),'')
                          END
where PbxCliChiamato = ''

Update #TMPPBX set PbxOpChiamato = isnull((select case when UPbxName <> '' then UpbxName else UpbxExt end
from vdox.dbo.TbUpBx where UpBxext = PbxIdChiamato),'')
where PbxOpChiamato = '' 

Update Vdox.dbo.TbPbx
set PbxNumChiamante = b.Pbxnumchiamante,
    PbxCliChiamante = b.PbxCliChiamante,
    PbxOpChiamante =  b.PbxOpChiamante,
    PbxCliChiamato =  b.PbxCliChiamato,
    PbxOpChiamato =   b.PbxOpChiamato
from Vdox.Dbo.TbPbx a inner join
     #TMPPBX b on a.PbxId = b.PbxId


--------------------------------------------








DELETE FROM TMPPHONE

INSERT INTO TMPPHONE(TDERIVATO,TTIPO,TNUMERO,TDURATA)

SELECT PBXIDCHIAMANTE,
CASE WHEN SUBSTRING(PBXIDCHIAMATO,1,2) = '00' THEN 0
WHEN SUBSTRING(PBXIDCHIAMATO,1,2) <> '00' AND SUBSTRING(PBXIDCHIAMATO,1,1) = '0' AND  SUBSTRING(PBXIDCHIAMATO,2,2)<>'11' THEN 1 
WHEN SUBSTRING(PBXIDCHIAMATO,1,3) = '011' THEN 2
WHEN SUBSTRING(PBXIDCHIAMATO,1,1) = '3' and len(PBXIDCHIAMATO)>3 THEN 3 
WHEN LEN(PBXIDCHIAMATO) < 4 AND len(pbxIdChiamante)<4 THEN 4 ELSE 5 END,
PBXIDCHIAMATO,
PbxDurata
FROM TBPBX WHERE len(pbxIdChiamante)<5 AND PbxData BETWEEN  @DAL and @AL
ORDER BY PBXIDCHIAMANTE,PBXIDCHIAMATO

DELETE FROM TMPPHONN
INSERT INTO TMPPHONN ([DERIVATO],[INTERNAZ],[DURATA-0],[NAZIONAL],[DURATA-1],[LOCALI],[DURATA-2],[CELLULAR],[DURATA-3],
                      [INTERNE],[DURATA-4],[ALTREFAX],[DURATA-5],[NRTOT],[DURATA-T],[DERIVADESC])
SELECT DISTINCT TDERIVATO,0,'',0,'',0,'',0,'',0,'',0,'',0,'',TDERIVATO FROM TMPPHONE

SELECT DISTINCT TDERIVATO, TTIPO,COUNT(TNUMERO) AS TNUMERO,
SUM(CAST(SUBSTRING(TDURATA,7,2) AS SMALLINT) + CAST(SUBSTRING(TDURATA,4,2) AS SMALLINT) * 60 + CAST(SUBSTRING(TDURATA,1,2) AS SMALLINT) * 3600) AS TDURATA
INTO #TMP FROM TMPPHONE 
GROUP BY TDERIVATO, TTIPO
ORDER BY TDERIVATO,TTIPO

SELECT DISTINCT TDERIVATO,SUM(TNUMERO) AS TNUMERO,SUM(TDURATA) AS TDURATA INTO #TMPT FROM  #TMP
GROUP BY TDERIVATO

DECLARE @P AS SMALLINT
SET @P = -1
LUPPA:
SET @P = @P + 1
IF @P > 6 GOTO OLTRE 
IF @P = 0 UPDATE TMPPHONN SET [INTERNAZ]= TNUMERO,[DURATA-0]=(SELECT dbo.FnHHMMSS(TDURATA)) FROM #TMP WHERE @P = TTIPO AND TDERIVATO = [DERIVATO] AND TDURATA > 0
IF @P = 1 UPDATE TMPPHONN SET [NAZIONAL]= TNUMERO,[DURATA-1]=(SELECT dbo.FnHHMMSS(TDURATA)) FROM #TMP WHERE @P = TTIPO AND TDERIVATO = [DERIVATO] AND TDURATA > 0
IF @P = 2 UPDATE TMPPHONN SET [LOCALI]= TNUMERO,[DURATA-2]=(SELECT dbo.FnHHMMSS(TDURATA)) FROM #TMP WHERE @P = TTIPO AND TDERIVATO = [DERIVATO] AND TDURATA > 0
IF @P = 3 UPDATE TMPPHONN SET [CELLULAR]= TNUMERO,[DURATA-3]=(SELECT dbo.FnHHMMSS(TDURATA)) FROM #TMP WHERE @P = TTIPO AND TDERIVATO = [DERIVATO] AND TDURATA > 0
IF @P = 4 UPDATE TMPPHONN SET [INTERNE]= TNUMERO,[DURATA-4]=(SELECT dbo.FnHHMMSS(TDURATA)) FROM #TMP WHERE @P = TTIPO AND TDERIVATO = [DERIVATO] AND TDURATA > 0
IF @P = 5 UPDATE TMPPHONN SET [ALTREFAX]= TNUMERO,[DURATA-5]=(SELECT dbo.FnHHMMSS(TDURATA)) FROM #TMP WHERE @P = TTIPO AND TDERIVATO = [DERIVATO] AND TDURATA > 0
IF @P = 6 UPDATE TMPPHONN SET [NRTOT]= TNUMERO,[DURATA-T]=(SELECT dbo.FnHHMMSS(TDURATA)) FROM #TMPT WHERE TDERIVATO = [DERIVATO] AND TDURATA > 0
GOTO LUPPA
OLTRE:

--- INSERISCI TOTALI GENERALI
SET @P=(SELECT COUNT(*) from #TMP)
IF @P = 0 GOTO FINE
INSERT INTO TMPPHONN ([DERIVATO],[INTERNAZ],[DURATA-0],[NAZIONAL],[DURATA-1],[LOCALI],[DURATA-2],[CELLULAR],[DURATA-3],
                      [INTERNE],[DURATA-4],[ALTREFAX],[DURATA-5],[NRTOT],[DURATA-T],[DERIVADESC])
SELECT 'zzzzz',(SELECT ISNULL(SUM(TNUMERO),0) FROM #TMP WHERE TTIPO = 0),
(SELECT dbo.FnHHMMSS((SELECT ISNULL(SUM(TDURATA),0) FROM #TMP WHERE TTIPO = 0))),
(SELECT ISNULL(SUM(TNUMERO),0) FROM #TMP WHERE TTIPO = 1),
(SELECT dbo.FnHHMMSS((SELECT ISNULL(SUM(TDURATA),0) FROM #TMP WHERE TTIPO = 1))),
(SELECT ISNULL(SUM(TNUMERO),0) FROM #TMP WHERE TTIPO = 2),
(SELECT dbo.FnHHMMSS((SELECT ISNULL(SUM(TDURATA),0) FROM #TMP WHERE TTIPO = 2))),
(SELECT ISNULL(SUM(TNUMERO),0) FROM #TMP WHERE TTIPO = 3),
(SELECT dbo.FnHHMMSS((SELECT ISNULL(SUM(TDURATA),0) FROM #TMP WHERE TTIPO = 3))),
(SELECT ISNULL(SUM(TNUMERO),0) FROM #TMP WHERE TTIPO = 4),
(SELECT dbo.FnHHMMSS((SELECT ISNULL(SUM(TDURATA),0) FROM #TMP WHERE TTIPO = 4))),
(SELECT ISNULL(SUM(TNUMERO),0) FROM #TMP WHERE TTIPO = 5),
(SELECT dbo.FnHHMMSS((SELECT ISNULL(SUM(TDURATA),0) FROM #TMP WHERE TTIPO = 5))),
(SELECT ISNULL(SUM(TNUMERO),0) FROM #TMP),
(SELECT dbo.FnHHMMSS((SELECT ISNULL(SUM(TDURATA),0) FROM #TMP))),'TOTALI'
--- INSERISCI TOTALI GENERALI
FINE:
DROP TABLE #TMP
DROP TABLE #TMPT

DELETE FROM TMPPHONT

INSERT INTO TMPPHONT ([SDERIVATO],[STIPO],[SNUMERO],[SSECONDI]) 
SELECT DERIVATO,'INTERNAZIONALI',INTERNAZ,(SELECT DBO.FnMinuti([DURATA-0])) from TMPPHONN

INSERT INTO TMPPHONT ([SDERIVATO],[STIPO],[SNUMERO],[SSECONDI]) 
SELECT DERIVATO,'NAZIONALI',NAZIONAL,(SELECT DBO.FnMinuti([DURATA-1])) from TMPPHONN

INSERT INTO TMPPHONT ([SDERIVATO],[STIPO],[SNUMERO],[SSECONDI]) 
SELECT DERIVATO,'LOCALI',LOCALI,(SELECT DBO.FnMinuti([DURATA-2])) from TMPPHONN

INSERT INTO TMPPHONT ([SDERIVATO],[STIPO],[SNUMERO],[SSECONDI]) 
SELECT DERIVATO,'CELLULARI',CELLULAR,(SELECT DBO.FnMinuti([DURATA-3])) from TMPPHONN

INSERT INTO TMPPHONT ([SDERIVATO],[STIPO],[SNUMERO],[SSECONDI]) 
SELECT DERIVATO,'INTERNE',INTERNE,(SELECT DBO.FnMinuti([DURATA-4])) from TMPPHONN

INSERT INTO TMPPHONT ([SDERIVATO],[STIPO],[SNUMERO],[SSECONDI]) 
SELECT DERIVATO,'ALTRE FAX',ALTREFAX,(SELECT DBO.FnMinuti([DURATA-5])) from TMPPHONN

DELETE FROM TMPPHONT WHERE SDERIVATO = 'zzzzz'
SELECT * FROM TMPPHONN ORDER BY DERIVATO



GO
/****** Object:  StoredProcedure [dbo].[XPRELCG]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XPRELCG] @DA as smallint
AS
select RivaTipo,AnnoCoge =datepart(year,pridatagio),PriRegIva,PriNumProt,PriBisRet ,PriCoDare,PriCoAvere,PriDocEst,PriDataEst,
AnaProg =case when RivaTipo = 2 Or RivaTipo = 4 Then (select AnaProg from TbAna Where AnaCod = PriCoAvere and AnaGrp = 'FO') 
else (select AnaProg from TbAna Where AnaCod = PriCoDare and AnaGrp = 'CL') end
into #tmp from COGE.dbo.tbpri 
inner join COGE.dbo.TbregIva on RivaAnno = datepart(year,pridatagio) and PriRegiva = RivaNReg
 where pricausale = 3 and datepart(year,pridatagio) > @DA  
 
UPDATE TBDOC SET docreg = PRIREGIVA,DOCPROT = PRINUMPROT from #tmp ,Tbdoc where docdata = PriDataEst AND DOCANAPROG =anaprog 
AND doctipo = 'FF' AND SUBSTRING(DOCDESC,12,CHARINDEX('DEL',DocDesc)-13) = PRIDOCEST AND DOCREG = 0

UPDATE TBDOC SET docreg = PRIREGIVA,DOCPROT = PRINUMPROT from #tmp ,Tbdoc where docdata = PriDataEst AND DOCANAPROG =anaprog 
AND doctipo = 'XF' AND SUBSTRING(DOCDESC,12,CHARINDEX('DEL',DocDesc)-13) = PRIDOCEST AND DOCREG = 0

 
delete #TMP from #TMP inner join Tbdoc on DocReg = PriRegIva and DocProt = PriNumProt and docdata = PriDataEst

SELECT *,AnaDesc = case when RivaTipo = 2 Or RivaTipo = 4 Then (select AnaDesc from TbAna Where AnaCod = PriCoAvere and AnaGrp = 'FO') 
else (select AnaDesc from TbAna Where AnaCod = PriCoDare and AnaGrp = 'CL') end
FROM #TMP
order by AnnoCoge,PriRegiva,PriNumProt,PriBisRet 
GO
/****** Object:  StoredProcedure [dbo].[XRILDOC]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[XRILDOC] @RIF as int,@TIPO as varchar(2),@BLOCK AS INT
AS

DELETE from TMPtbDoc where TMPDocBLOCK = @BLOCK

DECLARE @DOCRIF AS INT, @DOCFF as INT,@DOCBF as INT,@TD as varchar(2),@CL as varchar(2)
DECLARE @RIFCNT AS INT

SET @DOCRIF =(SELECT DISTINCT DOCNUMRIF FROM TBDOC WHERE DOCKEYCM = @RIF)
SET @CL = (SELECT ANAGRP FROM TBANA WHERE ANAPROG = (SELECT DOCANAPROG FROM TBDOC WHERE DOCKEYCM = @RIF))


SET @RIFCNT = -1

INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDocTipo,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DocTipo,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc Where DocKeyCm = @RIF



IF @TIPO = 'NC' GOTO ESCI 
	
---+++++++++++++++++++++++++++++++++ FATTURA FORNITORE ++++++++++++++++++++++++++
IF @TIPO = 'FF' 
BEGIN

 -- BOLLE FORNITORI 
set @TD = 'BF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND DOCNUMRIF IN (SELECT TESRIF FROM GEVE.DBO.TBTES WHERE TESFATRIF = @DOCRIF) 

-- ORDINI FORNITORI 
set @TD = 'OF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND DOCNUMRIF IN (SELECT DISTINCT CORORDRIF FROM GEVE.DBO.TBCOR WHERE CORRIF IN(SELECT TESRIF FROM GEVE.DBO.TBTES WHERE TESFATRIF = @DOCRIF)) 

GOTO ESCI

END


---+++++++++++++++++++++++++++++++++ BOLLA FORNITORE ++++++++++++++++++++++++++
IF @TIPO = 'BF' 
BEGIN
-- ORDINE FORNITORE
set @TD = 'OF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND DOCNUMRIF IN (SELECT DISTINCT CORORDRIF FROM GEVE.DBO.TBCOR INNER JOIN GEVE.DBO.TBTES ON TESRIF = CORRIF AND TESTIPODOC = cortipodoc  WHERE CORRIF = @DOCRIF) 

set @TD = 'FF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where DOCTIPO = @TD AND DOCNUMRIF > 0 AND DOCNUMRIF IN (SELECT DISTINCT TESFATRIF FROM GEVE.DBO.TBTES WHERE TESRIF =@DOCRIF) 
GOTO ESCI

END

---+++++++++++++++++++++++++++++++++ FATTURA FORNITORE ACCOMPAGANTORIA ++++++++++++++++++++++++++
IF @TIPO = 'XF' 
BEGIN
-- ORDINI FORNITORI 
set @TD = 'OF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND DOCNUMRIF IN (SELECT DISTINCT CORORDRIF FROM GEVE.DBO.TBCOR INNER JOIN GEVE.DBO.TBTES ON TESRIF = CORRIF AND TESTIPODOC = cortipodoc WHERE CORRIF = @DOCRIF) 

GOTO ESCI

END



---+++++++++++++++++++++++++++++++++ ORDINE FORNITORE ++++++++++++++++++++++++++
IF @TIPO = 'OF' 
BEGIN

-- BOLLA FO
set @TD = 'BF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where DOCTIPO = @TD AND DOCNUMRIF > 0 AND DOCNUMRIF 
IN (SELECT DISTINCT CORRIF FROM GEVE.DBO.TBCOR INNER JOIN GEVE.DBO.TBTES ON TESRIF = CORRIF AND TESTIPODOC = cortipodoc WHERE CORORDRIF = @DOCRIF) 

-- FATTURA 
set @TD = 'FF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where DOCTIPO = @TD AND DOCNUMRIF > 0 AND DOCNUMRIF 
IN (SELECT DISTINCT TESFATRIF FROM GEVE.DBO.TBTES WHERE TESRIF 
IN (SELECT DISTINCT CORRIF FROM GEVE.DBO.TBCOR WHERE CORORDRIF = @DOCRIF))
GOTO ESCI

END


---+++++++++++++++++++++++++++++++++ ALTRO DOCUMENTO FORNITORE ++++++++++++++++++++++++++
IF @TIPO = 'AF' 
BEGIN
-- ORDINE FORNITORE
set @TD = 'OF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND DOCNUMRIF > 0 AND DOCNUMRIF IN (SELECT DISTINCT CORORDRIF FROM GEVE.DBO.TBCOR WHERE CORRIF = @DOCRIF) 

-- FATTURA 
set @TD = 'FF'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where DOCTIPO = @TD AND DOCNUMRIF > 0 AND DOCNUMRIF 
IN (SELECT DISTINCT TESFATRIF FROM GEVE.DBO.TBTES WHERE TESRIF = @DOCRIF) 
GOTO ESCI

END
-----------------------------------FATTURE CLIENTI (REG 7 E REG. 1)-------------------------------------------
IF @TIPO = 'FC' 
BEGIN

 -- BOLLE CLIENTI
set @TD = 'BC'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND DOCNUMRIF IN (SELECT BolRif FROM GEVE.DBO.TBBol WHERE BolRifFat = @DOCRIF) 

-- ORDINI CLIENTI
set @TD = 'OC'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND 
      ( DOCNUMRIF IN (SELECT DISTINCT CORORDRIF FROM GEVE.DBO.TBCOR WHERE CORTIPODOC = 'B' AND CORRIF IN (SELECT BOLRIF FROM GEVE.DBO.TBBOL WHERE BolRifFat = @DOCRIF)) 
	   OR
	    DOCNUMRIF IN (SELECT DISTINCT CORORDRIF FROM GEVE.DBO.TBCOR WHERE CORTIPODOC = 'F' AND CORRIF = @DOCRIF))
GOTO ESCI

END

---+++++++++++++++++++++++++++++++++ BOLLA CLIENTE ++++++++++++++++++++++++++
IF @TIPO = 'BC' 
BEGIN
-- ORDINE FORNITORE
set @TD = 'OC'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where  DOCTIPO = @TD AND DOCNUMRIF IN (SELECT DISTINCT CORORDRIF FROM GEVE.DBO.TBCOR WHERE CORTIPODOC = 'B' and CORRIF = @DOCRIF) 

set @TD = 'FC'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where DOCTIPO = @TD AND DOCNUMRIF > 0 AND DOCNUMRIF IN (SELECT DISTINCT BOLRIFFAT FROM GEVE.DBO.TBBOL WHERE BOLRIF =@DOCRIF) 
GOTO ESCI

END

---+++++++++++++++++++++++++++++++++ ORDINE CLIENTE ++++++++++++++++++++++++++
IF @TIPO = 'OC' 
BEGIN

-- BOLLA CLI
set @TD = 'BC'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where DOCTIPO = @TD AND DOCNUMRIF > 0 AND DOCNUMRIF 
IN (SELECT DISTINCT CORRIF FROM GEVE.DBO.TBCOR WHERE CORTIPODOC = 'B' AND CORORDRIF = @DOCRIF) 

-- FATTURA 
set @TD = 'FC'
INSERT INTO TMPTbDoc(TMPDocBLOCK,TMPDocAnno,TMPDOCTIPO,TMPDocRifInterno,TMPDocEst,TMPDocAnaProg,
TMPDocData,TMPDocDesc,TMPDocNote,TMPDocNumRif,TMPDocProt,TMPDocReg,TMPDocAnnoCoge,TMPDocKeyCm)
	
select @BLOCK,DocAnno,DOCTIPO,DocRifInterno,DocEst,DocAnaProg,DocData,DocDesc,DocNote,DocNumRif,
DocProt,DocReg,DocAnnoCoge,DocKeyCm FROM TbDoc 
Where DOCTIPO = @TD AND DOCNUMRIF > 0 AND
 (DOCNUMRIF IN (SELECT DISTINCT CORRIF FROM GEVE.DBO.TBCOR WHERE CORTIPODOC = 'F' AND CORORDRIF = @DOCRIF) 
    OR 
  DOCNUMRIF IN  (SELECT DISTINCT BOLRIFFAT FROM GEVE.DBO.TBBOL WHERE BOLRIF IN (SELECT DISTINCT CORRIF FROM GEVE.DBO.TBCOR WHERE CORTIPODOC = 'B' AND CORORDRIF = @DOCRIF)) )

GOTO ESCI

END

ESCI:

GO
/****** Object:  StoredProcedure [dbo].[XVISDOC]    Script Date: 20/05/2026 15:38:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XVISDOC] @ID as int
AS
SELECT  AnaDesc,DocAnno,TdoDesc,DocDesc =REPLACE(docdesC,TdoInizialeDesc + ' NR. ' ,'') , 
DocPath=DocAnno + '\' + DocTipo + DocRifInterno + '.' + DocEst,
AnnStor=ISNULL(AnnStor, ''),
DocNumrif=ISNULL(DocNumRif, 0),
DocRifInterno,
AnaProg,AnaGrp,GrpDesc,DocNote,DocProt,DocReg,TdoCod,DocAnnoCoge,DocKeyCm,
DocNum = cast(0 as int),
DocData = cast(getdate() as smalldatetime),
DocP = REPLACE(REPLACE(docdesC,TdoInizialeDesc + ' NR. ' ,''),'DEL ','@'),
RifPri = ISNULL((select top 1 PriId from COGE.dbo.TbPri WHERE PridocAnn = DocAnno and Prinumprot = DocProt and priRegiva = DocReg),0),
FteNomeFile = cast('' as varchar(50)),
DocEst
INTO #TMP FROM TbDoc
INNER JOIN TbAna ON AnaProg = DocAnaProg
INNER JOIN TbTdo ON DocTipo = TdoCod
INNER JOIN TbAnn ON DocAnno = AnnAnno
INNER JOIN COGE.dbo.TbGrp ON AnaGrp = GrpCod
WHERE DocAnno IN(SELECT QA FROM tBqANN)


UPDATE #TMP SET FteNomeFile = isnull((SELECT top 1 FteNomeFile from  geve.dbo.tbfte_passiva where  fterifpri = rifpri and rifpri <> 0),'')

DELETE FROM #TMP WHERE TDOCOD IN (SELECT TdoCodGrup from TbTdoGr where TdoGrupId = @ID)

SELECT * from #TMP ORDER BY AnaGrp,AnaDesc,DocAnno,DocData,TdoCod,DocReg,DocProt,DocDesc

GO
USE [master]
GO
ALTER DATABASE [VDOX] SET  READ_WRITE 
GO
