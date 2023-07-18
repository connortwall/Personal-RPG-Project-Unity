using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace CW
{
    public class UIBossHealthBar : MonoBehaviour
    {
        public TMP_Text bossName;
        private Slider slider;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            SetUIHealthBarToInactive();
        }

        public void SetBossName(TMP_Text name)
        {
            bossName = name;
        }

        public void SetUIHealthBarToActive()
        {
            slider.gameObject.SetActive(true);
        }

        public void SetUIHealthBarToInactive()
        {
            slider.gameObject.SetActive(false);
        }

        public void SetBossMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetBossCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}
