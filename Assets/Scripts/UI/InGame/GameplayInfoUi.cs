using System;
using Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class GameplayInfoUi : MonoBehaviour {
        [SerializeField]
        private Text m_HealthText;
        [SerializeField]
        private Text m_MoneyText;
        [SerializeField]
        private Text m_WavesText;

        private void OnEnable() {
            SetHealth(Game.Player.Health);
            SetMoney(Game.Player.TurretMarket.Money);
            
            Game.Player.HealthChanged += SetHealth;
            Game.Player.TurretMarket.MoneyChanged += SetMoney;
        }

        private void OnDisable() {
            Game.Player.HealthChanged -= SetHealth;
            Game.Player.TurretMarket.MoneyChanged -= SetMoney;
        }

        private void SetHealth(int health) {
            m_HealthText.text = $"Health: {health}";
        }

        private void SetMoney(int money) {
            m_MoneyText.text = $"Money: {money}";
        }
        
        private void SetWaves(int wave) {
            m_WavesText.text = $"Wave: {wave}";
        }

    }
}