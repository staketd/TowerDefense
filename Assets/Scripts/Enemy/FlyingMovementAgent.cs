using Field;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy {
    public class FlyingMovementAgent : IMovementAgent {
        [SerializeField] 
        private float m_Speed;

        private Transform m_Transform;

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;
        private Node m_CurrentNode;
        private EnemyData m_Data;

        public FlyingMovementAgent(float mSpeed, Transform mTransform, Grid grid, EnemyData enemyData) {
            m_Speed = mSpeed;
            m_Transform = mTransform;
            m_Data = enemyData;
            
            SetTargetNode(grid.GetTargetNode());
            m_CurrentNode = grid.GetStartNode();
            m_Data.AttachMovementAgent(this);
        }

        public void TickMovement() {
            if (m_TargetNode == null) {
                return;
            }

            Vector3 target = m_TargetNode.Position;

            var position = m_Transform.position;
            Vector3 dir = target - position;
            dir.y = 0;

            Node nowNode = Game.Player.Grid.GetNodeAtPoint(position);
            if (nowNode != m_CurrentNode) {
                m_CurrentNode.EnemyDatas.Remove(m_Data);
                nowNode.EnemyDatas.Add(m_Data);
                m_CurrentNode = nowNode;
            }

            float distance = dir.magnitude;
            if (distance < TOLERANCE) {
                m_TargetNode = null;
                return;
            }

            dir = dir.normalized;

            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        public void AttachData(EnemyData enemyData) {
            m_Data = enemyData;
        }

        private void SetTargetNode(Node node) {
            m_TargetNode = node;
        }

        public void Die() {
            m_CurrentNode?.EnemyDatas.Remove(m_Data);
        }

        public Node GetCurrentNode() {
            return m_CurrentNode;
        }
    }
}