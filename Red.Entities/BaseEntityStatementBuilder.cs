using System;
using System.Collections.Generic;
using System.Text;

namespace Red.Entities
{
    abstract public class BaseEntityStatementBuilder : EntityStatementBuilder
    {
        private bool insertIdentities = false;
        private string identityFieldName = "_Id";

        public BaseEntityStatementBuilder()
        {
        }

        public override string BuildInsertCommandText(EntityType entityType)
        {
            List<string> fieldList = new List<string>();
            List<string> parameterList = new List<string>();

            foreach (string fieldName in entityType.FieldNames)
            {
                if (insertIdentities || fieldName != identityFieldName)
                {
                    fieldList.Add(FormatFieldName(fieldName));
                    parameterList.Add(FormatParameterName(fieldName));
                }
            }

            string table = entityType.TableName;
            string joinedFieldList = String.Join(", ", fieldList);
            string joinedParameterList = String.Join(", ", parameterList);

            return $"INSERT INTO {FormatTableName(table)} ({joinedFieldList}) VALUES ({joinedParameterList})";
        }

        public override string BuildUpdateCommandText(EntityType entityType)
        {
            List<string> assignmentsList = new List<string>();
            List<string> whereList = new List<string>();

            foreach (string fieldName in entityType.FieldNames)
            {
                if (fieldName == identityFieldName)
                {
                    whereList.Add($"{FormatFieldName(fieldName)} = {FormatParameterName(fieldName)}");
                }
                else
                {
                    assignmentsList.Add($"{FormatFieldName(fieldName)} = {FormatParameterName(fieldName)}");
                }
            }

            string table = entityType.TableName;
            string joinedAssignmentsList = String.Join(", ", assignmentsList);
            string joinedWhereList = String.Join(", ", whereList);

            return $"UPDATE {FormatTableName(table)} SET {joinedAssignmentsList} WHERE {joinedWhereList}";
        }

        public override string BuildSelectCommandText(EntityFetchRequest request)
        {
            string table = request.EntityType.TableName;

            StringBuilder builder = new StringBuilder($"SELECT {FormatTableName(table)}.* FROM {FormatTableName(table)}");

            List<string> predicateStrings = new List<string>();

            if (request.Predicates.Count > 0)
            {
                foreach (var predicate in request.Predicates)
                {
                    string matchMethod = request.Match == MatchRequirement.All ? " AND " : " OR ";
                    string predicateString = "(" + string.Join(matchMethod, this.ExpandPredicate(predicate, request.Parameters)) + ")";
                    predicateStrings.Add(predicateString);
                }
                builder.Append(" WHERE ");
                builder.Append(String.Join(" AND ", predicateStrings));
            }
            string result = builder.ToString();
            return result;
        }

        IEnumerable<string> ExpandPredicate(EntityFetchPredicate predicate, Dictionary<string, object> parameters)
        {
            List<string> stringConditions = new List<string>();

            foreach (var condition in predicate.Conditions)
            {
                #region determining string based on condition
                switch (condition.Condition)
                {
                    case ComparisonCondition.Equals:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} = {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.NotEquals:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} <> {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.Contains:
                        stringConditions.Add($"{IndexOfFunctionName()}({FormatFieldName(condition.FieldName)}, {FormatParameterName(condition.ParameterName)}) > 0");
                        break;
                    case ComparisonCondition.DoesNotContain:
                        stringConditions.Add($"{IndexOfFunctionName()}({FormatFieldName(condition.FieldName)}, {FormatParameterName(condition.ParameterName)}) = 0");
                        break;
                    case ComparisonCondition.LessThan:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} < {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.GreaterThan:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} > {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.LessThanOrEqual:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} <= {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.GreaterThanOrEqual:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} >= {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.Between:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} BETWEEN {FormatParameterName(condition.ParameterName)} AND {FormatParameterName(condition.SecondParameterName)}");
                        break;
                    case ComparisonCondition.NotBetween:
                        stringConditions.Add($"{FormatFieldName(condition.FieldName)} NOT BETWEEN {FormatParameterName(condition.ParameterName)} AND {FormatParameterName(condition.SecondParameterName)}");
                        break;
                    case ComparisonCondition.BeginsWith:
                        stringConditions.Add($"{IndexOfFunctionName()}({FormatFieldName(condition.FieldName)}, {FormatParameterName(condition.ParameterName)}) = 1");
                        break;
                    case ComparisonCondition.EndsWith:
                        stringConditions.Add($"{IndexOfFunctionName()}({FormatFieldName(condition.FieldName)}, {FormatParameterName(condition.ParameterName)}) = LENGTH({condition.FieldName}-LENGTH({FormatParameterName(condition.ParameterName)}))");
                        break;
                    case ComparisonCondition.DoesNotBeginWith:
                        stringConditions.Add($"{IndexOfFunctionName()}({FormatFieldName(condition.FieldName)}, {FormatParameterName(condition.ParameterName)}) <> 1");
                        break;
                    case ComparisonCondition.DoesNotEndWith:
                        stringConditions.Add($"{IndexOfFunctionName()}({FormatFieldName(condition.FieldName)}, {FormatParameterName(condition.ParameterName)}) <> LENGTH({condition.FieldName}-LENGTH({FormatParameterName(condition.ParameterName)}))");
                        break;
                    default:
                        throw new NotSupportedException($"Condition '{condition.Condition}' is not supported.");
                }
                #endregion determining string based on condition
            }
            return stringConditions;
        }

        public virtual string FormatTableName(string tableName)
        {
            return FormatFieldName(tableName);
        }

        abstract protected string IndexOfFunctionName();
    }
}
