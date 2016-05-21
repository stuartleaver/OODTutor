using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace T802.Web.Validators
{
    public class T802ValidatorFactory : AttributedValidatorFactory
    {
        public override IValidator GetValidator(Type type)
        {
            //if (type != null)
            //{
            //    var attribute = (ValidatorAttribute) Attribute.GetCustomAttribute(type, typeof (ValidatorAttribute));
            //}
            return null;
        }
    }
}