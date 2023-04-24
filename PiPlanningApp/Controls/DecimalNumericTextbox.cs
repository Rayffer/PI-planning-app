using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PiPlanningApp.Controls;

public partial class DecimalNumericTextbox : TextBox
{
    private static readonly Regex _regex = DecimalRegex();
    public int IntegerValue => int.TryParse(this.Text, out var result) ? result : 0;

    public DecimalNumericTextbox()
    {
        PreviewTextInput += this.NumericTextbox_PreviewTextInput;
    }

    ~DecimalNumericTextbox()
    {
        PreviewTextInput -= this.NumericTextbox_PreviewTextInput;
    }

    private void NumericTextbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (sender is DecimalNumericTextbox decimalNumericTextbox &&
            !_regex.IsMatch(decimalNumericTextbox.Text.Insert(decimalNumericTextbox.CaretIndex, e.Text)))
        {
            e.Handled = true;
        }
    }

    [GeneratedRegex("^[\\+\\-]{0,1}[0-9]{0,}[,.]{0,1}[0-9]{0,}$")]
    private static partial Regex DecimalRegex();
}