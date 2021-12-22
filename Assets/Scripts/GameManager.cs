using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace COVID_RUSH
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private int CurrentLevel = 1;
        private int currentVolume = 50;
        private int mCurrentTiming = 0;

        private EventStore EventManager = EventStore.instance;
        public enum GameState : int { Start, Information, Setting, Gaming, Wasted, Ended }

        private GameState mGameState = GameState.Start;

        public GameManager Instance
        {
            get { return instance; }
        }

        public GameState gameState
        {
            get { return mGameState;  }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            EventManager.Register("showWasted", this, (c,p) => ShowWasted());
            EventManager.Register("onSetLevelTiming", this, (_, p) => SetTiming(p));
        }

        private void FixedUpdate()
        {
            // TODO: Remove this developer function
            MyDeveloperShortCut();
        }

        public void SwitchToInformationScene()
        {
            mGameState = GameState.Information;
            EventManager.Notify("onSceneSwitch", this, mGameState);
        }

        public void SwitchToStartScene()
        {
            mGameState = GameState.Start;
            EventManager.Notify("onSceneSwitch", this, mGameState);
        }
        public void SwitchToSettingScene()
        {
            mGameState = GameState.Setting;
            EventManager.Notify("onSceneSwitch", this, mGameState);
        }

        public void SwitchToNewGame()
        {
            IEnumerator func()
            {
                mGameState = GameState.Gaming;
                ShowLoading();
                yield return new WaitForSeconds(2);
                // TODO: Use an enum to map state-to-code
                // 1 = scene of level 1
                EventManager.Notify("onLoadScene", this, 1);
            }

            StartCoroutine(func());
        }

        public void SetVolume(float volume)
        {
            currentVolume = (int) volume;
            Dictionary<string, string> variableDict = new Dictionary<string, string>();
            variableDict.Add("volume", currentVolume.ToString() + "%");
            EventManager.Notify("onVariableChange", this, variableDict);
        }

        private void MyDeveloperShortCut()
        {
            if (mGameState != GameState.Gaming) return;
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ShowWasted();
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                ShowCongratulation();
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                ShowLoading();
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                StartCountdown();
            }
        }

        private void ShowWasted()
        {
            EventManager.Notify("onPopupWasted", this, null);
        }
        private void ShowCongratulation()
        {
            EventManager.Notify("onPopupCongratulation", this, null);
        }
        private void ShowLoading()
        {
            EventManager.Notify("onPopupLoading", this, null);
        }
        private void StartCountdown()
        {
            EventManager.Notify("onPopupCountdown", this, null);
        }

        private void SetTiming(object timing)
        {
            mCurrentTiming = (int)timing;
            int min = Mathf.FloorToInt(mCurrentTiming / 60);
            int sec = mCurrentTiming % 60;
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "timing", (min < 10 ? "0" : "") + min.ToString() + ":" + (sec < 10 ? "0" : "") + sec.ToString() },
            };
            EventManager.Notify("onVariableChange", this, dict);
        }
    }
}
