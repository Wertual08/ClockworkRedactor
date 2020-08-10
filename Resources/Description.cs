using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor.Resources
{
    public class Description
    {
        private static readonly string Stamp = "CLOCKWORK_ENGINE_REDACTOR_DESCRIPTION_BASE";
        public static void WriteSignature(BinaryWriter w)
        {
            w.Write(0x00f00fff);
            w.Write(Stamp.ToCharArray());
            w.Write(0xff0ff000);
        }
        public static bool ReadSignature(BinaryReader r)
        {
            var astamp = Stamp.ToCharArray();
            long length = astamp.Length + 8L + 4L;
            if (r.BaseStream.Length - r.BaseStream.Position < length) return false;
            if (r.ReadUInt32() != 0x00f00fff) return false;
            foreach (var c in astamp) if (c != r.ReadChar()) return false;
            if (r.ReadUInt32() != 0xff0ff000) return false;
            return true;
        }

        public static readonly string RedactorVersion = "0.1.0.1";
        public static readonly string CurrentVersion = "0.0.0.1";
        public static readonly string Extension = "cedp";

        public string Name { get; private set; }

        public static void Create(string path, string name)
        {
            path = Path.Combine(path, name);
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(Path.Combine(path, "Resources"));
            Directory.CreateDirectory(Path.Combine(path, "Recycle"));
            Directory.CreateDirectory(Path.Combine(path, "Backups"));
            Directory.CreateDirectory(Path.Combine(path, "Compilation"));
            using (var writer = new BinaryWriter(File.Create(
                Path.Combine(path, name + "." + Extension))))
            {
                WriteSignature(writer);
                writer.Write(CurrentVersion); 
                writer.Write(name);
            }
        }
        public static string CheckVersion(string path)
        {
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                if (!ReadSignature(reader)) throw new Exception("Invalid description base file format.");
                var version = reader.ReadString();
                if (version.Length != "_._._._".Length) return "_._._._";
                if (!char.IsDigit(version[0]) || version[1] != '.' || !char.IsDigit(version[2]) || version[3] != '.' || 
                    !char.IsDigit(version[4]) || version[5] != '.' || !char.IsDigit(version[6])) return "_._._._";
                return version;
            }
        }

        public Description(string path)
        {
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                if (!ReadSignature(reader)) throw new Exception("Invalid description base file format.");
                Name = reader.ReadString();
            }
        }
    }
}
