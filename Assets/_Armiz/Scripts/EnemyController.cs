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
        private GameObject projectilePrefab;

        public void Initialize(Fighter _enemy, GameObject _projectilePrefab)
        {
            enemy = _enemy;
            projectilePrefab = _projectilePrefab;
            healthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();

            enemy.ResetHealth();
            SetHealthBar();
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

        public void SetHealthBar()
        {
            if (healthBar == null) return;
            healthBar.fillAmount = (enemy.GetCurrentHealth() / enemy.GetTotalHealth());
        }

        public void Hit()
        {
            SetHealthBar();
            colorAnimating = true;
            renderer.material.DOBlendableColor(enemyDamageolor, 0.1f).OnComplete(() => {
                colorAnimating = false;
            });
            transform.DOScale(0.6f, 0.1f);
            transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        }

        public void FireProjectileTo(Vector3 targetPos)
        {
            GameObject projectileGO = ObjectPool.Spawn(projectilePrefab, transform.position);

            // projectile animation
            Tween thisTween = projectileGO.transform.DOMove(targetPos, 0.7f);
            thisTween.OnComplete(() => {
                ObjectPool.Despawn(projectileGO);
            });

            // attack animation
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "allyBullet") Hit();
        }
    }
}
