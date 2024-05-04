using ProtoBuf;

namespace E016_ProtoBufExtendedModel;

[ProtoContract]
public class SaveData
{
    [ProtoMember(1)] public int Health { get; set; }
}