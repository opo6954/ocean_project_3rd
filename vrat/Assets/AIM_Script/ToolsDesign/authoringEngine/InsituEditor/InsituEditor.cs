using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace vrat
{
    /*
     * In-situ placement를 하는 얘의 기본임
     * 
     * 필요한 기능들:
     * environment에 list of gameobject instantiate하기(asset, environemtObject 모두에 해당)
     * raycastor와의 통신(asset이나 environmentObject를 raycastor를 통해 눌렀을 때의 callback 함수 등이 정의되어야 함/아마 최종적으로 location 등을 변경할 때 필요할 듯)
     * 각종 버튼이나 기타 작동은 1인칭 캐릭터 안에서 하자
     * 
     * */

        /*
         * 170918 지금 버그 있음 ㅠㅠㅠㅠ
         * 두 번째로 insitu eidtor로 들어갈 때 targetasset gameobject가 없어지는지 몰라도 자꾸 missing reference exception 뜸...
         * */
    public class InsituEditor : EditorTemplate
    {



        private string prefabPath;


        //respawn될 environment 부모
        [SerializeField]
        GameObject environment;

        //1인칭 캐릭터가 가지고 있는 view changer
        [SerializeField]
        ViewChanger fpsChar;

        //1인칭 캐릭터가 가지고 있는 ray castor
        [SerializeField]
        LocateAssetWithRay lasr;

        [SerializeField]
        AssetEditor assetEditor;

        //전체 asset list를 가지고 있음, 일단 배치를 하기 위해서
        List<ObjectTemplate> placeableList;


        //현재 respawned된 gameobject들
        GameObject[] respawned;


        //전체 asset의 이름 list를 가지고 있음, 
        string[] placeableFullNameList;

        string tagName;

        int currAssetIdx=-1;
        

        //In-situ mode 시작

        public void clear()
        {
            respawned = GameObject.FindGameObjectsWithTag(tagName);

            
            Debug.Log("On clear in insituEditor, " + respawned.Length);

            for (int i = 0; i < respawned.Length; i++)
            {
                Debug.Log(respawned[i].name);
                GameObject.Destroy(respawned[i].gameObject);
            }

            

            


        }

        //in situ module 초기화
        public void setParameter(string _prefabPath, string _tagName, List<ObjectTemplate> _obtList)
        {
            prefabPath = _prefabPath;
            placeableList = _obtList;
            tagName = _tagName;
        }

        public void initialize()
        {
            clear();
            respawnAllPlaceables();
        }

        private void respawnAllPlaceables()
        {
            Debug.Log("Respawan all placeables from the list of " + placeableList.Count);

            for(int i=0; i<placeableList.Count; i++)
            {
                //location parameter 불러오기
                Location l = (placeableList[i].variableContainer.getParameters("Location") as LocationXmlTemplate).getVariable();
                
                //instantiate하기
                GameObject respawnPlaceable = GameObject.Instantiate(Resources.Load(prefabPath + placeableList[i].ObjectName), environment.transform) as GameObject;

                respawnPlaceable.name = placeableList[i].ObjectName;

                //parent에 local 좌표로 맞추기

                respawnPlaceable.transform.localPosition = l.position;
                respawnPlaceable.transform.localRotation = Quaternion.Euler(l.rotation);

                respawnPlaceable.tag = tagName;
            }


            respawned = GameObject.FindGameObjectsWithTag(tagName);

        }

        //in situ mode에 들어갈 시 수행
        //걍 in situ mode에서는 1개씩 바꾸자
        public void OnClickInSituMode(string _objectName)
        {
            //1인칭 캐릭터 켜기
            //Debug.Log("in insitu editor... click insitu mode...");
            
            fpsChar.changeView();

            currAssetIdx=0;


            respawned = GameObject.FindGameObjectsWithTag(tagName);


            for (int i=0; i<respawned.Length; i++)
            {
                Debug.Log("for all respawned object name: " + respawned[i].name);
                Debug.Log("for all respawned object position: " + respawned[i].transform.position);

                if (respawned[i].name == _objectName)
                {
                    lasr.startLocate(environment, respawned[i], callbackFromFPS);
                    currAssetIdx = i;

                    return;

                }
            }

            Debug.Log("Cannot find respwned name and given obj name...");

            
            
        }

        //locate를 완료했을 때의 callback 함수임
        //매 순간 locate가 완료될 때마다 callback 함수를 실행한다
        public void callbackFromFPS(Vector3 pos, Vector3 rot)
        {
            Debug.Log("Locate finish for ");

            Debug.Log("placeableList length: " + placeableList.Count);
            Debug.Log("current asset idx: " + currAssetIdx);

            Debug.Log(placeableList[currAssetIdx].ObjectName);

            ObjectTemplate currOT = placeableList[currAssetIdx];

            (currOT.variableContainer.getParameters("Location") as LocationXmlTemplate).setParameter(new Location(pos, rot));

            assetEditor.OnSaveFromInSituPlacement();



        }


        void Update()
        {

        }
    }
}