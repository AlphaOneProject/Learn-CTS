using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    abstract class Character : Texture
    {
        public Character(int x, int y) : base(x, y, false)
        {
            
        }
    }
}
