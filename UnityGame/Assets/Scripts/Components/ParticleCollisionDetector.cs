using UnityEngine;
using System.Collections;

public class ParticleCollisionDetector : MonoBehaviour {

	public PrototypeController.IParticleCollisionListener listener;

	public void OnParticleCollision(GameObject other)
	{
		listener.OnParticleCollision (other);
	}
}
