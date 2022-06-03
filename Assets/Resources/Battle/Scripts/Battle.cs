using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Battle")]
    public class Battle : ScriptableObject
    {

        public string battleID = "battle1";

        public bool bossLevel;
        public bool fullRandom = false;

        public List<CharacterRes> players;
        public List<CharacterRes> enemysWave1;
        public List<CharacterRes> enemysWave2 = null;
        public List<CharacterRes> enemysWave3 = null;

        [Range(1, 3)]
        public int maxWave = 1;

        public CharacterRes boss = null;

        public bool hideAOE = false;
        public bool hidePotions = false;

        public bool powerBuff = false;
        public bool agilityBuff = false;
        public bool constitutionBuff = false;

        public bool[] notifyIsAlreadyShow = null;
    }
}
