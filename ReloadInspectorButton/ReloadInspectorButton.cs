using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;
using FrooxEngine.UIX;
using BaseX;

namespace ReloadInspectorButton
{
    public class ReloadInspectorButton : NeosMod
    {
        public override string Name => "ReloadInspectorButton";
        public override string Author => "badhaloninja";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/badhaloninja/ReloadInspectorButton";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("me.badhaloninja.ReloadInspectorButton");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(SceneInspector), "OnAttach")]
        class SceneInspector_OnAttach_Patch
        {
            public static void Postfix(SceneInspector __instance, SyncRef<Sync<string>> ____componentText, SyncRef<Slot> ____currentComponent, SyncRef<Slot> ____componentsContentRoot)
            {
                UIBuilder ui = new UIBuilder(____componentText.Target.Slot.Parent);
                // Fit with the size of other buttons
                ui.Style.FlexibleWidth = -1f;
                ui.Style.MinWidth = 64f;
                
                
                Uri Reload = NeosAssets.Common.Icons.Reload;
                color color = new color(0.8f, 1f, 1f);
                
                var btn = ui.Button(Reload, color, color.Black);
                btn.LocalPressed += (button, evnt) =>
                { // reload the inspector
                    if (____currentComponent.Target == null) return;

                    ____componentsContentRoot.Target.DestroyChildren(); // Clear the content

                    ____componentsContentRoot.Target.AddSlot("ComponentRoot") // Generate component inspector ui
                       .AttachComponent<WorkerInspector>()
                       .SetupContainer(____currentComponent.Target);
                };
                btn.Slot.ActiveSelf_Field.OverrideForUser(btn.LocalUser, true).Default.Value = false; // Make the button invisible for other users
            }
        }
    }
}