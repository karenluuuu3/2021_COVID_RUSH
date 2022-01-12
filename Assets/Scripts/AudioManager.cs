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
        private AudioSource [] mAudioSource;
        private Dictionary<AudioType, AudioClip> mAudioDict = new Dictionary<AudioType, AudioClip>();

        [SerializeField]
        public AudioClip lostItem;
        [SerializeField]
        public AudioClip win;
        [SerializeField]
        public AudioClip lose;
        [SerializeField]
        public AudioClip countDown;
        [SerializeField]
        public AudioClip alteoDay;
        [SerializeField]
        public AudioClip alteoBeat;
        [SerializeField]
        public AudioClip WDGMix;
        [SerializeField]
        public AudioClip MPSQMix;

        public AudioManager Instance
        {
            get { return instance; }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                // DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            mAudioDict.Add(AudioType.CountDown, countDown);
            mAudioDict.Add(AudioType.Win, win);
            mAudioDict.Add(AudioType.Lose, lose);
            mAudioDict.Add(AudioType.LostItem, lostItem);

            mAudioSource = GetComponents<AudioSource>();
            mEventStore.Register("onPlayAudioSource", this, (_, p) => Play(p));
            mEventStore.Register("onLevelAudio", this, (_, p) => LevelAudio((int)p));
        }

        private void Play(object type)
        {
            mAudioSource[0].clip = mAudioDict[(AudioType)type];
            mAudioSource[0].PlayDelayed(0.5f);
        }

        private void LevelAudio(int level)
        {
            if (mAudioSource.Length < 3) return;
            if (mAudioSource[1].isPlaying) mAudioSource[1].Stop();
            if (mAudioSource[2].isPlaying) mAudioSource[2].Stop();

            switch (level)
            {
                case 1:
                    mAudioSource[1].clip = alteoDay;
                    mAudioSource[1].Play();
                    mAudioSource[2].clip = alteoBeat;
                    mAudioSource[2].Play();
                    break;
                case 2:
                    mAudioSource[1].clip = WDGMix;
                    mAudioSource[1].Play();
                    break;
                case 3:
                    mAudioSource[1].clip = MPSQMix;
                    mAudioSource[1].Play();
                    break;
                default:
                    break;
            }
        }
    }
}