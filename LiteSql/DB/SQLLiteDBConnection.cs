using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.DB
{
    class SQLLiteDBConnection : IDatabaseConnection
    {
        SQLiteConnection sqlite;
        private string connString;

        public SQLLiteDBConnection(string conString)
        {
            this.connString = connString;
        }

        public SQLiteConnection getConnection()
        {
            sqlite = new SQLiteConnection("Data Source=" + this.connString);
            return sqlite;
        }
    }
}
