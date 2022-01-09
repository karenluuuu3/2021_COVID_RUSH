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
            gameObject.transform.position -= new Vector3(0, 10, 0);
            GameObject obj = GameObject.Find("Circle"); //.gameObject.GetComponent<Renderer>();
            Debug.Log(obj);
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
            Debug.Log(pos);
            gameObject.transform.position = pos + new Vector3(0, 10, 0);
            Color c = marker.material.color;
            c.a = 1;
            marker.material.color = c;
            mEventStore.Notify("onSetFlag", this, new Vector2(pos.x, pos.z));
        }
    }
}
