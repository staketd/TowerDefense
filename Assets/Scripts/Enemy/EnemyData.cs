
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
        

        public EnemyData(EnemyAsset asset) {
            Asset = asset;
            m_Health = asset.StartHealth;
        }

        public void AttachView(EnemyView view) {
            m_View = view;
            m_View.AttachData(this);
        }

        public void ReceiveDamage(float damage) {
            m_Health -= damage ;
            if (m_Health + damage > 0 && m_Health <= 0) {
                Die();
            }
        }

        private void Die() {
            m_View.AnimateDeath();
            Game.Player.EnemyDead(this);
            m_MovementAgent.Die();
        }

        public void AttachMovementAgent(IMovementAgent agent) {
            m_MovementAgent = agent;
        }
    }
}