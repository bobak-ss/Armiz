using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
    public class EnemyController : MonoBehaviour
    {

        //private GameController gameController;
        private Fighter enemy;
        private Image healthBar;

        public void Initialize(Fighter _enemy, Image _healthBar)
        {
            //gameController = _gameController;
            enemy = _enemy;
            healthBar = _healthBar;
            SetEnemyHealthBar();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetEnemyHealthBar()
        {
            if (healthBar == null) return;
            healthBar.fillAmount = (enemy.GetCurrentHealth() / enemy.GetTotalHealth());
        }

        public void Hit()
        {
            SetEnemyHealthBar();
        }
    }
}
