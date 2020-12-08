using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Armiz
{
    public static class EventManager
    {
        private static Action idleStateStart;
        private static Action attackStateStart;
        private static Action alliesAttack;
        private static Action enemiesAttack;
        
        public static void SubscribeIdleStateStart(Action callback)
        {
            idleStateStart += callback;
        }
        public static void UnsubscribeIdleStateStart(Action callback)
        {
            if (idleStateStart != null) idleStateStart -= callback;
        }
        public static void FireEventIdleStateStart()
        {
            idleStateStart?.Invoke();
        }
        
        public static void SubscribeAttackStateStart(Action callback)
        {
            attackStateStart += callback;
        }
        public static void UnsubscribeAttackStateStart(Action callback)
        {
            if (attackStateStart != null) attackStateStart -= callback;
        }
        public static void FireEventAttackStateStart()
        {
            attackStateStart?.Invoke();
        }
        
        public static void SubscribeAlliesAttack(Action callback)
        {
            alliesAttack += callback;
        }
        public static void UnsubscribeAlliesAttack(Action callback)
        {
            if (alliesAttack != null) alliesAttack -= callback;
        }
        public static void FireEventAlliesAttack()
        {
            alliesAttack?.Invoke();
        }
        
        public static void SubscribeEnemiesAttack(Action callback)
        {
            enemiesAttack += callback;
        }
        public static void UnsubscribeEnemiesAttack(Action callback)
        {
            if (enemiesAttack != null) enemiesAttack -= callback;
        }
        public static void FireEventEnemiesAttack()
        {
            enemiesAttack?.Invoke();
        }
    }   
}
