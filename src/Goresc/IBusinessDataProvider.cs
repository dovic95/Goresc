// <copyright file="IBusinessDataProvider.cs" company="Suez">
// Copyright (c) Suez. All rights reserved.
// </copyright>

namespace Goresc;

public interface IBusinessDataProvider
{
    Task<BusinessInformation> GetBusinessInformationAsync();
}