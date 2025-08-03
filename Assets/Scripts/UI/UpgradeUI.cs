using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Upgrades;

namespace UI
{
    public class UpgradeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IPointerMoveHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;

        [Header("Tilt Settings")] public float maxTiltAngle = 15f;
        public float tiltSmoothness = 0.1f;

        private Upgrade _upgrade;
        private bool _isHovered = false;

        public void Init(Upgrade upgrade)
        {
            if (upgrade == null) return;
            image.sprite = upgrade.Icon;
            title.text = upgrade.Name;
            description.text = upgrade.Description;
            _upgrade = upgrade;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            Tween.Scale(transform, 1.2f, 0.15f, useUnscaledTime: true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            Tween.Scale(transform, 1f, 0.15f, useUnscaledTime: true);
            Tween.Rotation(transform, Quaternion.identity, 0.25f, useUnscaledTime: true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_upgrade != null)
            {
                _upgrade.Apply();
            }

            UpgradeScreen.Current.Close();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_isHovered) return;
            var rectTransform = (RectTransform)transform;

            // Get the bounds of the UI element
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            // Calculate the center and width of the element
            float centerX = (corners[0].x + corners[2].x) * 0.5f;
            float elementWidth = corners[2].x - corners[0].x;

            // Get mouse position relative to element center (-1 to 1)
            float mouseX = eventData.position.x;
            float normalizedX = Mathf.Clamp((mouseX - centerX) / (elementWidth * 0.5f), -1f, 1f);

            // Calculate tilt angle
            float targetTiltZ = -normalizedX * maxTiltAngle;

            // Apply smooth rotation
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetTiltZ);
            Tween.Rotation(transform, targetRotation, tiltSmoothness, useUnscaledTime: true);
        }
    }
}