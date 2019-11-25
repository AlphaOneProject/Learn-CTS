using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class ItemManager
    {
        private static ItemManager instance;
        private static List<Texture> list_items;

        public ItemManager()
        {
            list_items = new List<Texture>();
        }

        public static ItemManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ItemManager();
            }
            return instance;
        }

        public Item CreateItem(int id, string name, int x, int y, string description)
        {
            Item item = new Item(id, name, x, y, description);
            list_items.Add(item);
            return item;
        }

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

        public List<Texture> GetList()
        {
            return list_items;
        }

        public void Clear()
        {
            list_items.Clear();
        }

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
