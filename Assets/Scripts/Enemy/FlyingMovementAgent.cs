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
        private Node m_NowNode;
        private EnemyData m_Data;

        public FlyingMovementAgent(float mSpeed, Transform mTransform, Grid grid, EnemyData enemyData) {
            m_Speed = mSpeed;
            m_Transform = mTransform;
            m_Data = enemyData;
            
            SetTargetNode(grid.GetTargetNode());
            m_NowNode = grid.GetStartNode();
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
            if (nowNode != m_NowNode) {
                m_NowNode.EnemyDatas.Remove(m_Data);
                nowNode.EnemyDatas.Add(m_Data);
                m_NowNode = nowNode;
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
            m_NowNode?.EnemyDatas.Remove(m_Data);
        }
    }
}