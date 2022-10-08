using System.Collections.Generic;
using ProtoBuf;

namespace ZeroGravity.Network;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class SubscribeToObjectsRequest : NetworkData
{
	public List<long> GUIDs;
}
