using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.WeaponDesign
{
    interface Receivable
    {
        void GiveTo(GamePlayer character);
    }
}
