using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.Utilities.Exceptions
{
    public class KendoException : Exception
    {
        public Exception OriginalException { get; set; }
        public KendoException(Exception ex) : base(ex.Message, ex)
        {
            OriginalException = ex;
        }
    }
}
