namespace Goresc;

using PuppeteerSharp;

public static class PuppeteerExtensions
{
    public static Task<IElementHandle[]> FindAsync(this IPage page, string expression)
        => page.XPathAsync(expression);
}