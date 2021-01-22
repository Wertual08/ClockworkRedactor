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
    class EntityCompiler
    {
        private struct Entity
        {
            public int RagdollID;
            public int OutfitID;
            public int FirstTrigger;
            public int TriggersCount;
            public int FirstHolder;
            public int HoldersCount;

            public ulong MaxHealth;
            public ulong MaxEnergy;
            public double Mass;

            public double GravityAcceleration;
            public double JumpVelocity;
            public double DragX;
            public double DragY;
            public double SqrDragX;
            public double SqrDragY;
            public double MoveForceX;
            public double MoveForceY;
        }
        private struct Holder
        {
            public int ActionID;
            public int HolderPoint;
            public int AnimationID;
        }
        private struct Trigger
        {
            public int ActionID;

            public double VelocityXLowBound;
            public double VelocityYLowBound;

            public double VelocityXHighBound;
            public double VelocityYHighBound;

            public double AccelerationXLowBound;
            public double AccelerationYLowBound;

            public double AccelerationXHighBound;
            public double AccelerationYHighBound;

            public int OnGround;
            public int OnRoof;
            public int OnWall;
            public int Direction;

            public int AnimationID;
        }

        private IDTable Table;
        private MessageQueue LogQueue;

        private TextureCompiler Texture = new TextureCompiler();
        private List<Holder> Holders = new List<Holder>();
        private List<Trigger> Triggers = new List<Trigger>();
        private List<Entity> Entities = new List<Entity>();

        public EntityCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            var entity = new Entity();

            LogQueue.Put($"Compiling [{path}]...");
            EntityResource res = null;
            try { res = new EntityResource(path); }
            catch
            {
                LogQueue.Put($"Entity [{path}] was not found. ID skipped.");
                return;
            }

            entity.RagdollID = Table[res.Ragdoll.Link];
            entity.OutfitID = Table[res.Outfit.Link];
            entity.FirstTrigger = Triggers.Count;
            entity.TriggersCount = res.Triggers.Count;
            entity.FirstHolder = Holders.Count;
            entity.HoldersCount = res.Holders.Count;

            entity.MaxHealth = res.MaxHealth;
            entity.MaxEnergy = res.MaxEnergy;
            entity.Mass = res.Mass;
            entity.GravityAcceleration = res.GravityAcceleration;
            entity.JumpVelocity = res.JumpVelocity;
            entity.DragX = res.DragX;
            entity.DragY = res.DragY;
            entity.SqrDragX = res.SqrDragX;
            entity.SqrDragY = res.SqrDragY;
            entity.MoveForceX = res.MoveForceX;
            entity.MoveForceY = res.MoveForceY;

            foreach (var trigger in res.Triggers)
            {
                var ctrigger = new Trigger();

                ctrigger.ActionID = Table["Entity.Trigger", trigger.Action];

                ctrigger.VelocityXLowBound = trigger.VelocityXLowBound;
                ctrigger.VelocityYLowBound = trigger.VelocityYLowBound;

                ctrigger.VelocityXHighBound = trigger.VelocityXHighBound;
                ctrigger.VelocityYHighBound = trigger.VelocityYHighBound;

                ctrigger.AccelerationXLowBound = trigger.AccelerationXLowBound;
                ctrigger.AccelerationYLowBound = trigger.AccelerationYLowBound;

                ctrigger.AccelerationXHighBound = trigger.AccelerationXHighBound;
                ctrigger.AccelerationYHighBound = trigger.AccelerationYHighBound;

                ctrigger.OnGround = trigger.OnGround;
                ctrigger.OnRoof = trigger.OnRoof;
                ctrigger.OnWall = trigger.OnWall;
                ctrigger.Direction = trigger.Direction;

                ctrigger.AnimationID = Table[trigger.Animation.Link];

                Triggers.Add(ctrigger);
            }
            foreach (var holder in res.Holders)
            {
                var cholder = new Holder();

                cholder.ActionID = Table["Entity.Holder", holder.Action];

                cholder.HolderPoint = holder.HolderPoint;

                cholder.AnimationID = Table[holder.Animation.Link];

                Holders.Add(cholder);
            }

            LogQueue.Put($"Entity [{path}] compiled with id [{id}].");
            res.Dispose();


            while (Entities.Count <= id) Entities.Add(new Entity());
            Entities[id] = entity;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(Triggers.Count);
            foreach (var t in Triggers) w.Write(t);
            w.Write(Holders.Count);
            foreach (var h in Holders) w.Write(h);
            w.Write(Entities.Count);
            foreach (var e in Entities) w.Write(e);
        }
    }
}
