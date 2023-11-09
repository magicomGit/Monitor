using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel.Models
{
    public class Candle
    {
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Volume { get; set; }
        public decimal Natr { get; set; }
        public decimal Value2 { get; set; }
        public decimal Value3 { get; set; }
        public decimal BuyVolume { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public string Symbol { get; set; } = string.Empty;
    }
}
