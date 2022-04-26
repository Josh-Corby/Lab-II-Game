using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : GameBehaviour
{
    public AudioSource SFX;
    public AudioClip buttonSound;

    public void PlayButtonSound()
    {
        SFX.clip = buttonSound;
        SFX.Play();
    }


}
