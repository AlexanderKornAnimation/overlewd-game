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
            if (log) log.text += (text + "\n");
            if (error)
            {
                Debug.LogError(text);
                FindObjectOfType<BattleManager>().debug = 2;
            }
        }
    } 
}