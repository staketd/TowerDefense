using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Field {
    public class GridHolder : MonoBehaviour {
        [SerializeField]
        private int m_GridWidth;

        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private float m_NodeSize;

        [SerializeField]
        private Vector2Int m_TargetCoordinate;

        [SerializeField]
        private Vector2Int m_StartCoordinate;

        private Grid m_Grid;

        private Vector3 m_Offset;

        private Camera m_Camera;

        public void OnValidate() {
            m_Camera = Camera.main;
            // Default plane scale
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            m_Offset = transform.position - new Vector3(width * 0.5f, 0f, height * 0.5f);
        }

        public void CreateGrid() {
            OnValidate();
            // Default plane scale
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            m_Offset = transform.position - new Vector3(width * 0.5f, 0f, height * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordinate, m_StartCoordinate);
        }

        public void RaycastInGrid() {
            if (m_Grid == null || m_Camera == null) {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.transform != transform) {
                    m_Grid.UnselectNode();
                    return;
                }

                m_Grid.SelectCoordinate(m_Grid.GetNodeCoordinateAtPoint(hit.point));
            } else {
                m_Grid.UnselectNode();
            }
        }

        public Vector2Int StartCoordinate => m_StartCoordinate;

        public Grid Grid => m_Grid;

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

            if (m_Grid == null) {
                return;
            }

            // foreach (Node node in m_Grid.EnumerateAllNodes()) {
            //     if (node.NextNode == null) {
            //         continue;
            //     }
            //
            //     if (node.IsOccupied) {
            //         Gizmos.color = Color.black;
            //         Gizmos.DrawSphere(node.Position, 0.5f);
            //         continue;
            //     }
            //
            //     switch (node.Availability) {
            //         case OccupationAvailability.CanOccupy:
            //             Gizmos.color = Color.green;
            //             Gizmos.DrawSphere(node.Position, 0.4f);
            //             break;
            //         case OccupationAvailability.CanNotOccupy:
            //             Gizmos.color = Color.red;
            //             Gizmos.DrawSphere(node.Position, 0.4f);
            //             break;
            //         case OccupationAvailability.Undefined:
            //             Gizmos.color = Color.gray;
            //             Gizmos.DrawSphere(node.Position, 0.4f);
            //             break;
            //         default:
            //             throw new ArgumentOutOfRangeException();
            //     }
            //
            //     Gizmos.color = Color.red;
            //     Vector3 start = node.Position;
            //     Vector3 end = node.NextNode.Position;
            //     Vector3 dir = end - start;
            //     start -= dir * 0.25f;
            //     end -= dir * 0.75f;
            //     Gizmos.DrawLine(start, end);
            //     Gizmos.DrawSphere(end, 0.1f);
            // }

            Node selectedNode = m_Grid.GetSelectedNode();
            if (selectedNode == null) {
                return;
            }
            Gizmos.DrawSphere(selectedNode.Position, 5f);
            var nodesInRange = m_Grid.GetNodesInCircle(selectedNode.Position, 5f);
            Gizmos.color = Color.blue;
            foreach (Node node in nodesInRange) {
                Gizmos.DrawSphere(node.Position, 0.2f);
            }
        }
    }
}