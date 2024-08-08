using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Agents
{
    /// <summary>
    /// An user-controlled ship.
    /// </summary>
    public class Player : Ship
    {
        [Header(nameof(Player))]
        [SerializeField, Min(0)] private int _score;

        [Space]
        [SerializeField] private TextMeshProUGUI _lifeTextMesh;
        [SerializeField] private TextMeshProUGUI _scoreTextMesh;

        public int score { get => _score; private set => _score = value; }

        private TextMeshProUGUI lifeTextMesh { get => _lifeTextMesh; }
        private TextMeshProUGUI scoreTextMesh { get => _scoreTextMesh; }

        #region Unity
        protected override void Update()
        {
            base.Update();
            UpdateInputs();
        }

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            base.OnCollisionEnter2D(other);

            // We collided with an alien.
            if (other.gameObject.IsEntity<Alien>())
            {
                // Game over.
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        #endregion

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            lifeTextMesh.text = life.ToString();
        }

        public override void OnTargetHit(IDamageable target)
        {
            base.OnTargetHit(target);

            if (target is Alien alien)
            {
                score += alien.points;
                scoreTextMesh.text = score.ToString();
            }
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
        }
    }
}
