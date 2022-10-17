#if UNITY_EDITOR
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Nutaku.Unity
{
    /// <summary>
    /// A login screen for the sandbox environment at runtime in Unity Editor. This is NOT the login method used for production.
    /// </summary>
    public class SandboxLoginView : MonoBehaviour
    {
        static SandboxLoginView instance;

        /// <summary>
        /// When the login succeeds, the result is sent to the stream.
        /// </summary>
        public static LoginInfo Login()
        {
            if (instance == null)
            {
                InitInstance();
            }
            if (PlayerPrefs.HasKey(PrefKeyAutologinToken))
            {
                var autologinToken = PlayerPrefs.GetString(PrefKeyAutologinToken);
                if (!string.IsNullOrEmpty(autologinToken))
                {
                    try
                    {
                        AutoLogin(autologinToken);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        PlayerPrefs.DeleteKey(PrefKeyAutologinToken);
                        return instance.ReadyToInput(ex.Message);
                    }
                }
            }
            return instance.ReadyToInput();
        }

        public class LoginException : Exception
        {
            public LoginException(string message)
                : base(message)
            {
            }
        }

        public const string PrefKeyAutologinToken = "Nutaku.Unity.SandboxLoginView.AutologinToken";

        public GameObject content;
        public GameObject touchBlocker;

        public Button loginButton;
        public Text messageText;
        public InputField emailInputField;
        public InputField passwordInputField;
        readonly LoginInfo _subject = new LoginInfo();

        void Awake()
        {
            loginButton.onClick.AddListener(LoginButtonClick);
        }

        private static void InitInstance()
        {
            instance = Instantiate(Resources.Load<SandboxLoginView>("Nutaku/Prefabs/Editor/Sandbox Login View")) as SandboxLoginView;
            instance.transform.SetAsLastSibling();
            instance.content.SetActive(false);
        }

        private void LoginButtonClick()
        {
            try
            {
                if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
                    messageText.text = "Please enter a valid sandbox Username and a Password.";
                else
                {
                    touchBlocker.SetActive(true);
                    Login(emailInputField.text, passwordInputField.text);
                }
            }
            catch (Exception ex)
            {
                messageText.text = ex.Message;
                touchBlocker.SetActive(false);
            }
        }

        LoginInfo ReadyToInput(string message = null)
        {
            content.SetActive(true);
            touchBlocker.SetActive(false);
            if (message != null)
            {
                messageText.text = message;
            }
            return _subject;
        }


        /// <summary>
        /// Get Login information
        /// </summary>
        void Login(string email, string password)
        {
            CoreApi.Login(
                email,
                password,
                SdkPlugin.settings.consumerKey,
                SdkPlugin.settings.consumerSecret, this, LoginCallback);
        }

        /// <summary>
        /// Login call back function with raw JSON result
        /// </summary>
        void LoginCallback(RawResult rawResult)
        {
            try
            {
                if ((rawResult.statusCode > 99) && (rawResult.statusCode < 300))
                {
                    var result = CoreApi.HandlePostCommandCallback<LoginResult>(rawResult);
                    if (result.code != "ok")
                    {
                        switch (result.error_no)
                        {
                            case 100:
                            case 103:
                            case 104:
                                throw new LoginException("There was an error with your information.\nPlease check whether the input information is correct.");
                            case 102:
                                throw new LoginException("This account is locked.\nPlease reset the password.");
                            default:
                                throw new LoginException("System error.");
                        }
                    }
                    PlayerPrefs.SetString(PrefKeyAutologinToken, result.result.autologin_token);
                    var output = new LoginInfo(
                        result.result.user_id,
                        new AccessToken(result.result.oauth_token, result.result.oauth_token_secret));
                    NativePluginStub._instance._loginInfo = output;
                    if (instance != null)
                        Destroy(instance.gameObject);
                }
                else
                {
                    UnityEngine.Debug.Log("Login failed");
                    messageText.text = Encoding.UTF8.GetString(rawResult.body);
                    touchBlocker.SetActive(false);
                }
            }
            catch (Exception ex)
            {
                messageText.text = ex.Message;
                touchBlocker.SetActive(false);
            }
        }

        static void AutoLogin(string autologinToken)
        {
            CoreApi.Autologin(
             autologinToken,
             SdkPlugin.settings.consumerKey,
             SdkPlugin.settings.consumerSecret, instance, AutoLoginCallback);
        }

        static void AutoLoginCallback(RawResult rawResult)
        {
            if ((rawResult.statusCode > 99) && (rawResult.statusCode < 300))
            {
                var result = CoreApi.HandlePostCommandCallback<LoginResult>(rawResult);
                if (result.code != "ok")
                {
                    PlayerPrefs.DeleteKey(PrefKeyAutologinToken);
                    instance.ReadyToInput("Autologin failed");
                    throw new LoginException("Autologin failed. Response body: " + Encoding.UTF8.GetString(rawResult.body));
                }
                PlayerPrefs.SetString(PrefKeyAutologinToken, result.result.autologin_token);
                if (instance != null)
                    Destroy(instance.gameObject);
                NativePluginStub._instance._loginInfo = new LoginInfo(
                    result.result.user_id,
                    new AccessToken(result.result.oauth_token, result.result.oauth_token_secret));
            }
            else
            {
                PlayerPrefs.DeleteKey(PrefKeyAutologinToken);
                instance.ReadyToInput("Autologin failed");
                throw new LoginException("Autologin failed. Response body: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
    }
}
#endif
