
using System;

namespace BLL.Helpers
{
    public static class FormulaHelper
    {
        public static Func<int, int, int> SumIntCalculator()
        {
            return (left, right) => left + right;
        }

        public static Func<float, float, float> SumFloatCalculator()
        {
            return (left, right) => left + right;
        }
    }
}
