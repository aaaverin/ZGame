using UnityEngine;
using System.Collections;

public class SpriteResizer : MonoBehaviour {

	public Sprite s1;
	public Sprite s2;
	public Sprite s3;

	void set(float size)
	{
		if (size == 0.6f) {
			GetComponent<SpriteRenderer> ().sprite = s2;
		} else if (size == 0.65f) {
			GetComponent<SpriteRenderer> ().sprite = s1;
		} else 
			GetComponent<SpriteRenderer> ().sprite = s3;
	}
}
