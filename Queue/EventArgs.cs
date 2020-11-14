using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue
{
    [Serializable]
    public class EventArgs<T> : System.EventArgs
    {
        public T Argument;

        public EventArgs()
            : this(default(T))
        {
        }

        public EventArgs(T argument)
        {
            Argument = argument;
        }
    }
}
