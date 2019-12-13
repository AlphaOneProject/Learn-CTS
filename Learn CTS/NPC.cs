using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class NPC : Character
    {
        // Attributes

        private int quiz;
        private Texture interact;
        private bool interactive = false;

        /// <summary>
        /// Constructor of interactive NPC.
        /// </summary>
        /// <param name="id">ID of the NPC</param>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="folder">The character folder of the npc.</param>
        /// <param name = "x" > The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="quiz">The quiz of the npc.</param>
        public NPC(int id, string name, string folder, int x, int y, int quiz) : base(id, name, folder, x, y)
        {
            interact = new Texture("interaction_npc", "characters", 0, 0);
            interact.SetX(this.GetX() + this.GetWidth() / 2 - interact.GetWidth() / 2);
            interact.SetY(this.GetY()-interact.GetHeight()-20);
            this.quiz = quiz;
        }

        /// <summary>
        /// Constructor of NPC
        /// </summary>
        /// <param name="id">ID of the NPC</param>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="x" > The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public NPC(string name, int id, int x, int y) : base(id, name, null, x, y)
        {
            this.quiz = -1;
        }

        /// <summary>
        /// Constructor of NPC
        /// </summary>
        /// <param name="id">ID of the NPC</param>
        /// <param name="x" > The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public NPC(int id, int x, int y) : this(null, id, x, y)
        {
        }

        /// <summary>
        /// Get the quiz of the NPC.
        /// </summary>
        /// <returns></returns>
        public int GetQuiz()
        {
            return this.quiz;
        }

        /// <summary>
        /// Remove the quiz of the NPC.
        /// </summary>
        public void RemoveQuiz()
        {
            this.quiz = -1;
        }

        /// <summary>
        /// Check if the NPC has the interactive icon.
        /// </summary>
        /// <returns>true if the NPC has the interactive icon, false otherwise.</returns>
        public bool IsInteractive()
        {
            return this.interactive;
        }

        /// <summary>
        /// Display the interaction icon.
        /// </summary>
        public void DisplayInteraction()
        {
            if (this.quiz > 0)
            {
                interactive = true;
                interact.SetX(this.GetX() + this.GetWidth() / 2 - interact.GetWidth() / 2);
                interact.SetY(this.GetY() - interact.GetHeight() - 20);
                this.AddChild(interact);
            }
        }

        /// <summary>
        /// Remove the interaction icon.
        /// </summary>
        public void RemoveInteraction()
        {
            if (this.quiz > 0)
            {
                interactive = false;
                this.RemoveChild(interact);
            }
        }
    }
}
