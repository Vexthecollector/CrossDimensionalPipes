using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PipeSystem;
using RimWorld;
using Verse;

namespace CrossDimensionalPipes
{
    public class TeleporterValveOutput : CompResource, IRenameable
    {
        public string Name;
        private CompBreakdownable compBreakdownable;
        private CompPowerTrader compPowerTrader;
        private CompFlickable compFlickable;
        private Command_Action setName;
        public string label;
        public string RenamableLabel { get => label ?? BaseLabel; set => label = value; }
        public string BaseLabel => parent.LabelCap;
        public string InspectLabel => RenamableLabel;
        public int MaxCanInput => (int)PipeNet.AvailableCapacity;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            compBreakdownable = parent.GetComp<CompBreakdownable>();
            compPowerTrader = parent.GetComp<CompPowerTrader>();
            compFlickable = parent.GetComp<CompFlickable>();
            WorldComponent_TeleporterOut.Instance.TeleOut.AddDistinct(this);
            setName= new Command_Action
            {
                action = () => Find.WindowStack.Add(new Dialog_RenameOutput(this)),
                defaultLabel = "Rename Output",
                defaultDesc = "Rename the Output"
            };
            if (respawningAfterLoad) return;
            Find.WindowStack.Add(new Dialog_RenameOutput(this));


        }

        public bool CanOutputNow
        {
            get
            {
                return (compBreakdownable == null || !compBreakdownable.BrokenDown)
                       && (compPowerTrader == null || compPowerTrader.PowerOn)
                       && (compFlickable == null || compFlickable.SwitchIsOn);
            }
        }

        public override void PostDeSpawn( Map previousMap)
        {
            base.PostDeSpawn(previousMap);
            WorldComponent_TeleporterOut.Instance.TeleOut.Remove(this);
        }

        public void Input(float amountToAdd)
        {
            if (CanOutputNow && amountToAdd > 0)
            {
                PipeNet.DistributeAmongStorage(amountToAdd, out _);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            yield return setName;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref label, "Name");
        }

    }
    public class Dialog_RenameOutput : Dialog_Rename<TeleporterValveOutput>
    {
        public TeleporterValveOutput Output;
        public Dialog_RenameOutput(TeleporterValveOutput output): base(output)
        {
            this.Output = output;
            this.curName = output.label ?? "Teleporter Output" + " #" + Rand.Range(1, 99).ToString("D2");
        }
    }
}
