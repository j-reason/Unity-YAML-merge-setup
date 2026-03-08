using CliWrap;
using CliWrap.Buffered;
using UnityEngine;
using YAMLMergeInEditor.Commands;

namespace YAMLMergeInEditor
{
	public static class Setup
	{

       public enum Response
		{
            Success = 0,

            NoGitInstallation = 1,
			NotGitRepo = 2,
			NoYAMLSetup = 3,
			
		}


		public static void RunSetup()
		{
			RunSetup(Settings.DefaultRepoPath);
		}

		public static void RunSetup(string path)
		{
			_= RunSetupAsync(path); //fire and forget
		}



		private static async Awaitable RunSetupAsync(string repoPath)
		{

			var output = await CheckSetup(repoPath);

			if (output >= Response.NoYAMLSetup)
				await YAMLCommands.InstallYAML(repoPath, Settings.DefaultRepoPath);


		}

        
		private static async Awaitable<Response> CheckSetup(string path)
		{

			if (!await GitCommands.isGitInstalled(path))
				return Response.NoGitInstallation;

			if (!await GitCommands.isProjectGitRepo(path))
				return Response.NotGitRepo;

			if (!await YAMLCommands.isYAMLInstalled(path))
				return Response.NoYAMLSetup;

			return Response.Success;
		}



	}

}