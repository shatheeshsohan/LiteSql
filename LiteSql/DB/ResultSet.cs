using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.DB
{
    public class ResultSet
    {
        public ResultSet() { }
        public List<object> Values = new List<object>();

        public override string ToString()
        {
            if (Values == null)
                return "ResultSet with no values";

            return $"ResultSet with {Values.Count} values";
        }
    }
}
