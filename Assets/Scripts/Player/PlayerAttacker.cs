using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace CW
{
    

public class PlayerAttacker : MonoBehaviour
{
    private PlayerAnimatorManager _playerAnimatorManager;
    private InputHandler inputHandler;
    private WeaponSlotManager WeaponSlotManager;
    public string lastAttack;

    public void Awake()
    {
        _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        inputHandler = GetComponent<InputHandler>();
        WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            _playerAnimatorManager.anim.SetBool("canDoCombo", false);
            
            // play associated combo depending on previous attack
            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
            }
            else if (lastAttack == weapon.th_light_attack_01)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true);
                lastAttack = weapon.th_light_attack_02;
            }
            else if (lastAttack == weapon.th_light_attack_02)
            {
                _playerAnimatorManager.PlayTargetAnimation(weapon.th_heavy_attack_01, true);
                lastAttack = weapon.th_heavy_attack_01;
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        // asign attacking weapon regardless
        WeaponSlotManager.attackingWeapon = weapon;
        // two hand attack
        if (inputHandler.twoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true);
            lastAttack = weapon.th_light_attack_01;
        }
        // one hand attack
        else
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }
       
    }
    
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        // two hand attack
        if (inputHandler.twoHandFlag)
        {
            _playerAnimatorManager.PlayTargetAnimation(weapon.th_heavy_attack_01, true);
            lastAttack = weapon.th_heavy_attack_01;
        } 
        // one hand attack
        else
        {
            WeaponSlotManager.attackingWeapon = weapon;
            _playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }
    }
}
}
