using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace PinSharper
{
    internal class ResultConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Result<T>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var result = new Result<T>();
            result.Code = (int)obj["code"];
            result.Data = (dynamic)obj["data"].ToObject<T>();
            result.Message = (string)obj["message"];
            result.MessageDetail = (string)obj["message_detail"];
            result.Succeeded = ((string)obj["status"] == "success");
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
