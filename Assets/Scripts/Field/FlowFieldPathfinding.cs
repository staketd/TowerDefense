using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Field {
    public class FlowFieldPathfinding {
        private Grid m_Grid;
        private Vector2Int m_Start;
        private Vector2Int m_FinalTarget;

        public FlowFieldPathfinding(Grid mGrid, Vector2Int mStart, Vector2Int mFinalTarget) {
            m_Grid = mGrid;
            m_Start = mStart;
            m_FinalTarget = mFinalTarget;
        }

        public void UpdatePaths() {
            ResetAllWeights();

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            m_Grid.GetNode(m_FinalTarget).PathWeight = 0f;
            queue.Enqueue(m_FinalTarget);

            while (queue.Count > 0) {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);
                foreach (Vector2Int neighbour in GetNeighbours(current)) {
                    Node neighbourNode = m_Grid.GetNode(neighbour);
                    if (!m_Grid.CheckCoordinate(neighbour) || neighbourNode.IsOccupied) {
                        continue;
                    }

                    if ((current - neighbour).magnitude > 1) {
                        // checking adjacent nodes
                        if (GetNeighbours(current).Any(adj =>
                            CheckAdjacent(adj, current) && CheckAdjacent(adj, neighbour) &&
                            m_Grid.GetNode(adj).IsOccupied)) {
                            continue;
                        }
                    }

                    if (currentNode.PathWeight + (current - neighbour).magnitude < neighbourNode.PathWeight) {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = currentNode.PathWeight + (current - neighbour).magnitude;
                        queue.Enqueue(neighbour);
                    }
                }
            }

            CalculateAccessibility();
        }

        public IEnumerable<Vector2Int> GetNeighbours(Vector2Int coordinate) {
            yield return coordinate + Vector2Int.left;
            yield return coordinate + Vector2Int.up;
            yield return coordinate + Vector2Int.right;
            yield return coordinate + Vector2Int.down;
            yield return coordinate + Vector2Int.left + Vector2Int.up;
            yield return coordinate + Vector2Int.left + Vector2Int.down;
            yield return coordinate + Vector2Int.right + Vector2Int.up;
            yield return coordinate + Vector2Int.right + Vector2Int.down;
        }

        private void ResetAllWeights() {
            foreach (Node node in m_Grid.EnumerateAllNodes()) {
                node.ResetWeight();
            }
        }

        private static bool CheckAdjacent(Vector2Int coordinate1, Vector2Int coordinate2) {
            return (coordinate1 - coordinate2).sqrMagnitude == 1;
        }

        public bool CanOccupy(Vector2Int coordinate) {
            Node node = m_Grid.GetNode(coordinate);
            switch (node.Availability) {
                case OccupationAvailability.CanOccupy:
                    return true;
                case OccupationAvailability.CanNotOccupy:
                    return false;
                case OccupationAvailability.Undefined:
                    bool hasOtherPath = CheckForPath(coordinate);
                    node.Availability = hasOtherPath ? OccupationAvailability.CanOccupy : OccupationAvailability.CanNotOccupy;
                    return hasOtherPath;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private bool CheckForPath(Vector2Int coordinate) {
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            visited.Add(coordinate);
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(m_Start);
            visited.Add(m_Start);

            while (queue.Count > 0) {
                Vector2Int current = queue.Dequeue();
                foreach (Vector2Int neighbour in GetNeighbours(current)) {
                    Node neighbourNode = m_Grid.GetNode(neighbour);
                    if (!m_Grid.CheckCoordinate(neighbour) || neighbourNode.IsOccupied || visited.Contains(neighbour)) {
                        continue;
                    }

                    if ((current - neighbour).magnitude > 1) {
                        // checking adjacent nodes
                        if (GetNeighbours(current).Any(adj =>
                            CheckAdjacent(adj, current) && CheckAdjacent(adj, neighbour) &&
                            (m_Grid.GetNode(adj).IsOccupied) || visited.Contains(adj))) {
                            continue;
                        }
                    }

                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                }
            }

            return visited.Contains(m_FinalTarget);
        }

        private void CalculateAccessibility() {
            foreach (Node node in m_Grid.EnumerateAllNodes()) {
                if (node.IsOccupied) {
                    node.Availability = OccupationAvailability.CanNotOccupy;
                } else {
                    node.Availability = OccupationAvailability.CanOccupy;
                }
            }

            Node startNode = m_Grid.GetNode(m_Start);
            Node finalNode = m_Grid.GetNode(m_FinalTarget);
            startNode.Availability = OccupationAvailability.CanNotOccupy;
            finalNode.Availability = OccupationAvailability.CanNotOccupy;
            startNode = startNode.NextNode;
            while (startNode != finalNode) {
                startNode.Availability = OccupationAvailability.Undefined;
                startNode = startNode.NextNode;
            }
        }
    }
}