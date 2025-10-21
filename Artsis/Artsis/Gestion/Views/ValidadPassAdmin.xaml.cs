using CommunityToolkit.Maui.Views;

namespace Artsis.Gestion.Views;

public partial class ValidarPassAdmin : Popup
{
	public ValidarPassAdmin()
	{
		InitializeComponent();
	}

    private void AceptarClicked(object sender, EventArgs e)
    {
        // Supongamos que tienes una propiedad "PasswordEntry" enlazada al Entry
        this.Close(PasswordEntry.Text);
    }

    private void CancelarClicked(object sender, EventArgs e)
    {
        this.Close(null);
    }
}