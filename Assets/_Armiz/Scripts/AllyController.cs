using Armiz;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    private GameController gameController;
    private GameObject projectilePrefab;
    private Fighter ally;

    public void Initialize(GameController _gameController, Fighter _ally, GameObject _projectile)
    {
        gameController = _gameController;
        ally = _ally;
        projectilePrefab = _projectile;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public float FireProjectile()
    {
        GameObject bulletGO = ObjectPool.Spawn(projectilePrefab, transform.position);
        Tween thisTween = bulletGO.transform.DOMove(gameController.enemyPos, 0.7f);
        thisTween.OnComplete(() => {
            ObjectPool.Despawn(bulletGO);
            //gameController.EnemyHit();
        });

        Vector3 enemyAllyVector = (transform.position - gameController.enemyPos).normalized;
        enemyAllyVector = new Vector3(enemyAllyVector.x, 0, enemyAllyVector.z);
        enemyAllyVector *= 0.25f;
        Vector3 allyOrgPosition = transform.position;
        transform.DOMove(allyOrgPosition + enemyAllyVector, 0.1f).OnComplete(() => {
            transform.DOMove(allyOrgPosition, 0.15f);
        });

        return ally.GetDamage();
    }
}
