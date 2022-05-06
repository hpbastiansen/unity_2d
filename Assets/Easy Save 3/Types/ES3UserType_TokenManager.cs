using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("TokenUI", "DefaultToken", "DefaultTokenActive", "CactusToken", "CactusTokenActive", "RevloverToken", "RevloverTokenActive", "WormToken", "WormTokenActive", "TokenUIactive", "TokensOwned", "TokensActive", "_myUIManager", "ShortInfo", "DashInfo", "ShieldInfo", "BulletInfo", "CounterInfo", "MovementInfo", "ShortInfoText", "DashInfoText", "ShieldInfoText", "BulletInfoText", "CounterInfoText", "MovementInfoText", "_uiManager", "CustomPlayerMoveSpeed", "CustomPlayerJumpHeight", "PlayerMovement", "GunLifeStealAmount", "CurrentWeapon", "CustomDashSpeed", "CustomDashDuration", "CustomDashCooldown", "CustomBlockLifeSteal", "CustomBlockLifeStealCooldown", "CustomBlockLifeStealActiveTime", "_playerHealth", "ShieldLifeSteal", "_shieldHP", "_weaponAccuracy", "TokenIndex", "UsingTokenMenu", "_spacechar", "_checkUI", "CactusSplinter", "WeaponControllerScript", "ReadyToGiveToken", "CactiDestoyed", "ShrubsDestoyed", "enabled", "name")]
	public class ES3UserType_TokenManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_TokenManager() : base(typeof(TokenManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (TokenManager)obj;
			
			writer.WritePropertyByRef("TokenUI", instance.TokenUI);
			writer.WritePropertyByRef("DefaultToken", instance.DefaultToken);
			writer.WriteProperty("DefaultTokenActive", instance.DefaultTokenActive, ES3Type_bool.Instance);
			writer.WritePropertyByRef("CactusToken", instance.CactusToken);
			writer.WriteProperty("CactusTokenActive", instance.CactusTokenActive, ES3Type_bool.Instance);
			writer.WritePropertyByRef("RevloverToken", instance.RevloverToken);
			writer.WriteProperty("RevloverTokenActive", instance.RevloverTokenActive, ES3Type_bool.Instance);
			writer.WritePropertyByRef("WormToken", instance.WormToken);
			writer.WriteProperty("WormTokenActive", instance.WormTokenActive, ES3Type_bool.Instance);
			writer.WriteProperty("TokenUIactive", instance.TokenUIactive, ES3Type_bool.Instance);
			writer.WriteProperty("TokensOwned", instance.TokensOwned, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UnityEngine.GameObject>)));
			writer.WriteProperty("TokensActive", instance.TokensActive, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<System.Boolean>)));
			writer.WritePrivateFieldByRef("_myUIManager", instance);
			writer.WriteProperty("ShortInfo", instance.ShortInfo, ES3Type_string.Instance);
			writer.WriteProperty("DashInfo", instance.DashInfo, ES3Type_string.Instance);
			writer.WriteProperty("ShieldInfo", instance.ShieldInfo, ES3Type_string.Instance);
			writer.WriteProperty("BulletInfo", instance.BulletInfo, ES3Type_string.Instance);
			writer.WriteProperty("CounterInfo", instance.CounterInfo, ES3Type_string.Instance);
			writer.WriteProperty("MovementInfo", instance.MovementInfo, ES3Type_string.Instance);
			writer.WritePropertyByRef("ShortInfoText", instance.ShortInfoText);
			writer.WritePropertyByRef("DashInfoText", instance.DashInfoText);
			writer.WritePropertyByRef("ShieldInfoText", instance.ShieldInfoText);
			writer.WritePropertyByRef("BulletInfoText", instance.BulletInfoText);
			writer.WritePropertyByRef("CounterInfoText", instance.CounterInfoText);
			writer.WritePropertyByRef("MovementInfoText", instance.MovementInfoText);
			writer.WritePrivateFieldByRef("_uiManager", instance);
			writer.WriteProperty("CustomPlayerMoveSpeed", instance.CustomPlayerMoveSpeed, ES3Type_float.Instance);
			writer.WriteProperty("CustomPlayerJumpHeight", instance.CustomPlayerJumpHeight, ES3Type_float.Instance);
			writer.WritePrivateFieldByRef("PlayerMovement", instance);
			writer.WriteProperty("GunLifeStealAmount", instance.GunLifeStealAmount, ES3Type_float.Instance);
			writer.WritePropertyByRef("CurrentWeapon", instance.CurrentWeapon);
			writer.WriteProperty("CustomDashSpeed", instance.CustomDashSpeed, ES3Type_float.Instance);
			writer.WriteProperty("CustomDashDuration", instance.CustomDashDuration, ES3Type_float.Instance);
			writer.WriteProperty("CustomDashCooldown", instance.CustomDashCooldown, ES3Type_float.Instance);
			writer.WriteProperty("CustomBlockLifeSteal", instance.CustomBlockLifeSteal, ES3Type_int.Instance);
			writer.WriteProperty("CustomBlockLifeStealCooldown", instance.CustomBlockLifeStealCooldown, ES3Type_int.Instance);
			writer.WriteProperty("CustomBlockLifeStealActiveTime", instance.CustomBlockLifeStealActiveTime, ES3Type_int.Instance);
			writer.WritePrivateFieldByRef("_playerHealth", instance);
			writer.WriteProperty("ShieldLifeSteal", instance.ShieldLifeSteal, ES3Type_float.Instance);
			writer.WritePrivateFieldByRef("_shieldHP", instance);
			writer.WritePrivateField("_weaponAccuracy", instance);
			writer.WriteProperty("TokenIndex", instance.TokenIndex, ES3Type_int.Instance);
			writer.WriteProperty("UsingTokenMenu", instance.UsingTokenMenu, ES3Type_bool.Instance);
			writer.WritePrivateField("_spacechar", instance);
			writer.WritePrivateFieldByRef("_checkUI", instance);
			writer.WritePropertyByRef("CactusSplinter", instance.CactusSplinter);
			writer.WritePropertyByRef("WeaponControllerScript", instance.WeaponControllerScript);
			writer.WriteProperty("ReadyToGiveToken", instance.ReadyToGiveToken, ES3Type_bool.Instance);
			writer.WriteProperty("CactiDestoyed", instance.CactiDestoyed, ES3Type_int.Instance);
			writer.WriteProperty("ShrubsDestoyed", instance.ShrubsDestoyed, ES3Type_int.Instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (TokenManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "TokenUI":
						instance.TokenUI = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "DefaultToken":
						instance.DefaultToken = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "DefaultTokenActive":
						instance.DefaultTokenActive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "CactusToken":
						instance.CactusToken = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "CactusTokenActive":
						instance.CactusTokenActive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "RevloverToken":
						instance.RevloverToken = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "RevloverTokenActive":
						instance.RevloverTokenActive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "WormToken":
						instance.WormToken = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "WormTokenActive":
						instance.WormTokenActive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "TokenUIactive":
						instance.TokenUIactive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "TokensOwned":
						instance.TokensOwned = reader.Read<System.Collections.Generic.List<UnityEngine.GameObject>>();
						break;
					case "TokensActive":
						instance.TokensActive = reader.Read<System.Collections.Generic.List<System.Boolean>>();
						break;
					case "_myUIManager":
					reader.SetPrivateField("_myUIManager", reader.Read<UIManager>(), instance);
					break;
					case "ShortInfo":
						instance.ShortInfo = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "DashInfo":
						instance.DashInfo = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "ShieldInfo":
						instance.ShieldInfo = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "BulletInfo":
						instance.BulletInfo = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "CounterInfo":
						instance.CounterInfo = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "MovementInfo":
						instance.MovementInfo = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "ShortInfoText":
						instance.ShortInfoText = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "DashInfoText":
						instance.DashInfoText = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "ShieldInfoText":
						instance.ShieldInfoText = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "BulletInfoText":
						instance.BulletInfoText = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "CounterInfoText":
						instance.CounterInfoText = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "MovementInfoText":
						instance.MovementInfoText = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "_uiManager":
					reader.SetPrivateField("_uiManager", reader.Read<UIManager>(), instance);
					break;
					case "CustomPlayerMoveSpeed":
						instance.CustomPlayerMoveSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "CustomPlayerJumpHeight":
						instance.CustomPlayerJumpHeight = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "PlayerMovement":
					reader.SetPrivateField("PlayerMovement", reader.Read<Movement>(), instance);
					break;
					case "GunLifeStealAmount":
						instance.GunLifeStealAmount = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "CurrentWeapon":
						instance.CurrentWeapon = reader.Read<Weapon>();
						break;
					case "CustomDashSpeed":
						instance.CustomDashSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "CustomDashDuration":
						instance.CustomDashDuration = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "CustomDashCooldown":
						instance.CustomDashCooldown = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "CustomBlockLifeSteal":
						instance.CustomBlockLifeSteal = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "CustomBlockLifeStealCooldown":
						instance.CustomBlockLifeStealCooldown = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "CustomBlockLifeStealActiveTime":
						instance.CustomBlockLifeStealActiveTime = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "_playerHealth":
					reader.SetPrivateField("_playerHealth", reader.Read<PlayerHealth>(), instance);
					break;
					case "ShieldLifeSteal":
						instance.ShieldLifeSteal = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "_shieldHP":
					reader.SetPrivateField("_shieldHP", reader.Read<ShieldHP>(), instance);
					break;
					case "_weaponAccuracy":
					reader.SetPrivateField("_weaponAccuracy", reader.Read<System.String>(), instance);
					break;
					case "TokenIndex":
						instance.TokenIndex = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "UsingTokenMenu":
						instance.UsingTokenMenu = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_spacechar":
					reader.SetPrivateField("_spacechar", reader.Read<System.String>(), instance);
					break;
					case "_checkUI":
					reader.SetPrivateField("_checkUI", reader.Read<UITest>(), instance);
					break;
					case "CactusSplinter":
						instance.CactusSplinter = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "WeaponControllerScript":
						instance.WeaponControllerScript = reader.Read<WeaponController>();
						break;
					case "ReadyToGiveToken":
						instance.ReadyToGiveToken = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "CactiDestoyed":
						instance.CactiDestoyed = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "ShrubsDestoyed":
						instance.ShrubsDestoyed = reader.Read<System.Int32>(ES3Type_int.Instance);
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


	public class ES3UserType_TokenManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TokenManagerArray() : base(typeof(TokenManager[]), ES3UserType_TokenManager.Instance)
		{
			Instance = this;
		}
	}
}