using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * For locking screen, available to change walk speed with parameter input value...
 * */
namespace vrat
{
    public class LockScreenEffect : EffectTemplate
    {

        //Lock Screen을 위한 FirstPersonController
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;

        bool isSetFPSCharacter = false;
        
        float walkSpeed;
        
        //FPS Character 설정해줘야 함
        //이 부분은 authoring이 아닌 runtime에서 수행될 듯

        public void setFPSCharacter(UnityStandardAssets.Characters.FirstPerson.FirstPersonController _controller)
        {
            controller = _controller;
            isSetFPSCharacter = true;
        }


        public override void initialize()
        {
            parameterNames[0] = "walkSpeed";
        }
        //        public ListOfXmlTemplate(string _name, string _type) : base(_name, _type)
        

        


        protected override void process()
        {
            //default(T) for float이 무슨 값인지 모르겠지만 이 부분에서 걸러주는 게 필요할 수도?
            
        }
    }
}