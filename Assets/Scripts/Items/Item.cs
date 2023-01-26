using UnityEngine;

namespace CW
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")] public Sprite itemIcon;

        public string itemName;
        public GameObject itemModel;
    }
}