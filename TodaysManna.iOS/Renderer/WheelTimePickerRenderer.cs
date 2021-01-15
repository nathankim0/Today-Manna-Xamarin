using TodaysManna.iOS.Renderers;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TimePicker), typeof(WheelTimePickerRenderer))]
namespace TodaysManna.iOS.Renderers
{
	public class WheelTimePickerRenderer : Xamarin.Forms.Platform.iOS.TimePickerRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<TimePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				UITextField entry = Control;
				UIDatePicker picker = (UIDatePicker)entry.InputView;
				picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
			}
		}
	}
}