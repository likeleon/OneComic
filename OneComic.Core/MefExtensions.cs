using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace OneComic.Core
{
    public static class MefExtensions
    {
        public static object GetExportedValueByType(this CompositionContainer container, Type type)
        {
            if (!container.ContainsAnyExportDefinitionMatches(type))
                return null;

            return container.GetExportsByType(type).FirstOrDefault().Value;
        }

        public static IEnumerable<object> GetExportedValuesByType(this CompositionContainer container, Type type)
        {
            if (!container.ContainsAnyExportDefinitionMatches(type))
                return Enumerable.Empty<object>();

            return container.GetExportsByType(type);
        }

        private static bool ContainsAnyExportDefinitionMatches(this CompositionContainer container, Type type)
        {
            var exportDefs = container.Catalog.Parts.SelectMany(p => p.ExportDefinitions);
            return exportDefs.Any(d => d.ContractName == type.FullName);
        }

        private static IEnumerable<Export> GetExportsByType(this CompositionContainer container, Type type)
        {
            var contract = AttributedModelServices.GetContractName(type);
            var definition = new ContractBasedImportDefinition(contract, contract, null, ImportCardinality.ExactlyOne, false, false, CreationPolicy.Any);
            return container.GetExports(definition);
        }
    }
}
