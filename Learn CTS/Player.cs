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

        // Attributes
        private static Player player_instance = null;
        private static string name = "";
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

        /// <summary>
        /// Get the instance of the player.
        /// </summary>
        /// <returns></returns>
        public static Player GetInstance()
        {
            if (player_instance == null) throw new Exception();
            else return player_instance;
        }

        /// <summary>
        /// Set the name of the player.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        public static void SetName(string name)
        {
            Player.name = name;
        }

        /// <summary>
        /// Set the folder of the player.
        /// </summary>
        /// <param name="folder">The folder used by the player.</param>
        public static void SetFolder(string folder)
        {
            Player.folder = folder;
        }

        /// <summary>
        /// Dispose the player.
        /// </summary>
        public override void Dispose()
        {
            player_instance = null;
            name = "";
            base.Dispose();
        }

        /// <summary>
        /// Set the current objective of the player.
        /// </summary>
        /// <param name="x">The x coordinate of the objective.</param>
        /// <param name="y">The y coordinate of the objective.</param>
        public override void SetObjective(int x, int y)
        {
            this.RemoveAllObjectives();
            base.SetObjective(x, y);
        }
    }
}
