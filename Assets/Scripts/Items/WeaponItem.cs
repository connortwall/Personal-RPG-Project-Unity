using UnityEngine;

namespace CW
{
    [CreateAssetMenu(menuName = "Items/WeaponItem")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;
        
        [Header("Animator Replacer")]
        public AnimatorOverrideController weaponController;
        public string leftHandIdleAnimation = "Left_Arm_Idle_01";
        
        [Header("Weapon Type")] 
        public WeaponType weaponType;
        
        [Header("Damage")] 
        // TODO: tweak in game design process
        public int physicalDamage = 25;
        public int fireDamage;
        public int criticalDamageMultiplier = 4;

        [Header("Poise")] 
        public float poiseBreak;
        public float offensivePoiseBonus;
        
        [Header("Absorption")] 
        public float physicalDamageAbsorption;
        
        [Header("Idle Animations")] 
        public string right_hand_idle;
        public string left_hand_idle;
        public string two_hand_idle;

        [Header("Stamina Costs")] 
        public int baseStamina;
        public float lightAttackMultipler;
        public float heavyAttackMultiplier;
    }
}