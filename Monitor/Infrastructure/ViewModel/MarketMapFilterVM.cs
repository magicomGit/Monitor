using Monitor.Infrastructure.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel
{
    public class MarketMapFilterVM
    {
        public static ObservableCollection<MarketMapFilterModel> NATR
        {
            get; set;
        } = new ObservableCollection<MarketMapFilterModel>();


        public static ObservableCollection<MarketMapFilterModel> Volume
        {
            get; set;
        } = new ObservableCollection<MarketMapFilterModel>();


        public MarketMapFilterVM()
        {
            
        }
    }
}
