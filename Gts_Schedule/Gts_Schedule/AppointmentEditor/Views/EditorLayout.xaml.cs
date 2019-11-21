using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gts_Schedule.AppointmentEditor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditorLayout : StackLayout
    {
		public EditorLayout ()
		{
			InitializeComponent ();
		}
	}
}