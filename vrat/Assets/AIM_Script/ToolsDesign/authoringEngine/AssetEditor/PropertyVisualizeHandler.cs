using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    //asset property type별로 각기 다른 종류임
    /*
     * 이거에 맞게 해야 할 듯
     * dropdown: 여러 개중에서 선택(주로 list form에서 수행)
     * textinput: 글자 입력
     * toggle: boolean값
     * locationfield: location 특수(position, rotation)
     * slider: slider 바 형식이 필요할 경우(값같은거?)
     * label: 수정할 수 없는 걍 정보 전달을 위한 값
     * */
    
    public enum VISUALIZEPROPTYPE
    {
        DROPDOWN=0, TEXTINPUT, TOGGLE, LOCATIONFIELD, SLIDER, LABEL
    }
     

    public class PropertyVisualizeHandler : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text propertyName;

        [SerializeField]
        GameObject propertyValueTemplate;

        [SerializeField]
        bool isOnAssetEditor;

        SetTypeTemplate typeTemplate;

        VISUALIZEPROPTYPE vp;
        GameObject currForm;



        private float yOffset = -50;
        private float xOffset = 10;

        void Start()
        {
            //asset editor에 있는 녀석일 경우
            if (isOnAssetEditor == true)
            {
                xOffset = 10;
                yOffset = -50;
            }
                //primitives editor에 있는 녀석일 경우
            else
            {
                xOffset = 10;
                yOffset = -30;
            }
        }

        public string getValueNParamName(ref string paramName)
        {
            paramName = propertyName.text;
            return typeTemplate.getValue();
        }
        
        public void positioningUI(int idx)
        {
            Vector3 position = new Vector3();
            position.x = xOffset;
            position.y = position.y + yOffset * idx;
            

            GetComponent<RectTransform>().localPosition = position;
        }

        //raw한 level에서 수행
        //parameter 이름과 parameter 타입, 추가 정보를 설정할 수 있음
        public void visualizePropertyRaw(string paramName, VISUALIZEPROPTYPE vp, string paramValue, string[] _additionalInfo, int idx)
        {
            string propName = paramName;

            propertyName.text = propName;

            int templateIdx = (int)vp;

            

            currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

            

            if (vp == VISUALIZEPROPTYPE.DROPDOWN)
            {
                if (_additionalInfo == null)
                {
                    Debug.Log("No additional info found...");
                    return;
                }

                if (_additionalInfo.Length <= 0)
                {
                    Debug.Log("No enough addional info found....");
                    return;
                }


                SetDropDownType sddt = currForm.GetComponent<SetDropDownType>();

                typeTemplate = sddt;

                List<string> additionalInfo = new List<string>();
                for (int i = 0; i < _additionalInfo.Length; i++)
                {
                    additionalInfo.Add(_additionalInfo[i]);
                }

                sddt.setValue(additionalInfo, int.Parse(paramValue));

            }
            else if (vp == VISUALIZEPROPTYPE.TOGGLE)
            {
                SetBoolType sbt = currForm.GetComponent<SetBoolType>();
                typeTemplate = sbt;
                sbt.setValue(bool.Parse(paramValue));                
            }
            else if (vp == VISUALIZEPROPTYPE.LABEL)
            {

            }
            else if (vp == VISUALIZEPROPTYPE.LOCATIONFIELD)
            {
                Debug.Log("No implemente...");
            }
            else if(vp == VISUALIZEPROPTYPE.TEXTINPUT)
            {
                SetInputType sit = currForm.GetComponent<SetInputType>();
                typeTemplate = sit;
                sit.setValue(paramValue);
            }


            positioningUI(idx);
            turnOffOther(templateIdx);
        }

        

        //property의 type별로 다른 visualizer가 수행됨
        public void visualizePropertyAll(XmlTemplate xt, int i)
        {
            if (xt.ClassName == "PrimitiveXmlTemplate")
            {
                var q = xt as PrimitiveXmlTemplate;
                visualizeProperty(q, i);
            }
            else if (xt.ClassName == "LocationXmlTemplate")
            {
                var q = xt as LocationXmlTemplate;
                visualizeProperty(q, i);
            }
            else if(xt.ClassName == "ListOfXmlTemplate")
            {
                var q = xt as ListOfXmlTemplate;
                visualizeProperty(q, i);
            }
            else if(xt.ClassName == "VariableXmlTemplate")
            {
                var q = xt as VariableXmlTemplate;
                visualizeProperty(q, i);
            }
        }


        void visualizeProperty(PrimitiveXmlTemplate pxt, int idx)
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
                typeTemplate = sbt;

                sbt.setValue(bool.Parse(pxt.getVariable()));
                sbt.setCallback(pxt.setparameter);
            }
                //다른 변수는 걍 inputField 쓰기 or 나중에 숫자일 경우 slider 형식도 고려하자
            else
            {
                vp = VISUALIZEPROPTYPE.TEXTINPUT;
                templateIdx = (int)vp;
                currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

                SetInputType sit = currForm.GetComponent<SetInputType>();
                typeTemplate = sit;

                sit.setValue(pxt.getVariable());
                sit.setCallback(pxt.setparameter);
            }

            positioningUI(idx);
            turnOffOther(templateIdx);
        }

        void visualizeProperty(LocationXmlTemplate lxt, int idx)
        {
            string propName = lxt.Name;
            string propType = lxt.Type;

            int templateIdx;

            propertyName.text = propName;

            vp = VISUALIZEPROPTYPE.LOCATIONFIELD;

            templateIdx = (int)vp;

            currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

            SetLocationType slt = currForm.GetComponent<SetLocationType>();
            typeTemplate = slt;

            slt.setValue(lxt.getVariable());


            positioningUI(idx);

            turnOffOther(templateIdx);
        }

        void visualizeProperty(ListOfXmlTemplate pxt, int idx)
        {
            string propName = pxt.Name;
            string propType = pxt.Type;

            int templateIdx;

            propertyName.text = propName;

            vp = VISUALIZEPROPTYPE.DROPDOWN;

            templateIdx = (int)vp;

            currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

            SetDropDownType sddt = currForm.GetComponent<SetDropDownType>();

            typeTemplate = sddt;

            sddt.setValue(pxt.getListofAllName(), pxt.selectedIdx);

            sddt.setCallback(pxt.setIdx);

            positioningUI(idx);

            turnOffOther(templateIdx);
        }

        //variable parameter를 위한 visualier
        //variable의 경우 
        void visualizeProperty(VariableXmlTemplate vxt, int idx)
        {
            string propName = vxt.Name;
            string propType = vxt.Type;

            int templateIdx;

            propertyName.text = "Explicit Variable";
            
            vp = VISUALIZEPROPTYPE.LABEL;

            templateIdx = (int)vp;

            currForm = propertyValueTemplate.transform.GetChild(templateIdx).gameObject;

            SetLabelType slt = currForm.GetComponent<SetLabelType>();

            typeTemplate = slt;

            slt.setValue(propName);

            positioningUI(idx);
            turnOffOther(templateIdx);
        }


        void turnOffOther(int idx)
        {
            for (int i = 0; i < propertyValueTemplate.transform.childCount; i++)
            {
                if (i == idx)
                {
                    propertyValueTemplate.transform.GetChild(i).gameObject.SetActive(true);
                    continue;
                }
                propertyValueTemplate.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}