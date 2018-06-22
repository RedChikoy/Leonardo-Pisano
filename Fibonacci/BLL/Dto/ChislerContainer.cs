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

        private readonly List<Chisler> _starterValues;
        private readonly List<Chisler> _continuerValues;
        private readonly ConcurrentDictionary<int, Chisler> _mqBug;

        public CancellationToken Token { get; private set; }
        public CancellationTokenSource TokenSource { get; private set; }

        private ChislerContainer()
        {
            _mqBug = new ConcurrentDictionary<int, Chisler>();
            _starterValues = new List<Chisler>();
            _continuerValues = new List<Chisler>();
            ResetToken();
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

        public Chisler GetCalcValue(int threadId, CalcRequestEnum сalcRequest)
        {
            Chisler result;

            lock (SyncRoot)
            {
                result = GetСalcValues(сalcRequest).SingleOrDefault(ch => ch.ThreadId == threadId);
                if (result == null)
                {
                    result = new Chisler {ThreadId = threadId, Value = 1};
                    GetСalcValues(сalcRequest).Add(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Обновить значение высчисления и вернуть
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="newValue"></param>
        /// <param name="сalcRequest"></param>
        /// <returns></returns>
        public Chisler UpdateCalcValue(int threadId, int newValue, CalcRequestEnum сalcRequest)
        {
            Chisler result;

            lock (SyncRoot)
            {
                result = GetСalcValues(сalcRequest).SingleOrDefault(ch => ch.ThreadId == threadId);
                if (result == null)
                {
                    result = new Chisler { ThreadId = threadId, Value = newValue };
                    GetСalcValues(сalcRequest).Add(result);
                }
                else
                {
                    result.Value = newValue;
                }
            }

            return result;
        }

        public IEnumerable<Chisler> GetCurentCalcValues(CalcRequestEnum сalcRequest)
        {
            return GetСalcValues(сalcRequest);
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

        public void ClearBug()
        {
            _mqBug.Clear();
        }

        public void ResetToken()
        {
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
        }

        public void Reset()
        {
            _starterValues.Clear();
            _continuerValues.Clear();
            ClearBug();
            ResetToken();
        }

        private List<Chisler> GetСalcValues(CalcRequestEnum сalcRequest)
        {
            return сalcRequest == CalcRequestEnum.Starter
                ? _starterValues
                : _continuerValues;
        }
    }
}
