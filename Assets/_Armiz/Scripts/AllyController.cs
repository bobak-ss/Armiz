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

        return ally.GetDamage();
    }
}
