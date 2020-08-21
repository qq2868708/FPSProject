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
    private float crouchedHeight;

    //重力
    public float gravity;

    public Vector3 moveDir;
    public float jumpHeight;

    public float sprintSpeed;
    public float walkSpeed;

    //记录前一帧的状态参量
    private float lastSpeed;
    private float hor;
    private float ver;
    private float Jump;
    private Vector3 lastMoveDir;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        player = this.transform;
        standHeight = characterController.height;
        crouchedHeight = 1;
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            Jump = 0;
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
            if (Input.GetKey(InputSettings.Jump)&&!isCrouched)
            {
                Jump = Mathf.Sqrt(2 * gravity * jumpHeight);
                Debug.Log("jump"+moveDir);
            }
            if (Input.GetKeyDown(InputSettings.Sprint))
            {
                lastSpeed = sprintSpeed;
            }
            else
            {
                lastSpeed = walkSpeed;
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
       
        Jump-= gravity * Time.deltaTime;
        moveDir.y =Jump;
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
            characterController.center=new Vector3(0, Mathf.Lerp(characterController.center.y,tmp_height,0.5f), 0);
            //characterController.center=new Vector3(0, 0.5f, 0);
        }
    }
}
