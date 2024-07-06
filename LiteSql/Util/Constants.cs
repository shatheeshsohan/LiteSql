using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.Util
{
    public static class Constants
    {
        public static int OBJKEY_INDEX = 0;
        public static string DISCONNET = "Disconnect";
        public static string CONNET = "Connect";
        public static string BASE_TABLE_SELECT = $"SELECT * FROM ";
        public static string ROW_UPDATE = "UPDATE";
        public static List<string> DB_BASE_VALUE_LIST = new List<string> { "row_id", "Bob", "Charlie" };
    }
}
