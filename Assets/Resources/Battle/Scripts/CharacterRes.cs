using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New CharacterContent", menuName = "ScriptableObjects/CharacterContent")]
    public class CharacterRes : ScriptableObject
    {
        //public string charName;
        public string key;
        public Sprite icoPortrait;
        public Sprite bigPortrait;

        public Skill[] skill;
        public Skill[] pSkill;
    }
}