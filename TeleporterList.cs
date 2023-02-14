using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Unity;
using UnityEngine;
using RimWorld.Planet;

namespace CrossDimensionalPipes
{
    public class TeleporterList
    {
    }
    public class WorldComponent_TeleporterIn: WorldComponent
    {
        public static WorldComponent_TeleporterIn Instance;

        private List<TeleporterValveInput> teleIn = new List<TeleporterValveInput>();

        public WorldComponent_TeleporterIn(World world) : base(world) => Instance = this;

        public List<TeleporterValveInput> TeleIn
        {
            get
            {
                return this.teleIn;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref this.teleIn, "teleporterIn", LookMode.Reference);
        }
    }

    public class WorldComponent_TeleporterOut : WorldComponent
    {
        public static WorldComponent_TeleporterOut Instance;

        private List<TeleporterValveOutput> teleOut = new List<TeleporterValveOutput>();

        public WorldComponent_TeleporterOut(World world) : base(world) => Instance = this;

        public List<TeleporterValveOutput> TeleOut
        {
            get
            {
                return this.teleOut;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref this.teleOut, "teleporterOut", LookMode.Reference);
        }
    }
}
