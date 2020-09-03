using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSProject.Character;

/// <summary>
///控制子弹的生命周期
/// </summary>
public class Bullet : MonoBehaviour
{
    //生命周期交给对象池管理
    public GameObjectPool instance;

    //记录上一帧的位置，由于子弹移动速度可能很快，远远超过碰撞检测的范围，所以使用与前一次形成的射线来检测碰撞
    private Vector3 old_Pos;
    //记录发射的位置，用于计算射程，从射出后保持不变
    private Vector3 origin_Pos;

    public FPSImpactAudio audioClips;
    //private ImpactListener listener;

    public int damage;

    //特效组件
    public GameObject bulletEffect;

    public LayerMask layer;

    private void Start()
    {
        instance = GameObjectPool.instance;
        old_Pos = this.transform.position;
        origin_Pos = this.transform.position;
    }

    private void Update()
    {
        //两帧的位置创建射线来检测碰撞
        Ray ray = new Ray(old_Pos, this.transform.position - old_Pos);
        if(Physics.Raycast(ray,out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag=="Monster")
            {
                if (hit.collider.gameObject.GetComponent<CharacterStatus>() != null)
                {
                    hit.collider.gameObject.GetComponent<CharacterStatus>().OnDamage(damage);
                }
            }
            if(hit.collider.gameObject!=this.gameObject&& hit.collider.gameObject.tag!="Player")
            {
                var tmp = instance.CreateObject("BulletEffect", bulletEffect);
                tmp.transform.rotation = Quaternion.LookRotation(hit.normal);
                tmp.transform.position = hit.point;
                tmp.SetActive(true);
                instance.CollectGameObject(tmp, 2);
                instance.CollectGameObject(this.gameObject);
            }
           
        }
        old_Pos = transform.position;
        //如果超过射程则销毁
        if (Vector3.Distance(this.transform.position, origin_Pos) > 200)
        {
            instance.CollectGameObject(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

       if( other.gameObject.tag == "Monster")
        {
            other.gameObject.GetComponent<CharacterStatus>().OnDamage(damage);
        }
    }
}
