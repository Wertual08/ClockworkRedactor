using ExtraSharp;
using Resource_Redactor.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Compiler
{
    class OutfitCompiler
    {
        private struct Outfit
        {
            public int FirstNode;
            public int NodesCount;
        }
        private struct Node
        {
            public int SpriteID;
            public int RagdollNodeIndex;
            public int ClotheType;
        }

        private IDTable Table;
        private MessageQueue LogQueue;

        private List<Node> Nodes = new List<Node>();
        private List<Outfit> Outfits = new List<Outfit>();

        public OutfitCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            var outfit = new Outfit();

            LogQueue.Put($"Compiling [{path}]...");
            OutfitResource res = null;
            try { res = new OutfitResource(path); }
            catch
            {
                LogQueue.Put($"Outfit [{path}] was not found. ID skipped.");
                return;
            }

            outfit.FirstNode = Nodes.Count;
            outfit.NodesCount = res.Nodes.Count;

            foreach (var node in res.Nodes)
            {
                var cnode = new Node();

                cnode.SpriteID = Table[node.Sprite.Link];
                cnode.RagdollNodeIndex = node.RagdollNode;
                cnode.ClotheType = (int)node.ClotheType;

                Nodes.Add(cnode);
            }
            res.Dispose();

            LogQueue.Put($"Outfit [{path}] compiled with id [{id}].");

            while (Outfits.Count <= id) Outfits.Add(new Outfit());
            Outfits[id] = outfit;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(Nodes.Count);
            foreach (var n in Nodes) w.Write(n);
            w.Write(Outfits.Count);
            foreach (var o in Outfits) w.Write(o);
        }
    }
}
