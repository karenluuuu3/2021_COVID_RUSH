using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace COVID_RUSH
{
    public class CompassController : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainDirection; // = GameObject.Find("Direction Indicator");
        [SerializeField]
        private GameObject otherDirectionContainer; // = GameObject.Find("Other Direction");
        private TMP_Text[] mIndicatorTextList;
        private int mMainDegree;
        private int mUnit = 10;
        private float mBias = 1000;
        private static Dictionary<int, string> mDegToDirectionDisplay = new Dictionary<int, string>
        {
            [0] = "N",
            [90] = "E",
            [180] = "S",
            [270] = "W"
        };

        void Start()
        {
            EventStore.instance.Register("onChangeForward", this, (c, p) => ChangePlayerDirection(p));
            mIndicatorTextList = otherDirectionContainer.GetComponentsInChildren<TMP_Text>();
            mMainDegree = 0;
        }

        private void UpdateMainIndicator(int deg)
        {
            mMainDegree = deg;
            TMP_Text displayer = mainDirection.GetComponent<TMP_Text>();
            displayer.text = DegreeToDisplayString(mMainDegree);
        }

        private void UpdateOtherIndicator(float deg)
        {
            float bias = GetActualDegree(deg) - mMainDegree;

            // prevent duplicated action
            if (bias == mBias) return;
            mBias = bias;

            // if bias is large enough, update each indicator
            int indicatorLength = mIndicatorTextList.Length;
            int halfIndicator = Mathf.FloorToInt(indicatorLength / 2);
            int indicatorWidth = 50;
            for (int i = 0; i < indicatorLength; i++)
            {
                mIndicatorTextList[i].text = DegreeToDisplayString(mMainDegree + (i - halfIndicator) * mUnit);
                mIndicatorTextList[i].transform.localPosition = new Vector3((i - indicatorLength / 2) * indicatorWidth, 0, 0);
            }

            // shrink the bias to -5 ~ 5
            if (bias > mUnit / 2)
            {
                bias -= (mUnit / 2);
            }
            if (bias < -mUnit / 2)
            {
                bias += (mUnit / 2);
            }

            // then, just translate the indicator
            int biasUnit = 5;
            foreach (TMP_Text indicator in mIndicatorTextList)
            {
                indicator.transform.localPosition += new Vector3(bias * biasUnit, 0, 0);
            }

        }

        private string DegreeToDisplayString(int degree)
        {
            if (degree < 0)
            {
                degree = (360 + degree) % 360;
            }
            if (mDegToDirectionDisplay.ContainsKey(degree))
            {
                return mDegToDirectionDisplay[degree];
            }

            return degree.ToString();
        }

        private int GetApproximatedDegree(float degree)
        {
            return (int) Mathf.Round(degree / mUnit) * mUnit;
        }
        private int GetActualDegree(float degree)
        {
            return (int) Mathf.Round(degree);
        }

        private void ChangePlayerDirection(object direction)
        {
            Vector2 dir = (Vector2)direction;
            float deg = (Mathf.Atan2(dir.y, dir.x)) * (180 / Mathf.PI);
            int approximatedDeg = GetApproximatedDegree(deg);

            if (approximatedDeg != mMainDegree)
            {
                UpdateMainIndicator(approximatedDeg);
            }
            UpdateOtherIndicator(deg);
        }
    }
}
