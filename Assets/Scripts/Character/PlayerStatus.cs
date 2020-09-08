using UnityEngine;
using System.Collections;
using AI.Perception;

namespace FPSProject.Character
{
    public class PlayerStatus :CharacterStatus
    {
        public LevelManager instance;

        public delegate void UpdatePlayerInfo();
        public UpdatePlayerInfo updatePlayerInfo;

        private void Start()
        {
            instance = LevelManager.instance;
        }

        public override void OnDamage(int damage)
        {
            this.currentHp -= damage;
            instance.CheckVictory();
        }

        private void Update()
        {
            if(currentHp<=0)
            {
                instance.CheckVictory();
            }
            if(updatePlayerInfo!=null)
            {
                updatePlayerInfo.Invoke();
            }
        }
    }
}