using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
 * Timeline에서의 dragMe를 다시 정의하자,
 * 
 * 여기서 icon에서의 image 여부 등을 바꿔야 합니당
 * 
 * icon instantiate 될 때 전체 form을 복사했다가 지우는 형식으로 합시당
 * 
 * 
 * primitveUI에서 dragMe 가능하고
 * 
 * primitveUIWindow로 drop시 위치 이동
 * timelineWindow로 drop시 event로 떨굴 수 있도록 하기...
 * 
 * */
namespace vrat
{
    //[RequireComponent(typeof(Image))]
    public class DragMeTimeline : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public bool dragOnSurfaces = true;

        private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
        private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

        [SerializeField]
        FileUITemplateManager fileUITemplate;

        [SerializeField]
        UnityEngine.UI.Image imageOnDrag;
        
        public delegate void OnDragStartCallback();
        public OnDragStartCallback callbackDrag;

        void Start()
        {
            //drag start를 위한 callback 함수 설정하기
            callbackDrag = fileUITemplate.OnDragStart;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            callbackDrag();

            var canvas = FindInParents<Canvas>(gameObject);
            if (canvas == null)
                return;

            // We have clicked something that can be dragged.
            // What we want to do is create an icon for this.
            m_DraggingIcons[eventData.pointerId] = new GameObject("icon");

            m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
            m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

            var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
            // The icon will be under the cursor.
            // We want it to be ignored by the event system.
            var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;


            //image.sprite =  GetComponent<Image>().sprite;

            image.sprite = imageOnDrag.sprite;

            image.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

            if (dragOnSurfaces)
                m_DraggingPlanes[eventData.pointerId] = transform as RectTransform;
            else
                m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;

            SetDraggedPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_DraggingIcons[eventData.pointerId] != null)
                SetDraggedPosition(eventData);
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            if (dragOnSurfaces && eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
                m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;

            var rt = m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos;
                rt.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //이 부분에서 어디에 떨어졌는지 알 수 있나?

            if (m_DraggingIcons[eventData.pointerId] != null)
                Destroy(m_DraggingIcons[eventData.pointerId]);

            m_DraggingIcons[eventData.pointerId] = null;
        }

        static public T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null) return null;
            var comp = go.GetComponent<T>();

            if (comp != null)
                return comp;

            var t = go.transform.parent;
            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
            return comp;
        }
    }
}