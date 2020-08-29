using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FireArmListener))]
[RequireComponent(typeof(FPSAnimatorController))]
public abstract class FireArm : MonoBehaviour,IWeapon
{
    public string weapn_Name;

    //枪口特效
    private Transform muzzlePoint;
    protected ParticleSystem muzzleParticle;

    //抛壳特效
    private Transform casingPoint;
    protected ParticleSystem casingParticle;

    //弹夹配置
    public int ammoInMag = 30;
    public int MaxAmmorCarried = 120;

    public int currentAmmoInMag;
    public int currentAmmoCarried;

    public float FireRate;
    public float time;

    //使用统一的动画机接口
    public FPSAnimatorController controller;

    //是否可以射击
    public bool isReady=false;
    //子弹的预制件
    public GameObject bullet;

    //声音组件
    public FireArmListener listener;

    //相机缩放
    public float originalFOV;
    public Camera eyeCamera;

    public bool isAiming;

    //弹道散射角度
    public float SpreadAngle;

    protected virtual void Start()
    {
        currentAmmoInMag = ammoInMag;
        currentAmmoCarried = MaxAmmorCarried;
        controller = GetComponent<FPSAnimatorController>();
        controller.Weapon = this;
        listener = GetComponent<FireArmListener>();
        eyeCamera = TransformHelper.FindChild(this.transform, "Main Camera").GetComponent<Camera>();
        originalFOV = eyeCamera.fieldOfView;

        muzzlePoint = TransformHelper.FindChild(this.transform, "MuzzlePoint");
        muzzleParticle = TransformHelper.FindChild(this.transform, "Muzzle").GetComponent<ParticleSystem>();
        casingPoint = TransformHelper.FindChild(this.transform, "Casing Spawn Point");
        casingParticle= TransformHelper.FindChild(this.transform, "Casing").GetComponent<ParticleSystem>();
    }

    //公开的抽象接口
    public abstract void DoAttack();

    public abstract void Shooting();

    public virtual void Reload()
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
            if (currentAmmoInMag <= 0)
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

    public abstract void Aim();

    public virtual bool IsAllowShoot()
    {
        if(Time.time>time)
        {
            time = Time.time + 60f / FireRate;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CreatBullet()
    {
        var tmp_obj = GameObjectPool.instance.CreatObject("Bullet",bullet);
        tmp_obj.transform.position = muzzlePoint.position;
        tmp_obj.transform.rotation = muzzlePoint.rotation;
        //stmp_obj.transform.eulerAngles += CaculateSpreadBullet();
        tmp_obj.GetComponent<Rigidbody>().velocity = tmp_obj.transform.forward * 100;
    }

    public virtual void CancelCurrent()
    {

    }

   

    //根据相机的缩放产生一个散布，相机fov越大，散布越小
    protected Vector3 CaculateSpreadBullet()
    {
        float tmp_SpreadPercent = eyeCamera.fieldOfView/SpreadAngle;
        return tmp_SpreadPercent * Random.insideUnitCircle;
    }

    //动画事件，设置是否可以射击
    public void SetState(int state)
    {
        if (state == 1)
        {
            isReady = false;
        }
        else
        {
            isReady = true;
        }
    }

    public void ReloadAmmo()
    {
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
        controller.playerAnimator.SetLayerWeight(1, 0);
    }
}
