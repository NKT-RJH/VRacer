using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MotionHouse
{
    public class MotionTest : MonoBehaviour
    {
        private float rollAngle = 0.0f;
        private float pitchAngle = 0.0f;
		int number = 26;
		int nn = 5;
        // Use this for initialization
        void Awake()
        {

            //프로그램이 시작되면 반드시 한번 호출해줍니다.
            MotionHouseSDK.MHRun();
				MotionHouseSDK.MotionControlStart();
			//MotionHouseSDK.MotionControlStart();
        }

        // Update is called once per frame
        void Update()
        {

			if (Input.GetKeyDown(KeyCode.Space))
			{
				//모션컨트롤을 시작할때 한번 호출해줍니다.
			}

			else if (Input.GetKey(KeyCode.Space))
            {
                this.rollAngle = Input.GetAxis("Horizontal") * 60.0f;
                this.pitchAngle = Input.GetAxis("Vertical") * 60.0f;

				Vector2 pos = new Vector2(1, -0.5f).normalized * number;
				//노말라이즈 최대 -26~26
				Debug.Log(pos);
				
                //MotionControlStart 이 호출되고 난 후 매프레임마다 호출해 주는 함수로 모션기기의
                //자세를 셋팅해주는 함수입니다.
                //주의 
                //MotionControlStart 함수가 호출되고 난후 이 함수가 호출되지 않으면 기기가 바닥으로 내려가 초기화 됩니다.
                MotionHouseSDK.MotionTelemetry(
                    0,               //Roll 각도 degree(2 축, 4 축, 6 축에서 지원) -28~28
                    0,                 //Pitch 각도 degree ( 2 축, 4 축 , 6 축에서 지원 ) -28~28
                    0,                          //좌우 움직임 ( 6 축에서 지원 )
                    0,                          //앞뒤 움직임 ( 6 축에서 지원 )
                    nn,                          //상하 움직임 ( 4 축 , 6 축에서 지원 )
                    0,                          //Yaw 각도 degree ( 6 축에서 지원 )
                    0                           //바람의 세기 ( Wind 장비가 있는 경우 바람의 세기로 사용됨 )
                    );
				nn *= -1;
            }

			else if (Input.GetKeyUp(KeyCode.Space))
			{
				//모션컨트롤이 종료 되면 호출해 줍니다.
				//number--;
				this.rollAngle = 0;
				this.pitchAngle = 0;

			}

		}

        private void OnApplicationQuit()
        {
			//프로그램이 종료될때 호출해줍니다.
				MotionHouseSDK.MotionControlEnd();
            MotionHouseSDK.MHStop();
        }

		GUIStyle style = new GUIStyle();
		public void OnGUI()
		{

			style.fontSize = 40;
			style.normal.textColor = Color.white;
			GUILayout.Label("스페이스 바 키를 누른 상태에서 \n방향키를 조작하여 Test 를 할 수 있습니다.", style);
			GUILayout.Label(string.Format("Roll Angle {0:F2}, Pitch Angle {1:F2}", this.rollAngle, this.pitchAngle), style);
		}
	}

}