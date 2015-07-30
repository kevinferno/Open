using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeGadgets.Open.MSDNAssist.Model
{
	[DataContract]
	public class UserData
	{
		[DataMember]
		public List<MonitoredFolder> MonitoredFolders
		{
			get { return (this._MonitoredFolders) ?? (this._MonitoredFolders = new List<MonitoredFolder>()); }
		}

		private List<MonitoredFolder> _MonitoredFolders;
	}
}
