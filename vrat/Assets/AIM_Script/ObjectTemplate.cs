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
 * 
 * 
 * 
*/

namespace vrat
{
    //object의 종류임, 일단 asset, vent, scenario로 정리
    public enum OBJTYPE
    {
        ASSET, EVENT, SCENARIO
    }
    
    public class VariableContainer
    {
        private List<XmlTemplate> variableList = new List<XmlTemplate>();

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

  
     
    public class ObjectTemplate : MonoBehaviour
    {
        //object의 이름
        private string objectName;

        protected VariableContainer variableContainer = new VariableContainer();

        private OBJTYPE objectType;

        protected string ObjectName
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

        protected OBJTYPE ObjectType
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

        
        protected virtual bool serialize2Xml(XmlDocument document)
        {
            XmlElement xe = serializeRootElement2Xml(document);
            serializeCustomizedProperty2Xml(document, xe);

            return true;
        }
        //xml 파일을 읽어서 
        //일단 deserialize은 좀 있다가 하고 serailize to Xml을 먼저 하자
        protected virtual bool deserializeFromXml(XmlNodeList childNodeList)
        {
            string type="";

            XmlNode topNode = childNodeList[0];

            string name = topNode.Attributes["name"].InnerText;

            XmlNode customizedProperty = topNode.ChildNodes[0];

            type = topNode.Name;

            Debug.Log("name: " + name);
            Debug.Log("type: " + type);

            Dictionary<string, string> tmp = new Dictionary<string,string>();

            foreach (XmlNode singleNode in customizedProperty)
            {
                Debug.Log("child: " + singleNode.Name);

                XmlAttributeCollection xac = singleNode.Attributes;

                string val = xac["contents"].InnerText;

                tmp.Add(singleNode.Name, val);
            }

            string[] keySet = tmp.Keys.ToArray();
             

            for (int i = 0; i < keySet.Length; i++)
            {
                Debug.Log(keySet[i] + "'s value is " + tmp[keySet[i]]);
            }

            
            return true;
        }

        




    }
}