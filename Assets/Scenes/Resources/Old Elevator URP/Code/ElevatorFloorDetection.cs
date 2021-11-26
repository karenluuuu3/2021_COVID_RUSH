using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldElevator {
    public class ElevatorFloorDetection : MonoBehaviour {
        public ElevatorBrain brain;

        void OnTriggerEnter(Collider col) {
            if (col.transform == brain.player) brain.playerInsideElevator = true;
        }

        void OnTriggerExit(Collider col) {
            if (col.transform == brain.player) brain.playerInsideElevator = false;
        }
    }
}