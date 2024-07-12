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
        public int cipherCompatibility { get; set; }
        public int cipherDefaultKdfIter { get; set; }
        public int cipherDdefaultPageSize { get; set; }
        public string cipherDefaultHmacAlgorithm { get; set; }
        public string cipherDefaultKdfAlgorithm { get; set; }

        public PreferenceData()
        {
            this.decryptKey = "";
            this.cipherCompatibility = 3;
            this.cipherDefaultKdfIter = 64000;
            this.cipherDdefaultPageSize = 1024;
            this.cipherDefaultHmacAlgorithm = "HMAC_SHA1";
            this.cipherDefaultKdfAlgorithm = "PBKDF2_HMAC_SHA1";

        }

        public PreferenceData(string decryptKey, int cipherCompatibility, int cipherDefaultKdfIter, int cipherDdefaultPageSize, string cipherDefaultHmacAlgorithm, string cipherDefaultKdfAlgorithm)
        {
            this.decryptKey = decryptKey;
            this.cipherCompatibility = cipherCompatibility;
            this.cipherDefaultKdfIter = cipherDefaultKdfIter;
            this.cipherDdefaultPageSize = cipherDdefaultPageSize;
            this.cipherDefaultHmacAlgorithm = cipherDefaultHmacAlgorithm;
            this.cipherDefaultKdfAlgorithm = cipherDefaultKdfAlgorithm;
        }
    }
}
