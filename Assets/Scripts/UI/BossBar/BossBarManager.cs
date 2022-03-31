using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DesoliteTanks.UI.BossBar
{
    public class BossBarManager : MonoBehaviour
    {
        public static BossBarManager Instance;
        private Dictionary<EntityHealth, BossBar> bossBars = new Dictionary<EntityHealth, BossBar>();

        public Transform barList;
        public GameObject barPrefab;

        private void Awake()
        {
            Instance = this;
        }

        public void AddBossBar(EntityHealth healthScript)
        {
            GameObject barObject = Instantiate(barPrefab, barList);
            BossBar bar = barObject.GetComponent<BossBar>();

            bar.InitializeBar(healthScript.Manager.name, healthScript.Manager.health, healthScript.Manager.settings.health);

            healthScript.onHealthChanged += bar.UpdateText;
            bossBars.Add(healthScript, bar);
        }

        public void RemoveBossBar(EntityHealth healthScript)
        {
            BossBar bar = bossBars[healthScript];
            healthScript.onHealthChanged -= bar.UpdateText;

            Destroy(bar.gameObject);
        }
    }
}