using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    /*
     * asset trigger를 위한 xml template임
    */
    public class AssetTriggerXmlTemplate : XmlTemplate
    {

        public string assetTriggerType = "";
        public List<string> actionList = new List<string>();
        public List<string> paramList = new List<string>();

        public AssetTriggerXmlTemplate(string _name, string _type)
            : base(_name, _type)
        {
            ClassName = "AssetTriggerXmlTemplate";
        }


        

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            string propertyName = Name;
            string propertyType = Type;
            string propertyTriggerType = assetTriggerType;


            //root 설정
            XmlElement individualProperty = document.CreateElement("vrat.AssetTriggerXmlTemplate");
            parentElement.AppendChild(individualProperty);

            individualProperty.SetAttribute("name", propertyName);
            individualProperty.SetAttribute("type", propertyType);

            //trigger type 설정
            XmlElement triggerTypeRoot = document.CreateElement("TriggerType");
            individualProperty.AppendChild(triggerTypeRoot);
            triggerTypeRoot.SetAttribute("name", "TriggerType");
            triggerTypeRoot.SetAttribute("type", ParameterConversion.str2ParamType("string").ToString());
            triggerTypeRoot.SetAttribute("contents",assetTriggerType);

            //action list 설정
            XmlElement actionTypeList = document.CreateElement("ActionList");
            individualProperty.AppendChild(actionTypeList);
            
            ListOfXmlTemplate lxt = new ListOfXmlTemplate("ActionList", "ListOfXmlTemplate", 0);

            for (int i = 0; i < actionList.Count; i++)
            {
                PrimitiveXmlTemplate pxt = new PrimitiveXmlTemplate("actionList" + i.ToString(), actionList[i], "string");
                lxt.addList(pxt);
            }

            lxt.XmlSerialize(document, actionTypeList, false);

            //paramList 설정
            XmlElement paramTypeList = document.CreateElement("ParamList");
            individualProperty.AppendChild(paramTypeList);

            ListOfXmlTemplate lxt2 = new ListOfXmlTemplate("ParamList", "ListOfXmlTemplate", 0);

            for (int i = 0; i < paramList.Count; i++)
            {
                PrimitiveXmlTemplate pxt = new PrimitiveXmlTemplate("paramList" + i.ToString(), paramList[i], "string");
                lxt2.addList(pxt);
            }

            lxt2.XmlSerialize(document, paramTypeList, false);

            return individualProperty;
        }

        public void deserializeFromXml(XmlNode xmlChildNode)
        {
            XmlNodeList xnl = xmlChildNode.ChildNodes;
            Name = xmlChildNode.Name;

            for (int i = 0; i < xnl.Count; i++)
            {
                string name = xnl[i].Name;

                if (name == "TriggerType")
                {
                    assetTriggerType = xnl[i].Attributes["contents"].InnerText;
                }
                else if (name == "ActionList")
                {
                    for(int j=0; j<xnl[i].ChildNodes[0].ChildNodes.Count ;j++)
                    {
                        XmlNode childInner = xnl[i].ChildNodes[0].ChildNodes[j];
                        string nameInner = childInner.Attributes["name"].InnerText;
                        string typeInner = childInner.Attributes["type"].InnerText;
                        string contentsInner = childInner.Attributes["contents"].InnerText;

                        //action list에 집어넣기
                        actionList.Add(contentsInner);
                    }
                }
                else if (name == "ParamList")
                {
                    for (int j = 0; j < xnl[i].ChildNodes[0].ChildNodes.Count; j++)
                    {
                        XmlNode childInner = xnl[i].ChildNodes[0].ChildNodes[j];
                        string nameInner = childInner.Attributes["name"].InnerText;
                        string typeInner = childInner.Attributes["type"].InnerText;
                        string contentsInner = childInner.Attributes["contents"].InnerText;

                        //action list에 집어넣기
                        paramList.Add(contentsInner);
                    }
                }
            }
        }

        
    }
}