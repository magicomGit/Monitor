using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel.Models
{
    public class BinanceBookTicker
    {
      
        public long UpdateId { get; set; }       
      
        public string Symbol { get; set; } = string.Empty;
       
        public decimal BestBidPrice { get; set; }
       
        public decimal BestBidQuantity { get; set; }
       
        public decimal BestAskPrice { get; set; }
        
        public decimal BestAskQuantity { get; set; }
    }
}
