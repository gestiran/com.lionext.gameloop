using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool TryAddLoop(PlayerLoopSystem system) {
            _systems.Add(system);
            ApplyLoop();
            return true;
        }

        public bool TryAddLoop<LoopType>(PlayerLoopSystem system) {
            Type loopType = typeof(LoopType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;

                PlayerLoopSystem loop = _systems[systemId];

                if (loop.subSystemList == null) loop.subSystemList = new[] { system };
                else {
                    List<PlayerLoopSystem> subSystems = loop.subSystemList.ToList();
                    subSystems.Add(system);
                    loop.subSystemList = subSystems.ToArray();
                }

                _systems[systemId] = loop;
                ApplyLoop();
                return true;
            }
            
            return false;
        }

        public bool TryRemoveLoop(PlayerLoopSystem system) {
            _systems.Remove(system);
            ApplyLoop();
            return true;
        }

        public bool TryRemoveLoop<LoopType>(PlayerLoopSystem system) {
            Type loopType = typeof(LoopType);
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                if (_systems[systemId].type != loopType) continue;
                if (_systems[systemId].subSystemList == null) break;

                PlayerLoopSystem root = _systems[systemId];
                List<PlayerLoopSystem> subSystems = root.subSystemList.ToList();
                subSystems.Remove(system);
                root.subSystemList = subSystems.ToArray();
                _systems[systemId] = root;
                ApplyLoop();
                return true;
            }

            return false;
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
        
        public void SetDefaultLoop() => PlayerLoop.SetPlayerLoop(_defaultLoop);

        private void ApplyLoop() {
            _currentLoop.subSystemList = _systems.ToArray();
            PlayerLoop.SetPlayerLoop(_currentLoop);
        }

        public override string ToString() {
            StringBuilder result = new StringBuilder(_systems.Count);

            const int namespaceOffset = 23;
            
            for (int systemId = 0; systemId < _systems.Count; systemId++) {
                result.Append($"{systemId:00}");

                string typeText = _systems[systemId].type.ToString();
                if (typeText.Contains("UnityEngine")) typeText = typeText.Substring(namespaceOffset);

                result.AppendLine($" - {typeText}");
                
                if (_systems[systemId].subSystemList == null) continue;
                
                for (int subSystemId = 0; subSystemId < _systems[systemId].subSystemList.Length; subSystemId++) {
                    result.Append($"{systemId:00}:{subSystemId:00}");

                    typeText = _systems[systemId].subSystemList[subSystemId].type.ToString();
                    if (typeText.Contains("UnityEngine")) typeText = typeText.Substring(namespaceOffset);
                    
                    result.AppendLine($" - {typeText}");
                }
            }
            
            return result.ToString();
        }
    }
}