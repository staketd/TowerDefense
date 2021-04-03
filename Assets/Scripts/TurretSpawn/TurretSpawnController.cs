using Field;
using Runtime;
using Turret;
using UnityEngine;
using Grid = Field.Grid;

namespace TurretSpawn {
    public class TurretSpawnController : IController {
        private Grid m_Grid;
        private TurretMarket m_Market;

        public TurretSpawnController(Grid grid, TurretMarket market) {
            m_Grid = grid;
            m_Market = market;
        }

        public void OnStart() {
            
        }

        public void OnStop() {
        }

        public void Tick() {
            if (!m_Grid.HasSelectedNode() || !Input.GetMouseButtonDown(0)) {
                return;
            }
            Node selectedNode = m_Grid.GetSelectedNode();
            Vector2Int selectedNodeCoordinate = m_Grid.GetSelectedNodeCoordinate();
            if (selectedNode.IsOccupied || !m_Grid.CanOccupyNode(selectedNodeCoordinate)) {
                return;
            }
            SpawnTurret(m_Market.ChosenTurret, selectedNodeCoordinate, selectedNode);
        }

        private void SpawnTurret(TurretAsset asset, Vector2Int coordinate, Node selectedNode) {
            TurretView view = Object.Instantiate(asset.ViewPrefab);
            TurretData data = new TurretData(asset, selectedNode);
            data.AttachView(view);
            Game.Player.TurretSpawned(data);
            if (!m_Grid.TryOccupyNode(coordinate, true)) {
                Debug.Log("Something has gone wrong");
            }
            m_Grid.UpdatePathFinding();
        }
    }
}