using ResrouceRedactor.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResrouceRedactor.Compiler
{
    class TextureCompiler
    {
        private static string ToHex(uint[] data)
        {
            var builder = new StringBuilder(data.Length * 2 * sizeof(uint));
            foreach (uint v in data)
                builder.Append(v.ToString("0X8"));
            return builder.ToString();
        }

        private List<uint> Data = new List<uint>();
        private Dictionary<string, int> Indexes = new Dictionary<string, int>();

        public int FindLoad(string link, Func<Bitmap, uint[]> converter, uint[] meta = null)
        {
            var name = link;
            if (meta != null) name = "~:/" + ToHex(meta) + "\\:~" + link;

            if (Indexes.ContainsKey(name)) return Indexes[name];

            try
            {
                using (var res = new TextureResource(link))
                {
                    var data = converter(res.Bitmap);

                    int index = Data.Count;
                    if (meta != null) Data.AddRange(meta);
                    if (data != null) Data.AddRange(data);
                    Indexes.Add(name, index);
                    return index;
                }
            }
            catch
            {
                return -1;
            }
        }
        public int FindLoad(string link, TextureResource res, Func<Bitmap, uint[]> converter, uint[] meta = null)
        {
            var name = link;
            if (meta != null) name = "~:/" + ToHex(meta) + "\\:~" + link;

            if (Indexes.ContainsKey(name)) return Indexes[name];

            var data = converter(res.Bitmap);

            int index = Data.Count;
            if (meta != null) Data.AddRange(meta);
            if (data != null) Data.AddRange(data);
            Indexes.Add(name, index);
            return index;
        }

        public void Write(BinaryWriter w)
        {
            w.Write(Data.Count);
            foreach (var d in Data) w.Write(d);
        }
    }
}
