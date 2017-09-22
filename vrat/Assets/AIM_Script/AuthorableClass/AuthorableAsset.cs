using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.IO;


namespace vrat
{

    /*
     * Asset serialize를 위한 class로써 asset의 모든 정보를 저장할 수 있음
     * */
    public class AuthorableAsset : ObjectTemplate
    {
        //xml 형식으로 serialize하기
        //모든 obj별로 override하면 됨


         


        public override void initialize()
        {
            base.initialize();

            ObjectType = OBJTYPE.ASSET;

            /*
             * common asset에 대한 trigger임
             * */
        }
         
        void Start()
        {
        }


        //일단 임시로 각 asset별로의 example xml serialize를 제공해보자
        public void exampleFireSerialize()
        {
            ObjectName = "fire";

            variableContainer.addParameter(new PrimitiveXmlTemplate("IsGraspable","false", "bool"));
            variableContainer.addParameter(new LocationXmlTemplate("Location", "location", new Location(new Vector3(10, 20, 30), new Vector3(50, 70, 210))));
            variableContainer.addParameter(new VariableXmlTemplate("Life", "float"));//explicit된 형태의 variable임
        }
        public void exampleExtinguisherSerialize()
        {
            ObjectName = "extinguisher";

            variableContainer.addParameter(new PrimitiveXmlTemplate("IsGraspable","true","bool"));
            variableContainer.addParameter(new LocationXmlTemplate("Location", "location", new Location(new Vector3(10, 20, 30), new Vector3(50, 70, 210))));
            ListOfXmlTemplate holdGesture = new ListOfXmlTemplate("HoldGestureType", "ListOfXmlTemplate", 0);
            ListOfXmlTemplate extinguishGesture = new ListOfXmlTemplate("ExtinguishGestureType", "ListOfXmlTemplate", 0);

            holdGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownZ", "z", "string"));
            holdGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownX", "x", "string"));
            holdGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownC", "c", "string"));

            extinguishGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownZ", "z", "string"));
            extinguishGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownX", "x", "string"));
            

            variableContainer.addParameter(holdGesture);
            variableContainer.addParameter(extinguishGesture);

        }
        public void examplePhoneSerialize()
        {
            ObjectName = "phone";

            variableContainer.addParameter(new PrimitiveXmlTemplate("IsGraspable","false", "bool"));
            variableContainer.addParameter(new LocationXmlTemplate("Location", "location", new Location(new Vector3(10, 20, 30), new Vector3(50, 70, 210))));
            ListOfXmlTemplate reportGesture = new ListOfXmlTemplate("ReportGestureType", "ListOfXmlTemplate", 0);

            
            reportGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownX", "x", "string"));
            reportGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownC", "c", "string"));

            variableContainer.addParameter(reportGesture);


        }
    }
}
