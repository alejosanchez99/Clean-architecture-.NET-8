namespace Restaurants.Domain.Exceptions;

public class NotFoundException(string resourceType, string ResourceIdentifier)
    : Exception($"{resourceType} with id: {ResourceIdentifier} doesn't exist");