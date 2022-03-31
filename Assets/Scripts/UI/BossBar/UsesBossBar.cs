using System.Collections;
using UnityEngine;

namespace DesoliteTanks.UI.BossBar
{
    public class UsesBossBar : Semaphore
    {
        private EntityHealth healthScript;
        protected override void SephamoreStart(Manager manager)
        {
            healthScript = GetComponent<EntityHealth>();
            BossBarManager.Instance.AddBossBar(healthScript);
        }

        private void OnDisable()
        {
            BossBarManager.Instance.RemoveBossBar(healthScript);
        }
    }
}