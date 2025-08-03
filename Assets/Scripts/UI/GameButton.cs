using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utils;

namespace UI
{
    public class GameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private UnityEvent onClick;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = (RectTransform)transform;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioPlayer.PlayClick();
            Tween.Scale(_rectTransform, 1.2f, 0.2f, useUnscaledTime: true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween.Scale(_rectTransform, 1f, 0.2f, useUnscaledTime: true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}