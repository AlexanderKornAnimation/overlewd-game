using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public abstract class BaseSystemNotif : MonoBehaviour
    {
        public async void Show()
        {
            await ShowAsync();
        }

        public async Task ShowAsync()
        {
            gameObject.SetActive(true);
            UIManager.SetUserInputLockerMode(UIManager.UserInputLockerMode.Manual, false);
            await Task.CompletedTask;
        }

        public async void Hide()
        {
            await HideAsync();
        }

        public async Task HideAsync()
        {
            gameObject.SetActive(false);
            UIManager.SetUserInputLockerMode(UIManager.UserInputLockerMode.Manual, true);
            await Task.CompletedTask;
        }

        public async void Close()
        {
            await CloseAsync();
        }

        public async Task CloseAsync()
        {
            await HideAsync();
            DestroyImmediate(gameObject);
        }

        public static T GetInstance<T>(Transform parent) where T : BaseSystemNotif
        {
            return typeof(T).GetMethod("GetInstance").Invoke(null, new[] { parent }) as T;
        }
    }
}

