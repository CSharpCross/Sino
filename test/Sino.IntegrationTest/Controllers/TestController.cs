using Microsoft.AspNetCore.Mvc;
using Sino.IntegrationTest.Fake;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.IntegrationTest.Controllers
{
    public class TestController : Controller
    {
		public ActionResult InjectsExplicitChildValidator(ParentModel model)
		{
			return TestResult();
		}

		public ActionResult InjectsExplicitChildValidatorCollection(ParentModel6 model)
		{
			return TestResult();
		}

		private ActionResult TestResult()
		{
			var isvalid = ModelState.IsValid;

			return Json(isvalid);
		}
	}
}
