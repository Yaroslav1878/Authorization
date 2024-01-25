namespace Authorization.Application.Helpers;

public static class RegexHelper
{
    public const string Numbers = "0-9";
    public const string Paragraphs = "\\r\\n";
    public const string EnglishLetters = "a-zA-Z";
    public const string SpecialPasswordCharacters = ".,?!:;'\"@#$%^&*(){}<>_+=`~\\/";
    public const string EmailCharacters = "-.@";

    public const string AllExceptParagraphs = "^[^" + Paragraphs + "]+$";
    public const string PasswordCharacters = "^[" + Numbers + EnglishLetters + SpecialPasswordCharacters + "]*$";
    public const string EnglishLettersNumbersAndEmailCharacters = "^[" + Numbers + EnglishLetters + EmailCharacters + "]*$";
}
