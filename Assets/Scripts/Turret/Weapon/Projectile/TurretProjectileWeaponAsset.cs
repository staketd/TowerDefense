using UnityEngine;

namespace Turret.Weapon.Projectile {
    [CreateAssetMenu(menuName = "Assets/Turrets/Turret Projectile Asset", fileName = "Turret Projectile Asset")]
    public class TurretProjectileWeaponAsset : TurretWeaponAssetBase {
        public float RateOfFire;
        public float MaxDistance;

        public ProjectileAssetBase ProjectileAsset;
        
        public override ITurrentWeapon GetWeapon(TurretView view) {
            return new TurretProjectileWeapon(this, view);
        }
    }
}