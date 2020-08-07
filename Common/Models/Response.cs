using Common.Helpers;
using Newtonsoft.Json;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace Common.Models
{
    public class Response
    {
        public Status Code { get; set; }

        public dynamic Body { get; set; } = new ExpandoObject();

        public override string ToString()
        {
            PropertyInfo[] _properties = GetType().GetProperties();

            var sb = new StringBuilder();
            foreach (var prop in _properties)
            {
                var value = prop.GetValue(this, null) ?? "(null)";

                var strvl = value.ToString();
                if (strvl.Contains("ExpandoObject"))
                {
                    strvl = JsonConvert.SerializeObject(value);
                }
                sb.AppendLine(string.Format("{0}: {1}", prop.Name, strvl));
            }

            return sb.ToString();
        }
    }
}
