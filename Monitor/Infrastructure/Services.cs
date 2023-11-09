using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Futures;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Monitor.Infrastructure.DTO;
using Monitor.Infrastructure.Enums;
using Monitor.Infrastructure.ViewModel;
using Monitor.Infrastructure.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Monitor.Infrastructure
{
    public static class Services
    {
        //------------- Binance----------
        public static BinanceRestClient binanceRestClient = new BinanceRestClient();
        public static BinanceSocketClient binanceSocketClient = new BinanceSocketClient();

        public static WebCallResult<BinanceFuturesUsdtExchangeInfo> SymbolData { get; set; }

        public static Processor Processor = new Processor();

        public static Dictionary<string, Dictionary<int, List<Candle>>> CandleDictionary =
            new Dictionary<string, Dictionary<int, List<Candle>>>();
        public static Dictionary<decimal, OrderBookModel> OrderBookAsks = new Dictionary<decimal, OrderBookModel>();
        public static Dictionary<decimal, OrderBookModel> OrderBookBids = new Dictionary<decimal, OrderBookModel>();
        public static Dictionary<string, SymbolData> SymbolDictionary = new Dictionary<string, SymbolData>();
        public static Dictionary<string, BinanceMiniTick> MiniTicklDictionary = new Dictionary<string, BinanceMiniTick>();
        public static Dictionary<string, BinanceBookTicker> BookTickerDictionary = new Dictionary<string, BinanceBookTicker>();

        public static List<MarketMapFilterModel> NatrSorted = new List<MarketMapFilterModel>();

        public static DateTime NextTF = new DateTime();
        public static int TimeFrame = 300;
        public static int NatrPeriod = 10;
        public static Info Info = new Info();

        //public static IEnumerable<string> AllSymbols { get; set; }
        public static IEnumerable<string> Symbols { get; set; } 

        //=============================================================================
        public static void GetAllCandles(int timeFrame, DateTime time)
        {
            if (NextTF == new DateTime())
            {
                Processor.GettingData = true;
                ShowMessage("GettingData..");
                NextTF = GetTFnextTime(timeFrame, time);

                Task.Run(async () =>
                {
                    foreach (var item in Symbols)
                    {
                        ShowMessage("GettingData Candles " + item + "..");
                        var candles = await GetCandles(item, TimeFrame, 50);
                        if (candles.Count != 0)
                        {
                            CandleDictionary[item][TimeFrame].AddRange(candles);
                            Natr(CandleDictionary[item][TimeFrame]);
                        }

                        Thread.Sleep(20);
                    }

                    //MaxNatrFilter();
                    Processor.GettingData = false;
                    ShowMessage("");
                });
            }


        }


        public static void UpdateCandles(int timeFrame, DateTime time, string symbol, IBinanceStreamKline kline)
        {

            if (Processor.GettingData)
            {
                return;
            }
            //------------------
            if (CandleDictionary[symbol][TimeFrame][^1].CloseTime < kline.CloseTime)
            {
                CandleDictionary[symbol][TimeFrame].Add(CandleDto.Dto(kline, symbol));
                Natr(CandleDictionary[symbol][TimeFrame]);
            }
            else
            {
                CandleDictionary[symbol][TimeFrame][^1].BuyVolume = kline.TakerBuyBaseVolume;
                CandleDictionary[symbol][TimeFrame][^1].Volume = kline.Volume;
                CandleDictionary[symbol][TimeFrame][^1].ClosePrice = kline.ClosePrice;
                CandleDictionary[symbol][TimeFrame][^1].OpenPrice = kline.OpenPrice;
                CandleDictionary[symbol][TimeFrame][^1].HighPrice = kline.HighPrice;
                CandleDictionary[symbol][TimeFrame][^1].LowPrice = kline.LowPrice;
            }
            ShowMessage(time.ToString("HH:mm:ss"));


        }

        public static DateTime GetTFnextTime(int timeFrame, DateTime time)
        {
            DateTime tfTime = new DateTime(time.Year, time.Month, time.Day);
            while (tfTime < time)
            {
                tfTime = tfTime.AddSeconds(timeFrame);
            }
            return tfTime;
        }

        public static void Natr(List<Candle> candles)
        {


            decimal atrPrevios = 0;

            for (int i = 0; i < candles.Count - 1; i++)//без последжней незакрытой свечи
            {
                if (i - 1 < NatrPeriod)
                {
                    continue;
                }

                if (i == NatrPeriod + 1)//первый ATR
                //if (i > -1)//первый ATR
                {
                    decimal trFirst = 0;
                    for (int i_Tr = 1; i_Tr <= NatrPeriod; i_Tr++)
                    {
                        var trFirstList = new List<decimal> {
                        candles[i_Tr].HighPrice - candles[i_Tr].LowPrice,
                        candles[i_Tr].HighPrice - candles[i_Tr - 1].ClosePrice,
                        candles[i_Tr].LowPrice - candles[i_Tr - 1].ClosePrice
                        };

                        trFirst += trFirstList.Max();
                    }

                    atrPrevios = (decimal)1 / NatrPeriod * trFirst;
                    //continue;
                }

                var trList = new List<decimal> {
                    candles[i].HighPrice - candles[i].LowPrice,
                    candles[i].HighPrice - candles[i - 1].ClosePrice,
                    candles[i].LowPrice - candles[i - 1].ClosePrice
                };


                var tr = trList.Max();

                var atr = (atrPrevios * (NatrPeriod - 1) + tr) / NatrPeriod;

                //var atr = (tr * ((double)1 /n)) + (atrPrevios * ((double)(n-1)/n));
                atrPrevios = atr;

                var natr = 100 * (atr / candles[i].ClosePrice);
                candles[i].Natr = natr;
            }
        }


        public async static Task<List<Candle>> GetCandles(string symbol, int timeframe, int candleQty)
        {
            KlineInterval klineInterval = (KlineInterval)timeframe;
            var result = await binanceRestClient.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol, klineInterval, null, null, candleQty);
            if (result.Success)
            {
                var candles = result.Data.Select(x => new Candle
                {
                    OpenPrice = x.OpenPrice,
                    HighPrice = x.HighPrice,
                    LowPrice = x.LowPrice,
                    ClosePrice = x.ClosePrice,
                    OpenTime = x.OpenTime,
                    CloseTime = x.CloseTime,
                    Symbol = symbol,
                    Volume = x.Volume,
                    BuyVolume = x.TakerBuyBaseVolume
                }).ToList();

                return candles;

            }
            else
            {
                MessageBox.Show(symbol + " не удалось загрузить свечи");
                return new List<Candle> { };
            }


        }

        public static void Max24VolumeFilter()
        {
            var volumes = Services.MiniTicklDictionary.Values.OrderByDescending(x => x.Volume).Select(x => new MarketMapFilterModel
            {
                Symbol = x.Symbol,
                Value = x.Volume,
            }).Take(10).ToList();

            App.Current.Dispatcher.Invoke(() =>
            {
                MarketMapFilterVM.Volume.Clear();
                volumes.ForEach(x => MarketMapFilterVM.Volume.Add(x));
            });
        }

        public static void MaxNatrFilter()
        {
            var candles = CandleDictionary.Where(x => x.Value[TimeFrame].Count > 0).Select(s => s.Value[TimeFrame][^2]).ToList();//берем предпоследнюю свечу т к последняя не сформирована

            var NatrSorted = candles.Select(s => new MarketMapFilterModel
            {
                Symbol = s.Symbol,
                Value = Math.Round(s.Natr,4)
            }).OrderByDescending(x => x.Value).Take(10).ToList();

            App.Current.Dispatcher.Invoke(() =>
            {
                MarketMapFilterVM.NATR.Clear();
                NatrSorted.ForEach(x => MarketMapFilterVM.NATR.Add(x));
            });
        }

        public static void ShowMessage(string message)
        {
            if (!Processor.ClosingApp)
            {
                App.Current.Dispatcher.Invoke(() => Info.Message = message);
            }
        }

        public static void SetOrderBook(DataEvent<IBinanceFuturesEventOrderBook> data , string symbol)
        {
            
            if (data.Data.Symbol == symbol && OrderBookVM.OrderBookAsks.Count == 20)
            {
                if (!BookTickerDictionary.ContainsKey(symbol))
                {
                    return;
                }
                var bestAsk = BookTickerDictionary[symbol].BestAskPrice;
                var bestBid = BookTickerDictionary[symbol].BestBidPrice;
                var tickSize = SymbolDictionary[symbol].TickSize;
                //===== Ask =======================================
                foreach (var ask in data.Data.Asks)
                {
                    if (OrderBookAsks.ContainsKey(ask.Price))
                    {
                        OrderBookAsks[ask.Price].Quantity = ask.Quantity;
                        OrderBookAsks[ask.Price].Price = ask.Price;
                        OrderBookAsks[ask.Price].Type = OrderBookType.Ask;
                        OrderBookAsks[ask.Price].Time = data.Data.EventTime;
                    }
                    else
                    {
                        OrderBookAsks.Add(ask.Price, new OrderBookModel { Type = OrderBookType.Ask, Price = ask.Price, Quantity = ask.Quantity , Time = data.Data.EventTime });
                    }
                }

                //---------------

                for (int i = 19; i >= 0; i--)
                {
                    var orederBookPrice = bestAsk + (tickSize * i);
                    OrderBookVM.OrderBookAsks[19 - i].Updated = false;
                    if (OrderBookAsks.TryGetValue(orederBookPrice, out var item))
                    {
                        OrderBookVM.OrderBookAsks[19 - i].Price = item.Price;
                        OrderBookVM.OrderBookAsks[19 - i].Quantity = item.Quantity;
                        OrderBookVM.OrderBookAsks[19 - i].Type = OrderBookType.Ask;
                        if (data.Data.EventTime == item.Time)
                        {
                            OrderBookVM.OrderBookAsks[19 - i].Updated = true;
                        }
                    }
                    

                }

                //===== Bid =======================================
                foreach (var bid in data.Data.Bids)
                {
                    if (OrderBookBids.ContainsKey(bid.Price))
                    {
                        OrderBookBids[bid.Price].Quantity = bid.Quantity;
                        OrderBookBids[bid.Price].Price = bid.Price;
                        OrderBookBids[bid.Price].Type = OrderBookType.Bid;
                        OrderBookBids[bid.Price].Time = data.Data.EventTime;
                    }
                    else
                    {
                        OrderBookBids.Add(bid.Price, new OrderBookModel { Type = OrderBookType.Bid, Price = bid.Price, Quantity = bid.Quantity, Time = data.Data.EventTime });
                    }
                }


                for (int i = 19; i >= 0; i--)
                {
                    var orederBookPrice = bestBid - (tickSize * i);
                    OrderBookVM.OrderBookBids[i].Updated = false;
                    if (OrderBookBids.TryGetValue(orederBookPrice, out var item))
                    {
                        OrderBookVM.OrderBookBids[ i].Price = item.Price;
                        OrderBookVM.OrderBookBids[ i].Quantity = item.Quantity;
                        OrderBookVM.OrderBookBids[ i].Type = OrderBookType.Bid;
                        if (data.Data.EventTime == item.Time)
                        {
                            OrderBookVM.OrderBookBids[i].Updated = true;
                        }
                    }

                }
                

            }
        }

        public async static Task GetSymbolsData()
        {

            if (SymbolData.Success)
            {
                foreach (var item in SymbolData.Data.Symbols)
                {
                    SymbolDictionary.Add(item.Name, new SymbolData
                    {
                        Name = item.Name,
                        LotStepSize = item.LotSizeFilter.StepSize,
                        MaxLot = item.LotSizeFilter.MaxQuantity,
                        MinLot = item.LotSizeFilter.MinQuantity,
                        MaxPrice = item.PriceFilter.MaxPrice,
                        MinPrice = item.PriceFilter.MinPrice,
                        TickSize = item.PriceFilter.TickSize
                    });
                }
                //JsonDataServices.SaveSymbolDataAsync(MarketData.BinanceSymbolData);
            }
            else
            {

            }

        }

        
    }
}
