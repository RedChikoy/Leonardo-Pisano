using System;

namespace Сontinuer.Models
{
    /// <summary>
    /// Модель повторителя
    /// </summary>
    public class СontinuerModel
    {
        /// <summary>
        /// Признак запуска расчётов.
        /// Изменяется только через API при получении команд о расчётах из Starter'a
        /// </summary>
        public bool IsCalcStarted { get; set; }
    }
}