using CleanArchitecture.Application.Common.Interfaces;

namespace Application.IntegrationTests.Common.DataBaseFakers;

public static class FakerManager
{
    public static ClientFaker ClientFaker { get; } = new();
    public static AddressFaker AddressFaker { get; } = new();
}