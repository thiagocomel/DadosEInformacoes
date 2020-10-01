using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
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

