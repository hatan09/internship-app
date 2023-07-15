namespace InternshipApp.Portal.Views;

public static class OptionEnumToStringConverter
{
    public static string Beautify(this string value)
    {
        return value.Replace("___", ". ").Replace("__", ", ").Replace("_", " ");
    }
}
