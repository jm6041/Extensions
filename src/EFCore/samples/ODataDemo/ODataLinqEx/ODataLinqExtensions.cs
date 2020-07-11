using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Expressions;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using ODataDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class ODataLinqExtensions
    {
        /*/// <summary>
        /// Apply $filter parameter to query.
        /// </summary>
        /// <param name="query">
        /// The OData aware query.
        /// </param>
        /// <param name="filterText">
        /// The $filter parameter text.
        /// </param>
        /// <param name="entitySetName">
        /// The entity set name.
        /// </param>
        /// <typeparam name="T">
        /// The query type param
        /// </typeparam>
        /// <returns>
        /// The <see cref="ODataQuery{T}"/> query with applied filter parameter.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Argument Null Exception
        /// </exception>
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string filterText, string entitySetName = null)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (filterText == null) throw new ArgumentNullException(nameof(filterText));

            IEdmModel edmModel = EdmModelBuilder.GetEdmModel();

            ODataQueryOptionParser queryOptionParser = GetParser(entitySetName, new Dictionary<string, string> { { "$filter", filterText } });

            DefaultQuerySettings settings = new DefaultQuerySettings();

            FilterClause filterClause = queryOptionParser.ParseFilter();
            SingleValueNode filterExpression = filterClause.Expression.Accept(
                new ParameterAliasNodeTranslator(queryOptionParser.ParameterAliasNodes)) as SingleValueNode;
            filterExpression = filterExpression ?? new ConstantNode(null);
            filterClause = new FilterClause(filterExpression, filterClause.RangeVariable);

            var validator = new FilterQueryValidator(settings);
            ODataValidationSettings oDataValidationSettings = new ODataValidationSettings()
            {
                MaxTop = settings.MaxTop
            };
            validator.Validate(filterClause, oDataValidationSettings, edmModel);

            Expression filter = FilterBinder.Bind(query, filterClause, typeof(T), query.ServiceProvider);
            var result = ExpressionHelpers.Where(query, filter, typeof(T));

            return new ODataQuery<T>(result, query.ServiceProvider);
        }*/

        public static ODataQueryOptionParser GetParser<T>(string entitySetName, IDictionary<string, string> raws)
        {
            IEdmModel edmModel = EdmModelBuilder.GetEdmModel();

            if (entitySetName == null)
            {
                entitySetName = typeof(T).Name;
            }

            IEdmEntityContainer[] containers =
                edmModel.SchemaElements.Where(
                        e => e.SchemaElementKind == EdmSchemaElementKind.EntityContainer &&
                             (e as IEdmEntityContainer).FindEntitySet(entitySetName) != null)
                    .OfType<IEdmEntityContainer>()
                    .ToArray();

            if (containers.Length == 0)
            {
                throw new ArgumentException($"Unable to find {entitySetName} entity set in the model.",
                    nameof(entitySetName));
            }

            if (containers.Length > 1)
            {
                throw new ArgumentException($"Entity Set {entitySetName} found more that 1 time",
                    nameof(entitySetName));
            }

            IEdmEntitySet entitySet = containers.Single().FindEntitySet(entitySetName);

            if (entitySet == null)
            {

            }

            ODataPath path = new ODataPath(new EntitySetSegment(entitySet));

            ODataQueryOptionParser parser = new ODataQueryOptionParser(edmModel, path, raws);

            // Workaround for strange behavior in QueryOptionsParserConfiguration constructor which set it to false always
            parser.Resolver.EnableCaseInsensitive = true;

            return parser;
        }
    }

    public static class EdmModelBuilder
    {
        private static IEdmModel _edmModel;

        public static IEdmModel GetEdmModel()
        {
            if (_edmModel == null)
            {
                var builder = new ODataConventionModelBuilder();
                builder.EntitySet<Product>("Products");
                _edmModel = builder.GetEdmModel();
            }

            return _edmModel;
        }

    }
}
