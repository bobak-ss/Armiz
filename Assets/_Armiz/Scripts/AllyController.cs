using Armiz;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyController : MonoBehaviour
{
    private GameController gameController;
    private GameObject projectilePrefab;
    private Fighter ally;
    private Vector3 allyOrgPosition;
    private Image healthBar;
    private Vector3 initialPos;
    private Vector3 initialScale;

    public void Initialize(GameController _gameController, Fighter _ally, GameObject _projectile)
    {
        gameController = _gameController;
        ally = _ally;
        projectilePrefab = _projectile;
        initialPos = transform.position;
        initialScale = transform.localScale;

        healthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        ally.ResetHealth();
        SetHealthBar();
    }

    void Start()
    {
        allyOrgPosition = transform.position;
    }

    void Update()
    {

    }

    public void SetHealthBar()
    {
        if (healthBar == null) return;
        healthBar.fillAmount = (ally.GetCurrentHealth() / ally.GetTotalHealth());
    }

    public void Hit()
    {
        Debug.Log(gameObject.name + " Hited!");
        if (ally.Damage(gameController.enemy.GetDamage()))
        {
            DespawnThisAlly();
            return;
        }

        SetHealthBar();

        //Ally Hit Animation
        transform.DOScale(0.5f * initialScale.x, 0.2f).OnComplete(() =>
        {
            transform.DOScale(initialScale, 0.2f);
        });
        transform.position = initialPos;
    }


    public float FireProjectile()
    {
        GameObject projectileGO = ObjectPool.Spawn(projectilePrefab, transform.position);

        // projectile animation
        Tween thisTween = projectileGO.transform.DOMove(gameController.enemyPos, 0.7f);
        thisTween.OnComplete(() => {
            ObjectPool.Despawn(projectileGO);
        });

        // attack animation
        Vector3 enemyAllyVector = (transform.position - gameController.enemyPos).normalized;
        enemyAllyVector = new Vector3(enemyAllyVector.x, 0, enemyAllyVector.z);
        enemyAllyVector *= 0.25f;
        allyOrgPosition = transform.position;
        transform.DOMove(allyOrgPosition + enemyAllyVector, 0.1f).OnComplete(() => {
            transform.DOMove(allyOrgPosition, 0.15f);
        });

        return ally.GetDamage();
    }

    private void DespawnThisAlly()
    {
        gameController.allyControllers.Remove(this);
        ObjectPool.Despawn(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemyBullet") Hit();
    }
}
