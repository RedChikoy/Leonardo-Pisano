using BLL.Dto;

namespace BLL.Interfaces
{
    public interface ITransportService
    {
        /// <summary>
        /// Получить значение из RabbitMQ по номеру потока
        /// </summary>
        /// <param name="threadNumber"></param>
        /// <returns></returns>
        Chisler GetValueMq(int threadNumber);

        /// <summary>
        /// Отправить значение через RabbitMQ
        /// </summary>
        /// <param name="value"></param>
        void SendValueMq(Chisler value);


        /// <summary>
        /// Отправить сообщение через API
        /// </summary>
        /// <param name="value"></param>
        void SentValueApi(Chisler value);
    }
}
