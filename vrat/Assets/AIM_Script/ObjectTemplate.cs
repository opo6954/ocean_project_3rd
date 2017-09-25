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
 * 
 * 170920//. 구현상의 문제....
 * 지금 문제가 있는데 xml deserialize를 할 때 따로 형식 없이 구성됨
 * 
 * 즉 xml로부터 죄다 파라미터 등을 raw하게 읽어옴
 * 
 * 본래는 event의 경우 나름 정해진 포맷이 있기 때문에
 * 
 * 이 포맷에 맞춰서 그 포맷의 파라미터를 바꾸는 형식으로 짜야 함
 * 
 * 물론 asset의 경우 그 포맷이 애매하기 때문에 걍 raw하게 읽어도 되는데 event 등은 왠만하면 다 정해진 형식이라 파라미터 찾아서 변경시키는 게 더 안전함
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
        ASSET, ROOM, ENVIRONMENT, EVENT, TIMELINE, SCENARIO
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

        public void clearVariableAll()
        {
            variableList.Clear();
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

            Debug.Log("Cannot found parameter named " + _paramName);
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

        
        //root에 붙여서 serialize하는 함수 만들어야 함...
        

        protected XmlElement serializeCustomizedProperty2Xml(XmlDocument document, XmlElement parentElement)
        {
            XmlElement individualProperty = document.CreateElement("CustomizedProperty");
            parentElement.AppendChild(individualProperty);
                       

            for (int i = 0; i < variableContainer.getVariableCount(); i++)
            {
                variableContainer.getParameters(i).XmlSerialize(document, individualProperty,false);
            }

            return individualProperty;

        }

        

        public virtual void testSerialize(string xmlName)
        {

            XmlDocument document = new XmlDocument();

            serialize2Xml(document,null,true);

            document.Save(xmlName);

            Debug.Log("Save xml with " + xmlName);
        }

        public virtual void testDeserialize(string fileName)
        {
            Debug.Log("Deserialize for " + fileName.ToString());
            XmlDocument ppa = new XmlDocument();

            ppa.Load(fileName);

            XmlNodeList pq = ppa.ChildNodes;

            //variable Container 죄다 지워버리자


            deserializeFromXml(pq[0]);

        }
        //아직 copy 구현 안됨 continue...
        public void copyValueOnly(ObjectTemplate _target)
        {
            ObjectName = _target.ObjectName;
            ObjectType = _target.ObjectType;
            variableContainer.copyValueOnly(_target.variableContainer);
        }

        public virtual bool serialize2Xml(XmlDocument document, XmlElement rootElement, bool isRoot)
        {
            XmlElement root = document.CreateElement(ObjectType.ToString());
            root.SetAttribute("name", ObjectName);

            if (isRoot == true)
            {
                document.AppendChild(root);
            }
            else
            {
                rootElement.AppendChild(root);
            }

            serializeCustomizedProperty2Xml(document, root);

            return true;
        }
        
        public virtual bool deserializeFromXml(XmlNode rootNode)
        {
            variableContainer.clearVariableAll();

            

            if (rootNode.Name != ObjectType.ToString())
            {
                Debug.Log("not equal for rootNode Name and obj type");
                Debug.Log("Root name: " + rootNode.Name);
                Debug.Log("obj type name: " + objectType.ToString());
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

        protected virtual bool deserializeChildEachInside(XmlNodeList xnList)
        {
            foreach (XmlNode xn in xnList)
            {
                XmlAttributeCollection xac = xn.Attributes;

                //기본 primitiveXmlTemplate일 시
                if (xn.Name == "vrat.ListOfXmlTemplate")
                {
                    variableContainer.addParameter(deserializeChildEachForList(xn));
                }
                else
                {
                    XmlTemplate xt = createInstanceListElement(xn);

                    if (variableContainer.checkParameter(xt.Name) == true)
                    {
                        Debug.Log("Already exist for " + xt.Name);
                    }
                    
                    variableContainer.addParameter(xt);
                }
            }

            return true;
        }
        //list type의 child를 위한 deserializer --> 내부적으로 child에 대해서도 돌려야 함
        //일단 돌리기

        //list of list of xmlTemplate은 생각하지 말자......... 쓰지마!!
        protected XmlTemplate deserializeChildEachForList(XmlNode _node)
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

                if (_childNode.Name == "vrat.ListOfXmlTemplate")
                {
                    xtList.addList(deserializeChildEachForList(_childNode));
                }
                else
                {
                    XmlTemplate xe = createInstanceListElement(_childNode);
                    xtList.addList(xe);
                }
            }

            return xtList;
        }
        //list xml template의 경우 xmltemplate을 새로 instantiate해야 하는 데 이 부분에서 수행함
        //이미 있는 것에서 찾아서 해도 됨


        protected virtual XmlTemplate createInstanceListElement(XmlNode _node)
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

            else if(_node.Name == "vrat.TriggerPrimitivesTemplate")
            {
                TriggerPrimitivesTemplate tpt = new TriggerPrimitivesTemplate(name, type);

                tpt.deserializeFromXml(_node);
                return tpt;
            }
            else if (_node.Name == "vrat.ActionPrimitivesTemplate")
            {
                ActionPrimitivesTemplate apt = new ActionPrimitivesTemplate(name, type);
                apt.deserializeFromXml(_node);

                return apt;
            }
            else if (_node.Name == "vrat.InstPrimitivesTemplate")
            {
                InstPrimitivesTemplate ipt = new InstPrimitivesTemplate(name, type);
                ipt.deserializeFromXml(_node);

                return ipt;
            }
            else if (_node.Name == "vrat.AssetTriggerXmlTemplate")
            {
                AssetTriggerXmlTemplate atxt = new AssetTriggerXmlTemplate(name, type);
                atxt.deserializeFromXml(_node);

                return atxt;
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
    }
}