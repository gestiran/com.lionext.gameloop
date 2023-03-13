using UnityEngine.LowLevel;

namespace Lionext.GameLoop.Handles {
    public class GameLoopHandle {
        private PlayerLoopSystem _currentLoop;
        
        private readonly PlayerLoopSystem _defaultLoop;

        public GameLoopHandle() {
            _defaultLoop = PlayerLoop.GetDefaultPlayerLoop();
            _currentLoop = PlayerLoop.GetCurrentPlayerLoop();
        }
        
        public void ConnectToLoop(PlayerLoopSystem system) {
            PlayerLoopSystem[] currentSystems = _currentLoop.subSystemList;

            PlayerLoopSystem[] newSystems = new PlayerLoopSystem[currentSystems.Length + 1];

            for (int systemId = 0; systemId < currentSystems.Length; systemId++) newSystems[systemId] = currentSystems[systemId];

            newSystems[currentSystems.Length] = system;

            _currentLoop.subSystemList = newSystems;
            PlayerLoop.SetPlayerLoop(_currentLoop);
        }

        public void DisconnectFromLoop(PlayerLoopSystem system) {
            PlayerLoopSystem[] currentSystems = _currentLoop.subSystemList;

            PlayerLoopSystem[] newSystems = new PlayerLoopSystem[currentSystems.Length - 1];

            for (int newSystemId = 0, systemId = 0; newSystemId < newSystems.Length; newSystemId++, systemId++) {
                if (currentSystems[systemId].type == typeof(PlayerLoopSystem)) {
                    systemId++;
                    continue;
                }

                newSystems[newSystemId] = currentSystems[systemId];
            }

            _currentLoop.subSystemList = newSystems;
            PlayerLoop.SetPlayerLoop(_currentLoop);
        }

        public void SetDefaultLoop() => PlayerLoop.SetPlayerLoop(_defaultLoop);
    }
}