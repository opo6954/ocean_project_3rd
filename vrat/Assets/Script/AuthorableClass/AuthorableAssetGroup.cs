using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class AuthorableAssetGroup : AuthorableAsset
    {
        public AuthorableAssetGroup()
        {
            //Asset에 필요한 파라미터 이름 넣기
            parameterName.Add("AssetGroup");
        }
    }
}