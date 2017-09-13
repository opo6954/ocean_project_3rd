using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * 오른쪽 위의 editor간 전환하는 버튼을 관리하는 녀석
     * 예를들어 in-situ에서는 1인칭 시점이기 때문에 사라져야 함 등의 기능을 해야함
     * */
    public class ModeSelector : UISelectorTemplate
    {
        //editor의 visible을 조정하는 editorSelector의 instance를 넣어주면 됨
        [SerializeField]
        EditorWindowSelector editorWindowSelector;


        //현재 무슨 window가 띄워져 있는 지 알려줘야 asset list window에서 더블클릭 등의 process를 수행
        [SerializeField]
        AssetListWindowHandler assetListWindowHandler;


        //특정 editorMode에 들어갈 경우의 callback임/ editor 들어가는 버튼에 callback함수로써 이 함수가 설정됨 with 파라미터

        public override void initialize()
        {
            base.initialize();

            for (int i = 0; i < childList.Count; i++)
            {
                childList[i].SetActive(true);
            }

        }

        public override void OnEnterCertainEditor(string _type)
        {

            //만족하는 editorWindow 켜기
            editorWindowSelector.OnEnterCertainEditor(_type);
            assetListWindowHandler.setCurrEditorWindow(_type);

            //editorSelector의 특정 editor 켜기
            for (int i = 0; i < childList.Count; i++)
            {
                if (childList[i].name.Contains(_type) == true)
                {
                    //버튼 비활성화
                    childList[i].GetComponent<UnityEngine.UI.Button>().interactable = false;
                }
                else
                {
                    //버튼 활성화
                    childList[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
                }
            }
        }
    }
}