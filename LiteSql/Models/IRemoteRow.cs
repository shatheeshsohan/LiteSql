using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.Models
{
    public interface IRemoteRow
    {
        public string rowType { get; set; }

        public void executeSQL(SQLiteConnection conn);

    }
}
