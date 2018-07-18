// <copyright file="MiddleTierCompiler.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.ClientGenerator
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Text;
	using DarkBond;
	using DarkBond.DataModelGenerator;
    using DarkBond.ServerGenerator;

	/// <summary>
	/// The command line version of the Custom Tool for generating code from a schema.
	/// </summary>
	class ClientCompiler
	{
		/// <summary>
		/// Dictionary of command line parameter switches and the states they invoke in the parser.
		/// </summary>
		private static Dictionary<String, ArgumentState> argumentStates = new Dictionary<String, ArgumentState>()
        {
            {"-i", ArgumentState.InputFileName},
            {"-lib", ArgumentState.Library},
            {"-ns", ArgumentState.TargetNamespace},
            {"-out", ArgumentState.OutputFileName}
        };

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(String[] args)
		{
			// These are the parameters that are parsed out of the command line.
			String inputFilePath = String.Empty;
			String outputFileName = String.Empty;
            List<String> libraries = new List<String>();
            String targetNamespace = "DefaultNamespace";

			try
			{
				// The command line parser is driven by different states that are triggered by the flags read.  Unless a flag has been parsed, the command line
				// parser assumes that it's reading the file name from the command line.
				ArgumentState argumentState = ArgumentState.InputFileName;

				// Parse the command line for arguments.
				foreach (String argument in args)
				{
					// Use the dictionary to transition from one state to the next based on the input parameters.
					ArgumentState nextArgumentState;
					if (ClientCompiler.argumentStates.TryGetValue(argument, out nextArgumentState))
					{
						argumentState = nextArgumentState;
						continue;
					}

					// The parsing state will determine which variable is read next.
					switch (argumentState)
					{
					case ArgumentState.InputFileName:

						// Expand the environment variables so that paths don't need to be absolute.
						inputFilePath = Environment.ExpandEnvironmentVariables(argument);

						// The output name defaults to the input file name with a new extension.
						outputFileName = String.Format("{0}.cs", Path.GetFileNameWithoutExtension(inputFilePath));

						break;

                    case ArgumentState.Library:

                        // Add this argument to the list of libraries that will be searched for types.
                        libraries.Add(argument);

                        break;

                    case ArgumentState.OutputFileName:

						// Expand the environment variables so that paths don't need to be absolute.
						outputFileName = Environment.ExpandEnvironmentVariables(argument);
						break;

					case ArgumentState.TargetNamespace:

						// This is the namespace that is used to create the target data model.
						targetNamespace = argument;
						break;

					}

					// The default state is to look for the input file name on the command line.
					argumentState = ArgumentState.InputFileName;

				}

				// This will read the input XML schema into a large buffer.
				String fileContents;
                using (StreamReader streamReader = new StreamReader(inputFilePath))
                {
                    fileContents = streamReader.ReadToEnd();
                }

                // Provide a Site from which the custom tool can get additional information about the environment.
                ServerCompilerSite serverCompilerService = new ServerCompilerSite(libraries);

                // This next step simulates the calling convention used by a custom tool, which is to say it uses unmanaged code.  The progress indicator is used 
				// to provide feedback, to either the IDE or the command line, about how far along the compilation is.  Note that the calling interface is from
				// unmanaged-code to managed code, the way you would expect the IDE to call the generator, so we create an unmanaged buffer pointer for the results.
				IntPtr[] buffer = new IntPtr[1];
				UInt32 bufferSize;
				DataModelGenerator dataModelGenerator = new DataModelGenerator();
				dataModelGenerator.Generate(inputFilePath, fileContents, targetNamespace, buffer, out bufferSize, new ProgressIndicator());

				// Once the buffer of source code is generated, it is copied back out of the unmanaged buffers and written to the output file.
				Byte[] outputBuffer = new byte[bufferSize];
				Marshal.Copy(buffer[0], outputBuffer, 0, Convert.ToInt32(bufferSize));
				using (StreamWriter streamWriter = new StreamWriter(outputFileName))
					streamWriter.Write(Encoding.UTF8.GetString(outputBuffer));

			}
			catch (Exception exception)
			{
				// This will catch any generic errors and dump them to the console.
				Console.WriteLine(exception.Message);

			}

			// The execution at this point was a success.
			return 0;

		}
	}
}
