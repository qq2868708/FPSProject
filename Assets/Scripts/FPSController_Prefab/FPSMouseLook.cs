using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

public class FPSMouseLook : MonoBehaviour
{
    public float lookSpeed;

    private float inputTempX;
    private float inputTempY;

    private Transform camera;
    private Vector3 cameraRotation;
    public Vector2 LimitY;
    public Transform player;

    private void Start()
    {
        camera = this.gameObject.transform;
    }

    private void Update()
    {
        inputTempX = Input.GetAxis(InputSettings.MouseX) * lookSpeed*Time.deltaTime;
        inputTempY = Input.GetAxis(InputSettings.MouseY) * lookSpeed*Time.deltaTime;

        //下面会让两个旋转被混合
        //transform.rotation *= Quaternion.Euler(inputTempX, inputTempY, 0);

        cameraRotation.x += inputTempX;
        cameraRotation.y -= inputTempY;

        cameraRotation.y = Mathf.Clamp(cameraRotation.y, LimitY.x, LimitY.y);

        camera.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);
        player.rotation = Quaternion.Euler(0,cameraRotation.x, 0);
    }
}
