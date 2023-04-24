using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHomework.Exceptions
{
    [Serializable]
    public class UserFetchException: Exception
    {
        public UserFetchException() { }
        public UserFetchException(string message) : base(message) { }
        public UserFetchException(string message, Exception inner) : base(message, inner) { }
    }
}
