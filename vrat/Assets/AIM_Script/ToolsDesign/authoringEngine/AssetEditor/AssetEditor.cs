using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
/*
 * Asset editor
 * 
 * assetList에서 더블클릭시
 * */
    public class AssetEditor : WindowTemplate
    {
        //public delegate void OnDoubleClickForEditor(AuthorableAsset aa);

        //asset list window handler한테 callback instance를 이 함수를 통해 넘겨줌/ asset 정보가 AuthorableAsset을 통해서 넘겨진다
        //그러면 이제 시작됨    
        public void OnSelectAsset(AuthorableAsset aa)
        {
        }

        // Use this for initialization
        void Start()
        {
            //test for .room file...

            /*
            RoomXmlTemplate r = new RoomXmlTemplate();

            r.initialize();
            r.exampleSerialize();

            r.testSerialize("test.room");

            RoomXmlTemplate q = new RoomXmlTemplate();

            q.initialize();
            q.testDeserialize("test.room");

            Debug.Log((q.variableContainer.getParameters(0) as PrimitiveXmlTemplate).getVariable());
            Debug.Log(((q.variableContainer.getParameters(1) as ListOfXmlTemplate).getXmlTemplate(0) as LocationXmlTemplate).Position);
            */

            /*
            //for example asset...
            AuthorableAsset aa = new AuthorableAsset();
            aa.initialize();
            
            aa.examplePhoneSerialize();
            aa.testSerialize("phone.asset");
            AuthorableAsset aa2 = new AuthorableAsset();
            aa2.testDeserialize("phone.asset");


            Debug.Log("total count: " + aa2.variableContainer.getNumberOfParameters());
            for (int i = 0; i < aa2.variableContainer.getNumberOfParameters(); i++)
            {
                Debug.Log(aa2.variableContainer.getParameters(i).Name);
            }
            */







            /*
             * <ROOM name="Cruise">
<CustomizedProperty>
<vrat.PrimitiveXmlTemplate name="PrefabName" type="string" contents="EngineRoom" />
<vrat.ListOfXmlTemplate name="TeleportableList" type="ListOfXmlTemplate" idx="0">
  <vrat.LocationXmlTemplate name="Teleportable1" type="location">
    <Position>
      <x>100</x>
      <y>100</y>
      <z>100</z>
    </Position>
    <Rotation>
      <x>50</x>
      <y>50</y>
      <z>50</z>
    </Rotation>
  </vrat.LocationXmlTemplate>
  <vrat.LocationXmlTemplate name="Teleportable2" type="location">
    <Position>
      <x>1</x>
      <y>1</y>
      <z>1</z>
    </Position>
    <Rotation>
      <x>150</x>
      <y>150</y>
      <z>150</z>
    </Rotation>
  </vrat.LocationXmlTemplate>
  <vrat.LocationXmlTemplate name="Teleportable3" type="location">
    <Position>
      <x>33</x>
      <y>33</y>
      <z>33</z>
    </Position>
    <Rotation>
      <x>250</x>
      <y>250</y>
      <z>250</z>
    </Rotation>
  </vrat.LocationXmlTemplate>
</vrat.ListOfXmlTemplate>
</CustomizedProperty>
</ROOM>
             * */

            //test for .asset file...

            /*
            AuthorableAsset aa = new AuthorableAsset();
            aa.initialize();
            aa.exampleSerialize();
            aa.testSerialize("test.asset");

            AuthorableAsset bb = new AuthorableAsset();
            bb.initialize();
            bb.testDeserialize("test.asset");
            */


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}