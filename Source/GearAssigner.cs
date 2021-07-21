﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DArcaneTechnology
{
    public static class GearAssigner
    {
        public static HashSet<string> exemptProjects = new HashSet<string>();
        public static Dictionary<string, string> overrideAssignment = new Dictionary<string, string>();
        public static Dictionary<string, string> hardAssignment = new Dictionary<string, string>();

        public static void HardAssign(ref Dictionary<ThingDef, ResearchProjectDef> thingDic, ref Dictionary<ResearchProjectDef, List<ThingDef>> researchDic)
        {
            foreach (string keyStr in hardAssignment.Keys)
            {
                if (overrideAssignment.ContainsKey(keyStr))
                    continue;
                ThingDef thing = DefDatabase<ThingDef>.GetNamedSilentFail(keyStr);
                if (thing != null)
                {
                    ResearchProjectDef rpd = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(hardAssignment[keyStr]);
                    if (rpd != null)
                    {
                        CompProperties_DArcane comp = thing.GetCompProperties<CompProperties_DArcane>();
                        if (comp == null)
                            thing.comps.Add(new CompProperties_DArcane(rpd));

                        if (!thingDic.ContainsKey(thing))
                            thingDic.Add(thing, rpd);

                        if (!researchDic.ContainsKey(rpd))
                            researchDic.Add(rpd, new List<ThingDef> { thing });
                        else
                        {
                            if (!researchDic[rpd].Contains(thing))
                            {
                                researchDic[rpd].Add(thing);
                            }
                        }
                    }
                }
            }
        }

        public static void OverrideAssign(ref Dictionary<ThingDef, ResearchProjectDef> thingDic, ref Dictionary<ResearchProjectDef, List<ThingDef>> researchDic)
        {
            foreach (string keyStr in overrideAssignment.Keys)
            {
                ThingDef thing = DefDatabase<ThingDef>.GetNamedSilentFail(keyStr);
                if (thing != null)
                {
                    ResearchProjectDef rpd = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(overrideAssignment[keyStr]);
                    if (rpd != null)
                    {
                        CompProperties_DArcane comp = thing.GetCompProperties<CompProperties_DArcane>();
                        if (comp == null)
                            thing.comps.Add(new CompProperties_DArcane(rpd));

                        thingDic.SetOrAdd(thing, rpd);

                        if (!researchDic.ContainsKey(rpd))
                            researchDic.Add(rpd, new List<ThingDef> { thing });
                        else
                        {
                            if (!researchDic[rpd].Contains(thing))
                            {
                                researchDic[rpd].Add(thing);
                            }
                        }
                    }
                }
            }
        }


        public static bool GetOverrideAssignment(ThingDef thing, out ResearchProjectDef rpd)
        {
            string name;
            if (overrideAssignment.TryGetValue(thing.defName, out name))
            {
                if (name == "None")
                {
                    rpd = null;
                    return true;
                }
                rpd = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(name);
                if (rpd != null)
                    return true;
            }
            rpd = null;
            return false;
        }

        public static bool ProjectIsExempt(ResearchProjectDef rpd)
        {
            return ArcaneTechnologySettings.exemptClothing && exemptProjects.Contains(rpd.defName);
        }

    }
}
