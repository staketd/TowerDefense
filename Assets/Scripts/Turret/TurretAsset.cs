using Turret.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace Turret {
    [CreateAssetMenu(menuName = "Assets/Turret Asset", fileName = "Turret Asset")]
    public class TurretAsset : ScriptableObject {
        public Sprite Sprite;
        public string Description;
        public TurretView ViewPrefab;
        public TurretWeaponAssetBase WeaponAsset;
        public int Price;
    }
}