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
    public class EnvironmentEditor : EditorTemplate
    {
        public EnvironmentEditor()
        {
        }

        //방 구조를 열 수 있는 형태의 편집창 열기
        public void loadRoomStructureInfoWindow()
        {
        }

        //방끼리 연결할 수 있는 형태의 편집창 열기
        public void loadRoomConnectEditorWindow()
        {

        }

        //방에서 in-situ로 background component를 배치할 수 있는 형태의 편집창 열기
        public void loadPlacementBackgroundComponentWindow()
        {
        }



    }
}