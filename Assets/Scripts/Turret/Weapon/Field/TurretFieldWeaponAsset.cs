using UnityEngine;

namespace Turret.Weapon.Field {
    [CreateAssetMenu(menuName = "Assets/Turrets/Field Turret", fileName = "Field Turret")]
    public class TurretFieldWeaponAsset : TurretWeaponAssetBase {
        public float RateOfFire;
        public float Radius;
        public float Damage;
        public override ITurrentWeapon GetWeapon(TurretView view) {
            return new TurretFieldWeapon(this, view);
        }
    }
}