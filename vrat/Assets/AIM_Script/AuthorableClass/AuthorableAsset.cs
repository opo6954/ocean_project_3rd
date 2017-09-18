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
        

        //일단 모든 asset에 공통된 얘들을 불러오자
        /*
        protected override bool deserializeFromXml(XmlNodeList childNodeList)
        {
            XmlNode rootNode = childNodeList[0];

            //ASSET이 root에 있을 경우 그대로 진행
            if(rootNode.Name != ObjectType.ToString())
            {
                return false;
            }

            XmlAttributeCollection xac1 = rootNode.Attributes;


            //asset의 이름 불러오기
            ObjectName = rootNode.Attributes[0].InnerText;
            

            XmlNode customizedProperty = rootNode.ChildNodes[0];

            //customizedProperty 값 확인
            if(customizedProperty.Name != "CustomizedProperty")
            { 
                return false;
            }

            XmlNodeList propertyList = customizedProperty.ChildNodes;

            //이 부분에서 primitiveXmlTemplate 등이 등장함

            
            foreach(XmlNode xn in propertyList)
            {
                string className = xn.Name;
                string name="";
                string type="";
                string contents="";

                XmlAttributeCollection xac = xn.Attributes;

                if (className == "vrat.PrimitiveXmlTemplate")
                {
                    name = xac["name"].InnerText;
                    type = xac["type"].InnerText;
                    contents = xac["contents"].InnerText;


                    if (variableContainer.checkParameter(name) == true)
                    {
                        XmlTemplate xt = variableContainer.getParameters(name);
                        xt.ClassName = "PrimitiveXmlTemplate";

                        (xt as PrimitiveXmlTemplate).setparameter(contents);
                    }
                }
                else if (className == "vrat.LocationXmlTemplate")
                {
                    name = xac["name"].InnerText;
                    type = xac["type"].InnerText;


                    //location에 맞는 거 하기

                    if (variableContainer.checkParameter(name) == true)
                    {
                        XmlTemplate xt = variableContainer.getParameters(name);
                        xt.ClassName = "LocationXmlTemplate";

                        Vector3 pos = new Vector3();
                        Vector3 rot = new Vector3();


                        foreach (XmlNode xnInner in xn.ChildNodes)
                        {

                            XmlAttributeCollection xacInner = xnInner.Attributes;

                            
                            if (xnInner.Name == "Position")
                            {
                                pos.x = float.Parse(xnInner["x"].InnerText);
                                pos.y = float.Parse(xnInner["y"].InnerText);
                                pos.z = float.Parse(xnInner["z"].InnerText);
;
                            }
                            if (xnInner.Name == "Rotation")
                            {
                                rot.x = float.Parse(xnInner["x"].InnerText);
                                rot.y = float.Parse(xnInner["y"].InnerText);
                                rot.z = float.Parse(xnInner["z"].InnerText);
                                
                            }
                        }
                        (xt as LocationXmlTemplate).setParameter(new Location(pos, rot));
                    }
                }
                else if (className == "vrat.ListOfXmlTemplate")
                {
                    name = xac["name"].InnerText;
                    type = xac["type"].InnerText;
                    string selectedIdx = xac["idx"].InnerText;




                    if (variableContainer.checkParameter(name) == true)
                    {
                        ListOfXmlTemplate xt = variableContainer.getParameters(name) as ListOfXmlTemplate;
                        xt.ClassName = "ListOfXmlTemplate";
                        xt.setIdx(int.Parse(selectedIdx));

                        foreach (XmlNode xnInner in xn.ChildNodes)
                        {
                            XmlAttributeCollection xacInner = xnInner.Attributes;

                            string classNameInner = xnInner.Name;
                            string nameInner = xacInner["name"].InnerText;
                            string typeInner = xacInner["type"].InnerText;

                            XmlTemplate xmlTemplate = System.Activator.CreateInstance(System.Type.GetType(classNameInner), new object[]{nameInner, typeInner}) as XmlTemplate;

                            xt.addList(xmlTemplate);
                        }
                    }
                }
            }
         
            return true;
        }
        */


        
    }
}
