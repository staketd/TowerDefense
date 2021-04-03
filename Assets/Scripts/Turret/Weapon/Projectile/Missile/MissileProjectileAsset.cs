using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Missile {
    [CreateAssetMenu(menuName = "Assets/Projectiles/Missile Projectile", fileName = "Missile Projectile")]
    public class MissileProjectileAsset : ProjectileAssetBase {
        [SerializeField]
        private MissileProjectile m_MissilePrefab;
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData data) {
            MissileProjectile projectile = Instantiate(m_MissilePrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            projectile.SetTarget(data);
            return projectile;
        }
    }
}