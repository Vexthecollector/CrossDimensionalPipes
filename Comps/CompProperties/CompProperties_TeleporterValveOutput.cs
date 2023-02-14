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
    public class CompProperties_TeleporterValveOutput : CompProperties_Resource
    {
        public CompProperties_TeleporterValveOutput()
        {
            compClass = typeof(TeleporterValveOutput);
        }

        public ThingDef thing;
    }
}
