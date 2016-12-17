using System;
using System.Collections.Generic;
using System.Text;

namespace Red.Entities
{
    public class SqlServerEntityStatementBuilder : BaseEntityStatementBuilder
    {
        public override string FormatParameterName(string parameterName)
        {
            return $"@{parameterName}";
        }

        public override string FormatFieldName(string fieldName)
        {
            return $"[{fieldName}]";
        }

        protected override string IndexOfFunctionName()
        {
            return "CHARINDEX";
        }
    }
}
