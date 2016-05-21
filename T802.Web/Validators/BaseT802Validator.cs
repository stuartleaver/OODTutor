using FluentValidation;

namespace T802.Web.Validators
{
    public abstract class BaseT802Validator<T> : AbstractValidator<T> where T : class
    {
        protected BaseT802Validator()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}