namespace DarkUI.Config
{
    public class ThemeProvider
    {
        private static ITheme theme;
        public static ITheme Theme
        {
            get
            {
                if (theme == null)
                    theme = new DarkTheme();

                return theme;
            }
            set
            {
                theme = value;
            }
        }
    }
}
