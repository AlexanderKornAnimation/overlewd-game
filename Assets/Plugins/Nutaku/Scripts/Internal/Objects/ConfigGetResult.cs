namespace Nutaku.Unity
{
    /// <summary>
	/// Structure that stores the result of 'Config.Get'
    /// </summary>
    public struct ConfigGetResult
    {
        public string code;
        public Result result;

        public struct Result
        {
            public int is_adult;
            public Maintenance maintenance;
            public Version version;

            public struct Maintenance
            {
                public int is_maintenance;
                public string message;
            }

            public struct Version
            {
                public int code;
                public string description;
                public int is_force_update;
                public string update_url;
            }
        }
    }
}
