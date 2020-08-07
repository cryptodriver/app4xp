using SQLite;

namespace Application.Infrastructure.SQLiter
{
    public class SQLiter : ISQLiter
    {
        private SQLiteConnection _dbc;

        private SQLiteConfig _config;

        public SQLiter(SQLiteConfig config)
        {
            _config = config;
        }

        public SQLiteConnection DBC()
        {
            if (_dbc == null)
            {
                this._dbc = new SQLiteConnection(_config.DBFile, SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite);
            };

            return _dbc;
        }

        public SQLiteConfig Config
        {
            get
            {
                return this._config;
            }
        }
    }
}
