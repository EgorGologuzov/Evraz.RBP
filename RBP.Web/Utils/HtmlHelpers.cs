using System.Globalization;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using RBP.Services.Utils;
using RBP.Web.Models;

namespace RBP.Web.Utils
{
    public static class HtmlHelpers
    {
        public const string RedStar = "<b class='text-danger'>  *</b>";
        public static readonly string[] ChartColors = { "#e52314", "#ed7817", "#fab82e", "#007bff", "#28a745", "#6610f2", "#e83e8c", "#20c997", "#17a2b8" };

        public static string GetColor(int index) => ChartColors[index % ChartColors.Length];

        public static IHtmlContent TextBox(this IHtmlHelper _,
            string name,
            string label,
            string placeholder = "",
            string? value = null,
            string type = "text",
            string? regex = null,
            string? invalidFeedback = null)
        {
            return new HtmlString(string.Format(@"
            <div class='form-group'>
                <label for='{0}'>{1}{6}</label>
                <input type='{2}' id='{0}' name='{0}' class='form-control' placeholder='{3}' {4} {7}>
                {5}
            </div>",
            name,
            label,
            type,
            placeholder,
            regex is null ? string.Empty : $"required regex='{regex}'",
            invalidFeedback is null ? string.Empty : $"<div class='invalid-feedback'>{invalidFeedback}</div>",
            regex is null ? string.Empty : RedStar,
            value is null ? string.Empty : $"value='{value}'"
            ));
        }

        public static string LabelHtml(
            string text,
            bool isRequired)
        {
            return string.Format(@"
            <label>{0}{1}</label>",
            text,
            isRequired ? RedStar : string.Empty
            );
        }

        public static IHtmlContent PrefixTextBox(this IHtmlHelper _,
            string name,
            string label,
            string prefix,
            string placeholder = "",
            string? value = null,
            string type = "text",
            string? regex = null,
            string? invalidFeedback = null)
        {
            return new HtmlString(string.Format(@"
            <div class='form-group'>
                <label for='{0}'>{1}{6}</label>
                <div class='input-group'>
                    <div class='input-group-prepend'>
                        <div class='input-group-text'>{7}</div>
                    </div>
                    <input type='{2}' id='{0}' name='{0}' class='form-control' placeholder='{3}' {4} {8}>
                    {5}
                </div>
            </div>",
            name,
            label,
            type,
            placeholder,
            regex is null ? string.Empty : $"required regex='{regex}'",
            invalidFeedback is null ? string.Empty : $"<div class='invalid-feedback'>{invalidFeedback}</div>",
            regex is null ? string.Empty : RedStar,
            prefix,
            value is null ? string.Empty : $"value='{value}'"
            ));
        }

        public static IHtmlContent AdaptiveTextBox(this IHtmlHelper _,
            string name,
            string label = null,
            string placeholder = "",
            string? value = null,
            string type = "text",
            string? regex = null,
            string? invalidFeedback = null,
            string bootstrapGridModes = "col-sm-12 col-md-6 col-lg-4",
            bool isEnabled = true)
        {
            return new HtmlString(string.Format(@"
            <div class='form-group {8}'>
                {1}
                <input type='{2}' id='{0}' name='{0}' class='form-control' placeholder='{3}' {4} {6} {7}>
                {5}
            </div>",
            name,
            string.IsNullOrEmpty(label) ? string.Empty : LabelHtml(label, regex is not null),
            type,
            placeholder,
            regex is null ? string.Empty : $"required regex='{regex}'",
            invalidFeedback is null ? string.Empty : $"<div class='invalid-feedback'>{invalidFeedback}</div>",
            value is null ? string.Empty : $"value='{value}'",
            isEnabled ? string.Empty : "disabled",
            bootstrapGridModes
            ));
        }

        public static IHtmlContent AdaptivePrefixTextBox(this IHtmlHelper _,
            string name,
            string label,
            string prefix,
            string placeholder = "",
            string? value = null,
            string type = "text",
            string? regex = null,
            string? invalidFeedback = null)
        {
            return new HtmlString(string.Format(@"
            <div class='form-group col-sm-12 col-md-6 col-lg-4'>
                <label for='{0}'>{1}{6}</label>
                <div class='input-group'>
                    <div class='input-group-prepend'>
                        <div class='input-group-text'>{7}</div>
                    </div>
                    <input type='{2}' id='{0}' name='{0}' class='form-control' placeholder='{3}' {4} {8}>
                    {5}
                </div>
            </div>",
            name,
            label,
            type,
            placeholder,
            regex is null ? string.Empty : $"required regex='{regex}'",
            invalidFeedback is null ? string.Empty : $"<div class='invalid-feedback'>{invalidFeedback}</div>",
            regex is null ? string.Empty : RedStar,
            prefix,
            value is null ? string.Empty : $"value='{value}'"
            ));
        }

