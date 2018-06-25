using BLL.Dto;

namespace BLL.Interfaces
{
    public interface ITransportService
    {
        void Send(Chisler value);

        Chisler Get(int queueNumber);
    }
}
