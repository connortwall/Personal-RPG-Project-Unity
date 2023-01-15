using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class BlockingCollider : MonoBehaviour
{
    public BoxCollider blockingBoxCollider;

    public float blockingPhysicalDamageAbsorption;

    private void Awake()
    {
        blockingBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetColliderDamageAbsorption(WeaponItem weapon)
    {
        if (weapon != null) 
        {
            blockingPhysicalDamageAbsorption = weapon.physicalDamageAbsorption;
        }
    }

    public void EnableBlockingCollider()
    {
        blockingBoxCollider.enabled = true;
    }
    public void DisableBlockingCollider()
    {
        blockingBoxCollider.enabled = false;
    }
}
}
