using ResrouceRedactor.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ResrouceRedactor
{
    public static class JsonElementExt
    {
        public static long TryFind(this JsonElement element, string name, long def)
        {
            JsonElement property;
            long result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt64(out result)) return result;
            else return def;
        }
        public static int TryFind(this JsonElement element, string name, int def)
        {
            JsonElement property;
            int result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt32(out result)) return result;
            else return def;
        }
        public static short TryFind(this JsonElement element, string name, short def)
        {
            JsonElement property;
            short result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt16(out result)) return result;
            else return def;
        }
        public static sbyte TryFind(this JsonElement element, string name, sbyte def)
        {
            JsonElement property;
            sbyte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSByte(out result)) return result;
            else return def;
        }
        public static ulong TryFind(this JsonElement element, string name, ulong def)
        {
            JsonElement property;
            ulong result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt64(out result)) return result;
            else return def;
        }
        public static uint TryFind(this JsonElement element, string name, uint def)
        {
            JsonElement property;
            uint result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt32(out result)) return result;
            else return def;
        }
        public static ushort TryFind(this JsonElement element, string name, ushort def)
        {
            JsonElement property;
            ushort result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt16(out result)) return result;
            else return def;
        }
        public static byte TryFind(this JsonElement element, string name, byte def)
        {
            JsonElement property;
            byte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetByte(out result)) return result;
            else return def;
        }
        public static float TryFind(this JsonElement element, string name, float def)
        {
            JsonElement property;
            float result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSingle(out result)) return result;
            else return def;
        }
        public static double TryFind(this JsonElement element, string name, double def)
        {
            JsonElement property;
            double result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDouble(out result)) return result;
            else return def;
        }
        public static DateTime TryFind(this JsonElement element, string name, DateTime def)
        {
            JsonElement property;
            DateTime result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTime(out result)) return result;
            else return def;
        }
        public static DateTimeOffset TryFind(this JsonElement element, string name, DateTimeOffset def)
        {
            JsonElement property;
            DateTimeOffset result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTimeOffset(out result)) return result;
            else return def;
        }
        public static decimal TryFind(this JsonElement element, string name, decimal def)
        {
            JsonElement property;
            decimal result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDecimal(out result)) return result;
            else return def;
        }
        public static Guid TryFind(this JsonElement element, string name, Guid def)
        {
            JsonElement property;
            Guid result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetGuid(out result)) return result;
            else return def;
        }
        public static string TryFind(this JsonElement element, string name, string def)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property)) return property.GetString();
            else return def;
        }
        public static bool TryFind(this JsonElement element, string name, bool def)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property)) return property.GetBoolean();
            else return def;
        }

        public static bool TryFind(this JsonElement element, string name, ref long val)
        {
            JsonElement property;
            long result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt64(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref int val)
        {
            JsonElement property;
            int result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt32(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref short val)
        {
            JsonElement property;
            short result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetInt16(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref sbyte val)
        {
            JsonElement property;
            sbyte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSByte(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref ulong val)
        {
            JsonElement property;
            ulong result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt64(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref uint val)
        {
            JsonElement property;
            uint result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt32(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref ushort val)
        {
            JsonElement property;
            ushort result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetUInt16(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref byte val)
        {
            JsonElement property;
            byte result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetByte(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref float val)
        {
            JsonElement property;
            float result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetSingle(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref double val)
        {
            JsonElement property;
            double result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDouble(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref DateTime val)
        {
            JsonElement property;
            DateTime result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTime(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref DateTimeOffset val)
        {
            JsonElement property;
            DateTimeOffset result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDateTimeOffset(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref decimal val)
        {
            JsonElement property;
            decimal result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetDecimal(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref Guid val)
        {
            JsonElement property;
            Guid result;

            if (element.TryGetProperty(name, out property) &&
                property.TryGetGuid(out result))
            {
                val = result;
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref string val)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property))
            {
                val = property.GetString();
                return true;
            }
            else return false;
        }
        public static bool TryFind(this JsonElement element, string name, ref bool val)
        {
            JsonElement property;

            if (element.TryGetProperty(name, out property))
            {
                val = property.GetBoolean();
                return true;
            }
            else return false;
        }

        public static void PopulateObject<T>(this JsonElement element, T target, JsonSerializerOptions options = null) where T : class =>
            element.PopulateObject(target, typeof(T), options);
        public static void OverwriteProperty<T>(T target, JsonProperty updatedProperty, JsonSerializerOptions options = null) where T : class =>
            OverwriteProperty(target, updatedProperty, typeof(T), options);
        public static void PopulateObject(this JsonElement element, object target, Type type, JsonSerializerOptions options = null)
        {
            foreach (var property in element.EnumerateObject())
                OverwriteProperty(target, property, type, options);
        }
        public static void OverwriteProperty(object target, JsonProperty updatedProperty, Type type, JsonSerializerOptions options = null)
        {
            var propertyInfo = type.GetProperty(updatedProperty.Name);

            if (propertyInfo == null) return;
            if (!propertyInfo.CanWrite) return;
            if (Attribute.IsDefined(propertyInfo, typeof(JsonIgnoreAttribute))) return;

            Type propertyType = propertyInfo.PropertyType;
            object parsedValue;

            if (propertyType.IsValueType || true)
            {
                parsedValue = JsonSerializer.Deserialize(
                    updatedProperty.Value.GetRawText(),
                    propertyType, options);
            }
            else
            {
                parsedValue = propertyInfo.GetValue(target);
                updatedProperty.Value.PopulateObject(parsedValue, propertyType);
            }

            propertyInfo.SetValue(target, parsedValue);
        }
    }
    public class JsonHandleSpecialDoublesAsStrings : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return double.Parse(reader.GetString());
            }
            return reader.GetDouble();
        }
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                writer.WriteStringValue(value.ToString());
            else writer.WriteNumberValue(value);
        }
    }
    public class JsonHandleSpecialFloatsAsStrings : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return float.Parse(reader.GetString());
            return reader.GetSingle();
        }
        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                writer.WriteStringValue(value.ToString());
            else writer.WriteNumberValue(value);
        }
    }
    public class JsonResourceIDConverter : JsonConverter<ResourceID>
    {
        public override ResourceID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return ResourceID.Parse(reader.GetString());
            reader.Skip(); 
            return new ResourceID();
        }
        public override void Write(Utf8JsonWriter writer, ResourceID value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
    public class JsonColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return Color.FromArgb(int.Parse(reader.GetString(), NumberStyles.HexNumber));
            reader.Skip();
            return Color.Black;
        }
        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToArgb().ToString("X8").ToString());
        }
    }
}
