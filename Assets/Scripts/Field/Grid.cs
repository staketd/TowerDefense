using UnityEngine;

namespace Field {
    public class Grid {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        public Grid(int width, int height) {
            m_Width = width;
            m_Height = height;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Width; i++) {
                for (int j = 0; j < m_Height; j++) {
                    m_Nodes[i, j] = new Node();
                }
            }
        }

        public Node GetNode(Vector2Int coordinate) {
            return GetNode(coordinate.x, coordinate.y);
        }

        private bool CheckCoordinate(int i, int j) {
            return i >= 0 && i < m_Width && j >= 0 && j < m_Height;
        }

        public Node GetNode(int i, int j) {
            return !CheckCoordinate(i, j) ? null : m_Nodes[i, j];
        }
    }
}