using System;
using Runtime;
using Turret;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace UI.InGame.TurretMarket {
    public class TurretMarketUI : MonoBehaviour {
        [SerializeField]
        private TurretInfoUI m_TurretInfoUIPrefab;
        
        [SerializeField]
        private Button m_OpenMarketButton;

        [SerializeField]
        private Button m_CloseMarketButton;

        [SerializeField]
        private GameObject m_MarketObject;

        [SerializeField]
        private Transform m_Content;

        private void Awake() {
            SubscribeOnButtons();
            ConstructTurretsList();
            
            CloseMarket();
        }

        private void SubscribeOnButtons() {
            m_OpenMarketButton.onClick.AddListener(OpenMarket);
            m_CloseMarketButton.onClick.AddListener(CloseMarket);
        }

        private void OpenMarket() {
            m_MarketObject.SetActive(true);
            m_OpenMarketButton.gameObject.SetActive(false);
            Game.Player.Pause();
        }

        private void CloseMarket() {
            m_MarketObject.SetActive(false);
            m_OpenMarketButton.gameObject.SetActive(true);
            Game.Player.Unpause();
        }

        private void ConstructTurretsList() {
            foreach (TurretAsset turretAsset in Game.CurrentLevel.TurretMarketAsset.TurretAssets) {
                TurretInfoUI turretInfoUI = Instantiate(m_TurretInfoUIPrefab, m_Content);
                turretInfoUI.SetTurret(turretAsset);
            }
        }
    }
}