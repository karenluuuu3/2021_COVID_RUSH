using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace COVID_RUSH
{
    public class Score
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

    public class GameStatusManager : MonoBehaviour
    {
        private EventStore mEventStore = EventStore.instance;
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
        private Score mScore = new Score();
        private enum LifeObject : int { Mask, Needle, Main };
        private int mCurrentNeedle = 0;
        private int mCurrentFacemask = 0;
        private HashSet<string> mEnemySet = new HashSet<string>();

        void Start()
        {
            mEventStore.Register("onPickupItem", this, (_, p) => HandlePickUp(p));
            mEventStore.Register("onEnterInfectedArea", this, (_, p) => HandleEnterInfectedArea());
            mEventStore.Register("onBarZeroing", this, (_, p) => HandleBarZeroing(p));
            mEventStore.Register("onEnemyLeave", this, (_, p) => HandleEnemyLeave(p));
            mEventStore.Register("onEnemyEnter", this, (_, p) => HandleEnemyEnter(p));
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // TODO: Remove this short-cut
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                HandleEnterInfectedArea();
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

        private LifeObject GetCurrentLifeObject()
        {
            if (mCurrentFacemask > 0) return LifeObject.Mask;
            if (mCurrentNeedle > 0) return LifeObject.Needle;
            return LifeObject.Main;
        }

        private void HandleBarZeroing(object bn)
        {
            string barName = (string)bn;
            if (barName == LifeObject.Main.ToString())
            {
                return;
            }

            if (barName == LifeObject.Mask.ToString())
            {
                SetFacemaskCountByDiff(-1);
                return;
            }

            if (barName == LifeObject.Needle.ToString())
            {
                SetNeedleCountByDiff(-1);
                return;
            }
        }

        private void HandleEnterInfectedArea()
        {
            ValueBar.UpdateFormat value = new ValueBar.UpdateFormat
            {
                name = GetCurrentLifeObject().ToString(),
                value = -0.1f,
            };
            mEventStore.Notify("onSetBarValueByDiff", this, value);
        }

        private void HandleEnemyEnter(object enemyName)
        {
            if (mEnemySet.Contains(enemyName)) return;

            mEnemySet.Add((string)enemyName);
            LifeObject current = GetCurrentLifeObject();
            switch (current)
            {
                case LifeObject.Main: SetLifeValue(-10.0f); ; break;
                case LifeObject.Needle: SetNeedleCountByDiff(-1); break;
                case LifeObject.Mask: SetFacemaskCountByDiff(-1); break;
                default: break;
            }
        }

        private void HandleEnemyLeave(object enemyName)
        {
            mEnemySet.Remove((string)enemyName);
        }

        private void SetLifeValue(float v)
        {
            ValueBar.UpdateFormat value = new ValueBar.UpdateFormat
            {
                name = LifeObject.Main.ToString(),
                value = v,
            };
            mEventStore.Notify("onSetBarValueByDiff", this, value);
        }

        private void PickUpVaccine()
        {
            mScore.vaccineCount++;
            SetLifeValue(20.0f);
        }

        private void SetNeedleCountByDiff(int diff)
        {
            bool shouldUpdateBar = mCurrentNeedle + diff > 0;
            mCurrentNeedle = Mathf.Clamp(mCurrentNeedle + diff, 0, 5);
            mScore.needleCount += Mathf.Max(0, diff);
            string value = String.Concat(Enumerable.Repeat("y", mCurrentNeedle));
            Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "needleCount", value },
                };
            mEventStore.Notify("onVariableChange", this, dict);

            if (!shouldUpdateBar) return;
            ValueBar.UpdateFormat barValue = new ValueBar.UpdateFormat
            {
                name = LifeObject.Needle.ToString(),
                value = 100.0f,
            };
            mEventStore.Notify("onSetFixedBarValue", this, barValue);
        }

        private void SetFacemaskCountByDiff(int diff)
        {
            bool shouldUpdateBar = mCurrentFacemask + diff > 0;
            mCurrentFacemask = Mathf.Clamp(mCurrentFacemask + diff, 0, 5);
            mScore.facemaskCount += Mathf.Max(0, diff);
            string value = String.Concat(Enumerable.Repeat("m", mCurrentFacemask));
            Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "facemaskCount", value },
                };
            mEventStore.Notify("onVariableChange", this, dict);

            if (!shouldUpdateBar) return;
            ValueBar.UpdateFormat barValue = new ValueBar.UpdateFormat
            {
                name = LifeObject.Mask.ToString(),
                value = 100.0f,
            };
            mEventStore.Notify("onSetFixedBarValue", this, barValue);
        }

        private void HandlePickUp(object tgN)
        {
            string tagName = (string)tgN;
            if (mItemType.Contain(tagName))
            {
                switch (tagName)
                {
                    case ItemType.Vaccine: PickUpVaccine(); break;
                    case ItemType.Needle: SetNeedleCountByDiff(1); break;
                    case ItemType.Facemask: SetFacemaskCountByDiff(1); break;
                    default: break;
                }
            }
        }
    }
}