using Newtonsoft.Json;

namespace Common.Utils;

public class JsonUtils
{
    public static string ToJson(object obj)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        //settings.Converters.Add(new IntCollectionConverter());
        return JsonConvert.SerializeObject(obj, settings);
    }

    public static T FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}