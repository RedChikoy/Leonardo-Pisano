using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BLL.Dto
{
    public class ChislerContainer
    {
        private static readonly object SyncRoot = new object();

        private static ChislerContainer _instance;

        private readonly List<Chisler> _calcValues;
        private ConcurrentDictionary<int, Chisler> _mqBug;

        public CancellationToken Token { get; }
        public CancellationTokenSource TokenSource { get; }

        private ChislerContainer()
        {
            _mqBug = new ConcurrentDictionary<int, Chisler>();
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;

            _calcValues = new List<Chisler>();
        }

        public static ChislerContainer GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncRoot)
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

            lock (SyncRoot)
            {
                if (result == null)
                {
                    result = new Chisler {ThreadId = threadId, Value = 1};
                    _calcValues.Add(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Обновить значение высчисления и вернуть
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public Chisler UpdateCalcValue(int threadId, int newValue)
        {
            var result = _calcValues.SingleOrDefault(ch => ch.ThreadId == threadId);

            lock (SyncRoot)
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

            return result;
        }

        public IEnumerable<Chisler> GetCurentCalcValues()
        {
            return _calcValues;
        }

        public void PutToBug(Chisler value)
        {
            _mqBug.TryAdd(value.ThreadId, value);
        }

        public Chisler GetFromBug(int threadId)
        {
            _mqBug.TryRemove(threadId, out var value);

            return value;
        }
    }
}
