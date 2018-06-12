using BLL.Interfaces;

namespace BLL.Services
{
    public class CalculationService: ICalculationService
    {
        public int Calculate(int prevValue, int currentValue)
        {
            prevValue = prevValue == 0 ? 1 : prevValue;
            currentValue = currentValue + prevValue;

            return currentValue;
        }
    }
}
