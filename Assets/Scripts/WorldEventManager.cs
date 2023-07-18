using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class WorldEventManager : MonoBehaviour
{
    public List<FogWall> fogWalls;
    public UIBossHealthBar bossHealthBar;
    private EnemyBossManager enemyBossManager;

    public bool bossFightIsActive; // is currently fightig boss
    public bool bossHasBeenAwakened; // awoke boss, already saw cutscene
    public bool bossHasBeenDefeated; // boss has been dfeated

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
    }

    public void ActivateBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();
        
        // activate level block walls
        foreach (var fogWall in fogWalls)
        {
            fogWall.ActivateFogWall();
        }
    }

    // after the boss has been defeated disable fight
    public void BossHasBeenDefeated()
    {
        bossHasBeenDefeated = true;
        bossFightIsActive = false;
        // Deactivate level block walls
        foreach (var fogWall in fogWalls)
        {
            fogWall.DeactivateFogWall();
        }
    }
}

}