using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dto
{
    /// <summary>
    /// Для сложения больших положительных чисел
    /// Задел на будущее
    /// </summary>
    public class BigNumber
    {
        private byte[] _digits;

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public static BigNumber operator +(BigNumber left, BigNumber right)
        {
            throw new NotImplementedException();
        }

        public static implicit operator string(BigNumber left)
        {
            throw new NotImplementedException();
        }
}
}
