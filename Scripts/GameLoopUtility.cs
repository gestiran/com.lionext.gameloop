using System;
using Lionext.GameLoop.Handles;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Lionext.GameLoop {
    public class GameLoopUtility {
        private static readonly GameLoopHandle _handle;

        static GameLoopUtility() {
            _handle = new GameLoopHandle();
        #if UNITY_EDITOR
            Application.quitting += OnQuit;
        #endif
        }

        public static bool TryAddLoop(PlayerLoopSystem system) => _handle.TryAddLoop(system);

        public static bool TryAddLoop<LoopType>(PlayerLoopSystem system) => _handle.TryAddLoop<LoopType>(system);

        public static bool TryRemoveLoop(PlayerLoopSystem system) => _handle.TryRemoveLoop(system);

        public static bool TryRemoveLoop<SubSystemType>(PlayerLoopSystem system) => _handle.TryRemoveLoop<SubSystemType>(system);

        public static bool TryConnectToLoop<LoopType>(Action update) => _handle.TryConnectToLoop<LoopType>(update);

        public static bool TryDisconnectFromLoop<LoopType>(Action update) => _handle.TryDisconnectFromLoop<LoopType>(update);

        public static void SetDefaultLoop() => _handle.SetDefaultLoop();

        public static PlayerLoopSystem CreateSystem<T>(Action update) {
            PlayerLoopSystem system = new PlayerLoopSystem();

            system.type = typeof(T);
            system.updateDelegate = update.Invoke;

            return system;
        }

        public static string CurrentLoopToString() => _handle.ToString();

    #if UNITY_EDITOR
            
        private static void OnQuit() {
            _handle.SetDefaultLoop();
        }
            
    #endif
    }
}