using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table.control.Exceptii
{
    class MutareInvalida : Exception
    {
        public MutareInvalida(string message) : base(message) { }

    }
}
