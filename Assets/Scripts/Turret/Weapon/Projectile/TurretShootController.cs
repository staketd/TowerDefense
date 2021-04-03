using Runtime;

namespace Turret.Weapon.Projectile {
    public class TurretShootController : IController {
        public void OnStart() {
        }

        public void OnStop() {
        }

        public void Tick() {
            foreach (TurretData data in Game.Player.TurretDatas) {
                data.Weapon.TickShoot();
            }
        }
    }
}