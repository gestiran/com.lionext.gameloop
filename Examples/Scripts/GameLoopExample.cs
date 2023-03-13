using System.Collections;
using Lionext.GameLoop;
using UnityEngine;

namespace LionextExample.GameLoop {
    public class GameLoopExample : MonoBehaviour {
        private IEnumerator Start() {
            yield return new WaitForSecondsRealtime(1);
            Debug.LogError(GameLoopUtility.CurrentLoopToString());
        }
    }
}