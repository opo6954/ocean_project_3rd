using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class AuthoringTest : MonoBehaviour {

	    // Use this for initialization
	    void Start () {
            
            //Test for scenario
            /*
            AuthorableScenario scenario = new AuthorableScenario();

            scenario.initialize();
            scenario.example();
            scenario.testSerialize("test.scenario");

            AuthorableScenario scenarioTmp = new AuthorableScenario();
            scenarioTmp.initialize();
            scenarioTmp.testDeserialize("test.scenario");
            scenarioTmp.testSerialize("testTmp.scenario");
            */

            /*
            //Test for timeline            
            AuthorableTimeline at = new AuthorableTimeline();
            at.initialize();
            at.example();

            at.testSerialize("tmpTimeline.timeline");

            AuthorableTimeline atmp = new AuthorableTimeline();

            atmp.initialize();
            atmp.testDeserialize("tmpTimeline.timeline");
            atmp.testSerialize("tmpTimeline2.timeline");
            */
            
            
            //Test for authorable event
            /*
            AuthorableEvent ee = new AuthorableEvent();
            ee.initialize();
            ee.example2();

            ee.testSerialize("tmpEvent.event");

            AuthorableEvent ee2 = new AuthorableEvent();
            ee2.initialize();
            ee2.testDeserialize("tmpEvent.event");
            ee2.testSerialize("MyEvent2.event");
            */



             //Test for Instruction xml primitives
            /*
            InstPrimitivesTemplate ipt = new InstPrimitivesTemplate("", "");
            ipt.testSerializeWithExt("instruction");
            ipt.printAllParameter();
            */
            
             

            
             //Test for Action xml primitives
            /*
            ActionPrimitivesTemplate apt = new ActionPrimitivesTemplate("", "");
            string ext = "action";

            apt.exampleActiveRenderAction();apt.testSerializeWithExt(ext);
            apt.exampleDeactiveRenderAction();apt.testSerializeWithExt(ext);
            apt.exampleEnableAssetTriggerAction();apt.testSerializeWithExt(ext);
            apt.exampleDisableAssetTriggerAction();apt.testSerializeWithExt(ext);
            apt.exampleStartParticleTriggerAction();apt.testSerializeWithExt(ext);
            apt.exampleStopParticleTriggerAction();apt.testSerializeWithExt(ext);


            ActionPrimitivesTemplate apt2 = new ActionPrimitivesTemplate("", "");
            apt2.testDeserialize("DisableAssetTriggerAction.action");
            

            
             
             
            
             //Test for Trigger xml primitives
            TriggerPrimitivesTemplate tpt = new TriggerPrimitivesTemplate("GazeTrigger", "TriggerPrimitivesTemplate");

            tpt.exampleGazeTrigger();tpt.testSerialize("GazeTrigger.trigger");
            tpt.exampleInputDownTrigger();tpt.testSerialize("InputDownTrigger.trigger");
            tpt.exampleCollisionTrigger();tpt.testSerialize("CollisionTrigger.trigger");
            tpt.exampleMonitorDistanceCapturedTrigger();tpt.testSerialize("MonitorDistanceCapturedTrigger.trigger");


            
            TriggerPrimitivesTemplate tptTmp = new TriggerPrimitivesTemplate("", "");
            PAUTPrimitivesTemplate t = tptTmp as PAUTPrimitivesTemplate;

            PAUTPrimitivesTemplate.CopyPAUT(tpt as PAUTPrimitivesTemplate, ref t);


            Debug.Log("For origin, " + tpt.Name);
            tpt.printAllParameter();

            Debug.Log("For copy dest, " + t.Name);
            t.printAllParameter();


            t.testSerialize("MonitorDistanceCapturedTriggerTmp.trigger");
            

            //TriggerPrimitivesTemplate tpt2 = new TriggerPrimitivesTemplate("", "");
            //tpt2.testDeserialize("GazeTrigger.trigger");tpt2.testSerialize("GazeTrigger2.trigger");

            //tpt2.testDeserialize("MonitorDistanceCapturedTrigger.trigger");tpt2.testSerialize("MonitorDistanceCapturedTrigger2.trigger");
            */
            
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
}