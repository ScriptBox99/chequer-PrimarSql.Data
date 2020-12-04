﻿using System;
using System.Collections.Generic;
using System.Data;
using Amazon.DynamoDBv2.Model;
using PrimarSql.Data.Models.Columns;
using PrimarSql.Data.Utilities;

namespace PrimarSql.Data.Processors
{
    internal sealed class CountFunctionProcessor : BaseProcessor
    {
        private DataTable _schemaTable;

        public bool Read { get; set; } = false;

        public override IColumn[] Columns { get; } = { new PropertyColumn("count(*)") };

        public override DataTable GetSchemaTable()
        {
            if (_schemaTable == null)
            {
                _schemaTable = DataProviderUtility.GetNewSchemaTable();
                _schemaTable.Rows.Add("count(*)", 0, typeof(object), null, false);
            }

            return _schemaTable;
        }

        public override object[] Process()
        {
            throw new NotSupportedException();
        }

        public override Dictionary<string, AttributeValue> Filter()
        {
            throw new NotSupportedException();
        }
    }
}
