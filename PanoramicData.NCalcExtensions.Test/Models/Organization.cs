using System.Runtime.Serialization;

namespace PanoramicData.NCalcExtensions.Test.Models;
[DataContract]
internal class Organization
{
	[DataMember(Name = "name")]
	public required string Name { get; set; }
}
