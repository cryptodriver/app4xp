using System.Reflection;
using System.Text;

namespace Presentaion.Desktop.WPF.Models
{
    public class Item
    {
        // would be used for UUID
        public string Key { get; set; }

        // usually, use these two properties
        public string Code { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            PropertyInfo[] _properties = GetType().GetProperties();

            var sb = new StringBuilder();
            foreach (var prop in _properties)
            {
                var value = prop.GetValue(this, null) ?? "(null)";
                sb.AppendLine(prop.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }
}
