using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

/*
 * 
 * timeline editor임
 * timeline을 수정할 수 있음
 * 
 * 일단 가능한 trigger list, action list, instrution list를 읽는 작업이 필요함
 * 
 * 이후에 onclickevent 만들고
 * 
 * properties에 파라미터 넣는 것도 필요하고 + Name
 * 
 * 그리고 properties와 name을 실시간 동기화 하자
 * 
 * */
namespace vrat
{

    public class TimelineEditor :  WindowTemplate
    {
        //asset list의 instance임
        [SerializeField]
        AssetListWindowHandler assetListWindowHandler;
        

        //subwindow의 instance임
        [SerializeField]
        GameObject chooseTypeWindow;

        [SerializeField]
        GameObject uiTemplatePosition;

        [SerializeField]
        ChooseTypeManager ctm;

        [SerializeField]
        UnityEngine.UI.Text primType;

        [SerializeField]
        UnityEngine.UI.Text innerType;

        [SerializeField]
        UnityEngine.UI.Text primName;

        //ui property position이 생성되는 곳
        [SerializeField]
        GameObject uiPropertyPosition;
        

        AuthorableAsset currAsset;
        Texture2D currPreviewImg;

        PAUTPrimitivesTemplate currPrimTemplate;

        //prim에서 property에서의 ui prefab임
        Object propertyUIPrimPrefab;


        //file로부터 load한 trigger list
        List<TriggerPrimitivesTemplate> triggerList = new List<TriggerPrimitivesTemplate>();

        //file로부터 load한 action list
        List<ActionPrimitivesTemplate> actionList = new List<ActionPrimitivesTemplate>();

        List<PropertyVisualizeHandler> propertyVisList = new List<PropertyVisualizeHandler>();

        //저장된 trigger UI prefab 경로
        string triggerUIPrefabPath = "TimelineEditor/triggerUITemplate";

        //저장된 action UI prefab 경로
        string actionUIPrefabPath = "TimelineEditor/actionUITemplate";
        
        //저장된 instruction UI prefab 경로
        string instructionUIPrefabPath = "TimelineEditor/instructionUITemplate";

        //저장된 property UI prefab 경로
        string propertyUIPrimPrefabPath = "TimelineEditor/propertyUIPrimTemplate";

        //순서대로 trigger, action, instruction임
        Object[] uiPrefab;

        //현재 respawn된 uiManagerList임
        List<primitivesUIManager> uiManagerList = new List<primitivesUIManager>();

        //저장된 trigger 경로
        string triggerPrefabPath = "/Resources/EventFiles/trigger/";

        //저장된 action 경로
        string actionPrefabPath = "/Resources/EventFiles/action/";

        //drag start가 assetlist로부터 왔는지 체크하기
        bool isDragFromAssetList = false;

        Vector2 currPointDropPos = new Vector2();


        //현재 가지고 있는 trigger이름
        string[] triggerNameList;

        //현재 가지고 있는 action이름
        string[] actionNameList;


        public void initialize()
        {
            //trigger, action, instructino ui prefab 모두 불러오기
            uiPrefab = new Object[3];


            uiPrefab[0] = Resources.Load(triggerUIPrefabPath);
            
            uiPrefab[1] = Resources.Load(actionUIPrefabPath);
            
            uiPrefab[2] = Resources.Load(instructionUIPrefabPath);

            propertyUIPrimPrefab = Resources.Load(propertyUIPrimPrefabPath);

            
            //.trigger, .action 파일 불러오기
            OnLoadOriginPrimitivesFromFiles();

            currPreviewImg = new Texture2D(2, 2);

        }

        
        void Start()
        {
            //초기화하기
            initialize();
        }

        //choose type임
        /*
         * idx 0: trigger 
         * idx 1: action
         * idx 2: instruction
         * */
        public void OnChooseTypes(int primIdx, int innerIdx)
        {
            chooseTypeWindow.SetActive(false);
            OnRespawnPrimitives(primIdx, innerIdx);
        }

        //original trigger, action, instruction을 저장된 file로부터 얻는다
        public void OnLoadOriginPrimitivesFromFiles()
        {
            //load trigger

            if (System.IO.Directory.Exists(Application.dataPath + triggerPrefabPath) == false)
            {
                Debug.Log("No path of " + triggerPrefabPath + ", please check trigger file path...");
            }

            //모든 file을 읽어서 .trigger 파일을 확인합시다
            string[] p = System.IO.Directory.GetFiles(Application.dataPath + triggerPrefabPath);

            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                fileStructure.getFileNameNExtension(p[i], ref extension, ref fileName);

                //triggerfile일 경우 바로 deserialize해서 불러오자
                if (extension == "trigger")
                {
                    TriggerPrimitivesTemplate tpt = new TriggerPrimitivesTemplate("", "");

                    tpt.testDeserialize(Application.dataPath + triggerPrefabPath + fileName + "." + extension);

                    //list에 집어넣기
                    triggerList.Add(tpt);
                }
            }

