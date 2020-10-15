using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DadosEInformacoes
{
    [TestClass]
    public class VendasSeparadas
    {
        [TestMethod]
        public void InserirVendasSeparadas()
        {
            CarregarNomes();
            CriarConexaoSQL();

            FbConnectionStringBuilder fbcs = new FbConnectionStringBuilder();
            fbcs.ConnectionString = strConn;
            fbcs.Charset = "WIN1252";

            FbConnection fbConn = new FbConnection(fbcs.ToString());

            DataTable tableNotas = PegarDadosNotas(fbcs, fbConn);
            DataTable tableServico = PegarDadosServicos(fbcs, fbConn);

            GerarRelatorio(tableNotas, tableServico);
        }

        [TestMethod]
        public void InserirVendasAtrasadas()
        {
            CarregarNomes();
            CriarConexaoSQL();

            FbConnectionStringBuilder fbcs = new FbConnectionStringBuilder();
            fbcs.ConnectionString = strConn;
            fbcs.Charset = "WIN1252";

            FbConnection fbConn = new FbConnection(fbcs.ToString());

            DataTable tableNotas = PegarDadosNotas(fbcs, fbConn);

            SalvarRevisoesAtrasadas(tableNotas);
        }

        private string strConn = @"DataSource=localhost; Database=D:\Thiago\DadosEInformacoes\Irmãos Neto\Dados.FDB; User=sysdba; password=masterkey";
        private string strconSQL = "Server=Intel;Database=DadosEInformacoes;Integrated Security=True;";
        private SqlConnection conexao;
        private SqlCommand command;
        private SqlCommand commandServico;
        private SqlCommand commandRevisaoAtrasada;

        Dictionary<string, string> dicNames = new Dictionary<string, string>();
        private void CriarConexaoSQL()
        {
            conexao = new SqlConnection(strconSQL);
            #region Vendas
            string strcommand = "INSERT INTO Vendas (nome, Identificacao, tipopessoa, sexo, ticketmedio, ultimavenda, valortotal, valoritens, qtItens, datavenda, idnf,Veiculo,Placa,Ano,Km, FaixaAno, FaixaTicketMedio) VALUES" +
           "(@nome, @Identificacao, @tipopessoa, @sexo, @ticketmedio, @ultimavenda, @valortotal, @valoritens, @qtItens, @datavenda, @idnf,@Veiculo, @Placa, @Ano, @Km,@FaixaAno, @FaixaTicketMedio)";
            command = new SqlCommand(strcommand, conexao);
            command.Parameters.Add("@nome", SqlDbType.VarChar);
            command.Parameters.Add("@Identificacao", SqlDbType.BigInt);
            command.Parameters.Add("@tipopessoa", SqlDbType.VarChar);
            command.Parameters.Add("@sexo", SqlDbType.VarChar);
            command.Parameters.Add("@ticketmedio", SqlDbType.Decimal);
            command.Parameters.Add("@ultimavenda", SqlDbType.DateTime);
            command.Parameters.Add("@valortotal", SqlDbType.Decimal);
            command.Parameters.Add("@valoritens", SqlDbType.Decimal);
            command.Parameters.Add("@qtItens", SqlDbType.Int);
            command.Parameters.Add("@datavenda", SqlDbType.DateTime);
            command.Parameters.Add("@idnf", SqlDbType.BigInt);

            command.Parameters.Add("@Veiculo", SqlDbType.VarChar);
            command.Parameters.Add("@Ano", SqlDbType.VarChar);
            command.Parameters.Add("@Placa", SqlDbType.VarChar);
            command.Parameters.Add("@Km", SqlDbType.VarChar);
            command.Parameters.Add("@FaixaAno", SqlDbType.VarChar);
            command.Parameters.Add("@FaixaTicketMedio", SqlDbType.VarChar);
            #endregion

            #region Serivços
            string strcmdServicos = @"INSERT INTO [dbo].[Servicos]
                                       ([idnf],[TipoServico]
                                       ,[NomeServico]
                                       ,[Funcionario]
                                       ,[Tempo]
                                       ,[VlrUnit]
                                       ,[QntItens]
                                       ,[VlrTotal])
                                    VALUES
                                       (@idnf
                                       ,@TipoServico
                                       ,@NomeServico
                                       ,@Funcionario
                                       ,@Tempo
                                       ,@VlrUnit
                                       ,@QntItens
                                       ,@VlrTotal)";
            commandServico = new SqlCommand(strcmdServicos, conexao);
            commandServico.Parameters.Add("@idnf", SqlDbType.BigInt);
            commandServico.Parameters.Add("@TipoServico", SqlDbType.VarChar);
            commandServico.Parameters.Add("@NomeServico", SqlDbType.VarChar);
            commandServico.Parameters.Add("@Funcionario", SqlDbType.VarChar);
            commandServico.Parameters.Add("@Tempo", SqlDbType.VarChar);
            commandServico.Parameters.Add("@VlrUnit", SqlDbType.Decimal);
            commandServico.Parameters.Add("@QntItens", SqlDbType.Int);
            commandServico.Parameters.Add("@VlrTotal", SqlDbType.Decimal);
            #endregion

            #region REvisão Atrasada
            string strcmdRevisaoAtrasada = @"INSERT INTO[dbo].[VendasAtrasadas]
                                          ([NomeCliente]
                                          ,[IdentificacaoCliente]
                                          ,[UltimaRevisao]
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
                                          (@NomeCliente
                                           ,@IdentificacaoCliente
                                           ,@UltimaRevisao
                                           ,@TicketMedio
                                           ,@TipoPessoa
                                           ,@Sexo
                                           ,@Veiculo
                                           ,@Placa
                                           ,@Ano
                                           ,@Km
                                           ,@FaixaAno
                                           ,@FaixaTicketMedio)";
            commandRevisaoAtrasada = new SqlCommand(strcmdRevisaoAtrasada, conexao);
            commandRevisaoAtrasada.Parameters.Add("@NomeCliente", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@IdentificacaoCliente", SqlDbType.BigInt);
            commandRevisaoAtrasada.Parameters.Add("@UltimaRevisao", SqlDbType.DateTime);
            commandRevisaoAtrasada.Parameters.Add("@TicketMedio", SqlDbType.Decimal);
            commandRevisaoAtrasada.Parameters.Add("@TipoPessoa", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@Sexo", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@Veiculo", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@Ano", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@Placa", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@Km", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@FaixaAno", SqlDbType.VarChar);
            commandRevisaoAtrasada.Parameters.Add("@FaixaTicketMedio", SqlDbType.VarChar);

            #endregion
        }
        private void CarregarNomes()
        {
            StreamReader sr = new StreamReader(@".\Arquivos\nomes.csv");

            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                List<string> linePositions = line.Split(',').ToList();

                List<string> names = linePositions[0].Split('|').ToList();
                string sexo = linePositions[1].ToString() == "F" ? "Feminino" : "Masculino";
                foreach (var name in names)
                {
                    if (!dicNames.ContainsKey(name))
                        dicNames.Add(name, sexo);
                }
            }
        }
        private DataTable PegarDadosServicos(FbConnectionStringBuilder fbcs, FbConnection fbConn)
        {
            FbCommand fbCmd = new FbCommand(@"select
                                                    nf.idnf,
                                                    tiposerv.nome as tiposervico,
                                                    serv.nome as servico,
                                                    conta.nome as funcionario,
                                                    serv_nf.tempo,
                                                    serv_nf.valor,
                                                    serv_nf.qtde,
                                                    serv_nf.total
                                                from
                                                    empresa_nf as nf  inner join
                                                    empresa_nf_servico serv_nf on serv_nf.idnf = nf.idnf inner join
                                                    empresa_conta conta on conta.idconta = serv_nf.idfuncionario inner join
                                                    vendas_servico serv on serv.idservico = serv_nf.idservico inner join
                                                    vendas_tiposervico tiposerv on tiposerv.idtiposervico = serv.idtiposervico
                                                where nf.emissao > '01.01.2020'", fbConn);

            DataTable dtService = new DataTable();
            try
            {
                FbDataAdapter fbDa = new FbDataAdapter(fbCmd);
                fbConn.Open();
                fbDa.Fill(dtService);

            }
            catch (FbException fbex)
            {
                Console.WriteLine("Erro ao acessar o FireBird " + fbex.Message, "Erro");
            }
            finally
            {
                fbConn.Close();
            }
            return dtService;
        }
        public DataTable PegarDadosNotas(FbConnectionStringBuilder fbcs, FbConnection fbConn)
        {
            FbCommand fbCmd = new FbCommand(@"select
                                                    nf.idnf,
                                                    conta.nome,
                                                    conta.cpf,
                                                    conta.cnpj,
                                                    conta.pessoa,
                                                    nf.vlrtotal, 
                                                    nf.emissao,
                                                    sum (prod.vlrunitario * prod.qtde) as vlrItens,
                                                    sum (prod.cmedio * prod.qtde) as vlrCustoItens,
                                                    count(prod.idnf_prod * prod.qtde) as qntprod,
                                                    nf.dadosadicionais
                                                from
                                                          empresa_nf as nf left join
                                                          empresa_nf_prod as prod on prod.idnf = nf.idnf inner join
                                                          empresa_conta as conta on conta.idconta = nf.idconta
                                                 where nf.cancelada = 'N' and  nf.terceiro = 'N' and nf.idnatoperacao not in (11, 12, 58, 53, 54, 36,37)
                                                group by
                                                    nf.idnf,
                                                    conta.nome,
                                                    conta.cpf,
                                                    conta.cnpj,
                                                    conta.pessoa,
                                                    nf.vlrtotal,
                                                    nf.emissao,
                                                    nf.dadosadicionais", fbConn);

            DataTable dtSales = new DataTable();
            try
            {
                FbDataAdapter fbDa = new FbDataAdapter(fbCmd);
                fbConn.Open();
                fbDa.Fill(dtSales);

            }
            catch (FbException fbex)
            {
                Console.Write("Erro ao acessar o FireBird " + fbex.Message, "Erro");
            }
            finally
            {
                fbConn.Close();
            }
            return dtSales;
        }
        private void GerarRelatorio(DataTable tableNotas, DataTable tableServico)
        {
            //SalvarDadosServicos(tableServico);
            SalvarDadosVendas(tableNotas);


        }
        private void SalvarDadosVendas(DataTable tableNotas)
        {
            Dictionary<long, Cliente> dicVendas = new Dictionary<long, Cliente>();
            MontarDicionarioVendas(tableNotas, dicVendas);

            CalcularTicketMedio(dicVendas);
            PegarInfosVeiculos(dicVendas);
            //RevisoesAtrasadas(dicVendas);

            #region Montar Relatório Clientes e vendas
            SalvarDadosVendas(dicVendas);
            #endregion
        }
        private void SalvarRevisoesAtrasadas(DataTable tableNotas)
        {
            Dictionary<long, Cliente> dicVendas = new Dictionary<long, Cliente>();
            MontarDicionarioVendas(tableNotas, dicVendas);

            CalcularTicketMedio(dicVendas);
            PegarInfosVeiculos(dicVendas);
            RevisoesAtrasadas(dicVendas);
        }
        private void PegarInfosVeiculos(Dictionary<long, Cliente> dicVendas)
        {
            foreach (long identificacao in dicVendas.Keys)
            {
                Cliente vendas = dicVendas[identificacao];
                foreach (Venda venda in vendas.ListaVendas)
                {
                    if (venda.DadosAdicionais.Length > 20 && !venda.DadosAdicionais.Contains("DIV-    ") &&
                        !venda.DadosAdicionais.Contains("BANCO DO BRASIL AGENCIA "))
                    {
                        #region Capturar placa
                        Regex regex = new Regex(@"[a-zA-Z]{3}\-\d{4}");
                        regex.Match(venda.DadosAdicionais);
                        int index = 0;

                        Match match = regex.Match(venda.DadosAdicionais);
                        if (match.Success)
                        {
                            index = match.Index;
                        }

                        venda.Placa = venda.DadosAdicionais.Substring(index, 8);
                        if (venda.Placa.Contains("Manutenç"))
                        {
                            int indexPlaca = venda.DadosAdicionais.IndexOf("veículo de placa ") + 17;
                            venda.Placa = venda.DadosAdicionais.Substring(indexPlaca, 7).Insert(3, "-");
                        }
                        else if (venda.Placa.Contains("DIV-    "))
                        {
                            int indexPlaca = venda.DadosAdicionais.IndexOf("veículo de placa ") + 17;
                            venda.Placa = venda.DadosAdicionais.Substring(indexPlaca, 7).Insert(3, "-");
                        }
                        #endregion
                        //if (venda.DadosAdicionais.Contains("SOLICITAÇÃO E APROVAÇÃO DE SERVIÇOS: "))
                        //{

                        //}

                        #region Capturar Veiculo
                        try
                        {
                            int indexStartCarro = venda.DadosAdicionais.IndexOf(" / ");
                            int indexEndCarro = venda.DadosAdicionais.IndexOf(" - ", indexStartCarro);
                            if (indexEndCarro == -1)
                                indexEndCarro = venda.DadosAdicionais.IndexOf(" ", indexStartCarro + 3);
                            int lenghtCarro = indexEndCarro - indexStartCarro;

                            string veiculo = venda.DadosAdicionais.Substring(indexStartCarro + 3, lenghtCarro - 3);

                            if (veiculo.Contains("\r"))
                                veiculo = veiculo.Substring(0, veiculo.IndexOf("\r"));

                            Match matchVeiculo = regex.Match(veiculo);
                            if (matchVeiculo.Success)
                            {
                                index = matchVeiculo.Index;
                                veiculo = veiculo.Substring(0, index);
                            }

                            venda.Veiculo = veiculo.Trim();
                        }
                        catch (Exception ex)
                        {
                            venda.Veiculo = "Não Identificado";
                        }
                        #endregion

                        #region CapturarAno
                        int indexStartAno = venda.DadosAdicionais.IndexOf("ANO: ") + 5;
                        venda.Ano = venda.DadosAdicionais.Substring(indexStartAno, 4);
                        venda.FaixaAno = ClassificarAno(venda.Ano);
                        #endregion

                        #region Capturar KM
                        if (venda.DadosAdicionais.Contains("KM:"))
                        {
                            int indexStartKM = venda.DadosAdicionais.IndexOf("KM: ") + 4;
                            string Km;
                            if (venda.DadosAdicionais.Length < indexStartKM + 6)
                            {
                                int lenght = venda.DadosAdicionais.Length - indexStartKM;
                                Km = venda.DadosAdicionais.Substring(indexStartKM, lenght);
                            }
                            else
                                Km = venda.DadosAdicionais.Substring(indexStartKM, 6);


                            venda.KM = new string(Km.Where(c => char.IsDigit(c)).ToArray());
                        }
                        else if (venda.DadosAdicionais.Contains("Manutenção efetuada no veículo de placa"))
                        {
                            int indexStartKM = venda.DadosAdicionais.IndexOf("KM ") + 3;
                            int indexEndKM = venda.DadosAdicionais.IndexOf(",", indexStartKM);
                            int lenghtCarro = indexEndKM - indexStartKM;
                            string Km = venda.DadosAdicionais.Substring(indexStartKM, lenghtCarro);

                            venda.KM = new string(Km.Where(c => char.IsDigit(c)).ToArray());
                            venda.Ano = "Não Identificado";
                        }
                        #endregion
                    }
                }
            }
        }
        private void SalvarDadosVendas(Dictionary<long, Cliente> dicVendas)
        {
            try
            {
                conexao.Open();
                foreach (long identificacao in dicVendas.Keys)
                {
                    Cliente cliente = dicVendas[identificacao];
                    foreach (Venda venda in cliente.ListaVendas)
                    {
                        command.Parameters["@nome"].Value = cliente.Nome;
                        command.Parameters["@Identificacao"].Value = cliente.Indentificacao;
                        command.Parameters["@tipopessoa"].Value = cliente.TipoPessoa;
                        command.Parameters["@sexo"].Value = cliente.Sexo;
                        command.Parameters["@ticketmedio"].Value = cliente.TicketMedio.ToString().Replace('.', ',');
                        command.Parameters["@ultimavenda"].Value = cliente.UltimaVenda.ToString("yyyy-MM-ddTHH:mm:ss");
                        command.Parameters["@valortotal"].Value = venda.Valor.ToString().Replace('.', ',');
                        command.Parameters["@valoritens"].Value = venda.VlrItens.ToString().Replace('.', ',');
                        command.Parameters["@qtItens"].Value = venda.QntItens;
                        command.Parameters["@datavenda"].Value = venda.Data.ToString("yyyy-MM-ddTHH:mm:ss");
                        command.Parameters["@idnf"].Value = venda.IDNF;
                        command.Parameters["@Veiculo"].Value = String.IsNullOrEmpty(venda.Veiculo) ? "Não Identificado" : venda.Veiculo;
                        command.Parameters["@Ano"].Value = String.IsNullOrEmpty(venda.Ano) ? "Não Identificado" : venda.Ano;
                        command.Parameters["@Placa"].Value = String.IsNullOrEmpty(venda.Placa) ? "Não Identificado" : venda.Placa;
                        command.Parameters["@Km"].Value = String.IsNullOrEmpty(venda.KM) ? "Não Identificado" : venda.KM;
                        command.Parameters["@FaixaAno"].Value = String.IsNullOrEmpty(venda.FaixaAno) ? "Não Identificado" : venda.FaixaAno;
                        command.Parameters["@FaixaTicketmedio"].Value = cliente.FaixaTicketMedio;
                        command.ExecuteNonQuery();
                    }
                }
                conexao.Close();
            }
            catch (Exception ex)
            {

            }
        }
        private void MontarDicionarioVendas(DataTable tableNotas, Dictionary<long, Cliente> dicVendas)
        {
            foreach (DataRow row in tableNotas.Rows)
            {
                Cliente cliente = new Cliente();
                cliente.Nome = row[1].ToString();
                if (row[2].ToString() != String.Empty)
                    cliente.Indentificacao = Convert.ToInt64(row[2]);
                else if (row[3].ToString() != String.Empty)
                    cliente.Indentificacao = Convert.ToInt64(row[3]);

                cliente.TipoPessoa = row.ItemArray[4].ToString() == "F" ? "Fisica" : "Juridica";
                string vlrItens = Convert.ToDouble(row.ItemArray[5].ToString()).ToString().Replace(',', '.');
                cliente.Sexo = PegarSexo(cliente.Nome, cliente.TipoPessoa);

                Venda venda = new Venda();
                venda.IDNF = Convert.ToInt32(row[0]);
                venda.Valor = Convert.ToDouble(row[5]);
                venda.Data = Convert.ToDateTime(row[6]);
                if ((row[7] is DBNull) == false)
                    venda.VlrItens = Convert.ToDouble(row[7]);
                if ((row[8] is DBNull) == false)
                    venda.CustoItens = Convert.ToDouble(row[8]);
                if ((row[9] is DBNull) == false)
                    venda.QntItens = Convert.ToInt16(row[9]);
                venda.DadosAdicionais = row[10].ToString();

                if (dicVendas.ContainsKey(cliente.Indentificacao))
                    dicVendas[cliente.Indentificacao].ListaVendas.Add(venda);
                else
                {
                    dicVendas.Add(cliente.Indentificacao, cliente);
                    dicVendas[cliente.Indentificacao].ListaVendas.Add(venda);
                }
            }
        }
        private void CalcularTicketMedio(Dictionary<long, Cliente> dicVendas)
        {
            double valorTotalTicketMedio = 0;
            int totalVendas = 0;
            foreach (long indetificacao in dicVendas.Keys)
            {
                double valorTotal = 0;
                DateTime ultimaVenda = DateTime.MinValue;
                foreach (Venda venda in dicVendas[indetificacao].ListaVendas)
                {
                    valorTotalTicketMedio = valorTotalTicketMedio + venda.Valor;
                    valorTotal = valorTotal + venda.Valor;
                    if (ultimaVenda < venda.Data)
                        ultimaVenda = venda.Data;
                }
                totalVendas = totalVendas + dicVendas[indetificacao].ListaVendas.Count;
                dicVendas[indetificacao].TicketMedio = valorTotal / dicVendas[indetificacao].ListaVendas.Count;
                dicVendas[indetificacao].FaixaTicketMedio = ClassificarTicketMedio(dicVendas[indetificacao].TicketMedio);
                dicVendas[indetificacao].UltimaVenda = ultimaVenda;
            }
        }
        private string ClassificarTicketMedio(double ticketMedio)
        {
            if (ticketMedio < 200)
                return "Muito Baixo";
            else if (ticketMedio < 350)
                return "Baixo";
            else if (ticketMedio < 600)
                return "Médio";
            else if (ticketMedio < 750)
                return "Alto";
            else
                return "Muito Alto";
        }
        private string ClassificarAno(string ano)
        {
            try
            {
                if (string.IsNullOrEmpty(ano))
                    return "Não Identificado";

                int anoCarro = Convert.ToInt32(ano);

                if (anoCarro < 1950)
                    return "1900 a 1949";
                else if (anoCarro < 1955)
                    return "1950 a 1954";
                else if (anoCarro < 1960)
                    return "1955 a 1959";
                else if (anoCarro < 1965)
                    return "1960 a 1964";
                else if (anoCarro < 1970)
                    return "1965 a 1969";
                else if (anoCarro < 1975)
                    return "1970 a 1974";
                else if (anoCarro < 1980)
                    return "1975 a 1979";
                else if (anoCarro < 1985)
                    return "1980 a 1984";
                else if (anoCarro < 1990)
                    return "1985 a 1989";
                else if (anoCarro < 1995)
                    return "1990 a 1994";
                else if (anoCarro < 2000)
                    return "1995 a 1999";
                else if (anoCarro < 2005)
                    return "2000 a 2004";
                else if (anoCarro < 2010)
                    return "2005 a 2009";
                else if (anoCarro < 2015)
                    return "2010 a 2014";
                else if (anoCarro < 2020)
                    return "2015 a 2019";
                else if (anoCarro > DateTime.Now.Year + 1)
                    return "Não identificado";
                else
                    return "2020";
            }
            catch (Exception ex)
            {
                return "Não identificado";
            }
        }
        private void RevisoesAtrasadas(Dictionary<long, Cliente> dicVendas)
        {
            foreach (Cliente cliente in dicVendas.Values)
            {
                bool revisao2anos = false;
                bool revisao1anos = false;
                bool revisaoAnoAtual = false;

                foreach (Venda venda in cliente.ListaVendas)
                {
                    if (venda.Data.Year == DateTime.Now.AddYears(-2).Year)
                        revisao2anos = true;
                    else if (venda.Data.Year == DateTime.Now.AddYears(-1).Year)
                        revisao1anos = true;
                    else if (venda.Data.Year == DateTime.Now.Year)
                        revisaoAnoAtual = true;
                }

                if (revisao2anos && revisao1anos && !revisaoAnoAtual)
                    cliente.RevisaoAtrasada = true;
            }


            List<Cliente> clienteRevisaoAtrasada = new List<Cliente>();
            clienteRevisaoAtrasada = dicVendas.Values.Where(v => v.RevisaoAtrasada == true).ToList();

            conexao.Open();
            foreach (Cliente cliente in clienteRevisaoAtrasada)
            {
                Venda venda = cliente.ListaVendas.OrderByDescending(pet => pet.Data).FirstOrDefault();

                commandRevisaoAtrasada.Parameters["@NomeCliente"].Value = cliente.Nome;
                commandRevisaoAtrasada.Parameters["@IdentificacaoCliente"].Value = cliente.Indentificacao;
                commandRevisaoAtrasada.Parameters["@UltimaRevisao"].Value = cliente.UltimaVenda;
                commandRevisaoAtrasada.Parameters["@TicketMedio"].Value = cliente.TicketMedio;
                commandRevisaoAtrasada.Parameters["@TipoPessoa"].Value = cliente.TipoPessoa;
                commandRevisaoAtrasada.Parameters["@Sexo"].Value = cliente.Sexo;
                commandRevisaoAtrasada.Parameters["@Veiculo"].Value = String.IsNullOrEmpty(venda.Veiculo) ? "Não Identificado" : venda.Veiculo;
                commandRevisaoAtrasada.Parameters["@Ano"].Value = String.IsNullOrEmpty(venda.Ano) ? "Não Identificado" : venda.Ano;
                commandRevisaoAtrasada.Parameters["@Placa"].Value = String.IsNullOrEmpty(venda.Placa) ? "Não Identificado" : venda.Placa;
                commandRevisaoAtrasada.Parameters["@Km"].Value = String.IsNullOrEmpty(venda.KM) ? "Não Identificado" : venda.KM;
                commandRevisaoAtrasada.Parameters["@FaixaAno"].Value = String.IsNullOrEmpty(venda.FaixaAno) ? "Não Identificado" : venda.FaixaAno;
                commandRevisaoAtrasada.Parameters["@FaixaTicketmedio"].Value = cliente.FaixaTicketMedio;
                commandRevisaoAtrasada.ExecuteNonQuery();
            }
            conexao.Close();
        }
        private void SalvarDadosServicos(DataTable tableServico)
        {
            List<Servico> listServices = new List<Servico>();

            foreach (DataRow row in tableServico.Rows)
            {
                Servico servico = new Servico();
                servico.IDNF = Convert.ToInt32(row[0]);
                servico.TipoServico = row[1].ToString();
                servico.NomeServico = row[2].ToString();
                servico.Funcionario = row[3].ToString();
                servico.Tempo = row[4].ToString();
                servico.VlrUnit = Convert.ToDouble(row[5]);
                servico.QntItens = Convert.ToInt32(row[6]);
                servico.VlrTotal = Convert.ToDouble(row[7]);
                listServices.Add(servico);
            }

            try
            {
                conexao.Open();
                foreach (Servico servico in listServices)
                {
                    commandServico.Parameters["@IDNF"].Value = servico.IDNF;
                    commandServico.Parameters["@TipoServico"].Value = servico.TipoServico;
                    commandServico.Parameters["@NomeServico"].Value = servico.NomeServico;
                    commandServico.Parameters["@Funcionario"].Value = servico.Funcionario;
                    commandServico.Parameters["@Tempo"].Value = servico.Tempo;
                    commandServico.Parameters["@VlrUnit"].Value = servico.VlrUnit;
                    commandServico.Parameters["@QntItens"].Value = servico.QntItens;
                    commandServico.Parameters["@VlrTotal"].Value = servico.VlrTotal;
                    commandServico.ExecuteNonQuery();
                }
                conexao.Close();
            }
            catch (Exception ex)
            {

            }
        }
        private string PegarSexo(string nome, string tipopessoa)
        {
            if (tipopessoa == "Juridica")
                return "Juridica";
            string primeiroNome = nome.Split(' ')[0];
            if (dicNames.ContainsKey(primeiroNome))
                return dicNames[primeiroNome];
            else
                return "Indefinido";
        }
    }

    public class Cliente
    {
        public long Indentificacao;
        public string Nome;
        public double TicketMedio;
        public string FaixaTicketMedio;
        public DateTime UltimaVenda;
        public string Sexo;
        public string TipoPessoa;
        public List<Venda> ListaVendas = new List<Venda>();
        public bool RevisaoAtrasada;
    }

    public class Venda
    {
        public int IDNF;
        public DateTime Data;
        public double Valor;
        public int QntItens;
        public double VlrItens;
        public double CustoItens;
        public string DadosAdicionais;
        public string Veiculo;
        public string Placa;
        public string KM;
        public string Ano;
        public string FaixaAno;
    }

    public class Servico
    {
        public int IDNF;
        public string TipoServico;
        public string NomeServico;
        public string Funcionario;
        public string Tempo;
        public double VlrUnit;
        public int QntItens;
        public double VlrTotal;
    }
}
