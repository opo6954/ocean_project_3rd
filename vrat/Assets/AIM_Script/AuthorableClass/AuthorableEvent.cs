using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * Event를 serialize하는 authorable template임
     * */
    public class AuthorableEvent : ObjectTemplate
    {
        public override void initialize()
        { 
            base.initialize();
            ObjectType = OBJTYPE.EVENT;
            initForEvent();
            Debug.Log("init?");
        }
        /*
         * 일단 hard coding으로 Trigger를 불러오자...
         * */

        public void addTrigger(PAUTPrimitivesTemplate origin)
        {
            //trigger 복사하기
            PAUTPrimitivesTemplate tpt = variableContainer.getParameters(1) as PAUTPrimitivesTemplate;
            PAUTPrimitivesTemplate.CopyPAUT(origin, ref tpt);
        }
        public void deleteTrigger()
        {
            TriggerPrimitivesTemplate tpt = variableContainer.getParameters(1) as TriggerPrimitivesTemplate;
            tpt.clearParamValueAll();
        }

        public void addBeforeAction(PAUTPrimitivesTemplate origin)
        {
            //action 복사하기
            

            ActionPrimitivesTemplate beforeAct = new ActionPrimitivesTemplate("", "");

            PAUTPrimitivesTemplate beforeActBase = beforeAct as PAUTPrimitivesTemplate;

            PAUTPrimitivesTemplate.CopyPAUT(origin, ref beforeActBase);

            Debug.Log("before act: " + beforeAct.Name);
            Debug.Log("list name of event: " + (variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate).Name);
            

            (variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate).addList(beforeAct);
        }
        public void addAfterAction(PAUTPrimitivesTemplate origin)
        {
            ActionPrimitivesTemplate afterAct = new ActionPrimitivesTemplate("", "");

            PAUTPrimitivesTemplate afterActBase = afterAct as PAUTPrimitivesTemplate;

            PAUTPrimitivesTemplate.CopyPAUT(origin, ref afterActBase);

            (variableContainer.getParameters("AfterActionList") as ListOfXmlTemplate).addList(afterActBase);
        }

        public void addInstruction(PAUTPrimitivesTemplate origin)
        {
            InstPrimitivesTemplate instruction = new InstPrimitivesTemplate("", "");

            PAUTPrimitivesTemplate instructionBase = instruction as PAUTPrimitivesTemplate;

            PAUTPrimitivesTemplate.CopyPAUT(origin, ref instructionBase);

            (variableContainer.getParameters("InstructionList") as ListOfXmlTemplate).addList(instructionBase);
        }

        public void deleteBeforeAction(PAUTPrimitivesTemplate origin)
        {
            (variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate).deleteList(origin);
        }

        public void deleteAfterAction(PAUTPrimitivesTemplate origin)
        {
            (variableContainer.getParameters("AfterActionList") as ListOfXmlTemplate).deleteList(origin);
        }
        //제일 끝에 있는 instruction 지우기
        public void deleteInstruction()
        {
            (variableContainer.getParameters("InstructionList") as ListOfXmlTemplate).deleteListEnd();
        }
        public void deleteInstruction(PAUTPrimitivesTemplate origin)
        {
            (variableContainer.getParameters("InstructionList") as ListOfXmlTemplate).deleteList(origin);
        }

        //event는 asset과는 다르게 기본 얘들이 정해져 있음
        void initForEvent()
        { 
            //attached asset 설정

            variableContainer.addParameter(new PrimitiveXmlTemplate("AttachedAsset", "string"));

            //Trigger 종류 설정
            variableContainer.addParameter(new TriggerPrimitivesTemplate("Trigger", "TriggerTemplate"));

            //Before Action, After Action, Instruction list 정의하기
            variableContainer.addParameter(new ListOfXmlTemplate("BeforeActionList", "ListOfXmlTemplate", 0));
            variableContainer.addParameter(new ListOfXmlTemplate("AfterActionList", "ListOfXmlTemplate", 0));
            variableContainer.addParameter(new ListOfXmlTemplate("InstructionList", "ListOfXmlTemplate", 0));

            //본 event를 소유하고 있는 역할
            variableContainer.addParameter(new PrimitiveXmlTemplate("OwnPlayer", "string"));
        }
        public void example2()
        {
            (variableContainer.getParameters("AttachedAsset") as PrimitiveXmlTemplate).setparameter("extinguisher");

            //trigger는 이미 attach된 상태임

            TriggerPrimitivesTemplate tpt = variableContainer.getParameters("Trigger") as TriggerPrimitivesTemplate;

            //trigger 부르기
            TriggerPrimitivesTemplate triggerFromFile = new TriggerPrimitivesTemplate("", "");
            triggerFromFile.testDeserialize("GazeTrigger.trigger");
            PAUTPrimitivesTemplate tptBase = tpt as PAUTPrimitivesTemplate;



            //action set 부르기
            ActionPrimitivesTemplate act = new ActionPrimitivesTemplate("", "");
            act.testDeserialize("StartParticleTriggerAction.action");
            
            ActionPrimitivesTemplate act2 = new ActionPrimitivesTemplate("", "");
            act2.testDeserialize("StopParticleTriggerAction.action");


            //action 만들기 list니까 action은 새로 만들어야 함
            ActionPrimitivesTemplate beforeAct1 = new ActionPrimitivesTemplate("", "");
            ActionPrimitivesTemplate afterAct1 = new ActionPrimitivesTemplate("", "");            






            //action 복사하기
            PAUTPrimitivesTemplate t = beforeAct1 as PAUTPrimitivesTemplate;
            PAUTPrimitivesTemplate.CopyPAUT(act as PAUTPrimitivesTemplate, ref t);

            t = afterAct1 as PAUTPrimitivesTemplate;
            PAUTPrimitivesTemplate.CopyPAUT(act2 as PAUTPrimitivesTemplate, ref t);


            //trigger 복사하기
            PAUTPrimitivesTemplate.CopyPAUT(triggerFromFile as PAUTPrimitivesTemplate, ref tptBase);


            //action list에 집어 넣기
            (variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate).addList(beforeAct1);
            (variableContainer.getParameters("AfterActionList") as ListOfXmlTemplate).addList(afterAct1);


            //instruction test

            //instruction은 걍 이렇게 간단하게 하면 됩니당

            InstPrimitivesTemplate ipt = new InstPrimitivesTemplate("", "");

            ipt.setParameterValue("Instruction", "어캐 하는 거죠?");
            ipt.setParameterValue("IsGlobalOrLocal", true.ToString());


            (variableContainer.getParameters("InstructionList") as ListOfXmlTemplate).addList(ipt);
        }
        //일단 대충 example을 만들어 보자
        public void example()
        {
            (variableContainer.getParameters("AttachedAsset") as PrimitiveXmlTemplate).setparameter("fire");

            //trigger는 이미 attach된 상태임

            TriggerPrimitivesTemplate tpt = variableContainer.getParameters("Trigger") as TriggerPrimitivesTemplate;

            //trigger 부르기
            TriggerPrimitivesTemplate triggerFromFile = new TriggerPrimitivesTemplate("", "");
            triggerFromFile.testDeserialize("MonitorDistanceCapturedTrigger.trigger");
            PAUTPrimitivesTemplate tptBase = tpt as PAUTPrimitivesTemplate;        

            

            //action set 부르기
            ActionPrimitivesTemplate act = new ActionPrimitivesTemplate("", "");
            act.testDeserialize("DisableAssetTriggerAction.action");

            ActionPrimitivesTemplate act2 = new ActionPrimitivesTemplate("", "");
            act2.testDeserialize("EnableAssetTriggerAction.action");

            ActionPrimitivesTemplate act3 = new ActionPrimitivesTemplate("", "");
            act3.testDeserialize("ActiveRenderAction.action");

            ActionPrimitivesTemplate act4 = new ActionPrimitivesTemplate("", "");
            act4.testDeserialize("DeactiveRenderAction.action");


            //action 만들기 list니까 action은 새로 만들어야 함
            ActionPrimitivesTemplate beforeAct1 = new ActionPrimitivesTemplate("", "");
            ActionPrimitivesTemplate beforeAct2 = new ActionPrimitivesTemplate("", "");
            ActionPrimitivesTemplate afterAct1 = new ActionPrimitivesTemplate("", "");
            ActionPrimitivesTemplate afterAct2 = new ActionPrimitivesTemplate("", "");

            

            


            //action 복사하기
            PAUTPrimitivesTemplate t = beforeAct1 as PAUTPrimitivesTemplate;
            PAUTPrimitivesTemplate.CopyPAUT(act as PAUTPrimitivesTemplate, ref t);

            t = beforeAct2 as PAUTPrimitivesTemplate;
            PAUTPrimitivesTemplate.CopyPAUT(act2 as PAUTPrimitivesTemplate, ref t);

            t = afterAct1 as PAUTPrimitivesTemplate;
            PAUTPrimitivesTemplate.CopyPAUT(act3 as PAUTPrimitivesTemplate, ref t);

            t = afterAct2 as PAUTPrimitivesTemplate;
            PAUTPrimitivesTemplate.CopyPAUT(act4 as PAUTPrimitivesTemplate, ref t);
            
            //trigger 복사하기
            PAUTPrimitivesTemplate.CopyPAUT(triggerFromFile as PAUTPrimitivesTemplate, ref tptBase);


            //action list에 집어 넣기
            (variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate).addList(beforeAct1);
            (variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate).addList(beforeAct2);
            (variableContainer.getParameters("AfterActionList") as ListOfXmlTemplate).addList(afterAct1);
            (variableContainer.getParameters("AfterActionList") as ListOfXmlTemplate).addList(afterAct2);


            //instruction test

            //instruction은 걍 이렇게 간단하게 하면 됩니당

            InstPrimitivesTemplate ipt = new InstPrimitivesTemplate("", "");

            ipt.setParameterValue("Instruction", "이거 뭡니까");
            ipt.setParameterValue("IsGlobalOrLocal", false.ToString());


            (variableContainer.getParameters("InstructionList") as ListOfXmlTemplate).addList(ipt);

        }
    }
}