using Application;
using Application.Core;
using Application.Infrastructure.SQLiter;
using Common.Helpers;
using Common.Models;
using Domain.Models;
using SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Services
{
    [Autofac(true)]
    public class SqliteService : ISqliteService
    {
        private ISQLiter _sqliter;
        private SQLiteConnection _dbc;

        public SqliteService()
        {
            if (_sqliter == null)
            {
                _sqliter = ServiceProvider.Instance.Get<ISQLiter>();
                _dbc = _sqliter.DBC();
            }
        }

        public Task<TResponse> UpdateSetting(TRequest request)
        {
            LoggerHelper.Write();

            return Task.Run(() =>
            {
                var key = ((object)request.Body.Key).ToString();
                var value = ((object)request.Body.Value).ToString();

                if (string.IsNullOrEmpty(request.Body.AccountName))
                {
                    var items = _dbc.Table<Setting>().Where(m => m.AccountId == 0 && m.Key == key);
                    if (items.Count() > 0)
                    {
                        var one = items.First();
                        one.Value = value;

                        _dbc.Update(one);
                    }
                }
                else
                {
                    var name = ((object)request.Body.AccountName).ToString();

                    var accts = _dbc.Table<Account>().Where(m => m.Name == name);
                    if (accts.Count() > 0)
                    {
                        var acct = accts.First();

                        var items = _dbc.Table<Setting>().Where(m => m.AccountId == acct.Id && m.Key == key);
                        if (items.Count() > 0)
                        {
                            var one = items.First();
                            one.Value = value;

                            _dbc.Update(one);
                        }
                        else
                        {
                            _dbc.Insert(new Setting()
                            {
                                AccountId = acct.Id,
                                Key = key,
                                Value = value,
                            });
                        }
                    }
                }

                return new TResponse() { Code = Status.OK };
            });
        }

        private int GetAccountId(string name) => _dbc.Table<Account>().SingleOrDefault(ac => 0 == name.CompareTo(ac.Name))?.Id ?? -1;
    }
}
