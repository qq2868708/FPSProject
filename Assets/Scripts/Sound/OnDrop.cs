using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///用于产生粒子系统的碰撞检测
/// </summary>
public class OnDrop : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("clip!");
        audioSource.Play();
    }
}
