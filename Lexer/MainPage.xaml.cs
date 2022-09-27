using Lexer.Core.UseCase;

namespace Lexer;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnStartClicked(object sender, EventArgs e)
	{
		var sourceCode = SourceCodeEdt.Text;

		try
		{
            var analyzer = new LexerAnalyzer();
            var lexemes = analyzer.TryToAnalyze(sourceCode);

            SourceCodeEdt.Text = "";

            foreach (var lexeme in lexemes)
                SourceCodeEdt.Text += lexeme + "\r\n";

            StartAnalysisBtn.IsVisible = false;
        } catch (Exception ex)
		{
			DisplayAlert("Sorry!", $"An error occur while we`re trying to analyse your source code. \n{ex.Message}", "OK");
		}
	}

	private void OnCleanClicked(object sender, EventArgs e)
	{
        SourceCodeEdt.Text = "";
        StartAnalysisBtn.IsVisible = true;
    }
}

