using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace vrat
{

    public class DropMeOnEditor : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        TimelineEditor timelineEditor;

        [SerializeField]
        RectTransform rectForPrimitives;

        


        public Image containerImage;
        public Image receivingImage;
        private Color normalColor;
        public Color highlightColor = Color.yellow;
	
        public void OnEnable ()
        {
	        if (containerImage != null)
		        normalColor = containerImage.color;
        }
	
        public void OnDrop(PointerEventData data)
        {
            //primitive window에 올 경우
            
            if (RectTransformUtility.RectangleContainsScreenPoint(rectForPrimitives, data.position, Camera.main) == true)
            {
                timelineEditor.OnDetectDropOnPrimitives(data);
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            //Debug.Log("On pointer enter on " + gameObject.name);
        }

        public void OnPointerExit(PointerEventData data)
        {
            //Debug.Log("Exit...");
        }
	
    }
}