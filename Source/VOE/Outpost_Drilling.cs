﻿using System.Collections.Generic;
using Outposts;
using RimWorld;
using Verse;

namespace VOE
{
    public class Outpost_Drilling : Outpost
    {
        private int constructionSkill;
        private int workUntilReady;
        protected virtual int WorkNeeded => 7 * 60000;

        public override void PostMake()
        {
            base.PostMake();
            workUntilReady = WorkNeeded;
        }

        public override IEnumerable<Thing> ProducedThings() => workUntilReady > 0 ? new List<Thing>() : MakeThings(ThingDefOf.Chemfuel, 500);

        public override void Tick()
        {
            base.Tick();
            if (workUntilReady > 0 && !Packing) workUntilReady -= constructionSkill;
        }

        public override void RecachePawnTraits()
        {
            constructionSkill = TotalSkill(SkillDefOf.Construction);
        }

        public override string ProductionString() => workUntilReady > 0
            ? "Outposts.Drilling".Translate(((float) workUntilReady / WorkNeeded).ToStringPercent(), (workUntilReady / constructionSkill).ToStringTicksToPeriodVerbose())
            : base.ProductionString();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workUntilReady, "workUntilReady");
        }
    }
}