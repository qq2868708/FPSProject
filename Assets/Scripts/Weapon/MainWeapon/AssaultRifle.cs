using UnityEngine;
using System.Collections;

public class AssaultRifle : FireArm
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
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
        if(currentAmmoCarried>=ammoInMag)
        {
            currentAmmoInMag = ammoInMag;
            currentAmmoCarried -= ammoInMag;
        }
        else
        {
            currentAmmoInMag = currentAmmoCarried;
            currentAmmoCarried = 0;
        }
    }

    protected override void Shooting()
    {
        gunAnimator.Play(AnimationSettings.shootClip, 0, 0);
    }

    public override void DoAttack()
    {
        base.DoAttack();
        if (currentAmmoInMag <= 0)
        {
            return;
        }
        if (IsAllowShoot())
        {
            currentAmmoInMag -= 1;
            Shooting();
            CreatBullet();
        }
    }
}
