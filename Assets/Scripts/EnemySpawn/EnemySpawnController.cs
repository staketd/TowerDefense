using System.Collections;
using Enemy;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace EnemySpawn {
    public class EnemySpawnController : IController {
        private SpawnWavesAsset m_SpawnWaves;
        private Grid m_Grid;

        private IEnumerator m_SpawnRoutine;
        private float m_WaitTime;

        public EnemySpawnController(SpawnWavesAsset mSpawnWaves, Grid mGrid) {
            m_SpawnWaves = mSpawnWaves;
            m_Grid = mGrid;
        }

        public void OnStart() {
            m_SpawnRoutine = SpawnRoutine();
        }

        public void OnStop() {
        }

        public void Tick() {
            if (m_WaitTime > Time.time) {
                return;
            }
            
            if (m_SpawnRoutine.MoveNext()) {
                if (m_SpawnRoutine.Current is CustomWaitForSeconds waitForSeconds) {
                    m_WaitTime = Time.time + waitForSeconds.Seconds;
                }
            }
        }

        private IEnumerator SpawnRoutine() {
            foreach (SpawnWave spawnWave in m_SpawnWaves.SpawnWaves) {
                yield return new CustomWaitForSeconds(spawnWave.TimeBeforeStartWave);

                for (int i = 0; i < spawnWave.Count; ++i) {
                    SpawnEnemy(spawnWave.EnemyAsset);
                    if (i < spawnWave.Count - 1) {
                        yield return new CustomWaitForSeconds(spawnWave.TimeBetweenSpawns);
                    }
                }
                
                //todo show wave number
            }
            Game.Player.LastWaveSpawned();
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

        private class CustomWaitForSeconds {
            public readonly float Seconds;

            public CustomWaitForSeconds(float seconds) {
                Seconds = seconds;
            }
            
        }
    }
}