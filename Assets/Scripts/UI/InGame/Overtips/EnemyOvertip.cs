using System;
using Enemy;
using UnityEngine;

namespace UI.InGame.Overtips {
    public class EnemyOvertip : MonoBehaviour {
        [SerializeField]
        private RectTransform m_RectTransform;

        private float m_StartHealth;
        
        public void SetData(EnemyData data) {
            m_StartHealth = data.Asset.StartHealth;

            data.HealthChanged += SetHealth;
            SetHealth(data.Health);
        }

        private void SetHealth(float health) {
            SetHealthBar(health / m_StartHealth);
        }
        
        private void SetHealthBar(float percentage) {
            percentage = Mathf.Clamp01(percentage);
            m_RectTransform.anchorMin = Vector2.zero;
            m_RectTransform.anchorMax = new Vector2(percentage, 1f);
        }
    }
}