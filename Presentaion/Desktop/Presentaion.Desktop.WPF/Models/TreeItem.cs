using System.Collections.ObjectModel;

namespace Presentaion.Desktop.WPF.Models
{
    public class TreeItem : Item
    {
        public ObservableCollection<TreeItem> Childs { get; set; } = new ObservableCollection<TreeItem>();
    }
}
