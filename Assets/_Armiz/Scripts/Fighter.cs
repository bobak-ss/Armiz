using System;
using UnityEngine;

[CreateAssetMenu]
public class Fighter : ScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _totalHealth;
    [SerializeField] private int _bounty;
    [SerializeField] private int _cost;
    [SerializeField] private FighterType _fighterType;

    public bool Damage(int amount)
    {
        _currentHealth = Math.Max(0, _currentHealth - amount);
        return _currentHealth == 0;
    }

    public void ResetHealth()
    {
        _currentHealth = _totalHealth;
    }

    public int GetDamage() { return _damage; }

    //TODO: parameter instead of method
    public int GetCurrentHealth() { return _currentHealth; }
    //TODO: parameter instead of method
    public float GetTotalHealth() { return _totalHealth; }
    public int GetBountyValue() { return _bounty; }
    public int GetCost() { return _cost; }

    public enum FighterType
    {
        Enemy,
        Ally
    }

}