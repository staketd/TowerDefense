using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using UnityEngine;

namespace Turret.Weapon {
    public static class EnemySearch {

        [CanBeNull]
        public static EnemyData GetClosestEnemy(Vector3 center, float maxDistance, List<Node> closestNodes) {
            float minSqrDistance = maxDistance * maxDistance;
            EnemyData closestEnemy = null;
            foreach (Node node in closestNodes) {
                foreach (EnemyData enemyData in node.EnemyDatas) {
                    float dist = (enemyData.View.transform.position - center).sqrMagnitude;
                    if (dist < minSqrDistance) {
                        minSqrDistance = dist;
                        closestEnemy = enemyData;
                    }
                }
            }
            return closestEnemy;
        }
    }
}