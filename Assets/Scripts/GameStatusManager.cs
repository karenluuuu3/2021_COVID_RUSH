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
        private int mCurrentNeedle = 0;
        private int mCurrentFacemask = 0;

        void Start()
        {
            mEventStore.Register("onPickupItem", this, (_, p) => HandlePickUp(p));
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

        private void SetLifeValue()
        {
            mEventStore.Notify("onSetLifeValueByDiff", this, -8);
        }

        private void PickUpVaccine()
        {
            mScore.vaccineCount++;
            mEventStore.Notify("onSetLifeValueByDiff", this, 20);
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
            mEventStore.Notify("onVariableChange", this, dict);
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
            mEventStore.Notify("onVariableChange", this, dict);
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