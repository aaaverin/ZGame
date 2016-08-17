using UnityEngine;
using Configs;

namespace Core
{
	//--Плаер
	public class Player
	{
		public PlayerConfig Config = ConfigRegistry.PlayerConfig;

		public Transform transform;

		private RectTransform hpRect;
		private float maxHpRectWidth = 100;

		private ParticleSystem bullets;

		public Player(Transform transform, RectTransform hpRect, ParticleSystem bullets)
		{
			this.transform = transform;
			this.hpRect = hpRect;
			this.bullets = bullets;
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
				bullets.gameObject.SetActive (false);
				return;
			}

			var dist = Config.AttackRange;
			if (enemy.dist < dist) {
				dist = enemy.dist;
				enemy.Damage (Config.AttackDamage);
			}

			bullets.gameObject.SetActive (true);

			enemy.instance.GetComponent<SpriteRenderer> ().color = Color.black;

			Vector3 dir = enemy.T.position - transform.position;
			dir.Normalize();

			float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;//Mathf.PI;
			transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward); 

			//bullets.startLifetime = dist / 1;
		}
	}
}

