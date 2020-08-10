using Resource_Redactor.Resources.Redactors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Resource_Redactor
{

    public class RedactorProperties
    {
        private static JsonSerializerOptions SerializerOptions_ = null;
        private static JsonSerializerOptions SerializerOptions
        {
            get
            {
                if (SerializerOptions_ != null) return SerializerOptions_;
                var options = new JsonSerializerOptions();
                options.IgnoreReadOnlyProperties = true;
                options.WriteIndented = true;
                options.Converters.Add(new JsonHandleSpecialDoublesAsStrings());
                options.Converters.Add(new JsonHandleSpecialFloatsAsStrings());
                options.Converters.Add(new JsonStringEnumConverter());
                return SerializerOptions_ = options;
            }
        }

        public List<string> OpenedTabs { get; set; } = new List<string>();
        public bool ExplorerVisible { get; set; } = true;
        public bool ExplorerRight { get; set; } = false;
        public ListViewMode ExplorerMode { get; set; } = ListViewMode.MediumIcon;
        public string ExplorerPath { get; set; } = "";
        public int SplitterDistance { get; set; } = 251;
        public int SelectedTab { get; set; } = 0;

        public RedactorProperties() { }
        public RedactorProperties(string path)
        {
            Open(path);
        }
        public void Open(string path)
        {
            try
            {
                var json_text = File.ReadAllText(path);
                using (var json = JsonDocument.Parse(json_text))
                    json.RootElement.PopulateObject(this, GetType(), SerializerOptions);
            }
            catch { }
        }
        public void Save(string path)
        {
            try
            {
                File.WriteAllText(path, JsonSerializer.Serialize(this, GetType(), SerializerOptions));
            }
            catch { }
        }
    }
}
