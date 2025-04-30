using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

//.LDB FILES
// TODO: Verify these values.
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LocomotionDb>))]
public class LocomotionDb
{
    [MetaMember("maAnimInfoList")]
    public Dictionary<string, AnimationInfo> AnimInfoList { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AnimationInfo>))]
    public class AnimationInfo
    {
        [MetaMember("mu64TimeStamp")]
        public ulong TimeStamp { get; set; }

        [MetaMember("mzName")]
        public string Name { get; set; }

        [MetaMember("meCategory")]
        public Category Category { get; set; }

        [MetaMember("mfDuration")]
        public float Duration { get; set; }

        [MetaMember("mfStartSpeed")]
        public float StartSpeed { get; set; }

        [MetaMember("mfEndSpeed")]
        public float EndSpeed { get; set; }

        [MetaMember("mfTurnAngle")]
        public float TurnAngle { get; set; }

        [MetaMember("mfMoveDistance")]
        public float MoveDistance { get; set; }

        [MetaMember("mbMoveStart")]
        public bool MoveStart { get; set; }

        [MetaMember("mbMoveStop")]
        public bool MoveStop { get; set; }

        [MetaMember("mbMove")]
        public bool Move { get; set; }

        [MetaMember("mbTurnLeft")]
        public bool TurnLeft { get; set; }

        [MetaMember("mbTurnRight")]
        public bool TurnRight { get; set; }

        [MetaMember("mbTurn")]
        public bool Turn { get; set; }
    }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<Category>))]
public enum Category
{
    // Category_
    Idle = 0,
    Start = 1,
    Move = 2,
    Stop = 3,
    Turn = 4,
    TurnAndStart = 5,
    MoveAndTurn = 6,
    StopAndTurn = 7,
    Unknown = 8,
    Number = 9,
    None = -1
}