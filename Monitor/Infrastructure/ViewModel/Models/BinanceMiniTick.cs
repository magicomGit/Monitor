using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel.Models
{
    public class BinanceMiniTick
    {
        public string Symbol { get; set; } = string.Empty; 

        public decimal LastPrice { get; set; }


        public decimal OpenPrice { get; set; }

        public decimal HighPrice { get; set; }

        
        public decimal LowPrice { get; set; }


        public decimal Volume { get; set; }

       
        public decimal QuoteVolume { get; set; }
    }
}
