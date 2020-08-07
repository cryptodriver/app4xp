using SQLite;
using System;
using System.Reflection;
using System.Text;

namespace Domain.Models
{
    public class BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public long CreatedAt { get; set; } = System.DateTime.Now.Ticks;
        public long UpdatedAt { get; set; } = System.DateTime.Now.Ticks;

        public DateTime ReadableCreatedAt
        {
            get
            {
                return new DateTime(this.CreatedAt).ToLocalTime();
            }
        }

        public DateTime ReadableUpdatedAt
        {
            get
            {
                return new DateTime(this.UpdatedAt).ToLocalTime();
            }
        }

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
