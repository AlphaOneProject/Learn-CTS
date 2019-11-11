using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public static void Begin_Control_Update(Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, 11, IntPtr.Zero,
                  IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

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

        public static int Min_Int(int nbr1, int nbr2)
        {
            int output = nbr1;
            if (nbr2 < output) { output = nbr2; }
            return output;
        }
    }
}
