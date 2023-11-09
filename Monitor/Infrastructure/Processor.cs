using Binance.Net.Enums;
using Monitor.Infrastructure.ViewModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor.Infrastructure
{
    public class Processor
    {
        public Processor()
        {
            Task.Run(() => NatrUpdate());
            Task.Run(() => Volume24Update());
        }
        public bool GettingData { get; set; }
        public bool ClosingApp { get; set; }
        public string SelectedSymbol { get; set; } = string.Empty;
        //======================================

        public async void StartBinanceSubscription()
        {

            //order book
            Services.ShowMessage("Subscribe To Order Book Updates..");
            await Services.binanceSocketClient.UsdFuturesApi.SubscribeToOrderBookUpdatesAsync(Services.Symbols, 100, data =>
             {
                 Services.SetOrderBook(data, SelectedSymbol);
             });
            //All book ticker
            await Services.binanceSocketClient.UsdFuturesApi.SubscribeToAllBookTickerUpdatesAsync(data =>
            {
                if (Services.BookTickerDictionary.ContainsKey(data.Data.Symbol))
                {
                    Services.BookTickerDictionary[data.Data.Symbol].Symbol = data.Data.Symbol;
                    Services.BookTickerDictionary[data.Data.Symbol].BestBidPrice = data.Data.BestBidPrice;
                    Services.BookTickerDictionary[data.Data.Symbol].BestBidQuantity = data.Data.BestBidQuantity;
                    Services.BookTickerDictionary[data.Data.Symbol].BestAskPrice = data.Data.BestAskPrice;
                    Services.BookTickerDictionary[data.Data.Symbol].BestAskQuantity = data.Data.BestAskQuantity;
                    Services.BookTickerDictionary[data.Data.Symbol].UpdateId = data.Data.UpdateId;
                }
                else
                {
                    Services.BookTickerDictionary.Add(data.Data.Symbol, new BinanceBookTicker
                    {
                        Symbol = data.Data.Symbol,
                        BestBidPrice = data.Data.BestBidPrice,
                        BestBidQuantity = data.Data.BestBidQuantity,
                        BestAskPrice = data.Data.BestAskPrice,
                        BestAskQuantity = data.Data.BestAskQuantity,
                        UpdateId = data.Data.UpdateId
                    });
                }
            });


            //mini tick last24hour
            await Services.binanceSocketClient.UsdFuturesApi.SubscribeToMiniTickerUpdatesAsync(Services.Symbols, data =>
            {
                if (Services.MiniTicklDictionary.ContainsKey(data.Data.Symbol))
                {
                    Services.MiniTicklDictionary[data.Data.Symbol].OpenPrice = data.Data.OpenPrice;
                    Services.MiniTicklDictionary[data.Data.Symbol].Volume = data.Data.Volume;
                    Services.MiniTicklDictionary[data.Data.Symbol].LowPrice = data.Data.LowPrice;
                    Services.MiniTicklDictionary[data.Data.Symbol].HighPrice = data.Data.HighPrice;
                    Services.MiniTicklDictionary[data.Data.Symbol].LastPrice = data.Data.LastPrice;
                    Services.MiniTicklDictionary[data.Data.Symbol].QuoteVolume = data.Data.QuoteVolume;
                    Services.MiniTicklDictionary[data.Data.Symbol].Symbol = data.Data.Symbol;
                }
                else
                {
                    Services.MiniTicklDictionary.Add(data.Data.Symbol, new BinanceMiniTick
                    {
                        OpenPrice = data.Data.OpenPrice,
                        Volume = data.Data.Volume,
                        LowPrice = data.Data.LowPrice,
                        HighPrice = data.Data.HighPrice,
                        LastPrice = data.Data.LastPrice,
                        QuoteVolume = data.Data.QuoteVolume
                    });
                }

            });



            //kline
            Services.ShowMessage("Subscribe To Kline Updates..");
            foreach (var symbol in Services.Symbols)
            {
                await Services.binanceSocketClient.UsdFuturesApi.SubscribeToKlineUpdatesAsync(symbol, (KlineInterval)Services.TimeFrame, data =>
                {
                    Services.GetAllCandles(Services.TimeFrame, data.Timestamp);
                    Services.UpdateCandles(Services.TimeFrame, data.Timestamp, data.Data.Symbol, data.Data.Data);
                });
            }







        }

        private void NatrUpdate()
        {
            while (true)
            {
                Services.MaxNatrFilter();

                Thread.Sleep(10000);
            }
        }
        private void Volume24Update()
        {
            while (true)
            {
                Services.Max24VolumeFilter();

                Thread.Sleep(10000);
            }
        }
    }
}
