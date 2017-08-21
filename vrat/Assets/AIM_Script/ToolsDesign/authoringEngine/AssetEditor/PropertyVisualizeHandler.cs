using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    public enum VISUALIZEPROPTYPE
    {
        DROPDOWN=0, TEXTINPUT, TOGGLE, LOCATIONFIELD, SLIDER
    }


    public class PropertyVisualizeHandler : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text propertyName;

        [SerializeField]
        GameObject propertyValueTemplate;

        VISUALIZEPROPTYPE vp;
        GameObject currForm;

        private readonly float yOffset = -50;
        private readonly float xOffset = 10;

        public void positioningUI(int idx)
        {
            Vector3 position = new Vector3();
            position.x = xOffset;
            position.y = position.y + yOffset * idx;
            GetComponent<RectTransform>().localPosition = position;
        }

        public void visualizeProperty(PrimitiveXmlTemplate pxt, int idx)
        {
            string propName = pxt.Name;
            string propType = pxt.Type;
            string contents = pxt.getVariable();

            int templateIdx;

            propertyName.text = propName;

            //boolean만 checkbox 쓰기
            if (propType == "bool")
            {
                vp = VISUALIZEPROPTYPE.TOGGLE;
                templateIdx = (int)vp;
                currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

                SetBoolType sbt = currForm.GetComponent<SetBoolType>();

                sbt.setValue(bool.Parse(pxt.getVariable()));
                sbt.setCallback(pxt.setparameter);
            }
                //다른 변수는 걍 inputField 쓰기
            else
            {
                vp = VISUALIZEPROPTYPE.TEXTINPUT;
                templateIdx = (int)vp;
                currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

                SetInputType sit = currForm.GetComponent<SetInputType>();

                sit.setValue(pxt.getVariable());
                sit.setCallback(pxt.setparameter);
            }

            positioningUI(idx);

            turnOffOther(templateIdx);
        }

        public void visualizeProperty(LocationXmlTemplate lxt, int idx)
        {
            string propName = lxt.Name;
            string propType = lxt.Type;

            int templateIdx;

            propertyName.text = propName;

            vp = VISUALIZEPROPTYPE.LOCATIONFIELD;

            templateIdx = (int)vp;

            currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

            SetLocationType slt = currForm.GetComponent<SetLocationType>();

            slt.setValue(lxt.getVariable());


            positioningUI(idx);

            turnOffOther(templateIdx);
        }

        public void visualizeProperty(ListOfXmlTemplate pxt, int idx)
        {
            string propName = pxt.Name;
            string propType = pxt.Type;

            int templateIdx;

            propertyName.text = propName;

            vp = VISUALIZEPROPTYPE.DROPDOWN;

            templateIdx = (int)vp;

            currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

            SetDropDownType sddt = currForm.GetComponent<SetDropDownType>();

            sddt.setValue(pxt.getListofAllName(), pxt.selectedIdx);

            sddt.setCallback(pxt.setIdx);

            positioningUI(idx);

            turnOffOther(templateIdx);
        }


        void turnOffOther(int idx)
        {
            for (int i = 0; i < propertyValueTemplate.transform.childCount; i++)
            {
                if (i == idx)
                    continue;
                propertyValueTemplate.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}