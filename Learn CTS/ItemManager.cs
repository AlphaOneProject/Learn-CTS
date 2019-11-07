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
        private static int item_id = 0;
        private static List<Item> list_items;

        public ItemManager()
        {
            list_items = new List<Item>();
        }

        public static ItemManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ItemManager();
            }
            return instance;
        }

        public Item CreateItem(string name, int x, int y)
        {
            Item item = new Item(ItemManager.item_id, name, x, y);
            ItemManager.item_id++;
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
                if (list_items[i].GetID() == id)
                {
                    list_items.Remove(list_items[i]);
                }
            }
        }

        public List<Item> GetList()
        {
            return list_items;
        }

        public void Clear()
        {
            list_items.Clear();
            item_id = 1;
        }

        public Item GetItemOnPoint(int mx, int my)
        {
            Item res = null;
            foreach (Item i in list_items)
            {
                if (i.GetXBoundaries()[0] < mx && mx < i.GetXBoundaries()[1]
                    && i.GetYBoundaries()[0] < my && my < i.GetYBoundaries()[1])
                {
                    res = i;
                }
            }
            return res;
        }
    }
}
