using System;
using System.Collections.Generic;
using System.Text;

namespace Red.Entities
{
    public class SqlServerEntityStatementBuilder : EntityStatementBuilder
    {
        private bool insertIdentities = false;
        private string identityFieldName = "_Id";

        public SqlServerEntityStatementBuilder()
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
                    fieldList.Add($"[{fieldName}]");
                    parameterList.Add($"@{fieldName}");
                }
            }

            string table = entityType.TableName;
            string joinedFieldList = String.Join(", ", fieldList);
            string joinedParameterList = String.Join(", ", parameterList);

            return $"INSERT INTO [{table}] ({joinedFieldList}) VALUES ({joinedParameterList})";
        }

        public override string BuildUpdateCommandText(EntityType entityType)
        {
            List<string> assignmentsList = new List<string>();
            List<string> whereList = new List<string>();

            foreach (string fieldName in entityType.FieldNames)
            {
                if (fieldName == identityFieldName)
                {
                    whereList.Add($"[{fieldName}] = @{fieldName}");
                }
                else
                {
                    assignmentsList.Add($"[{fieldName}] = @{fieldName}");
                }
            }

            string table = entityType.TableName;
            string joinedAssignmentsList = String.Join(", ", assignmentsList);
            string joinedWhereList = String.Join(", ", whereList);

            return $"UPDATE `{table}` SET {joinedAssignmentsList} WHERE {joinedWhereList}";
        }

        public override string BuildSelectCommandText(EntityFetchRequest request)
        {
            throw new NotImplementedException();
        }

        public override string FormatParameterName(string parameterName)
        {
            return $"@{parameterName}";
        }
    }
}
