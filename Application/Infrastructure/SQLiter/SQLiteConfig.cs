using System;
using System.IO;

namespace Application.Infrastructure.SQLiter
{
    public class SQLiteConfig
    {
        public string DBName { get; set; }

        public string DBPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public string DBFile
        {
            get
            {
                return Path.Combine(DBPath, DBName);
            }
        }
    }
}
