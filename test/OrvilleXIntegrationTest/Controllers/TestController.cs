using Microsoft.AspNetCore.Mvc;
using OrvilleXIntegrationTest.Fake;
using Sino.Web.Filters;

namespace OrvilleXIntegrationTest.Controllers
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

		public ParentModel StandardResultFilterWithNormal()
        {
			var model = new ParentModel();
            model.Child = new ChildModel() { Name = "test" };

			return model;
        }

		[StandardResultFilter(IsUse = false)]
		public ParentModel StandardResultFilterWithNoUse()
		{
			var model = new ParentModel();
			model.Child = new ChildModel() { Name = "nouse" };

			return model;
        }

		private ActionResult TestResult()
		{
			var isvalid = ModelState.IsValid;

			return Json(isvalid);
		}
	}
}
