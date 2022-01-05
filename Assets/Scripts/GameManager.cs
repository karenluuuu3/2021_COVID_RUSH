using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace COVID_RUSH
{
    public class GameManager : MonoBehaviour
    {
        private class Score
        {
            public int vaccineCount = 0;
            public int facemaskCount = 0;
            public int needleCount = 0;
            public int time = 0;
            public int VaccineScore { get { return 7 * vaccineCount; } }
            public int NeedleScore { get { return 5 * vaccineCount; } }
            public int FacemaskScore { get { return 3 * vaccineCount; } }
            public int GetScore(int endTime)
            {
                return VaccineScore + NeedleScore + FacemaskScore - Mathf.Abs(endTime - time);
            }

            public void Reset()
            {
                vaccineCount = 0;
                facemaskCount = 0;
                needleCount = 0;
                time = 0;
            }
        }

        public static GameManager instance = null;
        private int mCurrentLevel = 1;
        private int currentVolume = 50;
        private int mCurrentTiming = 0;
        private int mCurrentNeedle = 0;
        private int mCurrentFacemask = 0;
        private Score mScore = new Score();

        private EventStore EventManager = EventStore.instance;
        public enum GameState : int { Start, Information, Setting, Gaming, Wasted, LevelEnd, Ended }

        private class ItemType
        {
            public const string Vaccine = "Props_Vaccine";
            public const string Facemask = "Props_Facemask";
            public const string Needle = "Props_Needle";

            public bool Contain(string key)
            {
                return (Vaccine == key) || (Facemask == key) || (Needle == key);
            }
        }
        private ItemType mItemType = new ItemType();

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
            EventManager.Register("onPickupItem", this, (_, p) => HandlePickUp(p));
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
                EventManager.Notify("onLoadScene", this, 2);
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
            
            if (Input.GetKeyDown(KeyCode.F6))
            {
                SetNeedleCountByDiff(1);
                SetFacemaskCountByDiff(1);
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                SetNeedleCountByDiff(-1);
                SetFacemaskCountByDiff(-1);
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                PickUpVaccine();
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
        private void StartCountdown()
        {
            EventManager.Notify("onPopupCountdown", this, null);
            EventManager.Notify("onPlayAudioSource", this, AudioManager.AudioType.CountDown);
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

        private void PickUpVaccine()
        {
            mScore.vaccineCount++;
            EventManager.Notify("onSetLifeValueByDiff", this, 20);
        }

        private void SetNeedleCountByDiff(int diff)
        {
            mCurrentNeedle = Mathf.Clamp(mCurrentNeedle + diff, 0, 5);
            mScore.needleCount = mCurrentFacemask;
            string value = String.Concat(Enumerable.Repeat("y", mCurrentNeedle));
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "needleCount", value },
            };
            EventManager.Notify("onVariableChange", this, dict);
        }
        private void SetFacemaskCountByDiff(int diff)
        {
            mCurrentFacemask = Mathf.Clamp(mCurrentFacemask + diff, 0, 5);
            mScore.facemaskCount = mCurrentFacemask;
            string value = String.Concat(Enumerable.Repeat("m", mCurrentFacemask));
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "facemaskCount", value },
            };
            EventManager.Notify("onVariableChange", this, dict);
        }

        private void HandlePickUp(object tgN)
        {
            string tagName = (string)tgN;
            if (mItemType.Contain(tagName))
            {
                switch (tagName)
                {
                    case ItemType.Vaccine: PickUpVaccine(); break;
                    case ItemType.Needle: SetNeedleCountByDiff(1);  break;
                    case ItemType.Facemask: SetFacemaskCountByDiff(1); break;
                    default: break;
                }
            }
        }
    }
}
