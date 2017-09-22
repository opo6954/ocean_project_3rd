using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vrat
{
    /*
     * List 형태로 idx 관리하는 녀석
     * */
    public class SetDropDownType : SetTypeTemplate
    {
        [SerializeField]
        UnityEngine.UI.Dropdown dropdown;

        public delegate void OnValueChangedCallback(int value);
        public OnValueChangedCallback callback;
         
        


        void Start()
        {
            vp = VISUALIZEPROPTYPE.DROPDOWN;
        }

        public override string getValue()
        {
            return dropdown.value.ToString();
        }

        public void setValue(List<string> valueArr, int selectedIdx)
        {
            dropdown.AddOptions(valueArr);
            dropdown.value = selectedIdx;
            
            
            TextGenerator txt = new TextGenerator();

            TextGenerationSettings se = dropdown.itemText.GetGenerationSettings(dropdown.itemText.rectTransform.rect.size);

            float width = txt.GetPreferredWidth(valueArr[0], se);

            width = width * 2;


            Vector2 oriSize = dropdown.GetComponent<RectTransform>().sizeDelta;

            oriSize.x = width;

            dropdown.GetComponent<RectTransform>().sizeDelta = oriSize;

        }

        public void setCallback(OnValueChangedCallback _callback)
        {
            callback = _callback;

            dropdown.onValueChanged.AddListener(delegate { callback(dropdown.value); });
        }
        

    }
}