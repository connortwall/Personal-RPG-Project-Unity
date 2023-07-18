using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class Interactable : MonoBehaviour
{
    public float radius;
    public string interactableText;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }

    // called when player interacts, virtual so that it can be overridden
    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("you interacted with object");
    }
}
}
