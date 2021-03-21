using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy {
    public class FlyingMovementAgent : IMovementAgent {
        [SerializeField] 
        private float m_Speed;

        private Transform m_Transform;

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;
        
        public FlyingMovementAgent(float mSpeed, Transform mTransform, Grid grid) {
            m_Speed = mSpeed;
            m_Transform = mTransform;
            
            SetTargetNode(grid.GetTargetNode());
        }

        public void TickMovement() {
            if (m_TargetNode == null) {
                return;
            }

            Vector3 target = m_TargetNode.Position;

            Vector3 dir = target - m_Transform.position;
            dir.y = 0;

            float distance = dir.magnitude;
            if (distance < TOLERANCE) {
                m_TargetNode = null;
                return;
            }

            dir = dir.normalized;

            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node) {
            m_TargetNode = node;
        }
    }
}