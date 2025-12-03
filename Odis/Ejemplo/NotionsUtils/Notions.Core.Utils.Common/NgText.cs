using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notions.Core.Utils.Common
{
    public class NgText
    {
        public static string CapitalizarPrimeraLetra(string Texto)
        {
            if (string.IsNullOrWhiteSpace(Texto))
                return string.Empty;

            return char.ToUpper(Texto[0]) + Texto.Substring(1).ToLower();
        }
    }
}
