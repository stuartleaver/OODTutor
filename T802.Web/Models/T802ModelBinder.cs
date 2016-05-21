using System.Web.Mvc;

namespace T802.Web.Models
{
    public class T802ModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model is BaseT802Model)
            {
                ((BaseT802Model)model).BindModel(controllerContext, bindingContext);
            }
            return model;
        }
    }
}
