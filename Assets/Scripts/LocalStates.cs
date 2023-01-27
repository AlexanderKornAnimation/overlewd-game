using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class LocalStates
    {
        public abstract class Key
        {
            public string key { get; }
            public Key(string _key)
            {
                key = _key;
            }
            public bool Has() =>
                PlayerPrefs.HasKey(key);
            public void Delete()
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }
        public class KeyString : Key
        {
            public KeyString(string _key) : base(_key)
            {

            }
            public void Set(string value = "")
            {
                PlayerPrefs.SetString(key, value);
                PlayerPrefs.Save();
            }
            public string value
            {
                get => PlayerPrefs.GetString(key);
                set => Set(value);
            }
        }

        public const string GlobalPrefix = "Overlewd";
        public static class UIStates
        {
            public static readonly string Prefix = $"{GlobalPrefix}.UIStates";

            public static readonly KeyString QuestWidgetEnabled = new KeyString($"{Prefix}.QuestWidgetEnabled");

            public static List<Key> keys => new List<Key>
            {
                QuestWidgetEnabled
            };
            public static void Reset()
            {
                foreach (var k in keys)
                {
                    k.Delete();
                }
            }
        }

        public static void Reset()
        {
            UIStates.Reset();
        }
    }
}
