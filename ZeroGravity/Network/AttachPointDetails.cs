using ProtoBuf;

namespace ZeroGravity.Network;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class AttachPointDetails
{
	public int InSceneID;

	public IAuxDetails AuxDetails = null;
}
