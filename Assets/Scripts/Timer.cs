using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace COVID_RUSH
{
    public class Timer : MonoBehaviour
    {
        private class Bar
        {
            private GameObject mBody;
            private int mBarWidth = 5;

            public float height
            {
                set {
                    RectTransform rt = mBody.GetComponent<RectTransform>();
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
                }
            }

            public Bar(int posX=0, GameObject parent=null, float offsetX=0, float offsetY = 0)
            {
                if (parent == null) return;

                mBody = new GameObject();
                mBody.name = "Bar_" + posX.ToString();
                mBody.AddComponent<RawImage>();
                RectTransform rt = mBody.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0, 1);
                rt.localPosition = new Vector3(posX - offsetX, offsetY, 0);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mBarWidth);

                RawImage rawImage = mBody.GetComponent<RawImage>();
                rawImage.color = new Color32(255, 253, 219, 255); // = FFFDDB

                mBody.transform.SetParent(parent.transform, false);
            }
        }

        [SerializeField]
        public GameObject parent;
        [SerializeField]
        public int sampleInterval = 6;
        [SerializeField]
        public int frequencyScale = 100000;
        private EventStore mEventStore = EventStore.instance;
        private List<Bar> mBar = new List<Bar>();
        private int mBarUnit = 7;
        private int frameCount = 0;

        void Start()
        {
            mEventStore.Register("onTiming", this, (_, p) => TimingHandler(p));
            mEventStore.Register("onUpdateSpectrum", this, (_, p) => SpectrumHandler(p));
            
            RectTransform rt = (RectTransform)parent.transform;
            float width = rt.rect.width;
            float height = rt.rect.height;
            for (int i = 0; i * mBarUnit < width; i++)
            {
                mBar.Add(new Bar(i * mBarUnit, parent, width / 2, height / 2));
            }
        }

        private void FixedUpdate()
        {
            if (!canSample())
            {
                frameCount++;
            }
        }

        private bool canSample() { return frameCount >= sampleInterval;  }

        private void TimingHandler(object param)
        {
            
        }

        private float FrequencyToHeight(float freq)
        {
            return Mathf.Clamp(freq * frequencyScale, 10, 72);
        }

        private void SpectrumHandler(object param)
        {
            if (!canSample()) return;
            frameCount = 0;

            float [] frequency = (float[])param;
            int interval = Mathf.FloorToInt(frequency.Length / mBar.Count);
            for (int i = 0; i < mBar.Count; i++)
            {
                mBar[i].height = FrequencyToHeight(frequency[i * interval]);
            }
        }
    }
}
