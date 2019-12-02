using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    /// <summary>
    /// Manager of the items for the point and click game mode.
    /// Works as a singleton.
    /// </summary>
    class ItemManager
    {

        private List<Texture> list_items = new List<Texture>();
        private string game;
        private string situation_path;
        private string library_path;

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemManager(string game, string situation_path)
        {
            this.game = game;
            this.situation_path = situation_path;
            this.library_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + 
                Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + 
                Path.DirectorySeparatorChar + "library";
        }

        /// <summary>
        /// Creates an item and adds it to the list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Item CreateItem(int id, string name, int x, int y, string description, JObject actions)
        {
            Item item = new Item(id, name, x, y, description);
            item.SetActions(actions);
            list_items.Add(item);
            return item;
        }

        /// <summary>
        /// Gets the item with the ID passed in.
        /// </summary>
        /// <param name="id">ID of the item to get</param>
        /// <returns>Item by id.</returns>
        public Item GetItemByID(int id)
        {
            foreach (Item n in list_items)
            {
                if (n.GetID() == id)
                {
                    return n;
                }
            }
            return null;
        }

        /// <summary>
        /// Deletes the item which has the id passed in from the list.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteItemByID(int id)
        {
            for (int i = list_items.Count - 1; i >= 0; i--)
            {
                if (((Item)list_items[i]).GetID() == id)
                {
                    list_items.Remove(list_items[i]);
                }
            }
        }

        /// <summary>
        /// Returns the list of items
        /// </summary>
        /// <returns>List of items</returns>
        public List<Texture> GetList()
        {
            return list_items;
        }

        /// <summary>
        /// Clears the list of items
        /// </summary>
        public void Clear()
        {
            list_items.Clear();
        }

        /// <summary>
        /// Retrieves all the items used for a specific situation and puts them in the list.
        /// </summary>
        /// <param name="situation">JObject of the situation</param>
        public void GetItemsFromSituation()
        {
            JObject situation = Tools.Get_From_JSON(situation_path + Path.DirectorySeparatorChar + "item_test.json");
            for (int i = 1; i <= int.Parse(situation["events"].ToString()); i++)
            {
                string index = i.ToString();
                Item item = new Item(
                        int.Parse(situation[index]["item"]["id"].ToString()),
                        situation[index]["item"]["name"].ToString(),
                        int.Parse(situation[index]["x"].ToString()),
                        int.Parse(situation[index]["x"].ToString()),
                        situation[index]["item"]["description"].ToString()
                    );
                string nb_quizz = situation[index]["quizz"].ToString();
                item.SetActions( (JObject)(Tools.Get_From_JSON(library_path + Path.DirectorySeparatorChar + 
                    "dialogs" + Path.DirectorySeparatorChar + nb_quizz + ".json")) );
                list_items.Add(item);
            }
        }

    }
}
