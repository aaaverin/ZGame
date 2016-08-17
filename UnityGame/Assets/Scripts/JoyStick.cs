using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class JoyStick : MonoBehaviour 
{
	private SpriteRenderer back;
	[SerializeField] private Sprite backSprite;

	[SerializeField] private Sprite stickSprite; 

	private GameObject stick;

	private Vector3 initPoint;

	private Vector3 Dir
	{
		get { if (Input.GetMouseButton (0) && stick) {
				return (initPoint - Input.mousePosition);
			}
			return Vector3.zero;
		}
	}

	private void Awake()
	{
		back = GetComponent<SpriteRenderer>();
	} 

	[SerializeField]
	Transform player;

	private void Update()
	{
		var pos = player.position;
		pos -= Dir.normalized * 0.1f;
		player.position = pos;
		if (Input.GetMouseButtonDown(0))
		{
			//Vector2 mousePos = Input.mousePosition;
			if (Input.mousePosition.y < Screen.height/3)
			{
				back.sprite  = backSprite;
					initPoint = Input.mousePosition;
				
				stick = new GameObject();
				stick.name = "stick";
				stick.AddComponent<SpriteRenderer>().sprite = stickSprite;

			}
		}
		if (Input.GetMouseButton (0) && stick) 
		{
			var tp = Camera.main.ScreenToWorldPoint (initPoint);
			tp.z = 0;
			transform.position = tp;
			tp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			tp.z = 0;
			stick.transform.position = tp;
		}

		if (Input.GetMouseButtonUp(0) && stick)
		{
			Destroy(stick);
			back.sprite = null;
		}
	

	}
}
