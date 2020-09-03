using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSProject.Character;

public class MonsterAttack : MonoBehaviour
{
    public SphereCollider attackCollider;
    public CharacterStatus monsterStatus;
    // Start is called before the first frame update
    void Start()
    {
        attackCollider = GetComponentInChildren<SphereCollider>();
        monsterStatus = GetComponent<CharacterStatus>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.GetComponentInChildren<CharacterStatus>()!=null)
        {
            collider.transform.GetComponentInChildren<CharacterStatus>().OnDamage(monsterStatus.damage);
        }
    }
}
