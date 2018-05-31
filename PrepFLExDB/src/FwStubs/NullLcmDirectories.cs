using SIL.LCModel;

namespace PrepFLExDB
{
	internal class NullLcmDirectories : ILcmDirectories
	{
		public string ProjectsDirectory => null;

		public string TemplateDirectory => null;
	}
}
