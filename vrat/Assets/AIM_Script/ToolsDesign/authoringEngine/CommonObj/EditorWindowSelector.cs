using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * Editor window를 관리하는 녀석
     * Editor window의 visible 여부 등을 설정한다
     * */
    public class EditorWindowSelector : UISelectorTemplate
    {
        public override void initialize()
        {
            base.initialize();

            //editorWindow의 경우 초기에는 setactive를 false로 해놓는다
            for (int i = 0; i < childList.Count; i++)
            {
                childList[i].SetActive(false);
            }
        }

        //enter 들어올 경우 들어온 type만 켜고 나머지는 끄면 됨
        public override void OnEnterCertainEditor(string _type)
        {
            for (int i = 0; i < childList.Count; i++)
            {
                if (childList[i].name.Contains(_type) == true)
                {
                    childList[i].SetActive(true);
                }
                else
                    childList[i].SetActive(false);
            }
        }
    }
}