using System;
using System.Collections.Generic;

namespace BusinessDataTransformer.Model
{
    public class CompanyOutputData
    {
        public string ICO { get; set; }
        public string Name { get; set; }

        public Dictionary<int, List<OwnerInfo>> OwnersByYears { get; set; }

        public override string ToString()
        {
            var result = $"Company {Name} with ICO {ICO} has owners:";

            foreach (var year in OwnersByYears.Keys)
            {
                var yearOwners = OwnersByYears[year];

                foreach (var owner in yearOwners)
                {
                    result += $"\n {year} -> {owner.ToString()}";
                }
            }

            return result;
        }
    }
}
