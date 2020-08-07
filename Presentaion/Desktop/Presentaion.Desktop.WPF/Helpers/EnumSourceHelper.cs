using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace Presentaion.Desktop.WPF.Helpers
{
    public class EnumSourceHelper<T> : MarkupExtension
    {
        private static string DisplayName(T value)
        {
            var fileInfo = value.GetType().GetField(value.ToString());
            var descriptionAttribute = (DescriptionAttribute)fileInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault();
            return descriptionAttribute.Description;
        }

        public IEnumerable Source { get; }
            = typeof(T).GetEnumValues()
                .Cast<T>()
                .Select(value => new { Code = value, Name = DisplayName(value) });

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
