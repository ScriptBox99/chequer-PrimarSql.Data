using Amazon.DynamoDBv2.Model;

namespace PrimarSql.Data.Planners.Index
{
    internal sealed class AddIndexAction : IndexAction
    {
        public IndexDefinition IndexDefinition { get; set; }

        public override void Action(UpdateTableRequest request, TableDescription tableDescription)
        {
            var provisionedThroughput = new ProvisionedThroughput(
                tableDescription.ProvisionedThroughput.ReadCapacityUnits,
                tableDescription.ProvisionedThroughput.WriteCapacityUnits
            );
            
            request.GlobalSecondaryIndexUpdates.Add(new GlobalSecondaryIndexUpdate
            {
                Create = new CreateGlobalSecondaryIndexAction
                {
                    IndexName = IndexDefinition.IndexName,
                    Projection = IndexDefinition.Projection,
                    KeySchema = IndexDefinition.KeySchema,
                    ProvisionedThroughput = provisionedThroughput,
                }
            });
        }
    }
}
