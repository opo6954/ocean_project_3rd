using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * IMPELMENT FOR LATER...
     * */
    public class LoadShipEditor : EditorTemplate
    {
        //load ship environment 버튼 누를 때의 callback 동작



        public void OnClickLoadShip()
        {
            Debug.Log("Load ship environment~!~!");
            //LHW IMPLEMENTATION of loadship...


            uiSelectorHandler.turnOnEditorAll(true, false);
            uiSelectorHandler.turnOnEditorAll(false, true);


        }
    }
}