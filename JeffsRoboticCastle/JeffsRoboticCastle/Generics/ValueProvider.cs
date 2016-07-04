using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.Generics
{
    interface ValueProvider<T>
    {
        T Get(T outputType);
    }
}
