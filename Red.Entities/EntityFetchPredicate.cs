using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Red.Utility;

namespace Red.Entities
{
    /// <summary>
    /// Entity fetch request represents a non sql API to create IDbCommands for select queries.
    /// </summary>
    public class EntityFetchPredicate
    {
        public EntityFetchRequest Request { get; private set; }

        public EntityFetchPredicate(EntityFetchRequest request)
        {
            Request = request;
        }

        public List<EntityFetchCondition> Conditions
        {
            get;
            private set;
        } = new List<EntityFetchCondition>();

        private Dictionary<string, object> parameters
        {
            get
            {
                return Request.Parameters;
            }
        }

        protected string GetOrCreateParameterName(string name, object value)
        {
            if (parameters.ContainsValue(value))
            {
                foreach (string key in parameters.Keys)
                {
                    if (parameters[key].Equals(value))
                    {
                        return key;
                    }
                }
            }
            string newKey = name;
            while (parameters.ContainsKey(newKey))
            {
                newKey = newKey.NextInSequence();
            }
            parameters[newKey] = value;
            return newKey;
        }

        private EntityFetchPredicate AddScalarCondition(ComparisonCondition condition, string fieldName, object parameterValue)
        {
            string parameterName = GetOrCreateParameterName(fieldName, parameterValue);
            Conditions.Add(new EntityFetchCondition(condition, fieldName, parameterName));
            return this;
        }

        private EntityFetchPredicate AddDualCondition(ComparisonCondition condition, string fieldName, object minimum, object maximum)
        {
            string minimumParameterName = GetOrCreateParameterName(fieldName, minimum);
            string maximumParameterName = GetOrCreateParameterName(fieldName, maximum);

            Conditions.Add(new EntityFetchCondition(condition, fieldName, minimumParameterName, maximumParameterName));
            return this;
        }

        public EntityFetchPredicate WhereStringFieldContains(string fieldName, string needle)
        {
            return AddScalarCondition(ComparisonCondition.Contains, fieldName, needle);
        }
        public EntityFetchPredicate WhereStringFieldBeginsWith(string fieldName, string needle)
        {
            return AddScalarCondition(ComparisonCondition.BeginsWith, fieldName, needle);
        }
        public EntityFetchPredicate WhereStringFieldEndsWith(string fieldName, string needle)
        {
            return AddScalarCondition(ComparisonCondition.EndsWith, fieldName, needle);
        }
        public EntityFetchPredicate WhereStringFieldEquals(string fieldName, string needle)
        {
            return AddScalarCondition(ComparisonCondition.Equals, fieldName, needle);
        }

        public EntityFetchPredicate WhereIntegerFieldIsLessThan(string fieldName, long value)
        {
            return AddScalarCondition(ComparisonCondition.LessThan, fieldName, value);
        }
        public EntityFetchPredicate WhereIntegerFieldIsLessThanOrEqualTo(string fieldName, long value)
        {
            return AddScalarCondition(ComparisonCondition.LessThanOrEqual, fieldName, value);
        }
        public EntityFetchPredicate WhereIntegerFieldIsGreaterThan(string fieldName, long value)
        {
            return AddScalarCondition(ComparisonCondition.GreaterThan, fieldName, value);
        }
        public EntityFetchPredicate WhereIntegerFieldIsGreaterThanOrEqualTo(string fieldName, long value)
        {
            return AddScalarCondition(ComparisonCondition.GreaterThanOrEqual, fieldName, value);
        }
        public EntityFetchPredicate WhereIntegerFieldIsEqualTo(string fieldName, long value)
        {
            return AddScalarCondition(ComparisonCondition.Equals, fieldName, value);
        }
        public EntityFetchPredicate WhereIntegerFieldIsBetween(string fieldName, long minimum, long maximum)
        {
            return AddDualCondition(ComparisonCondition.Between, fieldName, minimum, maximum);
        }

        public EntityFetchPredicate WhereDateTimeFieldIsBefore(string fieldName, DateTime value)
        {
            return AddScalarCondition(ComparisonCondition.LessThan, fieldName, value);
        }
        public EntityFetchPredicate WhereDateTimeFieldIsBeforeOr(string fieldName, DateTime value)
        {
            return AddScalarCondition(ComparisonCondition.LessThanOrEqual, fieldName, value);
        }
        public EntityFetchPredicate WhereDateTimeFieldIsAfter(string fieldName, DateTime value)
        {
            return AddScalarCondition(ComparisonCondition.GreaterThan, fieldName, value);
        }
        public EntityFetchPredicate WhereDateTimeFieldIsAfterOrOn(string fieldName, DateTime value)
        {
            return AddScalarCondition(ComparisonCondition.GreaterThanOrEqual, fieldName, value);
        }
        public EntityFetchPredicate WhereDateTimeFieldIsEqualTo(string fieldName, DateTime value)
        {
            return AddScalarCondition(ComparisonCondition.Equals, fieldName, value);
        }
        public EntityFetchPredicate WhereDateTimeFieldIsBetween(string fieldName, DateTime minimum, DateTime maximum)
        {
            return AddDualCondition(ComparisonCondition.Between, fieldName, minimum, maximum);
        }
        public EntityFetchPredicate WhereDateTimeFieldIsOnDate(string fieldName, DateTime aDate)
        {
            return WhereDateTimeFieldIsBetween(fieldName, aDate.FirstSecondOnDay(), aDate.LastSecondOnDay());
        }

        public EntityFetchPredicate WhereDecimalFieldEquals(string fieldName, decimal value)
        {
            return AddScalarCondition(ComparisonCondition.Equals, fieldName, value);
        }
        public EntityFetchPredicate WhereDecimalFieldIsNotEqualTo(string fieldName, decimal value)
        {
            return AddScalarCondition(ComparisonCondition.NotEquals, fieldName, value);
        }
        public EntityFetchPredicate WhereDecimalFieldIsLessThan(string fieldName, decimal value)
        {
            return AddScalarCondition(ComparisonCondition.LessThan, fieldName, value);
        }
        public EntityFetchPredicate WhereDecimalFieldIsLessThanOrEqualTo(string fieldName, decimal value)
        {
            return AddScalarCondition(ComparisonCondition.LessThanOrEqual, fieldName, value);
        }
        public EntityFetchPredicate WhereDecimalFieldIsGreaterThan(string fieldName, decimal value)
        {
            return AddScalarCondition(ComparisonCondition.GreaterThan, fieldName, value);
        }
        public EntityFetchPredicate WhereDecimalFieldIsGreaterThanOrEqualTo(string fieldName, decimal value)
        {
            return AddScalarCondition(ComparisonCondition.GreaterThanOrEqual, fieldName, value);
        }
        public EntityFetchPredicate WhereDecimalFieldBetween(string fieldName, decimal minimum, decimal maximum)
        {
            return AddDualCondition(ComparisonCondition.Between, fieldName, minimum, maximum);
        }
        public EntityFetchPredicate WhereDecimalFieldIsNotBetween(string fieldName, decimal minimum, decimal maximum)
        {
            return AddDualCondition(ComparisonCondition.NotBetween, fieldName, minimum, maximum);
        }
    }
}
