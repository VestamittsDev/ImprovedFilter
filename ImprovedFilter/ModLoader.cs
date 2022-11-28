using HarmonyLib;

namespace ImprovedFilter
{
	public class ModLoader : IModLoader
	{
		public void OnCreated()
		{
			Harmony harmony = new Harmony("com.vestamitts.improved-filter");
			harmony.PatchAll();
		}

		public void OnGameLoaded(LoadMode mode)
		{
			
		}

		public void OnGameUnloaded()
		{
			
		}

		public void OnReleased()
		{
			
		}
	}
}
