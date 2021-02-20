using System;
using UnityEngine;

namespace Field {
    public class GridHolder : MonoBehaviour {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField] 
        private float m_NodeSize;

        private Grid m_Grid;

        private Vector3 m_Offset;

        private Camera m_Camera;

        private void Awake() {
            m_Grid = new Grid(m_GridWidth, m_GridHeight);
            m_Camera = Camera.main;

            
            // Default plane scale
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            m_Offset = transform.position - new Vector3(width * 0.5f, 0f, height * 0.5f);
        }

        private void Update() {
            if (m_Grid == null || m_Camera == null) {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.transform != transform) {
                    return;
                }

                Vector3 hitPosition = hit.point;

                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);

                Debug.Log("Hit " + x + " " + z);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 1f);
        }
    }
}