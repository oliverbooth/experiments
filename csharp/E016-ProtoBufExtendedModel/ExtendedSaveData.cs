using ProtoBuf;

namespace E016_ProtoBufExtendedModel;

[ProtoContract]
public class ExtendedSaveData
{
    [ProtoMember(1)] public int Health { get; set; }

    [ProtoMember(2)] public int Score { get; set; } = 50; // default value
}
