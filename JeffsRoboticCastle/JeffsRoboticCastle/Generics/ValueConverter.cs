using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// just a getter
namespace Castle
{
    interface ValueConverter<InputType, OutputType>
    {
        OutputType convert(InputType input);
    }
}
