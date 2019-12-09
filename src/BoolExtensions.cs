using System;

namespace System
{
    public static class BoolExtensions
    {
        static readonly string Yes = "是";

        static readonly string No = "否";

        public static string ToChineseBool(this bool val) {
            return val ? Yes : No; 
        }
    }
}
