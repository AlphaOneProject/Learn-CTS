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
        private int id;

        /// <summary>
        /// Constructor of NPC with custom names.
        /// </summary>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public NPC(int id, string name, string folder, bool b, int x, int y, int quiz) : base(id, name, folder, b, x, y)
        {
            this.quiz = quiz;
        }

        public int GetQuiz()
        {
            return this.quiz;
        }
    }
}
