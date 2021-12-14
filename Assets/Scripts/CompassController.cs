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

        void FixedUpdate()
        {
            TMP_Text displayer = mainDirection.GetComponent<TMP_Text>();
            displayer.text = DegreeToDisplayString(mMainDegree);

            int indicatorLength = mIndicatorTextList.Length;
            int halfIndicator = Mathf.FloorToInt(indicatorLength / 2);
            for (int i = 0; i < indicatorLength; i++)
            {
                mIndicatorTextList[i].text = DegreeToDisplayString(mMainDegree + (i - halfIndicator) * mUnit);
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

        private void ChangePlayerDirection(object direction)
        {
            Vector2 dir = (Vector2)direction;
            float deg = (Mathf.Atan2(dir.y, dir.x)) * (180 / Mathf.PI);
            mMainDegree = GetApproximatedDegree(deg);
        }
    }
}
