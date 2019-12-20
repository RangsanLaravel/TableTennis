using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessUtility
{
    public class RepositoryBase
    {
        public MySqlConnection connection;
        private MySqlTransaction transaction;

        public virtual void Repository(string connectionName)
        {
            connection = new MySqlConnection(connectionName);
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public void BeginTransaction()
        {
            this.transaction = connection.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }
    }
}
