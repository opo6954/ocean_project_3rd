using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vrat
{
    /*
     * Asset Trigger editor를 관리함
     * */
    public class AssetTriggerEditor : MonoBehaviour
    {
        //trigger list
        [SerializeField]
        Dropdown triggerList;

        //가능한 action list
        [SerializeField]
        Dropdown possibleActionList;

        //추가된 action list
        [SerializeField]
        Dropdown addedActionList;

         
        //customized param에서의 custom input field instance임
        [SerializeField]
        GameObject customValue;

        //customized param에서의 custom list instance임
        [SerializeField]
        GameObject customList;

        [SerializeField]
        Dropdown commonTriggerType;

        [SerializeField]
        AssetEditor assetEditor;

        void Start()
        {
            gameObject.SetActive(false);
        }


        void clearCustomizedContentsWithProperties()
        {
            customValue.transform.GetChild(1).GetComponent<InputField>().text = "";
            //customList.transform.GetChild(1).GetComponent<Dropdown>().ClearOptions();
            customList.transform.GetChild(1).GetComponent<Dropdown>().value = 0;
            triggerList.RefreshShownValue();
            customList.transform.GetChild(1).GetComponent<Dropdown>().RefreshShownValue();
        }

        //모든 생성된 contents 지우기
        void clearCustomizedContentsForInit()
        {
            possibleActionList.value = 0;
            addedActionList.value = 0;
            triggerList.value = 0;
            customList.transform.GetChild(1).GetComponent<Dropdown>().ClearOptions();
            clearCustomizedContentsWithProperties();
            

        }

        void clearCommonContents()
        {
            commonTriggerType.ClearOptions();
            //added action 초기화
            addedActionList.ClearOptions();

            commonTriggerType.RefreshShownValue();
            addedActionList.RefreshShownValue();
        }

        //action 추가하는 함수
        public void OnAddAction()
        {
            string name = possibleActionList.options[possibleActionList.value].text;
            Debug.Log("Selected action: " + name);

            foreach (Dropdown.OptionData od in addedActionList.options)
            {
                if (od.text == name)
                {
                    Debug.Log("Already exist action for " + od.text);
                    return;
                }
            }

            //집어넣기
            addedActionList.options.Add(new Dropdown.OptionData(name));

        }
        //trigger 설정하는 함수
        /*
         * trigger list의 순서
         * CollisionTrigger
         * GazeTrigger
         * InputDownTrigger
         * MonitorDistanceCapturedTrigger
         * */


        //아직 설정 안되있을시
        public void OnGetAssetTriggerInfo()
        {
            Debug.Log("With init...");
            clearCommonContents();
            clearCustomizedContentsForInit();
        }
        //이미 저장한 경력이 있을 경우
        public void OnGetAssetTriggerInfo(string triggerName, string[] actionList, string[] paramList)
        {
            clearCommonContents();


            clearCustomizedContentsForInit();

            Debug.Log("With save...");

            Debug.Log("TriggerNmae: " + triggerName);
            Debug.Log("Actoin length: " + actionList.Length);
            for (int i = 0; i < actionList.Length; i++)
            {
                Debug.Log("Action + " + i.ToString() + " : " + actionList[i]);
            }

            Debug.Log("Param length: " + paramList.Length);

            for (int i = 0; i < paramList.Length; i++)
            {
                Debug.Log("Param + " + i.ToString() + " : " + paramList[i]);
            }



            for (int i = 0; i < triggerList.options.Count; i++)
            {
                Debug.Log("In " + i.ToString() + "'s iter, " + triggerList.options[i].text);
                if (triggerName == triggerList.options[i].text)
                {
                    //index 넣어주기
                    Debug.Log("Trigger set idx: " + triggerList.value);
                    triggerList.value = i;
                    
                }
            }
            //trigger update하기
            OnSetTriggerInner(triggerList.value);

            //action list update하기
            for(int i=0; i<actionList.Length; i++)
            {
                addedActionList.options.Add(new Dropdown.OptionData(actionList[i]));
            }
            //첫 번째꺼는 triggerType 이름임
            string triggerTypeIdx = paramList[0];

            commonTriggerType.value = int.Parse(triggerTypeIdx);

            if (paramList.Length == 2)
            {
                string secondValue = paramList[1];
                customValue.transform.GetChild(1).GetComponent<InputField>().text = secondValue;
            }

            else if (paramList.Length == 3)
            {
                string secondValue = paramList[1];
                customValue.transform.GetChild(1).GetComponent<InputField>().text = secondValue;

                string thirdTypeIdx = paramList[2];
                customList.transform.GetChild(1).GetComponent<Dropdown>().value = int.Parse(thirdTypeIdx);
            }
                /*
                 * paramList[1] = customValue.transform.GetChild(1).GetComponent<InputField>().text;
                    paramList[2] = customList.transform.GetChild(1).GetComponent<Dropdown>().value.ToString();
                 * */

        }



        public void OnSetTrigger()
        {
            Debug.Log("In OnSetTrigger ...");
            Debug.Log(triggerList.value);
            OnSetTriggerInner(triggerList.value);
        }

        public void OnSetTriggerInner(int selectedTriggerIdx)
        {
            //WARNING, HARDCODING...
            clearCommonContents();
            clearCustomizedContentsWithProperties();
            

            switch (selectedTriggerIdx)
            {
                    //CollisionTrigger
                case 0:
                    commonTriggerType.options.Add(new Dropdown.OptionData("CollisionEnter"));
                    commonTriggerType.options.Add(new Dropdown.OptionData("CollisionHold"));
                    commonTriggerType.options.Add(new Dropdown.OptionData("CollisionExit"));
                    customValue.SetActive(false);
                    customList.SetActive(false);

                    break;
                case 1:
                    commonTriggerType.options.Add(new Dropdown.OptionData("GazeEnter"));
                    commonTriggerType.options.Add(new Dropdown.OptionData("GazeHold"));
                    commonTriggerType.options.Add(new Dropdown.OptionData("GazeExit"));
                    customValue.SetActive(false);
                    customList.SetActive(false);
                    break;
                case 2:
                    commonTriggerType.options.Add(new Dropdown.OptionData("InputDown"));

                    customValue.SetActive(true);
                    customList.SetActive(false);

                    customValue.transform.GetChild(0).GetComponent<Text>().text = "KeyName";
                    break;
                case 3:
                    commonTriggerType.options.Add(new Dropdown.OptionData("MonitorDistanceCaptured"));

                    customValue.SetActive(true);
                    customList.SetActive(true);

                    customValue.transform.GetChild(0).GetComponent<Text>().text = "Threshold";

                    customList.transform.GetChild(1).GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("Larger"));
                    customList.transform.GetChild(1).GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("LargerOrEqual"));
                    customList.transform.GetChild(1).GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("Equal"));
                    customList.transform.GetChild(1).GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("SmallerOrEqual"));
                    customList.transform.GetChild(1).GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("Smaller"));


                    break;
            }
        }
        //asset trigger 저장하는 함수
        //넘겨줄 것 list of action of name string
        //trigger type
        //기타 list of object로 주기
        public void OnSaveAssetTrigger()
        {
            string triggerType;
            string[] actionList;
            string[] paramList;
            //trigger type 저장
            triggerType = triggerList.options[triggerList.value].text;
            actionList = new string[addedActionList.options.Count];

            //action type 저장
            for (int i = 0; i < addedActionList.options.Count; i++)
            {
                actionList[i] = addedActionList.options[i].text;
            }

            switch (triggerList.value)
            {
                case 0:
                    paramList = new string[1];
                    paramList[0] = commonTriggerType.value.ToString();
                    assetEditor.OnAddAssetTrigger(triggerType, actionList, paramList);
                    break;
                case 1:
                    paramList = new string[1];
                    paramList[0] = commonTriggerType.value.ToString();
                    assetEditor.OnAddAssetTrigger(triggerType, actionList, paramList);
                    break;
                case 2:
                    paramList = new string[2];
                    paramList[0] = commonTriggerType.value.ToString();
                    paramList[1] = customValue.transform.GetChild(1).GetComponent<InputField>().text;
                    assetEditor.OnAddAssetTrigger(triggerType, actionList, paramList);
                    break;
                case 3:
                    paramList = new string[3];
                    paramList[0] = commonTriggerType.value.ToString();
                    paramList[1] = customValue.transform.GetChild(1).GetComponent<InputField>().text;
                    paramList[2] = customList.transform.GetChild(1).GetComponent<Dropdown>().value.ToString();
                    assetEditor.OnAddAssetTrigger(triggerType, actionList, paramList);
                    break;
            }

        }

        //선택된 action 지우는 함수
        public void OnDeleteAction()
        {
            
            //addedActionList로부터 지우기
            if (addedActionList.value < addedActionList.options.Count)
                addedActionList.options.RemoveAt(addedActionList.value);
            else
                Debug.Log("Nothing to delete...");
        }
        //취소 버튼 누르는 함수
        public void OnCancel()
        {
            this.gameObject.SetActive(false);
        }
    }
}