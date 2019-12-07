using Newtonsoft.Json.Linq;
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
        string description;
        JObject actions;

        public Item(int id, int x, int y) : base(x, y)
        {
            this.id = id;
            this.x_pos = x;
            this.y_pos = y;
        }

        public Item(int id, string name, int x, int y, int z) : base(name, "items", x, y, z)
        {
            this.id = id;
            this.x_pos = x;
            this.y_pos = y;
        }

        public int GetID()
        {
            return id;
        }

        /// <summary>
        /// Set the actions of the item
        /// </summary>
        /// <param name="actions">Actions of the item</param>
        public void SetActions(JObject actions)
        {
            this.actions = actions;
        }

        /// <summary>
        /// Returns the actions of the item
        /// </summary>
        /// <returns></returns>
        public JObject GetActions()
        {
            return actions;
        }

        /// <summary>
        /// Returns the description of the item
        /// </summary>
        /// <returns>Description of the item</returns>
        public string GetDescription()
        {
            return description;
        }

        public void SetDescription(string desc)
        {
            this.description = desc;
        }
    }
}
