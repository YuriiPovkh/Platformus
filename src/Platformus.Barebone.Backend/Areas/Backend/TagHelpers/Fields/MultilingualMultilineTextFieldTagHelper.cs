﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Platformus.Barebone.Backend
{
  [HtmlTargetElement("multilingual-multiline-text-field", Attributes = ForAttributeName + "," + LocalizationsAttributeName)]
  public class MultilingualMultilineTextFieldTagHelper : TagHelper
  {
    private const string ForAttributeName = "asp-for";
    private const string LocalizationsAttributeName = "asp-localizations";

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    [HtmlAttributeName(ForAttributeName)] 
    public ModelExpression For { get; set; }

    [HtmlAttributeName(LocalizationsAttributeName)]
    public IEnumerable<Localization> Localizations { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (this.For == null)
        return;

      output.SuppressOutput();
      output.Content.Clear();
      output.Content.AppendHtml(this.GenerateField());
    }

    private TagBuilder GenerateField()
    {
      TagBuilder tb = new TagBuilder("div");

      tb.AddCssClass("form__field field field--multilingual");

      FieldGenerator fieldGenerator = new FieldGenerator();

      tb.InnerHtml.Clear();
      tb.InnerHtml.AppendHtml(fieldGenerator.GenerateLabel(this.For));

      foreach (Localization localization in this.Localizations)
      {
        if (localization.Culture.Code != "__")
        {
          tb.InnerHtml.AppendHtml(fieldGenerator.GenerateCulture(localization));
          tb.InnerHtml.AppendHtml(new TextAreaGenerator().GenerateTextArea(this.ViewContext, this.For, localization, "field__text-area field__text-area--multilingual"));

          if (localization != this.Localizations.Last())
            tb.InnerHtml.AppendHtml(fieldGenerator.GenerateMultilingualSeparator());
        }
      }

      return tb;
    }
  }
}