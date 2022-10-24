using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TestApp.Controls.Lookup
{
    public class LookupMetadataExtension : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, 
            Func<object> modelAccessor, Type modelType, string propertyName)
        {
// ReSharper disable PossibleMultipleEnumeration
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
// ReSharper restore PossibleMultipleEnumeration
// ReSharper disable PossibleMultipleEnumeration
            var additionalValues = attributes.OfType<LookupAttribute>().FirstOrDefault();
// ReSharper restore PossibleMultipleEnumeration

            if (additionalValues != null)
            {
                metadata.AdditionalValues.Add(LookupConsts.LookupMetadata, additionalValues);
            }
            return metadata;
        }
    }
}