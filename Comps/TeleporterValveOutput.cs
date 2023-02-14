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
    public class TeleporterValveOutput : CompResource
    {
        public string Name { get; set; }
        private CompBreakdownable compBreakdownable;
        private CompPowerTrader compPowerTrader;
        private CompFlickable compFlickable;
        private Command_Action setName;

        public int MaxCanInput => (int)PipeNet.AvailableCapacity;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            compBreakdownable = parent.GetComp<CompBreakdownable>();
            compPowerTrader = parent.GetComp<CompPowerTrader>();
            compFlickable = parent.GetComp<CompFlickable>();
            if (respawningAfterLoad) return;
            Find.WindowStack.Add(new Dialog_RenameOutput(this));
            WorldComponent_TeleporterOut.Instance.TeleOut.Add(this);

            setName= new Command_Action
            {
                action = () => Find.WindowStack.Add(new Dialog_RenameOutput(this)),
                defaultLabel = "Rename Output",
                defaultDesc = "Rename the Output"
            };

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

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            WorldComponent_TeleporterOut.Instance.TeleOut.Remove(this);
        }

        public void Input(float amountToAdd)
        {
            if (CanOutputNow && amountToAdd > 0)
            {
                PipeNet.DistributeAmongStorage(amountToAdd);
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




    }
    public class Dialog_RenameOutput : Dialog_Rename
    {
        public TeleporterValveOutput Output;

        public Dialog_RenameOutput(TeleporterValveOutput output)
        {
            this.Output = output;
            this.curName = output.Name ?? "Teleporter Output" + " #" + Rand.Range(1, 99).ToString("D2");
        }


        protected override void SetName(string name)
        {
            this.Output.Name = name;
        }
    }
}
