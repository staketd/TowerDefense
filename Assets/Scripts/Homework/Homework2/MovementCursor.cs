using System;
using UnityEngine;

namespace Homework2 {
    public class MovementCursor : MonoBehaviour {
        [SerializeField]
        private int m_GridWidth;

        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private float m_NodeSize;

        [SerializeField]
        private MovementAgent m_MovementAgent;

        [SerializeField]
        private GameObject m_Cursor;

        private Vector3 m_Offset;

        private Camera m_Camera;

        private void Awake() {
            m_Camera = Camera.main;
            // Default plane scale
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            m_Offset = transform.position - new Vector3(width * 0.5f, 0f, height * 0.5f);
            m_MovementAgent.SetStart(transform.position);
        }

        private void Update() {
            if (m_Camera == null) {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.transform != transform) {
                    m_Cursor.SetActive(false);
                    return;
                }

                m_Cursor.SetActive(true);
                Vector3 hitPosition = hit.point;

                Vector3 difference = hitPosition - m_Offset;
                
                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);
                m_Cursor.transform.position = m_Offset +
                    new Vector3(x * m_NodeSize + m_NodeSize / 2f, 0f, z * m_NodeSize + m_NodeSize / 2f);
                if (Input.GetMouseButtonDown(1)) {
                    m_MovementAgent.SetTarget(m_Cursor.transform.position);
                }
            }
            else {
                m_Cursor.SetActive(false);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            Vector3 xShift = new Vector3(m_NodeSize, 0, 0);
            Vector3 zShift = new Vector3(0, 0, m_NodeSize);

            Vector3 pos1 = m_Offset + xShift;
            Vector3 pos2 = pos1 + new Vector3(0, 0, m_GridHeight * m_NodeSize);

            for (int i = 0; i < m_GridWidth - 1; ++i) {
                Gizmos.DrawLine(pos1, pos2);
                pos1 += xShift;
                pos2 += xShift;
            }

            pos1 = m_Offset + zShift;
            pos2 = pos1 + new Vector3(m_GridWidth * m_NodeSize, 0, 0);

            for (int i = 0; i < m_GridHeight - 1; i++) {
                Gizmos.DrawLine(pos1, pos2);
                pos1 += zShift;
                pos2 += zShift;
            }

            Gizmos.DrawSphere(m_Offset, 1f);
        }
    }
}