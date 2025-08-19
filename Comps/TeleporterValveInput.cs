using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PipeSystem;
using RimWorld;
using Verse;
using UnityEngine;

namespace CrossDimensionalPipes
{
    [StaticConstructorOnStartup]
    public class TeleporterValveInput : CompResource
    {
        private static readonly Texture2D teleporterIcon = ContentFinder<Texture2D>.Get("teleporter");
        public string Name { get; set; }
        private CompBreakdownable compBreakdownable;
        private CompPowerTrader compPowerTrader;
        private CompFlickable compFlickable;
        public ThingWithComps teleOutThing;
        public float amountToRemove=50;

        public TeleporterValveOutput teleOut;
        private Command_Action selectOutput;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            compBreakdownable = parent.GetComp<CompBreakdownable>();
            compPowerTrader = parent.GetComp<CompPowerTrader>();
            compFlickable = parent.GetComp<CompFlickable>();
            if(teleOutThing!=null)teleOut=teleOutThing.GetComp<TeleporterValveOutput>();
            selectOutput = new Command_Action
            {
                action = delegate
                {
                    process_Outputs();
                },
                defaultLabel = "Select Output",
                defaultDesc = "Select the Output",
                icon = teleporterIcon
            };
        }

        public bool CanInputNow
        {
            get
            {
                return (compBreakdownable == null || !compBreakdownable.BrokenDown)
                       && (compPowerTrader == null || compPowerTrader.PowerOn)
                       && (compFlickable == null || compFlickable.SwitchIsOn);
            }
        }
        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(300))
                CompTickRare();
        }



        public override void CompTickRare()
        {
            base.CompTickRare();
            if (teleOut != null)
            {

            if (CanInputNow && amountToRemove > 0 && teleOut.MaxCanInput > amountToRemove && amountToRemove < PipeNet.CurrentStored() &&teleOut.CanOutputNow)
            {
                teleOut.Input(amountToRemove);
                PipeNet.DrawAmongStorage(amountToRemove, PipeNet.storages);
            }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            yield return selectOutput;
        }

        public void process_Outputs()
        {
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            foreach (TeleporterValveOutput output in WorldComponent_TeleporterOut.Instance.TeleOut)
            {
                if (output.Resource == this.Resource)
                {

                list.Add(new FloatMenuOption("Select " + output.InspectLabel, () =>
                {
                    teleOut = output;
                    teleOutThing = teleOut.parent;
                }));
                }
            }
            if (list.Any<FloatMenuOption>())
            {
                Find.WindowStack.Add(new FloatMenu(list));
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref teleOutThing, "teleporterOut");
        }
    }
}
