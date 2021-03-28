using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile {
    public class TurretProjectileWeapon : ITurrentWeapon {
        private TurretProjectileWeaponAsset m_Asset;
        private TurretView m_View;
        private float m_TimeBetweenShots;
        private float m_MaxDistance;

        private List<Node> m_Nodes;

        private float m_LastShotTime;

        public TurretProjectileWeapon(TurretProjectileWeaponAsset asset, TurretView view) {
            m_Asset = asset;
            m_View = view;
            m_Nodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.transform.position, m_MaxDistance);
            m_TimeBetweenShots = 1f / m_Asset.RateOfFire;
            m_MaxDistance = m_Asset.MaxDistance;
        }
        
        public void TickShoot() {
            float passedTime = Time.time - m_LastShotTime;

            if (passedTime < m_TimeBetweenShots) {
                return;
            }

            EnemyData closestEnemyData =
                EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_Nodes);

            if (closestEnemyData == null) {
                return;
            }
            Shoot(closestEnemyData);
            m_LastShotTime = Time.time;
        }

        private void Shoot(EnemyData data) {
            m_Asset.ProjectileAsset.CreateProjectile(m_View.ProjectileOrigin.position, m_View.ProjectileOrigin.forward, data);
        }
    }
}