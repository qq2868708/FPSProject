using UnityEngine;
using System.Collections;

/// <summary>
/// 控制射击声音
/// </summary>
public class FireArmListener : MonoBehaviour
{

    public FPSFireArmAudio audios;
    [SerializeField]
    private FireArmAudio audioClips;

    public AudioSource audioSource;

    private string dataPath;

    private void Awake()
    {
        dataPath = "ScriptableObjects/FPS Fire Arm Audio";
        audios =Resources.Load<FPSFireArmAudio>(dataPath);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
            audioSource.clip = audioClips.reloadLeft;
            audioSource.Play();
        }
        else if (audioType == "reloadOutof")
        {
            audioSource.clip = audioClips.reloadOutof;
            audioSource.Play();
        }
        else if (audioType == "shoot")
        {
            audioSource.clip = audioClips.shootAudio;
            audioSource.Play();
        }

    }

}
