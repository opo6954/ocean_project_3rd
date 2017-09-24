using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * timeline window에서event ui manager를 담당함
     * 
     * */

    public enum PRIMDETECTDROP
    {
        TRIGGER, BEFOREACTION, AFTERACTION, INSTRUCTION, NONE
    }

    public class eventUIManager : FileUITemplateManager
    {
        //event 이름 입력하는 곳
        [SerializeField]
        UnityEngine.UI.InputField eventName;
         
        [SerializeField]
        eventInnerTypeManager triggerManager;

        [SerializeField]
        eventInnerTypeListManager beforeActionManager;

        [SerializeField]
        eventInnerTypeListManager afterActionManager;

        [SerializeField]
        eventInnerTypeManager instManager;

        int fromIdx=-1;
        int toIdx=-1;

        public void setFromIdx(int _fromIdx)
        {
            fromIdx = _fromIdx;
        }

        public void setToIdx(int _toIdx)
        {
            toIdx = _toIdx;
        }

        public int getFromIdx()
        {
            return fromIdx;
        }
        public int getToIdx()
        {
            return toIdx;
        }

        //before action list에 들어간 element 개수
        int beforeActionCount=0;
        //after action list에 들어간 element 개수
        int afterActionCount=0;

        //primitves list에서의 trigger의 idx임
        //이거 설정해야 추후에 update를 원활하게 할 수 있음
        int attachedTriggerIdx=-1;
        int attachedInstIdx=-1;
        int[] attachedBeforeActionIdx = new int[3];
        int[] attachedAfterActionIdx = new int[3];

        //properties를 업데이트하는 함수
        public delegate void OnUpdateProperties(int primIdx);
        public OnUpdateProperties callback;

        public void setOnUpdateProperties(OnUpdateProperties _callback)
        {
            callback = _callback;

            triggerManager.setOnClickAssetImg(UpdatePropertiesCallbackFromAssetClick);
            instManager.setOnClickAssetImg(UpdatePropertiesCallbackFromAssetClick);
            beforeActionManager.setOnClickAssetImg(UpdatePropertiesCallbackFromAssetClick);
            afterActionManager.setOnClickAssetImg(UpdatePropertiesCallbackFromAssetClick);

        }

        public void UpdatePropertiesCallbackFromAssetClick(int orderIdx, PRIMDETECTDROP _type)
        {
            switch (_type)
            {
                case PRIMDETECTDROP.TRIGGER:
                    if (attachedTriggerIdx == -1)
                        return;
                    callback(attachedTriggerIdx);
                    break;
                case PRIMDETECTDROP.INSTRUCTION:
                    if (attachedInstIdx == -1)
                        return;
                    callback(attachedInstIdx);
                    break;
                case PRIMDETECTDROP.BEFOREACTION:
                    if (attachedBeforeActionIdx[orderIdx] == -1)
                        return;
                    callback(attachedBeforeActionIdx[orderIdx]);
                    break;
                case PRIMDETECTDROP.AFTERACTION:
                    if (attachedAfterActionIdx[orderIdx] == -1)
                        return;
                    callback(attachedAfterActionIdx[orderIdx]);
                    break;
            }

        }





        //본 eventUIManager와 연결된 authorableEvent를 저장함
        public AuthorableEvent eventInfo = new AuthorableEvent();

        public bool isInitEvent = false;


        public void setPrimitivesIdx(int _idx, PRIMDETECTDROP _type)
        {
            switch (_type)
            {
                case PRIMDETECTDROP.TRIGGER:
                    attachedTriggerIdx = _idx;
                    break;
                case PRIMDETECTDROP.INSTRUCTION:
                    attachedInstIdx = _idx;
                    break;
                case PRIMDETECTDROP.BEFOREACTION:
                    beforeActionManager.setCurrPrimIdx(_idx);
                    break;
                case PRIMDETECTDROP.AFTERACTION:
                    afterActionManager.setCurrPrimIdx(_idx);
                    break;
                    
            }
        }


        public void initEvent()
        {
            if (isInitEvent == false)
            {
                eventInfo.initialize();
                isInitEvent = true;
                for (int i = 0; i < 3; i++)
                {
                    attachedBeforeActionIdx[i] = -1;
                    attachedAfterActionIdx[i] = -1;
                }

                

            }
        }

        //event에 drop 되었는지 확인하기
        public bool isDropOnEvent(Vector2 position, ref PRIMDETECTDROP dropSign)
        {
            dropSign = PRIMDETECTDROP.NONE;


            if (triggerManager.isDetectDrop(position) == true)
            {
                dropSign = PRIMDETECTDROP.TRIGGER;
                return true;
            }
            if (beforeActionManager.isDetectDrop(position) == true)
            {
                dropSign = PRIMDETECTDROP.BEFOREACTION;
                return true;
            }
            if (afterActionManager.isDetectDrop(position) == true)
            {
                dropSign = PRIMDETECTDROP.AFTERACTION;
                return true;
            }
            if (instManager.isDetectDrop(position) == true)
            {
                dropSign = PRIMDETECTDROP.INSTRUCTION;
                return true;
            }
            Debug.Log("No drop on inner component...");
            return false;
        }

        public void setTrigger(PAUTPrimitivesTemplate _origin)
        {
            //일단 삭제한 후
            deleteTrigger();
            //다시 집어넣기
            eventInfo.addTrigger(_origin);
        }
        public void deleteTrigger()
        {
            (eventInfo.variableContainer.getParameters(1) as PAUTPrimitivesTemplate).printAllParameter();

            eventInfo.deleteTrigger();
            eventInfo.initialize();
        }
        public void setInstruction(PAUTPrimitivesTemplate _origin)
        {
            deleteInstruction();
            eventInfo.addInstruction(_origin);
            Debug.Log("Instruction is set...");
        }
        public void deleteInstruction()
        {
            eventInfo.deleteInstruction();
            Debug.Log("Instruction is deleted...");
        }

        public void pushBeforeAction(PAUTPrimitivesTemplate _origin)
        {
            if (beforeActionCount == 3)
            {
                Debug.Log("Already full of before action list...");
                return;
            }
            beforeActionCount++;
            eventInfo.addBeforeAction(_origin);
            
        }
        public void pushAfterAction(PAUTPrimitivesTemplate _origin)
        {
            if (afterActionCount == 3)
            {
                Debug.Log("Already full of after action list...");
                return;
            }
            afterActionCount++;
            eventInfo.addAfterAction(_origin);
        }
        public void popBeforeAction(PAUTPrimitivesTemplate _origin)
        {
            if (beforeActionCount > 0)
            {
                eventInfo.deleteBeforeAction(_origin);
                beforeActionCount--;
                return;
            }
            Debug.Log("No element in beforeactionlist...");

        }
        public void popAfterAction(PAUTPrimitivesTemplate _origin)
        {
            if (afterActionCount > 0)
            {
                eventInfo.deleteAfterAction(_origin);
                afterActionCount--;
                return;
            }
            Debug.Log("No element in afteractionlist...");
        }

        //각기 요소별로 image 설정해주기
        public void setImage(Texture2D _image, PRIMDETECTDROP _type)
        {
            switch (_type)
            {
                case PRIMDETECTDROP.TRIGGER:
                    triggerManager.setAssetImg(_image);
                    break;
                case PRIMDETECTDROP.INSTRUCTION:
                    instManager.setAssetImg(_image);
                    break;
                case PRIMDETECTDROP.AFTERACTION:
                    afterActionManager.setAssetImg(_image);
                    attachedAfterActionIdx[afterActionManager.getCurrOrderIdx()] = afterActionManager.getCurrPrimIdx();

                    break;
                case PRIMDETECTDROP.BEFOREACTION:
                    beforeActionManager.setAssetImg(_image);
                    attachedBeforeActionIdx[beforeActionManager.getCurrOrderIdx()] = beforeActionManager.getCurrPrimIdx();
                    break;
                    
            }
        }



        
        



    }
}