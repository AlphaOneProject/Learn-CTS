using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class NPC : Character
    {

        /// <summary>
        /// Constructor of NPC
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public NPC(int x, int y) : base(x, y)
        {
        }

        /// <summary>
        /// Constructor of NPC with custom names.
        /// </summary>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public NPC(String name, int x, int y) : base(name, x, y)
        {
        }
    }
}
