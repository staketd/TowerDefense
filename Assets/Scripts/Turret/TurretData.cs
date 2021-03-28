using Field;

namespace Turret {
    public class TurretData {
        private TurretView m_View;
        private Node m_Node;

        public TurretView View => m_View;

        public Node Node => m_Node;

        public readonly TurretAsset Asset;

        public TurretData(TurretAsset asset, Node node) {
            Asset = asset;
            m_Node = node;
        }

        public void AttachView(TurretView view) {
            m_View = view;
            m_View.AttachData(this);
        }
    }
}