using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public class GameManager : MonoBehaviour
    {
        private enum GameState : byte { Start, Gaming, Ended }

        private int CurrentLevel = 1;

        // private EventStore EventManager = new EventStore();

        private void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
