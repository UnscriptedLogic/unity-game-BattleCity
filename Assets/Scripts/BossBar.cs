using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BossBar : MonoBehaviour
{
    public EntityManager entityManager;
    public TextMeshProUGUI healthText;
    public Slider slider;
    private float startHealth;

    private void Start()
    {
        slider.maxValue = 150;
    }

    public void Update()
    {
        healthText.text = entityManager.health.ToString() + "/150";
        slider.value = entityManager.health;
    }
}
