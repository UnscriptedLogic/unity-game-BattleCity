using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace DesoliteTanks.UI.BossBar
{
    public class BossBar : MonoBehaviour
    {
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI nameTMP;
        public Slider slider;
        private float maxHealth;

        public void InitializeBar(string entityName, int currentHealth, int maxHealth)
        {
            nameTMP.text = entityName;
            slider.maxValue = maxHealth;
            slider.value = currentHealth;

            this.maxHealth = maxHealth;
        }

        public void UpdateText(int amount)
        {
            healthText.text = amount.ToString() + "/" + maxHealth.ToString();
            slider.value = amount;
        }
    }
}