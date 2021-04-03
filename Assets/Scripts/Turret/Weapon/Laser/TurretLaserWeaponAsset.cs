using UnityEngine;

namespace Turret.Weapon.Laser {
    [CreateAssetMenu(menuName = "Assets/Turrets/Laser Turret", fileName = "Laser Turret")]
    public class TurretLaserWeaponAsset : TurretWeaponAssetBase {
        public float MaxDistance;
        public LineRenderer LineRendererPrefab;
        public float Damage;

        public override ITurrentWeapon GetWeapon(TurretView view) {
            return new TurretLaserWeapon(this, view, Instantiate(LineRendererPrefab));
        }
    }
}