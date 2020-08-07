using Application;
using Application.Infrastructure.SQLiter;
using Domain.Models;
using Persistance.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Persistance.Helpers
{
    public class SqliteHelper
    {
        private ISQLiter _sqliter;
        private SQLiteConnection _dbc;
        private Dictionary<string, dynamic> _all;
        private Config _config;

        public SqliteHelper(Config config, Dictionary<string, dynamic> all)
        {
            _config = config;
            _all = all;

            if (_sqliter == null)
            {
                _sqliter = new SQLiter(new SQLiteConfig { DBName = _config.DefaultLDBName, DBPath = _config.CurrentPath });
                _dbc = _sqliter.DBC();
            }
        }

        public void Go()
        {
            InitDb();

            LoadSettings();

            Garbage();
        }

        private bool InitDb()
        {
            if (File.Exists(_sqliter.Config.DBFile))
            {
                Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

                foreach (Assembly asm in asms)
                {
                    Type[] types = asm.GetTypes();

                    foreach (var type in types)
                    {
                        if (type.BaseType == typeof(BaseModel) && type != typeof(BaseModel))
                        {
                            _dbc.CreateTable(type);
                        }
                    }
                }

                var aset = _dbc.Table<Setting>().Where(m => m.AccountId == 0).ToList();

                if (aset.Count < 1)
                {
                    Repository.InitDB().ForEach(m =>
                    {
                        _dbc.Insert(m);
                    });
                }
            }

            return true;
        }

        private void LoadSettings()
        {
            var _settings = new Dictionary<string, string>();

            var act = _dbc.Table<Account>().OrderByDescending(x => x.IsCurrent);
            if (act.Count() > 0)
            {
                var cur = act.First();

                _config.CurrentId = cur.Id;
                _config.CurrentAccount = cur.Name;
                _config.CurrentPassword = cur.Password;
            }

            var items = _dbc.Table<Setting>().Where(m => m.AccountId == 0);
            foreach (var item in items)
            {
                _settings.Add(item.Key, item.Value);
                if (_config.CurrentId > 0)
                {
                    var mys = _dbc.Table<Setting>().Where(m => m.AccountId == _config.CurrentId && m.Key == item.Key);
                    if (mys.Count() > 0)
                    {
                        _settings[item.Key] = mys.First()?.Value;
                    }
                }
            }
            _all.Add("CSSET", _settings);
        }

        private void Garbage()
        {
            if (_dbc != null)
            {
                _dbc.Dispose();
            }
        }

    }
}
