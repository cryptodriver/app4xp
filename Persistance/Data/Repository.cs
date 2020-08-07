using Domain.Helpers;
using Domain.Models;
using System.Collections.Generic;

namespace Persistance.Data
{
    public class Repository
    {
        public static List<BaseModel> InitDB()
        {
            var db = new List<BaseModel>();
            db.AddRange(Settings());

            return db;
        }

        public static List<BaseModel> Settings()
        {
            return new List<BaseModel>() {
                new Setting() { Key = SettingKey.COM_THEMA, Value = "Cyan" },
            };
        }
    }
}
