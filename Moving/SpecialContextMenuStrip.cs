using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moving
{
    class SpecialContextMenuStrip : ContextMenuStrip
    {
        public SpecialContextMenuStrip(IContainer container) : base (container)
        {
        }
        public new Point Location;
    }
}
