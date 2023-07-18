using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{

    public class PoisonBuildUpBar : MonoBehaviour
    {
        public Slider slider;

        public void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            // set innitial value to full bar
            slider.value = 100;
            gameObject.SetActive(false);
        }
        

        public void SetPoisonBuildUp(int currentPoisonBuildUp)
        {
            slider.value = currentPoisonBuildUp;
        }
    }
}
