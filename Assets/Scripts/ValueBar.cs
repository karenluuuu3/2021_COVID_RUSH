using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace COVID_RUSH
{
    public class ValueBar : MonoBehaviour
    {
        [SerializeField]
        public string barName;
        [SerializeField]
        public float value = 100;
        private float mNextValue;
        private float mStep = 0.1f;
        private RectTransform rectangle;
        private float width;
        private EventStore mEventStore = EventStore.instance;

        public struct UpdateFormat
        {
            public string name;
            public float value;
        }

        private void Start()
        {
            rectangle = gameObject.GetComponent<RectTransform>();
            width = rectangle.rect.width;
            mNextValue = value;
            SetValue(value);
            mEventStore.Register("onSetBarValueByDiff", this, (_, p) => SetNextValueByDiff(p));
            mEventStore.Register("onSetFixedBarValue", this, (_, p) => SetFixedBarValue(p));
        }
        
        void FixedUpdate()
        {
            if (value < mNextValue)
            {
                SetValue(value + mStep);
            }
            else if (value > mNextValue)
            {
                SetValue(value - mStep);
            }
        }

        private void SetNextValueByDiff(object upf)
        {
            UpdateFormat v = (UpdateFormat)upf;
            if (barName != v.name) return;
            float f = Convert.ToSingle(v.value);
            SetNextValue(mNextValue + f);
        }

        private void SetNextValue(float f)
        {
            mNextValue = Mathf.Clamp(f, 0.0f, 100.0f);
        }

        private void SetFixedBarValue(object upf)
        {
            UpdateFormat v = (UpdateFormat)upf;
            if (barName != v.name) return;
            float f = Convert.ToSingle(v.value);
            SetValue(f);
            SetNextValue(f);
        }

        private void SetValue (float v)
        {
            value = Mathf.Clamp(v, 0.0f, 100.0f);
            if (value <= 0.0f)
            {
                mEventStore.Notify("onBarZeroing", this, barName);
            }
            rectangle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Round(value / 100 * width));
        }
    }
}
