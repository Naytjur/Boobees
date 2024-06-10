using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource buttonSound;
    public AudioSource placingSound;

    public void Start()
    {
        BuildManager.onBuildingPlaced += OnBuildingPlaced;
        PlantingManager.instance.onPlantPlanted += OnPlantPlaced;
    }
    public void PlaySound()
    {
        buttonSound.Play(0);
    }

    public void OnBuildingPlaced()
    {
        placingSound.Play(0);
    }

    public void OnPlantPlaced()
    {
        placingSound.Play(0);
    }
}
