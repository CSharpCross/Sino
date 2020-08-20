using Sino.Web.Dependency.Aop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Web.Test
{
	[SinoInterceptor(typeof(SinoInterceptor))]
	public class CalculatorServiceWithStandartInterceptorTyped : CalculatorService
	{

	}
}
