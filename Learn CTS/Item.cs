using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class Item : Texture
    {
        int id;
        int x_pos;
        int y_pos;
        String description;

        public Item(int id, String name, int x, int y) : base(name, x, y)
        {
            this.id = id;
            this.x_pos = x;
            this.y_pos = y;
            this.description = "salut c'est "+ name;
        }

        public Item(int id, String name, int x, int y, int z) : base(name, x, y, z)
        {
            this.id = id;
            this.x_pos = x;
            this.y_pos = y;
            this.description = "salut c'est " + name;
        }

        public int GetID()
        {
            return id;
        }

        public String GetDescription()
        {
            return description;
        }

        /// <summary>
        /// Returns the X boundaries of the Item :
        /// [0] = Position, 
        /// [1] = Width relative to position.
        /// </summary>
        /// <returns></returns>
        public int[] GetXBoundaries()
        {
            int[] boundaries = new int[2];
            boundaries[0] = this.x_pos;
            boundaries[1] = this.x_pos + this.GetWidth();
            return boundaries;
        }

        /// <summary>
        /// Returns the Y boundaries of the Item :
        /// [0] = Position, 
        /// [1] = Width relative to position.
        /// </summary>
        /// <returns></returns>
        public int[] GetYBoundaries()
        {
            int[] boundaries = new int[2];
            boundaries[0] = this.y_pos;
            boundaries[1] = this.y_pos + this.GetHeight();
            return boundaries;
        }
    }
}
