﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using RestProConnect.DataJson;
//
//    var gpStatus = GpStatus.FromJson(jsonString);

namespace RestProConnect.DataJson
{


    public partial class GpStatus
    {
        [JsonProperty("status")]
        public Dictionary<string, Status> Status { get; set; }

        [JsonProperty("settings")]
        public Dictionary<string, long> Settings { get; set; }
    }

    public partial struct Status
    {
        public long? Integer;
        public string String;

        public static implicit operator Status(long Integer) => new Status { Integer = Integer };
        public static implicit operator Status(string String) => new Status { String = String };
    }

    public partial class GpStatus
    {
        public static GpStatus FromJson(string json) => JsonConvert.DeserializeObject<GpStatus>(json, RestProConnect.DataJson.Converter2.Settings);
    }

    public static class Serialize2
    {
        public static string ToJson(this GpStatus self) => JsonConvert.SerializeObject(self, RestProConnect.DataJson.Converter2.Settings);
    }

    internal static class Converter2
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                StatusConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class StatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new Status { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Status { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Status");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Status)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type Status");
        }

        public static readonly StatusConverter Singleton = new StatusConverter();
    }
}
