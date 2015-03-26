using System;
using Helper.Timing;
using SharpDX;

namespace Helper
{
    public enum TriggerType
    {
        Door,
        Elevator,
        Teleport,
        Lever,
    }
    public enum TriggerState
    {
        Inactive,
        Active,
    } 

    public class Trigger
    {
        public Interval Duration;
        public Interval Cooldown;
        public Boolean Enabled;
        public Int32 EndAngle;
        public TriggerState InitialState;
        public Boolean IsFromValhalla;
        public Int32 MaxAngleRate;
        public Int32 MaxRate;
        public Int32 MoveCeiling;
        public Int32 MoveFloor;
        public Int32 MoveRooftop;
        public Int32 NextTrigger;
        public Int32 NextTriggerTiming;
        public Int32 OffHeight;
        public Int32 OffSound;
        public String OffText;
        public Int32 OnHeight;
        public Int32 OnSound;
        public String OnText;
        public Vector3 Position;
        public Int32 Random;
        public Int32 ResetTimer;
        public Int32 SlideAmount;
        public Int32 SlideAxis;
        public Int32 Speed;
        public Int32 StartAngle;
        public TriggerState CurrentState;
        public Int32 Team;
        public Int32 TextureOff;
        public Int32 TextureOn;
        public Int16 TriggerId;
        public TriggerType TriggerType;
        public Int32 X0;
        public Int32 X1;
        public Int32 X2;
        public Int32 X3;
        public Int32 X4;
        public Int32 Y0;
        public Int32 Y1;
        public Int32 Y2;
        public Int32 Y3;
        public Int32 Y4;
    }

}
