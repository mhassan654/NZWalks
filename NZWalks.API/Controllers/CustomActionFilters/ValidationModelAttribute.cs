using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.API.Controllers.CustomActionFilters
{
    public class ValidationModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext content)
        {
            if (content.ModelState.IsValid == false)
            {
                content.Result = new BadRequestResult();
            }
        }
    }
}
