using System.Runtime.Serialization;

namespace PanoramicData.NCalcExtensions.Test.Models;

[DataContract]
internal sealed class Organization
{
	[DataMember(Name = "name")]
	public required string Name { get; set; }
}
