using Monitor.Infrastructure.Enums;
using System;

namespace Monitor.Infrastructure.ViewModel.Models
{
    public class OrderBookModel : BaseVM
    {
        public DateTime Time { get; set; }

        private decimal _Price { get; set; }
        public decimal Price
        {
            get { return _Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        private decimal _Quantity { get; set; }
        public decimal Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }
        
        private bool _Updated { get; set; }
        public bool Updated
        {
            get { return _Updated; }
            set
            {
                if (_Updated != value)
                {
                    _Updated = value;
                    OnPropertyChanged("Updated");
                }
            }
        }


        private OrderBookType _Type { get; set; }
        public OrderBookType Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

    }
}
