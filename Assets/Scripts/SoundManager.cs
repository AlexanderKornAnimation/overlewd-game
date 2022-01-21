using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Overlewd
{
    public static class SoundManager
    {
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