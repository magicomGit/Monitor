using Monitor.Infrastructure;
using Monitor.Infrastructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Monitor
{
    /// <summary>
    /// Логика взаимодействия для OrderBookWindow.xaml
    /// </summary>
    public partial class OrderBookWindow : Window
    {
        public OrderBookWindow()
        {
            InitializeComponent();
        }

        public void SetSymbol()
        {
            TbSymbol.Text = Services.Processor.SelectedSymbol;
        }
        private  void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TbSymbol.Text = Services.Processor.SelectedSymbol;
            //Services.ShowMessage("Subscribe To Order Book Updates..");
            //var result = await Services.binanceSocketClient.UsdFuturesApi.SubscribeToPartialOrderBookUpdatesAsync("XRPUSDT", 20, 100, data =>
            //{
            //    var maxAsk = data.Data.Asks.Select(s => s.Quantity).Max();
            //    var maxBid = data.Data.Bids.Select(s => s.Quantity).Max();
            //    if (maxAsk > maxBid)
            //    {
            //        Services.Info.MaxQuantity = maxAsk;
            //    }
            //    else
            //    {
            //        Services.Info.MaxQuantity = maxBid;
            //    }


            //    int cnt = 19;
            //    foreach (var item in data.Data.Asks)
            //    {
            //        OrderBookVM.OrderBook[cnt].Price = item.Price;
            //        OrderBookVM.OrderBook[cnt].Quantity = item.Quantity;
            //        OrderBookVM.OrderBook[cnt].Type = Infrastructure.Enums.OrderBookType.Ask;
            //        cnt--;
            //    };

            //    cnt = 20;
            //    foreach (var item in data.Data.Bids)
            //    {
            //        OrderBookVM.OrderBook[cnt].Price = item.Price;
            //        OrderBookVM.OrderBook[cnt].Quantity = item.Quantity;
            //        OrderBookVM.OrderBook[cnt].Type = Infrastructure.Enums.OrderBookType.Bid;
            //        cnt++;
            //    };

            //    //Services.SetCandles(Services.TimeFrame, Data.Timestamp, Data.Data.Symbol); Services.Info.MaxQuantity
            //});

            //result.Data.CloseAsync().Wait();
        }
    }
}
