using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssaultRifle : FireArm
{
    private FPSMouseLook mouseLook;

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

    //相机缩放，加入倍镜功能
    public IEnumerator Scale()
    {
        float tmp_vel = 0.1f;
        ScopeItem scope=null;
        for (int i=0;i<attachmentsHolder.childCount;i++)
        {
            if (attachmentsHolder.GetChild(i).gameObject.activeSelf==true&&attachmentsHolder.GetChild(i).GetComponent<ScopeItem>()!=null)
            {
                scope=attachmentsHolder.GetChild(i).GetComponent<ScopeItem>();
                break;
            }
        }
        while(true)
        {
            yield return null;

            if(scope!=null)
            {
                if (isAiming)
                {
                    eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, originalFOV - 26-scope.eyeCameraFOV, ref tmp_vel, 0.3f);
                }
                else
                {
                    eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, originalFOV, ref tmp_vel, 0.3f);
                }
            }
            else
            {
                if (isAiming)
                {
                    eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, originalFOV - 26, ref tmp_vel, 0.3f);
                }
                else
                {
                    eyeCamera.fieldOfView = Mathf.SmoothDamp(eyeCamera.fieldOfView, originalFOV, ref tmp_vel, 0.3f);
                }
            }
            

            if(scope!=null)
            {
                if (isAiming)
                {
                    //gunCamera.transform.localPosition =new Vector3(gunCamera.transform.localPosition.x, Mathf.SmoothDamp(gunCamera.transform.localPosition.y, scope.gunTransform.y, ref tmp_vel, 0.8f), gunCamera.transform.localPosition.z);
                    gunCamera.transform.localPosition = new Vector3(gunCamera.transform.localPosition.x, scope.gunTransform.y, gunCamera.transform.localPosition.z);
                }
                else
                {
                    //gunCamera.transform.localPosition = new Vector3(gunCamera.transform.localPosition.x, Mathf.SmoothDamp(gunCamera.transform.localPosition.y, originPosition.y, ref tmp_vel, 0.8f), gunCamera.transform.localPosition.z);
                    gunCamera.transform.localPosition = new Vector3(gunCamera.transform.localPosition.x, originPosition.y, gunCamera.transform.localPosition.z);
                }
            }
        }
        
    }

    //动画事件，设置层的权重
    public void SetLayer(int weight)
    {
        controller.playerAnimator.SetLayerWeight(2, weight);
    }
}
