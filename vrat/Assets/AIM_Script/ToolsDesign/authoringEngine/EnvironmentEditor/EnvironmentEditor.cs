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
        //최근 room xml 정보
        AuthorableRoom currRoom;

        //environment 부모 object
        GameObject environmentParent;

        //최근 instantiate된 room object
        GameObject currRoomObject;

        //in-situ에 필요한 fps Character
        [SerializeField]
        ViewChanger fpsChar;

        string envPrefabPath;
        string charPrefabPath;

        //global로 넘겨주자
        public static AuthorableRoom _currRoomGlobal;
         

        public override void initialize()
        {

            fileType = "room";
            fileSavePath = Application.dataPath + "/Resources/RoomFiles/";
            envPrefabPath = "RoomFiles/prefab/";
            charPrefabPath = "EnvironmentEditor/FPSController";

            environmentParent = GameObject.FindGameObjectWithTag("environment");

            base.initialize();
            

            

        }

        //선택시 불러오기
        public override void OnDoubleClickFile(int idx)
        {
            if (isDoubleClick(idx) == true)
            {
                Debug.Log("We catch double click event on " + idx.ToString());

                currRoom = new AuthorableRoom();

                currRoom.initialize();

                currRoom.testDeserialize(currFileList[idx].fullFileNamePath);

                string prefabName = (currRoom.variableContainer.getParameters("PrefabName") as PrimitiveXmlTemplate).getVariable();
                
                currRoomObject = GameObject.Instantiate(Resources.Load(envPrefabPath + prefabName), new Vector3(), Quaternion.Euler(50, -30, 0), environmentParent.transform) as GameObject;
                currRoomObject.transform.localPosition = new Vector3();
                currRoomObject.transform.localRotation = Quaternion.Euler(50, -30, 0);

                //global로 room 정보 넣어주기
                _currRoomGlobal = currRoom;
                
                fpsChar.changeView();


                /*
                 * 이 부분에서 원래 방끼리 연결할 수 있는 형태의 편집창을 만들고
                 * 이후에 in-situ로 들어가야 하는데 
                 * 
                 * 일단은 바로 시점이 바뀌도록 하자
                 * */
                
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