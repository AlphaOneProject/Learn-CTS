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
            510,
            920,
            1528,
            1944,
            2552,
            2928
        };

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
