using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeGadgets.Open.MSDNAssist.Model
{
	[DataContract]
	public class UserData
	{
		[DataMember]
		public List<Folder> MonitoredFolders
		{
			get { return (this._MonitoredFolders) ?? (this._MonitoredFolders = new List<Folder>()); }
		}
		[DataMember]
		public string DestinationFolder { get; set; }

		private List<Folder> _MonitoredFolders;
	}
}
