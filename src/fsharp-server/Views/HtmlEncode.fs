namespace EAArchive.Views

module HtmlEncode =
    let htmlEncode (text: string) : string =
        text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;")
