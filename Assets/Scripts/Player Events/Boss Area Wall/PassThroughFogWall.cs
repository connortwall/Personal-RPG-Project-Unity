using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class PassThroughFogWall : Interactable
{
    private WorldEventManager worldEventManager;
    private void Awake()
    {
        worldEventManager = FindObjectOfType<WorldEventManager>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        playerManager.PassThroughFogWallInteraction(transform);
        worldEventManager.ActivateBossFight();
    }
}

}
