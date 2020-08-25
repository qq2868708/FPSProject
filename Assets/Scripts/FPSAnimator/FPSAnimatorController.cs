using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///利用角色状态对动画机进行维护
/// </summary>
public class FPSAnimatorController : MonoBehaviour
{
    public Animator playerAnimator;
    private string CurrentAnimation;
    private FPSController_CharacterController controller;
    private IWeapon weapon;

    public IWeapon Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }
    }

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
                    //奔跑时可以打断当前正在进行的动画
                    this.PlayAnimation(AnimationSettings.run);
                    weapon.CancelCurrent();
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

    public void SetTrigger(string animName,bool cancel)
    {
        playerAnimator.SetTrigger(animName);
        if(cancel)
        {

        }
    }

    private void Update()
    {
        this.PlayAnimation(controller.state);
    }
}
