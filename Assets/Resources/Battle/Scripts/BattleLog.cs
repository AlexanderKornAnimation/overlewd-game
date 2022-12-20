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
                Debug.LogWarning(text);
                //FindObjectOfType<BattleManager>().debug = 2;
            }
        }
        /*private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 22;

            GUI.Label(new Rect(Screen.width / 2 - 80, 64, 300, 500), $"Battle ID: {battleData.id}\n" +
                $"is BossLevel {battleData.isTypeBoss}", style);
        }*/
    }
}