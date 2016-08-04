using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class PrototypeController : MonoBehaviour
{
	//--Интерфейс
	public interface IParticleCollisionListener
	{
		void OnParticleCollision(GameObject other);
	}


	//--Конфиги
	public class UnitConfig
	{
		public Vector2 Size;
		public float Speed;
		public float MaxHp;
		public float AttackPerSecond;
		public float AttackDamage;  
	}

	public class EnemyConfig : UnitConfig
	{
		public float Score;
	}

	public class PlayerConfig : UnitConfig
	{
		public PlayerConfig()
		{
			Size = new Vector2(0.5f, 0.5f);
			Speed = 0.75f;
			MaxHp = 10.0f;
			AttackPerSecond = 2.0f;
			AttackDamage = 1.0f;
		}
		public float AttackRange = 4.0f;
		public float AmmoSpeed = 1.5f;
	}


	//--Мапа
	public class Map
	{
		static Vector2 Size = new Vector2(9,12);
	
		public static Vector2 GetRandom(Bounds visual)
		{
			float x = 0;
			float y = 0;
			float minX = -4.5f;
			float minY = -6;
			bool xNoty = Random.value > 0.5;
			if (xNoty) {
				x = Random.Range (minX, Size.x + minX);
				if (visual.max.x < x || visual.min.x > x) {
					y = Random.Range (minY, Size.y+minY);
				} else {
					y = Random.Range (minY, Size.y+minY - visual.size.y);
				}
			} else {
				y = Random.Range (minY, Size.y+minY);
				if (visual.max.y < y || visual.min.y > y) {
					x = Random.Range (minX, Size.x + minX);
				} else {
					x = Random.Range (minX, Size.x + minX- visual.size.x);
				}
			}
			return new Vector2 (x, y);
		}
	}


	//--Плаер
	public class Player
	{
		public PlayerConfig Config = new PlayerConfig();

		public Transform transform;
		RectTransform hpRect;

		float maxHpRectWidth = 0;

		private ParticleSystem ps;

		public Player(Transform transform, RectTransform hpRect, ParticleSystem ps)
		{
			this.transform = transform;
			this.hpRect = hpRect;
			this.ps = ps;
		}

		public void Damage(float damage)
		{
			float damagePercent = damage / Config.MaxHp;
			hpRect.offsetMin = new Vector2(hpRect.offsetMin.x + damagePercent, hpRect.offsetMin.y);
			float damageRectPercent = damagePercent * maxHpRectWidth;
			hpRect.rect.Set(hpRect.rect.x + damageRectPercent/2, hpRect.rect.y, hpRect.rect.width - damageRectPercent, hpRect.rect.height);
		}

		public void Attack(Enemy enemy)
		{
			if (enemy == null) {
				ps.gameObject.SetActive (false);
				return;
			}

			var dist = Config.AttackRange;
			if (enemy.dist < dist) {
				dist = enemy.dist;
				enemy.Damage (Config.AttackDamage);
			}

			ps.gameObject.SetActive (true);
			transform.LookAt (enemy.t.position);
			ps.startLifetime = dist / 1;
		}
	}


	//--Enemy
	public class Enemy : IParticleCollisionListener
	{
		const float ATTACK_RANGE = 0.1f;
		public EnemyConfig Config;

		float attackTimer = 0f;

		public bool DEAD = false;

		public Player player;
		public GameObject go;
		public Transform t;
		public Vector3 pos;
		public float dist;
		public Vector3 deltaPos;
		public RectTransform hpRect;

		public Enemy(GameObject e, Player p, EnemyConfig config)
		{
			player = p;
			go = e;
			t = go.transform;
			hpRect = e.GetComponentInChildren<RectTransform>();
			Config = config;
			go.GetComponent<ParticleCollisionDetector>().listener = this;
		}

		public void OnParticleCollision(GameObject other)
		{
			Debug.Log ("damaged");
			Damage (500);
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
			if (m < hpRect.offsetMax.x)
				hpRect.offsetMin = new Vector2 (m, hpRect.offsetMin.y);
			else {
				
				DEAD = true;
			}

		}

		public void Move()
		{
			pos = t.position;

			var shift = Config.Speed * Time.deltaTime;

			deltaPos = player.transform.position - pos;
			dist = deltaPos.magnitude;
			if (dist < shift)
				shift = deltaPos.magnitude;

			var d = deltaPos.normalized * shift;

			t.Translate (d, Space.World);

			dist -= shift;
		}
	}

	[SerializeField]
	Canvas canvas;

	EnemyConfig zombieConfig = new EnemyConfig
	{
		Size = new Vector2(0.65f, 0.65f),
		Speed = 0.65f,
		MaxHp = 7.0f,
		AttackPerSecond = 1.0f,
		AttackDamage = 1.5f,
		Score = 10.0f
	};

	EnemyConfig miniBossConfig = new EnemyConfig
	{
		Size = new Vector2(0.6f, 0.6f),
		Speed = 0.6f,
		MaxHp = 15.0f,
		AttackPerSecond = 3.0f,
		AttackDamage = 3.0f,
		Score = 20.0f
	};

	EnemyConfig bossConfig = new EnemyConfig
	{
		Size = new Vector2(1.2f, 1.2f),
		Speed = 0.75f,
		MaxHp = 40.0f,
		AttackPerSecond = 4.0f,
		AttackDamage = 4.0f,
		Score = 100.0f
	};

	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private Transform playerTransform;

	[SerializeField] private RectTransform playerHpRectTransform;

	[SerializeField] private ParticleSystem playerAttackParticleSystem;

	const int MaxEnemyCount = 10;
	const int BossScoreLimit = 150;
	const int GameFinishScore = 700;

	private List<Enemy> enemies = new List<Enemy>();

	Bounds vision = new Bounds();

	Player P;

	/*
	 * 0.3f - miniboss < 3
	 * 150 points - boss
	 */ 

	List<Enemy> miniBosses = new List<Enemy>();
	Enemy boss = null;
	int beforeBossScore = 0;
	int score;

	void Spawn()
	{
		while (enemies.Count < MaxEnemyCount) {
			if (beforeBossScore > BossScoreLimit) {
				beforeBossScore = 0;
				boss = CreateEnemy (bossConfig);
				enemies.Add (boss);
			} else {
				if (miniBosses.Count < 3 && Random.value > 0.3f) {
					var miniBoss = CreateEnemy (miniBossConfig);
					miniBosses.Add (miniBoss);
					enemies.Add (miniBoss);
				} else {
					enemies.Add(CreateEnemy (zombieConfig));
				}
			}
		}
	}

	Enemy CreateEnemy(EnemyConfig config)
	{
		var go = Instantiate (enemyPrefab);
		go.transform.position = Map.GetRandom (vision);
		go.transform.SetParent (canvas.transform, false);
		return new Enemy (go, P, config);
	}

	//30x22.5

	float playerAttackTimer = 0f;


	Enemy targetEnemy;

	// Use this for initialization
	void Start ()
	{
		P = new Player (playerTransform, playerHpRectTransform, playerAttackParticleSystem);

		var min = (Vector2)Camera.main.ViewportToWorldPoint (Vector3.zero);
		var max = (Vector2)Camera.main.ViewportToWorldPoint (Vector3.one);

		vision.SetMinMax (min, max);
	}

	void PlayerAttack(){
		
		if (targetEnemy == null && enemies.Count > 0) {
			targetEnemy = enemies.Aggregate((min,x) => (min==null || min.dist>x.dist)?x:min);
		}
		/*
		if (targetEnemy != null) {
			var target = enemies.Where (x => Vector3.Angle (targetEnemy.deltaPos, x.deltaPos) < 10.0f).Aggregate((min,x) => (min==null || min.dist>x.dist)?x:min);
			Debug.Log ("ATTACK");
			p.Attack (target);

		}*/
	}



	void Update ()
	{
		
		vision.center = playerTransform.position;

		Spawn ();

		enemies.ForEach (x => {
			x.Move();
			x.Attack();
		});

		playerAttackTimer += Time.deltaTime;
		if (playerAttackTimer > 1/P.Config.AttackPerSecond) {
			playerAttackTimer -= 1/P.Config.AttackPerSecond;
			PlayerAttack ();
			P.Attack (targetEnemy);

			if (targetEnemy != null && targetEnemy.DEAD) {
				enemies.Remove (targetEnemy);
				Destroy (targetEnemy.go);
				targetEnemy = null;
			}
		}
	}
}
