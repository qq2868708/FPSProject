﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FireArmListener))]
[RequireComponent(typeof(FPSAnimatorController))]
public abstract class FireArm : MonoBehaviour,IWeapon
{
    public string weapn_Name;

    //枪口特效
    [SerializeField]
    protected Transform muzzlePoint;
    [SerializeField]
    protected ParticleSystem muzzleParticle;

    //抛壳特效
    [SerializeField]
    protected Transform casingPoint;
    [SerializeField]
    protected ParticleSystem casingParticle;

    //弹夹配置
    public int ammoInMag = 30;
    public int MaxAmmorCarried = 120;

    public int currentAmmoInMag;
    public int currentAmmoCarried;

    //攻击速率
    public float FireRate;
    //控制射击间隔
    public float time;

    //攻击力
    public int damage;

    //射击的后坐力
    public Vector2 recoil;

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

    //弹道散射角度，非0值，0值报错，做除数
    public float SpreadAngle;

    //武器挂载点
    public Transform weaponHolder;

    //该手臂对应的武器，是一个可以在场景中显示的可以捡起的道具
    public GameObject prefab;

    //配件挂载点
    public Transform attachmentsHolder;
    //配件相机
    public Camera gunCamera;
    public Vector3 originPosition;

    //跟武器对应的图标
    public Sprite sprite;


    //如果使用Instantiate方法，必须写在Awake里，有严格的调用时机的区别，instantiate中awake会立刻执行，而start会延迟执行，导致有些赋值无效
    private void Awake()
    {
        currentAmmoInMag = ammoInMag;
        currentAmmoCarried = MaxAmmorCarried;
    }

    protected virtual void Start()
    {
        controller = GetComponent<FPSAnimatorController>();
        controller.Weapon = this;
        listener = GetComponent<FireArmListener>();
        eyeCamera = TransformHelper.FindChild(weaponHolder, "Main Camera").GetComponent<Camera>();
        originalFOV = eyeCamera.fieldOfView;

        muzzlePoint = TransformHelper.FindChild(this.transform, "MuzzlePoint");
        muzzleParticle = TransformHelper.FindChild(this.transform, "Muzzle").GetComponent<ParticleSystem>();
        casingPoint = TransformHelper.FindChild(this.transform, "Casing Spawn Point");
        casingParticle= TransformHelper.FindChild(this.transform, "Casing").GetComponent<ParticleSystem>();
        //获取相机的位置
        attachmentsHolder = TransformHelper.FindChild(this.transform, "Attachments");
        gunCamera = TransformHelper.FindChild(weaponHolder, "Gun Camera").GetComponent<Camera>();
        originPosition = gunCamera.transform.localPosition;
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

    public abstract void Aim(bool aim);

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
        var tmp_obj = GameObjectPool.instance.CreateObject("Bullet",bullet);
        tmp_obj.transform.position = muzzlePoint.position;
        tmp_obj.transform.rotation = muzzlePoint.rotation;
        tmp_obj.transform.eulerAngles += CaculateSpreadBullet();
        tmp_obj.GetComponent<Bullet>().damage = this.damage;
        tmp_obj.SetActive(true);
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
            currentAmmoCarried = currentAmmoCarried + currentAmmoInMag-ammoInMag;
            currentAmmoInMag = ammoInMag;
            
        }
        else if (currentAmmoCarried != 0)
        {
            currentAmmoInMag = currentAmmoCarried;
            currentAmmoCarried = 0;
        }
        controller.playerAnimator.SetLayerWeight(1, 0);
    }

    public virtual IEnumerator SwapWeapon()
    {
        for(int i=0; i< controller.playerAnimator.layerCount; i++)
        {
            controller.playerAnimator.SetLayerWeight(i, 0);
        }
        controller.playerAnimator.SetLayerWeight(0, 1);
        isAiming = false;
        isReady = false;
        controller.playerAnimator.SetTrigger(AnimationSettings.holster);
        yield return info();
    }

    private IEnumerator info()
    {
        while (true)
        {
            yield return null;
            AnimatorStateInfo tmp_AnimatorStateInfo = controller.playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (tmp_AnimatorStateInfo.normalizedTime > 0.9f)
            {
                isReady = true;
                break;
            }
        }
    }
}
