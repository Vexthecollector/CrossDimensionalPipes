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
        public int MaxCanInput => (int)PipeNet.AvailableCapacity;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            compBreakdownable = parent.GetComp<CompBreakdownable>();
            compPowerTrader = parent.GetComp<CompPowerTrader>();
            compFlickable = parent.GetComp<CompFlickable>();
            WorldComponent_TeleporterOut.Instance.TeleOut.Add(this);
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

        public void Input(float amountToAdd)
        {
            if (CanOutputNow && amountToAdd > 0)
            {
                PipeNet.DistributeAmongStorage(amountToAdd);
            }
        }
    }
}
