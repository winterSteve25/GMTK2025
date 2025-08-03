using System;
using System.Collections.Generic;
using Enemies;
using PrimeTween;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerExp : MonoBehaviour
    {
        public static PlayerExp Current { get; private set; }
        
        public float radius;
        public float experienceGainMultiplier;
        
        [SerializeField] private float exp;
        [SerializeField] private int level;
        
        [SerializeField] private Upgrade[] t1Upgrades;
        [SerializeField] private Upgrade[] t2Upgrades;
        [SerializeField] private Upgrade[] t3Upgrades;
        [SerializeField] private int t2Level;
        [SerializeField] private int t3Level;
        [SerializeField] private Image expBar;
        [SerializeField] private TMP_Text expText;

        private void Awake()
        {
            Current = this;
        }

        public void FixedUpdate()
        {
            var circles = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Exp Orb"));

            foreach (var result in circles)
            {
                result.transform.position = Vector2.Lerp(result.transform.position, transform.position, 0.1f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public void AddExp(float xp)
        {
            exp += xp * experienceGainMultiplier;

            var seq = Sequence.Create(useUnscaledTime: true)
                .Chain(Tween.UIFillAmount(expBar, Mathf.Clamp01(exp / ExpNeeded()), 0.2f));

            if (exp >= ExpNeeded())
            {
                exp -= ExpNeeded();
                level++;
                expText.text = level.ToString();
                UpgradeScreen.Current.Open(GenerateUpgrades());
                seq.Chain(Tween.UIFillAmount(expBar, Mathf.Clamp01(exp / ExpNeeded()), 0.2f));
            }
        }

        private Upgrade[] GenerateUpgrades()
        {
            var lst = new List<Upgrade>(t1Upgrades);

            if (level > t2Level)
            {
                lst.AddRange(t2Upgrades);
            }

            if (level > t3Level)
            {
                lst.AddRange(t3Upgrades);
            }
            
            var index = Random.Range(0, lst.Count);
            var index2 = Random.Range(0, lst.Count);

            if (index2 == index)
            {
                index2 = Random.Range(0, lst.Count);
            }
            
            var index3 = Random.Range(0, lst.Count);
            if (index3 == index2)
            {
                index3 = Random.Range(0, lst.Count);
            }
            
            return new[] { lst[index], lst[index2], lst[index3] };
        }

        private float ExpNeeded()
        {
            return level * level + 5;
        }
    }
}