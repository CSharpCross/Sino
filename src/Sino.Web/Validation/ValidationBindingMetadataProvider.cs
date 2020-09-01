using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Sino.Web.Validation
{
    /// <summary>
    /// 模型绑定提供器
    /// </summary>
    public class ValidationBindingMetadataProvider : IBindingMetadataProvider
    {
		public const string Prefix = "_FV_REQUIRED|";

		public void CreateBindingMetadata(BindingMetadataProviderContext context)
		{
			if (context.Key.MetadataKind == ModelMetadataKind.Property)
			{
				var original = context.BindingMetadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor;
				context.BindingMetadata.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(s => Prefix + original(s));
			}
		}
	}
}
