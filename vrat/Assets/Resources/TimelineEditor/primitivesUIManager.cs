using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    public enum PRIMITIVESUI
    {
        TRIGGER=0, ACTION=1, INSTRUCTION=2
    }
    public class primitivesUIManager : FileUITemplateManager
    {
        //무슨 ui type인지
        PRIMITIVESUI myUIType;
        

        //PAUT primitves의 xml template임
        PAUTPrimitivesTemplate infoTemplate;


        

        //나의 primitive 이름임, 이걸로 어느 trigger인지 찾을 수 있음
        string primName;


        float clickdelay = 0.5f;
        float clicktime = 0.0f;

        public delegate void OnDoubleClickCallback(int idx);
        public OnDoubleClickCallback callback;

        public PAUTPrimitivesTemplate getPAUTPrimitiveTemplate()
        {
            return infoTemplate;
        }

        public void setPAUTPrimitveTemplate(PAUTPrimitivesTemplate _template)
        {
            infoTemplate = new PAUTPrimitivesTemplate("", "");
            
            PAUTPrimitivesTemplate.CopyPAUT(_template, ref infoTemplate);
        }

        public void setMyPrimName(string _primName)
        {
            primName = _primName;
        }

        public void setMyUIType(PRIMITIVESUI _type)
        {
            setOnDragListener(tmp);
            myUIType = _type;
            
        }

        public PRIMITIVESUI getMyUIType()
        {
            return myUIType;
        }

        public string getMyPrimName()
        {
            return primName;
        }

        public override void setPosition(int _idx)
        {
            
            
            idx = _idx;

            int tmpIdx = idx;

            //홀수번째일 경우 xOffset만큼 주기

            Vector3 position = new Vector3(xInitOffset, 0, 0);

            if (tmpIdx % 3 == 1)
            {
                position.x = position.x + xOffset;
                tmpIdx = tmpIdx - 1;
            }

            int idxOri = tmpIdx / 3;

            position.y = 0 + yOffset * idxOri;

            GetComponent<RectTransform>().localPosition = position;
        }

        public void setDoubleClickCallback(OnDoubleClickCallback _callback)
        {
            callback = _callback;
        }

        //doubleclick callback임
        /*
         * click된 상태에서 
         * */
        public bool isDoubleClick()
        {
            if (Time.time - clicktime < clickdelay)
            {
                
                return true;
            }
            else
            {
                clicktime = Time.time;
            }
                
            return false;
            
        }

        public void tmp(int idx)
        {
            
        }

        

        //drag and drop으로 
        public void OnClickUITemplate()
        {
            if (isDoubleClick() == true)
            {
                callback(idx);
            }
        }
        /*
         * int clicked = 0;
        float clickdelay = 0.5f;
        float clicktime = 0.0f;
        int prevIdx = -1;

        protected bool isDoubleClick(int _idx)
        {
            clicked++;
            
            if (clicked == 1)
            {
                clicktime = Time.time;
                prevIdx = _idx;
            }


            if (clicked > 1 && Time.time - clicktime < clickdelay && prevIdx == _idx)
            {
                clicked = 0;
                clicktime = 0;
                prevIdx = -1;
                return true;
            }
            else if (clicked > 2 || Time.time - clicktime > 1)
            {
                clicked = 0;
                prevIdx = -1;
            }

            return false;
        }
         * */
    }
}