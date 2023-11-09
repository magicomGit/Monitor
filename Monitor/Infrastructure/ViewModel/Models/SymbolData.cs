using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Infrastructure.ViewModel.Models
{
    public class SymbolData
    {
        public string Name { get; set; }
        public decimal MinLot { get; set; }
        public decimal MaxLot { get; set; }
        public decimal LotStepSize { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal TickSize { get; set; }
    }
}
