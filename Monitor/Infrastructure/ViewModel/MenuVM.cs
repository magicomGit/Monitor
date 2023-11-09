using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor.Infrastructure.ViewModel.Models;

namespace Monitor.Infrastructure.ViewModel
{
    public class MenuVM
    {
        public static ObservableCollection<MenuModel> Menu
        {
            get; set;
        } = new ObservableCollection<MenuModel>();


        public MenuVM()
        {
            Menu = new ObservableCollection<MenuModel>
            {
                new MenuModel {Name = "Market map", Selected= true},
                new MenuModel{Name = "Filters", Selected= false}
            };
        }
    }
}
