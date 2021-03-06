﻿CREATE TABLE [dbo].[VendasAgrupadas]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nome] VARCHAR(MAX) NULL, 
    [Identificacao] BIGINT NULL, 
    [TipoPessoa] VARCHAR(MAX) NULL, 
    [Sexo] VARCHAR(MAX) NULL, 
    [TicketMedio] DECIMAL(18, 2) NULL, 
    [UltimaVenda] DATETIME NULL, 
    [ValorTotalAgrupado] DECIMAL(18, 2) NULL, 
    [DataVenda] DATETIME NULL, 
    [DataVendaAno] VARCHAR(4) NULL, 
    [DataVendaMes] VARCHAR(2) NULL, 
    [DataVendaDia] VARCHAR(2) NULL,
    [IdNFAgrupado] VARCHAR(MAX) NULL, 
    [Veiculo] VARCHAR(MAX) NULL, 
    [Ano] VARCHAR(MAX) NULL, 
    [Placa] VARCHAR(MAX) NULL, 
    [Km] VARCHAR(MAX) NULL, 
    [FaixaAno] VARCHAR(MAX) NULL, 
    [FaixaTicketMedio] VARCHAR(MAX) NULL, 
    [TipoServico] VARCHAR(200) NULL, 
    [NomeServico] VARCHAR(200) NULL, 
    [Funcionario] VARCHAR(100) NULL, 
    [ValorTotalServico] DECIMAL(18, 2) NULL, 
    [ValorTotalPecas] DECIMAL(18, 2) NULL, 
    [FormaPagto] VARCHAR(100) NULL, 
    [CondPagto] VARCHAR(100) NULL, 
    [Telefone] VARCHAR(200) NULL, 

)
