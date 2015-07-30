using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGadgets.Open.Framework.MVVM;

namespace CodeGadgets.Open.MSDNAssist.Model
{
	[DebuggerDisplay("{FolderName}")]
	public class MonitoredFolder
	{
		public string FolderName { get; set; }
	}
}
