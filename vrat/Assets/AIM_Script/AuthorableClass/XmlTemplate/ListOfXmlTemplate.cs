﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

namespace vrat
{
    /*
     * list 형태의 data를 다루는 xmlTemplate임
     * */
    public class ListOfXmlTemplate : XmlTemplate
    {
        List<XmlTemplate> xmlTemplateList = new List<XmlTemplate>();


        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement)
        {
            string propertyName = Name;
            string propertyType = Type;

            XmlElement individualProperty = document.CreateElement(this.GetType().ToString());
            
            individualProperty.SetAttribute("name", propertyName);
            individualProperty.SetAttribute("type", propertyType);

            parentElement.AppendChild(individualProperty);

            foreach(XmlTemplate q in xmlTemplateList)
            {
                q.XmlSerialize(document, individualProperty);
            }

            return individualProperty;
        }

        public ListOfXmlTemplate(string _name, string _type) : base(_name, _type)
        {

        }

        public bool addList(XmlTemplate _xt)
        {
            foreach (XmlTemplate xt in xmlTemplateList)
            {
                if (xt.Name == _xt.Name)
                    return false;
            }
            
            xmlTemplateList.Add(_xt);
            return true;
            
        }

        public XmlTemplate getXmlTemplate(string name)
        {
            foreach (XmlTemplate xt in xmlTemplateList)
            {
                if (xt.Name == name)
                    return xt;
            }

            return new NullXmlTemplate();
        }
        
    }
}