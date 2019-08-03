using UnityEngine;

public class SpawnZone : MonoBehaviour {
	public Vector3 SpawnPoint {
		get {
			return Random.insideUnitSphere * 5f;
		}
	}
}