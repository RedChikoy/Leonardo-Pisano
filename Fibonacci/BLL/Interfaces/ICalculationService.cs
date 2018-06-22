
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface ICalculationService
    {
        Chisler Calculate(Chisler chisler, CalcRequestEnum сalcRequest);

        Chisler Calculate(int threadId, int newPart, CalcRequestEnum сalcRequest);
    }
}
