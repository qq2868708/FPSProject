using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class FPSController_CharacterController : MonoBehaviour
{
    private CharacterController characterController;
    private Transform player;
    //下蹲状态记录
    private bool isCrouched=false;
    private float standHeight;
    public float crouchedHeight;

    //重力
    public float gravity;

    public Vector3 moveDir;
    public float jumpHeight;

    public float sprintSpeed;
    public float crouchedSprint;
    public float walkSpeed;
    public float crouchedWalk;

    //记录前一帧的状态参量
    private float lastSpeed;
    private float hor;
    private float ver;
    private float jump;
    private Vector3 lastMoveDir;

    //动画机
    private FPSAnimatorController animatorController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        player = this.transform;
        standHeight = characterController.height;
        animatorController=GetComponentInChildren<FPSAnimatorController>();
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            jump = 0;
            moveDir.y = -1;
            hor = Input.GetAxis(InputSettings.Hor);
            ver = Input.GetAxis(InputSettings.Ver);

            //不知道为什么这个会实时更新，即使不落地也会更新movedir的值，所以需要另一个中间变量lastMovdDir记录跳起之前的移动向量
            moveDir = player.TransformDirection(new Vector3(hor, 0, ver));

            lastMoveDir = moveDir;
            //无重力算法
            //characterController.Move(moveDir * Time.deltaTime * moveSpeed);
            //自带重力算法
            //characterController.SimpleMove(tmp_MoveDir * Time.deltaTime * MoveSpeed);
            //Debug.Log(moveDir);

            //蹲下时不允许起跳
            if (Input.GetKey(InputSettings.Jump)&&!isCrouched)
            {
                jump = Mathf.Sqrt(2 * gravity * jumpHeight);
            }

            

            //蹲下时有不同的移动速度
            if (Input.GetKey(InputSettings.Sprint))
            {
                if(isCrouched)
                {
                    lastSpeed = crouchedSprint;
                }
                else
                {
                    animatorController.PlayAnimation(AnimationSettings.run);
                    lastSpeed = sprintSpeed;
                }
               
            }
            else
            {
                if (isCrouched)
                {
                    lastSpeed = crouchedWalk;
                }
                else
                {
                    animatorController.PlayAnimation(AnimationSettings.walk);
                    lastSpeed = walkSpeed;
                }
                
            }

            //如果没有移动幅度太小，则判断为站立不动，动画效果可以使用一个速度变量来进行控制，配合blendtree可以达到更好的效果，但是觉得如果使用速度来控制的话在实际操作的时候不方便对移动速度的数值进行修改了
            if (new Vector2(moveDir.x, moveDir.y).magnitude < 0.1f)
            {
                animatorController.PlayAnimation(AnimationSettings.idle);
            }

            //实现下蹲的方法，同时模型坐标不变，不会陷进地面而导致动画出现问题
            //if(Input.GetKeyDown(InputSettings.Crouched))
            //{
            //    isCrouched = !isCrouched;
            //    if(isCrouched)
            //    {
            //        var tmp = characterController.center.y - (characterController.height - crouchedHeight) / 2f;
            //        characterController.height = crouchedHeight;
            //        characterController.center = new Vector3(0f, tmp, 0f);
            //    }
            //    else
            //    {
            //        var tmp = characterController.center.y - (characterController.height - standHeight) / 2f;
            //        characterController.height =standHeight;
            //        characterController.center = new Vector3(0f, tmp, 0f);
            //    }

            //}

            //用协程实现平滑下蹲和站立
            if (Input.GetKeyDown(InputSettings.Crouched))
            {
                isCrouched = !isCrouched;
                if(isCrouched)
                {
                    StartCoroutine("Crouch", crouchedHeight);
                }
                else
                {
                    StartCoroutine("Crouch", standHeight);
                }
                
            }

        }

        moveDir = lastMoveDir * lastSpeed * Time.deltaTime;
       
        jump-= gravity * Time.deltaTime;
        moveDir.y =jump;
        characterController.Move(moveDir);
    }

    //用协程实现平滑下蹲
    public IEnumerator Crouch(float targetHeight)
    {
        var tmp_height = characterController.height - targetHeight;
        tmp_height =characterController.center.y - tmp_height/2;
        Debug.Log(tmp_height);
        while(Mathf.Abs(characterController.height-targetHeight)>0.1)
        {
            yield return null;
            characterController.height = Mathf.Lerp(characterController.height, targetHeight, 0.5f);
            //下面这行会使得模型的位置保持不变，对于拥有完整模型的场合，可以导致双脚稳稳站在地上，对于仅拥有手臂而没有站立动画的场合，请注释
            //characterController.center=new Vector3(0, Mathf.Lerp(characterController.center.y,tmp_height,0.5f), 0);
        }
    }
}
