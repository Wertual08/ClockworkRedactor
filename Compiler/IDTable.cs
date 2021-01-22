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

        public Func<string, string> Delimiter { get; set; } = null;
        public Func<string, bool> Validator { get; set; } = null;

        public Dictionary<string, List<Item>> Categories { get; private set; } = new Dictionary<string, List<Item>>();
        public int Count 
        { 
            get 
            {
                int total = 0;
                foreach (var items in Categories)
                    total += items.Value.Count;
                return total;
            } 
        }

        public void Read(StreamReader r)
        {
            try
            {
                string line;
                while ((line = r.ReadLine()) != "{" && line != null) ;
                while ((line = r.ReadLine()) != "}" && line != null)
                {
                    int index = line.IndexOf(':');
                    string id_str = line.Substring(0, index);
                    string path = line.Substring(index + 1).Trim();

                    bool valid = Validator(path);
                    string category = Delimiter(path);

                    if (!Categories.ContainsKey(category))
                        Categories.Add(category, new List<Item>());

                    Categories[category].Add(new Item(int.Parse(id_str), path, valid, true));
                }

                foreach (var items in Categories)
                    items.Value.Sort((Item lhs, Item rhs) => { 
                        return lhs.ID < rhs.ID ? -1 : (lhs.ID > rhs.ID ? 1 : 0); 
                    });
            }
            catch
            {
                throw new Exception("Failed to read ID table." +
                        Environment.NewLine + "Invalid table format.");
            }
        }
        public void Write(StreamWriter w, string[] categories)
        {
            try
            {
                w.WriteLine("{");
                foreach (var c in categories)
                {
                    if (Categories.ContainsKey(c))
                        foreach (var i in Categories[c])
                            w.WriteLine(i.ID + ": " + i.Path);
                }
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
            bool valid = Validator(path);
            string category = Delimiter(path);

            if (!Categories.ContainsKey(category))
                Categories.Add(category, new List<Item>());

            var items = Categories[category];

            int id = 0;
            while (id < items.Count && items[id].ID == id) id++;

            items.Insert(id, new Item(id, path, true, false));
        }
        
        public int GetLastID(string category) { return Categories[category].Last().ID; }
        [IndexerName("MyItems")]
        public int this[string path] 
        { 
            get 
            { 
                foreach (var items in Categories)
                {
                    var item = items.Value.Find((Item i) => i.Path == path);
                    if (item != null) return item.ID;
                }
                return -1; 
            }
        }

        private Dictionary<string, Dictionary<string, int>> CustomValues = new Dictionary<string, Dictionary<string, int>>();
        [IndexerName("MyItems")]
        public int this[string sect, string val]
        {
            get
            {
                if (CustomValues.ContainsKey(sect) && CustomValues[sect].ContainsKey(val))
                    return CustomValues[sect][val];
                else
                {
                    if (!CustomValues.ContainsKey(sect))
                        CustomValues.Add(sect, new Dictionary<string, int>());
                    int res = CustomValues[sect].Count;
                    CustomValues[sect].Add(val, res);
                    return res;
                }
            }
        }
    }
}
