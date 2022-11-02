using UnityEngine;

public class LogitechSteeringWheel : MonoBehaviour
{
	private float damperForce = 45;

	public float DamperForce { get { return damperForce; } }

	private void Start()
    {
        LogitechGSDK.LogiSteeringInitialize(false);
    }

    private void OnApplicationQuit()
    {
        LogitechGSDK.LogiSteeringShutdown();
    }

    private void Update()
    {
        if (!(LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))) return;
		// FORCES AND EFFECTS 
		//activeForces = "Active forces and effects :\n";

		//Spring Force -> S
		//if (Input.GetKeyUp(KeyCode.S))
		//{
		//    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SPRING))
		//    {
		//        LogitechGSDK.LogiStopSpringForce(0);
		//        activeForceAndEffect[0] = "";
		//    }
		//    else
		//    {
		LogitechGSDK.LogiPlayDamperForce(0, 45);
        //        activeForceAndEffect[0] = "Spring Force\n ";
        //    }
        //}

        ////Constant Force -> C
        //if (Input.GetKeyUp(KeyCode.C))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_CONSTANT))
        //    {
        //        LogitechGSDK.LogiStopConstantForce(0);
        //        activeForceAndEffect[1] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlayConstantForce(0, 50);
        //        activeForceAndEffect[1] = "Constant Force\n ";
        //    }
        //}

        ////Damper Force -> D
        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_DAMPER))
        //    {
        //        LogitechGSDK.LogiStopDamperForce(0);
        //        activeForceAndEffect[2] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlayDamperForce(0, 50);
        //        activeForceAndEffect[2] = "Damper Force\n ";
        //    }
        //}

        ////Side Collision Force -> left or right arrow
        //if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    LogitechGSDK.LogiPlaySideCollisionForce(0, 60);
        //}

        ////Front Collision Force -> up arrow
        //if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    LogitechGSDK.LogiPlayFrontalCollisionForce(0, 60);
        //}

        ////Dirt Road Effect-> I
        //if (Input.GetKeyUp(KeyCode.I))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_DIRT_ROAD))
        //    {
        //        LogitechGSDK.LogiStopDirtRoadEffect(0);
        //        activeForceAndEffect[3] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlayDirtRoadEffect(0, 50);
        //        activeForceAndEffect[3] = "Dirt Road Effect\n ";
        //    }

        //}

        ////Bumpy Road Effect-> B
        //if (Input.GetKeyUp(KeyCode.B))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_BUMPY_ROAD))
        //    {
        //        LogitechGSDK.LogiStopBumpyRoadEffect(0);
        //        activeForceAndEffect[4] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlayBumpyRoadEffect(0, 50);
        //        activeForceAndEffect[4] = "Bumpy Road Effect\n";
        //    }

        //}

        ////Slippery Road Effect-> L
        //if (Input.GetKeyUp(KeyCode.L))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SLIPPERY_ROAD))
        //    {
        //        LogitechGSDK.LogiStopSlipperyRoadEffect(0);
        //        activeForceAndEffect[5] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlaySlipperyRoadEffect(0, 50);
        //        activeForceAndEffect[5] = "Slippery Road Effect\n ";
        //    }
        //}

        ////Surface Effect-> U
        //if (Input.GetKeyUp(KeyCode.U))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SURFACE_EFFECT))
        //    {
        //        LogitechGSDK.LogiStopSurfaceEffect(0);
        //        activeForceAndEffect[6] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlaySurfaceEffect(0, LogitechGSDK.LOGI_PERIODICTYPE_SQUARE, 50, 1000);
        //        activeForceAndEffect[6] = "Surface Effect\n";
        //    }
        //}

        ////Car Airborne -> A
        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_CAR_AIRBORNE))
        //    {
        //        LogitechGSDK.LogiStopCarAirborne(0);
        //        activeForceAndEffect[7] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlayCarAirborne(0);
        //        activeForceAndEffect[7] = "Car Airborne\n ";
        //    }
        //}

        ////Soft Stop Force -> O
        //if (Input.GetKeyUp(KeyCode.O))
        //{
        //    if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SOFTSTOP))
        //    {
        //        LogitechGSDK.LogiStopSoftstopForce(0);
        //        activeForceAndEffect[8] = "";
        //    }
        //    else
        //    {
        //        LogitechGSDK.LogiPlaySoftstopForce(0, 20);
        //        activeForceAndEffect[8] = "Soft Stop Force\n";
        //    }
        //}

        ////Play leds -> P
        //if (Input.GetKeyUp(KeyCode.P))
        //{
        //    LogitechGSDK.LogiPlayLeds(0, 20, 20, 20);
        //}

        //for (int i = 0; i < 9; i++)
        //{
        //    activeForces += activeForceAndEffect[i];
        //}
    }

	public void SetDamperForce(float value)
	{
		damperForce = value;
	}
}