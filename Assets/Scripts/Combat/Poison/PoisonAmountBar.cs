using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{
    

public class PoisonAmountBar : MonoBehaviour
{
    public Slider slider;

    public void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 0;
        gameObject.SetActive(false);
    }
        

    public void SetPoisonAmount(int currentPoisonAmount)
    {
        slider.value = currentPoisonAmount;
    }
}
}