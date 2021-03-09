using System;
using System.Collections;
using Field;
using UnityEngine;

namespace Unit {
    public class UnitSpawner : MonoBehaviour {
        [SerializeField]
        private GridMovementAgent m_MovementAgent;

        [SerializeField]
        private GridHolder m_GridHolder;

        private void Awake() {
            StartCoroutine(SpawnUnitsCoroutine());
        }

        private IEnumerator SpawnUnitsCoroutine() {
            while (true) {
                yield return new WaitForSeconds(1f);
                SpawnUnit();
            }
        }

        private void SpawnUnit() {
            Node startNode = m_GridHolder.Grid.GetNode(m_GridHolder.StartCoordinate);
            GridMovementAgent movementAgent = Instantiate(m_MovementAgent, startNode.Position, Quaternion.identity);
            movementAgent.SetStartNode(startNode);
        }
    }
}