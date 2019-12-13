using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class NPC_Manager
    {

        // Attributes
        private static NPC_Manager instance;
        private static int npc_id = 1;
        private static List<NPC> list_npcs = new List<NPC>();

        /// <summary>
        /// Constructor of NPC_Manager
        /// </summary>
        private NPC_Manager()
        {

        }

        /// <summary>
        /// Get the instance of NPC_Manager
        /// </summary>
        /// <returns></returns>
        public static NPC_Manager GetInstance()
        {
            if(instance == null)
            {
                instance = new NPC_Manager();
            }
            return instance;
        }

        /// <summary>
        /// Create a NPC.
        /// </summary>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="folder">The character folder of the npc.</param>
        /// <param name="x" > The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="quiz">The quiz of the npc.</param>
        /// <returns>The npc created.</returns>
        public NPC CreateNPC(string name, int x, int y, int quiz, string folder)
        {
            NPC npc = new NPC(NPC_Manager.npc_id, name, folder, x, y, quiz);
            NPC_Manager.npc_id++;
            list_npcs.Add(npc);
            return npc;
        }

        /// <summary>
        /// Create a NPC.
        /// </summary>
        /// <param name="x" > The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The npc created.</returns>
        public NPC CreateNPC(int x, int y)
        {
            NPC npc = new NPC(NPC_Manager.npc_id, x, y);
            NPC_Manager.npc_id++;
            list_npcs.Add(npc);
            return npc;
        }

        /// <summary>
        /// Create a NPC.
        /// </summary>
        /// <param name="name">Name of the NPC</param>
        /// <param name="x" > The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The npc created.</returns>
        public NPC CreateNPC(string name, int x, int y)
        {
            NPC npc = new NPC(name, NPC_Manager.npc_id, x, y);
            NPC_Manager.npc_id++;
            list_npcs.Add(npc);
            return npc;
        }

        /// <summary>
        /// Get the NPC according to its id.
        /// </summary>
        /// <param name="id">ID of the NPC.</param>
        /// <returns>The NPC if found, null otherwise.</returns>
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

        /// <summary>
        /// Remove a NPC by its id.
        /// </summary>
        /// <param name="id">The id of the npc.</param>
        public void RemoveNPC(int id)
        {
            RemoveNPC(GetNPCByID(id));
        }

        /// <summary>
        /// Remove a NPC.
        /// </summary>
        /// <param name="n">The npc that will be removed.</param>
        public void RemoveNPC(NPC n)
        {
            n.Dispose();
            list_npcs.Remove(n);
        }

        /// <summary>
        /// Get the list of npcs.
        /// </summary>
        /// <returns>The list of npcs.</returns>
        public List<NPC> GetList()
        {
            return list_npcs;
        }

        /// <summary>
        /// Clear and dispose the list of the npcs, and reset the id.
        /// </summary>
        public void Clear()
        {
            foreach (NPC n in list_npcs) n.Dispose();
            list_npcs.Clear();
            npc_id = 1;
        }

        /// <summary>
        /// Make all the npcs which have quiz to display the interaction icon.
        /// </summary>
        public void MakeAllNPCsInteractives()
        {
            foreach (NPC n in list_npcs)
            {
                n.DisplayInteraction();
            }
        }

        /// <summary>
        /// Make all the npcs which have quiz to remove the interaction icon.
        /// </summary>
        public void MakeAllNPCsNotInteractives()
        {
            foreach (NPC n in list_npcs)
            {
                n.RemoveInteraction();
            }
        }
    }
}
