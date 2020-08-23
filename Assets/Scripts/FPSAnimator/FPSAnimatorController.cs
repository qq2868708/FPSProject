using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class FPSAnimatorController : MonoBehaviour
{
    private Animator playerAnimator;
    private string CurrentAnimation;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        CurrentAnimation = playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public void PlayAnimation(string AnimName)
    {
        playerAnimator.SetBool(CurrentAnimation, false);
        CurrentAnimation = AnimName;
        playerAnimator.SetBool(AnimName,true);
    }
}
