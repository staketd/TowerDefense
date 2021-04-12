using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Bullet {
    
    [CreateAssetMenu(menuName = "Assets/Projectiles/Bullet Asset", fileName = "Bullet Asset")]
    public class BulletProjectileAsset : ProjectileAssetBase {
        [SerializeField]
        private BulletProjectile m_BulletPrefab;

        [SerializeField]
        public float m_Damage;

        [SerializeField]
        public float m_Speed;
        
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData data) {
            BulletProjectile projectile = Instantiate(m_BulletPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            projectile.SetAsset(this);
            return projectile;
        }
    }
}