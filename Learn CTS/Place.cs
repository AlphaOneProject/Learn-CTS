using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    /// <summary>
    /// Place used for a scenario.
    /// </summary>
    class Place : Texture
    {
        List<Texture> list_child = new List<Texture>();
        public Place(String nom, int x, int y, int z) : base(nom, x, y, z)
        {
        }

        /// <summary>
        /// Add a child to the place.
        /// </summary>
        /// <param name="t">Texture that will be add as a child.</param>
        public void AddChild(Texture t)
        {
            if (!list_childs.Contains(t))
            {
                this.list_childs.Add(t);
            }
        }
    }
}
