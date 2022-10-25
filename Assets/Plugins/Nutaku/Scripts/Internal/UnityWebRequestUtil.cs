using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Nutaku.Unity
{
    public class UnityWebRequestUtil
    {
        public delegate void callbackFunctionDelegate(RawResult rawResult);
        callbackFunctionDelegate callbackFunctionDelegateVar;
        private RawResult rawResult;

        public UnityWebRequestUtil() { }

        public void StartSendingRawRequest(String method, MonoBehaviour myMonoBehaviour, IEnumerable<KeyValuePair<string, string>> header, String url, String body, callbackFunctionDelegate callbackFunctionDelegate)
        {
            callbackFunctionDelegateVar = callbackFunctionDelegate;
            if ("GET".CompareTo(method) == 0)
            {
              myMonoBehaviour.StartCoroutine(GetRequest(url, header));
            }
            else if ("POST".CompareTo(method) == 0)
            {
                myMonoBehaviour.StartCoroutine(PostRequest(url, header, body));
            }
            else if ("PUT".CompareTo(method) == 0)
            {
                myMonoBehaviour.StartCoroutine(PutRequest(url, header, body));
            }
            else if ("DELETE".CompareTo(method) == 0)
            {
                myMonoBehaviour.StartCoroutine(DeleteRequest(url, header, body));
            }
        }

        IEnumerator PostRequest(string uri, IEnumerable<KeyValuePair<string, string>> headers, String body)
        {
            byte[] bytePostData = Encoding.UTF8.GetBytes(body);

            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, "POST"))
            {
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bytePostData);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    UnityEngine.Debug.Log("Error: " + webRequest.responseCode + "\n" + webRequest.error);
                    rawResult.body = Encoding.UTF8.GetBytes(webRequest.error);
                }
                else
                {
                    UnityEngine.Debug.Log("Received from server: " + webRequest.responseCode + "\n" + webRequest.downloadHandler.text);
                    rawResult.body = Encoding.UTF8.GetBytes(webRequest.downloadHandler.text);
                }
                rawResult.statusCode = (int)webRequest.responseCode;
                callbackFunctionDelegate callbackFunctionDelegateInstance = new callbackFunctionDelegate(callbackFunctionDelegateVar);
                callbackFunctionDelegateInstance(rawResult);
            }
        }

        IEnumerator GetRequest(string uri, IEnumerable<KeyValuePair<string, string>> headers)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.method = "GET";
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    UnityEngine.Debug.Log("Error: " + webRequest.responseCode + "\n" + webRequest.error);
                    rawResult.body = Encoding.UTF8.GetBytes(webRequest.error);
                }
                else
                {
                    UnityEngine.Debug.Log("Received from server: " + webRequest.responseCode + "\n" + webRequest.downloadHandler.text);
                    rawResult.body = Encoding.UTF8.GetBytes(webRequest.downloadHandler.text);
                }
                rawResult.statusCode = (int)webRequest.responseCode;
                callbackFunctionDelegate callbackFunctionDelegateInstance = new callbackFunctionDelegate(callbackFunctionDelegateVar);
                callbackFunctionDelegateInstance(rawResult);
            }
        }

        IEnumerator PutRequest(string uri, IEnumerable<KeyValuePair<string, string>> headers, String body)
        {
            byte[] bytePostData = Encoding.UTF8.GetBytes(body);

            using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, bytePostData))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    UnityEngine.Debug.Log("Error: " + webRequest.responseCode + "\n" + webRequest.error);
                    rawResult.body = Encoding.UTF8.GetBytes(webRequest.error);
                }
                else
                {
                    UnityEngine.Debug.Log("Received from server: " + webRequest.responseCode + "\n" + webRequest.downloadHandler.text);
                    rawResult.body = Encoding.UTF8.GetBytes(webRequest.downloadHandler.text);
                }
                rawResult.statusCode = (int)webRequest.responseCode;
                callbackFunctionDelegate callbackFunctionDelegateInstance = new callbackFunctionDelegate(callbackFunctionDelegateVar);
                callbackFunctionDelegateInstance(rawResult);
            }
        }

        IEnumerator DeleteRequest(string uri, IEnumerable<KeyValuePair<string, string>> headers, String body)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    UnityEngine.Debug.Log("Error: " + webRequest.error);
                    rawResult.body = Encoding.UTF8.GetBytes(webRequest.error);
                }
                else
                {
                    //https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Delete.html
                    //For Delete, UnityWebRequest does not return DownloadHander
                    //I can only get responseCode, not body, so I hardcode the body for rawResult
                    UnityEngine.Debug.Log("Received from server: " + webRequest.responseCode);
                    rawResult.body = Encoding.UTF8.GetBytes("{ \"entry\":\"\"}");
                }
                rawResult.statusCode = (int)webRequest.responseCode;
                callbackFunctionDelegate callbackFunctionDelegateInstance = new callbackFunctionDelegate(callbackFunctionDelegateVar);
                callbackFunctionDelegateInstance(rawResult);
            }
        }
    }
}
