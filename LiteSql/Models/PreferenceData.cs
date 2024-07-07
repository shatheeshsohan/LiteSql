using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.Models
{
    public class PreferenceData
    {
        public string decryptKey { get; set; }

        public PreferenceData()
        {
            this.decryptKey = "";
        }
    }
}
