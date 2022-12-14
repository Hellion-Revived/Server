using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZeroGravity;

public class PersistenceJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(PersistenceData) || objectType == typeof(PersistenceObjectData);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}
		try
		{
			JObject jo = JObject.Load(reader);
			return PersistenceData.GetData(jo, serializer);
		}
		catch (Exception ex)
		{
			Dbg.Exception(ex);
		}
		return null;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value);
	}
}
