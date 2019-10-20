using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class NPC : Character
    {
        // Attributes

        private int id;

        /// <summary>
        /// Constructor of NPC
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public NPC(int id, int x, int y) : base(x, y)
        {
            this.id = id;
        }

        /// <summary>
        /// Constructor of NPC with custom names.
        /// </summary>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public NPC(int id, String name, int x, int y) : base(name, x, y)
        {
            this.id = id;
        }

        public int GetID()
        {
            return this.id;
        }
    }
}
