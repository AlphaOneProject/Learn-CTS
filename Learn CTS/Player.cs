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

        private int? obj_x;
        private int? obj_y;

        /// <summary>
        /// Constructor of the player
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Player(int x, int y) : base(0, "Player", x, y)
        {
        }

        /// <summary>
        /// Set coordinates as objective
        /// </summary>
        /// <param name="x">The x coordinate of the objective.</param>
        /// <param name="y">The y coordinate of the objective.</param>

        public void GoTo(int x, int y)
        {
            this.obj_x = x - (this.GetX() + this.GetWidth()/2);
            this.obj_y = y - (this.GetY() + this.GetHeight());
        }

        /// <summary>
        /// Get the x coordinate of the objective.
        /// </summary>
        /// <returns>The x coordinate that has to be reached.</returns>

        public int GetObjX()
        {
            return (int)this.obj_x;
        }

        public void UpdateObjX(int x)
        {
            this.obj_x += x;
        }

        public void UpdateObjY(int y)
        {
            this.obj_y += y;
        }

        /// <summary>
        /// Get the y coordinate of the objective.
        /// </summary>
        /// <returns>The y coordinate that has to be reached.</returns>

        public int GetObjY()
        {
            return (int)this.obj_y;
        }

        /// <summary>
        /// Check if the player has an objective.
        /// </summary>
        /// <returns>Return true if he has an objective, false otherwise.</returns>

        public bool HasObjective()
        {
            return (this.obj_x != null && this.obj_y != null);
        }

        /// <summary>
        /// Check if the player has reached his objective.
        /// </summary>
        /// <returns>True if the player has reached his objective.</returns>

        public bool ReachedObjective()
        {
            return (
                this.ReachedObjX() &&
                this.ReachedObjY());
        }

        public bool ReachedObjX()
        {
            return (
                this.obj_x < 8 &&
                this.obj_x > -8);
        }

        public bool ReachedObjY()
        {
            return (
                this.obj_y < 8 &&
                this.obj_y > -8);
        }

        /// <summary>
        /// Remove the player's objective.
        /// </summary>

        public void RemoveObjective()
        {
            this.obj_x = null;
            this.obj_y = null;
        }
    }
}
