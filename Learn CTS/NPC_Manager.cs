using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class NPC_Manager
    {
        private static NPC_Manager instance;

        private static int npc_id = 1;

        private static List<NPC> list_npcs = new List<NPC>();

        private NPC_Manager()
        {

        }

        public static NPC_Manager GetInstance()
        {
            if(instance == null)
            {
                instance = new NPC_Manager();
            }
            return instance;
        }

        public NPC CreateNPC(string name, int x, int y, int quiz, string folder, bool b)
        {
            NPC npc = new NPC(NPC_Manager.npc_id, name, folder, b, x, y, quiz);
            NPC_Manager.npc_id++;
            list_npcs.Add(npc);
            return npc;
        }

        public NPC CreateNPC(int x, int y, bool b)
        {
            NPC npc = new NPC(NPC_Manager.npc_id, b, x, y);
            NPC_Manager.npc_id++;
            list_npcs.Add(npc);
            return npc;
        }

        public NPC GetNPCByID(int id)
        {
            foreach(NPC n in list_npcs)
            {
                if(n.GetID() == id)
                {
                    return n;
                }
            }
            return null;
        }

        public void DeleteNPCByID(int id)
        {
            for(int i = list_npcs.Count-1; i >= 0; i--)
            {
                if(list_npcs[i].GetID() == id)
                {
                    list_npcs.Remove(list_npcs[i]);
                }
            }
        }

        public List<NPC> GetList()
        {
            return list_npcs;
        }

        public void Clear()
        {
            list_npcs.Clear();
            npc_id = 1;
        }
    }
}
