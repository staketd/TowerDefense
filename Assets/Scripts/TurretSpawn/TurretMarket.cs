using System;
using Enemy;
using Runtime;
using Turret;
using UnityEngine;

namespace TurretSpawn {
    public class TurretMarket {
        private int m_Money;
        private TurretAsset m_ChosenTurret;

        public int Money => m_Money;

        public event Action<int> MoneyChanged;

        public TurretMarket() {
            m_Money = Game.CurrentLevel.StartMoney;
        }

        public TurretAsset ChosenTurret {
            get {
                if (m_ChosenTurret == null) {
                    return null;
                }
                return m_ChosenTurret.Price > m_Money ? null : m_ChosenTurret;     
            }
        } 

        public void ChooseTurret(TurretAsset asset) {
            if (asset.Price > m_Money) {
                return;
            }

            m_ChosenTurret = asset;
        }

        public void BuyTurret(TurretAsset turretAsset) {
            if (turretAsset.Price > m_Money) {
                Debug.LogError("Not enough money!");
                return;
            }
            m_Money -= turretAsset.Price;
            MoneyChanged?.Invoke(m_Money);
        }

        public void GetBounty(EnemyData data) {
            m_Money += data.Asset.Bounty;
            MoneyChanged?.Invoke(m_Money);
        }
    }
}