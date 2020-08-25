using UnityEngine;
using System.Collections;

/// <summary>
/// 控制脚步声音
/// </summary>
public class FireArmListener : MonoBehaviour
{

    public FPSFireArmAudio audios;
    [SerializeField]
    private FireArmAudio audioClips;

    public AudioSource audioReload;
    public AudioSource audioShoot;

    public void SetAudio(string audioName)
    {
        audioClips = null;
        foreach(var tmp_Clips in audios.fireArmAudio)
        {
            if(tmp_Clips.name==audioName)
            {
                audioClips = tmp_Clips;
                break;
            }
        }
    }

    public void PlayAudio(string audioType)
    {
        if(audios==null)
        {
            return;
        }
        if(audioType=="reloadLeft")
        {
            audioReload.clip = audioClips.reloadLeft;
            audioReload.Play();
        }
        else if (audioType == "reloadOutof")
        {
            audioReload.clip = audioClips.reloadOutof;
            audioReload.Play();
        }
        else if (audioType == "shoot")
        {
            audioShoot.clip = audioClips.shootAudio;
            audioShoot.Play();
        }

    }

}
