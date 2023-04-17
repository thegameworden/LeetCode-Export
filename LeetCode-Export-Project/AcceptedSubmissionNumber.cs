using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Export
{
    public class AcceptedSubmissionNumber
    {
        string difficulty;
        int count;

        public string Difficulty { get => difficulty; set => difficulty = value; }
        public int Count { get => count; set => count = value; }

        public override string ToString()
        {
            return $"{difficulty} : {count}";
        }
    }
}
