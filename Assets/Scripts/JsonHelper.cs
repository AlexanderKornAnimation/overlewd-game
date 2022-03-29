using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Overlewd
{
    public class RequireObjectPropertiesContractResolver : DefaultContractResolver
    {
        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);
            contract.ItemRequired = Required.AllowNull;
            return contract;
        }
    }

    public static class JsonHelper
    {
        public static T DeserializeObject<T>(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        Debug.LogError($"Deserialize entity \"{typeof(T).FullName}\" error:  {args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true;
                    },
                    MissingMemberHandling = MissingMemberHandling.Error,
                    // analog [JsonProperty(Required = Required.Always)] attribute for property
                    ContractResolver = new RequireObjectPropertiesContractResolver()
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
