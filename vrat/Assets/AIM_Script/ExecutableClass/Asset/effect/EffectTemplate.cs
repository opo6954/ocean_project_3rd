using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

/*
 * Effect for asset
 * 
 * like water out effect when the extinguisher operation trigger is worked * 
 * */
namespace vrat
{
    
    public class EffectTemplate : MonoBehaviour
    {

        //effect와 연결될 asset 정보
        protected AssetTemplate myAsset;

        //trigger로 종료되는 effect인지 아니면 trigger로 시작되는 effect인지 알려주는 flags
        protected bool isBeforeEffect;

        protected List<string> parameterNames = new List<string>();

        



        //연결된 trigger가 이 callback을 set하면 trigger 동작이 수행되면 이 callback 함수를 부르게 된다
        //완료된 Trigger으로부터 불리는 callback함수

        public void setBeforeEffect(bool _isBeforeEffect)
        {
            isBeforeEffect = _isBeforeEffect;
        }


        public virtual void initialize()
        {
        }
        

        protected virtual void callbackFromTrigger()
        {
            //trigger--> effect의 경우 effect의 process()를 수행한다.
            if (isBeforeEffect == false)
            {
                process();
            }            
            //effect-->trigger의 경우 아무것도 안한다.....
        }

        protected virtual void process()
        {
        }
        
    }
}
