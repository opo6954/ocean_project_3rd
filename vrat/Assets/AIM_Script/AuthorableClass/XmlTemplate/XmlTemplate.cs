using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    /* 
     * Xml serialize 및 deserialize하는 요소
     * */
    public abstract class XmlTemplate
    {
        //XmlTemplate의 이름(사용자 혹은 저작도구에서 따로 붙이는 이름)
        string name;        
        

        //XmlTemplate의 형식(class 이름이 들어감 나중에 deserialize할 때 class instantiate할 시 필요함
        string type;

        string className;
          
        protected bool isVariableSet;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
         
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public string ClassName
        {
            get
            {
                return className;
            }
            set
            {
                className = value;
            }
        }

        protected XmlTemplate(string _name, string _type)
        {
            Name = _name;
            Type = _type;
        }

        public abstract XmlElement XmlSerialize(XmlDocument document, XmlElement parentElement, bool isRoot);
    }
}
