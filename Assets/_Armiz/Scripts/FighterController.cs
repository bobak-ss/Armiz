using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Armiz
{
	public class FighterController : MonoBehaviour
    {
        public Color fighterOrgColor;
        public Color fighterDamageolor;

        private GameController gameController;
        private Renderer renderer;
        private Fighter fighter;
        private Image healthBar;
        private TextMeshPro healthTxt;
        private bool colorAnimating = false;
        private GameObject projectilePrefab;
        private Vector3 initialPos;
        private Vector3 initialScale;
        private bool isAlly;

        public void Initialize(GameController _gameController, Fighter _fighter, GameObject _projectilePrefab)
        {
            gameController = _gameController;
            fighter = _fighter;
            projectilePrefab = _projectilePrefab;
            initialPos = transform.position;
            initialScale = transform.localScale;
            healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            healthTxt = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshPro>();
            isAlly = fighter.GetFighterType() == Fighter.FighterType.Ally;

            fighter.ResetHealth();
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
                renderer.material.color = fighterOrgColor;
            }
        }

        public void SetHealthBar()
        {
            if (healthBar == null || healthTxt == null) return;
            healthBar.fillAmount = (fighter.GetCurrentHealth() / fighter.GetTotalHealth());
            healthTxt.text = string.Format("{0}", Math.Round(fighter.GetCurrentHealth()));
        }

        public void Hit()
        {
            Debug.Log(gameObject.name + " Hited!");
            SetHealthBar();

            colorAnimating = true;
            transform.DOScale(0.5f * initialScale.x, 0.1f).OnComplete(() =>
            {
                transform.DOScale(initialScale, 0.15f);
            });
            transform.position = initialPos;
            renderer.material.DOBlendableColor(fighterDamageolor, 0.1f).OnComplete(() => 
            {
                colorAnimating = false;

                bool dead = false;
                if (isAlly)
                    dead = fighter.Damage(gameController.enemy.GetDamage());
                else
                    dead = fighter.Damage(gameController.ally.GetDamage());
                if (dead)
                    DespawnThisFighter();
            });
        }

        public void FireProjectileTo(Vector3 targetPos)
        {
            GameObject projectileGO = ObjectPool.Spawn(projectilePrefab, transform.position);

            // projectile animation
            Tween thisTween = projectileGO.transform.DOMove(targetPos, 0.7f);
            thisTween.OnComplete(() => {
                ObjectPool.Despawn(projectileGO);
            });

            if (isAlly)
            {
                Vector3 enemyAllyVector = (transform.position - targetPos).normalized;
                enemyAllyVector = new Vector3(enemyAllyVector.x, 0, enemyAllyVector.z);
                enemyAllyVector *= 0.25f;
                transform.DOMove(initialPos + enemyAllyVector, 0.1f).OnComplete(() => {
                    transform.DOMove(initialPos, 0.15f);
                });
            }
        }

        private void DespawnThisFighter()
        {
            if (isAlly)
            {
                GameData.AllyCount--;
                gameController.allyFighterControllers.Remove(this);
                ObjectPool.Despawn(this.gameObject);
            }
            else
            {

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isAlly)
            {
                if (other.gameObject.tag == "enemyBullet")
                    Hit();
            }
            else
            {
                if (other.gameObject.tag == "allyBullet")
                    Hit();
            }
        }
    }
}
