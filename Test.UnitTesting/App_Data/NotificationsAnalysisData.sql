
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/02/2016 00:50:02
-- Generated from EDMX file: C:\Users\Andres\Desktop\PIsRepoLocal\Util.NotificacionAnalysis\Model\AnalysisContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [NotificationsAnalysisData];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ScreenShotDatosSet'
CREATE TABLE [dbo].[ScreenShotDatosSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [cantidadEnviosExitosos] bigint  NOT NULL,
    [cantidadEnviosReales] bigint  NOT NULL,
    [cantidadEnviosError] bigint  NOT NULL,
    [tiempoMaximoEsperaNotificacion] float  NOT NULL,
    [tiempoMinimoEsperaNotificacion] float  NOT NULL,
    [tiempoPromedioEnvioNotificacion] float  NOT NULL,
    [nivelRecursion] int  NOT NULL,
    [duracionRafaga] float  NOT NULL,
    [cantidadTopics] int  NOT NULL,
    [cantidadCodigosNotificaciones] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ScreenShotDatosSet'
ALTER TABLE [dbo].[ScreenShotDatosSet]
ADD CONSTRAINT [PK_ScreenShotDatosSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------