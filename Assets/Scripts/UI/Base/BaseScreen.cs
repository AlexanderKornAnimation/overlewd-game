using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public Missclick missclick { get; set; }
        public virtual void OnGameDataEvent(GameDataEvent eventData)
        {

        }

        public virtual void OnUIEvent(UIEvent eventData)
        {

        }

        public virtual void OnMissclick()
        {

        }

        public virtual ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenTopShow>();
        }

        public virtual ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenTopHide>();
        }

        public virtual async Task BeforeShowDataAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task BeforeShowMakeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task BeforeShowAsync()
        {
            await Task.CompletedTask;
        }

        public virtual void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericWindowShow);
        }

        public virtual async Task AfterShowAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task BeforeHideDataAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task BeforeHideMakeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task BeforeHideAsync()
        {
            await Task.CompletedTask;
        }

        public virtual void StartHide()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericWindowHide);
        }

        public virtual async Task AfterHideAsync()
        {
            await Task.CompletedTask;
        }

        public bool IsTransitionState()
        {
            return GetComponent<ScreenTransition>() != null;
        }

        public bool IsShowTransitionState()
        {
            return GetComponent<ScreenShow>() != null;
        }

        public bool IsHideTransitionState()
        {
            return GetComponent<ScreenHide>() != null;
        }

        public ScreenTransition GetTransition()
        {
            return GetComponent<ScreenTransition>();
        }
    }

    public abstract class BaseScreenInData
    {
        public int? eventId { get; set; }
        public AdminBRO.EventItem eventData =>
            GameData.events.GetEventById(eventId);

        public int? eventStageId { get; set; }
        public AdminBRO.EventStageItem eventStageData => 
            GameData.events.GetStageById(eventStageId);

        public int? ftueStageId { get; set; }
        public AdminBRO.FTUEStageItem ftueStageData =>
            GameData.ftue.GetStageById(ftueStageId);

        public int? questId { get; set; }
        public AdminBRO.QuestItem questData =>
            GameData.quests.GetById(questId);

        public int? dialogId { get; set; }
        public AdminBRO.Dialog dialogData =>
            eventStageData?.dialogData ?? ftueStageData?.dialogData ?? GameData.dialogs.GetById(dialogId);

        public int? battleId { get; set; }
        public AdminBRO.Battle battleData =>
            eventStageData?.battleData ?? ftueStageData?.battleData ?? GameData.battles.GetById(battleId);

        public bool hasEventStage => eventStageId.HasValue;
        public bool hasFTUEStage => ftueStageId.HasValue;

        public int? marketId { get; set; }
        public AdminBRO.MarketItem marketData =>
            GameData.markets.GetMarketById(marketId);

        protected bool IsType<T>() where T : BaseScreenInData =>
            this.GetType() == typeof(T);
        protected T As<T>() where T : BaseScreenInData =>
            this as T;
    }
}