using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Red.Utility;

namespace Red.Entities
{

    public class EntityFetchCondition
    {
        public ComparisonCondition Condition
        {
            get;
            private set;
        }

        public string FieldName
        {
            get;
            private set;
        }

        public string ParameterName
        {
            get;
            private set;
        }

        public string SecondParameterName
        {
            get;
            private set;
        }
        protected void AssertSupportsTwoParameters(ComparisonCondition condition)
        {
            switch (condition)
            {
                case ComparisonCondition.Between:
                case ComparisonCondition.NotBetween:
                    // supported.
                    break;
                default:
                    throw new NotSupportedException("{condition} does not support two parameters.");
            }

        }
        public EntityFetchCondition(ComparisonCondition condition, string fieldName, string parameterName, string secondParameterName = null)
        {
            Condition = condition;
            FieldName = fieldName;
            ParameterName = parameterName;

            if (secondParameterName != null)
            {
                AssertSupportsTwoParameters(condition);
                SecondParameterName = secondParameterName;
            }
        }
    }
    
}
