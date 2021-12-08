using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public class CanvasManager : MonoBehaviour
    {
        private Animator mAnimator;
        private EventStore mEventStore = EventStore.instance;
        public enum PopupType { Idle, Loading, Congratulation, Wasted };

        void Start()
        {
            mAnimator = GetComponent<Animator>();
            mEventStore.Register("onPopupWasted", this, (_, p) => StartCoroutine(Popup(PopupType.Wasted)));
            mEventStore.Register("onPopupCongratulation", this, (_, p) => StartCoroutine(Popup(PopupType.Congratulation)));
            mEventStore.Register("onPopupLoading", this, (_, p) => StartCoroutine(Popup(PopupType.Loading)));
        }

        public IEnumerator Popup(object popupType)
        {
            mAnimator.SetInteger("popupType", (int)popupType);
            yield return new WaitForSeconds(2);
            mAnimator.SetInteger("popupType", (int)PopupType.Idle);
        }
    }
}
