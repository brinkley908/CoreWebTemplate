using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Reflection;
using CoreWebTemplate.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace CoreWebTemplate
{
    public static class HtmlExtensions
    {
        private const string Id = "id";
        private const string Value = "value";
        private const string Label = "label";
        private const string Border = "border";
        private const string Type = "type";
        private const string Alert = "alert";
        private const string Mandatory = "mandatory";
        private const string ReadOnly = "readonly";
        private const string TypeText = "text";
        private const string TrueValue = "True";
        private const string FalseValue = "False";
        private const string PlaceHolder = "placeholder";

        private const string BorderAttribute = Border;
        private const string LabelAttribute = Label;
        private const string TypeAttribute = Type;
        private const string AlertAttribute = Alert;
        private const string MandatoryAttribute = Mandatory;
        private const string PlaceHolderAttribute = PlaceHolder;
        private static readonly string[] ExcludeAttributes = { BorderAttribute, LabelAttribute, TypeAttribute, AlertAttribute, MandatoryAttribute, PlaceHolderAttribute };

        private const string SingleQuote = "'";
        private const string DoubleQuote = "\"";

        private const string ctxNoBorder = "ctx-text-box-no-border";
        private const string ctxBorder = "ctx-text-box-border";
        private const string ctxReadonlyBorder = "ctx-text-box-ro-border";


        public static IHtmlContent XTextBox( this IHtmlHelper htmlHelper, string name, string value, string type = "text", XTextBoxControlConfig config = null )
        {

            if ( config == null )
                config = new XTextBoxControlConfig { BorderClass = ctxBorder };

     

           
            var str = @"
                            <div class='{containerclass}'>
                                    <div class='{wrapperclass} {borderclass}'>
                                        <div class='group'>
                                            <div class='{labelclass}' for='{labelfor}'>{labelvalue}</div>
                                            <div class='{requiredclass}'>{requiredvalue}</div>
                                            <div class='{validationclass}'>{validationvalue}</div>
                                        </div>
                                        <input type='{inputtype}' {attributes} name='{name}' id={id} value='{value}' placeholder='{placeholder}'  /> 
                                        <div class='{anchorclass}'>
                                    </div>
                                    <div class='{autocompleteclass}'></div>
                            </div>
                        ";

            str = str
                .Replace( "{containerclass}", config.ContainerClass ?? string.Empty )
                .Replace( "{wrapperclass}", config.WrapperClass ?? string.Empty )
                .Replace( "{borderclass}", config.BorderClass ?? string.Empty )
                .Replace( "{labelclass}", config.LabelClass ?? string.Empty )
                .Replace( "{labelfor}", name )
                .Replace( "{labelvalue}", config?.Label ?? name )
                .Replace( "{id}", name )
                .Replace( "{name}", name )
                .Replace( "{value}", value )
                .Replace( "{inputtype}", type ?? "text" )
                .Replace( "{requiredclass}", config.RequiredClass ?? string.Empty )
                .Replace( "{requiredvalue}", config.RequiredValue ?? string.Empty )
                .Replace( "{validationclass}", config.ValidationMessageClass ?? string.Empty )
                .Replace( "{validationvalue}", config?.Validation.ToHtml() ) 
                .Replace( "{attributes}", config.Attributes )
                .Replace( "{placeholder}", config.PlaceHolder )
                .Replace( "{anchorclass}", config.AnchorClass )
                .Replace( "{autocompleteclass}", config.AutoCompleteClass );


            return new HtmlString( str );
        }


        private static string RemoveAttributes( string[] attr )
        => string.Join( " ", attr.Where( x => !ExcludeAttributes.Any( r => x.ToLower().Contains( r ) ) ) ).Replace( SingleQuote, string.Empty );

        private static string GetAttribute( string name, string[] attributes, string defaultValue )
        {
            if ( attributes == null || attributes.Length == 0 )
                return defaultValue;

            foreach ( var attribute in attributes )
            {
                var valuePair = attribute.Split( '=' );
                if ( valuePair == null || valuePair.Length != 2 )
                    continue;

                if ( name.Equals( valuePair[0], StringComparison.OrdinalIgnoreCase ) )
                    return valuePair[1];

            }

            return defaultValue;
        }

        public static async Task<string> RenderViewAsync<TModel>( this Controller controller, string viewName, TModel model, bool partial = false )
        {
            if ( string.IsNullOrEmpty( viewName ) )
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using var writer = new StringWriter();
            IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService( typeof( ICompositeViewEngine ) ) as ICompositeViewEngine;
            ViewEngineResult viewResult = viewEngine.GetView( viewName, viewName, !partial );

            if ( viewResult.Success == false )
            {
                return $"A view with the name {viewName} could not be found";
            }

            ViewContext viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync( viewContext );

            return writer.GetStringBuilder().ToString();


        }

        public static void Contextualize<TModel>( this IHtmlHelper helper, Controller controller, string viewName, TModel model, bool partial = false )
        {

            using var writer = new StringWriter();
            IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService( typeof( ICompositeViewEngine ) ) as ICompositeViewEngine;
            ViewEngineResult viewResult = viewEngine.GetView( viewName, viewName, !partial );

            ViewContext viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions()
            );


            ( helper as IViewContextAware ).Contextualize( viewContext );

        }

        public static string ToHtml(this IHtmlContent value)
        {
            using var writer = new StringWriter();
            ( value as TagBuilder ).WriteTo( writer, HtmlEncoder.Default );

            return writer.GetStringBuilder().ToString();

        }


    }



    public class XTextBoxControlConfig
    {
        public string ContainerClass { get; set; } = "ctx-text-box-container";
        public string WrapperClass { get; set; } = "ctx-text-box--wrapper";
        public string BorderClass { get; set; } = "ctx-text-box-border";
        public string LabelClass { get; set; } = "lable";
        public string RequiredClass { get; set; } = "ctx-required";
        public string ValidationMessageClass { get; set; } = "ctx-validation-message";
        public string AnchorClass { get; set; } = "ctx-input-anchor";
        public string AutoCompleteClass { get; set; } = "ctx-text-box-autocomplete";
        public string Attributes { get; set; } = string.Empty;
        public string PlaceHolder { get; set; } = string.Empty;
        public string RequiredValue { get; set; } = string.Empty;
        public IHtmlContent Validation { get; set; }
        public string Label { get; set; }
    }


}
