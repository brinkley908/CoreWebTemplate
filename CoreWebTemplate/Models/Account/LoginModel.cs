using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CoreWebTemplate.Models.Account
{
    public class LoginModel : ModelBase
    {
        [Required( ErrorMessage = "Required" )]
        public string Username { get; set; }

        [Required( ErrorMessage = "Required" )]
        public string Password { get; set; }

        public  XTextBoxControlConfig UsernameConfig { get; set; } 

        public XTextBoxControlConfig PasswordConfig { get; set; }


        public static LoginModel GetModel( Controller controller, IHtmlHelper helper )
            => GetModel( null, controller, helper );

        public static LoginModel GetModel(LoginModel model, Controller controller, IHtmlHelper helper)
        {

            helper.Contextualize<LoginModel>( controller, "/Views/Account/Login.cshtml", model, true );

            if ( model == null )
            {
                model = new LoginModel
                {
                    Username = "demouser",
                    Password = "LetMeIn2020!!",
                };
            }

            model.BanerText = "Login";

            model.UsernameConfig = new XTextBoxControlConfig
            {
                PlaceHolder = "Please enter you user name",
                RequiredValue = "*",
                Validation = helper.ValidationMessage( "username" )
            };

            model.PasswordConfig = new XTextBoxControlConfig
            {
                PlaceHolder = "Please enter your password",
                RequiredValue = "*",
                Validation = helper.ValidationMessage( "password" )
            };

            return model;
        }

    }
}
