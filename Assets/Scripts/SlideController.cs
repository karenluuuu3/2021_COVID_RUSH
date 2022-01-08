using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public class SlideController : MonoBehaviour
    {
        private Animator mAnimator;
        private EventStore mEventStore = EventStore.instance;
        private int mCurrentlideIdx = 0;
        [SerializeField]
        public int totalSlideCount = 5;
        [SerializeField]
        public GameObject prevChevron;
        [SerializeField]
        public GameObject nextChevron;

        public void NextSlide()
        {
            if (mCurrentlideIdx == totalSlideCount - 1)
            {
                nextChevron.SetActive(false);
                return;
            }
            mCurrentlideIdx++;
            nextChevron.SetActive(true);
        }

        private void PrevSlide(int nextSlideIdx)
        {
            if (mCurrentlideIdx == 0)
            {
                prevChevron.SetActive(false);
                return;
            }
            mCurrentlideIdx--;
            prevChevron.SetActive(true);
        }
    }
}
