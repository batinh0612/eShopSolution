using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Utilities.Constant
{
    public class SystemConstant
    {
        public const string MainConnectionString = "eShopSolutionDb";

        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfLastestProducts = 6;
            public const string NA = "N/A";
        }
    }
}
