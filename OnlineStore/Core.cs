using OnlineStore.Models;
using System.Collections.ObjectModel;

namespace OnlineStore
{
    public class Core
    {
        public static StoreDbContext Context = new StoreDbContext();

        public static ObservableCollection<Product> Cart = new ObservableCollection<Product>();
    }
}
