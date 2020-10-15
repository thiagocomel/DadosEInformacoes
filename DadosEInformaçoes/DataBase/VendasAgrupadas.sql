CREATE TABLE [dbo].[VendasAgrupadas]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nome] VARCHAR(200) NULL, 
    [Identificacao] BIGINT NULL, 
    [TipoPessoa] VARCHAR(50) NULL, 
    [Sexo] VARCHAR(50) NULL, 
    [TicketMedio] DECIMAL(18, 2) NULL, 
    [UltimaVenda] DATETIME NULL, 
    [ValorTotalAgrupado] DECIMAL(18, 2) NULL, 
    [DataVenda] DATETIME NULL, 
    [IdNFAgrupado] VARCHAR(50) NULL, 
    [Veiculo] VARCHAR(50) NULL, 
    [Ano] VARCHAR(50) NULL, 
    [Placa] VARCHAR(50) NULL, 
    [Km] VARCHAR(50) NULL, 
    [FaixaAno] VARCHAR(50) NULL, 
    [FaixaTicketMedio] VARCHAR(50) NULL
)
