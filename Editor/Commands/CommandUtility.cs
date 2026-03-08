using CliWrap;
using CliWrap.Buffered;
using PlasticGui;
using UnityEngine;


namespace YAMLMergeInEditor
{
    public class CommandUtility
    {


        public static CliWrap.Command Git(string repositoryPath, params string[] arguments)
        {
            return Cli.Wrap("git").WithWorkingDirectory(repositoryPath).WithArguments(arguments);
        }

        public static async Awaitable Run(CliWrap.Command command)
        {
            try
            {
                Debug.Log(Debug.LogLevel.Trace, $"Running: {command}");
                await command.ExecuteAsync();
            }
            catch (System.Exception e)
            {
                Debug.Log(Debug.LogLevel.Error, $"Error Running: {command} \r\n {e}");
                throw e;
            }
        }

        public static async Awaitable<bool> RunAndCheck(CliWrap.Command command, System.Func<string,bool> check)
        {

            try
            {
                Debug.Log(Debug.LogLevel.Trace, $"Running: {command}");
                var output = await command.ExecuteBufferedAsync();
                Debug.Log(Debug.LogLevel.Debug, $"Output: {output.StandardOutput}");

                return check(output.StandardOutput);
            }
            catch (System.Exception e)
            {
                Debug.Log(Debug.LogLevel.Error, $"Error Running: {command} \r\n {e}");
                return false;
            }



        }

    }
}
