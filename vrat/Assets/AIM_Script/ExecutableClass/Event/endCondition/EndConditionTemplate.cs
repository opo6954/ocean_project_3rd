using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
/*
 * EndCondition for Event
 * 하지만 Asset에서 쓰이는 trigger가 endcondition을 상속하기 때문에 trigger를 event의 endcondition 대신 사용할 수 있음
 * 즉 trigger가 성공하면 바로 event가 종료되고 다음 event로 가는 형식으로...
 * */
namespace vrat
{
    public delegate void CallbackForNextEvent();

    public class EndConditionTemplate : MonoBehaviour
    {
        protected event CallbackForNextEvent eventHandler;

        public virtual void addCallbackForEffect(CallbackForNextEvent myCallback)
        {
            eventHandler += myCallback;
        }

        public virtual void meetCondition()
        {
            eventHandler();
        }
    } 
}