namespace HahaBuch.SharedContracts.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an entity cannot be deleted or archived, because it is referenced by other entities.
/// </summary>
public class ReferencedByEntitiesException(string message) : Exception(message);