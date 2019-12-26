using ChannakyaAccounting.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaAccounting.Helper
{
    public static class CustomHelper
    {
        private static ChannakyaAccEntities db = new ChannakyaAccEntities();
        public static MvcHtmlString Paging(this HtmlHelper helper, string action, string controller, int pageCount, int pageNo, int pageSize)
        {
            StringBuilder sb = new System.Text.StringBuilder();

            //sb.Append("<div class='box-body no-padding'>");
            sb.Append("<div class='btn-groups modal-footer' style='padding: 5px'>");
            sb.Append("<div class='form-group col-md-12 paging' style='margin:0px;padding-left:0px;' id ='paging' controller = '" + controller + "' action = '" + action + "' container = '' pageCount='" + pageCount + "'  >");

            string isFirstPage = "";
            string isLastPage = "";
            if (pageNo == 1)
            {
                isFirstPage = "disabled='disabled'";
            }
            if (pageNo == pageCount || pageCount == 0)
            {
                isLastPage = "disabled='disabled'";
            }
            sb.Append("<div class='col-md-12 startDiv' style='display:inline-flex;padding-left:0px;'>");
            sb.Append("<input type = 'button' value = '<<' id = 'btnFirst' class='btn btn-info btn-xs btnPaging' " + isFirstPage + "/>");
            sb.Append("<input type = 'button' value = '<' id = 'btnPrev' class='btn btn-info btn-xs btnPaging'  style='margin-left:2px;' " + isFirstPage + " />&nbsp");
            sb.Append("<div class='col-md-0 hidden-xs'>Page</div>");
            sb.Append("<input type='text' id='pageNo' class='form-control txtPaging'  style='max-width:75px;max-height:24px;text-align:center;padding:0px;' value='" + pageNo + "'/> ");
            sb.Append("<span style='display:inline-block'>&nbspof&nbsp;" + pageCount + "&nbsp</span>");
            sb.Append("<input type = 'button' value = ' > ' id = 'btnNxt' class='btn btn-info btn-xs btnPaging'  " + isLastPage + " />");
            sb.Append("<input type = 'button' value = '>>' id = 'btnLast' class='btn btn-info btn-xs btnPaging' style='margin-left:2px;'  " + isLastPage + " />");
            sb.Append("<div class='col-md-5'>");
            sb.Append("Page Size:<input type='text' id='pageSize' class='form-control txtPaging' style='max-width:30px;height:24px;text-align:center;display:inline-block;padding:0px;' value='" + pageSize + "'> ");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div id='erormsg'  class='alert-danger' style='display:none'>");
            sb.Append("<p>No page!! Moved to first page</p>");
            sb.Append("</div>");
            sb.Append("<div id='erormsgpagesize'  class='alert-danger' style='display:none'>");
            sb.Append("<p>Can not be less than 1!!! setting to default</p>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString AccountNumberSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return AccountNumberSearch(html, expression, EAccountDetailsShow.WithAccount.GetDescription(), EAccountFilter.DepositAccept.GetDescription(), EAccountType.Normal.GetDescription());
        }

        public static MvcHtmlString AccountNumberSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string eaccDetails, string accountFilter, string accountType)
        {
            MvcHtmlString mvcHtml = default(MvcHtmlString);
            StringBuilder htmlBuilder = new StringBuilder();

            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var cntrlName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var cntrlId = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(cntrlName);
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;

            htmlBuilder.AppendFormat(@"<div id='account-pop-up-div' class='modal fade' role='dialog'></div>");
            htmlBuilder.AppendFormat(@"<div class='box-tools'>");
            //htmlBuilder.AppendFormat(@"");
            htmlBuilder.AppendFormat(@"<div class='input-group input-group-sm pull-right account-number-div'>");
            htmlBuilder.AppendFormat(@"<input style='display:inline;' type='text' id='" + cntrlName + "'helperId='IAccno' class='form-control account-aumber' name='accountNumber' placeholder='Account Number' value='' showwith='" + eaccDetails + "' accountFilter='" + accountFilter + "' accountType='" + accountType + "' />");
            htmlBuilder.AppendFormat(@"<input type='hidden' id='" + cntrlName + "' class='account-id' helperId='IAccno' name='" + cntrlName + "' value='" + value + "' />");
            htmlBuilder.AppendFormat(@" <div class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type='button' name='btn-account-open-search' id='btn-account-openSearch' class='btn btn-default btn-account-open-search' style='margin-left: 0px;' ><i class='fa fa-search'></i></button>");
            htmlBuilder.AppendFormat(@"<button type='button' name='btn-account-view-details' id='btn-account-view-details-" + cntrlName + "' class='btn btn-default hidden btn-account-view-details-button' style='margin-left: 0px;' Action='Show'> <i class='fa fa-toggle-on' title='show details'></i><i class='fa fa-toggle-off hidden' title='hide details'></i></button>");
            htmlBuilder.AppendFormat(@"</div>");
            htmlBuilder.AppendFormat(@"</div>");
            htmlBuilder.AppendFormat(@"</div>");

            mvcHtml = MvcHtmlString.Create(htmlBuilder.ToString());

            return mvcHtml;
        }
    }
}