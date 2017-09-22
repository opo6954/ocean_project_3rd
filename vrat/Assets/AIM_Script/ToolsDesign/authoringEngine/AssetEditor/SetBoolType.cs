using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    public class SetBoolType : SetTypeTemplate
    {
        [SerializeField]
        UnityEngine.UI.Toggle toggle;

        public delegate void OnValueChangedCallback(string value);
        public OnValueChangedCallback callback;

        void Start()
        {
            vp = VISUALIZEPROPTYPE.TOGGLE;
        }

        public void setValue(bool t)
        {
            toggle.isOn = t;
        }

        public override string getValue()
        {
            return toggle.isOn.ToString();
        }

        public void setCallback(OnValueChangedCallback _callback)
        {
            callback = _callback;

            toggle.onValueChanged.AddListener(delegate { callback(toggle.isOn.ToString());}); 
        } 
    }
}