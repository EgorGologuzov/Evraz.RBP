using System.Globalization;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using RBP.Services.Models;
using RBP.Services.Utils;
using RBP.Web.Models;

namespace RBP.Web.Utils
{
    public static class HtmlHelpers
    {
        public const string RedStar = "<b class='text-danger'>  *</b>";
        public static readonly string[] ChartColors = { "#e52314", "#ed7817", "#fab82e", "#007bff", "#28a745", "#6610f2", "#e83e8c", "#20c997", "#17a2b8" };

        public static string GetColor(int index) => ChartColors[index % ChartColors.Length];

        /// <summary>
        /// Метод для форматирования HTML с ковычками значением, которое тоже может содержать ковычки.
        /// В HTML допускается использование только одного типа ковычек.
        /// </summary>
        public static string SafetyInsert(this string value, string html)
        {
            if (html.Contains('\'') && value.Contains('\''))
            {
                value = value.Replace("'", "&#39;");
            }
            else if (html.Contains('"') && value.Contains('"'))
            {
                value = value.Replace("'", "&quot");
            }

            return string.Format(html, value);
        }

        public static string EmptyIfNullElse(this object? value, Func<string> elseValue)
        {
            return value is null ? string.Empty : elseValue.Invoke();
        }

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
                <input {2} {0} class='form-control' {3} {4} {7}>
                {5}
            </div>",
            name.SafetyInsert("id='{0}' name='{0}'"),
            label,
            type.SafetyInsert("type='{0}'"),
            placeholder.SafetyInsert("placeholder='{0}'"),
            regex.EmptyIfNullElse(() => regex.SafetyInsert("required regex='{0}'")),
            invalidFeedback.EmptyIfNullElse(() => $"<div class='invalid-feedback'>{invalidFeedback}</div>"),
            regex.EmptyIfNullElse(() => RedStar),
            value.EmptyIfNullElse(() => value.SafetyInsert("value='{0}'"))
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
                    <input {2} {0} class='form-control' {3} {4} {8}>
                    {5}
                </div>
            </div>",
            name.SafetyInsert("id='{0}' name='{0}'"),
            label,
            type.SafetyInsert("type='{0}'"),
            placeholder.SafetyInsert("placeholder='{0}'"),
            regex.EmptyIfNullElse(() => regex.SafetyInsert("required regex='{0}'")),
            invalidFeedback.EmptyIfNullElse(() => $"<div class='invalid-feedback'>{invalidFeedback}</div>"),
            regex.EmptyIfNullElse(() => RedStar),
            prefix,
            value.EmptyIfNullElse(() => $"value='{value}'")
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
                <input {2} {0} class='form-control' {3} {4} {6} {7}>
                {5}
            </div>",
            name.SafetyInsert("id='{0}' name='{0}'"),
            string.IsNullOrEmpty(label) ? string.Empty : LabelHtml(label, regex is not null),
            type.SafetyInsert("type='{0}'"),
            placeholder.SafetyInsert("placeholder='{0}'"),
            regex.EmptyIfNullElse(() => regex.SafetyInsert("required regex='{0}'")),
            invalidFeedback.EmptyIfNullElse(() => $"<div class='invalid-feedback'>{invalidFeedback}</div>"),
            value.EmptyIfNullElse(() => value.SafetyInsert("value='{0}'")),
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
                    <input {2} {0} class='form-control' {3} {4} {8}>
                    {5}
                </div>
            </div>",
            name.SafetyInsert("id='{0}' name='{0}'"),
            label,
            type.SafetyInsert("type='{0}'"),
            placeholder.SafetyInsert("placeholder='{0}'"),
            regex.EmptyIfNullElse(() => regex.SafetyInsert("required regex='{0}'")),
            invalidFeedback.EmptyIfNullElse(() => $"<div class='invalid-feedback'>{invalidFeedback}</div>"),
            regex.EmptyIfNullElse(() => RedStar),
            prefix,
            value.EmptyIfNullElse(() => $"value='{value}'")
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
                <input type='date' {1} class='form-control' {2} {3} {4} {5} {7}>
            </div>",
            label,
            name.SafetyInsert("id='{0}' name='{0}'"),
            value is null ? DateTime.Now.ToString("yyyy-MM-dd").SafetyInsert("value='{0}'") : value.Value.ToString("yyyy-MM-dd").SafetyInsert("value='{0}'"),
            min.EmptyIfNullElse(() => min.Value.ToString("yyyy-MM-dd").SafetyInsert("min='{0}'")),
            max.EmptyIfNullElse(() => max.Value.ToString("yyyy-MM-dd").SafetyInsert("max='{0}'")),
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
                <select class='custom-select mr-sm-2' {1} {4}>
                    {2}
                </select>
            </div>",
            label,
            name.SafetyInsert("id='{0}' name='{0}'"),
            string.Concat(items.Select(i => string.Format(
                "<option {0} {1}>{2}</option>",
                i.Value == value ? "selected" : string.Empty,
                i.Value.SafetyInsert("value='{0}'"),
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
                <textarea class='form-control' {1} rows='3' {3} {5} {6}>{2}</textarea>
            </div>",
            label,
            name.SafetyInsert("id='{0}' name='{0}'"),
            value,
            isRequired ? "required" : string.Empty,
            isRequired ? RedStar : string.Empty,
            maxLength.EmptyIfNullElse(() => $"maxlength='{maxLength.Value}'"),
            isEnabled ? string.Empty : "disabled"
            ));
        }

        public static IHtmlContent HiddenInput(this IHtmlHelper _,
            string name,
            string? value = null)
        {
            return new HtmlString(string.Format(@"
            <input type='hidden' {0} {1} />",
            name.SafetyInsert("id='{0}' name='{0}'"),
            value.EmptyIfNullElse(() => value.SafetyInsert("value='{0}'"))
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
            <form {4} {0} class='my-auto' style='max-width: 550px; display: flex; flex-direction: row;'>
                <div class='form-group my-0 mr-2' style='flex: 1; max-width: 550px;'>
                    <input {2} type='text' class='form-control' {1} {3}>
                </div>
                <button type='submit' class='btn btn-primary'>Найти</button>
            </form>",
            action.SafetyInsert("action='{0}'"),
            placeholder.SafetyInsert("placeholder='{0}'"),
            name.SafetyInsert("id='{0}' name='{0}'"),
            value.EmptyIfNullElse(() => value.SafetyInsert("value='{0}'")),
            method.SafetyInsert("method='{0}'")
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
            IEnumerable<StringIntPare> data,
            string? title = null)
        {
            decimal sum = data.Sum(p => Math.Abs(p.Value));

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
                <div style=""flex: {0}; background-color: {1};"" data-toggle=""tooltip"" data-placement=""bottom"" {2}>
                </div>",
                (Math.Abs(p.Value) / sum).ToString("0.000", CultureInfo.InvariantCulture),
                GetColor(i),
                $"{p.Key} ({p.Value})".SafetyInsert("title='{0}'")
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
           IEnumerable<StringIntPare> data)
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
            IEnumerable<StringIntPare> data)
        {
            return new HtmlString(string.Format(@"
            <div style=""position: relative; width: 100%; height: 300px;"">
                <canvas {2} class=""chart mx-2 mb-2"" class=""my-1 mx-2""
                    chart-labels='{0}'
                    chart-data='{1}' chart-config=""doughnut""></canvas>
            </div>",
            data.Select(kvp => kvp.Key).ToJson(),
            data.Select(kvp => kvp.Value).ToJson(),
            name.SafetyInsert("id='{0}' name='{0}'")
            ));
        }
    }
}