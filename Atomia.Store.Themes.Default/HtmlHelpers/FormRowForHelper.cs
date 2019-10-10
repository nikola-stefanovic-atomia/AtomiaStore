using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Atomia.Store.Themes.Default.HtmlHelpers
{
    /// <summary>
    /// Html helper extensions for generating form row HTML.
    /// </summary>
    public static class FormRowForHelper
    {
        /// <summary>
        /// Combine label, textbox and validation message, wrapped in our customary .formrow and .col2row divs.
        /// </summary>
        /// <param name="html">The class to extend</param>
        /// <param name="expression">Expression for which model property to use</param>
        /// <param name="labelText">Label text</param>
        /// <param name="required">If the field is required or not</param>
        /// <returns>Escaped HTML of the combined label, text box and validation message.</returns>
        public static MvcHtmlString FormRowFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, Boolean required)
        {
            var asterisk = required ? @"<span class=""required"">*</span>" : "";
            var label = required ? html.LabelFor(expression, labelText, new { @class = "required" }) : html.LabelFor(expression, labelText);
            var textBox = html.TextBoxFor(expression);
            var validationMessage = html.ValidationMessageFor(expression);

            return new MvcHtmlString(String.Format(@"<div class=""formrow"">
                <h5>{0}{1}</h5>
                <div class=""col2row"">{2}{3}</div>
                <br class=""clear"">
            </div>", asterisk, label, textBox, validationMessage));
        }

        /// <summary>
        /// Combine label, textbox and validation message, wrapped in our customary .formrow and .col2row divs.
        /// </summary>
        /// <param name="html">The class to extend</param>
        /// <param name="expression">Expression for which model property to use</param>
        /// <param name="labelText">Label text</param>
        /// <param name="required">If the field is required or not</param>
        /// <param name="dataBindAttribute">Value to be added to the .col2row div data-bind attribute, for knockout bindings.</param>
        /// <returns>Escaped HTML of the combined label, text box and validation message.</returns>
        public static MvcHtmlString FormRowFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, Boolean required, string dataBindAttribute)
        {
            var asterisk = required ? @"<span class=""required"">*</span>" : "";
            var label = required ? html.LabelFor(expression, labelText, new { @class = "required" }) : html.LabelFor(expression, labelText);
            var textBox = html.TextBoxFor(expression);
            var validationMessage = html.ValidationMessageFor(expression);

            return new MvcHtmlString(String.Format(@"<div class=""formrow"">
                <h5>{0}{1}</h5>
                <div class=""col2row"" data-bind=""{2}"">{3}{4}</div>
                <br class=""clear"">
            </div>", asterisk, label, dataBindAttribute, textBox, validationMessage));
        }

        /// <summary>
        /// Combine label, textbox and validation message, wrapped in our customary .formrow and .col2row divs.
        /// </summary>
        /// <param name="html">The class to extend</param>
        /// <param name="expression">Expression for which model property to use</param>
        /// <param name="labelText">Label text</param>
        /// <param name="required">If the field is required or not</param>
        /// <param name="dataBindAttribute">Value to be added to the .col2row div data-bind attribute, for knockout bindings.</param>
        /// <param name="textBoxDataBindAttribute">Value to be added to the text box data-bind attribute, for knockout bindings.</param>
        /// <returns>Escaped HTML of the combined label, text box and validation message.</returns>
        public static MvcHtmlString FormRowFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, Boolean required, string dataBindAttribute, string textBoxDataBindAttribute)
        {
            var asterisk = required ? @"<span class=""required"">*</span>" : "";
            var label = required ? html.LabelFor(expression, labelText, new { @class = "required" }) : html.LabelFor(expression, labelText);
            var textBox = html.TextBoxFor(expression,new { data_bind = textBoxDataBindAttribute });
            var validationMessage = html.ValidationMessageFor(expression);

            var divDataBind = !string.IsNullOrEmpty(dataBindAttribute) ? String.Format(@"data-bind=""{0}""", dataBindAttribute) : string.Empty;

            return new MvcHtmlString(String.Format(@"<div class=""formrow"">
                <h5>{0}{1}</h5>
                <div class=""col2row"" {2}>{3}{4}</div>
                <br class=""clear"">
            </div>", asterisk, label, divDataBind, textBox, validationMessage));
        }
    }
}
