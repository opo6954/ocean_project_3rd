using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Trigger for asset
 * Containing active trigger like gesture or passive trigger like approach
 * 
 * Component of trigger:
 * 1. Trigger supervisor: Supervise input whether the trigger is worked or not
 * 2. 
  * */

namespace vrat
{
    public class TriggerTemplate : EndConditionTemplate
    {
         
        //trigger와 연결될 asset 정보
        protected AssetTemplate myAsset;

        //초기화 함수
        public virtual void initialize()
        {
        }

        //Set the asset of this trigger
        public virtual bool setAsset(AssetTemplate _asset)
        {
            myAsset = _asset;
            return true;
        }

        //trigger supervisor for checking out trigger
        public virtual bool triggerSupervisor()
        {
            return true;
        }

        //이 부분이 Trigger check하는 동안 계속 불리게 됨(아마 Update에 넣어야 할텐데
        //일단 authoring stage과 training stage를 나눠야 하므로 이렇게 하고 추후에 training state를 하면 asset단에서 update에서 돌아가도록 하면 됩니당)
        //만일 true일시(trigger 동작시) trigger()를 불러서 연결되어 있는 effectTemplate의 callback함수를 부르게 된다

        //while 문은 바깥에서 수행하기
        public void OnTrigger()
        {
            if (triggerSupervisor() == true)
            {
                meetCondition();
            }
        }

    }
}