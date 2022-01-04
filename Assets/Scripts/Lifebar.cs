using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace COVID_RUSH
{
    public class Lifebar : MonoBehaviour
    {
        private float mValue = 100;
        private float mNextValue = 100;
        private float mStep = 1.0f;
        private RectTransform rectangle;
        private float width;
        private EventStore mEventStore = EventStore.instance;

        private void Start()
        {
            rectangle = gameObject.GetComponent<RectTransform>();
            width = rectangle.rect.width;
            mEventStore.Register("onSetLifeValueByDiff", this, (_, v) => SetNextValueByDiff(v));
        }
        
        void FixedUpdate()
        {
            if (mValue < mNextValue)
            {
                SetValue(mValue + mStep);
            }
            else if (mValue > mNextValue)
            {
                SetValue(mValue - mStep);
            }
        }

        private void SetNextValueByDiff(object diff)
        {
            float f = Convert.ToSingle(diff);
            SetNextValue(mNextValue + f);
        }

        private void SetNextValue(object v)
        {
            float f = Convert.ToSingle(v);
            mNextValue = Mathf.Clamp(f, 0.0f, 100.0f);
        }

        private void SetValue (float v)
        {
            mValue = Mathf.Clamp(v, 0.0f, 100.0f);
            rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Round(mValue / 100 * width));
        }
    }
}
