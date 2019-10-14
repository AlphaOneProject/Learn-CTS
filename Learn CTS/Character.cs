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
        /// <summary>
        /// Constructor of character.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Character(int x, int y) : base(x, y, true)
        {
        }

        /// <summary>
        /// Constructor of character with a custom name.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Character(String name, int x, int y) : base(name, x, y)
        {
        }
    }
}
