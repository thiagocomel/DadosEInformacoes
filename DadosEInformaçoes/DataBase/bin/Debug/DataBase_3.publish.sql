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
PRINT N'Creating [dbo].[VendaAtrasada]...';


GO
CREATE TABLE [dbo].[VendaAtrasada] (
    [Id]                   INT             IDENTITY (1, 1) NOT NULL,
    [NomeCliente]          VARCHAR (100)   NULL,
    [IdentificacaoCliente] BIGINT          NULL,
    [UltimaRevisao]        DATETIME        NULL,
    [TicketMedio]          DECIMAL (18, 2) NULL,
    [TipoPessoa]           VARCHAR (20)    NULL,
    [Sexo]                 VARCHAR (20)    NULL,
    [Veiculo]              VARCHAR (20)    NULL,
    [Ano]                  VARCHAR (20)    NULL,
    [Placa]                VARCHAR (20)    NULL,
    [Km]                   VARCHAR (20)    NULL,
    [FaixaAno]             VARCHAR (20)    NULL,
    [FaixaTicketMedio]     VARCHAR (20)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Update complete.';


GO
