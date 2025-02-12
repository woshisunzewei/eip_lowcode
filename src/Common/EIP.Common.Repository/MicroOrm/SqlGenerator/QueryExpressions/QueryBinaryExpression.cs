using System.Collections.Generic;

namespace EIP.Common.Repository.MicroOrm.SqlGenerator.QueryExpressions
{
    /// <inheritdoc />
    /// <summary>
    /// `Binary` Query Expression
    /// </summary>
    internal class QueryBinaryExpression : QueryExpression
    {
        public QueryBinaryExpression(List<QueryExpression> nodes)
        {
            Nodes = nodes;
            NodeType = QueryExpressionType.Binary;
        }

        public List<QueryExpression> Nodes { get; }

        public override string ToString()
        {
            return $"[{base.ToString()} ({string.Join(",", Nodes)})]";
        }
    }
}