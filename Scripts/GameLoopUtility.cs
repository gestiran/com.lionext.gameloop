using Lionext.GameLoop.Handles;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Lionext.GameLoop {
    public static class GameLoopUtility {
        private static GameLoopHandle _handle;

        static GameLoopUtility() {
            _handle = new GameLoopHandle();
        #if UNITY_EDITOR
            Application.quitting += OnQuit;
        #endif
        }

        public static void ConnectToLoop(PlayerLoopSystem system) => _handle.ConnectToLoop(system);

        public static void DisconnectFromLoop(PlayerLoopSystem system) => _handle.DisconnectFromLoop(system);
        
        public static void SetDefaultLoop() => _handle.SetDefaultLoop();
        
    #if UNITY_EDITOR
            
        private static void OnQuit() {
            _handle.SetDefaultLoop();
        }
            
    #endif
    }
}