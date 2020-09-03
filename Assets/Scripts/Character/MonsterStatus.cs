using UnityEngine;
using System.Collections;
using FPSProject.Character;

public class MonsterStatus : CharacterStatus
{
    public override void OnDamage(int damage)
    {
        this.currentHp -= damage;
    }
}
