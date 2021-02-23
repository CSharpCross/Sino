using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.Linq;

namespace OrvilleX.Validation
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

					foreach (ModelError err in errorsToModify)
                    {
						entry.Value.Errors.Clear();
						entry.Value.ValidationState = ModelValidationState.Unvalidated;
						requiredErrorsNotHandledByFv.Add(new KeyValuePair<ModelStateEntry, ModelError>(entry.Value,
							new ModelError(err.ErrorMessage.Replace(ValidationBindingMetadataProvider.Prefix, string.Empty))));
                    }
                }
            }

			var result = base.Validate(metadata, key, model, alwaysValidateAtTopLevel);

			foreach (var pair in requiredErrorsNotHandledByFv)
            {
				if (pair.Key.ValidationState != ModelValidationState.Invalid)
                {
					pair.Key.Errors.Add(pair.Value);
					pair.Key.ValidationState = ModelValidationState.Invalid;
                }
            }

			foreach (var entry in Context.ModelState)
            {
				if (entry.Value.ValidationState == ModelValidationState.Invalid)
                {
					var existing = new HashSet<string>();

					foreach (var err in entry.Value.Errors.ToList())
                    {
						if (existing.Contains(err.ErrorMessage))
                        {
							entry.Value.Errors.Remove(err);
                        }
						else
                        {
							existing.Add(err.ErrorMessage);
                        }
                    }
                }
            }

			return result;
		}
	}
}
