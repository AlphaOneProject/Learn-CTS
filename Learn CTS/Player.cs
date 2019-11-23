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

        private static Player player_instance = null;

        /// <summary>
        /// Constructor of the player
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        private Player(String name, int x, int y) : base(0, name, "1", x, y)
        {
        }

        public static Player GetInstance()
        {
            if (player_instance == null) throw new Exception();
            else return player_instance;
        }

        public static Player Construct(String name, int x, int y)
        {
            if (player_instance != null) throw new Exception();
            player_instance = new Player(name, x, y);
            return player_instance;
        }

        public override void Dispose()
        {
            player_instance = null;
            base.Dispose();
        }

        public override void SetObjective(int x, int y)
        {
            this.RemoveAllObjectives();
            base.SetObjective(x, y);
        }
    }
}
