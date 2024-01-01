namespace Goresc;

public interface IBusinessInformationProvider
{
    Task<BusinessInformation> GetBusinessInformationAsync();
}