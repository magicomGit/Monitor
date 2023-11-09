using Monitor.Infrastructure.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel
{
    internal class OrderBookVM
    {
        public static ObservableCollection<OrderBookModel> OrderBook
        {
            get; set;
        } = new ObservableCollection<OrderBookModel>();

        public static ObservableCollection<OrderBookModel> OrderBookAsks
        {
            get; set;
        } = new ObservableCollection<OrderBookModel>();
        
        public static ObservableCollection<OrderBookModel> OrderBookBids
        {
            get; set;
        } = new ObservableCollection<OrderBookModel>();
        


        public OrderBookVM()
        {
            OrderBookAsks.Clear();
            OrderBookBids.Clear();
            OrderBook.Clear();

            for (int i = 0; i < 20; i++)
            {
                OrderBookAsks.Add(new OrderBookModel { Type = Enums.OrderBookType.Bid });
                OrderBook.Add(new OrderBookModel { Type = Enums.OrderBookType.Bid });
            }

            for (int i = 0; i < 20; i++)
            {
                OrderBookBids.Add(new OrderBookModel { Type = Enums.OrderBookType.Ask });
                OrderBook.Add(new OrderBookModel { Type = Enums.OrderBookType.Ask });
            }
        }
    }
}
