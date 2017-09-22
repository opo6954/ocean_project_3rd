using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace vrat
{
    /*
     * Location type의 property를 업데이트하는 함수
     * */
    public class SetLocationType : SetTypeTemplate
    {

        Vector3 posVec = new Vector3();
        Vector3 rotVec = new Vector3();


        [SerializeField]
        UnityEngine.UI.Text posValue;
        [SerializeField]
        UnityEngine.UI.Text rotValue;

        void Start()
        {
            vp = VISUALIZEPROPTYPE.LOCATIONFIELD;
        }

        public override string getValue()
        {
            return "";
        }

        public void setValue(Location t)
        {
            
            posVec = t.position;
            rotVec = t.rotation;

            posValue.text = posVec.ToString();
            rotValue.text = rotVec.ToString();
        }
    }
}