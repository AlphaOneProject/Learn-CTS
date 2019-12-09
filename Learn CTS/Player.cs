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

        private static string name = "Moi";

        private static string folder;

        /// <summary>
        /// Constructor of the player
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Player(int x, int y) : base(0, Player.name, folder, x, y)
        {
            if (player_instance != null) throw new Exception();
            player_instance = this;
        }

        public static Player GetInstance()
        {
            if (player_instance == null) throw new Exception();
            else return player_instance;
        }

        public static void SetName(string name)
        {
            Player.name = name;
        }

        public static void SetFolder(string folder)
        {
            Player.folder = folder;
        }

        public override void Dispose()
        {
            player_instance = null;
            name = "";
            base.Dispose();
        }

        public override void SetObjective(int x, int y)
        {
            this.RemoveAllObjectives();
            base.SetObjective(x, y);
        }
    }
}
