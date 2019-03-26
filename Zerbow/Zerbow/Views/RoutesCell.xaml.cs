using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Zerbow.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoutesCell : ViewCell
	{
		public RoutesCell ()
		{
            InitializeComponent();
            fromLabel.SetBinding(Label.TextProperty, new Binding("From"));
            toLabel.SetBinding(Label.TextProperty, new Binding("To"));
            imageRoute.SetBinding(Image.SourceProperty, new Binding("Photo"));
        }
	}
}