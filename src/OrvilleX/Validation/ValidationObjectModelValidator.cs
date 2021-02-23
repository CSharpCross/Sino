using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.Linq;

namespace OrvilleX.Validation
{
    public class ValidationObjectModelValidator : ObjectModelValidator
    {
		private readonly bool _runMvcValidation;
		private readonly ValidationModelValidatorProvider _fvProvider;

		public ValidationObjectModelValidator(
			IModelMetadataProvider modelMetadataProvider,
			IList<IModelValidatorProvider> validatorProviders, bool runMvcValidation)
		: base(modelMetadataProvider, validatorProviders)
		{
			_runMvcValidation = runMvcValidation;
			_fvProvider = validatorProviders.SingleOrDefault(x => x is ValidationModelValidatorProvider) as ValidationModelValidatorProvider;
		}

		public override ValidationVisitor GetValidationVisitor(ActionContext actionContext, IModelValidatorProvider validatorProvider, ValidatorCache validatorCache, 
			IModelMetadataProvider metadataProvider, ValidationStateDictionary validationState)
		{
			var validatorProviderToUse = _runMvcValidation ? validatorProvider : _fvProvider;

			var visitor = new DefaultValidationVisitor(
				actionContext,
				validatorProviderToUse,
				validatorCache,
				metadataProvider,
				validationState);

			return visitor;
		}
	}
}
