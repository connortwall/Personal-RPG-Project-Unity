using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{

    public class ExpCountBar : MonoBehaviour
{
    public TextMeshProUGUI expCountText;

    public void SetExpCountText(int expCount)
    {
        expCountText.text = expCount.ToString();
    }
    
}
}