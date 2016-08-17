using UnityEngine;
using Configs;

namespace Core
{
	//--Enemy
	public class Enemy : IParticleCollisionListener
	{
		float ATTACK_RANGE = 0.1f;

		public EnemyConfig Config;
		public bool DEAD = false;


		private float attackTimer = 0f;

		private Player player;

		public GameObject instance;
		private RectTransform hpRect;

		public Transform T;
		public Vector3 pos;
		public float dist;
		public Vector3 deltaPos;

		public Enemy(GameObject instance, Player player, EnemyConfig config)
		{
			this.player = player;
			this.instance = instance;
			T = instance.transform;
			hpRect = instance.GetComponentInChildren<RectTransform>();
			Config = config;
			instance.GetComponent<ParticleCollisionDetector>().listener = this;
			instance.SendMessage ("set", config.Size.x);
			T.localScale = config.Size;
			T.GetChild (0).localScale = new Vector3(1/config.Size.x,1/config.Size.y,1); 
			T.GetChild (1).localScale = new Vector3(1/config.Size.x,1/config.Size.y,1); 
		}

		public void OnParticleCollision(GameObject other)
		{
			Debug.Log ("damaged by "+other.name);
			if (other.name == "Super") {
				Debug.Log (instance.name + " contains rigidbody" +  instance.GetComponent<Rigidbody2D>());
				Rigidbody2D body = instance.GetComponent<Rigidbody2D>();
				if (body) {
					Vector3 direction = T.position - other.transform.position;
					direction = direction.normalized;
					Debug.Log (body.name + " add force");
					body.AddForce(direction * 5);
				}
			}
			Damage (20);
		}

		//mb reset?
		public void Reset()
		{

		}

		public void Attack()
		{
			attackTimer += Time.deltaTime;

			if (attackTimer > (1f / Config.AttackPerSecond)) {
				if (dist < (ATTACK_RANGE + Config.Size.x / 2 + player.Config.Size.x / 2)) {
					attackTimer -= (1f / Config.AttackPerSecond);
					player.Damage (player.Config.AttackDamage);
				}
			}
		}

		public void Damage(float damage)
		{
			var m = hpRect.offsetMin.x + 0.005f * damage;
			T.GetChild (1).GetComponent<ParticleSystem> ().Emit(1);
			if (m < hpRect.offsetMax.x)
				hpRect.offsetMin = new Vector2 (m, hpRect.offsetMin.y);
			else {

				DEAD = true;
			}

		}

		public void Move()
		{
			pos = T.position;

			var shift = Config.Speed * Time.deltaTime;

			deltaPos = player.transform.position - pos;
			dist = deltaPos.magnitude;
			if (dist < shift)
				shift = deltaPos.magnitude;

			var d = deltaPos.normalized * shift;

			T.Translate (d, Space.World);

			dist -= shift;
		}
	}
}

