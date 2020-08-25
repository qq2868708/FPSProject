using UnityEngine;
using System.Collections;

public class AssaultRifle : FireArm
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        listener.SetAudio(weapn_Name);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)&& isReady != true)
        {
            DoAttack();
        }

        if(Input.GetKeyDown(InputSettings.Reload))
        {
            if(currentAmmoInMag<ammoInMag)
            {
                if(currentAmmoCarried!=0)
                {
                    if(!isReady)
                    {
                        Reload();
                        isReady = true;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Aim();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            Aim();
        }
    }

    protected override void Reload()
    {
        if(currentAmmoCarried<=0)
        {
            return;
        }
        
        else
        {
            controller.playerAnimator.SetLayerWeight(1,1);
            if (currentAmmoInMag<=0)
            {
                controller.playerAnimator.SetTrigger(AnimationSettings.reloadOutof);
                listener.PlayAudio("reloadOutof");
            }
            else
            {
                listener.PlayAudio("reloadLeft");
                controller.playerAnimator.SetTrigger(AnimationSettings.reloadLeft);
            }
        }
    }

    public void ReloadAmmo()
    {
        Debug.Log("reloading");
        if (currentAmmoCarried >= ammoInMag)
        {
            currentAmmoInMag = ammoInMag;
            currentAmmoCarried -= ammoInMag;
        }
        else if (currentAmmoCarried != 0)
        {
            currentAmmoInMag = currentAmmoCarried;
            currentAmmoCarried = 0;
        }
        isReady = false;
        controller.playerAnimator.SetLayerWeight(1, 0);
    }

    protected override void Shooting()
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
            controller.playerAnimator.Play(AnimationSettings.shootClip, isAiming?2:0, 0);
            //controller.SetTrigger(AnimationSettings.fire,false);
            CreatBullet();
            listener.PlayAudio("shoot");
        }
    }

    public override void DoAttack()
    {
        base.DoAttack();
        Shooting();
    }

    //用于打断某些动画
    public override void CancelCurrent()
    {
        base.CancelCurrent();
        if(isReady)
        {
            controller.playerAnimator.SetLayerWeight(1, 0);
            isReady = false;
        }
    }

    //瞄准
    public override void Aim()
    {
        base.Aim();
        isAiming = !isAiming;
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
                eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, originalFOV - 26, ref tmp_vel, Time.deltaTime);
            }
            else
            {
                eyeCamera.fieldOfView = Mathf.SmoothDamp(originalFOV, eyeCamera.fieldOfView,  ref tmp_vel, Time.deltaTime);
            }
        }
        
    }

    //动画事件，设置层的权重
    public void SetLayer(int weight)
    {
        controller.playerAnimator.SetLayerWeight(2, weight);
    }
    //动画事件，设置是否可以射击
    public void SetState(int state)
    {
        if(state==1)
        {
            isReady = true;
        }
        else
        {
            isReady = false;
        }
    }
    
}
