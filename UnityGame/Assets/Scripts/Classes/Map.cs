using UnityEngine;

namespace Core
{
	//--Мапа
	public static class Map
	{
		static Vector2 Size = new Vector2(9,12);

		public static Vector2 GetRandom(Bounds visual)
		{
			visual.Expand (0.5f);
			Debug.Log ("visual min: " + visual.min+" max: " + visual.max);

			float x = 0;
			float y = 0;
			float minX = -4.5f;
			float minY = -6;
			bool xNoty = Random.value > 0.5;
			if (xNoty) {
				x = Random.Range (minX, Size.x + minX);
				if (visual.max.x < x || visual.min.x > x) {
					y = Random.Range (minY, Size.y+minY);
					Debug.Log ("1: "+ x+":"+y);
				} else {
					y = Random.Range (minY, Size.y+minY - visual.size.y);
					if (visual.Contains (new Vector3 (x, y, visual.center.z))) {
						y += visual.size.y;
					}
					Debug.Log ("2: "+ x+":"+y);
				}
			} else {
				y = Random.Range (minY, Size.y+minY);
				if (visual.max.y < y || visual.min.y > y) {
					x = Random.Range (minX, Size.x + minX);
					Debug.Log ("3: "+ x+":"+y);
				} else {
					x = Random.Range (minX, Size.x + minX- visual.size.x);
					if (visual.Contains (new Vector3 (x, y, visual.center.z))) {
						x += visual.size.x;
					}
					Debug.Log ("4: "+ x+":"+y);
				}
			}
			return new Vector2 (x, y);
		}
	}
}
