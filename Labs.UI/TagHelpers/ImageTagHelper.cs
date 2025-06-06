using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Labs.UI.TagHelpers
{
    [HtmlTargetElement("img", Attributes = "img-action, img-controller")]
    public class ImageTagHelper(LinkGenerator linkGenerator) : TagHelper
    {
        // Контроллер, формирующий путь к изображению
        [HtmlAttributeName("img-controller")]
        public string? ImgController { get; set; }

        // Действие, формирующее путь к изображению
        [HtmlAttributeName("img-action")]
        public string? ImgAction { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrWhiteSpace(ImgAction) && !string.IsNullOrWhiteSpace(ImgController))
            {
                var url = linkGenerator.GetPathByAction(ImgAction!, ImgController!);
                output.Attributes.SetAttribute("src", url);
            }
        }
    }
}
