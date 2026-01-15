using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace Task_03
{
    [Transaction(TransactionMode.Manual)]
    public class OpeningsCommand : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            try
            {
                string baseFolder =
                    Path.GetDirectoryName(
                        Assembly.GetExecutingAssembly().Location);

                string openingsPath =
                    Path.Combine(baseFolder, @"ExternalPlugins\Openings\Openings.dll");

                Assembly asm = Assembly.LoadFrom(openingsPath);

                Type cmdType = asm.GetTypes()
                    .First(t =>
                        typeof(IExternalCommand).IsAssignableFrom(t) &&
                        !t.IsAbstract);

                var command = (IExternalCommand)Activator.CreateInstance(cmdType);
                return command.Execute(commandData, ref message, elements);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Task 03", ex.Message);
                return Result.Failed;
            }
        }
    }
}
