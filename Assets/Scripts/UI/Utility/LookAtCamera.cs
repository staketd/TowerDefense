using System;
using UnityEngine;

namespace UI.Utility {
    public class LookAtCamera : MonoBehaviour {
        private void LateUpdate() {
            transform.forward = Camera.main.transform.forward;
        }
    }
}