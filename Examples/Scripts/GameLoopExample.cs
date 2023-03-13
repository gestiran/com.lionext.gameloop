using Lionext.GameLoop;
using UnityEngine;

namespace LionextExample.GameLoop {
    public class GameLoopExample : MonoBehaviour {
        private void Start() {
            Debug.LogError(GameLoopUtility.CurrentLoopToString());
        }
    }
}