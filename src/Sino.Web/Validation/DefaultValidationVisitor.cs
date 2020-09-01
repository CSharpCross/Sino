using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Web.Validation
{
    public class DefaultValidationVisitor : ValidationVisitor
    {
		private const string ROOT_METADATA = "_FV_ROOT_METADATA";

        public DefaultValidationVisitor(ActionContext actionContext, IModelValidatorProvider validatorProvider, ValidatorCache validatorCache, 
            IModelMetadataProvider metadataProvider, ValidationStateDictionary validationState)
            :base(actionContext, validatorProvider, validatorCache, metadataProvider, validationState)
        {
            ValidateComplexTypesIfChildValidationFails = true;
        }

		public override bool Validate(ModelMetadata metadata, string key, object model, bool alwaysValidateAtTopLevel)
		{
			Context.HttpContext.Items[ROOT_METADATA] = metadata;

			var requiredErrorsNotHandledByFv = new List<KeyValuePair<ModelStateEntry, ModelError>>();
			foreach (KeyValuePair<string, ModelStateEntry> entry in Context.ModelState)
            {
				List<ModelError> errorsToModify = new List<ModelError>();

				if (entry.Value.ValidationState == ModelValidationState.Invalid)
                {
					foreach(var err in entry.Value.Errors)
                    {
						if (err.ErrorMessage.StartsWith(ValidationBindingMetadataProvider.Prefix))
                        {
							errorsToModify.Add(err);
                        }
                    }


                }
            }

			// Apply any customizations made with the CustomizeValidatorAttribute
			if (model != null)
			{
				CacheCustomizations(Context, model, key);
			}

			var result = base.Validate(metadata, key, model, alwaysValidateAtTopLevel);

			// Re-add errors that we took out if FV didn't add a key.
			ReApplyImplicitRequiredErrorsNotHandledByFV(requiredErrorsNotHandledByFv);

			// Remove duplicates. This can happen if someone has implicit child validation turned on and also adds an explicit child validator.
			RemoveDuplicateModelstateEntries(Context);

			return result;
		}
	}
}
