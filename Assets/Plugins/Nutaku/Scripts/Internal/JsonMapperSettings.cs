using System;

namespace Nutaku.Unity
{
    /// <summary>
    /// Settings for LitJson
    /// </summary>
    static class JsonMapperSettings
    {
        internal static void Initialize()
        {
            Nutaku.Unity.Json.JsonMapper.RegisterImporter<string, int>(x => int.Parse(x));
            Nutaku.Unity.Json.JsonMapper.RegisterImporter<int, string>(x => x.ToString());
            Nutaku.Unity.Json.JsonMapper.RegisterImporter<bool, string>(x => x.ToString());
            Nutaku.Unity.Json.JsonMapper.RegisterImporter<string, DateTime>(x =>
                {
                    try
                    {
                        return DateTime.Parse(x);
                    }
                    catch
                    {
                        return new DateTime();
                    }
                });
        }
    }
}
