using Nutaku.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test class for Nutaku APIs
/// </summary>
public class TestUIController : MonoBehaviour
{
    public Text userIdText;
    public Text nicknameText;
    public ScrollRect logViewScrollRect;
    public GameObject logViewItemPrefab;

    public CanvasGroup loadingView;

    public Button configGetButton;
    public Button logoutAndExitButton;

    public Button GetProfileButton;
    public Button GetFriendsButton;
    public Button GetPaymentButton;
    public Button PostMakeRequestButton;

    public Button GetMyProfileButton;
    public Button GetMyFriendsButton;
	public Button PostPaymentButton;
	public Button PostPlayFabPaymentButton;

    public Button openPaymentViewButton;

    Stack<Text> _textObjectCache = new Stack<Text>();

    Payment _payment = new Payment();

    void Awake()
    {
        SdkPlugin.Initialize();
        
        GetPaymentButton.interactable = false;
        openPaymentViewButton.interactable = false;

        //Misc: MobileApi Config.Get
        configGetButton.onClick.AddListener(TestConfigGet);

        //Misc: LogoutAndExit
        logoutAndExitButton.onClick.AddListener(TestLogoutAndExit);

        //RestApiHelper 2Legged: Get Profile
        GetProfileButton.onClick.AddListener(TestGetProfile);

        //RestApiHelper 2Legged: Get Friends of current User
        GetFriendsButton.onClick.AddListener(TestGetFriends);

        //RestApiHelper 2Legged: Get Payment Records
        GetPaymentButton.onClick.AddListener(TestGetPayment);

        //RestApiHelper 2Legged: Send Make Request
        PostMakeRequestButton.onClick.AddListener(TestPostMakeRequest);

        //RestApiHelper 3Legged: Get My Profile
        GetMyProfileButton.onClick.AddListener(TestGetMyProfile);

        //RestApiHelper 3Legged: Get Friends of current User 
        GetMyFriendsButton.onClick.AddListener(TestGetMyFriends);

        //RestApiHelper 3Legged: Post Payment
        PostPaymentButton.onClick.AddListener(TestPostPayment);

        //RestApiHelper 3Legged: Post PlayFab Payment
        PostPlayFabPaymentButton.onClick.AddListener(TestPostPlayFabPayment);

        //RestApiHelper 3Legged: Open Payment View
        openPaymentViewButton.onClick.AddListener(TestOpenPaymentView);
    }

