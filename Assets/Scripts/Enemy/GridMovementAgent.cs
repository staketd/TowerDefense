using Field;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy {
    public class GridMovementAgent : IMovementAgent {
        [SerializeField] 
        private float m_Speed;

        private bool m_Dead = false;

        private Transform m_Transform;

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;
        private Node m_PrevNode;
        private float m_NodeSize;
        private EnemyData m_Data;

        public GridMovementAgent(float mSpeed, Transform mTransform, Grid grid, EnemyData enemyData) {
            m_Speed = mSpeed;
            m_Transform = mTransform;
            m_Data = enemyData;

            m_NodeSize = Game.Player.Grid.NodeSize;
            
            SetTargetNode(grid.GetStartNode());
            m_PrevNode = m_TargetNode;
            m_PrevNode.EnemyDatas.Add(m_Data);
            m_Data.AttachMovementAgent(this);
        }

        public void TickMovement() {
            if (m_TargetNode == null || m_Dead) {
                return;
            }

            Vector3 target = m_TargetNode.Position;

            Vector3 dir = target - m_Transform.position;
            dir.y = 0;

            float distance = dir.magnitude;

            if (Grid.InsideOfNode(m_Transform.position, m_TargetNode, m_NodeSize) && !m_TargetNode.EnemyDatas.Contains(m_Data)) {
                m_PrevNode.EnemyDatas.Remove(m_Data);
                m_TargetNode.EnemyDatas.Add(m_Data);
            }

            if (distance < TOLERANCE) {
                m_PrevNode = m_TargetNode;
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            dir = dir.normalized;

            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }


        private void SetTargetNode(Node node) {
            m_TargetNode = node;
        }

        public void Die() {
            m_Dead = true;
            m_TargetNode?.EnemyDatas.Remove(m_Data);
            m_PrevNode?.EnemyDatas.Remove(m_Data);
        }

        public Node GetCurrentNode() {
            return Game.Player.Grid.GetNodeAtPoint(m_Transform.position);
        }
    }
}