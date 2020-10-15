CREATE TABLE [dbo].[Servicos]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [IdNf] BIGINT NULL, 
    [TipoServico] VARCHAR(200) NULL, 
    [NomeServico] VARCHAR(200) NULL, 
    [Funcionario] VARCHAR(100) NULL, 
    [Tempo] VARCHAR(50) NULL, 
    [VlrUnit] DECIMAL(18, 2) NULL, 
    [QntItens] INT NULL, 
    [VlrTotal] DECIMAL(18, 2) NULL
)
