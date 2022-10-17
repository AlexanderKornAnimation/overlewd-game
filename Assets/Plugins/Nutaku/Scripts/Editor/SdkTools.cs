using UnityEditor;
using UnityEngine;

namespace Nutaku.Unity
{
    /// <summary>
	/// Unity Editor extension of Nutaku Unity SDK.
    /// </summary>
    public static class SdkTools
    {
        /// <summary>
		/// Delete the login information saved on the PC when running in Unity Editor
        /// </summary>
        [MenuItem("Nutaku Tools/Delete Login Info")]
        static void DeleteLoginInfo()
        {
            PlayerPrefs.DeleteKey(SandboxLoginView.PrefKeyAutologinToken);
        }
    }
}
