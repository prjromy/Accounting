using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ChannakyaAccounting.Helper
{
    public static class HtmlHelperExtensionTest
    {
        public static MvcHtmlString EditorForCustomerSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression,int cId,string custName=""

         ) where TModel : class
        {



            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var htmlFieldIdWithPrefix = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            var value = ModelMetadata.FromLambdaExpression(propertyExpression, html.ViewData).Model;
            string cName="";
            if(custName!="")
            {
                cName = custName;
            }


            htmlBuilder.AppendFormat(@"<div class='input-group section-customer' id=""{0}"">", htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, cId);
            htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt' value=""{0}""id=""{1}"" placeholder='Search...' style='max-width:1000px;'>", cName, htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-customer-popup'>");
            htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
            htmlBuilder.AppendFormat(@"</button>");
            htmlBuilder.AppendFormat(@"</span>");
            htmlBuilder.AppendFormat(@"</div>");

            return new MvcHtmlString(htmlBuilder.ToString());

        }


        public static MvcHtmlString EditorForEmployeeSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression, int empId, string empName = ""

        ) where TModel : class
        {



            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var htmlFieldIdWithPrefix = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            var value = ModelMetadata.FromLambdaExpression(propertyExpression, html.ViewData).Model;
            string cName = "";
            if (empName != "")
            {
                cName = empName;
            }


            htmlBuilder.AppendFormat(@"<div class='input-group section-customer' id=""{0}"">", htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, empId);
            htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt' value=""{0}""id=""{1}"" placeholder='Search...' style='max-width:1000px;'>", cName, htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-employee-popup'>");
            htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
            htmlBuilder.AppendFormat(@"</button>");
            htmlBuilder.AppendFormat(@"</span>");
            htmlBuilder.AppendFormat(@"</div>");

            return new MvcHtmlString(htmlBuilder.ToString());

        }

        public static MvcHtmlString EditorForUserSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression, int usrId, string usrName = ""

      ) where TModel : class
        {



            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var htmlFieldIdWithPrefix = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            var value = ModelMetadata.FromLambdaExpression(propertyExpression, html.ViewData).Model;
            string cName = "";
            if (usrName != "")
            {
                cName = usrName;
            }


            htmlBuilder.AppendFormat(@"<div class='input-group section-customer' id=""{0}"">", htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix,usrId);
            htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt' value=""{0}""id=""{1}"" placeholder='Search...' style='max-width:1000px;'>", cName, htmlFieldIdWithPrefix);
            htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-user-popup'>");
            htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
            htmlBuilder.AppendFormat(@"</button>");
            htmlBuilder.AppendFormat(@"</span>");
            htmlBuilder.AppendFormat(@"</div>");

            return new MvcHtmlString(htmlBuilder.ToString());

        }

        public static MvcHtmlString GenerateSubsiTypeList<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression, int subsiId,string placeholderName

            ) where TModel : class
        {

            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var htmlFieldIdWithPrefix = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            // var value = ModelMetadata.FromLambdaExpression(propertyExpression, html.ViewData).Model;

            var value = subsiId;


            if (subsiId == 1)
            {

                htmlBuilder.AppendFormat(@"<div class='input-group section-subsi' id=""{0}"">", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, null);
                htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt-subsi' value=""{0}"" autocomplete='off'id=""{1}"" placeholder='Search with Employee Name or Number' style='max-width:1000px;'>", "", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
                htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-search-subsi'
                         subsiId=""{0}"" placeholder=""{1}"">", value,placeholderName);
                htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
                htmlBuilder.AppendFormat(@"</button>");
                htmlBuilder.AppendFormat(@"</span>");
                htmlBuilder.AppendFormat(@"</div>");
            }
            if(subsiId==2)
            {

                htmlBuilder.AppendFormat(@"<div class='input-group section-subsi' id=""{0}"">", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, null);
                htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt-subsi' value=""{0}"" autocomplete='off'id=""{1}"" placeholder='Search with User Name' style='max-width:1000px;'>", "", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
                htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-search-subsi'
                         subsiId=""{0}"" placeholder=""{1}"">", value, placeholderName);
                htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
                htmlBuilder.AppendFormat(@"</button>");
                htmlBuilder.AppendFormat(@"</span>");
                htmlBuilder.AppendFormat(@"</div>");
            }

            if (subsiId == 3 || subsiId==4 || subsiId==5)
            {

                htmlBuilder.AppendFormat(@"<div class='input-group section-subsi' id=""{0}"">", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, null);
                htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt-subsi' value=""{0}"" autocomplete='off'id=""{1}"" placeholder='Search with Customer Name or Number' style='max-width:1000px;'>", "", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
                htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-search-subsi'
                         subsiId=""{0}"" placeholder=""{1}"">", value, placeholderName);
                htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
                htmlBuilder.AppendFormat(@"</button>");
                htmlBuilder.AppendFormat(@"</span>");
                htmlBuilder.AppendFormat(@"</div>");
            }
            if(subsiId==6 || subsiId==7 || subsiId==8)
            {

                htmlBuilder.AppendFormat(@"<div class='input-group section-subsi' id=""{0}"">", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}"" class='internal-value' value=""{1}"" />", htmlFieldNameWithPrefix, null);
                htmlBuilder.AppendFormat(@"<input type='text' name='display-txt' class='form-control display-txt-subsi' value=""{0}"" autocomplete='off'id=""{1}"" placeholder='Search with Product Code or Account Number' style='max-width:1000px;'>", "", htmlFieldIdWithPrefix);
                htmlBuilder.AppendFormat(@"<span class='input-group-btn'>");
                htmlBuilder.AppendFormat(@"<button type = 'button' name='search' class='btn btn-flat btn-search-subsi'
                         subsiId=""{0}"" placeholder=""{1}"">", value, placeholderName);
                htmlBuilder.AppendFormat(@"<i class='fa fa-search'></i>");
                htmlBuilder.AppendFormat(@"</button>");
                htmlBuilder.AppendFormat(@"</span>");
                htmlBuilder.AppendFormat(@"</div>");
            }
            return new MvcHtmlString(htmlBuilder.ToString());

        }
        public static MvcHtmlString EditorForList<TModel, TValue>(this HtmlHelper<TModel> html,
             Expression<Func<TModel, IEnumerable<TValue>>> propertyExpression,
             Expression<Func<TValue, string>> indexResolverExpression = null,
             string parentName = null,
             bool includeIndexField = true) where TModel : class
        {
            var items = propertyExpression.Compile()(html.ViewData.Model);
            var htmlBuilder = new StringBuilder();
            var htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            if (parentName != null)
            {
                htmlFieldName = parentName + '.' + htmlFieldName;
            }
            var htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);

            Func<TValue, string> indexResolver = null;
            if (indexResolverExpression == null)
            {
                indexResolver = x => null;
            }
            else
            {
                indexResolver = indexResolverExpression.Compile();
            }
            foreach (var item in items)
            {
                var dummy = new { Item = item };
                var guid = indexResolver(item);
                var memberExp = Expression.MakeMemberAccess(Expression.Constant(dummy), dummy.GetType().GetProperty("Item"));
                var singleItemExp = Expression.Lambda<Func<TModel, TValue>>(memberExp, propertyExpression.Parameters);

                if (String.IsNullOrEmpty(guid))
                {
                    guid = Guid.NewGuid().ToString();
                }
                else
                {
                    guid = html.AttributeEncode(guid);
                }
                if (includeIndexField)
                {
                    htmlBuilder.Append(_EditorForListIndexField<TValue>(htmlFieldNameWithPrefix, guid, indexResolverExpression));
                }
                htmlBuilder.Append(html.EditorFor(singleItemExp, null, string.Format("{0}[{1}]", htmlFieldName, guid)));


            }
            return new MvcHtmlString(htmlBuilder.ToString());
        }

        private static MvcHtmlString EditorForListIndexField<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, string>> indexResolverExpression = null)
        {
            var htmlPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            var first = htmlPrefix.LastIndexOf('[');
            var last = htmlPrefix.IndexOf(']', first + 1);
            if (first == -1 || last == -1)
            {
                throw new InvalidOperationException("EditorForListIndexField called when not in a EditorForList context");

            }
            var htmlFieldNameWithPrefix = htmlPrefix.Substring(0, first);
            var guid = htmlPrefix.Substring(first + 1, last - first - 1);

            return _EditorForListIndexField<TModel>(htmlFieldNameWithPrefix, guid, indexResolverExpression);

        }

        public static IHtmlString MyTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes,bool isSelected
        )
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (isSelected == false)
            {
                attributes["disabled"] = "disabled";
            }
            return htmlHelper.TextBoxFor(expression, attributes);
        }
        public static MvcHtmlString MyCheckBoxFor<T>(this HtmlHelper<T> html,
                                                   Expression<Func<T, bool>> expression,
                                                   object htmlAttributes, bool isSelected)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            if (isSelected == false)
            {
                attributes["disabled"] = "disabled";
            }
            return html.CheckBoxFor(expression, attributes);
        }

        private static MvcHtmlString _EditorForListIndexField<TModel>(string htmlFieldNameWithPrefix, string guid, Expression<Func<TModel, string>> indexResolverExpression)
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}.Index"" value=""{1}"" />", htmlFieldNameWithPrefix, guid);

            if (indexResolverExpression != null)
            {
                htmlBuilder.AppendFormat(@"<input type=""hidden"" id=""ColIndex"" name=""{0}[{1}].{2}"" value=""{1}"" />", htmlFieldNameWithPrefix, guid, ExpressionHelper.GetExpressionText(indexResolverExpression));
            }

            return new MvcHtmlString(htmlBuilder.ToString());
        }
    }
}
