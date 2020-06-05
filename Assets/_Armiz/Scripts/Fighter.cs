using System;
using UnityEngine;

[CreateAssetMenu]
public class Fighter : ScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _totalHealth;
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

    public int GetCurrentHealth() { return _currentHealth; }
    public enum FighterType
    {
        Enemy,
        Ally
    }
}