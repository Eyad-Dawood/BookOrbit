namespace BookOrbit.Domain.Common;

    static public class CommonErrors
    {
        static public Error RequiredProp(string Class,string PropertyCode,string PropertyDescription) =>
            Error.Validation($"{Class}.{PropertyCode}.Required",$"{Class} {PropertyDescription} is required.");

        static public Error InvalidProp(string Class, string PropertyCode, string PropertyDescription , string Details = "") =>
            Error.Validation($"{Class}.{PropertyCode}.Invalid", $"{Class} {PropertyDescription} is invalid. {Details}");

        static public Error Custom(string Class, string Code, string Description) =>
            Error.Validation($"{Class}.{Code}", Description);
}

