using System;
namespace Red.Entities.Web
{
	public class RemoteMethodAttribute : Attribute
	{
		public string ExposedAs
		{
			get;
			private set;
		}

		public RemoteMethodAttribute(string exposedAs = null)
		{
			ExposedAs = null;
		}
	}
}
