using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Resource_Redactor.Compiler
{
    public class IDTable
    {
        public class Item
        {
            public int ID { get; private set; }
            public string Path { get; private set; }
            public bool Valid { get; private set; }
            public bool Old { get; private set; }
            public Item(int id, string path, bool valid, bool old)
            {
                ID = id;
                Path = path;
                Valid = valid;
                Old = old;
            }
            public void UpdateID(int id) => ID = id;
        }

        public List<Item> Items { get; private set; } = new List<Item>();

        public void Read(StreamReader r, Func<string, bool> validator)
        {
            try
            {
                while (r.ReadLine() != "{") ;
                string line;
                while ((line = r.ReadLine()) != "}")
                {
                    int index = line.IndexOf(':');
                    string id_str = line.Substring(0, index);
                    string path = line.Substring(index + 1).Trim();
                    Items.Add(new Item(int.Parse(id_str), path, validator(path), true));
                }
                Items.Sort((Item lhs, Item rhs) => { return lhs.ID < rhs.ID ? -1 : (lhs.ID > rhs.ID ? 1 : 0); });
            }
            catch
            {
                throw new Exception("Failed to read ID table." +
                        Environment.NewLine + "Invalid table format.");
            }
        }
        public void Write(StreamWriter w)
        {
            try
            {
                Items.Sort((Item lhs, Item rhs) => { return lhs.ID < rhs.ID ? -1 : (lhs.ID > rhs.ID ? 1 : 0); });
                w.WriteLine("{");
                foreach (var i in Items) w.WriteLine(i.ID + ": " + i.Path);
                w.WriteLine("}");
            }
            catch
            {
                throw new Exception("Failed to write ID table." +
                        Environment.NewLine + "Invalid table format.");
            }
        }

        public void Add(string path)
        {
            if (Items.FindIndex((Item i) => { return i.Path == path; }) >= 0) return;
            int id = 0, ind = -1, t;
            while ((t = Items.FindIndex((Item i) => { return i.ID == id; })) >= 0) { ind = t; id++; }
            Items.Insert(ind + 1, new Item(id, path, true, false));
        }

        public int Count { get { return Items.Count; } }
        public int LastID { get { return Items[Items.Count - 1].ID; } }
        [IndexerName("MyItems")]
        public int this[string path] { get { return Items.Find((Item i) => i.Path == path && i.Valid)?.ID ?? -1; } }
    }
}
