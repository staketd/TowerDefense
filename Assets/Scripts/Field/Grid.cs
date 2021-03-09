using System.Collections.Generic;
using UnityEngine;

namespace Field {
    public class Grid {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private FlowFieldPathfinding m_PathFinding;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int tagret, Vector2Int start) {
            m_Width = width;
            m_Height = height;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Width; i++) {
                for (int j = 0; j < m_Height; j++) {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + 0.5f, 0, j + 0.5f) * nodeSize);
                }
            }

            m_PathFinding = new FlowFieldPathfinding(this, start, tagret);

            m_PathFinding.UpdatePaths();
        }

        public Node GetNode(Vector2Int coordinate) {
            return GetNode(coordinate.x, coordinate.y);
        }

        public bool CheckCoordinate(Vector2Int coordinate) {
            return CheckCoordinate(coordinate.x, coordinate.y);
        }

        public bool CheckCoordinate(int i, int j) {
            return i >= 0 && i < m_Width && j >= 0 && j < m_Height;
        }

        public Node GetNode(int i, int j) {
            return !CheckCoordinate(i, j) ? null : m_Nodes[i, j];
        }

        public IEnumerable<Node> EnumerateAllNodes() {
            for (int i = 0; i < m_Width; i++) {
                for (int j = 0; j < m_Height; j++) {
                    yield return GetNode(i, j);
                }
            }
        }

        public void UpdatePathFinding() {
            m_PathFinding.UpdatePaths();
        }

        public bool TryOccupyNode(Vector2Int coordinate, bool occupy) {
            Node node = GetNode(coordinate);
            if (occupy) {
                if (node.IsOccupied || !m_PathFinding.CanOccupy(coordinate)) {
                    Debug.Log("Unable to occupy");
                    return false;
                }
            } else if (!node.IsOccupied) {
                return false;
            }

            node.IsOccupied = occupy;
            return true;
        }
    }
}