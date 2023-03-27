using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondLabNet
{
    public class AnimalsNotFoundException : Exception
    {
        public AnimalsNotFoundException() { }
        public AnimalsNotFoundException(string message) : base(message) { }
        public AnimalsNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class NoTwoArgumentsException : Exception
    {
        public NoTwoArgumentsException() { }
        public NoTwoArgumentsException(string message) : base(message) { }
        public NoTwoArgumentsException(string message, Exception inner) : base(message, inner) { }
    }
}
