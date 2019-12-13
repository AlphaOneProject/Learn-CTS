using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class Egg : Texture
    {

        // Attributes
        private List<Image> list_eggs;
        private int state = 0;

        /// <summary>
        /// Construct an egg
        /// </summary>
        /// <param name="x">The position of the egg on the x axis.</param>
        /// <param name="y">The position of the egg on the y axis.</param>
        public Egg(int x, int y) : base(x, y)
        {
            list_eggs = new List<Image>();
            string path_eggs = Texture.GetDirImages() + Path.DirectorySeparatorChar + "eggs" + Path.DirectorySeparatorChar;
            for (int i = 0; i < 6; i++)
            {
                list_eggs.Add(Image.FromFile(path_eggs + "egg" + i.ToString() + ".png"));
            }
            this.SetImage(list_eggs[0]);
        }

        /// <summary>
        /// Return the image according to the score.
        /// </summary>
        /// <returns></returns>
        public override Image GetImage()
        {
            return list_eggs[this.state];
        }

        /// <summary>
        /// Set the state of the egg.
        /// </summary>
        /// <param name="d">The state of the egg.</param>
        public void SetState(int state)
        {
            this.state = state;
        }

    }
}
