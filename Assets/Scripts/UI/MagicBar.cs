using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{
    

public class MagicBar : MonoBehaviour
{
    public Slider slider;

    public void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxMagic(float maxMagic)
    {
        slider.maxValue = maxMagic;
        slider.value = maxMagic;
    }
        

    public void SetCurrentMagic(float currentMagic)
    {
        slider.value = currentMagic;
    }
}
}
