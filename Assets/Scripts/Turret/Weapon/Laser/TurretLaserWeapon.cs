using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Laser {
    public class TurretLaserWeapon : ITurrentWeapon {
        private TurretLaserWeaponAsset m_Asset;
        private TurretView m_View;
        private float m_MaxDistance;
        private float m_Damage;

        private LineRenderer m_LineRenderer;

        private List<Node> m_Nodes;
        
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;
        
        public TurretLaserWeapon(TurretLaserWeaponAsset asset, TurretView view, LineRenderer lineRenderer) {
            m_LineRenderer = lineRenderer;
            m_Damage = asset.Damage;
            m_View = view;
            m_Asset = asset;
            m_MaxDistance = m_Asset.MaxDistance;
            m_Nodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.transform.position, m_MaxDistance);
        }
        public void TickShoot() {
            TickWeapon();
        }

        private void TickWeapon() {
            m_ClosestEnemyData =
                EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_Nodes);
            if (m_ClosestEnemyData == null) {
                m_LineRenderer.gameObject.SetActive(false);
                return;
            }
            
            m_LineRenderer.SetPositions(new [] {
                m_View.ProjectileOrigin.position,
                m_ClosestEnemyData.View.transform.position
            });
            
            m_LineRenderer.gameObject.SetActive(true);

            TickTower();
            Shoot(m_ClosestEnemyData);
        }

        private void Shoot(EnemyData closestEnemyData) {
            closestEnemyData.ReceiveDamage(m_Damage * Time.deltaTime);
        }

        private void TickTower() {
            if (m_ClosestEnemyData != null) {
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }
    }
}