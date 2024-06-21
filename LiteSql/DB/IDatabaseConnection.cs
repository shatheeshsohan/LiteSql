using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.DB
{
    interface IDatabaseConnection
    {
        public SQLiteConnection getConnection();
    }
}
