using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    


public class DestroyAfterCastingSpell : MonoBehaviour
{
    private CharacterManager characterCastingSpell;
    
    private void Awake()
    {
        characterCastingSpell = GetComponentInParent<CharacterManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if(characterCastingSpell != null && characterCastingSpell.isFiringSpell){
            Destroy(gameObject);
    }
}
}
}