using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.NCAdmin.Infrastructure
{
    [HtmlTargetElement("input", Attributes = "asp-custom-radio")]
    public class CheckBoxTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            
        }
    }
}
