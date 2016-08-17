using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]
public class AlternativeFire : MonoBehaviour {

	float t = 0;

	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (Input.GetMouseButtonDown (2)) {
			Fire ();
		}
	}

	public void Fire()
	{
		if (t < 10)
			return;
		t = 0;

		float x = -35;

		float step = 70.0f/5;

		for (int i = 0; i < 5; i++) {
			SetAngle (x);
			Play ();
			x += step;
		}

	}

	void Play()
	{
		GetComponent<ParticleSystem> ().Emit (1);
	}

	void SetAngle(float x) {
		var r = transform.localEulerAngles;
		r.x = x;
		transform.localEulerAngles = r;
	}

	void OnGUI()
	{
		if (Event.current.isMouse && Event.current.clickCount == 2)
			Fire ();
		
	}
}
