using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            mEventStore.Register("onLoadScene", this, (_, param) => Load(param));
        }
        private void OnDestroy()
        {
            mEventStore.RemoveLisenterFromAllEvent(this);
        }

        public void Switch(object gameState)
        {
            mAnimator.SetInteger("GameState", (int)gameState);
        }

        public void Load(object sceneIdx)
        {
            SceneManager.LoadScene((int)sceneIdx);
        }
    }
}

