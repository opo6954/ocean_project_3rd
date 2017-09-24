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

        //primitves에서 uiTemplate이 놓이는 부분임
        [SerializeField]
        GameObject uiTemplatePosition;


        //timeline에서 uiTemplate이 노이는 부분임
        [SerializeField]
        GameObject eventUITemplatePosition;


        [SerializeField]
        ChooseTypeManager ctm;

        [SerializeField]
        UnityEngine.UI.Text primType;

        [SerializeField]
        UnityEngine.UI.Text innerType;

        [SerializeField]
        UnityEngine.UI.InputField nameFromUser;

        //ui property position이 생성되는 곳
        [SerializeField]
        GameObject uiPropertyPosition;

        [SerializeField]
        NodeLineManager beginNode;
        

        AuthorableAsset currAsset;
        Texture2D currPreviewImg;

        PAUTPrimitivesTemplate currPrimTemplate;

        //prim에서 property에서의 ui prefab임
        Object propertyUIPrimPrefab;


        //event의 prefab임
        Object eventUIPrefab;


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

        string eventUIPrefabPath = "TimelineEditor/eventUITemplate";

        //순서대로 trigger, action, instruction임
        Object[] uiPrefab;


        //현재 respawn된 uiManagerList임
        List<primitivesUIManager> uiManagerList = new List<primitivesUIManager>();

        //현재 respawn된 eventUIManagerList임
        List<eventUIManager> eventuiManagerList = new List<eventUIManager>();


        //일단 event의 idx를 저장하는 형태의 list로 만들고 최종 저장할 때 timeline을 구축하자
        //서로 연결된 형태의 timeline list
        List<int> eventListWithIdx = new List<int>();

        //저장된 trigger 경로
        string triggerPrefabPath = "/Resources/EventFiles/trigger/";

        //저장된 action 경로
        string actionPrefabPath = "/Resources/EventFiles/action/";

        

        //drag start가 assetlist로부터 왔는지 체크하기
        bool isDragFromAssetList = false;

        //drag start가 primitve로부터 왔는지 체크하기d
        bool isDragFromPrimitives = false;

        //drag start가 timeline으로부터 왔는지 체크하기
        bool isDragFromTimeline = false;


        //가장 최근 drop을 한 position을 저장함(primitives에서)
        Vector2 currPointDropPos = new Vector2();

        //가장 최근 timeline window에서 drop을 한 positio을 저장함
        Vector2 currPointDropPosTimeline = new Vector2();




        //현재 가지고 있는 trigger이름
        string[] triggerNameList;

        //현재 가지고 있는 action이름
        string[] actionNameList;


        //currently drag된 idx

        int currDraggedIdx = -1;
        int currDraggedEventIdx = -1;

        //currently property가 보여지는 idxㅇ
        int currPropertyIdx = -1;

        //최종적인 authorable scenario임
        AuthorableScenario authorableScenario;

        



        public void initialize()
        {
            //trigger, action, instructino ui prefab 모두 불러오기
            uiPrefab = new Object[3];


            uiPrefab[0] = Resources.Load(triggerUIPrefabPath);
            
            uiPrefab[1] = Resources.Load(actionUIPrefabPath);
            
            uiPrefab[2] = Resources.Load(instructionUIPrefabPath);

            propertyUIPrimPrefab = Resources.Load(propertyUIPrimPrefabPath);

            eventUIPrefab = Resources.Load(eventUIPrefabPath);


            
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

            //idx 설정
            currPropertyIdx = idx;

            //primitives type 설정
            primType.text = uiManagerList[idx].getMyUIType().ToString();

            //inner type 설정
            innerType.text = uiManagerList[idx].getMyPrimName();

            //초기 user로부터 보
            string _name = uiManagerList[idx].getNameFromUser();
            if (_name == "")
            {
                uiManagerList[idx].setNameFromUser(uiManagerList[idx].getMyUIType().ToString() + idx.ToString());
            }
            
            nameFromUser.text = uiManagerList[idx].getNameFromUser();
            

            //property visualizer 실행함
            visualizePrimProperty(uiManagerList[idx]);


        }

        public void visualizePrimProperty(primitivesUIManager pum)
        {
            //prim property 초기화
            clearPrimProperty();



            for (int i = 0; i < currPrimTemplate.getNumberOfParameter(); i++)
            {
                ParameterConversion pc = currPrimTemplate.getParameterValue(i);




                

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


            uiManagerList[currPropertyIdx].setNameFromUser(nameFromUser.text);
            //uiManagerList[currPropertyIdx].setNameFromUser("TMP");



        }



        public void clearPrimProperty()
        {
            propertyVisList.Clear();

            for (int i = 0; i < uiPropertyPosition.transform.childCount; i++)
            {
                GameObject.Destroy(uiPropertyPosition.transform.GetChild(i).gameObject);
            }
            
        }

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


        //event를 respawn하는거임

        //몇 번 째 primitive idx를 가지고 계산함
        //일단 기본적으로 trigger가 있기 때문에 trigger 관련된 update를 해줘야 할텐데..
        public void OnRespawnEvent()
        {
            Debug.Log("Respawn for event...");
            GameObject go = (GameObject)Instantiate(eventUIPrefab, eventUITemplatePosition.transform);

            //일단 이 위치로 respawn을 해보자
            //go.GetComponent<RectTransform>().anchoredPosition = currPointDropPos;
            go.GetComponent<RectTransform>().anchoredPosition = currPointDropPosTimeline;

            eventUIManager eum = go.GetComponent<eventUIManager>();


            //idx 설정하기(list로부터 instance를 따오기 위한..)

            eum.setIdx(eventuiManagerList.Count);

            if (uiManagerList[currDraggedIdx].getMyUIType() == PRIMITIVESUI.TRIGGER)
            {
                eum.initEvent();
                //trigger 설정하기
                

                eum.setTrigger(uiManagerList[currDraggedIdx].getPAUTPrimitiveTemplate());
                eum.setImage(uiManagerList[currDraggedIdx].currImg, PRIMDETECTDROP.TRIGGER);
                eum.setPrimitivesIdx(currDraggedIdx, PRIMDETECTDROP.TRIGGER);
            }
            else
            {
                Debug.Log("dragged is not trigger... something wrong...");
            }
            //eum에 추후에 드래그, 클릭 등의 콜백 함수를 설정해야 함...

            eum.setOnDragListener(OnDragFromTimeline);
            eum.setOnUpdateProperties(OnUpdateProperties);
            


            //list에 넣기
            eventuiManagerList.Add(eum);
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

                pum.setOnDragListener(OnDragFromPrimitves);

                //uiManagerList에 넣기
                uiManagerList.Add(pum);
            }
            else
            {
                Debug.Log("Wrong with primitives idx.. input idx is " + primIdx.ToString());
            }

            

                


        }


        public void OnDragFromPrimitves(int idx)
        {
            Debug.Log("Detect drag from primiteves with " + uiManagerList[idx].getMyPrimName());
            isDragFromPrimitives = true;
            isDragFromTimeline = false;
            isDragFromAssetList = false;
            //drag하고 있는 primitives의 idx를 저장함
            currDraggedIdx = idx;
        }

        //timeline window에서 drag가 시작될 경우
        public void OnDragFromTimeline(int idx)
        {
            //LHWLHW
            Debug.Log("Detect drag start on timeline window with ");
            
            isDragFromTimeline = true;
            isDragFromPrimitives = false;
            isDragFromAssetList = false;
            //drag하고 있는 event의 idx를 저장함
            currDraggedEventIdx = idx;
        }


        //timeline window에서 drop이 detect될 경우에 불려짐
        public void OnDetectDropOnTimeline(PointerEventData data)
        {
            Debug.Log("Detect on primitves names " + uiManagerList[currDraggedIdx].getMyPrimName());



            

            RectTransformUtility.ScreenPointToLocalPointInRectangle(eventUITemplatePosition.GetComponent<RectTransform>(), new Vector2(data.position.x, data.position.y), Camera.main, out currPointDropPosTimeline);

            if (isDragFromPrimitives == true)
            {
                PRIMITIVESUI uiType = uiManagerList[currDraggedIdx].getMyUIType();
                PRIMDETECTDROP dropSign = PRIMDETECTDROP.NONE;
                int selectedEventIdx = -1;

                switch (uiType)
                {
                    //trigger만 특수하게 처리함

                    case PRIMITIVESUI.TRIGGER:

                        //eventui에 drop한 경우
                        if (isDropOnAnyEventUI(data, uiType, ref dropSign, ref selectedEventIdx) == true)
                        {
                            Debug.Log("Drop on event..");
                            //만일 trigger region에 떨어졌을 경우 trigger를 바꾸기d
                            if (dropSign == PRIMDETECTDROP.TRIGGER)
                            {
                                
                                eventuiManagerList[selectedEventIdx].setTrigger(uiManagerList[currDraggedIdx].getPAUTPrimitiveTemplate());
                                eventuiManagerList[selectedEventIdx].setPrimitivesIdx(currDraggedIdx, PRIMDETECTDROP.TRIGGER);
                                eventuiManagerList[selectedEventIdx].setImage(uiManagerList[currDraggedIdx].currImg, dropSign);
                            }
                        }
                        //걍 빈 공간에 drop한 경우 --> 새롭게 event를 respawn하자
                        else
                        {
                            OnRespawnEvent();
                            Debug.Log("Drop on space with trigger...");
                        }
                        break;
                    //action이나 instruction인 경우 
                    default:
                        if (isDropOnAnyEventUI(data, uiType, ref dropSign, ref selectedEventIdx) == true)
                        {
                            Debug.Log("Drop on event...");
                            //action primitive이고
                            if (uiType == PRIMITIVESUI.ACTION)
                            {
                                //before action에 드랍될 경우
                                if (dropSign == PRIMDETECTDROP.BEFOREACTION)
                                {
                                    eventuiManagerList[selectedEventIdx].pushBeforeAction(uiManagerList[currDraggedIdx].getPAUTPrimitiveTemplate());
                                    eventuiManagerList[selectedEventIdx].setPrimitivesIdx(currDraggedIdx, PRIMDETECTDROP.BEFOREACTION);
                                    eventuiManagerList[selectedEventIdx].setImage(uiManagerList[currDraggedIdx].currImg, dropSign);
                                    
                                    
                                    
                                }
                                //after action에 드랍될 경우
                                else if (dropSign == PRIMDETECTDROP.AFTERACTION)
                                {
                                    eventuiManagerList[selectedEventIdx].pushAfterAction(uiManagerList[currDraggedIdx].getPAUTPrimitiveTemplate());
                                    eventuiManagerList[selectedEventIdx].setPrimitivesIdx(currDraggedIdx, PRIMDETECTDROP.AFTERACTION);
                                    eventuiManagerList[selectedEventIdx].setImage(uiManagerList[currDraggedIdx].currImg, dropSign);
                                }
                            }
                                //instruction primitives이고
                            else if(uiType == PRIMITIVESUI.INSTRUCTION)
                            {
                                //instruction 영역에 떨어졌을 경우
                                if (dropSign == PRIMDETECTDROP.INSTRUCTION)
                                {
                                    eventuiManagerList[selectedEventIdx].setInstruction(uiManagerList[currDraggedIdx].getPAUTPrimitiveTemplate());
                                    eventuiManagerList[selectedEventIdx].setPrimitivesIdx(currDraggedIdx, PRIMDETECTDROP.INSTRUCTION);
                                    eventuiManagerList[selectedEventIdx].setImage(uiManagerList[currDraggedIdx].currImg, dropSign);
                                }
                            }
                            
                        }
                        break;
                }


                isDragFromPrimitives = false;   

                Debug.Log("Drop detect on timeline window...");
            }
            
                //이 경우에는 이동만 함
            else if (isDragFromTimeline)
            {
                //위치 바꾸기

                var rt = eventuiManagerList[currDraggedEventIdx].gameObject.GetComponent<RectTransform>();

                RectTransformUtility.ScreenPointToLocalPointInRectangle(eventUITemplatePosition.GetComponent<RectTransform>(), new Vector2(data.position.x - rt.rect.width / 2, data.position.y + rt.rect.height / 2), Camera.main, out currPointDropPosTimeline);

                rt.anchoredPosition = currPointDropPosTimeline;

                isDragFromTimeline = false;
                Debug.Log("Drag From primitves and drop from primitves");
            }
        }



        public bool isDropOnAnyEventUI(PointerEventData data, PRIMITIVESUI myType, ref PRIMDETECTDROP dropSign, ref int selectedIdx)
        {
            for (int i = 0; i < eventuiManagerList.Count; i++)
            {
                if (eventuiManagerList[i].isDropOnEvent(data.position, ref dropSign) == true)
                {
                    selectedIdx = i;
                    return true;
                }
            }
            return false;
        }

        //primitves editor에서 drop이 detect될 경우에 불려짐
        public void OnDetectDropOnPrimitives(PointerEventData data)
        {
            //drop이 detect된 경우


            //일단 action을 만들지 trigger를 만들지 instruction을 만들지 결정해야 함
            //--> 오른쪽 버튼 누를 때의 메뉴가 나오면 좋을텐데... UI 짜는게 또 노가다인듯
            //일단 subwindow로 만들어서 setactive를 하자

            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiTemplatePosition.GetComponent<RectTransform>(), new Vector2(data.position.x, data.position.y), Camera.main, out currPointDropPos);

            if (isDragFromAssetList == true)
            {
                chooseTypeWindow.SetActive(true);
                ctm.OnEnterFirstPhase();

                isDragFromAssetList = false;

                
            }
            //primitives window로부터 drag가 시작되었을 경우 drop이 여기서 검출시
            else if (isDragFromPrimitives == true)
            {
                //위치 바꾸기

                var rt = uiManagerList[currDraggedIdx].gameObject.GetComponent<RectTransform>();

                RectTransformUtility.ScreenPointToLocalPointInRectangle(uiTemplatePosition.GetComponent<RectTransform>(), new Vector2(data.position.x - rt.rect.width / 2, data.position.y + rt.rect.height / 2), Camera.main, out currPointDropPos);
                
                rt.anchoredPosition = currPointDropPos;

                isDragFromPrimitives = false;
                Debug.Log("Drag From primitves and drop from primitves");
            }
        }

        //drag n drop을 timeline있는 상태에서 했을 경우 발생함
        public void OnSelectAsset(string _fileName, AuthorableAsset _currAssetInfo, Texture2D _currPreviewImg)
        {
            currAsset = _currAssetInfo;
            currPreviewImg = _currPreviewImg;
            isDragFromAssetList = true;
            isDragFromPrimitives = false;
        }

        public void OnExportAll()
        {
            Debug.Log("Export all and save as xml...");

            //eventListWithIdx

            
            int startIdx = beginNode.toIdx;

            if(startIdx < 0)
            {
                Debug.Log("No available timelien exist...");
                return;
            }

            
            int testIdx = startIdx;
            //탐색하면서 eventListWithIdx를 구축하기

            //일단 초기화 한 후에 시작
            eventListWithIdx.Clear();

            int q = 0;
            while (q < 10)
            {
                q++;
                Debug.Log("test idx: " + testIdx);
                
                
                int nextIdx = eventuiManagerList[testIdx].getToIdx();

                if (nextIdx < 0)
                    break;
                else
                {
                    testIdx = nextIdx;
                    eventListWithIdx.Add(testIdx);
                }
                
            }

            //이제 모든 timeline 만들기

            AuthorableTimeline at = new AuthorableTimeline();
            at.initialize();
            //걍 1명에 대해서 수행하기(Sailor)
            (at.variableContainer.getParameters("Player") as PrimitiveXmlTemplate).setparameter("Sailor");

            for (int i = 0; i < eventListWithIdx.Count; i++)
            {
                //event 넣기
                at.eventList.Add(eventuiManagerList[eventListWithIdx[i]].eventInfo);
            }
            authorableScenario = new AuthorableScenario();

            authorableScenario.initialize();

            //임시로 이렇게 넣자
            authorableScenario.ObjectName = "FireExtinguisherTraining";

            //역할 정보
            ListOfXmlTemplate lxtRole = authorableScenario.variableContainer.getParameters("TraineeRoleInfo") as ListOfXmlTemplate;
            
            lxtRole.addList(new PrimitiveXmlTemplate("Role1", "Captain", "string"));
            lxtRole.addList(new PrimitiveXmlTemplate("Role2", "Sailor1", "string"));

            //훈련 환경 정보(VR/AR/Desktop)
            ListOfXmlTemplate lxtCondition = authorableScenario.variableContainer.getParameters("TrainCondition") as ListOfXmlTemplate;

            lxtCondition.addList(new PrimitiveXmlTemplate("Platform1", "VR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform2", "AR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform3", "Desktop", "string"));

            //room 집어 넣기
            authorableScenario.roomList.Add(EnvironmentEditor._currRoomGlobal);

            //asset 집어넣기
            for (int i = 0; i < AssetListWindowHandler.authorableAssetListGlobal.Count; i++)
            {
                authorableScenario.assetList.Add(AssetListWindowHandler.authorableAssetListGlobal[i]);
            }

            authorableScenario.timelineList.Add(at);

            //일단 임시로 걍 저장하자

            //.scenario 파일로 serailize하기
            authorableScenario.testSerialize("power.scenario");




        }
    }
}
