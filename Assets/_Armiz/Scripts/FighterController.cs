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
        public Fighter fighter;

        private GameController gameController;
        private Renderer renderer;
        private Image healthBar;
        private TextMeshPro healthTxt;
        private bool colorAnimating = false;
        private bool transformAnimating = false;
        private GameObject projectilePrefab;
        private Vector3 initialPos;
        private Vector3 initialScale;
        private bool isAlly;
        private bool isDead = false;

        public void Initialize(GameController _gameController, Fighter _fighter, float _health, GameObject _projectilePrefab)
        {
            gameController = _gameController;
            fighter = _fighter;
            projectilePrefab = _projectilePrefab;
            //initialPos = transform.position;
            initialScale = transform.localScale;
            healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            healthTxt = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshPro>();
            isAlly = fighter.GetFighterType() == Fighter.FighterType.Ally;

            fighter.SetHealth(_health);
            SetHealthBar();
            
            EventManager.SubscribeAlliesAttack(OnAlliesAttack);
            EventManager.SubscribeEnemiesAttack(OnEnemiesAttack);
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
            if (!transformAnimating)
            {
                transform.localScale = initialScale;
                transform.position = initialPos;
            }

            if (isDead)
            {
                DespawnThisFighter();
                isDead = false;
            }
        }
        
        private void OnAlliesAttack()
        {
            if (!isAlly) return;
            FireProjectileTo(gameController.enemyPos);
        }

        private void OnEnemiesAttack()
        {
            if (isAlly) return;
            if (gameController.allyFighterControllers.Count < 1) return;
            
            var targetedAllyToHit = 
                gameController.allyFighterControllers[UnityEngine.Random.Range(0, gameController.allyFighterControllers.Count - 1)];
            FireProjectileTo(targetedAllyToHit.transform.position);
        }

        public void SetPosition(Vector3 _pos)
        {
            initialPos = _pos;
            transform.position = _pos;
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
            transformAnimating = true;
            transform.DOScale(0.8f * initialScale.x, 0.1f).OnComplete(() =>
            {
                transform.DOScale(initialScale, 0.15f).OnComplete(() =>
                {
                    if (isAlly)
                        isDead = fighter.Damage(gameController.enemy.GetDamage());
                    else
                        isDead = fighter.Damage(gameController.ally.GetDamage());
                    transformAnimating = false;
                });
            });
            renderer.material.DOBlendableColor(fighterDamageolor, 0.1f).OnComplete(() => 
            {
                colorAnimating = false;
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

        public int GetLevel() { return fighter.GetLevel(); }
        public void LevelUp() 
        { 
            fighter.LevelUp();
            SetHealthBar();
        }

        private void DespawnThisFighter()
        {
            EventManager.UnsubscribeAlliesAttack(OnAlliesAttack);
            EventManager.UnsubscribeEnemiesAttack(OnEnemiesAttack);
            
            if (isAlly)
            {
                GameData.AllyCount--;
                gameController.allyFighterControllers.Remove(this);
                ObjectPool.Despawn(this.gameObject);
            }
            else
            {
                fighter.LevelUp();
                gameController.enemyFighterControllers.Remove(this);
                ObjectPool.Despawn(this.gameObject);
                gameController.EnemyDied();
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
