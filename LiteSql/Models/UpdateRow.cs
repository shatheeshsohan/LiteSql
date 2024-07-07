using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.Models
{
    public class UpdateRow : IRemoteRow
    {
        public string tableName { get; set; }
        public string columnName { get; set; }
        public string value { get; set; }
        public string objkey { get; set; }
        public string rowType { get;set; }

        public UpdateRow(string tableName, string columnName, string value, string objkey, string rowType)
        {
            this.tableName = tableName;
            this.columnName = columnName;
            this.value = value;
            this.objkey = objkey;
            this.rowType = rowType;
        }

        public void executeSQL(SQLiteConnection conn)
        {
            string formattedSQL = String.Format("UPDATE {0} SET {1} = '{2}' WHERE obj_key = '{3}'", this.tableName, this.columnName, this.value, this.objkey);
            conn.Execute(formattedSQL);
        }
    }
}
