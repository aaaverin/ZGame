using UnityEngine;
using System.Collections;

public class ParticleCollisionDetector : MonoBehaviour {

	public IParticleCollisionListener listener;

	public void OnParticleCollision(GameObject other)
	{
		listener.OnParticleCollision (other);
	}
}
//--Интерфейс
public interface IParticleCollisionListener
{
	void OnParticleCollision(GameObject other);
}
