using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class CameraShaking
{
    public Vector3 value;

    private float frequence;
    private float damp;
    private Vector3 dampValue;

    public CameraShaking(float _frequence,float _damp)
    {
        frequence = _frequence;
        damp = _damp;
    }

    public void UpdateSpring(float _deltaTime,Vector3 _target)
    {
        value -= _deltaTime * frequence * dampValue;
        dampValue = Vector3.Lerp(dampValue, value - _target, damp * _deltaTime);
    }
}
