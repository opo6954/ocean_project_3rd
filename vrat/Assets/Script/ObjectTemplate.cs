using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/*
 * 저작 가능한 authoring template 있음
 * parameter들을 dictionary 형태로 저장하고 있고 필요한 parameter의 이름 역시 저장하고 있음
 * 
*/

namespace vrat
{

    public class ObjectTemplate : MonoBehaviour
    {

        //parameter 개수
        protected int numParam;

        //parameter를 가지고 있는 dictionary
        protected Dictionary<string, object> parameters = new Dictionary<string, object>();

        //parameter의 이름들
        protected List<string> parameterName = new List<string>();

        protected string type;



        //초기화
        public virtual void initialize()
        {

        }



        public virtual bool addParameter(string paramName, object paramValue)
        {
            if (parameters.ContainsKey(paramName) == true)
            {
                parameters.Add(paramName, paramValue);
                return true;
            }

            return false;
        }


        //xml 파일로 저장
        public bool serialize2Xml()
        {
            return true;
        }
        //xml 파일을 읽어서 
        public bool deserializeFromXml()
        {
            return true;
        }
        //xml로 시나리오 읽어서 instantiate해주기
        //Authoring result --> Executable result로 바꾸는 거임

        public T instantiateXml<T>()
        {
            if (type == "Asset")
            {
            }
            else if (type == "Event")
            {
            }
            else if (type == "Scenario")
            {
            }
            return default(T);
        }


    }
}