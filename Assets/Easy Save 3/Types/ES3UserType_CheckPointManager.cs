using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("W1C1", "W1C1Scenename", "W1C2", "W1C2Scenename", "W1C3", "W1C3Scenename", "W1C4", "W1C4Scenename", "W1C5", "W1C5Scenename", "W1C6", "W1C6Scenename", "W1Scenes", "W2Scenes", "W3Scenes", "AmmoAndHealth", "Value")]
	public class ES3UserType_CheckPointManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CheckPointManager() : base(typeof(CheckPointManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (CheckPointManager)obj;
			
			writer.WriteProperty("W1C1", instance.W1C1, ES3Type_int.Instance);
			writer.WriteProperty("W1C1Scenename", instance.W1C1Scenename, ES3Type_string.Instance);
			writer.WriteProperty("W1C2", instance.W1C2, ES3Type_int.Instance);
			writer.WriteProperty("W1C2Scenename", instance.W1C2Scenename, ES3Type_string.Instance);
			writer.WriteProperty("W1C3", instance.W1C3, ES3Type_int.Instance);
			writer.WriteProperty("W1C3Scenename", instance.W1C3Scenename, ES3Type_string.Instance);
			writer.WriteProperty("W1C4", instance.W1C4, ES3Type_int.Instance);
			writer.WriteProperty("W1C4Scenename", instance.W1C4Scenename, ES3Type_string.Instance);
			writer.WriteProperty("W1C5", instance.W1C5, ES3Type_int.Instance);
			writer.WriteProperty("W1C5Scenename", instance.W1C5Scenename, ES3Type_string.Instance);
			writer.WriteProperty("W1C6", instance.W1C6, ES3Type_int.Instance);
			writer.WriteProperty("W1C6Scenename", instance.W1C6Scenename, ES3Type_string.Instance);
			writer.WriteProperty("W1Scenes", instance.W1Scenes, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<System.String>)));
			writer.WriteProperty("W2Scenes", instance.W2Scenes, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<System.String>)));
			writer.WriteProperty("W3Scenes", instance.W3Scenes, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<System.String>)));
			writer.WritePropertyByRef("AmmoAndHealth", instance.AmmoAndHealth);
			writer.WriteProperty("Value", instance.Value, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (CheckPointManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "W1C1":
						instance.W1C1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "W1C1Scenename":
						instance.W1C1Scenename = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "W1C2":
						instance.W1C2 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "W1C2Scenename":
						instance.W1C2Scenename = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "W1C3":
						instance.W1C3 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "W1C3Scenename":
						instance.W1C3Scenename = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "W1C4":
						instance.W1C4 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "W1C4Scenename":
						instance.W1C4Scenename = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "W1C5":
						instance.W1C5 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "W1C5Scenename":
						instance.W1C5Scenename = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "W1C6":
						instance.W1C6 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "W1C6Scenename":
						instance.W1C6Scenename = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "W1Scenes":
						instance.W1Scenes = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "W2Scenes":
						instance.W2Scenes = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "W3Scenes":
						instance.W3Scenes = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "AmmoAndHealth":
						instance.AmmoAndHealth = reader.Read<HealthAndAmmoForStage>();
						break;
					case "Value":
						instance.Value = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_CheckPointManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CheckPointManagerArray() : base(typeof(CheckPointManager[]), ES3UserType_CheckPointManager.Instance)
		{
			Instance = this;
		}
	}
}