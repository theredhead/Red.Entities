﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Red.Entities
{
    public class MySqlEntityStatementBuilder : EntityStatementBuilder
    {
        private bool insertIdentities = false;
        private string identityFieldName = "_Id";

        public MySqlEntityStatementBuilder()
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
                    fieldList.Add($"`{fieldName}`");
                    parameterList.Add($":{fieldName}");
                }
            }

            string table = entityType.TableName;
            string joinedFieldList = String.Join(", ", fieldList);
            string joinedParameterList = String.Join(", ", parameterList);

            return $"INSERT INTO `{table}` ({joinedFieldList}) VALUES ({joinedParameterList})";
        }

        public override string BuildUpdateCommandText(EntityType entityType)
        {
            List<string> assignmentsList = new List<string>();
            List<string> whereList = new List<string>();

            foreach (string fieldName in entityType.FieldNames)
            {
                if (fieldName == identityFieldName)
                {
                    whereList.Add($"`{fieldName}` = :{fieldName}");
                }
                else
                {
                    assignmentsList.Add($"`{fieldName}` = :{fieldName}");
                }
            }

            string table = entityType.TableName;
            string joinedAssignmentsList = String.Join(", ", assignmentsList);
            string joinedWhereList = String.Join(", ", whereList);

            return $"UPDATE `{table}` SET {joinedAssignmentsList} WHERE {joinedWhereList}";
        }

        public override string BuildSelectCommandText(EntityFetchRequest request)
        {
            string table = request.EntityType.TableName;

            StringBuilder builder = new StringBuilder($"SELECT `{table}`.* FROM `{table}`");

            List<string> predicateStrings = new List<string>();

            if (request.Predicates.Count > 0)
            {
                foreach (var predicate in request.Predicates)
                {
                    string matchMethod = request.Match == MatchRequirement.All ? " AND " : " OR ";
                    string predicateString = string.Join(matchMethod, this.ExpandPredicate(predicate, request.Parameters));
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
                        stringConditions.Add($"`{condition.FieldName}` = {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.NotEquals:
                        stringConditions.Add($"`{condition.FieldName}` <> {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.Contains:
                        stringConditions.Add($"INSTR(`{condition.FieldName}`, {FormatParameterName(condition.ParameterName)}) > 0");
                        break;
                    case ComparisonCondition.DoesNotContain:
                        stringConditions.Add($"INSTR(`{condition.FieldName}`, {FormatParameterName(condition.ParameterName)}) = 0");
                        break;
                    case ComparisonCondition.LessThan:
                        stringConditions.Add($"`{condition.FieldName}` < {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.GreaterThan:
                        stringConditions.Add($"`{condition.FieldName}` > {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.LessThanOrEqual:
                        stringConditions.Add($"`{condition.FieldName}` <= {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.GreaterThanOrEqual:
                        stringConditions.Add($"`{condition.FieldName}` >= {FormatParameterName(condition.ParameterName)}");
                        break;
                    case ComparisonCondition.Between:
                        stringConditions.Add($"`{condition.FieldName}` BETWEEN {FormatParameterName(condition.ParameterName)} AND {FormatParameterName(condition.SecondParameterName)}");
                        break;
                    case ComparisonCondition.NotBetween:
                        stringConditions.Add($"`{condition.FieldName}` NOT BETWEEN {FormatParameterName(condition.ParameterName)} AND {FormatParameterName(condition.SecondParameterName)}");
                        break;
                    case ComparisonCondition.BeginsWith:
                        stringConditions.Add($"INSTR(`{condition.FieldName}`, {FormatParameterName(condition.ParameterName)}) = 1");
                        break;
                    case ComparisonCondition.EndsWith:
                        stringConditions.Add($"INSTR(`{condition.FieldName}`, {FormatParameterName(condition.ParameterName)}) = LENGTH({condition.FieldName}-LENGTH({FormatParameterName(condition.ParameterName)}))");
                        break;
                    case ComparisonCondition.DoesNotBeginWith:
                        stringConditions.Add($"INSTR(`{condition.FieldName}`, {FormatParameterName(condition.ParameterName)}) <> 1");
                        break;
                    case ComparisonCondition.DoesNotEndWith:
                        stringConditions.Add($"INSTR(`{condition.FieldName}`, {FormatParameterName(condition.ParameterName)}) <> LENGTH({condition.FieldName}-LENGTH({FormatParameterName(condition.ParameterName)}))");
                        break;
                    default:
                        throw new NotSupportedException($"Condition '{condition.Condition}' is not supported.");
                }
                #endregion determining string based on condition
            }
            return stringConditions;
        }

        public override string FormatParameterName(string parameterName)
        {
            return $"@{parameterName}";
        }
    }
}