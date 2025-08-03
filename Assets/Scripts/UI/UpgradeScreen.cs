using PrimeTween;
using UnityEngine;
using Upgrades;
using Utils;

namespace UI
{
    public class UpgradeScreen : MonoBehaviour
    {
        public static UpgradeScreen Current { get; private set; }

        [SerializeField] private CanvasGroup group;
        [SerializeField] private RectTransform row;
        [SerializeField] private UpgradeUI[] upgrades;

        private void Awake()
        {
            Current = this;
        }

        public void Open(Upgrade[] data)
        {
            group.blocksRaycasts = true;
            group.interactable = true;
            PlayerInput.Disabled = true;
            
            Tween.Custom(Time.timeScale, 0, 0.1f, x => Time.timeScale = x, useUnscaledTime: true);
            Tween.Alpha(group, 1, 0.1f, useUnscaledTime: true);
            Tween.UIAnchoredPositionY(row, 0, 0.15f, useUnscaledTime: true);
            
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].Init(data[i]);
                var rectTransform = (RectTransform)upgrades[i].transform;
                var pivot = rectTransform.pivot;
                pivot.y = 1.5f;
                rectTransform.pivot = pivot;
                Tween.UIPivotY(rectTransform, 0.5f, 0.2f, useUnscaledTime: true, startDelay: i * 0.05f);
            }
        }

        public void Close()
        {
            Tween.Custom(Time.timeScale, 1, 0.1f, x => Time.timeScale = x, useUnscaledTime: true);
            Tween.Alpha(group, 0, 0.1f, useUnscaledTime: true);
            Tween.UIAnchoredPositionY(row, -1080.2f, 0.15f, useUnscaledTime: true);
            
            group.interactable = false;
            group.blocksRaycasts = false;
            PlayerInput.Disabled = false;
        }
    }
}