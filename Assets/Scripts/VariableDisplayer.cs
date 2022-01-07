using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace COVID_RUSH
{
    public class VariableDisplayer : MonoBehaviour
    {
        [SerializeField]
        private string variableName;
        [SerializeField]
        private TMP_Text displayer;

        public VariableDisplayer(string _variableName, TMP_Text _displayer)
        {
            variableName = _variableName;
            displayer = _displayer;
        }

        void Start()
        {
            EventStore.instance.Register("onVariableChange", this, (_, param) => UpdateVariable(param));
        }

        private void UpdateVariable(object value)
        {
            Dictionary<string, string> dict = (Dictionary<string, string>) value;
            string content;
            if (dict.TryGetValue(variableName, out content))
            {
                displayer.text = content;
            }
        }
    }
}