        public static IHtmlContent AdaptiveDateInput(this IHtmlHelper _,
            string name,
            string label,
            DateTime? value = null,
            DateTime? min = null,
            DateTime? max = null,
            bool isRequired = false,
            bool isEnabled = true,
            string bootstrapGridModes = "col-sm-12 col-md-6 col-lg-4")
        {
            return new HtmlString(string.Format(@"
            <div class='form-group {8}'>
                <label>{0}{6}</label>
                <input type='date' name='{1}' class='form-control' {2} {3} {4} {5} {7}>
            </div>",
            label,
            name,
            value is null ? string.Format("value='{0}'", DateTime.Now.ToString("yyyy-MM-dd")) : string.Format("value='{0}'", value.Value.ToString("yyyy-MM-dd")),
            min is null ? string.Empty : string.Format("min='{0}'", min.Value.ToString("yyyy-MM-dd")),
            max is null ? string.Empty : string.Format("max='{0}'", max.Value.ToString("yyyy-MM-dd")),
            isRequired ? "required" : string.Empty,
            isRequired ? RedStar : string.Empty,
            isEnabled ? string.Empty : "disabled",
            bootstrapGridModes
            ));
        }

        public static IHtmlContent AdaptiveSelect(this IHtmlHelper _,
            string name,
            string label,
            IEnumerable<KeyValuePair<string, string>> items,
            string? value = null,
            string bootstrapGridModes = "col-sm-12 col-md-6 col-lg-4",
            bool isEnabled = true)
        {
            return new HtmlString(string.Format(@"
            <div class='form-group {3}'>
                <label>{0}</label>
                <select class='custom-select mr-sm-2' name='{1}' {4}>
                    {2}
                </select>
            </div>",
            label,
            name,
            string.Concat(items.Select(i => string.Format(
                "<option {0} value='{1}'>{2}</option>",
                i.Value == value ? "selected" : string.Empty,
                i.Value,
                i.Key
                ))),
            bootstrapGridModes,
            isEnabled ? string.Empty : "disabled"
            ));
        }

        public static IHtmlContent AdaptiveTextarea(this IHtmlHelper _,
            string name,
            string label,
            string? value = null,
            bool isRequired = false,
            int? maxLength = null,
            bool isEnabled = true)
        {
            return new HtmlString(string.Format(@"
            <div class='form-group col-12'>
                <label>{0}{4}</label>
                <textarea class='form-control' name='{1}' rows='3' {3} {5} {6}>{2}</textarea>
            </div>",
            label,
            name,
            value,
            isRequired ? "required" : string.Empty,
            isRequired ? RedStar : string.Empty,
            maxLength is null ? string.Empty : $"maxlength='{maxLength.Value}'",
            isEnabled ? string.Empty : "disabled"
            ));
        }

        public static IHtmlContent HiddenInput(this IHtmlHelper _,
            string name,
            string? value = null)
        {
            return new HtmlString(string.Format(@"
            <input type='hidden' id={0} name='{0}' value='{1}' />",
            name,
            value
            ));
        }

        public static IHtmlContent ErrorMessage(this IHtmlHelper _,
            string message)
        {
            return new HtmlString(string.Format(@"
            <div class='card error-message border-danger mt-0 mb-1'>
                <div class='card-body text-danger px-3 py-1'>
                    <p class='card-text small'>{0}</p>
                </div>
            </div>",
            message
            ));
        }

        public static IHtmlContent OkMessage(this IHtmlHelper _,
            string message)
        {
            return new HtmlString(string.Format(@"
            <div class='card error-message border-success mt-0 mb-1'>
                <div class='card-body text-success px-3 py-1'>
                    <p class='card-text small'>{0}</p>
                </div>
            </div>",
            message
            ));
        }

        public static IHtmlContent EmptyView(this IHtmlHelper _)
        {
            return new HtmlString(string.Format(@"
            <div class='card border-danger w-100'>
                <span class='text-danger mx-auto'>Ничего нет</span>
            </div>"
            ));
        }

        public static IHtmlContent SearchForm(this IHtmlHelper _,
            string action,
            string placeholder,
            string? value = null,
            string name = "SearchRequest",
            string method = "get")
        {
            return new HtmlString(string.Format(@"
            <form method='{4}' action='{0}' class='my-auto' style='max-width: 550px; display: flex; flex-direction: row;'>
                <div class='form-group my-0 mr-2' style='flex: 1; max-width: 550px;'>
                    <input name='{2}' type='text' class='form-control' placeholder='{1}' value='{3}'>
                </div>
                <button type='submit' class='btn btn-primary'>Найти</button>
            </form>",
            action,
            placeholder,
            name,
            value,
            method
            ));
        }

