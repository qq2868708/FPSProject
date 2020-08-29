using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///玩家移动控制
/// </summary>
public class FPSMoveMent : MonoBehaviour
{
    private Rigidbody player;
    //玩家移动速度
    public float speed;
    //是否落地
    private bool isGrounded;
    //跳跃高度
    public float jumpHeight;
    //使用重力
    public float gravity;

 

    //临时分量
    private float tmp_Hor;
    private float tmp_Ver;

    private void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        if(isGrounded)
        {
            //获取键盘移动分量
            tmp_Hor = Input.GetAxis(InputSettings.Hor);
            tmp_Ver = Input.GetAxis(InputSettings.Ver);

            var tmp_Dir = new Vector3(tmp_Hor, 0, tmp_Ver);
            tmp_Dir = this.transform.TransformDirection(tmp_Dir);
            tmp_Dir *= speed*Time.deltaTime;
            var tmp_Speed = player.velocity;
            var tmp_SpeedChange = tmp_Dir - tmp_Speed;
            tmp_SpeedChange.y = 0;
            //ForceMode指定直接进行速度操作而忽略质量
            player.AddForce(tmp_SpeedChange, ForceMode.VelocityChange);
            if(Input.GetKeyDown(InputSettings.Jump))
            {
                var tmp_Jump = Mathf.Sqrt(2 * gravity * jumpHeight);
                var tmp_JumpDir = new Vector3(0, tmp_Jump, 0);
                player.AddForce(tmp_JumpDir, ForceMode.VelocityChange);
            }

            
        }
        player.AddForce(new Vector3(0, -gravity*player.mass, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
