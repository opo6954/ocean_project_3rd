using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace vrat
{
    /*
     * ChooseType 관련 관리
     * 2단계로 구성
     * 1단계: trigger, action, instruction 설정
     * 2단계: 각각의 inner type 설정(instruction은 없음)
     * */
    public class ChooseTypeManager : MonoBehaviour
    {
        //primitive type 결정
        [SerializeField]
        GameObject choosePrimType;

        //내부 결정
        [SerializeField]
        GameObject chooseInnerType;

        //innerType Template이 소환될 구역임
        [SerializeField]
        GameObject innerTypeArea;

        //timeline editor instance 가지고 있음 callback 바로 부를려공
        [SerializeField]
        TimelineEditor timelineEditor;

        //innerType을 위한 ui prefab임
        Object innerTypeUIPrefab;

        string[] triggerTypesName;
        string[] actionTypesName;
        string[] triggerTypesInst;
        string[] actionTypesInst;

        void Start()
        {
            innerTypeUIPrefab = Resources.Load("TimelineEditor/innerSetTemplate");
        }

        //이름하고 설명 정도?
        //timeline editor에서 일단 이거 불러야 한다
        public void getInitInnerTypes(string[] _triggerTypesName, string[] _actionTypesName, string[] _triggerTypesInst, string[] _actionTypesInst)
        {
            triggerTypesName = new string[_triggerTypesName.Length];
            actionTypesName = new string[_actionTypesName.Length];
            triggerTypesInst = new string[_triggerTypesInst.Length];
            actionTypesInst = new string[_actionTypesInst.Length];

            System.Array.Copy(_triggerTypesName,triggerTypesName, _triggerTypesName.Length);
            System.Array.Copy(_triggerTypesInst,triggerTypesInst, _triggerTypesName.Length);
            System.Array.Copy(_actionTypesName,actionTypesName, _actionTypesName.Length);
            System.Array.Copy(_actionTypesInst,actionTypesInst, _actionTypesName.Length);
        }
        
        //처음에 무슨 type 만들지 선택함 idx0: trigger, idx1: action idx2: instruction
        public void OnClickFirstPhase(int idx)
        {
            switch(idx)
            {
                case 0:
                    choosePrimType.SetActive(false);
                    //목록에 맞게 update하기
                    chooseInnerType.SetActive(true);
                    break;
                case 1:
                    choosePrimType.SetActive(false);
                    //목록에 맞게 update하기
                    chooseInnerType.SetActive(true);
                    break;
                case 2:
                    choosePrimType.SetActive(false);
                    chooseInnerType.SetActive(false);
                    timelineEditor.OnChooseTypes(2, -1);
                    break;
            }
        }

        public void respawnList(string[] nameList)
        {
            //respawn...
            for(int i=0; i<nameList.Length; i++)
            {
                GameObject go = (GameObject)Instantiate(innerTypeUIPrefab,innerTypeArea.transform);

                go.GetComponent<RectTransform>().anchoredPosition = new Vector2();
                go.GetComponent<innerTypeContainer>().setIdx(i);
            

                //go.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
            }

        }

        //두번째에서 inner type으로 무엇을 만들지 선택함 단 instruction은 예외임
        public void OnClickSecondPhase(int idx)
        {
            
        }

        

        
    }
}