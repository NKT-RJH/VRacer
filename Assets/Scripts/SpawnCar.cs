using UnityEngine;

public class SpawnCar : MonoBehaviour
{
	[SerializeField] private GameObject[] cars;

	[SerializeField] private Transform startPath;

	private void Awake()
	{
		Instantiate(cars[PlayData.car], startPath.position, startPath.rotation);
	}
}
