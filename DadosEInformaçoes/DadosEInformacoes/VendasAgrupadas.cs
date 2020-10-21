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
        public string NomeServico { get; set; }
        public string Funcionario { get; set; }
        public int QtItensServico { get; set; }
        public string TempoServico { get; set; }
        public string TipoServico { get; set; }
        public decimal ValorTotalServico { get; set; }
        public decimal ValorUnitServico { get; set; }
        public string FormaPagto { get; set; }
        public string CondPagto { get; set; }
    }
    public class VendaAgrupada
    {
        public string Nome { get; set; }
        public long Identificacao { get; set; }
        public string TipoPessoa { get; set; }
        public string Sexo { get; set; }
        public decimal TicketMedio { get; set; }
        public DateTime UltimaVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public int DataVendaAno { get; set; }
        public int DataVendaMes { get; set; }
        public int DataVendaDia { get; set; }
        public string Veiculo { get; set; }
        public string Ano { get; set; }
        public string Placa { get; set; }
        public string Km { get; set; }
        public string FaixaAno { get; set; }
        public string FaixaTicketMedio { get; set; }
        public List<long> IdsNF { get; private set; }
        public List<decimal> ValoresTotais { get; private set; }
        public string TipoServico { get; set; }
        public string NomeServico { get; set; }
        public string Funcionario { get; set; }
        public decimal ValorTotalServico { get; set; }
        public decimal ValorTotalPecas { get; set; }
        public string FormaPagto { get; set; }
        public string CondPagto { get; set; }
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

        public void ClassificarTicketMedio()
        {
            if (TicketMedio < 350)
                this.FaixaTicketMedio = "Muito Baixo";
            else if (TicketMedio < 500)
                this.FaixaTicketMedio = "Baixo";
            else if (TicketMedio < 750)
                this.FaixaTicketMedio = "Médio";
            else if (TicketMedio < 1000)
                this.FaixaTicketMedio = "Alto";
            else if (TicketMedio > 1000)
                this.FaixaTicketMedio = "Muito Alto";
        }
    }
    public class Cliente
    {
        public long Indentificacao;
        public string Nome;
        public decimal TicketMedio;
        public string FaixaTicketMedio;
        public DateTime UltimaVenda;
        public string Sexo;
        public string TipoPessoa;
        public List<VendaAgrupada> ListaVendas = new List<VendaAgrupada>();
        public bool RevisaoAtrasada;
    }
    public class VendaAtrasada
    {
        public VendaAtrasada(VendaAgrupada item)
        {
            this.Nome = item.Nome;
            this.Identificacao = item.Identificacao;
            this.TipoPessoa = item.TipoPessoa;
            this.Sexo = item.Sexo;
            this.TicketMedio = item.TicketMedio;
            this.UltimaVenda = item.UltimaVenda;
            this.Veiculo = item.Veiculo;
            this.Ano = item.Ano;
            this.Placa = item.Placa;
            this.Km = item.Km;
            this.FaixaAno = item.FaixaAno;
            this.FaixaTicketMedio = item.FaixaTicketMedio;
        }
        public string Nome { get; set; }
        public long Identificacao { get; set; }
        public DateTime UltimaVenda { get; set; }
        public decimal TicketMedio { get; set; }
        public string TipoPessoa { get; set; }
        public string Sexo { get; set; }
        public string Veiculo { get; set; }
        public string Ano { get; set; }
        public string Placa { get; set; }
        public string Km { get; set; }
        public string FaixaAno { get; set; }
        public string FaixaTicketMedio { get; set; }
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
                          ,[NomeServico]
                          ,[Funcionario]
                          ,[QtItensServico]
                          ,[TempoServico]
                          ,[TipoServico]
                          ,[ValorTotalServico]
                          ,[ValorUnitServico]
                          ,[FormaPagto]
                          ,[CondPagto]
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
                          ,[NomeServico]
                          ,[Funcionario]
                          ,[QtItensServico]
                          ,[TempoServico]
                          ,[TipoServico]
                          ,[ValorTotalServico]
                          ,[ValorUnitServico]
                          ,[FormaPagto]
                          ,[CondPagto]
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
                                ,[DataVendaAno]
                                ,[DataVendaMes]
                                ,[DataVendaDia]
                                ,[IdNFAgrupado]
                                ,[Veiculo]
                                ,[Ano]
                                ,[Placa]
                                ,[Km]
                                ,[FaixaAno]
                                ,[FaixaTicketMedio]
                                ,[TipoServico]
                                ,[NomeServico]
                                ,[Funcionario]
                                ,[ValorTotalServico]
                                ,[ValorTotalPecas]
                                ,[FormaPagto]
                                ,[CondPagto])
                            VALUES
                                (@Nome
                                ,@Identificacao
                                ,@TipoPessoa
                                ,@Sexo
                                ,@TicketMedio
                                ,@UltimaVenda
                                ,@ValorTotalAgrupado
                                ,@DataVenda
                                ,@DataVendaAno
                                ,@DataVendaMes
                                ,@DataVendaDia
                                ,@IdNFAgrupado
                                ,@Veiculo
                                ,@Ano
                                ,@Placa
                                ,@Km
                                ,@FaixaAno
                                ,@FaixaTicketMedio
                                ,@TipoServico
                                ,@NomeServico
                                ,@Funcionario
                                ,@ValorTotalServico
                                ,@ValorTotalPecas
                                ,@FormaPagto
                                ,@CondPagto)";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var affectedRows = connection.Execute(sqlInsert, vendasAgrupadas);

                Console.WriteLine($"Affected Rows: {affectedRows}");
            }
        }

        [TestMethod]
        public void InserirVendasAtrasadas_Ok()
        {
            var sqlSelect = @"SELECT [Nome]
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
                          ,[FaixaTicketMedio]
                      FROM 
                           [dbo].[VendasAgrupadas]";

            var vendasAgrupadas = _db.Query<VendaAgrupada>(sqlSelect).ToList();

            Dictionary<long, Cliente> dicVendas = new Dictionary<long, Cliente>();
            List<VendaAtrasada> vendasAtrasadas = new List<VendaAtrasada>();
            foreach (VendaAgrupada venda in vendasAgrupadas)
            {
                if (dicVendas.ContainsKey(venda.Identificacao))
                    dicVendas[venda.Identificacao].ListaVendas.Add(venda);
                else
                {
                    Cliente cliente = new Cliente
                    {
                        FaixaTicketMedio = venda.FaixaTicketMedio,
                        Indentificacao = venda.Identificacao,
                        Nome = venda.Nome,
                        Sexo = venda.Sexo,
                        TicketMedio = venda.TicketMedio,
                        TipoPessoa = venda.TipoPessoa,
                        UltimaVenda = venda.UltimaVenda
                    };
                    dicVendas.Add(venda.Identificacao, cliente);
                    dicVendas[cliente.Indentificacao].ListaVendas.Add(venda);
                }
            }

            foreach (Cliente cliente in dicVendas.Values)
            {
                bool revisao2anos = false;
                bool revisao1anos = false;
                bool revisaoAnoAtual = false;

                foreach (VendaAgrupada venda in cliente.ListaVendas)
                {
                    if (venda.DataVenda.Year == DateTime.Now.AddYears(-2).Year)
                        revisao2anos = true;
                    else if (venda.DataVenda.Year == DateTime.Now.AddYears(-1).Year)
                        revisao1anos = true;
                    else if (venda.DataVenda.Year == DateTime.Now.Year)
                        revisaoAnoAtual = true;
                }

                if (revisao2anos && revisao1anos && !revisaoAnoAtual)
                {
                    cliente.RevisaoAtrasada = true;
                    vendasAtrasadas.Add(new VendaAtrasada(cliente.ListaVendas.LastOrDefault()));
                }
            }

            var sqlInsert = @"INSERT INTO[dbo].[VendasAtrasadas]
                                          ([Nome]
                                          ,[Identificacao]
                                          ,[UltimaVenda]
                                          ,[TicketMedio]
                                          ,[TipoPessoa]
                                          ,[Sexo]
                                          ,[Veiculo]
                                          ,[Placa]
                                          ,[Ano]
                                          ,[Km]
                                          ,[FaixaAno]
                                          ,[FaixaTicketMedio])
                                    VALUES
                                          (@Nome
                                           ,@Identificacao
                                           ,@UltimaVenda
                                           ,@TicketMedio
                                           ,@TipoPessoa
                                           ,@Sexo
                                           ,@Veiculo
                                           ,@Placa
                                           ,@Ano
                                           ,@Km
                                           ,@FaixaAno
                                           ,@FaixaTicketMedio)";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var affectedRows = connection.Execute(sqlInsert, vendasAtrasadas);

                Console.WriteLine($"Affected Rows: {affectedRows}");
            }
        }

        //[TestMethod]
        //public void InserirVendasGarantia_Ok()
        //{
        //    var sqlSelect = @"SELECT [Nome]
        //                  ,[Identificacao]
        //                  ,[TipoPessoa]
        //                  ,[Sexo]
        //                  ,[TicketMedio]
        //                  ,[UltimaVenda]
        //                  ,[ValorTotalAgrupado]
        //                  ,[DataVenda]
        //                  ,[IdNFAgrupado]
        //                  ,[Veiculo]
        //                  ,[Ano]
        //                  ,[Placa]
        //                  ,[Km]
        //                  ,[FaixaAno]
        //                  ,[FaixaTicketMedio]
        //                  ,[TipoServico]
        //                  ,[NomeServico]
        //                  ,[Funcionario]
        //                  ,[ValorTotalServico]
        //                  ,[ValorTotalPecas]
        //              FROM 
        //                   [dbo].[VendasAgrupadas]";

        //    var vendasAgrupadas = _db.Query<VendaAgrupada>(sqlSelect).ToList();

        //    Dictionary<long, Cliente> dicVendas = new Dictionary<long, Cliente>();
        //    List<VendaAtrasada> vendasAtrasadas = new List<VendaAtrasada>();
        //    foreach (VendaAgrupada venda in vendasAgrupadas)
        //    {
        //        if (dicVendas.ContainsKey(venda.Identificacao))
        //            dicVendas[venda.Identificacao].ListaVendas.Add(venda);
        //        else
        //        {
        //            Cliente cliente = new Cliente
        //            {
        //                FaixaTicketMedio = venda.FaixaTicketMedio,
        //                Indentificacao = venda.Identificacao,
        //                Nome = venda.Nome,
        //                Sexo = venda.Sexo,
        //                TicketMedio = venda.TicketMedio,
        //                TipoPessoa = venda.TipoPessoa,
        //                UltimaVenda = venda.UltimaVenda
        //            };
        //            dicVendas.Add(venda.Identificacao, cliente);
        //            dicVendas[cliente.Indentificacao].ListaVendas.Add(venda);
        //        }
        //    }

        //    foreach (Cliente cliente in dicVendas.Values)
        //    {
        //        DateTime vendaAnterior = cliente.ListaVendas[0].DataVenda;
        //        bool isFirst = true;
        //        foreach (VendaAgrupada venda in cliente.ListaVendas)
        //        {
        //            int dias = (venda.DataVenda - vendaAnterior).Days;
        //            if (dias < 7 && !isFirst)
        //                vendasAtrasadas.Add(new VendaAtrasada(venda));
        //            isFirst = false;
        //        }
        //    }

        //    var sqlInsert = @"INSERT INTO[dbo].[VendasAtrasadas]
        //                                  ([Nome]
        //                                  ,[Identificacao]
        //                                  ,[UltimaVenda]
        //                                  ,[TicketMedio]
        //                                  ,[TipoPessoa]
        //                                  ,[Sexo]
        //                                  ,[Veiculo]
        //                                  ,[Placa]
        //                                  ,[Ano]
        //                                  ,[Km]
        //                                  ,[FaixaAno]
        //                                  ,[FaixaTicketMedio])
        //                            VALUES
        //                                  (@Nome
        //                                   ,@Identificacao
        //                                   ,@UltimaVenda
        //                                   ,@TicketMedio
        //                                   ,@TipoPessoa
        //                                   ,@Sexo
        //                                   ,@Veiculo
        //                                   ,@Placa
        //                                   ,@Ano
        //                                   ,@Km
        //                                   ,@FaixaAno
        //                                   ,@FaixaTicketMedio)";

        //    using (var connection = new SqlConnection(CONNECTION_STRING))
        //    {
        //        var affectedRows = connection.Execute(sqlInsert, vendasAtrasadas);

        //        Console.WriteLine($"Affected Rows: {affectedRows}");
        //    }
        //}
        private List<VendaAgrupada> AgruparVendas(IEnumerable<VendaSeparada> vendasSeparadas)
        {
            var vendasAgrupadas = new Dictionary<IdentificacaoVenda, VendaAgrupada>();

            foreach (var item in vendasSeparadas)
            {
                var id = new IdentificacaoVenda { DataVenda = item.DataVenda, CPF_CNPJ = item.Identificacao };

                if (!vendasAgrupadas.ContainsKey(id))
                {
                    VendaAgrupada vendaAgrupada = new VendaAgrupada();

                    vendaAgrupada.Nome = item.Nome;
                    vendaAgrupada.Identificacao = item.Identificacao;
                    vendaAgrupada.TipoPessoa = item.TipoPessoa;
                    vendaAgrupada.Sexo = item.Sexo;
                    vendaAgrupada.TicketMedio = item.TicketMedio;
                    vendaAgrupada.UltimaVenda = item.UltimaVenda;
                    vendaAgrupada.DataVenda = item.DataVenda;
                    vendaAgrupada.DataVendaAno = item.DataVenda.Year;
                    vendaAgrupada.DataVendaMes = item.DataVenda.Month;
                    vendaAgrupada.DataVendaDia = item.DataVenda.Day;
                    vendaAgrupada.Veiculo = item.Veiculo;
                    vendaAgrupada.Ano = item.Ano;
                    vendaAgrupada.Placa = item.Placa;
                    vendaAgrupada.Km = item.Km;
                    vendaAgrupada.FaixaAno = item.FaixaAno;
                    vendaAgrupada.FaixaTicketMedio = item.FaixaTicketMedio;
                    if (String.IsNullOrEmpty(item.TipoServico) == false)
                        vendaAgrupada.TipoServico = item.TipoServico;
                    if (String.IsNullOrEmpty(item.NomeServico) == false)
                        vendaAgrupada.NomeServico = item.NomeServico;
                    if (item.ValorTotalServico > 0)
                        vendaAgrupada.ValorTotalServico = item.ValorTotalServico;
                    if (item.ValorItens  > 0)
                        vendaAgrupada.ValorTotalPecas = item.ValorItens;
                    if (String.IsNullOrEmpty(item.Funcionario) == false)
                        vendaAgrupada.Funcionario = item.Funcionario;
                    if (String.IsNullOrEmpty(item.FormaPagto) == false)
                        vendaAgrupada.FormaPagto = item.FormaPagto;
                    if (String.IsNullOrEmpty(item.CondPagto) == false)
                        vendaAgrupada.CondPagto = item.CondPagto;
                    
                    vendaAgrupada.AdicionarValorTotal(item.ValorTotal);
                    vendaAgrupada.AdicionarIdNF(item.IdNF);
                    vendasAgrupadas.Add(id, vendaAgrupada);
                }
                else
                {
                    var vendaAgrupada = vendasAgrupadas[id];

                    vendaAgrupada.AdicionarValorTotal(item.ValorTotal);

                    vendaAgrupada.AdicionarIdNF(item.IdNF);

                    if (String.IsNullOrEmpty(item.TipoServico) == false)
                        vendaAgrupada.TipoServico = item.TipoServico;
                    if (String.IsNullOrEmpty(item.NomeServico) == false)
                        vendaAgrupada.NomeServico = item.NomeServico;
                    if (item.ValorTotalServico > 0)
                        vendaAgrupada.ValorTotalServico = item.ValorTotalServico;
                    if (item.ValorItens > 0)
                        vendaAgrupada.ValorTotalPecas = item.ValorItens;
                    if (String.IsNullOrEmpty(item.Funcionario) == false)
                        vendaAgrupada.Funcionario = item.Funcionario;
                    if (String.IsNullOrEmpty(item.FormaPagto) == false)
                        vendaAgrupada.FormaPagto = item.FormaPagto;
                    if (String.IsNullOrEmpty(item.CondPagto) == false)
                        vendaAgrupada.CondPagto = item.CondPagto;
                }
            }

            foreach (IdentificacaoVenda identificacao in vendasAgrupadas.Keys)
            {
                List<KeyValuePair<IdentificacaoVenda, VendaAgrupada>> vendas = vendasAgrupadas.Where(x => x.Key.CPF_CNPJ == identificacao.CPF_CNPJ).ToList();

                decimal valorTotal = 0;
                DateTime ultimaVenda = DateTime.MinValue;
                foreach (KeyValuePair<IdentificacaoVenda, VendaAgrupada> venda in vendas)
                {
                    valorTotal = valorTotal + venda.Value.ValorTotalAgrupado;
                    if (ultimaVenda < venda.Key.DataVenda)
                        ultimaVenda = venda.Key.DataVenda;
                }

                vendasAgrupadas[identificacao].TicketMedio = valorTotal / vendas.Count;
                vendasAgrupadas[identificacao].ClassificarTicketMedio();
                vendasAgrupadas[identificacao].UltimaVenda = ultimaVenda;
            }
            return vendasAgrupadas.Values.OrderBy(x => x.DataVenda).ToList();
        }
    }
}