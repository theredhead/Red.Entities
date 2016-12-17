using System;
using System.Collections.Generic;

namespace Red.Entities
{
    /// <summary>
    /// An Entity statement builder helps an EntityHub be independent from Dayabase engine specifics
    /// by provider a layer between the hub and the database.
    /// </summary>
    abstract public class EntityStatementBuilder
    {
        abstract public string BuildInsertCommandText(EntityType entityType);
        abstract public string BuildUpdateCommandText(EntityType entityType);

        abstract public string BuildSelectCommandText(EntityFetchRequest request);

        abstract public string FormatFieldName(string fieldName);
        abstract public string FormatParameterName(string parameterName);
    }
}
