namespace MatchMasterWEB.ControllerServices.GetRatingMethods
{
	public class TextColourMethod
	{
		public string GetTextColour(double rating)
		{
			return rating switch
			{
				< 5 => "#FF0000",
				>= 5 and < 7 => "#FFA500",
				>= 7 and < 8 => "#6BBE00",
				> 8 => "#008000",
				_ => "#FFA500",
			};
		}
	}
}
