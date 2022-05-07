using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("CheckPoints", "enabled", "name")]
	public class ES3UserType_StageCheckPointManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_StageCheckPointManager() : base(typeof(StageCheckPointManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (StageCheckPointManager)obj;
			
			writer.WriteProperty("CheckPoints", instance.CheckPoints, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UnityEngine.GameObject>)));
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (StageCheckPointManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "CheckPoints":
						instance.CheckPoints = reader.Read<System.Collections.Generic.List<UnityEngine.GameObject>>();
						break;
					case "enabled":
						instance.enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_StageCheckPointManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_StageCheckPointManagerArray() : base(typeof(StageCheckPointManager[]), ES3UserType_StageCheckPointManager.Instance)
		{
			Instance = this;
		}
	}
}