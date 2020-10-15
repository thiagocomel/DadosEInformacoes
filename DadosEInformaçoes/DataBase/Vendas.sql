CREATE TABLE [dbo].[Vendas]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nome] VARCHAR(200) NULL, 
    [Identificacao] BIGINT NULL, 
    [TipoPessoa] VARCHAR(50) NULL, 
    [Sexo] VARCHAR(50) NULL, 
    [TicketMedio] DECIMAL(18, 2) NULL, 
    [UltimaVenda] DATETIME NULL, 
    [ValorTotal] DECIMAL(18, 2) NULL, 
    [ValorItens] DECIMAL(18, 2) NULL, 
    [QtItens] INT NULL, 
    [DataVenda] DATETIME NULL, 
    [IdNf] BIGINT NULL, 
    [Veiculo] VARCHAR(100) NULL, 
    [Ano] VARCHAR(20) NULL, 
    [Placa] VARCHAR(20) NULL, 
    [Km] VARCHAR(20) NULL, 
    [FaixaAno] VARCHAR(50) NULL, 
    [FaixaTicketMedio] VARCHAR(50) NULL 
)
