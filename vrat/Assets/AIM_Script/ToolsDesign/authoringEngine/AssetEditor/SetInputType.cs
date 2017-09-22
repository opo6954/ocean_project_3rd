using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    /*
     * Input Field를 관리하는 녀석
     * */
    public class SetInputType : SetTypeTemplate
    {
        [SerializeField]
        UnityEngine.UI.InputField input;

        public delegate void OnValueChangedCallback(string value);
        public OnValueChangedCallback callback;

        void Start()
        {
            vp = VISUALIZEPROPTYPE.TEXTINPUT;
        }

        public override string getValue()
        {
            return input.text;
        }

        public void setValue(string t)
        {
            input.text = t;
        }

        public void setCallback(OnValueChangedCallback _callback)
        {
            callback = _callback;

            input.onValueChanged.AddListener(delegate { callback(input.text); });
        }

        /*
        public override void setValue<T>(string txt)
        {
            input.text = txt;
            SetTypeTemplate stt = new SetInputType();
        }

        public override string getValue<T>()
        {
            return input.text;
        }
         * */
    }
}