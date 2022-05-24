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

        public List<Character> players;
        public List<Character> enemysWave1;
        public List<Character> enemysWave2 = null;
        public List<Character> enemysWave3 = null;

        [Range(1, 3)]
        public int maxWave = 1;

        public Character boss = null;

        public bool hideAOE = false;
        public bool hidePotions = false;

        public bool powerBuff = false;
        public bool agilityBuff = false;
        public bool constitutionBuff = false;

        public bool[] notifyIsAlreadyShow = null;
    }
}
