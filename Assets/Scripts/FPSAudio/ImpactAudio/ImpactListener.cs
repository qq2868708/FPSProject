using UnityEngine;
using System.Collections;

/// <summary>
/// 控制特效撞击的声音
/// </summary>
public class ImpactListener : MonoBehaviour
{
    
    public AudioSource audioSource;
    public FPSImpactAudio audioClips;

    public AudioClip clip;

    //记录音量
    private float volume;

    private float time;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //在指定位置播放声音
    public void Play(Vector3 pos)
    {
        foreach(var tmp in audioClips.impactAudio)
        {
            if(this.tag==tmp.tag)
            {
                clip = tmp.audioClips[0];
            }
        }
        if(clip==null)
        {
            clip = audioClips.impactAudio[0].audioClips[0];
        }
        AudioSource.PlayClipAtPoint(clip, pos);
    }
}
