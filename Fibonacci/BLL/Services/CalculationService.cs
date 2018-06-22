using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class CalculationService: ICalculationService
    {
        public Chisler Calculate(Chisler chisler, CalcRequestEnum сalcRequest)
        {
            return Calculate(chisler.ThreadId, chisler.Value, сalcRequest);
        }

        public Chisler Calculate(int threadId, int newPart, CalcRequestEnum сalcRequest)
        {
            var container = ChislerContainer.GetInstance();
            var currentChisler = container.GetCalcValue(threadId, сalcRequest);
            
            //Расчёт
            var currentValue = currentChisler.Value == 0 ? 1 : currentChisler.Value;
            var newValue = currentValue + newPart;

            //Обновление контейнера результатов
            return container.UpdateCalcValue(threadId, newValue, сalcRequest);
        }
    }
}
