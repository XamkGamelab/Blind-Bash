using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReference : MonoBehaviour
{

    [HideInInspector]
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
    }
}
