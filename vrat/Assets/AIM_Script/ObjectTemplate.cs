using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEngine;

/*
 * 저작 가능한 authoring template 있음
 * parameter들을 dictionary 형태로 저장하고 있고 필요한 parameter의 이름 역시 저장하고 있음
 * 
 * object template이 authoring에서도 쓰이고 training에서도 공통으로 쓰이는 structure를 만들자
 * 
 * 일단 xml serializer가 필요함
 * 
 * variable의 종류
 * 
 * 
 * 
 * 
 * 
*/

namespace vrat
{
    //object의 종류임, 일단 asset, vent, scenario로 정리
    public enum OBJTYPE
    {
        ASSET, ROOM, ENVIRONMENT, EVENT, TRANSITION, SCENARIO
    }
    
    public class VariableContainer
    {
        private List<XmlTemplate> variableList = new List<XmlTemplate>();

        public void copyValueOnly(VariableContainer _target)
        {
            foreach (XmlTemplate xt in _target.variableList)
            {
                variableList.Add(xt);
            }
        }

        public int getVariableCount()
        {
            return variableList.Count;
        }

        //check if there exist paramName in variableList
        public bool checkParameter(string paramName)
        {
            foreach (XmlTemplate v in variableList)
            {
                if (v.Name == paramName)
                    return true;
            }

            return false;
        }

        //Add parameter with paramName and paramValue
        public bool addParameter(XmlTemplate _paramValue)
        {
            foreach (XmlTemplate v in variableList)
            {
                if (v.Name == _paramValue.Name)
                {
                    return false;
                }
            }

            variableList.Add(_paramValue);

            return true;
        }

        //Get the parameter with paramName

        public XmlTemplate getParameters(int idx)
        {
            if (variableList.Count > idx)
            {
                return variableList[idx];
            }
            return new NullXmlTemplate();
        }

        public XmlTemplate getParameters(string _paramName)
        {
            foreach (XmlTemplate v in variableList)
            {
                if (v.Name == _paramName)
                {
                    return v;
                }
            }

            return new NullXmlTemplate();
        }

        public int getNumberOfParameters()
        {
            return variableList.Count;
        }

    } 

  
     
    public class ObjectTemplate
    {
        //object의 이름
        private string objectName;

        public VariableContainer variableContainer = new VariableContainer();

        private OBJTYPE objectType;

        public string ObjectName
        {
            get
            {
                return objectName;
            }
            set
            {
                objectName = value;
            }
        }

        public OBJTYPE ObjectType
        {
            get
            {
                return objectType;
            }
            set
            {
                objectType = value;
            }
        }




        //초기화
        public virtual void initialize()
        {

        }

       


        //xml 파일로 저장

        //일단 가장 윗레벳에서 serialize2Xml 수행하기
        //파라미터로 받은 xml document에 관련 정보를 쓰면 됩니다

        

        

        protected XmlElement serializeCustomizedProperty2Xml(XmlDocument document, XmlElement parentElement)
        {
            XmlElement individualProperty = document.CreateElement("CustomizedProperty");
            parentElement.AppendChild(individualProperty);
                       

            for (int i = 0; i < variableContainer.getVariableCount(); i++)
            {
                variableContainer.getParameters(i).XmlSerialize(document, individualProperty);
            }

            return individualProperty;

        }

        protected XmlElement serializeRootElement2Xml(XmlDocument document)
        {
            XmlElement rootElement = document.CreateElement(ObjectType.ToString());

            rootElement.SetAttribute("name", ObjectName);

            document.AppendChild(rootElement);


            return rootElement;
        }

        public virtual void testSerialize(string xmlName)
        {
            XmlDocument document = new XmlDocument();

            serialize2Xml(document);

            document.Save(xmlName);

            Debug.Log("Save xml with " + xmlName);
        }

        public virtual void testDeserialize(string fileName)
        {
            XmlDocument ppa = new XmlDocument();

            ppa.Load(fileName);

            XmlNodeList pq = ppa.ChildNodes;



            deserializeFromXml(pq);

        }

