namespace ModelSaber
{
    public static class Utilities
    {
        public static string SafeGameName(string gameTitle)
        {
            return gameTitle
                .Replace(" ", "-")
                .ToLower();
        }

        public static string FormattedGameName(string gameTitle)
        {
            return gameTitle.Replace("-", " ");
        }
    }
}