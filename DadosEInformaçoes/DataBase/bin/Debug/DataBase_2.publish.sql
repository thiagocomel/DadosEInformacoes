﻿/*
Deployment script for DadosEInformacoes

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "DadosEInformacoes"
:setvar DefaultFilePrefix "DadosEInformacoes"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Starting rebuilding table [dbo].[Vendas]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Vendas] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [Nome]             VARCHAR (200)   NULL,
    [Identificacao]    BIGINT          NULL,
    [TipoPessoa]       VARCHAR (50)    NULL,
    [Sexo]             VARCHAR (50)    NULL,
    [TicketMedio]      DECIMAL (18, 2) NULL,
    [UltimaVenda]      DATETIME        NULL,
    [ValorTotal]       DECIMAL (18, 2) NULL,
    [ValorItens]       DECIMAL (18, 2) NULL,
    [QtItens]          INT             NULL,
    [DataVenda]        DATETIME        NULL,
    [IdNf]             BIGINT          NULL,
    [Veiculo]          VARCHAR (100)   NULL,
    [Ano]              VARCHAR (20)    NULL,
    [Placa]            VARCHAR (20)    NULL,
    [Km]               VARCHAR (20)    NULL,
    [FaixaAno]         VARCHAR (50)    NULL,
    [FaixaTicketMedio] VARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Vendas])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Vendas] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Vendas] ([Id], [Nome], [Identificacao], [TipoPessoa], [Sexo], [TicketMedio], [UltimaVenda], [ValorTotal], [ValorItens], [QtItens], [DataVenda], [IdNf], [Veiculo], [Ano], [Placa], [Km], [FaixaAno], [FaixaTicketMedio])
        SELECT   [Id],
                 [Nome],
                 [Identificacao],
                 [TipoPessoa],
                 [Sexo],
                 [TicketMedio],
                 [UltimaVenda],
                 [ValorTotal],
                 [ValorItens],
                 [QtItens],
                 [DataVenda],
                 [IdNf],
                 [Veiculo],
                 [Ano],
                 [Placa],
                 [Km],
                 [FaixaAno],
                 [FaixaTicketMedio]
        FROM     [dbo].[Vendas]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Vendas] OFF;
    END

DROP TABLE [dbo].[Vendas];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Vendas]', N'Vendas';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Update complete.';


GO
