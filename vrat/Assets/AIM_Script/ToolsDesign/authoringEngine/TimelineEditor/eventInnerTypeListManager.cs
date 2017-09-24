using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * before action과 after action만을 위한 class임
     * list of assetimg로 해야 함
     * */

    public class eventInnerTypeListManager : eventInnerTypeManager
    {
        private int assetImgLength = 3;

        [SerializeField]
        List<UnityEngine.UI.RawImage> assetImgList = new List<UnityEngine.UI.RawImage>();




        //여기에 primitves idx를 넣어두자
        public int[] currPrimIdxList = new int[3];


        public int currPrimIdx;
        public int currOrderIdx;

        public override void setAssetImg(Texture2D _image)
        {
            //빈 곳부터 넣기
            for (int i = 0; i < assetImgList.Count; i++)
            {
                if (assetImgList[i].texture == null)
                {
                    assetImgList[i].texture = _image;
                    currPrimIdxList[i] = currPrimIdx;
                    currOrderIdx = i;
                    return;
                }
            }

            //texture 2d 설정하기
            
        }
        //특정 위치에 있는 assetImg 지우기
        public override void deleteAssetImg(int assetImgIdx)
        {
            if (assetImgIdx < assetImgLength)
            {
                assetImgList[assetImgIdx].texture = null;
            }
        }

        public void setCurrPrimIdx(int _currPrimIdx)
        {
            currPrimIdx = _currPrimIdx;
        }

        public int getCurrOrderIdx()
        {
            return currOrderIdx;
        }
        public int getCurrPrimIdx()
        {
            return currPrimIdx;
        }



    }
}