using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * instruction을 위한 것임
     * 
     * 얘는 굳이 지정할 필요 있을까?
     * 
     * 
     * */
    
    public class InstPrimitivesTemplate : PAUTPrimitivesTemplate
    {
        public void example()
        {
            
        }

        public void initForInst()
        {

        }

        //일단 걍 임시로 만들자
        public InstPrimitivesTemplate(string _name, string _type) : base(_name, _type)
        {
            ClassName = "InstPrimitivesTemplate";

            Name = "Instruction";

            ParameterConversion pc = new ParameterConversion();
            pc.setParameterType(PARAMTYPE.STRING);
            pc.setParameterName("Instruction");
            pc.setParameter("이곳으로 와서 X 버튼을 누르세여");

            paramValue.Add(pc);

            ParameterConversion pc_option = new ParameterConversion();
            pc_option.setParameterType(PARAMTYPE.BOOL);
            pc_option.setParameterName("IsGlobalOrLocal");
            pc_option.setParameter(true.ToString());

            paramValue.Add(pc_option);
           
        }


        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            return base.XmlSerialize(document, parentElement, isRoot);
        }
    }
}