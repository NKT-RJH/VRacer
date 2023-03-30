using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
	protected virtual void Awake()
	{
		T[] objects = FindObjectsOfType(typeof(T)) as T[];

		if (objects.Length == 1)
		{
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}
}
