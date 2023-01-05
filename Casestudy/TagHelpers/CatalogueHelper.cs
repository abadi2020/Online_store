using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;
using System.Text;
using Casestudy.Models;
using Newtonsoft.Json;
using Casestudy.Utils;

namespace Casestudy.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("catalogue", Attributes = BrandIdAttribute)]
    public class CatalogueHelper : TagHelper
    {
        private const string BrandIdAttribute = "category";
        [HtmlAttributeName(BrandIdAttribute)]
        public string BrandId { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public CatalogueHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_session.Get<ProductViewModel[]>("productList") != null && Convert.ToInt32(BrandId) > 0)
            {
                var innerHtml = new StringBuilder();
                ProductViewModel[] productList = _session.Get<ProductViewModel[]>("productList");
                innerHtml.Append("<div class=\"col-xs-12\" style=\"font-size:x-large;\"><span>Catalogue</span></div>");
                foreach (ProductViewModel vmProduct in productList)
                {
                    if (vmProduct.BrandId == Convert.ToInt32(BrandId))
                    {
                        // replace single quotes with HTML entity: &#39;
                        vmProduct.Description = vmProduct.Description.Replace("'", "&#39;");

                        vmProduct.JsonData = JsonConvert.SerializeObject(vmProduct);
                        innerHtml.Append("<div class=\"col-sm-3 col-xs-12 text-center\" style=\"border:solid darkblue;\">");
                    
                        innerHtml.Append("<span class=\"col-xs-12\"><img src=\"/images/"+vmProduct.GraphicName+"\"/> </span>");

                        innerHtml.Append("<p><span style=\"font-size:large;\">" + vmProduct.ProductName + "</span></p><div>");
                        innerHtml.Append("<span>For tech. specification<br />Click Details</span></div>");
                        innerHtml.Append("<div style=\"padding-bottom: 10px;\"><a href=\"#details_popup\" data-toggle=\"modal\" class=\"btn btn-default\"");
                        innerHtml.Append(" id=\"modalbtn" + vmProduct.Id + "\" data-id=\"" + vmProduct.Id + "\" data-details='" + vmProduct.JsonData + "'>Details</a></div></div>");
                    }
                }
                output.Content.SetHtmlContent(innerHtml.ToString());
            }
        }
    }

}
