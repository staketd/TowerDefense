using UnityEngine;

namespace Enemy {
    [CreateAssetMenu(menuName = "Assets/Enemy Asset", fileName = "Enemy Asset")]
    public class EnemyAsset : ScriptableObject {
        public EnemyView ViewPrefab;
        public float StartHealth;
        public bool IsFlyingEnemy;
        public float Speed;
    }
}