using UnityEngine;

namespace Configs
{
	public static class ConfigRegistry
	{
		public static EnemyConfig ZombieConfig = new EnemyConfig
		{
			Size = new Vector2(0.65f, 0.65f),
			Speed = 0.65f,
			MaxHp = 7.0f,
			AttackPerSecond = 1.0f,
			AttackDamage = 1.5f,
			Score = 10.0f,
			Color = new Color(53,94,0)
		};

		public static EnemyConfig MiniBossConfig = new EnemyConfig
		{
			Size = new Vector2(0.6f, 0.6f),
			Speed = 0.6f,
			MaxHp = 15.0f,
			AttackPerSecond = 3.0f,
			AttackDamage = 3.0f,
			Score = 20.0f,
			Color = new Color(53,94,59)
		};

		public static EnemyConfig BossConfig = new EnemyConfig
		{
			Size = new Vector2(1.2f, 1.2f),
			Speed = 0.75f,
			MaxHp = 40.0f,
			AttackPerSecond = 4.0f,
			AttackDamage = 4.0f,
			Score = 100.0f,
			Color = new Color(53,94,159)
		};

		public static PlayerConfig PlayerConfig = new PlayerConfig();
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
		public Color Color;
	}

	public class PlayerConfig : UnitConfig
	{
		public float AttackRange = 4.0f;
		public float AmmoSpeed = 1.5f;
		public PlayerConfig()
		{
			Size = new Vector2(0.5f, 0.5f);
			Speed = 0.75f;
			MaxHp = 10.0f;
			AttackPerSecond = 2.0f;
			AttackDamage = 1.0f;
		}
	}
}

