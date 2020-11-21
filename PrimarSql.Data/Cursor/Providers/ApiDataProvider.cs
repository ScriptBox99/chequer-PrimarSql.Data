﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using PrimarSql.Data.Expressions.Generators;
using PrimarSql.Data.Extensions;
using PrimarSql.Data.Models;
using PrimarSql.Data.Requesters;
using PrimarSql.Data.Sources;

namespace PrimarSql.Data.Cursor.Providers
{
    public class ApiDataProvider : IDataProvider
    {
        private readonly TableDescription _tableDescription;
        private IRequester _requester;

        public QueryContext Context { get; }

        public SelectQueryInfo QueryInfo { get; }

        public bool HasRows => _requester.HasRows;

        public DataCell[] Current { get; private set; }

        public string TableName =>
            (QueryInfo.TableSource as AtomTableSource)?.TableName ?? throw new InvalidOperationException("TableSource is not AtomTableSource.");

        public string IndexName =>
            (QueryInfo.TableSource as AtomTableSource)?.IndexName;

        public ApiDataProvider(QueryContext context, SelectQueryInfo queryInfo)
        {
            Context = context;
            QueryInfo = queryInfo;
            _tableDescription = context.GetTableDescription(TableName);
            SetRequester();
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public bool Next()
        {
            var flag = _requester.Next();

            Current = flag ? ToDataCells(_requester.Current) : null;

            return flag;
        }

        private void SetRequester()
        {
            var generator = new ExpressionGenerator(_tableDescription, IndexName, QueryInfo.WhereExpression);
            var generateResult = generator.Analyze();

            var sortKeyExists = !string.IsNullOrEmpty(generator.SortKeyName);

            // HashKey Only
            if (!sortKeyExists)
            {
                // If HashKey not found in where expr
                if (generateResult.HashKey == null)
                    _requester = new ScanRequester();
                else
                {
                    if (string.IsNullOrEmpty(IndexName) && string.IsNullOrWhiteSpace(generateResult.FilterExpression))
                        _requester = new GetItemRequester();
                    else
                        _requester = new QueryRequester();
                }
            }
            // HashKey and SortKey
            else
            {
                // If HashKey or SortKey not found in where expr
                if (generateResult.HashKey == null ||
                    generateResult.SortKey == null)
                    _requester = new ScanRequester();
                else
                {
                    if (string.IsNullOrEmpty(IndexName) &&
                        string.IsNullOrWhiteSpace(generateResult.FilterExpression) &&
                        generateResult.SortKey.Operator == "=")
                        _requester = new GetItemRequester();
                    else
                    {
                        _requester = new QueryRequester();
                    }
                }
            }

            _requester.SetParameters(
                Context.Client,
                QueryInfo,
                generateResult.ExpressionAttributeNames,
                generateResult.ExpressionAttributeValues,
                generateResult.HashKey,
                generateResult.SortKey,
                TableName,
                generateResult.FilterExpression
            );
        }

        public DataCell[] ToDataCells(Dictionary<string, AttributeValue> item)
        {
            return item.Select(c =>
            {
                var value = c.Value.ToValue();
                var dataType = value.GetType();

                return new DataCell
                {
                    Data = value,
                    DataType = dataType,
                    TypeName = dataType.Name,
                    Name = c.Key,
                };
            }).ToArray();
        }
    }
}
