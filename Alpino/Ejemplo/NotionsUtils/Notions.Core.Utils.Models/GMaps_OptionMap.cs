using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notions.Core.Utils.Models
{
    public class GMaps_OptionMap
    {
        public Decimal centerLat {get; set; }
        public Decimal centerLong { get; set; }
        public int zoom { get; set; }
        public String key { get; set; }
    }
}
