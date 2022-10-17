namespace Nutaku.Unity
{
    /// <summary>
	/// Structure that stores the result of 'User.St'.
    /// </summary>
    public struct UserStResult
    {
        public string code;
        public int error_no;
        public Result result;

        public struct Result
        {
            public string st;
        }
    }
}
