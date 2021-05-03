using System.Collections.Generic;
using Runtime;
using UnityEngine;

namespace Enemy {
    public class EnemyDeathController : IController {
        private List<EnemyData> m_DiedEneyDatas = new List<EnemyData>();
        public void OnStart() {
            
        }

        public void OnStop() {
        }

        public void Tick() {
            
            foreach (EnemyData enemyData in Game.Player.EnemyDatas) {
                if (enemyData.IsDead) {
                    Game.Player.TurretMarket.GetBounty(enemyData);
                    m_DiedEneyDatas.Add(enemyData);
                }
            }

            foreach (EnemyData enemyData in m_DiedEneyDatas) {
                enemyData.Die();
            }
            
            m_DiedEneyDatas.Clear();
        }
    }
}