using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    

public class IllusionaryDoor : MonoBehaviour
{
    public bool doorHasBeenHit;
    public Material illusionaryDoorMaterial;
    public float alpha;
    public float fadeTimer = 2.5f;

    public BoxCollider doorCollider;
    public AudioSource audioSource;
    public AudioClip illusionaryDoorSound;

    private void Awake()
    {
        doorCollider = GetComponent<BoxCollider>();
        illusionaryDoorMaterial = GetComponent<Material>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (doorHasBeenHit)
        {
            FadeIllusionaryDoor();
        }
    }

    public void FadeIllusionaryDoor()
    {
        alpha = illusionaryDoorMaterial.color.a;
        alpha = alpha - Time.deltaTime / fadeTimer;
        // color will keep changing over time, only chage transperancy
        Color fadeDoorColor = new Color(1, 1, 1, alpha);
        illusionaryDoorMaterial.color = fadeDoorColor;

        if (doorCollider.enabled)
        {
            doorCollider.enabled = false;
            audioSource.PlayOneShot(illusionaryDoorSound);
        }

        // detroy object when its faded
        if (alpha <= 0)
        {
            Destroy(this);
        }
    }
}
}