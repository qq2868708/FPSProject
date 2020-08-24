using UnityEngine;
using System.Collections;

/// <summary>
/// 控制脚步声音
/// </summary>
public class FootStepListener : MonoBehaviour
{
    //角色控制器
    public CharacterController player;
    public AudioSource audioSource;
    public FPSFootAudio audioClips;

    //记录音量
    private float volume;

    private float time;
    //用于获得角色的控制信息
    private FPSController_CharacterController controller;

    // Use this for initialization
    void Start()
    {
        controller = this.GetComponent<FPSController_CharacterController>();
        volume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        //在地面上才有脚步声
        if(player.isGrounded)
        {
            //判断运动状态，只有走和跑有脚步
            if (controller.state !=CharacterState.idle)
            {
                //检测一下地面的性质
                if (Physics.Raycast(this.transform.position + player.center, Vector3.down, out RaycastHit hit, player.height / 2 + 0.5f))
                {
#if UNITY_EDITOR
                    Debug.DrawLine(this.transform.position + player.center, this.transform.position + player.center + new Vector3(0, -1, 0) * (player.height / 2 + 0.5f), Color.red);
#endif
                    //根据地面性质选择一种声音
                    foreach (var tmp in audioClips.footStepAudios)
                    {
                        if (Time.time > time && hit.collider.tag == tmp.tag)
                        {

                            //int tmp_Index = UnityEngine.Random.Range(0, tmp.audioClips.Count);
                            int tmp_Index = 0;
                            AudioClip clip = tmp.audioClips[tmp_Index];
                            audioSource.clip = clip;

                            //根据运动状态设置不同的声音大小和声音间隔
                            if (controller.state == CharacterState.run)
                            {
                                audioSource.volume = 1.5f * volume;
                                time = Time.time + clip.length-tmp.interval;
                                audioSource.Play();
                            }
                            else if (controller.state == CharacterState.walk)
                            {
                                audioSource.volume = volume;
                                time = Time.time + clip.length + tmp.interval;
                                audioSource.Play();
                            }
                            else if(controller.state==CharacterState.crouched_Run)
                            {
                                audioSource.volume = 0.75f * volume;
                                time = Time.time + clip.length + tmp.interval*1.2f;
                                audioSource.Play();
                            }
                            else if (controller.state == CharacterState.crouched_Walk)
                            {
                                audioSource.volume = 0.5f * volume;
                                time = Time.time + clip.length + tmp.interval * 1.4f;
                                audioSource.Play();
                            }
                        }
                    }
                }

            }
        }
    }
}
