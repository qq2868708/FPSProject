using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///利用角色状态对动画机进行维护
/// </summary>
public class FPSAnimatorController : MonoBehaviour
{
    private Animator playerAnimator;
    private string CurrentAnimation;
    private FPSController_CharacterController controller;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        CurrentAnimation = playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        controller = GetComponentInParent<FPSController_CharacterController>();
    }

    public void PlayAnimation(string AnimName)
    {
        playerAnimator.SetBool(CurrentAnimation, false);
        CurrentAnimation = AnimName;
        playerAnimator.SetBool(AnimName,true);
    }

    public void PlayAnimation(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.idle:
                {
                    this.PlayAnimation(AnimationSettings.idle);
                    break;
                }
            case CharacterState.walk:
                {
                    this.PlayAnimation(AnimationSettings.walk);
                    break;
                }
            case CharacterState.run:
                {
                    this.PlayAnimation(AnimationSettings.run);
                    break;
                }
            case CharacterState.crouched_Idle:
                {
                    this.PlayAnimation(AnimationSettings.idle);
                    break;
                }
            default:
                {
                    this.PlayAnimation(AnimationSettings.walk);
                    break;
                }
        }
    }

    private void Update()
    {
        Debug.Log(controller.state);
        this.PlayAnimation(controller.state);
    }
}
