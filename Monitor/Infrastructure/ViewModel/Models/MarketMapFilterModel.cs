using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel.Models
{
    public class MarketMapFilterModel : BaseVM
    {
        private string _Symbol { get; set; }
        public string Symbol
        {
            get { return _Symbol; }
            set
            {
                if (_Symbol != value)
                {
                    _Symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
        }


        private decimal _Value { get; set; }
        public decimal Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }
    }
}
