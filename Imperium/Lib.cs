using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imperium
{
    class Lib
    {
        public static bool boolNotify(bool x)
        {
            MessageBox.Show(x ? "Enabled" : "Disabled");
            return x;
        }
    }
}
