using System;
using UnityEngine;

namespace Homework2 {
    public class MovementAgent : MonoBehaviour {
        [SerializeField] 
        private float m_Speed;

        private Vector3 m_Start;

        private Vector3 m_Target;
        private const float TOLERANCE = 0.1f;

        public void SetTarget(Vector3 target) {
            m_Target = target;
        }

        public void SetStart(Vector3 start) {
            m_Start = start;
        }

        private void Start() {
            transform.position = m_Start;
        }

        private void Update() {
            float distance = (m_Target - transform.position).magnitude;
            if (distance < TOLERANCE) {
                return;
            }

            Vector3 dir = (m_Target - transform.position).normalized;

            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            transform.Translate(delta);
        }
    }
}
