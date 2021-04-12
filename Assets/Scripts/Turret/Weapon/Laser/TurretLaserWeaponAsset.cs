using UnityEngine;

namespace Turret.Weapon.Laser {
    [CreateAssetMenu(menuName = "Assets/Turrets/Laser Turret", fileName = "Laser Turret")]
    public class TurretLaserWeaponAsset : TurretWeaponAssetBase {
        [SerializeField]
        public float MaxDistance;
        [SerializeField]
        public LineRenderer LineRendererPrefab;
        [SerializeField]
        public float Damage;

        public override ITurrentWeapon GetWeapon(TurretView view) {
            return new TurretLaserWeapon(this, view, Instantiate(LineRendererPrefab));
        }
    }
}