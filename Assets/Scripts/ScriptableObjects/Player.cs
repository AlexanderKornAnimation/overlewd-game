using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player", order = 51)]
public class Player : ScriptableObject
{
    [SerializeField]
    private NetworkHelper networkHelper;

    [SerializeField]
    private ResourceManager resourceManager;

    [Serializable]
    public class PlayerInfo
    {
        public int id;
        public string name;
    }

    public IEnumerator GetPlayerInfo(Action<PlayerInfo> success)
    {
        return networkHelper.GetWithToken("https://overlude-api.herokuapp.com/me", networkHelper.tokens.accessToken, s =>
        {
            Debug.Log("ME");
            Debug.Log(s);

            var playerInfo = JsonUtility.FromJson<PlayerInfo>(s);
            success(playerInfo);
        });
    }

    public IEnumerator ChangeName(string name, Action<PlayerInfo> success)
    {
        var formMe = new WWWForm();
        formMe.AddField("name", name);
        return networkHelper.PostWithToken("https://overlude-api.herokuapp.com/me", formMe, networkHelper.tokens.accessToken, s =>
        {
            Debug.Log("ME");
            Debug.Log(s);

            var playerInfo = JsonUtility.FromJson<PlayerInfo>(s);
            success(playerInfo);
        });
    }
}
