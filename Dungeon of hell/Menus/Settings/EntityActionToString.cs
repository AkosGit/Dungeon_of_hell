using HUD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Utils;

namespace Dungeon_of_hell
{
    public class EntityActionToString : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			EntityActions action = (EntityActions)value;
			switch (action)
			{
				case EntityActions.Forward:
					return "Forward";
				case EntityActions.Left:
					return "Left";
				case EntityActions.Backwards:
					return "Backwards";
				case EntityActions.Right:
					return "Right";
				case EntityActions.Shoot:
					return "Shoot";
				case EntityActions.Reload:
					return "Reload";
				default:
					return "Use";
			}
		}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
			string action = (string)value;
			switch (action)
			{
				case "Forward":
					return EntityActions.Forward;
				case "Left":
					return EntityActions.Left;
				case "Backwards":
					return EntityActions.Backwards;
				case "Right":
					return EntityActions.Right;
				case "Shoot":
					return EntityActions.Shoot;
				case "Reload":
					return EntityActions.Reload;
				default:
					return EntityActions.Use;
			}

		}
    }
}
