using Binance.Net.Interfaces;
using CryptoExchange.Net.CommonObjects;
using Monitor.Infrastructure.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.DTO
{
    public static class CandleDto
    {
        public static Candle Dto(IBinanceStreamKline kline, string symbol)
        {
            var candle = new Candle
            {
                BuyVolume = kline.TakerBuyBaseVolume,
                OpenPrice = kline.OpenPrice,
                HighPrice = kline.HighPrice,
                LowPrice = kline.LowPrice,
                ClosePrice = kline.ClosePrice,
                OpenTime = kline.OpenTime,
                CloseTime = kline.CloseTime,                
                Volume = kline.Volume,                
                Symbol = symbol
            };

            return candle;  
        }
    }
}
