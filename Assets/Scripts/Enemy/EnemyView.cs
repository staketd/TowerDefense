using UI.InGame.Overtips;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy {
    public class EnemyView : MonoBehaviour {
        [SerializeField]
        private EnemyOvertip m_Overtip;
        
        private EnemyData m_Data;
        private IMovementAgent m_MovementAgent;

        [SerializeField]
        private float m_YCoordinate;

        [SerializeField]
        private Animator m_Animator;

        private static readonly int DieAnimatorIndex = Animator.StringToHash("Die");
        private static readonly int NotDeadAnimatorIndex = Animator.StringToHash("NotDead");

        public float YCoordinate => m_YCoordinate;

        public EnemyData Data => m_Data;
 
        public IMovementAgent MovementAgent => m_MovementAgent;
        public void AttachData(EnemyData data) {
            m_Data = data;
            m_Overtip.SetData(data);
        }

        public void CreateMovementAgent(Grid grid) {
            if (Data.Asset.IsFlyingEnemy) {
                m_MovementAgent = new FlyingMovementAgent(m_Data.Asset.Speed, transform, grid, m_Data);
            } else {
                m_MovementAgent = new GridMovementAgent(m_Data.Asset.Speed, transform, grid, m_Data);
            }
        }

        public void AnimateDeath() {
            m_Animator.SetTrigger(DieAnimatorIndex);
            m_Animator.SetBool(NotDeadAnimatorIndex, false);
        }

        public void Die() {
            Destroy(gameObject, 3f);
        }

        public void Reach() {
            Destroy(gameObject, 0.1f);
        }
    }
}