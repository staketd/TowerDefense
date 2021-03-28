using UnityEngine;

namespace Turret.Weapon {
    public abstract class TurretWeaponAssetBase : ScriptableObject {
        public abstract ITurrentWeapon GetWeapon(TurretView view);
    }
}