using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        public void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxStamina(float maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }
        

        public void SetCurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
        }
    }
}