            //load action

            if (System.IO.Directory.Exists(Application.dataPath + actionPrefabPath) == false)
            {
                Debug.Log("No path of " + actionPrefabPath + ", please check action file path...");
            }

            p = System.IO.Directory.GetFiles(Application.dataPath + actionPrefabPath);

            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                fileStructure.getFileNameNExtension(p[i], ref extension, ref fileName);

                if (extension == "action")
                {
                    ActionPrimitivesTemplate apt = new ActionPrimitivesTemplate("", "");

                    apt.testDeserialize(Application.dataPath + actionPrefabPath + fileName + "." + extension);

                    actionList.Add(apt);
                                        
                }
            }


            triggerNameList = new string[triggerList.Count];
            actionNameList = new string[actionList.Count];

            string[] triggerDesList = new string[triggerList.Count];
            string[] actionDesList = new string[actionList.Count];

            //triggerList를 통해서 모든 가능한 trigger의 이름, description을 넣기
            for (int i = 0; i < triggerList.Count; i++)
            {
                triggerNameList[i] = triggerList[i].Name;

                //일단 description은 이름을 넣자
                triggerDesList[i] = triggerList[i].Name;
            }

            for (int i = 0; i < actionList.Count; i++)
            {
                actionNameList[i] = actionList[i].Name;

                //일단 description은 이름을 넣자
                actionDesList[i] = actionList[i].Name;
            }



