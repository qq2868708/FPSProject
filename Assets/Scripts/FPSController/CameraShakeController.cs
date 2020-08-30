using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 屏幕抖动
/// </summary>
public class CameraShakeController : MonoBehaviour
{
    public Vector2 minRecoilRange;
    public Vector2 maxRecoilRange;

    public float frequence=25;
    public float damp=15;

    public CameraShaking cameraShaking;

    public Transform cameraTransform;

    private void Start()
    {
        cameraShaking = new CameraShaking(frequence,damp);
        cameraTransform = transform;
    }

    private void Update()
    {
        cameraShaking.UpdateSpring(Time.deltaTime, Vector3.zero);
        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, Quaternion.Euler(cameraShaking.value), Time.deltaTime * 10);
    }

    public void StartSpring()
    {
        cameraShaking.value = new Vector3(0, UnityEngine.Random.Range(minRecoilRange.x, maxRecoilRange.x), UnityEngine.Random.Range(minRecoilRange.y, maxRecoilRange.y));
    }
}
