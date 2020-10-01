using System;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace fireBirdWin
{
    /// <summary>
    /// Usa padrão Singleton para obter uma instancia do FireBird
    /// </summary>
    public class daoFireBird
    {
        private static readonly daoFireBird instanciaFireBird = new daoFireBird();

        private daoFireBird() { }

        public static daoFireBird getInstancia()
        {
            return instanciaFireBird;
        }

        public FbConnection getConexao()
        {
            string conn = @"DataSource=localhost; Database=F:\BKP\Particular\Irmãos Neto\Dados.FDB; UserId=SYSDBA; Pwd=masterkey";
            return new FbConnection(conn);
        }
    }
}