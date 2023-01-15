using UnityEngine;

namespace CW
{
    [CreateAssetMenu(menuName = "Items/WeaponItem")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Damage")] 
        // TODO: tweak in game design process
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Absorption")] 
        public float physicalDamageAbsorption;

        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
        
        [Header("Two Handed Attack Animations")]
        public string th_light_attack_01;
        public string th_light_attack_02;
        public string th_heavy_attack_01;

        [Header("Weapon Art")] 
        public string weapon_art;

        [Header("Idle Animations")] 
        public string right_hand_idle;
        public string left_hand_idle;
        public string two_hand_idle;

        [Header("Stamina Costs")] 
        public int baseStamina;
        public float lightAttackMultipler;
        public float heavyAttackMultiplier;

        [Header("Weapon Type")] 
        public bool isSpellCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;
        public bool isMeleeWeapon;
        public bool isShieldWeapon;

    }
}