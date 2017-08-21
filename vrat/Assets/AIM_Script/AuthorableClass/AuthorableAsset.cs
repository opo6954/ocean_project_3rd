using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.IO;


namespace vrat
{


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

            initForAsset();
        }

        void Start()
        {
        }


        void initForAsset()
        {

            variableContainer.addParameter(new PrimitiveXmlTemplate("RoleInfo1","string"));
            variableContainer.addParameter(new PrimitiveXmlTemplate("IsInventory","bool"));
            variableContainer.addParameter(new LocationXmlTemplate("Location", "location", new Location(new Vector3(10, 20, 30), new Vector3(50, 70, 210))));


            ListOfXmlTemplate tl = new ListOfXmlTemplate("TriggerList", "ListOfXmlTemplate",0);
            ListOfXmlTemplate be = new ListOfXmlTemplate("BeforeEffectList", "ListofXmlTemplate",0);
            ListOfXmlTemplate ae = new ListOfXmlTemplate("AfterEffectList", "ListofXmlTemplate",0);

            variableContainer.addParameter(tl);
            variableContainer.addParameter(be);
            variableContainer.addParameter(ae);
        }



        public void exampleSerialize()
        {
            ObjectName = "over";
            
            (variableContainer.getParameters("RoleInfo1") as PrimitiveXmlTemplate).setparameter("Sailor");
            (variableContainer.getParameters("IsInventory") as PrimitiveXmlTemplate).setparameter(false.ToString());


            ListOfXmlTemplate tl = variableContainer.getParameters("TriggerList") as ListOfXmlTemplate;
            ListOfXmlTemplate be = variableContainer.getParameters("BeforeEffectList") as ListOfXmlTemplate;
            ListOfXmlTemplate ae = variableContainer.getParameters("AfterEffectList") as ListOfXmlTemplate;

            tl.setIdx(1);
            be.setIdx(0);
            ae.setIdx(2);
            

            be.addList(new ClassNameXmlTemplate("myEffect1", "LockScreenEffect"));
            be.addList(new ClassNameXmlTemplate("myEffect2", "LockScreenEffect"));

            ae.addList(new ClassNameXmlTemplate("myEffect1", "LockScreenEffect"));
            ae.addList(new ClassNameXmlTemplate("myEffect2", "LockScreenEffect"));
            ae.addList(new ClassNameXmlTemplate("myEffect3", "LockScreenEffect"));

            tl.addList(new ClassNameXmlTemplate("myTrigger1", "HoldTriggerTemplate"));
            tl.addList(new ClassNameXmlTemplate("myTrigger2", "ApproachTriggerTemplate"));
            tl.addList(new ClassNameXmlTemplate("myTrigger3", "ActTriggerTemplate"));
        }

        public void testSerialize(string xmlName)
        {
            XmlDocument document = new XmlDocument();

            serialize2Xml(document);

            document.Save(xmlName);

            Debug.Log("Save xml with " + xmlName);
        }

        public void testDeserialize(string fileName )
        {
            XmlDocument ppa = new XmlDocument();

            ppa.Load(fileName);

            XmlNodeList pq = ppa.ChildNodes;

            

            deserializeFromXml(pq);

        }


        //일단 모든 asset에 공통된 얘들을 불러오자

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

            /*
             * FOR DEBUG
            Debug.Log(variableContainer.getNumberOfParameters());

            for(int i=0; i<variableContainer.getNumberOfParameters(); i++)
            {
                Debug.Log(variableContainer.getParameters(i).Name);
            }
             * */

            return true;
        }
        
    }
}
