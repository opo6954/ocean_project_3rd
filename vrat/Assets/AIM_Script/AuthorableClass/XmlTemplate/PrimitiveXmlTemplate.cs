using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    /*
     * 기본 값들에 대한 xml tempalte(int, string, float 등)
     * */
    public class PrimitiveXmlTemplate : XmlTemplate
    {
        string variable="";
         

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            string propertyName = Name;
            string propertyType = Type;
            string propertyValue = variable;
            
            XmlElement individualProperty = document.CreateElement("vrat.PrimitiveXmlTemplate");
            parentElement.AppendChild(individualProperty);

            individualProperty.SetAttribute("name", propertyName);
            individualProperty.SetAttribute("type", propertyType);
            individualProperty.SetAttribute("contents", propertyValue);
             
             
            return individualProperty;
        }

        public string getVariable()
        {
            return variable;
        }

        public void setparameter(string _variable)
        {
            variable = _variable;

        }
         
        public PrimitiveXmlTemplate(string _name, string _type) : base(_name, _type)
        {
            ClassName = "PrimitiveXmlTemplate";
        }

        public PrimitiveXmlTemplate(string _name, string _variable, string _type) : base(_name, _type)
        {
            variable = _variable;
            ClassName = "PrimitiveXmlTemplate";
        }

    }
}