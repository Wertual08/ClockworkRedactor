using ExtraForms;
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
    class EventCompiler
    {
        public struct Event
        {
            public ulong MinDelay;
            public ulong MaxDelay;
            public int FirstAction;
            public int ActionsCount;
        }
        public struct Action
        {
            public int Type;
            public int LinkID;
            public int OffsetX;
            public int OffsetY;
        }

        private IDTable Table;
        private MessageQueue LogQueue;
        private List<Event> Events = new List<Event>();
        private List<Action> Actions = new List<Action>();

        public EventCompiler(IDTable table, MessageQueue log_queue)
        {
            Table = table;
            LogQueue = log_queue;
        }

        public void Compile(string path, int id)
        {
            var even = new Event();

            LogQueue.Put($"Compiling [{path}]...");
            EventResource res = null;
            try { res = new EventResource(path); }
            catch
            {
                LogQueue.Put($"Event [{path}] was not found. ID skipped.");
                return;
            }

            even.MinDelay = res.MinDelay;
            even.MaxDelay = res.MaxDelay;
            even.FirstAction = Actions.Count;
            even.ActionsCount = res.Actions.Count;

            foreach (var a in res.Actions)
            {
                var ca = new Action();

                ca.LinkID = Table[a.Tile.Link];
                ca.Type = (int)a.Type;
                ca.OffsetX = a.OffsetX;
                ca.OffsetY = a.OffsetY;

                if (ca.LinkID < 0) LogQueue.Put($"Warning: Link [{a.Tile.Link}] was not found.");

                Actions.Add(ca);
            }

            LogQueue.Put($"Event [{path}] compiled with id [{id}].");
            res.Dispose();

            while (Events.Count <= id) Events.Add(new Event());
            Events[id] = even;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(Events.Count);
            foreach (var e in Events) w.Write(e);
            w.Write(Actions.Count);
            foreach (var a in Actions) w.Write(a);
        }
    }
}
