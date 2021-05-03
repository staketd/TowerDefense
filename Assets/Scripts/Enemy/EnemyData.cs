
using System;
using Runtime;
using UnityEngine;

namespace Enemy {
    public class EnemyData {
        private EnemyView m_View;
        private float m_Health;
        private IMovementAgent m_MovementAgent;

        public EnemyView View => m_View;

        public readonly EnemyAsset Asset;

        public bool IsDead => m_Health <= 0;

        public float Health => m_Health;

        public event Action<float> HealthChanged; 
        

        public EnemyData(EnemyAsset asset) {
            Asset = asset;
            m_Health = asset.StartHealth;
        }

        public void AttachView(EnemyView view) {
            m_View = view;
            m_View.AttachData(this);
        }

        public void ReceiveDamage(float damage) {
            if (IsDead) {
                return;
            }
            if (m_Health - damage <= 0) {
                m_Health = 0;
            } else {
                m_Health -= damage;
            }
            HealthChanged?.Invoke(m_Health);
        }

        public void Die() {
            m_View.AnimateDeath();
            Game.Player.EnemyDied(this);
            m_MovementAgent.Die();
            m_View.Die();
        }

        public void Reach() {
            m_Health = 0;
            Game.Player.EnemyReachedTarget(this);
            m_MovementAgent.Die();
            m_View.Reach();
        }

        public void AttachMovementAgent(IMovementAgent agent) {
            m_MovementAgent = agent;
        }
    }
}