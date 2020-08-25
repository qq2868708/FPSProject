using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

[CreateAssetMenu(menuName = "FPS/FPSImpactAudioData")]
public class FPSImpactAudio : ScriptableObject
{
    public List<ImpactAudio> impactAudio = new List<ImpactAudio>();
}

[System.Serializable]
public class ImpactAudio
{
    public string tag;
    public List<AudioClip> audioClips;
    public float interval;
}
