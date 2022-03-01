using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Overlewd
{
    public static class JsonHelper
    {
        public static T DeserializeObject<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        Debug.LogError($"Deserialize entity \"{typeof(T).FullName}\" error:  {args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true;
                    },
                    MissingMemberHandling = MissingMemberHandling.Error
                });
            }
            catch (JsonSerializationException ex)
            {
                Debug.LogError($"Deserialize Error: {ex.Message}");
                return default(T);
            }
        }

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
