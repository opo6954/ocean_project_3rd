using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * 수정할 수 없는 label type임
     * */
    public class SetLabelType : SetTypeTemplate
    {
        [SerializeField]
        UnityEngine.UI.Text label;

        // Use this for initialization
        void Start()
        {
            vp = VISUALIZEPROPTYPE.LABEL;
        }

        public override string getValue()
        {
            return "";
        }

        public void setValue(string t)
        {
            label.text = t;
        }
    }
}