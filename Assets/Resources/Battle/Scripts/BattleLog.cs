using TMPro;
using UnityEngine;
namespace Overlewd
{
    public class BattleLog : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI log;

        public void Add(string text, bool error = false)
        {
            if (!error) 
            { 
                if (log) log.text += ($"{text}\n");
            }
            else
            {
                if (log) log.text += ($"<color=\"red\">{text}</color>\n");
                Debug.LogError(text);
                //FindObjectOfType<BattleManager>().debug = 2;
            }
        }
    } 
}