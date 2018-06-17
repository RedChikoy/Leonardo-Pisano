using System;
using System.Collections.Generic;
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface IThreadingService
    {
        /// <summary>
        /// Запустить потоки вычисления Starter
        /// </summary>
        /// <param name="count"></param>
        void StartThreads(int count);

        /// <summary>
        /// Остановить потоки вычисления Starter
        /// </summary>
        void StopThreads();

        /// <summary>
        /// Получить результаты вычисления из контейнера
        /// </summary>
        IEnumerable<Chisler> GetCurrentValues();

        /// <summary>
        /// Запустить вычисление Сontinuer
        /// </summary>
        /// <param name="value"></param>
        void StartСontinuerThread(Chisler value);
    }
}
