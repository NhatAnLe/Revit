#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using LUV_RevitAPI_Extensions;
#endregion

namespace shareParameter
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            CategorySet categorySet = new CategorySet();
            categorySet.Insert(Category.GetCategory(doc, BuiltInCategory.OST_StructuralFraming));
            categorySet.Insert(Category.GetCategory(doc, BuiltInCategory.OST_Floors));

            CustomParameter customParameter = new CustomParameter("commentAA", " comments", BindingType.Instance, ParameterType.Text,
                BuiltInParameterGroup.PG_IDENTITY_DATA, categorySet, true, true);

            using(Transaction t = new Transaction(doc,"set share parameter"))
            {
                t.Start();

                DefinitionFile definitionFile = app.OpenSharedParameterFile();




                customParameter.CreateParameter(definitionFile, uiapp, "commentCC");
                t.Commit();
            }
            



            return Result.Succeeded;
        }
    }
}
