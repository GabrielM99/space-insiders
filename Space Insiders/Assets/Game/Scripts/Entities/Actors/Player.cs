using System;
using System.Collections.Generic;
using Game.Effects;
using Game.Items;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Entities
{
    /// <summary>
    /// An user-controlled ship.
    /// </summary>
    public class Player : Ship
    {
        [Header(nameof(Player))]
        [SerializeField, Min(0)] private int _score;

        [Space]
        [SerializeField] private ValueGraphics _lifeValueGraphics;
        [SerializeField] private ValueGraphics _scoreValueGraphics;

        [Space]
        [SerializeField] private PauseScreen _pauseScreen;

        private int score
        {
            get => _score;

            set
            {
                _score = value;
                scoreValueGraphics.SetValue(value);
            }
        }

        private ValueGraphics lifeValueGraphics { get => _lifeValueGraphics; }
        private ValueGraphics scoreValueGraphics { get => _scoreValueGraphics; }

        private PauseScreen pauseScreen { get => _pauseScreen; }

        private List<EffectInfo> effects { get; set; }
        private List<EffectInfo> effectsToRemove { get; set; }

        #region Unity
        protected override void Awake()
        {
            base.Awake();
            effects = new List<EffectInfo>();
            effectsToRemove = new List<EffectInfo>();
        }

        protected override void Start()
        {
            base.Start();
            life.onValueChanged += (value) => lifeValueGraphics.SetValue(value);
        }

        protected override void Update()
        {
            base.Update();
            UpdateInputs();
            UpdateEffects();
        }

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            base.OnCollisionEnter2D(other);

            // We collided with an alien.
            if (other.gameObject.IsEntity<Alien>())
            {
                Destroy();
            }
        }
        #endregion

        public override void OnTargetHit(IDamageable target)
        {
            base.OnTargetHit(target);

            if (target is Alien alien && alien.life.isEmpty)
            {
                score += alien.points;
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            level.Restart();
        }

        /// <summary>
        /// Called when the player collects an item.
        /// </summary>
        public void OnCollect(Item item)
        {
            foreach (Effect effect in item.effects)
            {
                ApplyEffect(effect);
            }
        }

        /// <summary>
        /// Applies an effect.
        /// </summary>
        private void ApplyEffect(Effect effect)
        {
            effect.OnStart(this);

            if (effect.duration > 0f)
            {
                effects.Add(new EffectInfo(effect));
            }
        }

        /// <summary>
        /// Updates all effects.
        /// </summary>
        private void UpdateEffects()
        {
            foreach (EffectInfo effectInfo in effects)
            {
                effectInfo.effect.OnUpdate(this);

                if (effectInfo.timer.Run())
                {
                    effectsToRemove.Add(effectInfo);
                }
            }

            foreach (EffectInfo effectToRemove in effectsToRemove)
            {
                RemoveEffect(effectToRemove);
            }

            effectsToRemove.Clear();
        }

        /// <summary>
        /// Removes an effect.
        /// </summary>
        private void RemoveEffect(EffectInfo effectInfo)
        {
            effectInfo.effect.OnStop(this);
            effects.Remove(effectInfo);
        }

        /// <summary>
        /// Updates all player inputs.
        /// </summary>
        private void UpdateInputs()
        {
            movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

            if (Input.GetKey(KeyCode.Space))
            {
                Shoot(Vector2.up);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseScreen.Toggle();
            }
        }
    }
}
