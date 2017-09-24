using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace vrat
{
    public class DragMeEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public bool dragOnSurfaces = true;

        private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
        private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

        [SerializeField]
        FileUITemplateManager fileUITemplate;

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



            //본인 것을 복제하기
            m_DraggingIcons[eventData.pointerId] = GameObject.Instantiate(gameObject);


            //이미지 관련된 것 외의 것은 모두 꺼버리기
            m_DraggingIcons[eventData.pointerId].GetComponent<eventUIManager>().enabled = false;
            m_DraggingIcons[eventData.pointerId].GetComponent<Button>().enabled = false;
            m_DraggingIcons[eventData.pointerId].GetComponent<DragMeEvent>().enabled = false;



            m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
            m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

            // The icon will be under the cursor.
            // We want it to be ignored by the event system.
            var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;


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
            //가운데에 놓기
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], new Vector2(eventData.position.x - rt.rect.width / 2, eventData.position.y + rt.rect.height / 2), eventData.pressEventCamera, out globalMousePos))
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