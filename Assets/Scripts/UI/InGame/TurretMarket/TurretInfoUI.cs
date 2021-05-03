using System;
using Runtime;
using Turret;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.TurretMarket {
    public class TurretInfoUI : MonoBehaviour {
        [SerializeField]
        private Image m_Image;

        [SerializeField]
        private Text m_PriceText;

        [SerializeField]
        private Text m_DescriptionText;

        [SerializeField]
        private Button m_ChooseButton;

        private TurretAsset m_Asset;

        public void SetTurret(TurretAsset asset) {
            m_Asset = asset;
            m_Image.sprite = asset.Sprite;
            m_DescriptionText.text = asset.Description;
            m_PriceText.text = $"Price: {asset.Price}";

            m_ChooseButton.onClick.AddListener(OnClick);
            Game.Player.TurretMarket.MoneyChanged += CheckAvailability;
        }

        private void OnDisable() {
            Game.Player.TurretMarket.MoneyChanged -= CheckAvailability;
        }

        private void OnClick() {
            Game.Player.TurretMarket.ChooseTurret(m_Asset);
        }

        private void CheckAvailability(int money) {
            m_ChooseButton.interactable = money >= m_Asset.Price;
        }
    }
}