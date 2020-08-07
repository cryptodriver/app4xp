using DeviceId;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helpers
{
    public class DeviceHelper
    {
        public static string GetDeviceId(string _seed = "")
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            string _devid = new DeviceIdBuilder().AddMachineName().AddMotherboardSerialNumber().AddProcessorId().AddSystemDriveSerialNumber().ToString();
            string _source = string.Format("{0}@{1}", _devid, _seed);

            byte[] md5byte = md5.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(_source));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < md5byte.Length; i++)
            {
                result.Append(md5byte[i].ToString("x2"));
            }

            return result.ToString();
        }

        public static string GetDeviceToken()
        {
            // Do nothing for now, would be used for FCMS
            return string.Empty;
        }
    }
}
