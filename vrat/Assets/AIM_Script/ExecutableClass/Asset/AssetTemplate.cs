using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
/*
 * Asset template
 * Only asset cannot do training... asset collaborated event can be possible in training stage
 * 
 * Execute Asset in training mode...
 * 
 * 
 * 
 * */
namespace vrat
{
    public class AssetTemplate : AuthorableAsset
    { 

    }
}
/*
 * 소화기 Asset
3D model: 전체 소화기 mesh(수정 불가)
Bounding box: 전체 소화기 bounding box(수정 불가)
Effect list:(Timeline authoring시 effect list와 trigger list에서 trigger와 effect 선택 가능)
Highlight
Informing
소화기 들기
노즐 위로 이동
물 분사
Trigger list:(Timeline authoring시 effect list와 trigger list에서 trigger와 effect 선택 가능)
Asset의 기본 trigger
(Asset 발견, asset이 시야에 들어올 시 등)
밑의 파트 asset에서의 trigger 생성 가능(Asset list를 만들 시 asset list에서 각 asset의 trigger를 어떻게 합칠 지 선택 가능(sequential or parallel, AND or OR)
New trigger:
전체 분사 trigger: 먼저 안전핀의 trigger  손잡이의  trigger AND trigger  바디의 trigger를 설정
전체 순서대로 동작 trigger: 안전핀의 순서대로동작 trigger  손잡이의 순서대로동작 trigger 노즐의 순서대로 동작 trigger  바디의 순서대로동작 trigger
Location:
In-situ에서 변경 가능
List of asset: 안전핀, 손잡이, 노즐, 바디

 * */
