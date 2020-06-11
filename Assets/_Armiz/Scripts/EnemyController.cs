using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
    public class EnemyController : MonoBehaviour
    {

        public Color defaultEnemyColor;

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
            gameObject.GetComponent<Renderer>().material.DOBlendableColor(Color.red, 0.1f).OnComplete(() => {
                gameObject.GetComponent<Renderer>().material.DOBlendableColor(defaultEnemyColor, 0.2f);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("HITTT!");
            if (other.gameObject.tag == "Bullet") Hit();
        }
    }
}
