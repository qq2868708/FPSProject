using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
namespace FPSProject.Character
{
    public class CharacterAnimation : MonoBehaviour
    {
        private Animator anim;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        string AnimName = "idle";

        public void PlayAnimation(string animName)
        {
            anim.SetBool(AnimName, false);
            anim.SetBool(animName, true);
            AnimName = animName;
        }
    }
}