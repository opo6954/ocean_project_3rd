using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Asset template
 * Only asset cannot do training... asset collaborated event can be possible in training stage
 * 
 * 
 * 
 * */
namespace vrat
{
    public class AssetTemplate : ObjectTemplate
    {


        //attached role information
        protected string roleInfo;


        //attached event template
        protected EventTemplate eventTemplate;


        //Collider for boundingBox
        protected Collider boundingBox;

        //Is Inventory-able
        protected bool isInventory;

        //Possible trigger list
        protected List<TriggerTemplate> triggerList=new List<TriggerTemplate>();

        //Possible after effect list
        protected List<EffectTemplate> afterEffectList=new List<EffectTemplate>();

        //Possible before effect list
        protected List<EffectTemplate> beforeEffectList=new List<EffectTemplate>();


        //Location info. of Asset;(from in-situ authoring)
        protected Transform location;

         

        public bool addParameter(string _parameterName, object _parameterValue)
        {
            //parameterName에 있는 지 확인하고 없으면 false
            if (parameterName.Contains(_parameterName) == false)
            {
                return false;
            }
            //parameter dictionary에 이미 존재하는 경우 false
            if (parameters.ContainsKey(_parameterName) == true || parameters.ContainsValue(_parameterValue) == true)
            {
                return false;
            }

            parameters.Add(_parameterName, _parameterValue);

            return true;
        }


        //get the parameter in asset...
        public T getParameter<T>(string _parameterName)
        {
            //No parametername found...
            if (parameterName.Contains(_parameterName) == false)
            {
                return default(T);
            }

            if (parameters.ContainsKey(_parameterName) == false)
            {
                return default(T);
            }

            return (T)parameters[_parameterName];
        }



        public bool setRoleInfo(string roleName)
        {
            if (RoleInfoTemplate.isRoleNameExist(roleName) == true)
            {
                roleInfo = roleName;
                return true;
            }
            return false;
        }

        //event 설정 함수
        public void setEvent(EventTemplate _eventTemplate)
        {
            eventTemplate = _eventTemplate;
        }
        //trigger 추가 함수
        public bool addTrigger(TriggerTemplate _trigger)
        {
            if (triggerList.Contains(_trigger) == true)
            {
                return false;
            }

            triggerList.Add(_trigger);
            return true;
        }
        //before effect 추가 함수
        public bool addBeforeEffect(EffectTemplate _effect)
        {
            if (beforeEffectList.Contains(_effect) == true)
            {
                return false;
            }

            beforeEffectList.Add(_effect);
            return true;
        }
        //after effect 추가 함수
        public bool addAfterEffect(EffectTemplate _effect)
        {
            if (afterEffectList.Contains(_effect) == true)
            {
                return false;
            }
            afterEffectList.Add(_effect);
            return true;
        }


    }
}
/*
 * 소화기 Asset
3D model: 전체 소화기 mesh(수정 불가)
Bounding box: 전체 소화기 bounding box(수정 불가)
Effect list:(Timeline authoring시 effect list와 trigger list에서 trigger와 effect 선택 가능)
Highlight
Informing
소화기 들기
노즐 위로 이동
물 분사
Trigger list:(Timeline authoring시 effect list와 trigger list에서 trigger와 effect 선택 가능)
Asset의 기본 trigger
(Asset 발견, asset이 시야에 들어올 시 등)
밑의 파트 asset에서의 trigger 생성 가능(Asset list를 만들 시 asset list에서 각 asset의 trigger를 어떻게 합칠 지 선택 가능(sequential or parallel, AND or OR)
New trigger:
전체 분사 trigger: 먼저 안전핀의 trigger  손잡이의  trigger AND trigger  바디의 trigger를 설정
전체 순서대로 동작 trigger: 안전핀의 순서대로동작 trigger  손잡이의 순서대로동작 trigger 노즐의 순서대로 동작 trigger  바디의 순서대로동작 trigger
Location:
In-situ에서 변경 가능
List of asset: 안전핀, 손잡이, 노즐, 바디

 * */
