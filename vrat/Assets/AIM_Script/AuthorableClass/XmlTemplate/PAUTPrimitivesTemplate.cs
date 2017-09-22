using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    //parameter의 종류인데 일단 이렇게 정의만 해놓자
    //아직 적용은 되지 않은 상태임


    public enum PARAMTYPE
    {
        STRING, INT, FLOAT, BOOL, LOCATION, CHOICE
    }

    public class ParameterConversion
    {
        //parameter 종류
        PARAMTYPE parameterType;

        //파라미터 값
        string paramValue;

        //choice의 경우의 답안지 역할을 함
        List<string> additionInfo = new List<string>();

        //파라미터 이름
        string paramName;

        public static PARAMTYPE str2ParamType(string str)
        {
            if (str == "STRING" || str == "string")
                return PARAMTYPE.STRING;
            else if (str == "INT" || str == "int")
                return PARAMTYPE.INT;
            else if (str == "FLOAT" || str == "float")
                return PARAMTYPE.FLOAT;
            else if (str == "BOOL" || str == "bool")
                return PARAMTYPE.BOOL;
            else if(str =="CHOICE" || str == "CHOICE")
                return PARAMTYPE.CHOICE;
            else
                return PARAMTYPE.LOCATION;
        }

        public ParameterConversion()
        {
        }

        public void setParameterType(PARAMTYPE _parameterType)
        {
            parameterType = _parameterType;
            if (_parameterType == PARAMTYPE.CHOICE)
            {
                paramValue = "0";
            }
        }

        void clear()
        {
            additionInfo.Clear();
            additionInfo = new List<string>();
        }

        //파라미터의 이름 설정
        public void setParameterName(string _name)
        {
            paramName = _name;
        }

        public void setParameter(string _value)
        {
            paramValue = _value;
        }


        //data type 가져오기
        public PARAMTYPE getParameterType()
        {
            return parameterType;
        }

        public string getParameter()
        {
            return paramValue;
        }

        public string getParamName()
        {
            return paramName;
        }

        public void setAdditionalInfo(string _addInfo)
        {
            additionInfo.Add(_addInfo);
        }

        public void setAdditionalInfo(string[] _addInfo)
        {
            for (int i = 0; i < _addInfo.Length; i++)
            {
                additionInfo.Add(_addInfo[i]);
            }
        }

        public string[] getAdditionalInfo()
        {
            string[] p = new string[additionInfo.Count];
            additionInfo.CopyTo(p);

            return p;
        }

        public static void coypValue(ParameterConversion origin, ref ParameterConversion dest)
        {
            //type 복사
            dest.setParameterType(origin.getParameterType());

            //parameter 이름 복사
            dest.setParameterName(origin.getParamName());

            //parameter 값 복사
            dest.setParameter(origin.getParameter());

            //choice일 경우 additional info 복사
            if(origin.getParameterType() == PARAMTYPE.CHOICE)
            {
                string [] strType = origin.getAdditionalInfo();

                dest.setAdditionalInfo(strType);
            }
        }

       

        public void printAllParam()
        {
            Debug.Log("Parameter name: " + paramName);
            Debug.Log("Parameter value: " + paramValue);
            Debug.Log("Parameter type: " + parameterType.ToString());
            
            for (int i = 0; i < additionInfo.Count; i++)
            {
                Debug.Log("Additional Info " + i.ToString() + " : " + additionInfo[i]);
            }
        }
    }

    public class PAUTPrimitivesTemplate : XmlTemplate
    {
        //얘를 가지고 있는 asset 이름
        protected string ownedAssetName = "";

        //이 녀석을 붙일 수 있는 asset 타입임
        protected string attachedAssetType = "";

        //파라미터 값 및 타입 list
        protected List<ParameterConversion> paramValue = new List<ParameterConversion>();

        //variable type 이름 list
        protected List<ParameterConversion> variableType = new List<ParameterConversion>();

        public PAUTPrimitivesTemplate(string _name, string _type) : base(_name, _type)
        {

        }

        public void clearParamValueAll()
        {
            paramValue.Clear();
            variableType.Clear();
        }

        public static void CopyPAUT(PAUTPrimitivesTemplate origin, ref PAUTPrimitivesTemplate dest)
        {
            
            dest.Name = origin.Name;

            //붙여진 asset 복사
            dest.ownedAssetName = origin.ownedAssetName;

            //가능한 asset type 복사
            dest.attachedAssetType = origin.attachedAssetType;

            //parameter 복사
            for (int i = 0; i < origin.paramValue.Count; i++)
            {
                ParameterConversion pc = new ParameterConversion();
                ParameterConversion.coypValue(origin.paramValue[i], ref pc);


                dest.paramValue.Add(pc);                
            }

            //variable 복사
            for (int i = 0; i < origin.variableType.Count; i++)
            {
                ParameterConversion pc = new ParameterConversion();
                ParameterConversion.coypValue(origin.variableType[i], ref pc);
                dest.variableType.Add(pc);
            }
        }
        

         
        //모든 파라미터 이름만 가져오기
        public string[] getAllParameterName()
        {
            string[] parametersName = new string[paramValue.Count];
            for(int i=0; i<paramValue.Count; i++)
            {
                parametersName[i] = paramValue[i].getParamName();
            }

            return parametersName;
        }

        //파라미터 이름이 주어질 시 parameter 값 가져오기
        public string getParameterValue(string _paramName, ref string _type)
        {
            foreach (ParameterConversion pc in paramValue)
            {
                if (pc.getParamName() == _paramName)
                {
                    _type = pc.getParameterType().ToString();
                    return pc.getParameter();
                }
            }

            Debug.Log("No parameter named " + _paramName + " found...");

            return "";
        }

        public string getVariableValue(string _variableName)
        {
            foreach (ParameterConversion pc in variableType)
            {
                if (pc.getParamName() == _variableName)
                {
                    return pc.getParamName();
                }
            }

            Debug.Log("No variable named " + _variableName + " found...");
            return "";

        }

        public void setParameterValue(string _paramName, string _value)
        {
            foreach (ParameterConversion pc in paramValue)
            {
                if (pc.getParamName() == _paramName)
                {
                    pc.setParameter(_value);
                    return;
                }
            }

            Debug.Log("No parameter named " + _paramName + " found...");

        }

        public void testSerializeWithExt(string extension)
        {
            XmlDocument document = new XmlDocument();

            XmlSerialize(document,null,true);

            document.Save(Name + "." + extension);
        }

        public void testSerialize(string name)
        {
            XmlDocument document = new XmlDocument();


            XmlSerialize(document,null,true);

            document.Save(name);
        }

        public void testDeserialize(string name)
        {
            XmlDocument document = new XmlDocument();
            document.Load(name);

            XmlNodeList nodeList = document.ChildNodes;

            deserializeFromXml(nodeList[0]);


        }

        //trigger에서의 deserialize 짜기
        //trigger, action, instruction은 아마 공유할 듯d
        public void deserializeFromXml(XmlNode childNodeList)
        {
            clearParamValueAll();

            if (childNodeList.Name != this.GetType().ToString())
            {
                Debug.Log("In power...");
                Debug.Log(childNodeList.Name);
                Debug.Log(this.GetType().ToString());

                Debug.Log("It is not trigger xml template...");
                return;
            }

            Name = childNodeList.Attributes["name"].InnerText;


            XmlNodeList childList = childNodeList.ChildNodes;

            foreach (XmlNode xn in childList)
            {
                if (xn.Name == "PairwiseAsset")
                {
                    ownedAssetName = xn.Attributes["contents"].InnerText;
                }
                else if (xn.Name == "PossibleAttachedAssetType")
                {
                    attachedAssetType = xn.Attributes["contents"].InnerText;
                }
                else if (xn.Name == "CustomizedProperty")
                {
                    foreach (XmlNode xnInner in xn.ChildNodes)
                    {
                        if (xnInner.Attributes["name"].InnerText == "PropertiesList")
                        {
                            deserializePropertiesList(xnInner);
                        }
                    }
                }
            }
        }

        //inner property에 대한 deserialize임
        void deserializePropertiesList(XmlNode _xnode)
        {
            foreach (XmlNode xn in _xnode.ChildNodes)
            {
                //내부에 list가 또 있을 경우
                if (xn.Name == "vrat.ListOfXmlTemplate")
                {
                    string name = xn.Attributes["name"].InnerText;
                    string type = xn.Attributes["type"].InnerText;
                    string idx = xn.Attributes["idx"].InnerText;

                    ParameterConversion pc = new ParameterConversion();
                    pc.setParameterType(ParameterConversion.str2ParamType(type));
                    pc.setParameterName(name);
                    pc.setParameter(idx);

                    foreach (XmlNode xnInner in xn.ChildNodes)
                    {
                        //일단 죄다 string으로 가정합시다 처리 귀찮음
                        string value = xnInner.Attributes["contents"].InnerText;
                        pc.setAdditionalInfo(value);
                    }
                    paramValue.Add(pc);
                }
                //primitive한 xml일 경우
                else if (xn.Name == "vrat.PrimitiveXmlTemplate")
                {
                    string name = xn.Attributes["name"].InnerText;
                    string type = xn.Attributes["type"].InnerText;
                    ParameterConversion pc = new ParameterConversion();
                    pc.setParameterType(ParameterConversion.str2ParamType(type));
                    pc.setParameterName(name);
                    pc.setParameter(xn.Attributes["contents"].InnerText);
                    paramValue.Add(pc);
                }

                else if (xn.Name == "vrat.VariableXmlTemplate")
                {
                    //처리 필요함
                }

                else if (xn.Name == "vrat.LocationXmlTemplate")
                {
                    //location 처리 필요함
                }
            }
        }

        //모든 parameter print하기, debugging 용임
        public void printAllParameter()
        {
            Debug.Log("Class Name is " + ClassName);
            Debug.Log("Name is " + Name);
            Debug.Log("Pairwise asset is " + ownedAssetName);
            Debug.Log("Attachable type is " + attachedAssetType);

            
            foreach (ParameterConversion pc in paramValue)
            {
                pc.printAllParam();
            }
        }

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement rootElement, bool isRoot)
        {
            //일단 단일 파일로 저장할 경우 parentElement가 없음을 유의해야 함...
            string propertyName = Name;
            string propertyType = Type;

            
            XmlElement root = document.CreateElement(this.GetType().ToString());

            root.SetAttribute("name", Name);
            root.SetAttribute("type", ClassName);


            

            //얘가 가장 최상위 파일일 시
            if (isRoot == true)
            {
                document.AppendChild(root);
            }
                //얘가 최상위가 아닐 시
            else
            {
                rootElement.AppendChild(root);
            }
            XmlElement parentElement = root;



            XmlElement pairAssetProperty = document.CreateElement("PairwiseAsset");

            parentElement.AppendChild(pairAssetProperty);

            pairAssetProperty.SetAttribute("name", "OwnedAssetName");
            pairAssetProperty.SetAttribute("type", ParameterConversion.str2ParamType("string").ToString());
            pairAssetProperty.SetAttribute("contents", ownedAssetName);




            XmlElement attachedAssetProperty = document.CreateElement("PossibleAttachedAssetType");

            parentElement.AppendChild(attachedAssetProperty);

            attachedAssetProperty.SetAttribute("name", "AttachedAssetType");
            attachedAssetProperty.SetAttribute("type", ParameterConversion.str2ParamType("string").ToString());
            attachedAssetProperty.SetAttribute("contents", attachedAssetType);



            XmlElement individualProperty = document.CreateElement("CustomizedProperty");

            parentElement.AppendChild(individualProperty);


            //1차적으로 pair asset을 넣자

            

            ListOfXmlTemplate lxt = new ListOfXmlTemplate("PropertiesList","ListOfXmlTemplate", 0);

            foreach (ParameterConversion pc in paramValue)
            {
                //choice일 경우에만 다르게 함
                if (pc.getParameterType() == PARAMTYPE.CHOICE)
                {
                    ListOfXmlTemplate innerlxt = new ListOfXmlTemplate(pc.getParamName(), pc.getParameterType().ToString(), int.Parse(pc.getParameter()));


                    string[] q = pc.getAdditionalInfo();

                    for (int i = 0; i < q.Length; i++)
                    {
                        PrimitiveXmlTemplate innerpxt = new PrimitiveXmlTemplate(pc.getParamName().ToString() + i.ToString(), q[i], "STRING");
                        innerlxt.addList(innerpxt);
                    }

                    lxt.addList(innerlxt);

                }

                else
                {
                    PrimitiveXmlTemplate pxt = new PrimitiveXmlTemplate(pc.getParamName(), pc.getParameter(), pc.getParameterType().ToString());
                    lxt.addList(pxt);
                    
                }
            }
            lxt.XmlSerialize(document, individualProperty,false);

            return individualProperty;
        }

    }
}