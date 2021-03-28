using System;
using System.Collections.Generic;
using System.Drawing;
using Support;
using UnityEngine;

namespace Field {
    public class Grid {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private Vector3 m_Offset;
        private float m_NodeSize;

        public float NodeSize => m_NodeSize;

        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;

        private Node m_SelectedNode = null;
        private Vector2Int m_SelectedNodeCoordinate = new Vector2Int();

        private FlowFieldPathfinding m_PathFinding;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int tagret, Vector2Int start) {
            m_Width = width;
            m_Height = height;

            m_Offset = offset;
            m_NodeSize = nodeSize;

            m_StartCoordinate = start;
            m_TargetCoordinate = tagret;

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

        public Vector2Int GetNodeCoordinateAtPoint(Vector3 point) {
            Vector3 projectedCoordinates = new Vector3(point.x, m_Offset.y, point.z);
            Vector3 diff = projectedCoordinates - m_Offset;

            int x = (int) (diff.x / m_NodeSize);
            int z = (int) (diff.z / m_NodeSize);

            return new Vector2Int(x, z);
        }

        public List<Vector2Int> GetNodeCoordinatesInCircle(Vector3 point, float radius) {
            List<Vector2Int> nodesCoordinates = new List<Vector2Int>();
            if (radius < 1e-5) {
                return nodesCoordinates;
            }
            Vector2Int startNode = GetNodeCoordinateAtPoint(point);
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            visited.Add(startNode);
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(startNode);
            
            Vector2 projectedPoint = new Vector2(point.x, point.z);

            while (queue.Count > 0) {
                Vector2Int nowNode = queue.Dequeue();
                Node node = GetNode(nowNode);
                Vector2 projectedPosition = new Vector2(node.Position.x, node.Position.z);
                if (!CheckNodeIntersectsWithCircle(projectedPosition, projectedPoint, radius)) {
                    continue;
                }
                nodesCoordinates.Add(nowNode);
                foreach (Vector2Int coordinate in GetStraightNeighbours(nowNode)) {
                    if (!visited.Contains(coordinate)) {
                        visited.Add(coordinate);
                        queue.Enqueue(coordinate);
                    }
                }
            }

            return nodesCoordinates;
        }
        
        private IEnumerable<Vector2Int> GetStraightNeighbours(Vector2Int coordinate) {
            if (coordinate.x != 0) {
                yield return coordinate + Vector2Int.left;
            }

            if (coordinate.x != m_Width - 1) {
                yield return coordinate + Vector2Int.right;
            }

            if (coordinate.y != 0) {
                yield return coordinate + Vector2Int.down;
            }

            if (coordinate.y != m_Height - 1) {
                yield return coordinate + Vector2Int.up;
            }
        }

        private bool CheckNodeIntersectsWithCircle(Vector2 nodeCenterCoordinate, Vector2 circleCenter, float radius) {
            Vector2 offset = (Vector2.left + Vector2.down) * m_NodeSize / 2f;
            return Geometry.DoRectangleAndCircleIntersect(
                nodeCenterCoordinate - offset,
                nodeCenterCoordinate + offset,
                circleCenter,
                radius);
        }

        public List<Node> GetNodesInCircle(Vector3 point, float radius) {
            return GetNodeCoordinatesInCircle(point, radius).ConvertAll(GetNode);
        }

        public Node GetNodeAtPoint(Vector3 point) {
            return GetNode(GetNodeCoordinateAtPoint(point));
        }

        public void UpdatePathFinding() {
            m_PathFinding.UpdatePaths();
        }

        public Node GetStartNode() {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode() {
            return GetNode(m_TargetCoordinate);
        }

        public void SelectCoordinate(Vector2Int coordinate) {
            m_SelectedNode = GetNode(coordinate);
            m_SelectedNodeCoordinate = coordinate;
        }

        public void UnselectNode() {
            m_SelectedNode = null;
        }

        public Node GetSelectedNode() {
            return m_SelectedNode;
        }

        public Vector2Int GetSelectedNodeCoordinate() {
            return m_SelectedNodeCoordinate;
        }

        public bool HasSelectedNode() {
            return m_SelectedNode != null;
        }
        
        public bool CanOccupyNode(Vector2Int coordinate) {
            return m_PathFinding.CanOccupy(coordinate);
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

        public static bool InsideOfNode(Vector3 point, Node node, float nodeSize) {
            Vector2 offset = (Vector2.left + Vector2.up) * nodeSize / 2;
            Vector2 projectedCenter = new Vector2(node.Position.x, node.Position.z);
            Vector2 ptA = projectedCenter + offset;
            Vector2 ptB = projectedCenter - offset;
            Vector2 projectedPoint = new Vector2(point.x, point.y);
            return projectedPoint.x >= ptB.x && projectedPoint.y >= ptB.y &&
                   projectedPoint.x <= ptA.x && projectedPoint.y <= ptA.y;
        }
    }
}