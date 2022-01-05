using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance = new AudioManager();
        public EventStore mEventStore = EventStore.instance;
        public enum AudioType : int { CountDown, Win, Lose, LostItem };
        private AudioSource mAudioSource;
        private Dictionary<AudioType, AudioClip> mAudioDict = new Dictionary<AudioType, AudioClip>();

        [SerializeField]
        public AudioClip lostItem;
        [SerializeField]
        public AudioClip win;
        [SerializeField]
        public AudioClip lose;
        [SerializeField]
        public AudioClip countDown;

        public AudioManager Instance
        {
            get { return instance; }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            mAudioDict.Add(AudioType.CountDown, countDown);
            mAudioDict.Add(AudioType.Win, win);
            mAudioDict.Add(AudioType.Lose, lose);
            mAudioDict.Add(AudioType.LostItem, lostItem);

            mAudioSource = GetComponent<AudioSource>();
            mEventStore.Register("onPlayAudioSource", this, (_, p) => Play(p));
        }

        private void Play(object type)
        {
            mAudioSource.clip = mAudioDict[(AudioType)type];
            mAudioSource.PlayDelayed(0.5f);
        }
    }
}