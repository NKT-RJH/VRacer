using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarSpeed : MonoBehaviour
{
    private TextMeshProUGUI KPHText;
    private Transform KPHPos;
    private CarController car;

    private void Start()
    {
        KPHText = GetComponent<TextMeshProUGUI>();
        KPHPos = GameObject.Find("KPHPos").transform;
        car = FindObjectOfType<CarController>();
    }

    private void Update()
    {
        Vector3 Dir = Camera.main.WorldToScreenPoint(KPHPos.position) - KPHText.transform.position;
        KPHText.transform.Translate(Dir);
        KPHText.text = string.Format("{0} KM/H", (int)car.KPH);
    }
}
