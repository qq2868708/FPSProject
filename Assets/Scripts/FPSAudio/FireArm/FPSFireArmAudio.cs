using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "FPS/FireArmAudio")]
public class FPSFireArmAudio : ScriptableObject
{
    public List<FireArmAudio> fireArmAudio = new List<FireArmAudio>();
}

[System.Serializable]
public class FireArmAudio
{
    public string name;
    public AudioClip shootAudio;
    public AudioClip reloadLeft;
    public AudioClip reloadOutof;
    public float interval;
}