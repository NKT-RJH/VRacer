using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace MotionHouse
{

    public enum DeviceType : int
    {
        DEVICE_TYPE_ALL = -1,
        DEVICE_TYPE_NONE = 0,
        DEVICE_TYPE_MH_CAC_STEWART_PLATFORM_TEST,
        DEVICE_TYPE_MH_CAC_4UN_VER3_5MM,
        DEVICE_TYPE_MH_CAC_3GEAR_60,
        DEVICE_TYPE_MH_CAC_2DOF_2,
        DEVICE_TYPE_MH_CAC_2DOF_5MM,
        DEVICE_TYPE_MH_CAC_3DOF_5MM,
        DEVICE_TYPE_MH_CAC_4DOF_5MM,
        DEVICE_TYPE_MH_CAC_1DOF_PRESS,
        DEVICE_TYPE_MH_CAC_1DOF_LADDER,
        DEVICE_TYPE_MH_CAC_MOTION_2DOF,
        DEVICE_TYPE_MH_CAC_4CYLINDER_1GEAR,
        DEVICE_TYPE_MH_CAC_4UN_17BIT,
        DEVICE_TYPE_MH_AC_MOTION_3DOF_GEARS_60_750W,
        DEVICE_TYPE_MH_CAC_SURGE_TL,
        DEVICE_TYPE_MH_CAC_4UN_TL,
        DEVICE_TYPE_MH_CAC_4UN_VER3,
        DEVICE_TYPE_MH_CAC_2UN_VER3,
        DEVICE_TYPE_MH_CAC_SURGE_TL_VER2,
        DEVICE_TYPE_MH_BELT_TENSIONER,
        DEVICE_TYPE_MH_CAC_2GEAR_1GEAR,
        DEVICE_TYPE_MH_STEP_MOTION_2DOF,
        DEVICE_TYPE_MH_TORQUE_CONTROLLER,
        DEVICE_TYPE_MH_STEP_MOTION_4DOF,
        DEVICE_TYPE_SLI,
        DEVICE_TYPE_NEXTION_DISPLAY,
        DEVICE_TYPE_MH_STEP_MOTION_4DOF_TRACTION_LOSS,
        DEVICE_TYPE_MH_AC_MOTION_2DOF_FLIGHT_5MM,
        DEVICE_TYPE_MH_CAC_6AXIS_HDI,
        DEVICE_TYPE_MH_CAC_1DOF_TEST,
        DEVICE_TYPE_MH_AC_MOTION_4DOF,
        DEVICE_TYPE_MH_WIND_GENERATOR,
        DEVICE_TYPE_MH_CAC_6AXIS_RD,
        DEVICE_TYPE_MH_CAC_1AXIS_RD_STEP,
        DEVICE_TYPE_MH_AC_MOTION_6DOF,
        DEVICE_TYPE_MH_CAC_2AXIS_ACCIDENTAL_FALL,
        DEVICE_TYPE_MH_AC_MOTION_6DOF_CUSTOM,           //Treeze
        DEVICE_TYPE_MH_AC_MOTION_5DOF,
        DEVICE_TYPE_MH_AC_MOTION_4DOF_5MM,
        DEVICE_TYPE_MH_WIND_GENERATOR_V2,
        DEVICE_TYPE_MH_CAC_MOTION_4DOF,
        DEVICE_TYPE_MH_CAC_6UN,
        DEVICE_TYPE_MH_CAC_MOTION_2U,
        DEVICE_TYPE_MH_AC_MOTION_4DOF_TRACTION_LOSS,
        DEVICE_TYPE_MH_STEP_MOTION_5DOF,
        DEVICE_TYPE_MH_AC_MOTION_3DOFB,
        DEVICE_TYPE_MH_CAC_2UN_VER2,
        DEVICE_TYPE_MH_CAC_4UN_VER2,
        DEVICE_TYPE_MH_CAC_4UN_VER2_5MM,
        DEVICE_TYPE_MH_CAC_BELT_TENSIONER,
        DEVICE_TYPE_MH_AC_MOTION_3DOF_GEARS,    //M-Line old 3gear box simulator
        DEVICE_TYPE_MH_AC_MOTION_3DOF_GEARS_2,  //M-Line old 3gear box simulator, not used.
        DEVICE_TYPE_MH_AC_MOTION_3DOF_GEARS_17_BIT,
        DEVICE_TYPE_MH_AC_MOTION_3DOF_2GEARS_1CYLINDER,
        DEVICE_TYPE_MH_CAC_4DOF_GEARS_KI,
        DEVICE_TYPE_MH_CAC_3DOF_TOP_KI,
        DEVICE_TYPE_MH_CAC_3DOF_BASE_KI,
        DEVICE_TYPE_MH_CAC_4DOF_TL_5MM,
        DEVICE_TYPE_MH_CAC_STEWART_PLATFORM,

#if EXTERNAL_MOTION_CONTROLLER
        DEVICE_TYPE_EXTERNAL_MOTION,
#else
        DEVICE_TYPE_RESERVE59,
#endif

        DEVICE_TYPE_MH_CAC_6DOF_GEARS,      //Korea airforce, 6dof
        DEVICE_TYPE_MH_CAC_3UN,
        DEVICE_TYPE_MH_CAC_4UN_VER3_B,
        DEVICE_TYPE_MH_CAC_4UN_VER3_15BIT,
        DEVICE_TYPE_MH_CAC_2UN_VER3_15BIT,

        //no multiple stage device
        DEVICE_TYPE_MH_STEP_MOTION_2DOF_2U,
        DEVICE_TYPE_MH_CAC_4UN_TL_300,
        DEVICE_TYPE_MH_CAC_3CYLINDER_5_150,
        DEVICE_TYPE_MH_CAC_1DOF_FRONTIS,
        DEVICE_TYPE_MH_CAC_3DOF_5MM_ETRI,
        DEVICE_TYPE_MH_CAC_4GP,
        DEVICE_TYPE_MH_CAC_SIMG,            //Simg 3cylinder, 5mm lead, 150mm stroke
        DEVICE_TYPE_MH_TEST_MOTION,
        DEVICE_TYPE_MH_TZ_SEAT_1,
        DEVICE_TYPE_MH_TZ_SEAT_2,
        DEVICE_TYPE_MH_TZ_SEAT_3,
        DEVICE_TYPE_MH_TZ_SEAT_4,
        DEVICE_TYPE_MH_AC_MOTION_3DOF_GEARS_100_750W,
        DEVICE_TYPE_MH_CAC_3DOF_GEAR_NOVATECH,
        DEVICE_TYPE_MH_CAC_2DOF_FRONTIS,
        DEVICE_TYPE_MH_AC_MOTION_3DOF_GEARS_60_400W,
        DEVICE_TYPE_MH_SEAT_CONTROLLER,
        DEVICE_TYPE_MH_HIWIN_AC_1DOF_HYUNDAI,
        DEVICE_TYPE_MH_HIWIN_AC_3DOF_BASE_HONGIK,
        DEVICE_TYPE_MH_HIWIN_AC_4DOF_TOP_HONGIK,
        DEVICE_TYPE_MH_CAC_3DOF_5MM_MSTONE,
        DEVICE_TYPE_MH_HIWIN_AC_6DOF_GEARS,
        DEVICE_TYPE_MH_HIL_4AXIS,
        DEVICE_TYPE_MH_HIL_SURGE,
        DEVICE_TYPE_MH_HIL_SWAY_TL,

        DEVICE_TYPE_RESERVE90,
        DEVICE_TYPE_RESERVE91,
        DEVICE_TYPE_RESERVE92,
        DEVICE_TYPE_RESERVE93,
        DEVICE_TYPE_RESERVE94,
        DEVICE_TYPE_RESERVE95,
        DEVICE_TYPE_RESERVE96,
        DEVICE_TYPE_RESERVE97,
        DEVICE_TYPE_RESERVE98,
        DEVICE_TYPE_RESERVE99,
        DEVICE_TYPE_RESERVE100,
        DEVICE_TYPE_RESERVE101,
        DEVICE_TYPE_RESERVE102,
        DEVICE_TYPE_RESERVE103,
        DEVICE_TYPE_RESERVE104,
        DEVICE_TYPE_RESERVE105,
        DEVICE_TYPE_RESERVE106,
        DEVICE_TYPE_RESERVE107,
        DEVICE_TYPE_RESERVE108,
        DEVICE_TYPE_RESERVE109,
        DEVICE_TYPE_RESERVE110,
        DEVICE_TYPE_RESERVE111,
        DEVICE_TYPE_RESERVE112,
        DEVICE_TYPE_RESERVE113,
        DEVICE_TYPE_RESERVE114,
        DEVICE_TYPE_RESERVE115,
        DEVICE_TYPE_RESERVE116,
        DEVICE_TYPE_RESERVE117,
        DEVICE_TYPE_RESERVE118,
        DEVICE_TYPE_RESERVE119,
        DEVICE_TYPE_RESERVE120,
        DEVICE_TYPE_RESERVE121,
        DEVICE_TYPE_RESERVE122,
        DEVICE_TYPE_RESERVE123,
        DEVICE_TYPE_RESERVE124,
        DEVICE_TYPE_RESERVE125,
        DEVICE_TYPE_RESERVE126,
        DEVICE_TYPE_RESERVE127,
        DEVICE_TYPE_RESERVE128,
        DEVICE_TYPE_RESERVE129,
        DEVICE_TYPE_RESERVE130,
        DEVICE_TYPE_RESERVE131,
        DEVICE_TYPE_RESERVE132,
        DEVICE_TYPE_RESERVE133,
        DEVICE_TYPE_RESERVE134,
        DEVICE_TYPE_RESERVE135,
        DEVICE_TYPE_RESERVE136,
        DEVICE_TYPE_RESERVE137,
        DEVICE_TYPE_RESERVE138,
        DEVICE_TYPE_RESERVE139,
        DEVICE_TYPE_RESERVE140,
        DEVICE_TYPE_RESERVE141,
        DEVICE_TYPE_RESERVE142,
        DEVICE_TYPE_RESERVE143,
        DEVICE_TYPE_RESERVE144,
        DEVICE_TYPE_RESERVE145,
        DEVICE_TYPE_RESERVE146,
        DEVICE_TYPE_RESERVE147,
        DEVICE_TYPE_RESERVE148,
        DEVICE_TYPE_RESERVE149,
        DEVICE_TYPE_RESERVE150,

        TRACE_DEVICE_3DOF_BASE_HONGIK,
        TRACE_DEVICE_4DOF_TOP_HONGIK,
        TRACE_DEVICE_6DOF_GEARS,
        TRACE_DEVICE_CAC_2UN_VER2,
        TRACE_DEVICE_CAC_4UN_VER2,
        TRACE_DEVICE_CAC_SURGE_TL,
        TRACE_DEVICE_4DOF_GEAR_KIU,
        TRACE_DEVICE_CAC_3DOF_5MM_TOP_KIU,
        TRACE_DEVICE_CAC_3DOF_5MM_BASE_KIU,
        TRACE_DEVICE_MH_CAC_4CYLINDER_1GEAR,
        TRACE_DEVICE_CAC_3DOF_5MM_MSTONE,
        TRACE_DEVICE_CAC_STEWART_PLATFORM,

        TRACE_DEVICE_4UN_TL,
        TRACE_DEVICE_4UN_VER3,
        TRACE_DEVICE_2UN_VER3,
        TRACE_DEVICE_SURGETL_VER2,
        TRACE_DEVICE_STEWART_TEST,
        TRACE_DEVICE_4UN_17BIT,
        TRACE_DEVICE_6AXIS_HDI,
        TRACE_DEVICE_4UN_VER3_5MM,
        TRACE_DEVICE_3GEAR_60,

        TRACE_DEVICE_HIL_4AXIS,
        TRACE_DEVICE_HIL_SURGE,
        TRACE_DEVICE_HIL_SWAY_TL,

        DEVICE_TYPE_MAX,
    };

    public enum MHSafetyBeltStatus : int
    {
        BELT_STATE_INIT = 0,
        BELT_STATE_NOT_USED = 1,
        BELT_STATE_FASTEN = 2,
        BELT_STATE_LOOSE = 3,
    };

    public enum InitErrorCode : int
    {
        MHSERVICE_INIT_SUCCESS = 0,
        GAME_MANAGER_INIT_FAIL = 1,
        DEVICE_MANAGER_INIT_FAIL = 2,
        MEMORY_ALLOC_FAIL = 3,
        DLL_LOAD_FAIL = 4,
        NETWORK_MANAGER_INIT_FAIL = 5,
        INVALID_SDK_PATH = 6,
        MOTIONHOUSE_SDK_ALREADY_INITIALIZED = 7,
        MOTIONHOUSE_SDK_ALREADY_DESTROYED = 8,
        MOTIONHOUSE_SDK_NOT_INITIALIZED = 9,
        EXCEPTION_OCCUR_IN_INITIALIZING = 10,
        DEVICE_PARAM_UPDATED_REQUEST_ALL_DEVICE_POWER_RESTART = 11,
    };

    public enum TraceState : int
    {
        TRACE_STATE_NOT_FOUNDED = -1,
        TRACE_STATE_INIT = 0,
        TRACE_STATE_REQUEST_SYNC_DONE,
        TRACE_STATE_REQUEST_GET_RANGE_INFO,
        TRACE_STATE_GOT_RANGE_INFO,
        TRACE_STATE_GETTING_TRACE,
        TRACE_STATE_ERROR_OCCURED,
    };

    public enum DCCC_State : int
    {
        DCCC_STATE_NOT_USED = 0x100,
        DCCC_STATE_WAIT_COMMAND = 0,

        DCCC_STATE_MOVE_TOP_START,
        DCCC_STATE_MOVE_UNTIL_TOP_SENSOR_DETECT,
        DCCC_STATE_TOP_STOPPED,

        DCCC_STATE_MOVE_BOTTOM_START,
        DCCC_STATE_MOVE_UNTIL_BOTTOM_SENSOR_DETECT,
        DCCC_STATE_BOTTOM_STOPPED,
    };

    public enum DCCC_Command : int
    {
        DCCC_COMMAND_NONE = 0,
        DCCC_COMMAND_MOVE_TOP,
        DCCC_COMMAND_MOVE_BOTTOM,
    };

    public enum SeatAxisIndex : int
    {
        NONE_AXIS = -1,
        SLIDE_AXIS = 0,
        RECLINE_AXIS,
        SEAT_FRONT_AXIS,
        SEAT_REAR_AXIS,
        //EXT_AXIS,
        MAX_SEAT_AXIS
    };


    public enum AXIS_CONTROL_MODE
    {
        TELEMETRY_CONTROL_MODE = 0,
        AXIS_CONTROL_MODE = 1,
        ALWAYS_CONTROL_MODE = 2,
        PRELOAD_MOTION = 3,
        AXIS_POSITION_CONTROL_MODE = 4,
    };



    unsafe public struct TargetAxisPosition
    {
        public fixed float m_OffsetFromCenter[8];      //   8 -> max 8 axis, Unit : cm
        public int m_VibrationFreq;            //   1~20 Hz
        public fixed float m_VibrationDisplacement[8];   //   Unit : cm
        public int m_VibDuration;
    }

    unsafe public struct AxisPositionCheck
    {
        public fixed float m_OffsetFromCenter[8];
    }

    public class MotionHouseSDK
    {

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MHRun();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MHStop();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMHSDKVersion();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetUsageTime(int index);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSerialNumber(int index);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetProductDate(int index);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDeviceVersion(int index);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetConnectedDeviceCount();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDeviceStatus(int index);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetConnectedDeviceType(int index);



        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MotionControlStart();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MotionControlEnd();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetCenterCode(int deviceType);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetMotionDeviceConfig(
           bool m_UseFlag,
           int m_DeviceType,
           int m_DeviceControlDelay,

           double m_MaxRoll,
           double m_MaxPitch,
           double m_MaxTractionLoss,
           double m_MaxSway,
           double m_MaxSurge,
           double m_MaxHeave
        );


        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetWindDeviceConfig(
           bool m_UseFlag,
           int m_DeviceType,
           int m_DeviceControlDelay
        );

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetMotionProfile(
           int m_DeviceType,
            bool m_UseFlag,

            bool m_UseRoll,
            int m_RollWeight,

            bool m_UsePitch,
            int m_PitchWeight,

            bool m_UseSway,
            int m_SwayWeight,

            bool m_UseSurge,
            int m_SurgeWeight,

            bool m_UseHeave,
            int m_HeaveWeight,
            bool m_UseTractionLoss,
            int m_TractionLossWeight,

            int m_ActuratorAccel,
            int m_ActuratorMaxSpeed,
            int m_ActuratorVibration,
            int m_ActuratorSmooth
        );

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IsWindDeviceType(int deviceType);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IsMotionDeviceType(int deviceType);


        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetMotionFilter(
           int m_DeviceType,
           int m_RollFilter,
           int m_PitchFilter,
           int m_SwayFilter,
           int m_SurgeFilter,
           int m_HeaveFilter,
           int m_TLFilter
        );

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetWindProfile(
           int m_DeviceType,
           bool m_UseFlag,
           int m_BaseOffset,
           int m_Min,
           int m_Max,
           int m_MaxPower
        );


        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetDllMotionTelemetry(int motionState, float roll, float pitch, float sway, float surge, float heave, float yaw, float wind);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void SetSDKPath(string path);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RequestReadyState();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsReadyState();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RequestSleepState();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsSleepState();


        //[DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        //public static extern bool SetDllMotionTelemetryEx(MHTelemetryInfoInterface info);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSafetyBeltStatus();


        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetMotionProfileTreeze(
           int m_DeviceType,
           int m_UseFlag,

           int m_UseMinusSurge,
           float m_MinusSurge,

           int m_UseRoll,
           int m_RollWeight,

           int m_UsePitch,
           int m_PitchWeight,

           int m_UseSway,
           int m_SwayWeight,

           int m_UseSurge,
           int m_SurgeWeight,

           int m_UseHeave,
           int m_HeaveWeight,

           int m_UseSuspension,
           int m_SuspensionWeight,

           int m_UseTractionLoss,
           int m_TractionLossWeight,

           int m_ActuratorAccel,
           int m_ActuratorMaxSpeed,
           int m_ActuratorVibration,
           int m_ActuratorSmooth
        );

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartTelemetryLogging();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int StopTelemetryLogging();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearDeviceErrorCode(int index, int errorCode);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool SetIniPath(string path);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetModuleUseFlag(int index, int flag);


        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetDCCCCommand(int command);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDCCCState();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSeatPositionControllerState();

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSeatPosition(int slide, int recline, int seatFront, int seatRear);


        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAxisControlMode(int deviceType, int use);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAxisPositions(int deviceType, TargetAxisPosition data);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAxisPositionVibration(int deviceType, TargetAxisPosition data);


        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAxisCount(int deviceType);



        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CheckAxisPositions(int deviceType, AxisPositionCheck data);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MotionDeviceStart(int deviceType);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MotionDeviceEnd(int deviceType);

        [DllImport("MotionHouseSDK_x64", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HoldPosition(int deviceType, int flag);

        ///////////////////////////////////////////////////////////////////////////////////











        /// <summary>
        /// 모션의 움직임을 셋팅
        /// </summary>
        /// <param name="roll">Roll 각도 degree ( 2 축, 4 축 , 6 축에서 지원 )</param>
        /// <param name="pitch">Pitch 각도 degree ( 2 축, 4 축 , 6 축에서 지원 )</param>
        /// <param name="sway">좌우 움직임 ( 6 축에서 지원 )</param>
        /// <param name="surge">앞뒤 움직임 ( 6 축에서 지원 )</param>
        /// <param name="heave">상하 움직임 ( 4 축 , 6 축에서 지원 )</param>
        /// <param name="tractionloss">Yaw 각도 degree ( 6 축에서 지원 )</param>
        /// <param name="speed">바람의 세기 ( Wind 장비가 있는 경우 바람의 세기로 사용됨 )</param>
        public static void MotionTelemetry(float roll, float pitch, float sway, float surge, float heave, float tractionloss, float speed)
        {
            SetDllMotionTelemetry(
                1,
                GetAngle180Value(roll) * Mathf.Deg2Rad,
                GetAngle180Value(pitch) * Mathf.Deg2Rad,
                sway,
                surge,
                heave,
                GetAngle180Value(tractionloss) * Mathf.Deg2Rad,
                speed);

        }

        /// <summary>
        /// 각도의 값을 -180 ~ 180 도 값사이도 얻는다.
        /// </summary>
        /// <param name="angle">현제 각도 값</param>
        /// <returns></returns>
        private static float GetAngle180Value(float angle)
        {
            float value = angle % 360.0f;

            if (value > 180.0f)
                value -= 360.0f;
            else if (value < -180.0f)
                value += 360.0f;

            return value;
        }





        #region Trace

        //
        // Trace 추가 ( 2021 04 20 )
        //

        [DllImport("MotionHouseSDK_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDeviceTraceState(int deviceType);

        [DllImport("MotionHouseSDK_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDeviceTraceTime(int deviceType);

        [DllImport("MotionHouseSDK_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int GetDeviceTraceRangeInfo(int deviceType, IntPtr minPositions, IntPtr maxPositions);

        [DllImport("MotionHouseSDK_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int GetDeviceTraceTargetPositions(int deviceType, IntPtr positions);

        [DllImport("MotionHouseSDK_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int GetDeviceTracePulsePositions(int deviceType, IntPtr positions);

        [DllImport("MotionHouseSDK_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int GetDeviceTraceEncoderPositions(int deviceType, IntPtr positions);

        #endregion
    }


}