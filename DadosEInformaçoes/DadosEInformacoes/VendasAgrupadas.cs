using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;

namespace DadosEInformacoes
{
    public class VendaSeparada
    {
        public string Nome { get; set; }
        public long Identificacao { get; set; }
        public string TipoPessoa { get; set; }
        public string Sexo { get; set; }
        public decimal TicketMedio { get; set; }
        public DateTime UltimaVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorItens { get; set; }
        public int QtItens { get; set; }
        public DateTime DataVenda { get; set; }
        public long IdNF { get; set; }
        public string Veiculo { get; set; }
        public string Ano { get; set; }
        public string Placa { get; set; }
        public string Km { get; set; }
        public string FaixaAno { get; set; }
        public string FaixaTicketMedio { get; set; }

    }

    public class VendaAgrupada
    {
        public VendaAgrupada(VendaSeparada item)
        {
            this.Nome = item.Nome;
            this.Identificacao = item.Identificacao;
            this.TipoPessoa = item.TipoPessoa;
            this.Sexo = item.Sexo;
            this.TicketMedio = item.TicketMedio;
            this.UltimaVenda = item.UltimaVenda;
            this.DataVenda = item.DataVenda;
            this.Veiculo = item.Veiculo;
            this.Ano = item.Ano;
            this.Placa = item.Placa;
            this.Km = item.Km;
            this.FaixaAno = item.FaixaAno;
            this.FaixaTicketMedio = item.FaixaTicketMedio;

            this.AdicionarValorTotal(item.ValorTotal);
            this.AdicionarIdNF(item.IdNF);
        }

        public string Nome { get; set; }
        public long Identificacao { get; set; }
        public string TipoPessoa { get; set; }
        public string Sexo { get; set; }
        public decimal TicketMedio { get; set; }
        public DateTime UltimaVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public string Veiculo { get; set; }
        public string Ano { get; set; }
        public string Placa { get; set; }
        public string Km { get; set; }
        public string FaixaAno { get; set; }
        public string FaixaTicketMedio { get; set; }

        public List<long> IdsNF { get; private set; }
        public List<decimal> ValoresTotais { get; private set; }

        public string IdNFAgrupado
        {
            get
            {
                return string.Join("; ", IdsNF);
            }
        }

        public decimal ValorTotalAgrupado
        {
            get
            {
                return ValoresTotais.Sum();
            }
        }

        public void AdicionarValorTotal(decimal valor)
        {
            if (this.ValoresTotais == null)
                this.ValoresTotais = new List<decimal>();

            this.ValoresTotais.Add(valor);
        }

        public void AdicionarIdNF(long idNF)
        {
            if (this.IdsNF == null)
                this.IdsNF = new List<long>();

            this.IdsNF.Add(idNF);

            this.IdsNF.Sort();
        }

        public void CalcularTicketMedio()
        {
            this.TicketMedio = ValorTotalAgrupado / ValoresTotais.Count;

            if (TicketMedio < 200)
                this.FaixaTicketMedio = "Muito Baixo";
            else if (TicketMedio < 350)
                this.FaixaTicketMedio = "Baixo";
            else if (TicketMedio < 600)
                this.FaixaTicketMedio = "Médio";
            else if (TicketMedio < 750)
                this.FaixaTicketMedio = "Alto";
            else
                this.FaixaTicketMedio = "Muito Alto";
        }
    }

    public class IdentificacaoVenda
    {
        public DateTime DataVenda { get; set; }

        public long CPF_CNPJ { get; set; }

        public List<long> IdNF { get; set; }

        public override bool Equals(object obj)
        {
            IdentificacaoVenda identificacao = obj as IdentificacaoVenda;

            return this.DataVenda.Date == identificacao.DataVenda.Date &&
                this.CPF_CNPJ == identificacao.CPF_CNPJ;
        }

        public override int GetHashCode()
        {
            int hashCode = 1278947931;
            hashCode = hashCode * -1521134295 + DataVenda.GetHashCode();
            hashCode = hashCode * -1521134295 + CPF_CNPJ.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<long>>.Default.GetHashCode(IdNF);
            return hashCode;
        }
    }

    [TestClass]
    public class VendasAgrupadas
    {
        private const string CONNECTION_STRING = "Server=Intel;Database=DadosEInformacoes;Integrated Security=True;";

        private IDbConnection _db;

        public VendasAgrupadas()
        {
            _db = new SqlConnection(CONNECTION_STRING);
        }

        [TestMethod]
        public void VendasSeparadas_Ok()
        {
            var sql = @"SELECT [Nome]
                          ,[Identificacao]
                          ,[TipoPessoa]
                          ,[Sexo]
                          ,[TicketMedio]
                          ,[UltimaVenda]
                          ,[ValorTotal]
                          ,[ValorItens]
                          ,[QtItens]
                          ,[DataVenda]
                          ,[IdNF]
                          ,[Veiculo]
                          ,[Ano]
                          ,[Placa]
                          ,[Km]
                          ,[FaixaAno]
                          ,[FaixaTicketMedio]
                      FROM [dbo].[Vendas]";

            var vendasSeparadas = _db.Query<VendaSeparada>(sql);

            Assert.AreEqual(15989, vendasSeparadas.Count());
        }

        [TestMethod]
        public void VendasSeparadasPorCliente_Ok()
        {
            var sql = @"SELECT [Nome]
                          ,[Identificacao]
                          ,[TipoPessoa]
                          ,[Sexo]
                          ,[TicketMedio]
                          ,[UltimaVenda]
                          ,[ValorTotal]
                          ,[ValorItens]
                          ,[QtItens]
                          ,[DataVenda]
                          ,[IdNF]
                          ,[Veiculo]
                          ,[Ano]
                          ,[Placa]
                          ,[Km]
                          ,[FaixaAno]
                          ,[FaixaTicketMedio]
                      FROM 
                          [dbo].[Vendas]
 
                      WHERE [Nome]='ALEXANDRE RECH'";

            var vendasSeparadas = _db.Query<VendaSeparada>(sql);

            Assert.AreEqual(10, vendasSeparadas.Count());
        }