        public static IHtmlContent ListInputForm(this IHtmlHelper _,
            string targetInputId,
            string viewKey,
            string viewValue,
            string caption,
            IList<IHtmlContent> controls,
            bool isEnabled = true)
        {
            return new HtmlString(string.Format(@"
            <form id=""{0}-form"" class=""list-input py-2"" {5}>
                <h4 class=""text-muted"">{3}</h4>
            
                <input type=""hidden"" id=""{0}-form-target-input"" value=""{0}"">
                <input type=""hidden"" id=""{0}-form-map"" value='{{""key"":""{1}"",""value"":""{2}""}}'>
                <div style=""height: 10px;""></div>
                <div id=""{0}-form-list"" class=""d-inline""></div>
                <div style=""height: 15px;""></div>
                <div class=""form-row"">
                    {4}
                </div>
                <div class=""form-row"" style=""margin: 0px;"">
                    <button class=""btn btn-primary ml-auto"" type=""button"" onclick=""addItem_ILS('{0}-form')"" {5}>Добавить</button>
                </div>
            </form>",
            targetInputId,
            viewKey,
            viewValue,
            caption,
            controls is null || controls.Count == 0 ? string.Empty : string.Concat(controls.Select(ToHtml)),
            isEnabled ? string.Empty : "disabled"
            ));
        }

        public static string ToHtml(this IHtmlContent content)
        {
            using StringWriter writer = new();
            content.WriteTo(writer, HtmlEncoder.Default);

            return writer.ToString();
        }

        public static IHtmlContent DistributionBar(this IHtmlHelper _,
            IEnumerable<KeyValuePair<string, decimal>> data,
            string? title = null)
        {
            decimal sum = data.Sum(p => p.Value);

            return new HtmlString(string.Format(@"
            <div>
                {0}
                <div class=""d-flex flex-row bg-light"" style=""height: 25px;"">
                    {1}
                </div>
                <div class=""d-inline"">
                    {2}
                </div>
            </div>",
            title is null ? string.Empty : $"<h6 class=\"card-subtitle mb-1 text-muted m-0\">{title}</h6>",
            string.Concat(data.Select((p, i) => string.Format(@"
                <div style=""flex: {0}; background-color: {1};"" data-toggle=""tooltip"" data-placement=""bottom"" title=""{2}"">
                </div>",
                (p.Value / sum).ToString("0.000", CultureInfo.InvariantCulture),
                GetColor(i),
                $"{p.Key} ({Math.Round(p.Value, 1)})"
                ))),
            string.Concat(data.Select((p, i) => string.Format(@"
                <div class=""d-inline-block mt-1 mr-1"">
                    <div class=""d-flex flex-row align-items-center"">
                        <div class=""d-inline rounded-circle mr-1 my-auto"" style=""width: 15px; height: 15px; background-color: {0};"">&nbsp</div>
                        <p class=""m-0 small d-inline"">{1}</p>
                    </div>
                </div>
                ",
                GetColor(i),
                p.Key
                )))
            ));
        }

        public static IHtmlContent ChartOneLine(this IHtmlHelper _,
           IEnumerable<KeyValuePair<string, decimal>> data)
        {
            return new HtmlString(string.Format(@"
            <div style=""position: relative; width: 100%; height: 230px;"">
                <canvas class=""chart mx-2 mb-2"" class=""my-1 mx-2""
                        chart-labels='{0}'
                        chart-data='{1}' chart-config=""one-line""></canvas>
            </div>",
            data.Select(kvp => kvp.Key).ToJson(),
            data.Select(kvp => kvp.Value).ToJson()
            ));
        }

        public static IHtmlContent ChartDoughnut(this IHtmlHelper _,
            string name,
            IEnumerable<KeyValuePair<string, decimal>> data)
        {
            return new HtmlString(string.Format(@"
            <div style=""position: relative; width: 100%; height: 300px;"">
                <canvas id=""{2}"" class=""chart mx-2 mb-2"" class=""my-1 mx-2""
                    chart-labels='{0}'
                    chart-data='{1}' chart-config=""doughnut""></canvas>
            </div>",
            data.Select(kvp => kvp.Key).ToJson(),
            data.Select(kvp => kvp.Value).ToJson(),
            name
            ));
        }
    }
}