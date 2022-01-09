using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH {
    public class RankParticle : MonoBehaviour
    {
        private ParticleSystem ps;
        private EventStore mEventStore = EventStore.instance;

        void Start()
        {
            ps = GetComponent<ParticleSystem>();
            mEventStore.Register("onRanking", this, (_, p) => Emit());
        }

        private void OnDestroy()
        {
            mEventStore.RemoveLisenterFromAllEvent(this);
        }

        private void Emit()
        {
            ps.Play();
        }
    }
}
