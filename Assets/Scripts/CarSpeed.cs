using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarSpeed : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public CarController car;
	
    private void Update()
    {
        speedText.text = string.Format("{0} KM/H", (int)car.KPH);
    }
}
