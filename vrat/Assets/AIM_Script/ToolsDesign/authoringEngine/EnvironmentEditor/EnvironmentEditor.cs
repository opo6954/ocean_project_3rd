using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace vrat
{
    /*
     * Environment editor의 기능
     * 1. 전체적인 구조만 있는 방 불러오기
     * 2. 방을 불러오고 나서 출구와 입구를 드래그 앤 드롭으로 이어서 순간이동할 수 있는 지역 만들기
     * 3. 이후 방이 instantiate됨
     * 4. 사실적 효고를 위한 asset이 아닌 기타 배경이 되는 요소들의 배치
     * */
    public class EnvironmentEditor : FileExplorerTemplate
    {
        RoomXmlTemplate currRoom;

        public override void initialize()
        {

            fileType = "room";
            fileSavePath = Application.dataPath + "/Resources/RoomFiles/";
            base.initialize();
            //Test for serialize

            //Test for room file serialize

            RoomXmlTemplate r1 = new RoomXmlTemplate();

            r1.initialize();
            r1.exampleSerialize();
            r1.testSerialize("test.room");

            RoomXmlTemplate r2 = new RoomXmlTemplate();
            r2.initialize();

            r2.testDeserialize("test.room");

            Debug.Log("powerover");

            Debug.Log(r2.variableContainer.getParameters(0));


        }

        //선택시 불러오기
        public override void OnDoubleClickFile(int idx)
        {
            if (isDoubleClick(idx) == true)
            {
                Debug.Log("We catch double click event on " + idx.ToString());

                currRoom = new RoomXmlTemplate();

                currRoom.initialize();

                currRoom.testDeserialize(currFileList[idx].fullFileNamePath);

                Debug.Log("Read prefabName: " + currRoom.variableContainer.getParameters("PrefabName"));



                

            }
        }


        //170912 추후에 구현하자
        //방끼리 연결할 수 있는 형태의 편집창 열기
        public void loadRoomConnectEditorWindow()
        {

        }

        //170912 추후에 구현하자
        //방에서 in-situ로 background component를 배치할 수 있는 형태의 편집창 열기
        public void loadPlacementBackgroundComponentWindow()
        {
        }
       


    }
}