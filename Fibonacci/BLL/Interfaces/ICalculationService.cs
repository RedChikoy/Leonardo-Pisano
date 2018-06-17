
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface ICalculationService
    {
        Chisler Calculate(Chisler chisler);

        Chisler Calculate(int threadId, int newPart);
    }
}
