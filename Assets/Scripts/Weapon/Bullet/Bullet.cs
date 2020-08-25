using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private AudioClip clip;
    public FPSImpactAudio audioClips;
    private ImpactListener listener;

    //特效组件
    public GameObject bulletEffect;

    private void Start()
    {
        instance = GameObjectPool.instance;
        old_Pos = this.transform.position;
        origin_Pos = this.transform.position;
        listener = GetComponent<ImpactListener>();
    }

    private void Update()
    {
        //两帧的位置创建射线来检测碰撞
        Ray ray = new Ray(old_Pos, this.transform.position - old_Pos);
        if(Physics.Raycast(ray,out RaycastHit hit))
        {
            //防止和自己碰到
            if(hit.collider.gameObject!=this.gameObject)
            {
                //产生特效并在一定时间后让对象池回收
                var tmp = instance.CreatObject("BulletEffect", bulletEffect);
                tmp.transform.rotation = Quaternion.LookRotation(hit.normal);
                tmp.transform.position = hit.point;
                instance.CollectGameObject(tmp, 2);

                //产生声音
                if (listener != null)
                {
                    Debug.Log(1);
                    listener.Play(hit.transform.position);
                }

                //回收子弹
                instance.CollectGameObject(this.gameObject);
            }
        }
        //如果超过射程则销毁
        if (Vector3.Distance(this.transform.position, origin_Pos) > 200)
        {
            instance.CollectGameObject(this.gameObject);
        }
    }
}
