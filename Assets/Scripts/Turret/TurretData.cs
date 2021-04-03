using Field;
using Turret.Weapon;

namespace Turret {
    public class TurretData {
        private TurretView m_View;
        private Node m_Node;
        private ITurrentWeapon m_Weapon;

        public TurretView View => m_View;

        public Node Node => m_Node;

        public readonly TurretAsset Asset;
        public ITurrentWeapon Weapon => m_Weapon;

        public TurretData(TurretAsset asset, Node node) {
            Asset = asset;
            m_Node = node;
        }

        public void AttachView(TurretView view) {
            m_View = view;
            m_View.AttachData(this);
            m_Weapon = Asset.WeaponAsset.GetWeapon(m_View);
        }
    }
}