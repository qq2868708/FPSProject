using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSProject.Character
{
    public class CharacterStatus : MonoBehaviour,IOnDamage
    {
        public int maxHp;
        public int currentHp;

        public float attackDistance;
        public float attackSpeed;

        public int damage;


        public virtual void OnDamage(int damage)
        {

        }

        public virtual void Dead()
        {

        }
    }
}