using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace Learn_CTS
{
    class Tools
    {
        /// <summary>
        /// Recover the content of a JSON file at a specified path.
        /// </summary>
        /// <param name="path">Path to the targeted JSON file.</param>
        /// <returns>Content of the JSON file under a JObject structure.</returns>
        public static JObject Get_From_JSON(string path)
        {
            JObject output;
            using (StreamReader stream_r = new StreamReader(@"" + path))
            {
                string json_file = stream_r.ReadToEnd();
                output = JObject.Parse(json_file);
            }
            return output;
        }

        /// <summary>
        /// Set the content of the JSON file at the specified path.
        /// </summary>
        /// <param name="path">Path to the targeted JSON file.</param>
        /// <param name="new_content">JObject containing the variables needed in the file.</param>
        public static void Set_To_JSON(string path, JObject new_content)
        {
            File.WriteAllText(@"" + path, new_content.ToString());

            // Write JSON directly to the specified file.
            using (StreamWriter file = File.CreateText(@"" + path))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                new_content.WriteTo(writer);
            }
        }

        /// <summary>
        /// Stops the render update on a specified Control.
        /// </summary>
        /// <param name="control">Targeted Control.</param>
        public static void Begin_Control_Update(Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, 11, IntPtr.Zero,
                  IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        /// <summary>
        /// Resumes the render update on a specified Control.
        /// </summary>
        /// <param name="control">Targeted Control.</param>
        public static void End_Control_Update(Control control)
        {
            // Create a C "true" boolean as an IntPtr
            IntPtr wparam = new IntPtr(1);
            Message msgResumeUpdate = Message.Create(control.Handle, 11, wparam,
                  IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgResumeUpdate);
            control.Invalidate();
            control.Refresh();
        }

        /// <summary>
        /// Allows the user to input a text and his font size in order to get the final number of pixels it will use, in a textbox for example.
        /// </summary>
        /// <param name="text">Text needing estimation.</param>
        /// <param name="font_size">Font size desired.</param>
        /// <returns>Number of pixels used by the text.</returns>
        public static int Get_Text_Width(Control ctrl, string text, int font_size)
        {
            int width;
            Label lbl_width_measure = new Label()
            {
                Name = "lbl_width_measure",
                Text = text,
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft Sans Serif", font_size, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Visible = false
            };
            ctrl.Controls.Add(lbl_width_measure);
            width = lbl_width_measure.Width;
            ctrl.Controls.Remove(lbl_width_measure);
            return width;
        }

        /// <summary>
        /// Return the minimum between two specified integers.
        /// </summary>
        /// <param name="nbr1">First integer.</param>
        /// <param name="nbr2">Second integer.</param>
        /// <returns>Minimum of both parameters.</returns>
        public static int Min_Int(int nbr1, int nbr2)
        {
            int output = nbr1;
            if (nbr2 < output) { output = nbr2; }
            return output;
        }

        /// <summary>
        /// Return the maximum between two specified integers.
        /// </summary>
        /// <param name="nbr1">First integer.</param>
        /// <param name="nbr2">Second integer.</param>
        /// <returns>Maximum of both parameters.</returns>
        public static int Max_Int(int nbr1, int nbr2)
        {
            int output = nbr1;
            if (nbr2 > output) { output = nbr2; }
            return output;
        }

        /// <summary>
        /// Check if a npc is colliding at a point is colliding with a vehicule.
        /// </summary>
        /// <param name="p1">The position of the npc.</param>
        /// <returns>True if the npc is colliding with the vehicule, false otherwise.</returns>
        public static bool IsCollidingWithVehicule(Vehicule v, Point p1)
        {
            //Texture.InitializePath(game); //todo : préciser le jeu dans le constructeur.
            //Transport v = new Tram(0, 0); //todo : pareil que ci-dessus.
            v.ChangeInside();
            NPC n = new NPC(1, p1.X, p1.Y);
            bool b = v.CollideWith(n, false);
            v.Dispose();
            n.Dispose();
            return b;
        }

        /// <summary>
        /// Changes the opacity of the image according to the opacity value given.
        /// </summary>
        /// <param name="img">Image to modify</param>
        /// <param name="opacityvalue">Opacity value</param>
        /// <returns></returns>
        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            try
            {
                Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
                Graphics graphics = Graphics.FromImage(bmp);
                ColorMatrix colormatrix = new ColorMatrix();
                colormatrix.Matrix33 = opacityvalue;
                ImageAttributes imgAttribute = new ImageAttributes();
                imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
                graphics.Dispose();   // Releasing all resource used by graphics 
                return bmp;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("L'image " + img.ToString() + " est introuvable. Vérifiez qu'elle existe.");
                return null;
            }
        }

        /// <summary>
        /// Copy directory and its content.
        /// </summary>
        /// <param name="source">Source directory to copy</param>
        /// <param name="target">Target directoty to copy source dir. into</param>
        public static void DirectoryCopy(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), false);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                DirectoryCopy(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
