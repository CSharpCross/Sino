using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrvilleX.Validation
{
	/// <summary>
	/// asp.net core模型验证提供器
	/// </summary>
    public class ValidationModelValidatorProvider : IModelValidatorProvider
    {
		private readonly bool _implicitValidationEnabled;

		public ValidationModelValidatorProvider(bool implicitValidationEnabled)
		{
			_implicitValidationEnabled = implicitValidationEnabled;
		}

		public virtual void CreateValidators(ModelValidatorProviderContext context)
		{
			context.Results.Add(new ValidatorItem
			{
				IsReusable = false,
				Validator = new ValidationModelValidator(_implicitValidationEnabled)
			});
		}
	}

	/// <summary>
	/// 自定义模型验证
	/// </summary>
	public class ValidationModelValidator : IModelValidator
	{
		private readonly bool _implicitValidationEnabled;

		public ValidationModelValidator(bool implicitValidationEnabled)
		{
			_implicitValidationEnabled = implicitValidationEnabled;
		}

		public virtual IEnumerable<ModelValidationResult> Validate(ModelValidationContext mvContext)
		{
			if (ShouldSkip(mvContext))
			{
				return Enumerable.Empty<ModelValidationResult>();
			}

			var factory = mvContext.ActionContext.HttpContext.RequestServices.GetService(typeof(IValidatorFactory)) as IValidatorFactory;
			var validator = factory?.GetValidator(mvContext.ModelMetadata.ModelType);

			if (validator != null)
			{
				var selector = mvContext.ActionContext.HttpContext.RequestServices.GetRequiredService<ValidatorConfiguration>().ValidatorSelectors.DefaultValidatorSelectorFactory();
				var interceptor = validator as IValidatorInterceptor
								  ?? mvContext.ActionContext.HttpContext.RequestServices.GetService<IValidatorInterceptor>();

				IValidationContext context = new ValidationContext<object>(mvContext.Model, new PropertyChain(), selector);
				context.RootContextData["InvokedByMvc"] = true;
				context.SetServiceProvider(mvContext.ActionContext.HttpContext.RequestServices);

				if (interceptor != null && mvContext.ActionContext is ControllerContext)
				{
					context = interceptor.BeforeMvcValidation((ControllerContext)mvContext.ActionContext, context) ?? context;
				}
				var result = validator.Validate(context);

				if (interceptor != null && mvContext.ActionContext is ControllerContext)
				{
					result = interceptor.AfterMvcValidation((ControllerContext)mvContext.ActionContext, context, result) ?? result;
				}

				return result.Errors.Select(x => new ModelValidationResult(x.PropertyName, x.ErrorMessage));
			}

			return Enumerable.Empty<ModelValidationResult>();
		}

		protected bool ShouldSkip(ModelValidationContext mvContext)
		{
			if (mvContext.Model == null)
			{
				return true;
			}

			if (!_implicitValidationEnabled)
			{

				var rootMetadata = GetRootMetadata(mvContext);

				if (rootMetadata == null) return true;

				if (mvContext.ModelMetadata.MetadataKind == ModelMetadataKind.Property)
				{
					if (!ReferenceEquals(rootMetadata, mvContext.ModelMetadata))
					{
						return true;
					}
				}
				else if (mvContext.ModelMetadata.MetadataKind == ModelMetadataKind.Type)
				{
					if (!ReferenceEquals(rootMetadata, mvContext.ModelMetadata))
					{
						return true;
					}
				}
			}

			return false;
		}

		protected static ModelMetadata GetRootMetadata(ModelValidationContext mvContext)
		{
			if (mvContext.ActionContext.HttpContext.Items
				.TryGetValue("_FV_ROOT_METADATA", out var rootMetadata))
			{
				return rootMetadata as ModelMetadata;
			}

			return null;
		}
	}
}
