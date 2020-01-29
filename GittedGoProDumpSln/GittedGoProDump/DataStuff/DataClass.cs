using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GittedGoProDump.DataStuff
{
    public partial class GpMediaData
    {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("media")]
            public List<Media> Media { get; set; }
        public string FirsttSnapped()
        {
            return (Media?.Any() != true) ? "0" : Media[0].Fs.FirstOrDefault().N;
        }
        public string LastSnapped() {
            return (Media?.Any() != true)? "0":Media[0].Fs.LastOrDefault().N;
        }
        public int LastIndex() {
            return (Media?.Any() != true) ? -1 : Media[0].Fs.Count -1;
        }

        public string Get_NextPicAfter(string argThisOne) {
             
            int _indexOfThisOne = GetIndexOf(argThisOne);
            if (_indexOfThisOne >= 0)
            {
                if (LastIndex() > _indexOfThisOne)
                {
                    _indexOfThisOne++;
                    return Media[0].Fs[_indexOfThisOne].N;
                }
            }
            return "0";
        }

        int GetIndexOf(string ArgPicName) {
            int outputindex = -1;           
            for (int i = 0; i < Media[0].Fs.Count(); i++)
            {
                if (string.Compare(Media[0].Fs[i].N, ArgPicName) == 0)
                {
                    outputindex = i;
                    break;
                }
            }
            return outputindex;
        }

  }

        public partial class Media
        {
            [JsonProperty("d")]
            public string D { get; set; }

            [JsonProperty("fs")]
            public List<F> Fs { get; set; }
        }

        public partial class F
        {
            [JsonProperty("n")]
            public string N { get; set; }

            [JsonProperty("mod")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long Mod { get; set; }

            [JsonProperty("s")]
            [JsonConverter(typeof(ParseStringConverter))]
            public long S { get; set; }
        }

        public partial class GpMediaData
        {
            public static GpMediaData FromJson(string json) => JsonConvert.DeserializeObject<GpMediaData>(json, GittedGoProDump.DataStuff.Converter.Settings);
        }

        public static class Serialize
        {
            public static string ToJson(this GpMediaData self) => JsonConvert.SerializeObject(self, GittedGoProDump.DataStuff.Converter.Settings);
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                long l;
                if (Int64.TryParse(value, out l))
                {
                    return l;
                }
                throw new Exception("Cannot unmarshal type long");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (long)untypedValue;
                serializer.Serialize(writer, value.ToString());
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }
     
}
