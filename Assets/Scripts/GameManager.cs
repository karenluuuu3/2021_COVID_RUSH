using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace COVID_RUSH
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        private int mCurrentLevel = 0;
        private int currentVolume = 50;

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
            EventManager.Register("onPlayerDied", this, (_, p) => LevelLose());
            EventManager.Register("onLevelPass", this, (_, p) => LevelWin());
            EventManager.Register("onClosePopup", this, (_, p) => HandleCloseDashboard((CanvasManager.PopupType) p));
            EventManager.Register("onBackToMenu", this, (_, p) => HandleReset());
        }

        private void FixedUpdate()
        {
            // TODO: Remove this developer function
            MyDeveloperShortCut();
        }

        public bool IsGaming() {return mGameState == GameState.Gaming;}

        public void LevelWin()
        {
            IEnumerator func()
            {
                mGameState = GameState.LevelEnd;
                ShowCongratulation();
                yield return new WaitForSeconds(3);
                ShowScoreDashboard();
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
                SceneManager.LoadScene(++mCurrentLevel);
                yield return new WaitForSeconds(1);
                StartCountdown();
                yield return new WaitForSeconds(4);
                EventManager.Notify("onLevelAudio", this, mCurrentLevel);
                mGameState = GameState.Gaming;
                StartTiming();
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

        private void HandleCloseDashboard(CanvasManager.PopupType popupType)
        {
            if (popupType == CanvasManager.PopupType.Dashboard)
            {
                SwitchToNewGame();
            }
        }

        private void LevelLose()
        {
            IEnumerator func()
            {
                mGameState = GameState.LevelEnd;
                ShowWasted();
                yield return new WaitForSeconds(3);
                ShowScoreDashboard();
            }

            StartCoroutine(func());
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
            EventManager.Notify("onPlayAudioSource", this, AudioManager.AudioType.Lose);
        }
        private void ShowCongratulation()
        {
            EventManager.Notify("onPopupCongratulation", this, null);
            EventManager.Notify("onPlayAudioSource", this, AudioManager.AudioType.Win);
        }
        private void ShowLoading()
        {
            EventManager.Notify("onPopupLoading", this, null);
        }

        private void ShowScoreDashboard()
        {
            EventManager.Notify("onShowScoreDashboard", this, null);
        }

        private void StartCountdown()
        {
            EventManager.Notify("onPopupCountdown", this, null);
            EventManager.Notify("onPlayAudioSource", this, AudioManager.AudioType.CountDown);
        }
        private void StartTiming()
        {
            EventManager.Notify("onStartTiming", this, null);
        }

        private void HandleReset()
        {
            SceneManager.LoadScene(0);
            SwitchToStartScene();
        }

        private void Update()
        {
            if (GameManager.instance.IsGaming())
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
}
