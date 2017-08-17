using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Asset Group의 정의
 * Asset으로부터 상속을 받음
 * List of Asset이 정의되어 있음
 * */
namespace vrat
{
    public class AssetGroupTemplate : AssetTemplate
    {

        //Assets in groups
        public List<AssetTemplate> assetList=new List<AssetTemplate>();


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}