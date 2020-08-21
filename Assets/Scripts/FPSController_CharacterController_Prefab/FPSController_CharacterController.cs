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
    public float moveSpeed;

    public float gravity;

    public Vector3 moveDir;
    public float jumpHeight;

    public float sprintSpeed;
    public float walkSpeed;

    //记录前一帧的状态参量
    private float lastSpeed;
    private float Hor;
    private float Ver;
    private float Jump;
    private Vector3 lastMoveDir;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        player = this.transform;
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            Jump = 0;
            moveDir.y = -1;
            Hor = Input.GetAxis(InputSettings.Hor);
            Ver = Input.GetAxis(InputSettings.Ver);

            //不知道为什么这个会实时更新，即使不落地也会更新movedir的值，所以需要另一个中间变量lastMovdDir记录跳起之前的移动向量
            moveDir = player.TransformDirection(new Vector3(Hor, 0, Ver));

            lastMoveDir = moveDir;
            //无重力算法
            //characterController.Move(moveDir * Time.deltaTime * moveSpeed);
            //自带重力算法
            //characterController.SimpleMove(tmp_MoveDir * Time.deltaTime * MoveSpeed);
            //Debug.Log(moveDir);
            if (Input.GetKeyDown(InputSettings.Jump))
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
        }
        moveDir = lastMoveDir * lastSpeed * Time.deltaTime;
       
        Jump-= gravity * Time.deltaTime;
        moveDir.y =Jump;
        characterController.Move(moveDir);
    }
}
