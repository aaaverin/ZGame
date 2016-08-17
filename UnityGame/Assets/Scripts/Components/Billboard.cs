using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
	
	// Update is called once per frame
	void LateUpdate () {
		var e = transform.rotation.eulerAngles;
		e.x = 0;
		e.y = 0;
		transform.rotation = Quaternion.Euler (e);
	}
}
