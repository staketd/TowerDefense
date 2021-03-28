using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Field {
    public class Node {

        public Vector3 Position;
        public Node NextNode;
        public bool IsOccupied;
        public float PathWeight;
        public OccupationAvailability Availability;
        public HashSet<EnemyData> EnemyDatas = new HashSet<EnemyData>();

        public Node(Vector3 position) {
            Position = position;
        }

        public void ResetWeight() {
            PathWeight = float.MaxValue;
            NextNode = this;
        }
    }
}