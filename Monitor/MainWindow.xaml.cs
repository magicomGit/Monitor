using Monitor.Infrastructure;
using Monitor.Infrastructure.ViewModel;
using Monitor.Infrastructure.ViewModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StakMap.DataContext = Services.Info;

            Services.SymbolData = await Services.binanceRestClient.UsdFuturesApi.ExchangeData.GetExchangeInfoAsync();
            Services.ShowMessage("Getting Symbol Data..");
            //Services.AllSymbols = Services.SymbolData.Data.Symbols.Select(x => x.Name);
            Services.Symbols = new List<string> { "ETHUSDT", "BTCUSDT", "XRPUSDT", "BNBUSDT", "ADAUSDT", "SOLUSDT", "LTCUSDT", "CHZUSDT", "LINKUSDT", "EOSUSDT", "TRXUSDT", "FILUSDT" };

            //---- словарь свечей
            foreach (string symbol in Services.Symbols)
            {
                Services.CandleDictionary.Add(symbol, new Dictionary<int, List<Candle>>());
                Services.CandleDictionary[symbol].Add(Services.TimeFrame, new List<Candle>());
            }


            //market map
            int cnt = 0;
            foreach (var symbol in Services.SymbolData.Data.Symbols)
            {
                if (cnt < 10)
                {
                    MarketMapFilterVM.NATR.Add(new MarketMapFilterModel { Symbol = symbol.Name, Value = 0 });
                    MarketMapFilterVM.Volume.Add(new MarketMapFilterModel { Symbol = symbol.Name, Value = 0 });
                }
                cnt++;
            }

            await Services.GetSymbolsData();

            Services.Processor.StartBinanceSubscription();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var stackPanel = (StackPanel)sender;
            var name = ((TextBlock)stackPanel.Children[0]).Text;



            foreach (var item in MenuVM.Menu)
            {
                if (item.Name == name)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }

            GridBoard.Visibility = Visibility.Collapsed;
            GridFilters.Visibility = Visibility.Collapsed;

            switch (name)
            {
                case "Market map":
                    GridBoard.Visibility = Visibility.Visible;
                    break;
                case "Filters":
                    GridFilters.Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Services.Processor.ClosingApp = true;
            Services.binanceSocketClient.UnsubscribeAllAsync();
        }



        private void Symbol_MouseDown(object sender, MouseButtonEventArgs e)
        {
            

            var textBlock = (TextBlock)sender;

            Services.Processor.SelectedSymbol = textBlock.Text;

            var wind = Application.Current.Windows.Cast<Window>().Where(x=> x.Name == "WindOrderBook").FirstOrDefault();

            if (wind == null)
            {
                var orderBookWindow = new OrderBookWindow();
                orderBookWindow.Owner = Application.Current.MainWindow;
                orderBookWindow.Show();
            }
            else
            {
               // (OrderBookWindow)wind.SetSymbol();
            }

        }
    }
}
