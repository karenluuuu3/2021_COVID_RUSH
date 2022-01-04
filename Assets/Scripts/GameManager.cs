using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace COVID_RUSH
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private int mCurrentLevel = 1;
        private int currentVolume = 50;
        private int mCurrentTiming = 0;

        private EventStore EventManager = EventStore.instance;
        public enum GameState : int { Start, Information, Setting, Gaming, Wasted, LevelEnd, Ended }

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

        public bool IsLevelEnd() { return mCurrentTiming == 0; }
        public bool IsGaming() { return mGameState == GameState.Gaming;  }

        public void LevelEnd()
        {
            IEnumerator func()
            {
                mGameState = GameState.LevelEnd;
                ShowCongratulation();
                yield return new WaitForSeconds(2);
                
                // TODO: Add dashboard
            }

            StartCoroutine(func());
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
                ShowLoading();
                yield return new WaitForSeconds(2);
                // TODO: Use an enum to map state-to-code
                // 1 = scene of level 1
                EventManager.Notify("onLoadScene", this, mCurrentLevel);
                yield return new WaitForSeconds(1);
                StartCountdown();
                yield return new WaitForSeconds(4);
                mGameState = GameState.Gaming;
                while (!IsLevelEnd())
                {
                    yield return new WaitForSeconds(1);
                    SetTiming(mCurrentTiming - 1);
                }
                LevelEnd();
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
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SetLifeValue();
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

        private void SetLifeValue ()
        {
            EventManager.Notify("onSetLifeValueByDiff", this, -8);
        }
    }
}
