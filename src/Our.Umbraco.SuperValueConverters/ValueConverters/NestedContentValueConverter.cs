using Our.Umbraco.SuperValueConverters.Helpers;
using Our.Umbraco.SuperValueConverters.Models;
using Our.Umbraco.SuperValueConverters.PreValues;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.PropertyEditors.ValueConverters;

namespace Our.Umbraco.SuperValueConverters.ValueConverters
{
    public class NestedContentValueConverter : SuperValueConverterBase
    {
        public NestedContentValueConverter()
            : base(new NestedContentManyValueConverter())
        {

        }

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
			// S6 Adding some of our custom DataType definitions so they are recognized as NestedContent property editors (which they contain)
			switch (propertyType.PropertyEditorAlias)
			{
				case Constants.PropertyEditors.NestedContentAlias:
				case "S6AccountingOrderLineItemsEditor":
				case "S6AccountingAddressEditor":
					return true;
					break;
				default:
					return false;
					break;
			}
            //return propertyType.PropertyEditorAlias.Equals(Constants.PropertyEditors.NestedContentAlias);
        }

        public override IPickerSettings GetSettings(PublishedPropertyType propertyType)
        {
			// S6 TODO Add prevalue to package.manifest so custom wrapper property editors can reference a DataType in the UI by key to get the settings?
			// S6 For now, just get preValues with hardcoded key to see if the rest of the workflow succeeds
			int datatypeId = -1;
			IDictionary<string, string> preValues = null;
			switch (propertyType.PropertyEditorAlias)
			{
				case "S6AccountingOrderLineItemsEditor": // Use config from 2237 S6NestedContentLineItem DataType									
					datatypeId = 2237;
					break;
				case "S6AccountingAddressEditor": // S6 Billing and Shipping Accounting Address Editor DataTypes both render an instance of S6 Nested Content Address Details Data Type: 22d6b4a4-8847-49d3-b443-b05a4bd47199, 2263
					datatypeId = 2263;
					break;
				default:
					datatypeId = propertyType.DataTypeId;
					break;
			}

			preValues = DataTypeHelper.GetPreValues(datatypeId);

			//var preValues = DataTypeHelper.GetPreValues(propertyType.DataTypeId); // Orig

			var settings = new NestedContentSettings();
			
			return PreValueMapper.Map(settings, preValues);
        }
    }
}