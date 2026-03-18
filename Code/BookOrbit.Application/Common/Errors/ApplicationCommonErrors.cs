namespace BookOrbit.Application.Common.Errors;
static public class ApplicationCommonErrors
{
    static public Error NotFoundClass(string Class, string PropertyCode, string PropertyDescription) =>
        Error.NotFound($"{Class}.With{PropertyCode}.NotFound", $"{Class} with the specified {PropertyDescription} was not found.");
     static public Error AlreadyExists(string Class, string PropertyCode, string PropertyDescription) =>
        Error.Validation($"{Class}.{PropertyCode}.AlreadyExists", $"A {Class} with the {PropertyDescription} is already exists.");

    static public Error NotFoundProp(string Class, string PropertyCode, string PropertyDescription) =>
        Error.NotFound($"{Class}.{PropertyCode}.NotFound", $"{PropertyDescription} was not found in the system.");
}