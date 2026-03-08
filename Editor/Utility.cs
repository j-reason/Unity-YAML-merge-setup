using CliWrap;
using CliWrap.Buffered;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace YAMLMergeInEditor
{
	public static class Utility
	{


        public static CliWrap.Command Git(string repositoryPath, params string[] arguments)
        {
            return Cli.Wrap("git").WithWorkingDirectory(repositoryPath).WithArguments(arguments);
        }


        

        
    } 
}
