using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
    public class EnemyController : MonoBehaviour
    {

        public Color enemyOrgColor;
        public Color enemyDamageolor;

        //private GameController gameController;
        private Fighter enemy;
        private Image healthBar;

        private bool colorAnimating = false;

        private Renderer renderer;

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
            renderer = gameObject.GetComponent<Renderer>();
        }

        void Update()
        {
            if (!colorAnimating)
            {
                renderer.material.color = enemyOrgColor;
            }
        }

        public void SetEnemyHealthBar()
        {
            if (healthBar == null) return;
            healthBar.fillAmount = (enemy.GetCurrentHealth() / enemy.GetTotalHealth());
        }

        public void Hit()
        {
            SetEnemyHealthBar();
            colorAnimating = true;
            renderer.material.DOBlendableColor(enemyDamageolor, 0.1f).OnComplete(() => {
                colorAnimating = false;
            });
            transform.DOScale(0.6f, 0.1f);
            transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("HITTT!");
            if (other.gameObject.tag == "Bullet") Hit();
        }
    }
}
