namespace BLL.Dto
{
    public class Chisler
    {
        /// <summary>
        /// Id вычислителя
        /// </summary>
        public int ThreadId { get; set; }

        //TODO прикрутить строковое хранение для больших чисел, если будет время
        /// <summary>
        /// Текущее значение
        /// </summary>
        public int Value { get; set; }
    }
}
