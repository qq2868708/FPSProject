using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 后坐力
/// </summary>

public class FPSMouseLook : MonoBehaviour
{
    public float lookSpeed;

    private float inputTempX;
    private float inputTempY;

    private Transform Mousecamera;
    private Vector3 cameraRotation;
    public Vector2 LimitY;
    public Transform player;

    //后坐力
    //曲线用于模拟对镜头造成的上抬效果的影响
    public AnimationCurve recoilCurve;
    public Vector2 recoilRange;
    private float currentRecoilTime;
    private Vector2 currentRecoil;
    public float recoilFadeOutTime=0.3f;
    
    public CameraShakeController cameraShakeController;
    //用于场景管理器初始化位置
    public Quaternion playerQuaternion;

    //场景管理器，用于处理场景的输入
    public LevelManager instance;

    private void Start()
    {
        Mousecamera = this.gameObject.transform;
        cameraShakeController = GetComponentInChildren<CameraShakeController>();
        this.transform.rotation = playerQuaternion;
        instance = LevelManager.instance;
    }

    private void Update()
    {
        if(instance.gameStart)
        {
            inputTempX = Input.GetAxis(InputSettings.MouseX) * lookSpeed * Time.deltaTime;
            inputTempY = Input.GetAxis(InputSettings.MouseY) * lookSpeed * Time.deltaTime;

            //下面会让两个旋转被混合
            //transform.rotation *= Quaternion.Euler(inputTempX, inputTempY, 0);

            cameraRotation.x += inputTempX;
            cameraRotation.y -= inputTempY;

            CaculateRecoilTime();

            cameraRotation.x += currentRecoil.x;
            cameraRotation.y -= currentRecoil.y;

            cameraRotation.y = Mathf.Clamp(cameraRotation.y, LimitY.x, LimitY.y);

            Mousecamera.rotation = playerQuaternion * Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);
            player.rotation = playerQuaternion * Quaternion.Euler(0, cameraRotation.x, 0);
        }
    }

    private void CaculateRecoilTime()
    {
        //根据时间计算一个值
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / recoilFadeOutTime;
        float tmp_RecoilValue = recoilCurve.Evaluate(tmp_RecoilFraction);

        //后坐力慢慢趋近于0
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
    }

    public void FiringForTest()
    {
        //产生一个瞬时后坐力，然后跟随曲线进行衰减
        currentRecoil += recoilRange;
        //每次射击时清0，不然曲线超过范围将不会发挥作用
        currentRecoilTime = 0;

        cameraShakeController.StartSpring();
    }
        
}
