using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel.Models
{
    public class Info: BaseVM
    {
        private string _Message { get; set; }
        public string Message
        {
            get { return _Message; }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        public decimal MaxQuantity { get; set; }
    }
}
