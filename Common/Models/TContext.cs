using Application.Interfaces;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Common.Models
{
    public class TContext : IContext
    {
        public CancellationToken CToken = new CancellationTokenSource().Token;

        public dynamic Body { get; set; } = new ExpandoObject();

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
