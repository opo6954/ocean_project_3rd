using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    public class ClassNameXmlTemplate : XmlTemplate
    {

         public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            string propertyName = Name;
            string propertyType = Type;

            XmlElement individualProperty = document.CreateElement(this.GetType().ToString());
            parentElement.AppendChild(individualProperty);

            individualProperty.SetAttribute("name", propertyName);
            individualProperty.SetAttribute("type", propertyType);

            return individualProperty;
        }



        public ClassNameXmlTemplate(string _name, string _type) : base(_name, _type)
        {
            
        }        
    }
}
