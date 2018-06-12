using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BLL.Dto
{
    public class ChislerContainer
    {
        private static object syncRoot = new object();

        private static ChislerContainer _instance;

        private List<Chisler> _calcValues;

        public CancellationToken Token { get; }
        public CancellationTokenSource TokenSource { get; }

        private ChislerContainer()
        {
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;

            _calcValues = new List<Chisler>();
        }

        public static ChislerContainer GetInstance()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new ChislerContainer();
                    }
                }
            }
            return _instance;
        }

        public Chisler GetCalcValue(int threadId)
        {
            var result = _calcValues.SingleOrDefault(ch => ch.ThreadId == threadId);

            lock (syncRoot)
            {
                if (result == null)
                {
                    result = new Chisler {ThreadId = threadId, Value = 1};
                    _calcValues.Add(result);
                }
            }

            return result;
        }

        public void UpdateCalcValue(int threadId, int newValue)
        {
            var result = _calcValues.SingleOrDefault(ch => ch.ThreadId == threadId);

            lock (syncRoot)
            {
                if (result == null)
                {
                    result = new Chisler { ThreadId = threadId, Value = newValue };
                    _calcValues.Add(result);
                }
                else
                {
                    result.Value = newValue;
                }
            }
        }

        public IEnumerable<Chisler> GetCurentCalcValues()
        {
            return _calcValues;
        }
    }
}
