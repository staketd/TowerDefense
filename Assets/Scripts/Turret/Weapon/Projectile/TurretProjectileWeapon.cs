using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile {
    public class TurretProjectileWeapon : ITurrentWeapon {
        private TurretProjectileWeaponAsset m_Asset;
        private TurretView m_View;

        private List<IProjectile> m_Projectiles = new List<IProjectile>();
        private float m_TimeBetweenShots;
        private float m_MaxDistance;

        [CanBeNull]
        private EnemyData m_ClosestEnemyData;

        private List<Node> m_Nodes;

        private float m_LastShotTime;

        public TurretProjectileWeapon(TurretProjectileWeaponAsset asset, TurretView view) {
            m_Asset = asset;
            m_View = view;
            m_TimeBetweenShots = 1f / m_Asset.RateOfFire;
            m_MaxDistance = m_Asset.MaxDistance;
            m_Nodes = Game.Player.Grid.GetNodesInCircle(m_View.ProjectileOrigin.transform.position, m_MaxDistance);
        }

        public void TickShoot() {
            TickWeapon();
            TickProjectiles();
        }

        private void TickTower() {
            if (m_ClosestEnemyData != null) {
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }

        private void TickWeapon() {
            float passedTime = Time.time - m_LastShotTime;

            if (passedTime < m_TimeBetweenShots) {
                return;
            }

            m_ClosestEnemyData =
                EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_Nodes);

            if (m_ClosestEnemyData == null) {
                return;
            }
            
            TickTower();

            Shoot(m_ClosestEnemyData);
            m_LastShotTime = Time.time;
        }

        private void TickProjectiles() {
            List<IProjectile> toDelete = new List<IProjectile>();
            for (var i = 0; i < m_Projectiles.Count; i++) {
                IProjectile projectile = m_Projectiles[i];
                projectile.TickApproaching();
                if (projectile.DidHit()) {
                    projectile.DestroyProjectile();
                    m_Projectiles[i] = null;
                }
            }

            m_Projectiles.RemoveAll(projectile => projectile == null);
        }

        private void Shoot(EnemyData data) {
            m_Projectiles.Add(
                m_Asset.ProjectileAsset.CreateProjectile(m_View.ProjectileOrigin.position,
                    m_View.ProjectileOrigin.forward, data));
        }
    }
}