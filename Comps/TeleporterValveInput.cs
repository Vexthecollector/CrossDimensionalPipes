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
    public class TeleporterValveInput : CompResource
    {
        public string Name { get; set; }
        private CompBreakdownable compBreakdownable;
        private CompPowerTrader compPowerTrader;
        private CompFlickable compFlickable;
        public float amountToRemove;

        public TeleporterValveOutput teleOut;
        private Command_Action selectOutput;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            compBreakdownable = parent.GetComp<CompBreakdownable>();
            compPowerTrader = parent.GetComp<CompPowerTrader>();
            compFlickable = parent.GetComp<CompFlickable>();

            selectOutput = new Command_Action
            {
                action = delegate
                {
                    
                },
                defaultLabel = "Select Output",
                defaultDesc = "Select the Output"
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
            if (parent.IsHashIntervalTick(250))
                CompTickRare();
        }
        


        public override void CompTickRare()
        {
            base.CompTickRare();
            if (CanInputNow && amountToRemove > 0 && teleOut.MaxCanInput > amountToRemove)
            {
                teleOut.Input(amountToRemove);
                PipeNet.DrawAmongStorage(amountToRemove,PipeNet.storages);
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

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption option in base.CompFloatMenuOptions(selPawn)) yield return option;


            foreach (TeleporterValveOutput output in WorldComponent_TeleporterOut.Instance.TeleOut)
                yield return new FloatMenuOption("Select "+output.Name, () =>
                  {
                      teleOut = output;
                  });
            }
        }
}
