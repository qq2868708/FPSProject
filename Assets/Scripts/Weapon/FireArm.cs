using UnityEngine;
using System.Collections;

public abstract class FireArm : MonoBehaviour,IWeapon
{
    //枪口特效
    public Transform muzzlePoint;
    public ParticleSystem muzzleParticle;

    //抛壳特效
    public Transform casingPoint;
    public ParticleSystem casingParticle;

    //弹夹配置
    public int ammoInMag = 30;
    public int MaxAmmorCarried = 120;

    public int currentAmmoInMag;
    public int currentAmmoCarried;

    public float FireRate;
    public float time;

    public Animator gunAnimator;

    //是否在换弹
    public bool isReloading=false;

    public GameObject bullet;

    protected Animator GunAnimator;

    protected virtual void Start()
    {
        currentAmmoInMag = ammoInMag;
        currentAmmoCarried = MaxAmmorCarried;
        gunAnimator = GetComponent<Animator>();
    }

    public virtual void DoAttack()
    {
        
    }

    protected abstract void Shooting();

    protected abstract void Reload();

    public bool IsAllowShoot()
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
        tmp_obj.GetComponent<Rigidbody>().velocity = tmp_obj.transform.forward * 100;
    }
}
