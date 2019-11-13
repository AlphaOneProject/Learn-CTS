using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Player : Character
    {

        /// <summary>
        /// Constructor of the player
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Player(String name, int x, int y) : base(0, name, "1", x, y)
        {
        }

        public override void SetObjective(int x, int y)
        {
            this.RemoveAllObjectives();
            base.SetObjective(x, y);
        }
    }
}
