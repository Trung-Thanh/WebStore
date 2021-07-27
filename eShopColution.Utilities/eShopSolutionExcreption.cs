using System;
using System.Collections.Generic;
using System.Text;

namespace eShopColution.Utilities
{
    public class eShopSolutionExcreption : Exception
    {
        public eShopSolutionExcreption()
        {
        }

        public eShopSolutionExcreption(string message)
            : base(message)
        {
        }

        public eShopSolutionExcreption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
