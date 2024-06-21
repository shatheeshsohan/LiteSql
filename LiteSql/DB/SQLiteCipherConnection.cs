using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteSql.DB
{
    public class SQLiteCipherConnection : IDatabaseConnection
    {
        private string dbPath;
        private string encryptKey;
        private SQLiteConnection? dbConnection = null;

        public SQLiteCipherConnection(string dbPath, string encryptKey) { 
            this.dbPath = dbPath;
            this.encryptKey = encryptKey;
        }

        public SQLiteConnection getConnection()
        {
            SQLiteConnectionString options = new SQLiteConnectionString(dbPath, true, key: encryptKey,
            postKeyAction: db =>
            {
             db.Execute("PRAGMA cipher_compatibility    = 3;");
             db.Execute("PRAGMA cipher_default_kdf_iter = 64000;");
             db.Execute("PRAGMA cipher_default_page_size  = 1024;");
             db.Execute("PRAGMA cipher_default_hmac_algorithm  = HMAC_SHA1;");
             db.Execute("PRAGMA cipher_default_kdf_algorithm   = PBKDF2_HMAC_SHA1;");
            });

            dbConnection = new SQLiteConnection(options);

            return dbConnection;
        }
    }
}
