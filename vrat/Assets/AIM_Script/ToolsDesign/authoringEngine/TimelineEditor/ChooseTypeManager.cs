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

        [SerializeField]
        UnityEngine.UI.Text chooseNameText;

        [SerializeField]
        UnityEngine.UI.RawImage background;

        //innerType을 위한 ui prefab임
        Object innerTypeUIPrefab;

        int primIdx;
        int secondIdx;

        string[] triggerTypesName;
        string[] actionTypesName;
        string[] triggerTypesInst;
        string[] actionTypesInst;

        void Start()
        {
            innerTypeUIPrefab = Resources.Load("TimelineEditor/innerSetTemplate");

            choosePrimType.SetActive(false);
            chooseInnerType.SetActive(false);
            background.enabled = false;
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

        public void OnEnterFirstPhase()
        {
            background.enabled = true;
            //모든 choose window 끄기
            choosePrimType.SetActive(true);
        }
        
        //처음에 무슨 type 만들지 선택함 idx0: trigger, idx1: action idx2: instruction
        public void OnClickFirstPhase(int idx)
        {
            
            primIdx = idx;

            background.enabled = true;

            switch(primIdx)
            {
                case 0:
                    choosePrimType.SetActive(false);

                    //목록에 맞게 update하기                    
                    respawnList(triggerTypesName);
                    chooseNameText.text = "Choose trigger types";

                    chooseInnerType.SetActive(true);
                    break;
                case 1:
                    choosePrimType.SetActive(false);
                    
                    //목록에 맞게 update하기
                    respawnList(actionTypesName);
                    chooseNameText.text = "Choose action types";

                    chooseInnerType.SetActive(true);
                    break;
                case 2:
                    choosePrimType.SetActive(false);
                    chooseInnerType.SetActive(false);
                    timelineEditor.OnChooseTypes(primIdx, -1);
                    break;
            }
        }

        public void respawnList(string[] nameList)
        {
            //respawn...


            for(int i=0; i<nameList.Length; i++)
            {
                GameObject go = (GameObject)Instantiate(innerTypeUIPrefab,innerTypeArea.transform);

                //위치하고
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2();

                innerTypeContainer itc = go.GetComponent<innerTypeContainer>();
                itc.setIdx(i);
                itc.setPositioning(i);
                itc.setText(nameList[i]);
                itc.setOnClickListener(OnClickSecondPhase);
            }

        }

        //두번째에서 inner type으로 무엇을 만들지 선택함 단 instruction은 예외임
        public void OnClickSecondPhase(int idx)
        {
            background.enabled = false;
            //모든 choose window 끄기
            choosePrimType.SetActive(false);
            chooseInnerType.SetActive(false);
            //second idx 설정하고
            secondIdx = idx;

            //timelineeditor로 callback 보내기(OnChooseTypes with primIdx, secondIdx)
            timelineEditor.OnChooseTypes(primIdx, secondIdx);
        }

        

        
    }
}