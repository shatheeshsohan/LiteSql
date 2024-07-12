using LiteSql.Models;
using SQLite;

namespace LiteSql.DB
{
    public class SQLiteCipherConnection : IDatabaseConnection
    {
        private string dbPath;
        private SQLiteConnection? dbConnection = null;
        private SettingsData? settingsData = null;

        public SQLiteCipherConnection(string dbPath, SettingsData settingsData) { 
            this.dbPath = dbPath;
            this.settingsData = settingsData;
        }

        public SQLiteConnection getConnection()
        {
            if (settingsData != null)
            {
                SQLiteConnectionString options = new SQLiteConnectionString(dbPath, true, key: settingsData.decryptKey,
                postKeyAction: db =>
                {
                    db.Execute("PRAGMA cipher_compatibility    = " + settingsData.cipherCompatibility + ";");
                    db.Execute("PRAGMA cipher_default_kdf_iter = " + settingsData.cipherDefaultKdfIter + ";");
                    db.Execute("PRAGMA cipher_default_page_size  = " + settingsData.cipherDdefaultPageSize + ";");
                    db.Execute("PRAGMA cipher_default_hmac_algorithm  = " + settingsData.cipherDefaultHmacAlgorithm + ";");
                    db.Execute("PRAGMA cipher_default_kdf_algorithm   = " + settingsData.cipherDefaultKdfAlgorithm + ";");
                });

                dbConnection = new SQLiteConnection(options);
            }
            return dbConnection;
        }
    }
}
