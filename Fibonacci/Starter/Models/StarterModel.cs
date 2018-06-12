using System;

namespace First.Models
{
    /// <summary>
    /// Модель стартера расчётов
    /// </summary>
    public class StarterModel
    {
        /// <summary>
        /// Кол-во потоков расчёта
        /// </summary>
        public int ThreadsCount { get; set; }

        /// <summary>
        /// Признак запуска расчётов
        /// </summary>
        public bool IsCalcStarted { get; set; }
    }
}