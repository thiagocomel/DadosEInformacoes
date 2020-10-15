CREATE TABLE [dbo].[VendasAtrasadas]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NomeCliente] VARCHAR(200) NULL, 
    [IdentificacaoCliente] BIGINT NULL, 
    [UltimaRevisao] DATETIME NULL, 
    [TicketMedio] DECIMAL(18, 2) NULL, 
    [TipoPessoa] VARCHAR(50) NULL, 
    [Sexo] VARCHAR(50) NULL, 
    [Veiculo] VARCHAR(50) NULL, 
    [Ano] VARCHAR(50) NULL, 
    [Placa] VARCHAR(50) NULL, 
    [Km] VARCHAR(50) NULL, 
    [FaixaAno] VARCHAR(50) NULL, 
    [FaixaTicketMedio] VARCHAR(50) NULL
)
