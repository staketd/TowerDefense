using System;
using System.Collections.Generic;
using Enemy;
using EnemySpawn;
using Field;
using Main;
using Turret.Weapon.Projectile;
using TurretSpawn;
using UnityEngine;

namespace Runtime {
    public class Runner : MonoBehaviour {
        private List<IController> m_Controllers;

        private bool m_IsRunning = false;

        private void Update() {
            if (!m_IsRunning) {
                return;
            }
            TickControllers();
        }

        public void StartRunning() {
            CreateAllControllers();
            OnStartControllers();
            m_IsRunning = true;
        }
        
        public void StopRunning() {
            OnStopControllers();
            m_IsRunning = false;
        }

        private void CreateAllControllers() {
            m_Controllers = new List<IController> {
                new GridRaycastController(Game.Player.GridHolder),
                new EnemySpawnController(Game.CurrentLevel.SpawnWavesAsset, Game.Player.Grid),
                new TurretSpawnController(Game.Player.Grid, Game.Player.TurretMarket),
                new MovementController(),
                new EnemyReachController(Game.Player.Grid),
                new TurretShootController(),
                new EnemyDeathController(),
                new LoseController(),
                new WinController()
            };
        }

        private void OnStartControllers() {
            m_Controllers.ForEach(controller => {
                try {
                    controller.OnStart();
                }
                catch (Exception e) {
                    Debug.LogError(e);
                }
            });
        }
        
        private void TickControllers() {
            m_Controllers.ForEach(controller => {
                if (!m_IsRunning) {
                    return;
                }
                try {
                    controller.Tick();
                }
                catch (Exception e) {
                    Debug.LogError(e);
                }
            });
        }

        private void OnStopControllers() {
            m_Controllers.ForEach(controller => {
                try {
                    controller.OnStop();
                }
                catch (Exception e) {
                    Debug.LogError(e);
                }
            });
        }
    }
}