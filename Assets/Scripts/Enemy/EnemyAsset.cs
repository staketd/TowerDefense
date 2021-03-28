using UnityEngine;

namespace Enemy {
    [CreateAssetMenu(menuName = "Assets/Enemy Asset", fileName = "Enemy Asset")]
    public class EnemyAsset : ScriptableObject {
        public EnemyView ViewPrefab;
        public int StartHealth;
        public bool IsFlyingEnemy;
    }
}