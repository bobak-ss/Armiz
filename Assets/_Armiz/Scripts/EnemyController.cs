using DG.Tweening;
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
            enemy.ResetHealth();
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
            Color currentColor = gameObject.GetComponent<Renderer>().material.color;
            gameObject.GetComponent<Renderer>().material.DOBlendableColor(Color.red, 0.1f).OnComplete(() => {
                gameObject.GetComponent<Renderer>().material.DOBlendableColor(currentColor, 0.2f); 
            });
        }
    }
}
