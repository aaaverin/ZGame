using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

using Core;
using Configs;

public class PrototypeController : MonoBehaviour
{
	[SerializeField]
	UnityEngine.UI.Text text;

	[SerializeField]
	Canvas canvas;

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
				boss = CreateEnemy (ConfigRegistry.BossConfig);
				enemies.Add (boss);
			} else {
				if (miniBosses.Count < 3 && Random.value > 0.3f) {
					var miniBoss = CreateEnemy (ConfigRegistry.MiniBossConfig);
					miniBosses.Add (miniBoss);
					enemies.Add (miniBoss);
				} else {
					enemies.Add(CreateEnemy (ConfigRegistry.ZombieConfig));
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

	void OnEnable()
	{
		beforeBossScore = 0;
		score = 0;
		text.text = score.ToString();
		enemies = new List<Enemy> ();
		miniBosses = new List<Enemy> ();
		boss = null;
	}

	void AddScore(int score)
	{
		if (boss == null) {
			beforeBossScore += score;
		}
		this.score += score;

		text.text = this.score.ToString();
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
		if (playerAttackTimer > 1 / P.Config.AttackPerSecond) {
			playerAttackTimer -= 1 / P.Config.AttackPerSecond;
			PlayerAttack ();
			P.Attack (targetEnemy);
		}

		if (boss != null && boss.DEAD) {
			boss = null;
		}

		enemies.ForEach (x => {
			if (x.DEAD) {
				if (targetEnemy == x)
					targetEnemy = null;
				Destroy (x.instance);
				AddScore ((int)x.Config.Score);
			}
				
		});
		enemies.RemoveAll(x=>x.DEAD);
		miniBosses.RemoveAll (x => x.DEAD);

	}
}
