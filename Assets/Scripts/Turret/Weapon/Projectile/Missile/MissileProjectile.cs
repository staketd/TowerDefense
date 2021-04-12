using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Missile {
    public class MissileProjectile : MonoBehaviour, IProjectile {
        private EnemyData m_Target;
        private float m_Speed = 2;
        private float m_Damage = 10;
        private float m_ExplosionRadius = 2;
        private float m_RotateSpeed = 1000000f;
        private bool m_DidHit = false;
        private Vector3 m_ExplosionCenter;
        public void TickApproaching() {
            transform.Translate(transform.up.normalized * (m_Speed * Time.deltaTime), Space.World);
            transform.LookAt(m_Target.View.transform, Vector3.forward);
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Enemy")) return;
            EnemyView enemyView = other.GetComponent<EnemyView>();
            if (enemyView == null) return;
            m_DidHit = true;
            m_ExplosionCenter = transform.position;
        }

        public bool DidHit() {
            return m_DidHit;
        }

        public void DestroyProjectile() {
            if (!m_DidHit) {
                return;
            }
            List<Node> nodes = Game.Player.Grid.GetNodesInCircle(m_ExplosionCenter, m_ExplosionRadius);
            foreach (Node node in nodes) {
                foreach (EnemyData data in node.EnemyDatas) {
                    data.ReceiveDamage(m_Damage);
                }
            }
            Destroy(gameObject);
        }

        public void SetTarget(EnemyData target) {
            m_Target = target;
        }

        public void SetAsset(MissileProjectileAsset missileProjectileAsset) {
            m_Damage = missileProjectileAsset.m_Damage;
            m_Speed = missileProjectileAsset.m_Speed;
            m_ExplosionRadius = missileProjectileAsset.m_ExplosionRadius;
        }
    }
}