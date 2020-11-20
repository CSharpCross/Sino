using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace OrvilleX.Validation
{
    /// <summary>
    /// 验证拦截器
    /// </summary>
    public interface IValidatorInterceptor
    {
        /// <summary>
        /// 验证前
        /// </summary>
        IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext);

        /// <summary>
        /// 验证后
        /// </summary>
        ValidationResult AfterMvcValidation(ControllerContext controllerContext, IValidationContext commonContext, ValidationResult result);
    }
}
