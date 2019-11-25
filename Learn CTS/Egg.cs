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
        private List<Image> list_eggs;
        private int d = 0;

        public Egg(int x, int y) : base(x, y)
        {
            list_eggs = new List<Image>();
            string path_eggs = Texture.GetDirImages() + Path.DirectorySeparatorChar + "others" + Path.DirectorySeparatorChar + "eggs" + Path.DirectorySeparatorChar;
            for (int i = 0; i < 6; i++)
            {
                list_eggs.Add(Image.FromFile(path_eggs + "egg" + i.ToString() + ".png"));
            }
            this.SetImage(list_eggs[0]);
        }

        public override Image GetImage()
        {
            return list_eggs[d];
        }

        public void SetD(int d)
        {
            this.d = d;
        }

    }
}
