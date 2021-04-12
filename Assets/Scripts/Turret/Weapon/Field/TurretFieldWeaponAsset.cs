using UnityEngine;

namespace Turret.Weapon.Field {
    [CreateAssetMenu(menuName = "Assets/Turrets/Field Turret", fileName = "Field Turret")]
    public class TurretFieldWeaponAsset : TurretWeaponAssetBase {
        [SerializeField]
        public float RateOfFire;
        [SerializeField]
        public float Radius;
        [SerializeField]
        public float Damage;
        public override ITurrentWeapon GetWeapon(TurretView view) {
            return new TurretFieldWeapon(this, view);
        }
    }
}