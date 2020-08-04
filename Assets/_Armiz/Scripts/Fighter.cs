using System;
using UnityEngine;

[CreateAssetMenu]
public class Fighter : ScriptableObject
{
    [SerializeField] private float _damage;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _totalHealth;
    [SerializeField] private int _bounty;
    [SerializeField] private int _cost;
    [SerializeField] private int _level;
    [SerializeField] private FighterType _fighterType;

    public bool Damage(float amount)
    {
        _currentHealth = Math.Max(0, _currentHealth - amount);
        return _currentHealth == 0;
    }

    public void ResetHealth()
    {
        _currentHealth = _totalHealth;
    }

    //TODO: parameter instead of method
    public float GetDamage() { return _damage; }
    public float GetCurrentHealth() { return _currentHealth; }
    public float GetTotalHealth() { return _totalHealth; }
    public int GetBountyValue() { return _bounty; }
    public int GetCost() { return _cost; }
    public FighterType GetFighterType() { return _fighterType; }

    public void LevelUp()
    {
        _level = Math.Max(1, _level + 1);
        switch (_fighterType)
        {
            case FighterType.Enemy:
                _damage = (float)(1f + 0.6f * Math.Pow(_level + 1, 1.5f));
                _totalHealth = (float)(17f + 1f * Math.Pow(_level + 1, 1.9f));
                break;
            case FighterType.Ally:
                _damage = (float)(1f + 0.8f * Math.Pow(_level + 1, 1.5f));
                _totalHealth = (float)(8f + 0.6f * Math.Pow(_level + 1, 1.5f));
                break;
            default:
                break;
        }
        ResetHealth();
    }
    public int GetLevel() { return _level; }

    public enum FighterType
    {
        Enemy,
        Ally
    }

}