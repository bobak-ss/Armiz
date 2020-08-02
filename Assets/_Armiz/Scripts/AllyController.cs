using Armiz;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyController : MonoBehaviour
{
    private GameController gameController;
    private GameObject projectilePrefab;
    private Fighter ally;
    private Image healthBar;
    private TextMeshPro healthTxt;
    private Vector3 initialPos;
    private Vector3 initialScale;

    public void Initialize(GameController _gameController, Fighter _ally, GameObject _projectile)
    {
        gameController = _gameController;
        ally = _ally;
        projectilePrefab = _projectile;
        initialPos = transform.position;
        initialScale = transform.localScale;

        healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        healthTxt = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshPro>();

        ally.ResetHealth();
        SetHealthBar();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetHealthBar()
    {
        if (healthBar == null || healthTxt == null) return;
        healthBar.fillAmount = (ally.GetCurrentHealth() / ally.GetTotalHealth());
        healthTxt.text = string.Format("{0}", Math.Round(ally.GetCurrentHealth()));
        //healthTxt.text = string.Format("{0}/{1}", Math.Round(ally.GetCurrentHealth(), 1) * 10, Math.Round(ally.GetTotalHealth(), 1) * 10);
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
        transform.DOScale(0.5f * initialScale.x, 0.1f).OnComplete(() =>
        {
            transform.DOScale(initialScale, 0.15f);
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
        transform.DOMove(initialPos + enemyAllyVector, 0.1f).OnComplete(() => {
            transform.DOMove(initialPos, 0.15f);
        });

        return ally.GetDamage();
    }

    private void DespawnThisAlly()
    {
        GameData.AllyCount--;
        gameController.allyControllers.Remove(this);
        ObjectPool.Despawn(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemyBullet") Hit();
    }
}
