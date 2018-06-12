using System;
using System.Collections.Generic;
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface IThreadingService
    {
        void StartThreads(int count);

        void StopThreads();

        IEnumerable<Chisler> GetCurrentValues();
    }
}
