using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Overlewd
{
    public static class SoundManager
    {
        public class SoundPath
        {
            public const string CastleScreenButtons = "event:/UI/Buttons/Basic_menu_click";
            public const string CastleWindowSlideOn = "event:/UI/Windows/Window_slide_on";
            public const string CastleWindowSlideOff = "event:/UI/Windows/Window_slide_off";
            public const string SidebarOverlayOn = "event:/UI/Windows/Deeds_slide_on";
            public const string SidebarOverlayOff = "event:/UI/Windows/Deeds_slide_off";
        }
        
        private static EventInstance music;

        public static void PlayUISound(string soundEventPath)
        {
            RuntimeManager.PlayOneShot(soundEventPath);
        }

        public static void InstantiateMusic(string eventPath)
        {
            music = RuntimeManager.CreateInstance(eventPath);
        }

        public static void PlayMusic()
        {
            music.start();
        }

        public static void StopMusic(bool fade)
        {
            if (fade)
            {
                music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                return;
            }

            music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}