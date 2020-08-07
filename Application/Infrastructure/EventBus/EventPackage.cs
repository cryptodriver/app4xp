using System;
using System.Reflection;
using System.Text;

namespace Application.Infrastructure.EventBus
{
    public class EventPackage
    {
        public string Id => Guid.NewGuid().ToString();

        public EventTag Tag { get; set; }

        public dynamic Payload { get; set; }

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
