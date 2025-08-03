using System;
using Player;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zones;

namespace Utils
{
    public class CameraEffects : MonoBehaviour
    {
        public static CameraEffects Current { get; private set; }
        public float trauma = 0;

        [SerializeField] private CinemachinePositionComposer camPos;
        [SerializeField] private float maxShakeAngle = 10;
        [SerializeField] private float maxShakeOffset = 40;
        [SerializeField] private float freqMultiplierAngle = 10;
        [SerializeField] private float freqMultiplier = 10;
        [SerializeField] private float zoneTransitionTime = 0.2f;
        [SerializeField] private Volume volume;

        private bool _wasInZone = true;
        private bool _transitioningZoneState;
        private bool _transitionIn;
        private float _zoneStateTime;
        private float _zoneState;
        private float _originalVignetteIntensity;

        public float ChromaticAberration
        {
            get
            {
                if (volume.profile.TryGet(out ChromaticAberration ca))
                {
                    return ca.intensity.value;
                }

                return 0;
            }
            set
            {
                if (volume.profile.TryGet(out ChromaticAberration ca))
                {
                    ca.intensity.value = value;
                }
            }
        }

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            if (volume.profile.TryGet(out Vignette vignette))
            {
                _originalVignetteIntensity = vignette.intensity.value;
            }
        }

        public void Update()
        {
            var dt = Time.deltaTime;
            var traumaClamped = Mathf.Clamp01(trauma - dt * 0.55f);
            var shake = traumaClamped * traumaClamped;

            var angle = maxShakeAngle * shake *
                        (Mathf.PerlinNoise(1000, Time.timeSinceLevelLoad * freqMultiplierAngle) * 2 - 1);
            var offX = maxShakeOffset * shake *
                       (Mathf.PerlinNoise(2000, Time.timeSinceLevelLoad * freqMultiplier) * 2 - 1);
            var offY = maxShakeOffset * shake *
                       (Mathf.PerlinNoise(3000, Time.timeSinceLevelLoad * freqMultiplier) * 2 - 1);

            camPos.TargetOffset = new Vector3(offX, offY, 0);
            camPos.transform.rotation = Quaternion.Euler(0, 0, angle);
            ChromaticAberration = traumaClamped * traumaClamped * traumaClamped;
            trauma = traumaClamped;

            var nowInZone = Zone.PlayerInZone;
            if (_wasInZone != nowInZone)
            {
                _transitioningZoneState = true;
                _transitionIn = nowInZone;
            }

            var transitionTime = zoneTransitionTime + PlayerZoneDamage.Current.DamageCooldown * 0.75f;

            if (_transitioningZoneState)
            {
                if (_zoneStateTime < 0 || _zoneStateTime > transitionTime)
                {
                    _transitioningZoneState = false;
                }

                if (_transitionIn)
                {
                    _zoneStateTime -= dt;
                }
                else
                {
                    _zoneStateTime += dt;
                }

                _zoneStateTime = Mathf.Clamp(_zoneStateTime, 0, transitionTime);
                _zoneState = Mathf.Clamp01((_zoneStateTime - PlayerZoneDamage.Current.DamageCooldown * 0.75f) / zoneTransitionTime);
            }
            
            if (volume.profile.TryGet(out Vignette vignette))
            {
                vignette.intensity.value = _originalVignetteIntensity + 0.2f * _zoneState * (Mathf.Sin(Time.time * 3)+1) / 2;
                vignette.color.value = Color.Lerp(Color.black, Color.red, _zoneState);
            }

            _wasInZone = nowInZone;
        }
    }
}