using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{

    public AudioSource clickSound;
    public AudioSource plantingSound;
    public void ButtonSound ()
    {
        clickSound.Play(0);

    }
}
