using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Bullet {
    
    [CreateAssetMenu(menuName = "Assets/Projectiles/Bullet Asset", fileName = "Bullet Asset")]
    public class BulletProjectileAsset : ProjectileAssetBase {
        [SerializeField]
        private BulletProjectile m_BulletPrefab;
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData data) {
            return Instantiate(m_BulletPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
        }
    }
}