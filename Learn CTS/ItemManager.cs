using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        // Instance of the manager
        private static ItemManager instance;

        // List of the items 
        private static List<Texture> list_items;

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemManager()
        {
            list_items = new List<Texture>();
        }

        /// <summary>
        /// Returns an instance of the manager, according to the singleton pattern.
        /// </summary>
        /// <returns></returns>
        public static ItemManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ItemManager();
            }
            return instance;
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
        public void GetItemsFromSituation(JObject situation)
        {
            for (int i = 1; i <= int.Parse(situation["events"].ToString()); i++)
            {
                String index = i.ToString();
                Item item = new Item(
                        int.Parse(situation[index]["item"]["id"].ToString()),
                        situation[index]["item"]["name"].ToString(),
                        int.Parse(situation[index]["x"].ToString()),
                        int.Parse(situation[index]["x"].ToString()),
                        situation[index]["item"]["description"].ToString()
                    );
                list_items.Add(item);
            }
        }

    }
}
