namespace Goresc;

using Navigators;

public class NavigatorsOptions
{
    public ICookiesConsentNavigator CookiesConsentNavigator { get; private set; } = new DefaultCookiesConsentNavigator();

    /// <summary>
    /// Defines a new cookies consent navigator to be used instead of the default one. 
    /// </summary>
    public NavigatorsOptions UseCookiesConsentNavigator<T>(T navigator) where T : ICookiesConsentNavigator
    {
        this.CookiesConsentNavigator = navigator;

        return this;
    }
}