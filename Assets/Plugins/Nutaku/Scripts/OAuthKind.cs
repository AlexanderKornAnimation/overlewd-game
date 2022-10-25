namespace Nutaku.Unity
{
    /// <summary>
    /// OAuth type
    /// </summary>
    public enum OAuthKind
    {
        /// <summary>
        /// Do not perform authentication.
        /// </summary>
        None,

        /// <summary>
        /// 2-legged
        /// </summary>
        TwoLegged,

        /// <summary>
        /// 3-legged
        /// </summary>
        ThreeLegged,
    }
}
