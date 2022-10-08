using ProtoBuf;

namespace ZeroGravity.Network;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class CheckInResponse : NetworkData
{
	public ResponseResult Response = ResponseResult.Success;

	public string Message = "";

	public long ServerID;

	public IPAddressRange[] AdminIPAddressRanges;
}
