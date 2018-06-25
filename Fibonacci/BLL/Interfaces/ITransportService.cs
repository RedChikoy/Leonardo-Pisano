using System.Threading.Tasks;
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface ITransportService
    {
        /// <summary>
        /// Отправить объект
        /// </summary>
        /// <param name="value"></param>
        void Send(Chisler value);

        /// <summary>
        /// Отправить объект асинхронно
        /// </summary>
        /// <param name="value"></param>
        Task SendAsync(Chisler value);

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="queueNumber"></param>
        /// <returns></returns>
        Chisler Get(int queueNumber);

        /// <summary>
        /// Закрыть все канал транспорта
        /// </summary>
        void Close(int queueNumber);
    }
}
