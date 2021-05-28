using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace restapi.Helpers
{
    public static class PublicJsonSerializer
    {
        public static void Serialize(Stream outputStream, object value)
        {
            var serializer = new JsonSerializer();

            using (var streamWriter = new StreamWriter(outputStream))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonWriter, value);
                }
            }
        }

        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, PublicSerializerSettings.SerializerSettings);
        }

        public static string SerializeObjectIndented(object value)
        {
            return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, PublicSerializerSettings.SerializerSettings);
        }

        public static T Deserialize<T>(Stream inputStream)
        {
            var serializer = new JsonSerializer();

            using (var streamReader = new StreamReader(inputStream))
            {
                using (var jsonWriter = new JsonTextReader(streamReader))
                {
                    return serializer.Deserialize<T>(jsonWriter);
                }
            }
        }

        public static object DeserializeObject(string json, Type t)
        {
            return JsonConvert.DeserializeObject(json, t, PublicSerializerSettings.SerializerSettings);
        }

        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, PublicSerializerSettings.SerializerSettings);
        }
    }

    public static class PublicSerializerSettings
    {
        static JsonSerializerSettings serializerSettings;

        public static JsonSerializerSettings SerializerSettings
        {
            get
            {
                if (serializerSettings == null)
                {
                    List<JsonConverter> converters = new List<JsonConverter>()
                        {
                            new StringEnumConverter(new CamelCaseNamingStrategy(), true)
                        };

                    serializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters = converters,
                        NullValueHandling = NullValueHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.None,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DateFormatString = "u",
                        DateParseHandling = DateParseHandling.DateTime,
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,

                    };

                    serializerSettings.Formatting = Formatting.Indented;
                    serializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }

                return serializerSettings;
            }
        }
    }
}
