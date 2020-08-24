using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

[CreateAssetMenu(menuName = "FPS/FootAudioData")]
public class FPSFootAudio : ScriptableObject
{
    public List<FootStepAudio> footStepAudios = new List<FootStepAudio>();
}

[System.Serializable]
public class FootStepAudio
{
    public string tag;
    public List<AudioClip> audioClips;
    public float interval;
}
