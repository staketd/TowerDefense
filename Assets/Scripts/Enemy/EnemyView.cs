using UnityEngine;
using Grid = Field.Grid;

namespace Enemy {
    public class EnemyView : MonoBehaviour {
        private EnemyData m_Data;
        private IMovementAgent m_MovementAgent;

        [SerializeField]
        private float m_YCoordinate;

        public float YCoordinate => m_YCoordinate;

        public EnemyData Data => m_Data;

        public IMovementAgent MovementAgent => m_MovementAgent;
        public void AttachData(EnemyData data) {
            m_Data = data;
        }

        public void CreateMovementAgent(Grid grid) {
            if (Data.Asset.IsFlyingEnemy) {
                m_MovementAgent = new FlyingMovementAgent(5f, transform, grid);
            } else {
                m_MovementAgent = new GridMovementAgent(2f, transform, grid);
            }
        }
    }
}