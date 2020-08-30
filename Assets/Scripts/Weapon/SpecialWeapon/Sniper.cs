using UnityEngine;
using System.Collections;

public class Sniper : FireArm
{
    private FPSMouseLook mouseLook;

    //大狙有个单独的声音，即每次开枪以后的上膛声音，后期再考虑另外配置
    public AudioClip clip;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        listener.SetAudio(weapn_Name);
        mouseLook = FindObjectOfType<FPSMouseLook>();
    }


    public override void Shooting()
    {
        if (currentAmmoInMag <= 0)
        {
            return;
        }
        if (IsAllowShoot())
        {
            muzzleParticle.Play();
            casingParticle.Play();
            currentAmmoInMag -= 1;
            //根据是否瞄准选择激活哪一个动画层
            controller.playerAnimator.Play(AnimationSettings.shootClip, isAiming?2:0, 0);
            CreatBullet();
            listener.PlayAudio("shoot");
            mouseLook.FiringForTest();
        }
    }

    public override void DoAttack()
    {
        if(isReady==true)
        {
            Shooting();
        }
    }

    //用于打断某些动画
    public override void CancelCurrent()
    {
        base.CancelCurrent();
        if(isReady)
        {
            controller.playerAnimator.SetLayerWeight(1, 0);
        }
    }

    //瞄准
    public override void Aim(bool aim)
    {
        isAiming = aim;
        if (isAiming)
        {
            controller.playerAnimator.SetLayerWeight(2, 1);
            controller.playerAnimator.SetBool(AnimationSettings.aim, isAiming);
        }
        else
        {
            controller.playerAnimator.SetBool(AnimationSettings.aim, isAiming);
            
        }
        StartCoroutine(Scale());
    }

    //相机缩放
    public IEnumerator Scale()
    {
        float tmp_vel = 0.1f;
        while(true)
        {
            yield return null;
            if (isAiming)
            {
                eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, originalFOV - 26, ref tmp_vel, 0.8f);
            }
            else
            {
                eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, originalFOV,  ref tmp_vel, 0.8f);
            }
        }
        
    }

    //动画事件，设置层的权重
    public void SetLayer(int weight)
    {
        controller.playerAnimator.SetLayerWeight(2, weight);
    }

    public override void Reload()
    {
        if (currentAmmoCarried <= 0)
        {
            return;
        }
        else if (currentAmmoInMag == ammoInMag)
        {
            return;
        }
        if (isReady)
        {
            controller.playerAnimator.SetLayerWeight(2, 0);
            controller.playerAnimator.SetLayerWeight(1, 1);
            //大狙一发子弹装填一次，所以包含多个音效
            controller.playerAnimator.SetTrigger(AnimationSettings.reloadOnce);
            listener.PlayAudio("reloadLeft");
        }
    }

    //大狙装填声音
    public void PlayInsert()
    {
        listener.audioSource.clip = clip;
        listener.audioSource.Play();
    }

    public void PlayClose()
    {
        listener.PlayAudio("reloadOutof");
    }
}