            //이 부분 짜야 update가능함
            ctm.getInitInnerTypes(triggerNameList, actionNameList, triggerDesList, actionDesList);

        }

        public void OnUpdateProperties(int idx)
        {
            currPrimTemplate = uiManagerList[idx].getPAUTPrimitiveTemplate();

            //primitives type 설정
            primType.text = uiManagerList[idx].getMyUIType().ToString();

            //inner type 설정
            innerType.text = uiManagerList[idx].getMyPrimName();

            //primitives의 이름 설정
            primName.text = uiManagerList[idx].getMyUIType().ToString() + idx.ToString();

            
            //uiManagerList[idx].GetComponent<PropertyVisualizeHandler>().visualizePropertyAll(

            //여기에서 visualizer 실행해야 함...

            //이 부분에서 update properties를 하자

            visualizePrimProperty(uiManagerList[idx]);

            Debug.Log("Update Properties for " + uiManagerList[idx].gameObject.name);
            Debug.Log(uiManagerList[idx].getMyUIType());
        }

        public void visualizePrimProperty(primitivesUIManager pum)
        {
            //prim property 초기화
            clearPrimProperty();



            for (int i = 0; i < currPrimTemplate.getNumberOfParameter(); i++)
            {
                ParameterConversion pc = currPrimTemplate.getParameterValue(i);


                Debug.Log("param name: " + pc.getParamName());
                Debug.Log("param type: " + pc.getParameterType());


                

                //ui prefab instance 생성하기d
                GameObject go = (GameObject)GameObject.Instantiate(propertyUIPrimPrefab, uiPropertyPosition.transform);

                string paramName = pc.getParamName();
                PARAMTYPE pm = pc.getParameterType();
                
                go.name = paramName;

                if(pm == PARAMTYPE.BOOL)
                {
                    go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.TOGGLE, pc.getParameter(), null,i);
                }
                else if(pm == PARAMTYPE.CHOICE)
                {
                    string[] addionalInfo = pc.getAdditionalInfo();
                    go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.DROPDOWN, pc.getParameter(), addionalInfo,i);
                }
                else if(pm == PARAMTYPE.LOCATION)
                {
                    Debug.Log("Not implement...");
                }
                    //나머지는 모두 text input임
                else
                {
                    go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.TEXTINPUT, pc.getParameter(), null,i);
                }

                propertyVisList.Add(go.GetComponent<PropertyVisualizeHandler>());

            }

            /*
            //variable은 일단 제끼자
            for (int i = 0; i < currPrimTemplate.getNumberOfVariable(); i++)
            {
            }
             * */
        }

        public void OnGetValueFromProperty()
        {
            
            for (int i = 0; i < propertyVisList.Count; i++)
            {
                string paramName="";
                string paramValue="";

                paramValue = propertyVisList[i].getValueNParamName(ref paramName);

                if (paramName == "" || paramValue == "")
                {
                    Debug.Log("empty with param Name and value...");
                    return;
                }

                currPrimTemplate.setParameterValue(paramName, paramValue);
            }
        }



        public void clearPrimProperty()
        {
            propertyVisList.Clear();

            for (int i = 0; i < uiPropertyPosition.transform.childCount; i++)
            {
                GameObject.Destroy(uiPropertyPosition.transform.GetChild(i).gameObject);
            }
            
        }
        /*
         *  public void visualizeAssetProperty()
        {
            clearAssetProperty();

            for (int i = 0; i < currAssetInfo.variableContainer.getNumberOfParameters(); i++)
            {
                XmlTemplate xt = currAssetInfo.variableContainer.getParameters(i);

                

                //항목 소환
                GameObject go = (GameObject)GameObject.Instantiate(propertyUIPrefab, new Vector3(), new Quaternion(), propertyTemplatePosition.transform);

                go.name = xt.Name;

                go.GetComponent<PropertyVisualizeHandler>().visualizePropertyAll(xt, i);
            }

        }
         * */

        //저장할 때의 버튼임
        public void OnSaveProperties()
        {
            Debug.Log("Save...");
            OnGetValueFromProperty();
        }
        public TriggerPrimitivesTemplate getTriggerPrimWithName(List<TriggerPrimitivesTemplate> _list, string _name)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Name == _name)
                {
                    return _list[i];
                }
            }

            

            //만일 없을 경우 걍 null을 return함
            return new TriggerPrimitivesTemplate("", "");
        }

        public ActionPrimitivesTemplate getActionPrimWithName(List<ActionPrimitivesTemplate> _list, string _name)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Name == _name)
                {
                    return _list[i];
                }
            }

            //만일 없을 경우 걍 null을 return함
            return new ActionPrimitivesTemplate("","");

        }
       

        //일단은 respawn 먼저 합시당
        public void OnRespawnPrimitives(int primIdx, int innerIdx)
        {
            
            //이 부분에서 몇 번째 trigger, action을 선택할 지도 subwindow를 통해서 받아오자
            
            if(primIdx >= 0 && primIdx < 3)
            {
                //ui template respawn...
                GameObject go = (GameObject)Instantiate(uiPrefab[primIdx], uiTemplatePosition.transform);

                go.GetComponent<RectTransform>().anchoredPosition = currPointDropPos;

                primitivesUIManager pum = go.GetComponent<primitivesUIManager>();



                pum.name = ((PRIMITIVESUI)primIdx).ToString();

                //ui type 정해주기 trigger인지, action인지, instruction인지
                pum.setMyUIType((PRIMITIVESUI)primIdx);
                
                
                pum.setDoubleClickCallback(OnUpdateProperties);

                

                //몇 번째인지 idx 넣기
                pum.setIdx(uiManagerList.Count);

                //부착된 asset 이름 설정
                pum.setName(currAsset.ObjectName);

                //부탁된 asset의 preview image 설정
                pum.setImg(currPreviewImg);

                if(pum.getMyUIType() == PRIMITIVESUI.TRIGGER)
                {
                    pum.setMyPrimName(triggerNameList[innerIdx]);
                    pum.setPAUTPrimitveTemplate(getTriggerPrimWithName(triggerList, triggerNameList[innerIdx]));
                }
                else if(pum.getMyUIType() == PRIMITIVESUI.ACTION)
                {
                    pum.setMyPrimName(actionNameList[innerIdx]);
                    pum.setPAUTPrimitveTemplate(getActionPrimWithName(actionList, actionNameList[innerIdx]));
                }
                else if (pum.getMyUIType() == PRIMITIVESUI.INSTRUCTION)
                {
                    pum.setMyPrimName("General");
                    pum.setPAUTPrimitveTemplate(new InstPrimitivesTemplate("instruction" + pum.idx.ToString(), ""));
                }

                //uiManagerList에 넣기
                uiManagerList.Add(pum);

            }
            else
            {
                Debug.Log("Wrong with primitives idx.. input idx is " + primIdx.ToString());
            }
             

        }

        
        
        public void OnDetectDropOnPrimitives(PointerEventData data)
        {
            //drop이 detect된 경우


            //일단 action을 만들지 trigger를 만들지 instruction을 만들지 결정해야 함
            //--> 오른쪽 버튼 누를 때의 메뉴가 나오면 좋을텐데... UI 짜는게 또 노가다인듯
            //일단 subwindow로 만들어서 setactive를 하자
            if (isDragFromAssetList == true)
            {
                chooseTypeWindow.SetActive(true);
                ctm.OnEnterFirstPhase();

                isDragFromAssetList = false;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(uiTemplatePosition.GetComponent<RectTransform>(), data.position, Camera.main, out currPointDropPos);

            }
        }


        //drag n drop을 timeline있는 상태에서 했을 경우 발생함
        public void OnSelectAsset(string _fileName, AuthorableAsset _currAssetInfo, Texture2D _currPreviewImg)
        {
            currAsset = _currAssetInfo;
            currPreviewImg = _currPreviewImg;
            isDragFromAssetList = true;
        }
    }
}
