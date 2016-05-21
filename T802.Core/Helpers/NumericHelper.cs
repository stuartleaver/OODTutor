using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T802.Core.Helpers
{
    public class NumericHelper
    {
        /// <summary>
        /// Calculate if number is odd or even
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Boolean</returns>
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}
