using System.Collections;
using System.Collections.Generic;
using Game.Effects;
using Game.Items;
using Game.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Entities
{
    /// <summary>
    /// An user-controlled ship.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Player : Ship
    {
        [Header(nameof(Player))]
        [SerializeField, Min(0)] private int _score;

        [Space]
        [SerializeField] private ValueGraphics _lifeValueGraphics;
        [SerializeField] private ValueGraphics _scoreValueGraphics;

        [Space]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _collectSound;

        private const string MOVEMENT_BUTTON = "Horizontal";
        private const string SHOOT_BUTTON = "Shoot";
        private const string PAUSE_BUTTON = "Pause";

        public int score
        {
            get => _score;

            private set
            {
                _score = value;
                scoreValueGraphics.SetValue(value);
            }
        }

        private AudioSource audioSource { get => _audioSource; set => _audioSource = value; }
        private AudioClip collectSound { get => _collectSound; }

        private bool isMouseOverUI => EventSystem.current.IsPointerOverGameObject();

        private ValueGraphics lifeValueGraphics { get => _lifeValueGraphics; }
        private ValueGraphics scoreValueGraphics { get => _scoreValueGraphics; }

        private List<EffectInfo> effects { get; set; }
        private List<EffectInfo> effectsToRemove { get; set; }

        #region Unity
        protected override void Reset()
        {
            base.Reset();
            audioSource = GetComponent<AudioSource>();
        }

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
            level.GameOver();
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

            audioSource.PlayOneShot(collectSound);
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
            movementDirection = new Vector2(Input.GetAxisRaw(MOVEMENT_BUTTON), 0f);

            if (!isMouseOverUI)
            {
                if (Input.GetButton(SHOOT_BUTTON))
                {
                    Shoot(Vector2.up);
                }
            }

            if (Input.GetButtonDown(PAUSE_BUTTON))
            {
                level.TogglePause();
            }
        }
    }
}
