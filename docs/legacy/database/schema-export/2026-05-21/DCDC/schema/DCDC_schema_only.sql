USE [master]
GO
/****** Object:  Database [DCDC]    Script Date: 21/05/2026 14:15:04 ******/
CREATE DATABASE [DCDC]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DCDC_Data', FILENAME = N'C:\DATI\DBCLIENTI\XSEL\DCDC.mdf' , SIZE = 40960KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'DCDC_Log', FILENAME = N'C:\DATI\DBCLIENTI\XSEL\DCDC_1.ldf' , SIZE = 25480KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO
ALTER DATABASE [DCDC] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DCDC].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [DCDC] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DCDC] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DCDC] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DCDC] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DCDC] SET ARITHABORT OFF 
GO
ALTER DATABASE [DCDC] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DCDC] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DCDC] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DCDC] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DCDC] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DCDC] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DCDC] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DCDC] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DCDC] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DCDC] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DCDC] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DCDC] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DCDC] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DCDC] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DCDC] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DCDC] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DCDC] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DCDC] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DCDC] SET  MULTI_USER 
GO
ALTER DATABASE [DCDC] SET PAGE_VERIFY TORN_PAGE_DETECTION  
GO
ALTER DATABASE [DCDC] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DCDC] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DCDC] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DCDC] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DCDC', N'ON'
GO
USE [DCDC]
GO
/****** Object:  User [REDACTED_DB_USER_1]    Script Date: REDACTED ******/
-- REDACTED: database user omitted from documentation export
GO
/****** Object:  Schema [REDACTED_DB_SCHEMA_1]    Script Date: REDACTED ******/
-- REDACTED: database schema/user placeholder omitted from documentation export
GO
/****** Object:  Table [dbo].[TBCDC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBCDC](
	[CdcCod] [smallint] NOT NULL,
	[CdcDes] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TBCDC] PRIMARY KEY CLUSTERED 
(
	[CdcCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBCOG]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBCOG](
	[CogLdp] [smallint] NOT NULL,
	[CogCdc] [smallint] NOT NULL,
	[CogRep] [smallint] NOT NULL,
	[CogConto] [varchar](5) NOT NULL,
	[CogPerc] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_TBCOG] PRIMARY KEY CLUSTERED 
(
	[CogLdp] ASC,
	[CogCdc] ASC,
	[CogRep] ASC,
	[CogConto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBDMC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBDMC](
	[DCGMCID] [int] NOT NULL,
	[DCGMCPROG] [smallint] NOT NULL,
	[DCGMCCogLdp] [smallint] NOT NULL,
	[DCGMCCogCdc] [smallint] NOT NULL,
	[DCGMCCogRep] [smallint] NOT NULL,
	[DCGMCCogConto] [varchar](5) NOT NULL,
	[DCGMCIMPORTO] [decimal](13, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBIMC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBIMC](
	[IDMCC] [int] IDENTITY(1,1) NOT NULL,
	[IDMCDT] [smalldatetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBLDP]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBLDP](
	[LdpRif] [smallint] IDENTITY(1,1) NOT NULL,
	[LdpSigla] [varchar](12) NOT NULL,
	[LdpDesc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TBLDP_1] PRIMARY KEY CLUSTERED 
(
	[LdpSigla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBMCC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBMCC](
	[MCCID] [int] NOT NULL,
	[MCCPROG] [smallint] NOT NULL,
	[MCCPrkAammgg] [smalldatetime] NOT NULL,
	[MCCPrkId] [int] NOT NULL,
	[MCCPrkProg] [smallint] NOT NULL,
	[MCCPrkDa] [varchar](1) NOT NULL,
	[MCCCogLdp] [smallint] NOT NULL,
	[MCCCogCdc] [varchar](1) NOT NULL,
	[MCCCogRep] [smallint] NOT NULL,
	[MCCCogDet] [smallint] NOT NULL,
	[MCCCogConto] [varchar](5) NOT NULL,
	[MCCIMPORTO] [decimal](13, 2) NOT NULL,
 CONSTRAINT [PK_TBMCC] PRIMARY KEY CLUSTERED 
(
	[MCCID] ASC,
	[MCCPROG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBREP]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBREP](
	[RepCod] [smallint] NOT NULL,
	[RepDes] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TBREP] PRIMARY KEY CLUSTERED 
(
	[RepCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPCDC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPCDC](
	[IDTMP] [int] NOT NULL,
	[TLDP] [smallint] NOT NULL,
	[TCDC] [smallint] NOT NULL,
	[TREP] [smallint] NOT NULL,
	[TCONTO] [varchar](5) NOT NULL,
	[QSALDO] [decimal](11, 2) NOT NULL,
	[TLDPDESC] [varchar](50) NULL,
	[TCDCDESC] [varchar](50) NULL,
	[TREPDESC] [varchar](50) NULL,
	[TCONTODESC] [varchar](32) NULL,
	[TCR] [smallint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMPTCM]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMPTCM](
	[TMPID] [int] NOT NULL,
	[LDPRIF] [smallint] NOT NULL,
	[LDPSIGLA] [varchar](12) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRIMC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRIMC](
	[IDMCC] [int] IDENTITY(1,1) NOT NULL,
	[IDMCDT] [smalldatetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TRMCC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRMCC](
	[MCCID] [int] NOT NULL,
	[MCCPROG] [smallint] NOT NULL,
	[MCCPrkAammgg] [smalldatetime] NOT NULL,
	[MCCPrkId] [int] NOT NULL,
	[MCCPrkProg] [smallint] NOT NULL,
	[MCCPrkDa] [varchar](1) NOT NULL,
	[MCCCogLdp] [smallint] NOT NULL,
	[MCCCogCdc] [varchar](1) NOT NULL,
	[MCCCogRep] [smallint] NOT NULL,
	[MCCCogDet] [smallint] NOT NULL,
	[MCCCogConto] [varchar](5) NOT NULL,
	[MCCIMPORTO] [decimal](13, 2) NOT NULL,
 CONSTRAINT [PK_TRMCC] PRIMARY KEY CLUSTERED 
(
	[MCCID] ASC,
	[MCCPROG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[FnCDCF1]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[FnCDCF1](@dal smalldatetime,@al smalldatetime)
returns table
as
return
select MCCCOGLDP,LDPDES,MCCCOGCDC,CDCDES,MCCCOGREP,REPDES,MCCCOGCONTO,
CONTODES =ISNULL((SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE PIACODCO = MCCCOGCONTO),'***ERRATO***'),
MCCIMPORTO = case when (SELECT Piafl01 FROM COGE.DBO.TBPIA WHERE PIACODCO = MCCCOGCONTO) = 7 and MCCprkda = 0 then SUM(MCCIMPORTO * -1)
when (SELECT Piafl01 FROM COGE.DBO.TBPIA WHERE PIACODCO = MCCCOGCONTO) = 6 and MCCprkda = 1 then SUM(MCCIMPORTO * -1)
ELSE SUM(MCCIMPORTO) END,
PIAFL01 = (SELECT Piafl01 FROM COGE.DBO.TBPIA WHERE PIACODCO = MCCCOGCONTO) 
from TBMCC 
INNER JOIN TBLDP ON MCCCOGLDP = LDPCOD 
INNER JOIN TBCDC ON MCCCOGCDC = CDCCOD
INNER JOIN TBREP ON MCCCOGREP = REPCOD
WHERE mccPrkaammgg between @DAL and @AL
GROUP BY MCCCOGLDP,LDPDES,MCCCOGCDC,CDCDES,MCCCOGREP,REPDES,MCCCOGCONTO,mccprkda


GO
/****** Object:  UserDefinedFunction [dbo].[FnRipCdc]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[FnRipCdc](@ID int)
returns table
as
return
select 0 AS RIGA,ISNULL(MCCID,0) AS MCCID,ISNULL(MCCPROG,0)AS MCCPROG,ISNULL(MCCIMPORTO,0)AS MCCIMPORTO,
PRKAAMMGG,PRKID,PRKPROG,PRKDA,
COGLDP,LDPDES,COGCDC,CDCDES,COGREP,REPDES,COGPERC,COGCONTO,
CONTODES =ISNULL((SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE PIACODCO = COGCONTO),'***ERRATO***'),
IMPORTO = CONVERT(DECIMAL(13,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end)
 when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END),
COSTO = CONVERT(DECIMAL(13,2),0) 
from COGE.DBO.TBPRK 
INNER JOIN TBCOG ON COGCONTO = PRKCONTO 
INNER JOIN TBLDP ON COGLDP = LDPCOD 
INNER JOIN TBCDC ON COGCDC = CDCCOD
INNER JOIN TBREP ON COGREP = REPCOD
LEFT OUTER JOIN TBMCC ON MCCPRKID = PRKID AND MCCPRKPROG = PRKPROG AND MCCPRKDA = PRKDA and (COGLDP <> MCCCOGLDP AND COGCDC <> MCCCOGCDC AND COGREP <> MCCCOGREP)
INNER JOIN COGE.DBO.TBPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@id

GO
/****** Object:  UserDefinedFunction [dbo].[FnRipRip]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[FnRipRip](@ID int)
returns table
as
return
select 0 AS RIGA,ISNULL(MCCID,0) AS MCCID,ISNULL(MCCPROG,0)AS MCCPROG,ISNULL(MCCIMPORTO,0)AS MCCIMPORTO,
PRKAAMMGG,PRKID,PRKPROG,PRKDA,
COGLDP,LDPDES,COGCDC,CDCDES,COGREP,REPDES,COGPERC,COGCONTO,
CONTODES =ISNULL((SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE PIACODCO = COGCONTO),'***ERRATO***'),
IMPORTO = CONVERT(DECIMAL(13,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) 
when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END),
COSTO = CONVERT(DECIMAL(13,2),0) 
from TBMCC
INNER JOIN COGE.DBO.TBPRK ON MCCPRKID = PRKID AND MCCPRKPROG = PRKPROG AND MCCPRKDA = PRKDA
INNER JOIN TBCOG ON COGCONTO = PRKCONTO and COGLDP = MCCCOGLDP AND COGCDC = MCCCOGCDC AND COGREP = MCCCOGREP
INNER JOIN COGE.DBO.TBPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
inner JOIN TBLDP ON COGLDP = LDPCOD 
inner JOIN TBCDC ON COGCDC = CDCCOD
inner JOIN TBREP ON COGREP = REPCOD
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@ID

GO
/****** Object:  View [dbo].[CRPIACDC]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[CRPIACDC]
AS
SELECT     TOP 100 PERCENT COGE.dbo.TbPia.PiaCodCo AS CodiceSottoconto, COGE.dbo.TbPia.PiaAnaCo AS Descrizione, SUBSTRING(COGE.dbo.TbPia.PiaCodCo, 1, 2) 
                      AS MASTRO, TbPia_1.PiaCodCo, TbPia_1.PiaAnaCo, TbPia_1.PiaFl01, TbPia_1.PiaFl02, TbPia_1.PiaFl03, TbPia_1.PiaFl04, TbPia_1.PiaFl05, 
                      TbPia_1.PiaFl06, TbPia_1.PiaFl07, PiaFl08 = cast(TbPia_1.PiaFl08 as smallint), PiaFl09=cast(TbPia_1.PiaFl09 as smallint), TbPia_1.PiaFl10, TbPia_1.PiaFl11, TbPia_1.PiaFl12,
					  Cdc = case when exists (select CogConto from dcdc.dbo.TBCOG where TbPia_1.PiaCodCo=Cogconto) then cast(1 as bit)  else cast(0 as bit) end
FROM         COGE.dbo.TbPia LEFT OUTER JOIN
                      COGE.dbo.TbPia TbPia_1 ON SUBSTRING(COGE.dbo.TbPia.PiaCodCo, 1, 2) = SUBSTRING(TbPia_1.PiaCodCo, 1, 2) AND SUBSTRING(TbPia_1.PiaCodCo, 4, 2) <> '00'
	WHERE     (SUBSTRING(COGE.dbo.TbPia.PiaCodCo, 4, 2) = '00')
ORDER BY COGE.dbo.TbPia.PiaCodCo





GO
/****** Object:  View [dbo].[VCDCCPT]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VCDCCPT]
AS
SELECT     dbo.TBCOG.CogLdp, dbo.TBCOG.CogCdc, dbo.TBCOG.CogRep, dbo.TBCOG.CogConto, dbo.TBCDC.CdcDes, dbo.TBLDP.LdpDes, dbo.TBREP.RepDes, 
                      COGE.dbo.TbPia.PiaAnaCo
FROM         dbo.TBCOG INNER JOIN
                      dbo.TBLDP ON dbo.TBCOG.CogLdp = dbo.TBLDP.LdpCod INNER JOIN
                      dbo.TBCDC ON dbo.TBCOG.CogCdc = dbo.TBCDC.CdcCod INNER JOIN
                      dbo.TBREP ON dbo.TBCOG.CogRep = dbo.TBREP.RepCod INNER JOIN
                      COGE.dbo.TbPia ON dbo.TBCOG.CogConto = COGE.dbo.TbPia.PiaCodCo
GO
/****** Object:  View [dbo].[VCDCH8]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VCDCH8]
AS
SELECT     TOP 100 PERCENT priartfisc, MCCCOGCONTO, prkTipoCo, PRKDESC =
                          (SELECT     PIAANACO
                            FROM          coge.dbo.TBPIA
                            WHERE      PIACODCO = MCCCOGCONTO), pridatagio, pridataest, cau.CiiCau AS Causale, PriNumProt, PriregIva, PriDocEst, 
                      CPT = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare END, 
                      CPTDESC = ISNULL
                          ((SELECT     PIAANACO
                              FROM         coge.dbo.TBPIA
                              WHERE     PIACODCO = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE
                                                     pricodare END),
                          (SELECT     ANADESC
                            FROM          VDOX.DBO.TBANA
                            WHERE      ANACOD = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare
                                                    END AND ANAGRP = 'CL' OR
                                                   ANACOD = CASE WHEN PriCausale = 1 THEN PriCoDare WHEN pricausale = 2 THEN PriCoAvere WHEN prkda = 0 THEN priCoAvere ELSE pricodare
                                                    END AND ANAGRP = 'FO')), 
DARE = CONVERT(DECIMAL(13, 2), case when PriCausale = 1 then 0 when pricausale = 2 then MCCIMPORTO when prkda = 0 then MCCIMPORTO else 0 end),
AVERE = CONVERT(DECIMAL(13, 2), case when PriCausale = 1 then MCCIMPORTO when pricausale = 2 then 0 when prkda = 0 then 0 else  MCCIMPORTO end), pridesc + pridescb AS descriz, PrkAammgg, PIAFL =
                          (SELECT     PIAFL01
                            FROM          coge.dbo.TBPIA
                            WHERE      PIACODCO = mccCOGCONTO), PRICAUSALE, MCPT = CASE WHEN PRICAUSALE = 3 THEN
                          (SELECT     CASE WHEN PRICODARE <> MCCCOGCONTO THEN PRICODARE ELSE PRICOAVERE END
                            FROM          coge.dbo.TBPRI AS Q
                            WHERE      Q.PRIID = PRKID AND Q.PRIPROG = (PRKPROG - 1)) ELSE '*' END, MCCCOGLDP, LDPDES, MCCCOGCDC, CDCDES, MCCCOGREP, 
                      REPDES
FROM         TBMCC INNER JOIN
                      TBLDP ON MCCCOGLDP = LDPCOD INNER JOIN
                      TBCDC ON MCCCOGCDC = CDCCOD INNER JOIN
                      TBREP ON MCCCOGREP = REPCOD INNER JOIN
                      coge.dbo.Tbprk ON prkId = MccPrkId AND PrkProg = MccPrkProg AND PrkDa = MccPrkDa INNER JOIN
                      coge.dbo.Tbpri ON MccPrkId = Priid AND MccPrkProg = Priprog LEFT OUTER JOIN
                      coge.dbo.tbcii AS cau ON pricausale = cau.ciicod


GO
/****** Object:  View [dbo].[VCOG]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[VCOG]
AS
SELECT CogLdp,CogCdc,CogRep,CogConto,CogPerc,PiaAnaCo 
from TBCOG INNER JOIN COGE.DBO.TBPIA
ON COGCONTO = PIACODCO




GO
/****** Object:  View [dbo].[VPIA]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE View [dbo].[VPIA]
as
select PiaCodCo,PiaAnaCo,
Cr =case when PiaFl01= 7 then 'R' else 'C' end
from coge.dbo.tbpia where piafl01 = 7 or piafl01 = 6



GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_TBLDP]    Script Date: 21/05/2026 14:15:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TBLDP] ON [dbo].[TBLDP]
(
	[LdpSigla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TBCDC] ADD  CONSTRAINT [DF_TBCDC_CdcDes]  DEFAULT ('') FOR [CdcDes]
GO
ALTER TABLE [dbo].[TBCOG] ADD  CONSTRAINT [DF_TBCOG_CogCdc]  DEFAULT (0) FOR [CogCdc]
GO
ALTER TABLE [dbo].[TBCOG] ADD  CONSTRAINT [DF_TBCOG_CogRep]  DEFAULT (0) FOR [CogRep]
GO
ALTER TABLE [dbo].[TBCOG] ADD  CONSTRAINT [DF_TBCOG_CogConto]  DEFAULT ('00.00') FOR [CogConto]
GO
ALTER TABLE [dbo].[TBCOG] ADD  CONSTRAINT [DF_TBCOG_CogPerc]  DEFAULT (0.00) FOR [CogPerc]
GO
ALTER TABLE [dbo].[TBLDP] ADD  CONSTRAINT [DF_TBLDP_LdpDes]  DEFAULT ('') FOR [LdpDesc]
GO
ALTER TABLE [dbo].[TBMCC] ADD  CONSTRAINT [DF_TBMCC_MCCCogCdc]  DEFAULT ('0') FOR [MCCCogCdc]
GO
ALTER TABLE [dbo].[TBMCC] ADD  CONSTRAINT [DF_TBMCC_MCCCogRep]  DEFAULT ((0)) FOR [MCCCogRep]
GO
ALTER TABLE [dbo].[TBMCC] ADD  CONSTRAINT [DF_TBMCC_MCCCogDet]  DEFAULT ((0)) FOR [MCCCogDet]
GO
ALTER TABLE [dbo].[TBMCC] ADD  CONSTRAINT [DF_TBMCC_MCCCogConto]  DEFAULT ('00.00') FOR [MCCCogConto]
GO
ALTER TABLE [dbo].[TBMCC] ADD  CONSTRAINT [DF_TBMCC_MCCIMPORTO]  DEFAULT ((0)) FOR [MCCIMPORTO]
GO
ALTER TABLE [dbo].[TBREP] ADD  CONSTRAINT [DF_TBREP_RepDes]  DEFAULT ('') FOR [RepDes]
GO
ALTER TABLE [dbo].[TMPTCM] ADD  CONSTRAINT [DF_TMPTCM_LDPSIGLA]  DEFAULT ('') FOR [LDPSIGLA]
GO
ALTER TABLE [dbo].[TRMCC] ADD  CONSTRAINT [DF_TRMCC_MCCCogCdc]  DEFAULT ((0)) FOR [MCCCogCdc]
GO
ALTER TABLE [dbo].[TRMCC] ADD  CONSTRAINT [DF_TRMCC_MCCCogRep]  DEFAULT ((0)) FOR [MCCCogRep]
GO
ALTER TABLE [dbo].[TRMCC] ADD  CONSTRAINT [DF_TRMCC_MCCCogDet]  DEFAULT ((0)) FOR [MCCCogDet]
GO
ALTER TABLE [dbo].[TRMCC] ADD  CONSTRAINT [DF_TRMCC_MCCCogConto]  DEFAULT ('00.00') FOR [MCCCogConto]
GO
ALTER TABLE [dbo].[TRMCC] ADD  CONSTRAINT [DF_TRMCC_MCCIMPORTO]  DEFAULT ((0)) FOR [MCCIMPORTO]
GO
/****** Object:  StoredProcedure [dbo].[RLDPCREA]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[RLDPCREA] @ID  as int,@MCC as int
as

if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABCM'))
   begin
   drop table ##TABCM
  end   
  
  if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABTO'))
   begin
   drop table TABTO
  end   
  
select @MCC as MCCID,ISNULL(MCCPROG,0)AS MCCPROG,ISNULL(MCCIMPORTO,0)AS MCCIMPORTO,
PRKAAMMGG,PRKID,PRKPROG,PRKDA,PRKCONTO,
ISNULL(LDPRIF,0) AS TCMRIF,ISNULL(LDPSIGLA,'') AS TCMSIGLA,
CONTODES =ISNULL((SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE PIACODCO = PRKCONTO),'***ERRATO***'),
IMPORTO = CONVERT(DECIMAL(12,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) 
when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END)
INTO ##TABCM from TMPTCM 
INNER JOIN  COGE.DBO.TRPRK ON PrkId = TMPID
INNER JOIN COGE.DBO.TRPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
INNER JOIN TBCOG ON COGCONTO = PRKCONTO 
LEFT OUTER JOIN TRMCC ON MCCPRKID = PRKID AND MCCPRKPROG = PRKPROG AND MCCPRKDA = PRKDA
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@ID

select distinct PRKCONTO,IMPORTO INTO ##TABTO from ##TABCM 





GO
/****** Object:  StoredProcedure [dbo].[RLDPRIP]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[RLDPRIP](@ID int)
as

if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABCM'))
   begin
   drop table ##TABCM
  end   

if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABTO'))
   begin
   drop table ##TABTO
  end   
  
select ISNULL(MCCID,0) AS MCCID,ISNULL(MCCPROG,0)AS MCCPROG,ISNULL(MCCIMPORTO,0)AS MCCIMPORTO,
PRKAAMMGG,PRKID,PRKPROG,PRKDA,PRKCONTO,
ISNULL(LDPRIF,0) AS LDPRIF,ISNULL(LDPSIGLA,'') AS LDPSIGLA,
CONTODES =ISNULL((SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE PIACODCO = PRKCONTO),'***ERRATO***'),
IMPORTO = CONVERT(DECIMAL(12,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) 
when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END)
INTO ##TABCM from COGE.DBO.TRPRK
INNER JOIN COGE.DBO.TRPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
INNER JOIN TBCOG ON COGCONTO = PRKCONTO 
LEFT OUTER JOIN TRMCC ON MCCPRKID = PRKID AND MCCPRKPROG = PRKPROG AND MCCPRKDA = PRKDA
LEFT OUTER JOIN TBLDP ON MCCCogLdp = LdpRif 
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@ID

select distinct PRKCONTO,IMPORTO INTO ##TABTO from ##TABCM 






GO
/****** Object:  StoredProcedure [dbo].[XDADCG]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XDADCG] @ID as int
AS
select * from coge.dbo.tbprk inner join coge.dbo.TbPri on  PrkId = PriId and PrkProg = PriProg
where PrkId = @ID and PrkConto in (select distinct CogConto from TBCOG)
order by PrkId,prkprog

GO
/****** Object:  StoredProcedure [dbo].[XGRIDFAT]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XGRIDFAT] @ANNO as smallint, @REG AS SMALLINT
AS
--DECLARE @ANNO as smallint, @REG AS SMALLINT
--SET @ANNO=2016
--SET @REG=8
select DISTINCT PriId,PriDocEst,PriDataEst, 
Cconto=case when Pricausale = 2 then Pricoavere else pricodare end,
AnaDesc=case when Pricausale = 2 then (select Anadesc from Vdox.dbo.tbana where anacod = Pricoavere and anagrp = 'FO') else
(select Anadesc from Vdox.dbo.tbana where anacod = PriCoDare and anagrp = 'CL') end,
NC = case when MCCCOGDET < 1 THEN 0 ELSE 1 END,
PriNumProt,PriDataGio,
DAREAVERE = case when MCCPrkDa = 0 THEN 'D' else 'A' end,
FdcgNumRif = isnull((select top 1 FdcgNumRif from geve.dbo.tbndcg where FDcgNumero = pridocest and fdcgdata=pridataest and fdcgcli=
(case when Pricausale = 2 then Pricoavere else pricodare end) and fdcgregistro=PriRegIva),-1),PriDesc,
Valore = Cast(null as decimal(12,2)),FRifInterno='000000',S1='0',PriDescrizione=PriDesc + PriDescB
into #TMPFAT
 from tbmcc
 inner join coge.dbo.tbpri as a on a.priid = mccprkid and priprog = mccprkprog
 where datepart(year,pridatagio) = @ANNO and priregiva = @REG and a.pricausale < 3

 UPDATE #TMPFAT SET VALORE = CASE WHEN dareavere='D' then PRIIMPAVERE - PRIIMPDARE else PRIIMPDARE - PRIIMPAVERE END from #TMPFAT AS A INNER JOIN COGE.DBO.TBPRI AS B ON A.PRIID = B.PRIID AND B.PRICAUSALE = 3
 UPDATE #TMPFAT SET FRifInterno =isNull((select DocRifInterno from Vdox.dbo.TbDoc where FdcgNumRif = DocNumRif AND DocTipo= 'FF'),'000000')
 UPDATE #TMPFAT SET S1 ='1' WHERE FRifInterno > '000000' 
 
 SELECT DISTINCT PRIID,N=COUNT(PRIID) INTO #TMP FROM #TMPFAT GROUP BY PRIID HAVING COUNT(PRIID) > 1

 UPDATE #TMPFAT SET NC=0 WHERE PRIID IN(SELECT PRIID FROM #TMP)
 
  SELECT DISTINCT * FROM #TMPFAT ORDER BY PriNumProt

GO
/****** Object:  StoredProcedure [dbo].[XGRIDFATPRINT]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XGRIDFATPRINT] @ANNO as smallint, @REG AS SMALLINT
AS
--DECLARE @ANNO as smallint, @REG AS SMALLINT
--SET @ANNO=2015
--SET @REG=8
select DISTINCT PriId,PriDocEst,PriDataEst, 
Cconto=case when Pricausale = 2 then Pricoavere else pricodare end,
AnaDesc=case when Pricausale = 2 then (select Anadesc from Vdox.dbo.tbana where anacod = Pricoavere and anagrp = 'FO') else
(select Anadesc from Vdox.dbo.tbana where anacod = PriCoDare and anagrp = 'CL') end,
NC = case when MCCCOGDET < 1 THEN 0 ELSE 1 END,
PriNumProt,PriDataGio,
DAREAVERE = case when MCCPrkDa = 0 THEN 'D' else 'A' end,
FdcgNumRif = isnull((select top 1 FdcgNumRif from geve.dbo.tbndcg where FDcgNumero = pridocest and fdcgdata=pridataest and fdcgcli=
(case when Pricausale = 2 then Pricoavere else pricodare end) and fdcgregistro=PriRegIva),0),PriDesc,
Valore = Cast(null as decimal(12,2)),FRifInterno='000000',S1='0',PriDescrizione=PriDesc + PriDescB
into #TMPFAT
 from tbmcc
 inner join coge.dbo.tbpri as a on a.priid = mccprkid and priprog = mccprkprog
 where datepart(year,pridatagio) = @ANNO and priregiva = @REG and a.pricausale < 3

 UPDATE #TMPFAT SET VALORE = CASE WHEN dareavere='D' then PRIIMPAVERE - PRIIMPDARE else PRIIMPDARE - PRIIMPAVERE END from #TMPFAT AS A INNER JOIN COGE.DBO.TBPRI AS B ON A.PRIID = B.PRIID AND B.PRICAUSALE = 3
 UPDATE #TMPFAT SET FRifInterno =isNull((select DocRifInterno from Vdox.dbo.TbDoc where FdcgNumRif = DocNumRif and DocTipo= 'FF'),'000000')
 UPDATE #TMPFAT SET S1 ='1' WHERE FRifInterno > '000000' 
 
 SELECT DISTINCT PRIID,N=COUNT(PRIID) INTO #TMP FROM #TMPFAT GROUP BY PRIID HAVING COUNT(PRIID) > 1

 UPDATE #TMPFAT SET NC=0 WHERE PRIID IN(SELECT PRIID FROM #TMP)
 
  SELECT DISTINCT PRIID INTO #TMPPRID FROM #TMPFAT 

  select PRIID,
LDPSIGLA = (select LdpSigla from TbLdp where MCCCogLdp = LdpRif),
VOCE ='',-- isnull((Select VOCE from geve.dbo.Vrepertorio where MccCogCdc=RepLiv1 and  MccCogRep=RepLiv2 and  MccCogDet=RepLiv3),''),-- per ora gruppo pasta
CONTO = MCCCogConto,
CONTODES = (Select PiaAnaCo from coge.dbo.tbpia where PiaCodCo = MCCCogConto),
IMPORTO = MCCIMPORTO
 from tbmcc
 inner join coge.dbo.tbpri on priid = mccprkid and priprog = mccprkprog
 where priid IN (SELECT PRIID from #TMPPRID)

GO
/****** Object:  StoredProcedure [dbo].[XGRIDPNOTA]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XGRIDPNOTA] @ANNO as smallint
as

--DECLARE @ANNO AS SMALLINT
--SET @ANNO=2015

select DISTINCT PriId,PriDocEst,PriDataEst,Cconto=case when pricoavere <>'00.10' then Pricoavere else pricodare end,
AnaDesc=case when pricodare <>'00.10' then (select PiaAnaCo from coge.dbo.tbPia where PiaCodCo = Pricodare)
when pricoavere <>'00.10' then (select PiaAnaCo from coge.dbo.tbPia where PiaCodCo = Pricoavere) else '' end,
NC = case when MCCCOGDET < 1 THEN 0 ELSE 1 END,
PriNumProt,PriDataGio,
DAREAVERE = case when MCCPrkDa = 0 THEN 'D' else 'A' end,
FdcgNumRif = cast(0 as int),
PriDesc, Valore = MCCIMPORTO,PriDescrizione=PriDesc + PriDescB
into #TMPFAT
 from tbmcc
 inner join coge.dbo.tbpri on priid = mccprkid and priprog = mccprkprog
 where datepart(year,pridatagio) = @ANNO and priregiva = 0 and pricausale > 3
 ORDER BY PriNumProt

 --SELECT DISTINCT PRIID,N=COUNT(PRIID) INTO #TMP FROM #TMPFAT GROUP BY PRIID HAVING COUNT(PRIID) > 1

 --UPDATE #TMPFAT SET NC=0 WHERE PRIID IN(SELECT PRIID FROM #TMP)
 
  SELECT DISTINCT * FROM #TMPFAT ORDER BY PriNumProt

GO
/****** Object:  StoredProcedure [dbo].[XGRIDPNOTAONE]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XGRIDPNOTAONE] @ANNO as smallint
as

--DECLARE @ANNO AS SMALLINT
--SET @ANNO=2015
--drop table #TMPPRID

select DISTINCT PriId,PriDataEst,PriNumProt,PriDataGio,
NC = case when MCCCOGDET < 1 THEN 0 ELSE 1 END
into #TMPPRID
 from tbmcc
 inner join coge.dbo.tbpri on priid = mccprkid and priprog = mccprkprog
 where datepart(year,pridatagio) = @ANNO and priregiva = 0 and pricausale > 3
 ORDER BY PriNumProt


 SELECT DISTINCT PRIID,N=COUNT(PRIID) INTO #TMP FROM #TMPPRID GROUP BY PRIID HAVING COUNT(PRIID) > 1

 UPDATE #TMPPRID SET NC=0 WHERE PRIID IN(SELECT PRIID FROM #TMP)

   SELECT DISTINCT * FROM #TMPPRID ORDER BY PriNumProt

 

GO
/****** Object:  StoredProcedure [dbo].[XGRIDPNOTAPRINT]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XGRIDPNOTAPRINT] @ANNO as smallint
as

--DECLARE @ANNO AS SMALLINT
--SET @ANNO=2015
--drop table #TMPPRID

select DISTINCT PriId,PriDataEst,PriNumProt,PriDataGio
into #TMPPRID
 from tbmcc
 inner join coge.dbo.tbpri on priid = mccprkid and priprog = mccprkprog
 where datepart(year,pridatagio) = @ANNO and priregiva = 0 and pricausale > 3
 ORDER BY PriNumProt

 select PRIID,
LDPSIGLA = (select LdpSigla from TbLdp where MCCCogLdp = LdpRif),
VOCE = '',--isnull((Select VOCE from geve.dbo.Vrepertorio where MccCogCdc=RepLiv1 and  MccCogRep=RepLiv2 and  MccCogDet=RepLiv3),''),
CONTO = MCCCogConto,
CONTODES = (Select PiaAnaCo from coge.dbo.tbpia where PiaCodCo = MCCCogConto),
DAREAVERE = case when MCCPrkDa = 0 THEN 'D' else 'A' end,
PRIDESCRIZIONE=PriDesc + PriDescB,
IMPORTO = MCCIMPORTO
 from tbmcc
 inner join coge.dbo.tbpri on priid = mccprkid and priprog = mccprkprog
 where priid IN (SELECT PRIID from #TMPPRID)


GO
/****** Object:  StoredProcedure [dbo].[XLDPCREA]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XLDPCREA] @ID  as int,@MCC as int
as

if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABCM'))
   begin
   drop table ##TABCM
  end   
  
 if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABTO'))
   begin
   drop table ##TABTO
  end   
  
select @MCC as MCCID,ISNULL(MCCPROG,0)AS MCCPROG,ISNULL(MCCIMPORTO,0)AS MCCIMPORTO,
PRKAAMMGG,PRKID,PRKPROG,PRKDA,PRKCONTO,
ISNULL(LDPRIF,0) AS LDPRIF,ISNULL(LDPSIGLA,'') AS LDPSIGLA,
CONTODES =ISNULL((SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE PIACODCO = PRKCONTO),'***ERRATO***'),
IMPORTO = CONVERT(DECIMAL(12,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) 
when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END)
INTO ##TABCM from TMPTCM 
INNER JOIN  COGE.DBO.TBPRK ON PrkId = TMPID
INNER JOIN COGE.DBO.TBPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
INNER JOIN TBCOG ON COGCONTO = PRKCONTO 

LEFT OUTER JOIN TBMCC ON MCCPRKID = PRKID AND MCCPRKPROG = PRKPROG AND MCCPRKDA = PRKDA
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@ID

select distinct PRKCONTO,IMPORTO INTO ##TABTO from ##TABCM 





GO
/****** Object:  StoredProcedure [dbo].[XLDPRIP]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[XLDPRIP](@ID int)
as

if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABCM'))
   begin
   drop table ##TABCM
  end   
  
  if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TABTO'))
   begin
   drop table ##TABTO
  end   

select ISNULL(MCCID,0) AS MCCID,ISNULL(MCCPROG,0)AS MCCPROG,ISNULL(MCCIMPORTO,0)AS MCCIMPORTO,
PRKAAMMGG,PRKID,PRKPROG,PRKDA,PRKCONTO,
ISNULL(LDPRIF,0) AS LDPRIF,ISNULL(LDPSIGLA,'') AS LDPSIGLA,
CONTODES =ISNULL((SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE PIACODCO = PRKCONTO),'***ERRATO***'),
IMPORTO = CONVERT(DECIMAL(12,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) 
when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END)
INTO ##TABCM from COGE.DBO.TBPRK
INNER JOIN COGE.DBO.TBPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
INNER JOIN TBCOG ON COGCONTO = PRKCONTO 

LEFT OUTER JOIN TBMCC ON MCCPRKID = PRKID AND MCCPRKPROG = PRKPROG AND MCCPRKDA = PRKDA

LEFT OUTER JOIN TBLDP ON MCCCogLdp = LdpRif 
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@ID

select distinct PRKCONTO,IMPORTO INTO ##TABTO from ##TABCM 







GO
/****** Object:  StoredProcedure [dbo].[XMONDOCpt]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[XMONDOCpt] @ID int
AS
if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TACPT'))
   begin
   drop table ##TACPT
  end   
CREATE TABLE [##TACPT] (
	[COGCONTO] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[IMPORTO]  [decimal](13,2) NOT NULL ,
	[RIPARTITO] [decimal](13,2) NOT NULL,
    [RESIDUO] [decimal](13,2) NOT NULL,
	[DAREAVERE] varchar(1) COLLATE Latin1_General_CI_AS NOT NULL ,
) ON [PRIMARY]

select DISTINCT COGCONTO,
IMPORTO =  CONVERT(DECIMAL(13,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) 
when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END)
,CONVERT(DECIMAL(13,2),0) AS RIPARTITO,CONVERT(DECIMAL(13,2),0) AS RESIDUO,priprog,
DAREAVERE = case when prkda = 0 THEN 'D' else 'A' end
INTO #TMP
from COGE.DBO.TBPRK 
INNER JOIN TBCOG ON COGCONTO = PRKCONTO 
INNER JOIN COGE.DBO.TBPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@ID

INSERT INTO ##TACPT (cogconto,DAREAVERE,importo,ripartito,residuo)
SELECT DISTINCT COGCONTO,DAREAVERE,SUM(importo),SUM(ripartito),SUM(residuo) 
FROM #TMP
group by COGCONTO,DAREAVERE
DROP TABLE #TMP




GO
/****** Object:  StoredProcedure [dbo].[XRIPARTO]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[XRIPARTO] @PRIID as int
AS
select *,
LDPSIGLA = (select LdpSigla from TbLdp where MCCCogLdp = LdpRif),
VOCE = '',--isnull((Select VOCE from geve.dbo.Vrepertorio where MccCogCdc=RepLiv1 and  MccCogRep=RepLiv2 and  MccCogDet=RepLiv3),''), -- SOLO 1 LIVELLO GRUPPO PASTA
CONTODES = (Select PiaAnaCo from coge.dbo.tbpia where PiaCodCo = MCCCogConto),
IMPORTO = MCCIMPORTO,
RIPARTITO=0,
RESIDUO=0,DAREAVERE = case when MCCPrkDa = 0 THEN 'D' else 'A' end,PriDescrizione=PriDesc + PriDescB
 from tbmcc
 inner join coge.dbo.tbpri on priid = mccprkid and priprog = mccprkprog
 where priid = @PRIID


GO
/****** Object:  StoredProcedure [dbo].[XRipCpt]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[XRipCpt] @ID int
AS
if exists (
SELECT * FROM tempdb.dbo.sysobjects WHERE (name = '##TACPT'))
   begin
   drop table ##TACPT
  end   
CREATE TABLE [##TACPT] (
	[CogConto] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[IMPORTO]  [decimal](13,2) NOT NULL ,
	[RIPARTITO] [decimal](13,2) NOT NULL,
        [RESIDUO] [decimal](13,2) NOT NULL 
) ON [PRIMARY]

select DISTINCT COGCONTO,
IMPORTO =  CONVERT(DECIMAL(13,2),CASE when PriCausale < 3 then PriImpDare+(case when pricodiva = 0 then 0 else priimpavere * cii.ciiInd / 100 end) 
when prkda = 0 then priimpdare when RIvaTipo = 5  then (priimpAVERE / (100+cii.ciiali))*100 else PriImpAvere END)
,CONVERT(DECIMAL(13,2),0) AS RIPARTITO,CONVERT(DECIMAL(13,2),0) AS RESIDUO,priprog
INTO #TMP
from COGE.DBO.TBPRK 
INNER JOIN TBCOG ON COGCONTO = PRKCONTO 
INNER JOIN COGE.DBO.TBPRI ON PRKID = PRIID AND PRKPROG = PRIPROG
left outer join COGE.DBO.tbcii as cii on pricodiva = cii.ciicod
left outer join COGE.DBO.TBREGIVA on PriRegIva =RivaNreg and Datepart(year,Pridatagio) = RivaAnno 
WHERE PRKID =@ID

INSERT INTO ##TACPT (cogconto,importo,ripartito,residuo)
SELECT cogconto,SUM(importo),SUM(ripartito),SUM(residuo) 
FROM #TMP
group by COGCONTO
DROP TABLE #TMP

GO
/****** Object:  StoredProcedure [dbo].[XYNOCLAS]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[XYNOCLAS] (@DAL AS SMALLDATETIME, @AL AS SMALLDATETIME,@ID AS INT)
AS
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMPNOCL]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
   begin
  CREATE TABLE [TMPNOCL] (
        [BLOCKID][int] NOT NULL,
	[PIACODCO] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[PIAANACO]  [varchar] (32) COLLATE Latin1_General_CI_AS NOT NULL ,
	[PRKID] [int] NOT NULL,
        [PRKPROG] [smallint] NOT NULL,
 [PRKAAMMGG] [smalldatetime] NOT NULL, 
 [PRIREGIVA] [smallint] NOT NULL ,
 [PRINUMPROT] [int] NOT NULL 
 ) ON [PRIMARY]
end
---REM SELEZIONI CONTO ECONOMICI NON ACCOPPIATI MOVIMENTATI
DELETE FROM TMPNOCL WHERE BLOCKID = @ID

INSERT INTO TMPNOCL(BLOCKID,PIACODCO,PIAANACO,PRKID,PRKPROG,PRKAAMMGG ,PRIREGIVA,PRINUMPROT)
SELECT @ID,PIACODCO,PIAANACO,PRKID,PRKPROG,PRKAAMMGG ,PRIREGIVA,PRINUMPROT FROM COGE.DBO.TBPIA 
INNER JOIN COGE.DBO.TBPRK ON PRKCONTO = PIACODCO 
INNER JOIN COGE.DBO.TBPRI ON PRIID = PRKID AND PRIPROG = PRKPROG 
WHERE PIACODCO NOT IN (SELECT COGCONTO FROM DCDC.DBO.TBCOG) 
AND PIACODCO IN(SELECT PRKCONTO FROM COGE.DBO.TBPRK WHERE PRKTIPOCO = 0)
AND PRKAAMMGG BETWEEN @DAL AND @AL
GROUP BY PIACODCO,PIAANACO,PIAFL01,PRKID,PRKPROG,PRKAAMMGG,PRIREGIVA,PRINUMPROT
 HAVING PIAFL01 BETWEEN 6 AND 7
---REM QUADRATURA
SELECT MCCCOGCONTO AS CONTO,PRKDESC AS DESCRI, sum(DARE) AS DARE,0 as COGED, SUM(AVERE) AS AVERE,0 AS COGEA  INTO #TMPCD from VCDCH8 
WHERE (PRIDATAGIO BETWEEN @DAL AND @AL ) AND (PIAFL BETWEEN 6 AND 7)
GROUP BY MCCCOGCONTO,PRKDESC
order by MCCCOGCONTO

SELECT PRKCONTO AS CONTO,PRKDESC AS DESCRI, 0 AS DARE,sum(DARE) AS COGED,0 AS AVERE ,SUM(AVERE) AS COGEA INTO #TMPCO FROM COGE.DBO.VH8 
WHERE PRKTIPOCO = 0 AND (PRIDATAGIO BETWEEN @DAL AND @AL ) AND (PIAFL BETWEEN 6 AND 7)
GROUP BY PRKCONTO,PRKDESC
order by PRKCONTO

SELECT conto,DESCRI,dare,coged,avere,cogea into #tmp FROM #TMPCD
UNION 
SELECT conto,descri, dare,coged,avere,cogea FROM #TMPCO
ORDER BY CONTO
SELECT conto,descri,sum(dare) as dare,sum(coged) as coged, sum(avere) as avere,sum(cogea) as cogea into #TMPTOT FROM #TMP
group by conto,DESCRI
ORDER BY CONTO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMPQUAD]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
   begin
  CREATE TABLE [TMPQUAD] (
        [QUAKID][int] NOT NULL,
	[QUACODCO] [varchar] (5) COLLATE Latin1_General_CI_AS NOT NULL ,
	[QUAANACO]  [varchar] (32) COLLATE Latin1_General_CI_AS NOT NULL ,
	[SALDOCOG] [DECIMAL](12,2) NOT NULL,
        [SALDOCDC] [DECIMAL](12,2) NOT NULL
 
 ) ON [PRIMARY]
end

DELETE FROM TMPQUAD WHERE QUAKID = @ID

INSERT INTO TMPQUAD(QUAKID,QUACODCO,QUAANACO,SALDOCOG,SALDOCDC)
SELECT @ID,conto,Descri,(COGED-COGEA),(DARE-AVERE) FROM #TMPTOT  WHERE (DARE-AVERE)<>(COGED-COGEA)



GO
/****** Object:  StoredProcedure [dbo].[YLPCDRE]    Script Date: 21/05/2026 14:15:04 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[YLPCDRE] @DAL as smalldatetime,@AL as smalldatetime,@BLOCK as int
 as
INSERT INTO DCDC.DBO.TMPCDC(IDTMP,TLDP,TCDC,TREP,TCONTO,QSALDO)
SELECT   distinct @BLOCK ,MCCCOGLDP,MCCCOGCDC,MCCCOGREP,MCCCOGCONTO,
CASE WHEN MCCPRKDA = 0 THEN SUM(MCCIMPORTO) * 1 ELSE SUM(MCCIMPORTO) * -1 END
FROM       DCDC.DBO.TBMCC 
WHERE     MCCPRKAAMMGG BETWEEN  @DAL AND @AL                      
GROUP BY MCCCOGLDP,MCCCOGLDP,MCCCOGCDC,MCCCOGREP,MCCPRKDA,MCCCOGCONTO

UPDATE DCDC.DBO.TMPCDC set TLDPDESC = (SELECT LDPDES FROM DCDC.DBO.TBLDP WHERE TLDP = LDPCOD AND IDTMP = @BLOCK),
TCDCDESC = (SELECT CDCDES FROM DCDC.DBO.TBCDC WHERE TCDC = CDCCOD AND IDTMP = @BLOCK),
TREPDESC = (SELECT REPDES FROM DCDC.DBO.TBREP WHERE TREP = REPCOD AND IDTMP = @BLOCK),
TCONTODESC = (SELECT PIAANACO FROM COGE.DBO.TBPIA WHERE TCONTO = PIACODCO AND IDTMP = @BLOCK),
TCR = (SELECT PIAFL01 FROM COGE.DBO.TBPIA WHERE TCONTO = PIACODCO AND IDTMP = @BLOCK)
GO
USE [master]
GO
ALTER DATABASE [DCDC] SET  READ_WRITE 
GO
