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
        if(Input.GetMouseButton(0)&&isReloading!=true)
        {
            DoAttack();
        }

        if(Input.GetKeyDown(InputSettings.Reload))
        {
            if(currentAmmoInMag<ammoInMag)
            {
                if(currentAmmoCarried!=0)
                {
                    if(!isReloading)
                    {
                        Reload();
                        isReloading = true;
                    }
                }
            }
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
        isReloading = false;
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
            controller.playerAnimator.Play(AnimationSettings.shootClip, 0, 0);
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
        if(isReloading)
        {
            controller.playerAnimator.SetLayerWeight(1, 0);
            isReloading = false;
        }
    }
}
