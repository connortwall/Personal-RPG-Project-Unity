using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    public class EventColliderBeginBossFight : MonoBehaviour
{
    private WorldEventManager worldEventManager;

    private void Awake()
    {
        worldEventManager = FindObjectOfType<WorldEventManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            worldEventManager.ActivateBossFight();
        }
    }
}
}
