using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player
{
    [Serializable]
    public class PlayerInfo
    {
        public int id;
        public string name;
    }

    public static IEnumerator GetPlayerInfo(Action<PlayerInfo> success)
    {
        yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/me", NetworkHelper.tokens.accessToken, s =>
        {
            var playerInfo = JsonUtility.FromJson<PlayerInfo>(s);
            success?.Invoke(playerInfo);
        });
    }

    public static IEnumerator ChangeName(string name, Action<PlayerInfo> success)
    {
        var formMe = new WWWForm();
        formMe.AddField("name", name);
        yield return NetworkHelper.PostWithToken("https://overlude-api.herokuapp.com/me", formMe, NetworkHelper.tokens.accessToken, s =>
        {
            var playerInfo = JsonUtility.FromJson<PlayerInfo>(s);
            success?.Invoke(playerInfo);
        });
    }
}
