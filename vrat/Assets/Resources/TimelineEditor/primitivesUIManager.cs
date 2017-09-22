﻿using System.Collections;
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
        PRIMITIVESUI myUIType;

        int clicked = 0;
        float clickdelay = 0.5f;
        float clicktime = 0.0f;

        public delegate void OnDoubleClickCallback(int idx);
        public OnDoubleClickCallback callback;
        

        public void setMyUIType(PRIMITIVESUI _type)
        {
            myUIType = _type;
            
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
        public bool isDoubleClick()
        {
            clicked++;

            if (clicked == 1)
            {
                clicktime = Time.time;
                
            }


            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                return true;
                
            }
            else if (clicked > 2 || Time.time - clicktime > 1)
            {
                clicked = 0;
                
            }

            return false;

        }

        public void OnClick()
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