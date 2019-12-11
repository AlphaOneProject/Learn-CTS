﻿using Newtonsoft.Json.Linq;
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
        /// Constructor of NPC with custom names.
        /// </summary>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public NPC(int id, string name, string folder, int x, int y, int quiz) : base(id, name, folder, x, y)
        {
            interact = new Texture("Interaction", "characters", 0, 0);
            interact.SetX(this.GetX() + this.GetWidth() / 2 - interact.GetWidth() / 2);
            interact.SetY(this.GetY()-interact.GetHeight()-20);
            this.quiz = quiz;
        }

        public NPC(string name, int id, int x, int y) : base(id, name, null, x, y)
        {
            this.quiz = -1;
        }

        public NPC(int id, int x, int y) : this(null, id, x, y)
        {
        }

        public int GetQuiz()
        {
            return this.quiz;
        }

        public void RemoveQuiz()
        {
            this.quiz = -1;
        }

        public bool IsInteractive()
        {
            return this.interactive;
        }

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
