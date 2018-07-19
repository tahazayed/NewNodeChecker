using System;

namespace NewNodeChecker.Models
{
    public class RunStepLog : LogBase
    {
        public string StepName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

    }
}
