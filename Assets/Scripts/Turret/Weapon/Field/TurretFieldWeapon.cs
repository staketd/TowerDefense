using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Field {
    public class TurretFieldWeapon : ITurrentWeapon {
        private TurretView m_View;
        private List<Node> m_Nodes;

        private float m_Damage;
        private float m_Radius;

        public TurretFieldWeapon(TurretFieldWeaponAsset asset, TurretView view) {
            m_View = view;
            m_Damage = asset.Damage;
            m_Radius = asset.Radius;
            m_Nodes = Game.Player.Grid.GetNodesInCircle(m_View.transform.position, asset.Radius);
        }
        public void TickShoot() {
            TickWeapon();
        }

        private void TickWeapon() {
            List<EnemyData> enemyDatas = EnemySearch.GetEnemiesInNodes(m_Nodes);
            foreach (EnemyData enemyData in enemyDatas) {
                Vector3 rad = m_View.transform.position - enemyData.View.transform.position;
                if (rad.sqrMagnitude < m_Radius) {
                    enemyData.ReceiveDamage(m_Damage * Time.deltaTime);
                }
            }
        }
    }
}