using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notions.Core.Utils.Models
{
    public class GMaps_MarkerModel
    {
        public string? id {get; set; }
        public GMaps_CoordenadaModel position { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public bool anda { get; set; }

    }
}
