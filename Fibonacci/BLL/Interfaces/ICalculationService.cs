
using System;
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface ICalculationService
    {
        Chisler Calculate(Chisler chisler, CalcRequestEnum сalcRequest, Func<int, int, int> calculator);

        Chisler Calculate(int threadId, int newPart, CalcRequestEnum сalcRequest, Func<int, int, int> calculator);
    }
}
