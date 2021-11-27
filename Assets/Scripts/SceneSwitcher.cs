using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public class SceneSwitcher : MonoBehaviour
    {
        private Animator mAnimator;
        private EventStore mEventStore = EventStore.instance;

        void Start()
        {
            mAnimator = GetComponent<Animator>();
            mEventStore.Register("onSceneSwitch", this, (_, param) => Switch(param));
        }
        public void Switch(object gameState)
        {
            mAnimator.SetInteger("GameState", (int)gameState);
        }
    }
}

