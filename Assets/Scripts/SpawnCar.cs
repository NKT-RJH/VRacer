using UnityEngine;

public class SpawnCar : MonoBehaviour
{
	[SerializeField] private GameObject[] cars;

	[SerializeField] private Transform startPath;
	//[SerializeField] private Twins<Transform> startPathMulty;

	private void Awake()
	{
		//GameObject carToSpawn = cars[PlayData.car];

		Instantiate(cars[PlayData.car], startPath.position, startPath.rotation);
		//switch (PlayData.mode)
		//{
		//	case -1:
		//		break;
		//	case 0:
		//		Instantiate(carToSpawn, startPathSolo.position, startPathSolo.rotation);
		//		break;
		//	case 1:
		//		for (int count = 0; count < startPathMulty.GetValue().Length; count++)
		//		{
		//			Instantiate(carToSpawn, startPathMulty.GetValue()[count].position, startPathMulty.GetValue()[count].rotation);
		//		}
		//		break;
		//}
	}
}
