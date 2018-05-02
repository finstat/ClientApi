using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi
{
    public class DistraintResult
    {
        public int Count { get; set; }
        public DistraintPreview[] Distraints { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(base.ToString());

            result.AppendLine(string.Format("Count: {0}", Count));
            if (Distraints != null && Distraints.Length > 0)
            {
                result.AppendLine("\nDistraints:");
                foreach (var distraint in Distraints)
                {
                    result.AppendLine(distraint.ToString());
                }
            }
            return result.ToString();
        }
    }
}
