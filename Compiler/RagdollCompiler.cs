using ExtraSharp;
using ResrouceRedactor.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResrouceRedactor.Compiler
{
    class RagdollCompiler
    {
        private struct Ragdoll
        {
            public int FirstNode;
            public int NodesCount;
            public double HitboxW;
            public double HitboxH;
        }
        private struct Node
        {
            public float OffsetX;
            public float OffsetY;
            public int MainNode;
        }

        private IDTable Table;
        private MessageQueue LogQueue;

        private List<Node> Nodes = new List<Node>();
        private List<Ragdoll> Ragdolls = new List<Ragdoll>();

        public RagdollCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            var ragdoll = new Ragdoll();

            LogQueue.Put($"Compiling [{path}]...");
            RagdollResource res = null;
            try { res = new RagdollResource(path); }
            catch
            {
                LogQueue.Put($"Ragdoll [{path}] was not found. ID skipped.");
                return;
            }

            ragdoll.FirstNode = Nodes.Count;
            ragdoll.NodesCount = res.Count;
            ragdoll.HitboxW = res.HitboxW;
            ragdoll.HitboxH = res.HitboxH;

            foreach (var node in res.Nodes)
            {
                var cnode = new Node();
                cnode.MainNode = node.MainNode;
                cnode.OffsetX = node.OffsetX;
                cnode.OffsetY = node.OffsetY;
                Nodes.Add(cnode);
            }

            LogQueue.Put($"Ragdoll [{path}] compiled with id [{id}].");
            res.Dispose();

            while (Ragdolls.Count <= id) Ragdolls.Add(new Ragdoll());
            Ragdolls[id] = ragdoll;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(Nodes.Count);
            foreach (var n in Nodes) w.Write(n);
            w.Write(Ragdolls.Count);
            foreach (var r in Ragdolls) w.Write(r);
        }
    }
}
