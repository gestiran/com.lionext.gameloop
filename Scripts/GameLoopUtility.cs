using System;
using Lionext.GameLoop.Handles;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Lionext.GameLoop {
    public static class GameLoopUtility {
        private static readonly GameLoopHandle _handle;

        static GameLoopUtility() {
            _handle = new GameLoopHandle();
        #if UNITY_EDITOR
            Application.quitting += OnQuit;
        #endif
        }

        public static void AddLoop(PlayerLoopSystem system) => _handle.AddLoop(system);

        public static void AddLoop<LoopType>(PlayerLoopSystem system) => _handle.AddLoop<LoopType>(system);
        
        public static void AddLoop<LoopType, SubSystemType>(PlayerLoopSystem system) => _handle.AddLoop<LoopType, SubSystemType>(system);

        public static void RemoveLoop(PlayerLoopSystem system) => _handle.RemoveLoop(system);

        public static void RemoveLoop<SubSystemType>(PlayerLoopSystem system) => _handle.RemoveLoop<SubSystemType>(system);

        public static bool TryConnectToLoop<LoopType>(Action update) => _handle.TryConnectToLoop<LoopType>(update);
        
        public static bool TryConnectToLoop<LoopType, SubSystemType>(Action update) => _handle.TryConnectToLoop<LoopType, SubSystemType>(update);
        
        public static bool TryDisconnectFromLoop<LoopType>(Action update) => _handle.TryDisconnectFromLoop<LoopType>(update);
        
        public static bool TryDisconnectFromLoop<LoopType, SubSystemType>(Action update) => _handle.TryDisconnectFromLoop<LoopType, SubSystemType>(update);
        
        public static void SetDefaultLoop() => _handle.SetDefaultLoop();

        public static PlayerLoopSystem CreateSystem<T>(Action update) {
            PlayerLoopSystem system = new PlayerLoopSystem();

            system.type = typeof(T);
            system.updateDelegate = update.Invoke;

            return system;
        }
        
    #if UNITY_EDITOR
            
        private static void OnQuit() {
            _handle.SetDefaultLoop();
        }
            
    #endif
    }
}