        public void copyValueOnly(ObjectTemplate _target)
        {
            ObjectName = _target.ObjectName;
            ObjectType = _target.ObjectType;
            variableContainer.copyValueOnly(_target.variableContainer);
            
            
          
        }


        protected virtual bool serialize2Xml(XmlDocument document)
        {
            XmlElement xe = serializeRootElement2Xml(document);
            serializeCustomizedProperty2Xml(document, xe);

            return true;
        }
        
        protected virtual bool deserializeFromXml(XmlNodeList childNodeList)
        {
            
            string type = "";

            XmlNode rootNode = childNodeList[0];

            if (rootNode.Name != ObjectType.ToString())
            {
                return false;
            }

            XmlAttributeCollection xac1 = rootNode.Attributes;

            ObjectName = rootNode.Attributes[0].InnerText;

            

            XmlNode childProperty = rootNode.ChildNodes[0];

            if (childProperty.Name != "CustomizedProperty")
            {
                return false;
            }
            
            XmlNodeList propertyList = childProperty.ChildNodes;

            

            deserializeChildEachInside(propertyList);

            return true;
        }

        private bool deserializeChildEachInside(XmlNodeList xnList)
        {
            foreach (XmlNode xn in xnList)
            {
                
                XmlAttributeCollection xac = xn.Attributes;

                //기본 primitiveXmlTemplate일 시
                if (xn.Name == "vrat.ListOfXmlTemplate")
                {
                    deserializeChildEachForList(xn);
                }
                else
                {
                    variableContainer.addParameter(createInstanceListElement(xn));
                }
            }

            return true;
        }
        //list type의 child를 위한 deserializer --> 내부적으로 child에 대해서도 돌려야 함
        //일단 돌리기

        //list of list of xmlTemplate은 생각하지 말자......... 쓰지마!!
        private bool deserializeChildEachForList(XmlNode _node)
        {
            XmlAttributeCollection xac = _node.Attributes;

            string name = xac["name"].InnerText;
            string type = xac["type"].InnerText;
            string selectedIdx = xac["idx"].InnerText;

            ListOfXmlTemplate xtList = new ListOfXmlTemplate(name, type, int.Parse(selectedIdx));
            xtList.ClassName = "ListOfXmlTemplate";
            xtList.setIdx(int.Parse(selectedIdx));

            foreach(XmlNode _childNode in _node.ChildNodes)
            {
                xtList.addList(createInstanceListElement(_childNode));
            }

            variableContainer.addParameter(xtList);


            return true;
        }
        //list xml template의 경우 xmltemplate을 새로 instantiate해야 하는 데 이 부분에서 수행함
        private XmlTemplate createInstanceListElement(XmlNode _node)
        {
            XmlAttributeCollection xac = _node.Attributes;

            string name = xac["name"].InnerText;
            string type = xac["type"].InnerText;

            if (_node.Name == "vrat.PrimitiveXmlTemplate")
            {

                string contents = xac["contents"].InnerText;

                PrimitiveXmlTemplate pxt = new PrimitiveXmlTemplate(name, contents, type);

                return pxt;

            }
            else if (_node.Name == "vrat.LocationXmlTemplate")
            {
                LocationXmlTemplate lxt = new LocationXmlTemplate(name, type, loadLocationFromXmlNode(_node));


                return lxt;
            }
            else if (_node.Name == "vrat.ClassNameXmlTemplate")
            {
                ClassNameXmlTemplate cxl = new ClassNameXmlTemplate(name, type);
                return cxl;
            }

            else if (_node.Name == "vrat.VariableXmlTemplate")
            {
                VariableXmlTemplate vxt = new VariableXmlTemplate(name, type);
                return vxt;
            }

            else
            {
                Debug.Log("No deserializer found for " + _node.Name);
                return new NullXmlTemplate();
            }
        }
        

        //xmlNode로부터 location정보를 불러온다(position, rotation)
        private Location loadLocationFromXmlNode(XmlNode _node)
        {
            Vector3 pos = new Vector3();
            Vector3 rot = new Vector3();

            foreach (XmlNode xnInner in _node.ChildNodes)
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

            Location l = new Location(pos, rot);

            return l;
        }

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
    */






    }
}