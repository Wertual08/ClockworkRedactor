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
    class AnimationCompiler
    {
        private struct Animation
        {
            public int FirstNode;
            public int FrameCount;
            public int NodesPerFrame;
            public int Dependency;
            public float FramesPerUnitRatio;
        }
        private struct Node
        {
            public int Properties;
            public float OffsetX;
            public float OffsetY;
            public float Angle;
        }

        private IDTable Table;
        private MessageQueue LogQueue;

        private TextureCompiler Texture = new TextureCompiler();
        private List<Node> Nodes = new List<Node>();
        private List<Animation> Animations = new List<Animation>();

        public AnimationCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            var animation = new Animation();

            LogQueue.Put($"Compiling [{path}]...");
            AnimationResource res = null;
            try { res = new AnimationResource(path); }
            catch
            {
                LogQueue.Put($"Animation [{path}] was not found. ID skipped.");
                return;
            }

            animation.FirstNode = Nodes.Count;
            animation.FrameCount = res.Count;
            animation.NodesPerFrame = res.NodesCount;
            animation.Dependency = (int)res.Dependency;
            animation.FramesPerUnitRatio = res.FramesPerUnitRatio;

            foreach (var frame in res.Frames)
            {
                for (int n = 0; n < frame.Count; n++)
                {
                    var node = frame[n];
                    var cnode = new Node();
                    cnode.Properties = (int)node.Properties;
                    cnode.OffsetX = node.OffsetX;
                    cnode.OffsetY = node.OffsetY;
                    cnode.Angle = node.Angle;
                    Nodes.Add(cnode);
                }
            }

            LogQueue.Put($"Animations [{path}] compiled with id [{id}].");
            res.Dispose();

            while (Animations.Count <= id) Animations.Add(new Animation());
            Animations[id] = animation;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(Nodes.Count);
            foreach (var n in Nodes) w.Write(n);
            w.Write(Animations.Count);
            foreach (var a in Animations) w.Write(a);
        }
    }
}
