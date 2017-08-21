using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * Event를 serialize하는 클래스
     * */
    public class AuthorableEvent : ObjectTemplate
    {
        public override void initialize()
        {
            base.initialize();

            ObjectType = OBJTYPE.EVENT;

            initForEvent();

        }

        void initForEvent()
        {
            //연결된 asset의 이름 설정
            variableContainer.addParameter(new PrimitiveXmlTemplate("Attached Asset", "string"));
            
            //Transition 종류 설정
            variableContainer.addParameter(new ClassNameXmlTemplate("Transition", "TransitionTemplate"));

            //이전 event의 이름
            variableContainer.addParameter(new PrimitiveXmlTemplate("Before Event", "string"));

            //다음 event의 이름
            variableContainer.addParameter(new PrimitiveXmlTemplate("Next Event", "string"));

            //본 event를 소유하고 있는 역할
            variableContainer.addParameter(new PrimitiveXmlTemplate("Trainee", "string"));
        }

    }
}