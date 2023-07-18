using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    public class EquipmentModelChanger : MonoBehaviour
    {
        public List<GameObject> bodyPartModels;

        private void Awake()
        {
            GetAllBodyPartModels();
        }

        private void GetAllBodyPartModels()
        {
            int childrenGameObjects = transform.childCount;
            for (int i = 0; i < childrenGameObjects; i++)
            {
                bodyPartModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllBodyPartModels()
        {
            foreach (GameObject torsoModel in bodyPartModels)
            {
                torsoModel.SetActive(false);
            }
        }

        public void EquipBodyPartModelByName(string bodyPartName)
        {
            for (int i = 0; i < bodyPartModels.Count; i++)
            {
                if (bodyPartModels[i].name == bodyPartName)
                {
                    bodyPartModels[i].SetActive(true);
                }                
            }
        }
    }
}