/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.UI.Bootstrap
{
    /// <summary>
    /// A display only Input box for formatted text
    /// </summary>
    public class InputReadOnlyText : ComponentBase
    {
        [Parameter] public string Value { get; set; } = String.Empty;

        [Parameter] public bool AsMarkup { get; set; } = true;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(2, "class", "form-control");
            builder.AddAttribute(2, "readonly", "readonly");
            if (AsMarkup) builder.AddAttribute(4, "value", (MarkupString)this.Value);
            else builder.AddAttribute(4, "value", this.Value);
            builder.CloseElement();
        }

    }
}
