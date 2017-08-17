using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    public class EventTemplate : ObjectTemplate
    {

        //Ending Condition of event...

        //end condition을 만나게 되면 설정된 callback 함수를 실행하면서 다음 event로 이동됨

        //Attached Asset on Event... only one asset can be attached to event...
        public AssetTemplate attachedAsset;
        
        private EndConditionTemplate ect;

        private EventTemplate nextEvent;





        //end Condition 설정하기
        public virtual void setEndCondition(EndConditionTemplate _ect)
        {
            ect = _ect;
        }

        //다음 event 설정하기
        public virtual void setNextEvent(EventTemplate _nextEvent)
        {
            nextEvent = _nextEvent;
        }


        public virtual void callbackFromEndCondition()
        {
            //go to next event; call process of event
            nextEvent.process();   
        }

        public virtual void process()
        {

        }

        
        

        

        



    }
}