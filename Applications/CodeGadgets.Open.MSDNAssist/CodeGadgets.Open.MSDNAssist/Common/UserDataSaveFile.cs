using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGadgets.Framework.Persistence;
using CodeGadgets.Open.MSDNAssist.Model;

namespace CodeGadgets.Open.MSDNAssist
{
	public class UserDataSaveFile : XmlSerializedSaveFileBase<UserData>
	{
		public override IEnumerable<Type> GetKnownTypes()
		{
			return base.GetKnownTypes();
		}
	}
}
