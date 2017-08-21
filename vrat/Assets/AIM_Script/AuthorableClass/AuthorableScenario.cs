using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
/*
 * Scenario를 serialize하는 클래스
 * 최종적으로 모든 게 필요함
    */
    public class AuthorableScenario : ObjectTemplate
    {

        //scenario에 포함된 asset list
        List<AuthorableAsset> assetList = new List<AuthorableAsset>();

        //scenario에 포함된 event list
        List<AuthorableEvent> eventList = new List<AuthorableEvent>();


        public override void initialize()
        {
            base.initialize();
            ObjectType = OBJTYPE.SCENARIO;

            initForScenario();
        }

        void initForScenario()
        {
            //피훈련자 역할 정보
            variableContainer.addParameter(new ListOfXmlTemplate("Trainee role info", "ListOfXmlTemplate",0));
            //맵 정보(아마 걍 맵의 prefab 이름만 저장하면 될 듯)
            variableContainer.addParameter(new PrimitiveXmlTemplate("Map info", "string"));

            //serialize와 deserialize 조금 다를 듯
        }

    }
}