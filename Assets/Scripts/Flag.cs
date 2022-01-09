using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public class Flag : MonoBehaviour
    {
        private EventStore mEventStore = EventStore.instance;
        private Renderer marker;

        void Start()
        {
            mEventStore.Register("onCreateFlag", this, (_, p) => SetFlag());
            Vector3 p = gameObject.transform.position;
            gameObject.transform.position = new Vector3(p.x, -10, p.z);
            GameObject obj = GameObject.Find("Circle");
            marker = obj.GetComponent<Renderer>();
            Color c = marker.material.color;
            c.a = 0;
            marker.material.color = c;
        }

        private void OnDestroy()
        {
            mEventStore.RemoveLisenterFromAllEvent(this);
        }

        private void SetFlag()
        {
            Vector3 pos = gameObject.transform.position;
            Vector3 p = gameObject.transform.position;
            gameObject.transform.position = new Vector3(p.x, 0.4f, p.z);
            Color c = marker.material.color;
            c.a = 1;
            marker.material.color = c;
            mEventStore.Notify("onSetFlag", this, new Vector2(pos.x, pos.z));
        }
    }
}
