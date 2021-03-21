using Enemy;
using Resources;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace EnemySpawn {
    public class EnemySpawnController : IController {
        private SpawnWavesAsset m_SpawnWaves;
        private Grid m_Grid;
        private float m_SpawnStartTime;
        private float m_PassedTimeAtPreviousFrame = -1f;

        public EnemySpawnController(SpawnWavesAsset mSpawnWaves, Grid mGrid) {
            m_SpawnWaves = mSpawnWaves;
            m_Grid = mGrid;
        }

        public void OnStart() {
            m_SpawnStartTime = Time.time;
        }

        public void OnStop() {
        }

        public void Tick() {
            float passedTime = Time.time - m_SpawnStartTime;
            float timeToSpawn = 0f;

            foreach (SpawnWave spawnWave in m_SpawnWaves.SpawnWaves) {
                timeToSpawn += spawnWave.TimeBeforeStartWave;

                for (int i = 0; i < spawnWave.Count; ++i) {
                    if (passedTime >= timeToSpawn && m_PassedTimeAtPreviousFrame < timeToSpawn) {
                        SpawnEnemy(spawnWave.EnemyAsset);
                    }

                    if (i < spawnWave.Count - 1) {
                        timeToSpawn += spawnWave.TimeBetweenSpawns;
                    }
                }
                
            }

            
            m_PassedTimeAtPreviousFrame = passedTime;
        }

        private void SpawnEnemy(EnemyAsset asset) {
            EnemyView view = Object.Instantiate(asset.ViewPrefab);
            Vector3 startPosition = m_Grid.GetStartNode().Position;
            view.transform.position = new Vector3(startPosition.x, view.YCoordinate, startPosition.z);
            EnemyData data = new EnemyData(asset);
            
            data.AttachView(view);
            view.CreateMovementAgent(m_Grid);

            Game.Player.EnemySpawned(data);
        }
    }
}