    void TestConfigGet()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("ConfigGet Start");
            CoreApi.GetConfig(SdkPlugin.settings.consumerKey, SdkPlugin.settings.consumerSecret, this, TestConfigGetCallback);
        }
        catch (Exception ex)
        {
            logException(ex);
        }
    }

    void TestConfigGetCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = CoreApi.HandlePostCommandCallback<ConfigGetResult>(rawResult);
                logMessage("ConfigGet Success");
                logMessage("code: " + result.code);
                logMessage("is_adult: " + result.result.is_adult.ToString());
                logMessage("maintenance - is_maintenance: " + result.result.maintenance.is_maintenance);
                logMessage("maintenance - message: " + result.result.maintenance.message);
                logMessage("version - code: " + result.result.version.code);
                logMessage("version - description: " + result.result.version.description);
                logMessage("version - is_force_update: " + result.result.version.is_force_update);
                logMessage("version - update_url: " + result.result.version.update_url);
            }
            else
            {
                logError("ConfigGet Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("ConfigGet Failure");
            logException(ex);
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestGetProfile()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("GetProfile Start");
            var userId = SdkPlugin.loginInfo.userId;
            logMessage("Get Profile of UserID: " + userId);
            RestApiHelper.GetProfile(userId, this, TestGetProfileCallback);
        }
        catch (Exception ex)
        {
            logError("GetProfile Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestGetProfileCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("GetProfile Success");
                logMessage("Http Status Code: " + result.statusCode);
                var profile = result.GetFirstEntry();
                OutputPersonData(profile);
                userIdText.text = profile.id;
                nicknameText.text = profile.nickname;
            }
            else
            {
                logError("TestGetProfile Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("TestGetProfile Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestGetFriends()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("GetFriends Start");
            var userId = SdkPlugin.loginInfo.userId;
            logMessage("Get Friend list of UserID: " + userId);

            var queryParams = new PeopleQueryParameterBuilder
            {
                count = 3
            };
            RestApiHelper.GetFriends(userId, this, TestGetFriendsCallback, queryParams);
        }
        catch (Exception ex)
        {
            logError("GetFriends Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestGetFriendsCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("GetFriends Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                foreach (var person in result.GetEntry())
                    OutputPersonData(person);
            }
            else
            {
                logError("GetFriends Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("GetFriends Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }
    
    void TestGetPayment()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("GetPayment Start");
            RestApiHelper.GetPayment(SdkPlugin.loginInfo.userId, CheckAndGetPayment().paymentId, this, TestGetPaymentCallback);
        }
        catch (Exception ex)
        {
            logError("GetPayment Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestGetPaymentCallback(RawResult rawResult)
    {
        BeginLoading();
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Payment>(rawResult);
                logMessage("GetPayment Success");
                logMessage("Http Status Code: " + result.statusCode);
                var payment = result.GetFirstEntry();
                OutputPaymentData(payment);
            }
            else
            {
                logError("GetPayment Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("GetPayment Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestPostMakeRequest()
    {
        BeginLoading();
        const string TestName = "PostMakeRequest";
        try
        {
            logMessage("");
            logMessage(TestName + " Start");
            var callbackUrl = "https://postman-echo.com/post"; //this sample URL should just echo back the details received from Nutaku's request
            var postData = new Dictionary<string, string>
            {
                { "login_check", "1" }
            };
            logMessage("Callback URL: " + callbackUrl);
            RestApiHelper.PostMakeRequest(callbackUrl, postData, this, TestPostMakeRequestCallback);
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestPostMakeRequestCallback(RawResult rawResult)
    {
        const string TestName = "PostMakeRequest";
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandlePostMakeRequestCallback(rawResult);
                logMessage(TestName + " Success");
                logMessage("Http Status Code: " + result.statusCode);

                logMessage("rc: " + result.rc);
                logMessage("body: \n" + result.body);
                logMessage("headers:");
                foreach (var header in result.headers)
                {
                    logMessage(string.Format("\t{0}: {1}", header.Key, header.Value));
                }
            }
            else
            {
                logError("PostMakeRequest Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestGetMyProfile()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("GetMyProfile Start");
            logMessage(string.Format("Get Your(ID:{0}) Profile",SdkPlugin.loginInfo.userId));
            RestApiHelper.GetMyProfile(this, TestGetMyProfileCallback);
        }
        catch (Exception ex)
        {
            logError("GetMyProfile Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestGetMyProfileCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("GetMyProfile Success");
                logMessage("Http Status Code: " + result.statusCode);
                var profile = result.GetFirstEntry();
                OutputPersonData(profile);
                userIdText.text = profile.id;
                nicknameText.text = profile.nickname;
            }
            else
            {
                logError("GetMyProfile Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("GetMyProfile Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestGetMyFriends()
    {
        BeginLoading();
        try
        {
            logMessage("");
            logMessage("GetMyFriends Start");
            logMessage(string.Format("Get Your(ID:{0}) Friend list", SdkPlugin.loginInfo.userId));
            RestApiHelper.GetMyFriends(this, TestGetMyFriendsCallback);
        }
        catch (Exception ex)
        {
            logError("GetMyFriends Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestGetMyFriendsCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                logMessage("GetMyFriends Success");
                logMessage("Http Status Code: " + result.statusCode);
                logMessage("Total Results: " + result.totalResults);
                logMessage("Items Per Page: " + result.itemsPerPage);
                logMessage("Start Index: " + result.startIndex);
                foreach (var person in result.GetEntry())
                    OutputPersonData(person);
            }
            else
            {
                logError("GetMyFriends Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("GetMyFriends Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestPostPayment()
    {
        BeginLoading();
        try
        {
            Payment payment = new Payment();
            logMessage("");
            logMessage("PostPayment Start");

            //This is just a documented shortcut available on sandbox
            payment.callbackUrl = "https://skip.payment.handler";

            payment.finishPageUrl = "http://www.nutaku.net/";
            payment.message = "Test Payment";

            PaymentItem item = new PaymentItem
            {
                itemId = "item1",
                itemName = "Much Doge",
                unitPrice = 100,
                imageUrl = "https://dogecoin.com/imgs/dogecoin-300.png",
                description = "item description text"
            };
            payment.paymentItems.Add(item);

            RestApiHelper.PostPayment(payment, this, TestPostPaymentCallback);
        }
        catch (Exception ex)
        {
            logError("PostPayment Failure");
            DumpError(ex);
            EndLoading();
        }
    }

	void TestPostPlayFabPayment()
	{
		BeginLoading();
		try
		{
			Payment payment = new Payment();
			logMessage("");
			logMessage("PostPlayFabPayment Start");

			// Use your previous payment handler if you ever had the standard flow. Otherwise you can skip this line
			payment.callbackUrl = ""; //"https://skip.payment.handler"; This is just a documented shortcut available on sandbox.

			payment.finishPageUrl = "http://www.nutaku.net/";
			payment.message = "Test PlayFab Payment";

			payment.playFabId = "CD13F18CEE0DCE88"; // you'll need to provide the value received from PlayFab LoginWithCustomId
			payment.sessionTicket = "CD13F18CEE0DCE88---FF516-8D7C058ED5B0C87-XawWjK7VkQu9GB4Am7DOF/fpq89aK44nned0zPx8ysM="; // you'll need to provide the value received from PlayFab LoginWithCustomId
			payment.catalogVersion = "MyCatalogVersion";
			payment.storeId = "MyStoreID";

            PaymentItem item = new PaymentItem
            {
                itemId = "doge1",
                itemName = "Much Doge playfab",
                unitPrice = 11,
                imageUrl = "https://dogecoin.com/imgs/dogecoin-300.png",
                description = "playfab item description text"
            };
            payment.paymentItems.Add(item);

			RestApiHelper.PostPayment(payment, this, TestPostPaymentCallback);
		}
		catch (Exception ex)
		{
			logError("PostPayment Failure");
			DumpError(ex);
			EndLoading();
		}
	}

    void TestPostPaymentCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                Payment payment = new Payment();
                var result = RestApi.HandleRequestCallback<Payment>(rawResult);

                logMessage("PostPayment Success");
                logMessage("Http Status Code: " + result.statusCode);

                payment = result.GetFirstEntry();
                OutputPaymentData(payment);

                _payment = payment;
                GetPaymentButton.interactable = true;
                openPaymentViewButton.interactable = true;
            }
            else
            {
                logError("PostPayment Failure");
                logError("Http Status Code: " + (int)rawResult.statusCode);
                logError("Http Status Message: " + Encoding.UTF8.GetString(rawResult.body));
            }
        }
        catch (Exception ex)
        {
            logError("PostPayment Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestOpenPaymentView()
    {
        BeginLoading();
        const string TestName = "TestOpenPaymentView";
        try
        {
            logMessage("");
            logMessage(TestName + " Start");

            SdkPlugin.OpenPaymentView(CheckAndGetPayment(), TestPaymentDelegate);
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
            EndLoading();
        }
    }

    void TestPaymentDelegate(WebViewEvent result)
    {
        const string TestName = "TestParsePaymentResult";
        try
        {
            logMessage(TestName + " Success");
            logMessage("Event: " + result.kind);
            logMessage("Result: " + result.value);
            logMessage("Message: " + result.message);
            if (result.kind == WebViewEventKind.Succeeded)
            {
                openPaymentViewButton.interactable = false;
            }
        }
        catch (Exception ex)
        {
            logError(TestName + " Failure");
            DumpError(ex);
        }
        finally
        {
            EndLoading();
        }
    }

    void TestLogoutAndExit()
    {
        SdkPlugin.logoutAndExit();
    }

    void DumpError(Exception ex)
    {
        var webEx = ex as WebException;
        if (webEx != null)
        {
            var response = webEx.Response as HttpWebResponse;
            if (response != null)
            {
                logError("Http Status Code: " + (int)response.StatusCode);
            }
        }
        logException(ex);
    }

    class TestException : Exception
    {
        public TestException(string message)
            : base(message)
        {
        }

        public TestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    Payment CheckAndGetPayment()
    {
        if (_payment == null)
        {
            GetPaymentButton.interactable = false;
            openPaymentViewButton.interactable = false;
            throw new TestException("Payment is not registered");
        }

        return _payment;
    }

    void BeginLoading()
    {
        loadingView.gameObject.SetActive(true);
    }

    void EndLoading()
    {
        loadingView.gameObject.SetActive(false);
    }

    void OutputPersonData(Person person)
    {
        logMessage("id: " + person.id);
        logMessage("nickname: " + person.nickname);
        logMessage("profileUrl: " + person.profileUrl);
        logMessage("thumbnailUrl: " + person.thumbnailUrl);
        logMessage("thumbnailUrlSmall: " + person.thumbnailUrlSmall);
        logMessage("thumbnailUrlLarge: " + person.thumbnailUrlLarge);
        logMessage("thumbnailUrlHuge: " + person.thumbnailUrlHuge);
        logMessage("birthday: " + person.birthday);
        logMessage("gender: " + person.gender);
        logMessage("age: " + person.age);
        logMessage("addresses (country): " + person.addresses.country);
        logMessage("hasApp: " + person.hasApp);
        logMessage("userType: " + person.userType);
        logMessage("grade: " + person.grade);
        logMessage("languagesSpoken: " + person.languagesSpoken);
    }
    
    void OutputPaymentData(Payment payment)
    {
        logMessage("paymentId: " + payment.paymentId);
        logMessage("status: " + payment.status);
        logMessage("transactionUrl: " + payment.transactionUrl);
        logMessage("orderedTime: " + payment.orderedTime);
        logMessage("executedTime: " + payment.executedTime);

        foreach (var items in payment.paymentItems)
        {
            logMessage("\titemId: " + items.itemId);
            logMessage("\titemName: " + items.itemName);
            logMessage("\tunitPrice: " + items.unitPrice);
            logMessage("\timageUrl: " + items.imageUrl);
            logMessage("\tdescription: " + items.description);
        }
    }

    void logMessage(string message, LogType logType = LogType.Log)
    {
        Text text;
        if (_textObjectCache.Count > 0)
        {
            text = _textObjectCache.Pop();
            text.rectTransform.SetSiblingIndex(text.rectTransform.parent.childCount - 1);
            text.gameObject.SetActive(true);
        }
        else
        {
            text = (Instantiate(logViewItemPrefab) as GameObject).GetComponent<Text>();
        }
        text.text = message;
#if UNITY_ANDROID && !UNITY_EDITOR
        text.fontSize = 2;
#endif

        switch (logType)
        {
            case LogType.Exception:
                text.color = Color.magenta;
                break;

            case LogType.Warning:
                text.color = Color.yellow;
                break;

            case LogType.Error:
                text.color = Color.red;
                break;

            case LogType.Assert:
                text.color = Color.red;
                break;

            default:
                text.color = Color.white;
                break;
        }

        text.rectTransform.SetParent(logViewScrollRect.content, false);
        logViewScrollRect.normalizedPosition = new Vector2(0f, 0f);
    }

    void logError(string message)
    {
        logMessage("Error: " + message, LogType.Error);
    }

    void logException(Exception ex)
    {
        if (ex.StackTrace == null)
            logMessage("Exception: " + ex.Message, LogType.Exception);
        else
            logMessage("Exception: " + ex.Message + ".\r\nStack trace: " + ex.StackTrace, LogType.Exception);
    }
}
