using Application;
using Application.Helpers;
using Common.Helpers;
using I18NPortable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Persistance.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common
{
    public class XSetting
    {
        public static string DefaultLocal = "ja";

        public static Config Config { get; set; }

        public static II18N Strings { get; } = I18N.Current;

        public static Dictionary<string, dynamic> All = new Dictionary<string, dynamic>();

        public static void Init(Config _config)
        {
            Config = _config;

            Config.CurrentPath = BasePath;

            All.Clear();

            // Load setting from file.json
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            foreach (var res in resources)
            {
                if (res.EndsWith(".json"))
                {
                    using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(res)))
                    {
                        All.Add(res.Split('.')[2].ToUpper(), JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd()));
                    }
                }
            }

            // Load brand properties
            foreach (KeyValuePair<string, dynamic> kv in All)
            {
                var paths = ((JObject)kv.Value).SelectTokens($"..[?(@ =~ /^*{{.+}}*$/)]").Select(v => v.Path).ToList();

                foreach (var path in paths)
                {
                    var jtoken = ((JObject)kv.Value).SelectToken(path);

                    var ttt = jtoken.ToString();
                    var kkk = ttt.Substring(ttt.IndexOf('{') + 1, (ttt.IndexOf('}') - ttt.IndexOf('{') - 1));

                    // find value by key in another setting file
                    var vvv = String.Empty;
                    foreach (KeyValuePair<string, dynamic> ikv in All)
                    {
                        if (kkk.Contains(ikv.Key))
                        {
                            vvv = ((JObject)ikv.Value).SelectToken(kkk.Substring(kkk.IndexOf('.') + 1)).ToString();
                            break;
                        }
                    }

                    jtoken.Replace(ttt.Replace(string.Format("{{{0}}}", kkk), vvv));
                }
            }

            // Load location resource 
            I18N.Current.SetNotFoundSymbol("$").SetFallbackLocale(DefaultLocal).Init(assembly);

            // Set log level
            Config.DefaultLogLevel = All["LOGGER"]["level"];

            // Load from db
            new SqliteHelper(Config, XSetting.All).Go();
        }

        public static void Completed()
        {
            // Do something 
        }

        public static string BasePath
        {
            get
            {
                string _basepath = string.Empty;

                switch (Config.CurrentUI)
                {
                    case UIType.WPF:
                        _basepath = AppDomain.CurrentDomain.BaseDirectory;
                        break;
                    case UIType.MACOS:
                        _basepath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        break;
                    case UIType.IOS:
                    case UIType.DROID:
                        _basepath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        break;
                    default:
                        break;
                }

                return _basepath;
            }
        }

        private static string DeviceToken
        {
            get
            {
                string _token = string.Empty;

                switch (Config.CurrentUI)
                {
                    case UIType.WPF:
                        _token = DeviceHelper.GetDeviceId(Config.CurrentAccount);
                        break;
                    case UIType.MACOS:
                    case UIType.IOS:
                    case UIType.DROID:
                        _token = DeviceHelper.GetDeviceToken();
                        break;
                    default:
                        break;
                }

                return _token;
            }
        }

        public static string LogPath => Path.Combine(BasePath, Config.LogLPath);
    }
}
