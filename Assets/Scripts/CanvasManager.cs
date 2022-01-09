using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public class CanvasManager : MonoBehaviour
    {
        private Animator mAnimator;
        private EventStore mEventStore = EventStore.instance;
        public enum PopupType { Idle, Loading, Congratulation, Wasted, Countdown, Dashboard };

        void Start()
        {
            mAnimator = GetComponent<Animator>();
            mEventStore.Register("onPopupWasted", this, (_, p) => StartCoroutine(Popup(PopupType.Wasted)));
            mEventStore.Register("onPopupCongratulation", this, (_, p) => StartCoroutine(Popup(PopupType.Congratulation)));
            mEventStore.Register("onPopupLoading", this, (_, p) => StartCoroutine(Popup(PopupType.Loading)));
            mEventStore.Register("onPopupCountdown", this, (_, p) => StartCoroutine(Popup(PopupType.Countdown, 4)));
            mEventStore.Register("onShowScoreDashboard", this, (_, p) => Show(PopupType.Dashboard));
        }

        private void OnDestroy()
        {
            mEventStore.RemoveLisenterFromAllEvent(this);
        }

        public IEnumerator Popup(object popupType, int duration=2)
        {
            mAnimator.SetInteger("popupType", (int)popupType);
            yield return new WaitForSeconds(duration);
            mAnimator.SetInteger("popupType", (int)PopupType.Idle);
        }

        public void Show(object popupType)
        {
            mAnimator.SetInteger("popupType", (int)popupType);

            /*if ((PopupType)popupType == PopupType.Dashboard)
            {
                IEnumerator func()
                {
                    yield return new WaitForSeconds(4);
                    mEventStore.Notify("onRanking", this, null);
                }
                StartCoroutine(func());
            }*/
        }

        public void Hide(int duration=2)
        {
            IEnumerator func()
            {
                mAnimator.SetInteger("popupType", (int)PopupType.Idle);
                yield return new WaitForSeconds(duration);
                mEventStore.Notify("onClosePopup", this, PopupType.Dashboard);
            }

            StartCoroutine(func());
        }

        public void OnBackToMenu()
        {
            mEventStore.Notify("onBackToMenu", this, null);
        }
    }
}
