using Enemy;
using UnityEngine;

namespace Resources {
    [CreateAssetMenu(menuName = "Assets/Enemy Asset", fileName = "Enemy Asset")]
    public class EnemyAsset : ScriptableObject {
        public EnemyView ViewPrefab;
        public int StartHealth;
        public bool IsFlyingEnemy;
    }
}