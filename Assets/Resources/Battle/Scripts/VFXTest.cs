using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VFXTest : MonoBehaviour
    {

        [SerializeField] private Button dropPlayer, dropEnemy, dropBoss;
        private Transform playerT, enemyT, bossT, spawnPoint;

        BattleManager bm;
        SpineWidget sw;

        [SerializeField] TMP_InputField inputField;
        string animationTitle = "fx_aoe_heal";
        [SerializeField] TextMeshProUGUI log;
        bool loop = true;

        private void Start()
        {
            bm = FindObjectOfType<BattleManager>();

            playerT = bm.transform.Find("BattleCanvas/BattleLayer/topVFX/L");
            enemyT = bm.transform.Find("BattleCanvas/BattleLayer/topVFX/R");
            bossT = bm.transform.Find("BattleCanvas/BattleLayer/battlePosBoss");

            dropPlayer?.onClick.AddListener(delegate { SetTransform(playerT); });
            dropEnemy?.onClick.AddListener(delegate { SetTransform(enemyT); });
            dropBoss?.onClick.AddListener(delegate { SetTransform(bossT); });
        }

        void SetTransform(Transform spawnPoint)
        {
            if (bm == null) 
                spawnPoint = transform;
            else
                this.spawnPoint = spawnPoint;
            SpawnAnimation();
        }

        void GetAnimationTitle() => animationTitle = inputField.text;
        public void ChangeLoop() => loop = !loop;

        void SpawnAnimation()
        {
            GetAnimationTitle();
            if (sw != null) Destroy(sw.gameObject);
            sw = null;
            sw = SpineWidget.GetInstance(GameData.animations.GetByTitle(animationTitle), spawnPoint);
            sw?.PlayAnimation("action", loop);
            if (sw != null)
                log.text = $"{animationTitle} Successful Load";
            else
                log.text = $"<color=\"red\">{animationTitle} wont load, check title name or resource on server</color>\n";
        }
        private void OnDestroy()
        {
            if (sw != null) Destroy(sw.gameObject);
        }
    }
}