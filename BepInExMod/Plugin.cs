using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace cloverpitjumpstart;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        this.harmony.PatchAll();
    }

    [HarmonyPatch(typeof(GameplayData))]
    [HarmonyPatch(".ctor")]
	public class GameplayDataPatch
	{
		static void Postfix(object __instance)
		{
			FieldInfo cloverTickets = typeof(GameplayData).GetField("cloverTickets", BindingFlags.NonPublic | BindingFlags.Instance);
            if (cloverTickets != null)
            {
                cloverTickets.SetValue(__instance, 4L);
            }

            FieldInfo storeFreeRestocks = typeof(GameplayData).GetField("_storeFreeRestocks", BindingFlags.NonPublic | BindingFlags.Instance);
            if (storeFreeRestocks != null)
            {
                storeFreeRestocks.SetValue(__instance, 1L);
            }
		}
	}

    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
}
