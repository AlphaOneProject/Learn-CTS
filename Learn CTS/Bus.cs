using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class Bus : Vehicule
    {
        private static int[] pos_doors = new int[]{
            160,
            664,
            1256
        };

        /*private static int[] pos_textures_doors = new int[]
        {
            88,
            56,
            152
        };*/

        /// <summary>
        /// Constructor of a tram.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Bus(int x, int y) : base("bus", x, y, pos_doors)
        {
        }
    }
}
