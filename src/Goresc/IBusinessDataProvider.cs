namespace Goresc;

public interface IBusinessDataProvider
{
    Task<BusinessInformation> GetBusinessInformationAsync();
}