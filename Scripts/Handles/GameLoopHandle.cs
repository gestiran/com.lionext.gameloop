using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.LowLevel;

namespace Lionext.GameLoop.Handles {
    public class GameLoopHandle {
        private PlayerLoopSystem _currentLoop;
        
        private readonly List<PlayerLoopSystem> _systems;
        private readonly PlayerLoopSystem _defaultLoop;
        
        public GameLoopHandle() {
            _defaultLoop = PlayerLoop.GetDefaultPlayerLoop();
            _currentLoop = PlayerLoop.GetCurrentPlayerLoop();

            _systems = _currentLoop.subSystemList.ToList();
        }

        public void AddLoop(PlayerLoopSystem system) {
            _systems.Add(system);
            ApplyLoop();
        }

        public void AddLoop<LoopType>(PlayerLoopSystem system) {
            Type loopType = typeof(LoopType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                
                _systems.Insert(systemId, system);
                return;
            }

            AddLoop(system);
            ApplyLoop();
        }
        
        public void AddLoop<LoopType, SubSystemType>(PlayerLoopSystem system) {
            Type loopType = typeof(LoopType);
            Type subSystemType = typeof(SubSystemType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                if (_systems[systemId].subSystemList == null) break;

                PlayerLoopSystem root = _systems[systemId];
                List<PlayerLoopSystem> subSystems = root.subSystemList.ToList();
                
                for (int subSystemId = 0; subSystemId < subSystems.Count; subSystemId++) {
                    if (_systems[systemId].type != subSystemType) continue;
                    
                    subSystems.Insert(subSystemId, system);
                    root.subSystemList = subSystems.ToArray();
                    _systems[systemId] = root;
                    return;
                }
            }

            AddLoop(system);
            ApplyLoop();
        }

        public void RemoveLoop(PlayerLoopSystem system) {
            _systems.Remove(system);
            ApplyLoop();
        }

        public void RemoveLoop<SubSystemType>(PlayerLoopSystem system) {
            Type loopType = typeof(SubSystemType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                if (_systems[systemId].subSystemList == null) break;

                PlayerLoopSystem root = _systems[systemId];
                List<PlayerLoopSystem> subSystems = root.subSystemList.ToList();
                subSystems.Remove(system);
                root.subSystemList = subSystems.ToArray();
                _systems[systemId] = root;
                ApplyLoop();
            }
        }

        public bool TryConnectToLoop<LoopType>(Action update) {
            Type loopType = typeof(LoopType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                
                PlayerLoopSystem root = _systems[systemId];
                root.updateDelegate += update.Invoke;
                _systems[systemId] = root;
                ApplyLoop();
                return true;
            }

            return false;
        }
        
        public bool TryConnectToLoop<LoopType, SubSystemType>(Action update) {
            Type loopType = typeof(LoopType);
            Type subSystemType = typeof(SubSystemType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                if (_systems[systemId].subSystemList == null) return false;

                PlayerLoopSystem root = _systems[systemId];

                for (int subSystemId = 0; subSystemId < root.subSystemList.Length; subSystemId++) {
                    if (_systems[systemId].type != subSystemType) continue;
                    
                    root.subSystemList[subSystemId].updateDelegate += update.Invoke;
                    _systems[systemId] = root;
                    ApplyLoop();
                    return true;
                }
            }

            return false;
        }
        
        public bool TryDisconnectFromLoop<LoopType>(Action update) {
            Type loopType = typeof(LoopType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                
                PlayerLoopSystem root = _systems[systemId];
                root.updateDelegate -= update.Invoke;
                _systems[systemId] = root;
                ApplyLoop();
                return true;
            }

            return false;
        }
        
        public bool TryDisconnectFromLoop<LoopType, SubSystemType>(Action update) {
            Type loopType = typeof(LoopType);
            Type subSystemType = typeof(SubSystemType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                if (_systems[systemId].subSystemList == null) return false;

                PlayerLoopSystem root = _systems[systemId];

                for (int subSystemId = 0; subSystemId < root.subSystemList.Length; subSystemId++) {
                    if (_systems[systemId].type != subSystemType) continue;
                    
                    root.subSystemList[subSystemId].updateDelegate -= update.Invoke;
                    _systems[systemId] = root;
                    ApplyLoop();
                    return true;
                }
            }

            return false;
        }
        
        public void SetDefaultLoop() => PlayerLoop.SetPlayerLoop(_defaultLoop);

        private void ApplyLoop() {
            _currentLoop.subSystemList = _systems.ToArray();
            PlayerLoop.SetPlayerLoop(_currentLoop);
        }
    }
}