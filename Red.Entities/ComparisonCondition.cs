using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Red.Utility;

namespace Red.Entities
{
    public enum ComparisonCondition
    {
        Equals,
        NotEquals,
        Contains,
        DoesNotContain,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        Between,
        NotBetween,
        BeginsWith,
        EndsWith,
        DoesNotBeginWith,
        DoesNotEndWith
    }
    
}