        [TestMethod]
        public void VendasAgruapdasDiasDiferentesParaUnicoCliente_Ok()
        {
            var sql = @"SELECT [Nome]
                          ,[Identificacao]
                          ,[TipoPessoa]
                          ,[Sexo]
                          ,[TicketMedio]
                          ,[UltimaVenda]
                          ,[ValorTotal]
                          ,[ValorItens]
                          ,[QtItens]
                          ,[DataVenda]
                          ,[IdNF]
                          ,[Veiculo]
                          ,[Ano]
                          ,[Placa]
                          ,[Km]
                          ,[FaixaAno]
                          ,[FaixaTicketMedio]
                      FROM 
                           [dbo].[Vendas]
 
                      WHERE [Nome]='ALEXANDRE RECH'";

            var vendasSeparadas = _db.Query<VendaSeparada>(sql);

            var vendasAgrupadas = AgruparVendas(vendasSeparadas);

            Assert.AreEqual(5, vendasAgrupadas.Count);
        }

        [TestMethod]
        public void VendasAgruapdasDiasDiferentesParaVariosClientes_Ok()
        {
            var sql = @"SELECT [Nome]
                          ,[Identificacao]
                          ,[TipoPessoa]
                          ,[Sexo]
                          ,[TicketMedio]
                          ,[UltimaVenda]
                          ,[ValorTotal]
                          ,[ValorItens]
                          ,[QtItens]
                          ,[DataVenda]
                          ,[IdNF]
                          ,[Veiculo]
                          ,[Ano]
                          ,[Placa]
                          ,[Km]
                          ,[FaixaAno]
                          ,[FaixaTicketMedio]
                      FROM 
                           [dbo].[Vendas]
 
                      WHERE [IdNF] > 22 and [IdNF] < 110  order by [IdNF]";

            var vendasSeparadas = _db.Query<VendaSeparada>(sql);

            var vendasAgrupadas = AgruparVendas(vendasSeparadas);

            Assert.AreEqual(11, vendasAgrupadas.Count);
            Assert.AreEqual(177.47m, vendasAgrupadas[0].ValorTotalAgrupado);
            Assert.AreEqual(100.0m, vendasAgrupadas[3].ValorTotalAgrupado);

        }

        [TestMethod]
        public void InserirVendasAgruapdas_Ok()
        {
            var sqlSelect = @"SELECT [Nome]
                          ,[Identificacao]
                          ,[TipoPessoa]
                          ,[Sexo]
                          ,[TicketMedio]
                          ,[UltimaVenda]
                          ,[ValorTotal]
                          ,[ValorItens]
                          ,[QtItens]
                          ,[DataVenda]
                          ,[IdNF]
                          ,[Veiculo]
                          ,[Ano]
                          ,[Placa]
                          ,[Km]
                          ,[FaixaAno]
                          ,[FaixaTicketMedio]
                      FROM 
                           [dbo].[Vendas]";

            var vendasSeparadas = _db.Query<VendaSeparada>(sqlSelect);

            var vendasAgrupadas = AgruparVendas(vendasSeparadas);

            var sqlInsert = @"INSERT INTO [dbo].[VendasAgrupadas]
                                ([Nome]
                                ,[Identificacao]
                                ,[TipoPessoa]
                                ,[Sexo]
                                ,[TicketMedio]
                                ,[UltimaVenda]
                                ,[ValorTotalAgrupado]
                                ,[DataVenda]
                                ,[IdNFAgrupado]
                                ,[Veiculo]
                                ,[Ano]
                                ,[Placa]
                                ,[Km]
                                ,[FaixaAno]
                                ,[FaixaTicketMedio])
                            VALUES
                                (@Nome
                                ,@Identificacao
                                ,@TipoPessoa
                                ,@Sexo
                                ,@TicketMedio
                                ,@UltimaVenda
                                ,@ValorTotalAgrupado
                                ,@DataVenda
                                ,@IdNFAgrupado
                                ,@Veiculo
                                ,@Ano
                                ,@Placa
                                ,@Km
                                ,@FaixaAno
                                ,@FaixaTicketMedio)";

            //_db.Query(sqlInsert, vendasAgrupadas);

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var affectedRows = connection.Execute(sqlInsert, vendasAgrupadas);

                Console.WriteLine($"Affected Rows: {affectedRows}");
            }
        }

        private List<VendaAgrupada> AgruparVendas(IEnumerable<VendaSeparada> vendasSeparadas)
        {
            var vendasAgrupadas = new Dictionary<IdentificacaoVenda, VendaAgrupada>();

            foreach (var item in vendasSeparadas)
            {
                var id = new IdentificacaoVenda { DataVenda = item.DataVenda, CPF_CNPJ = item.Identificacao };

                if (!vendasAgrupadas.ContainsKey(id))
                {
                    vendasAgrupadas.Add(id, new VendaAgrupada(item));
                }
                else
                {
                    var vendaAgrupada = vendasAgrupadas[id];

                    vendaAgrupada.AdicionarValorTotal(item.ValorTotal);

                    vendaAgrupada.AdicionarIdNF(item.IdNF);

                    vendaAgrupada.CalcularTicketMedio();
                }
            }

            return vendasAgrupadas.Values.OrderBy(x => x.DataVenda).ToList();
        }
    }
}