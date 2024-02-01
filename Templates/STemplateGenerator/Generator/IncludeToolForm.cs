#region using
using Generator.Business.MediatR.Grid;
using Generator.Business.MediatR.Grid.RequestModel;
using Generator.Business.ServiceCollection;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using Generator.IncludeToolFormModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Message = Generator.Const.Message;
#endregion
namespace Generator
{
    internal partial class IncludeToolForm : Form
    {
        #region Service
        private readonly EnvDTE.ProjectItem projectItem;
        private readonly IMediatRCreateGridManager _mediatRCreateGridManager;
        private static string _fileFullPath;
        private readonly string _className;
        private readonly string _aggregateModelFolderPathString;
        private readonly bool _differentFile;
        #endregion
        #region Form Veriable
        private string[] GetFolderClassesName => FolderHelper.GetFolderClassesName(_aggregateModelFolderPathString);
        private static IEnumerable<SyntaxSemanticGridPropertyInfo> _mainClassNotSystemProperties = [];
        private static IEnumerable<SyntaxSemanticGridPropertyInfo> _mainClassSystemProperties = [];
        private readonly List<string> _includeCheckboxList = [];
        private readonly List<string> _propertiesCheckedList = [];
        private List<SyntaxSemanticGridPropertyInfo> _notSystemProperties = [];
        private readonly Dictionary<string, IEnumerable<SyntaxSemanticGridPropertyInfo>> _semanticAllClassProperties = [];
        #endregion
        #region Ctor
        public IncludeToolForm(EnvDTE.ProjectItem projectItem, bool differentFile)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            InitializeComponent();
            _differentFile = differentFile;
            this.projectItem = projectItem;
            _fileFullPath = projectItem.FileNames[0];
            _className = Path.GetFileNameWithoutExtension(_fileFullPath);
            _aggregateModelFolderPathString = Path.GetDirectoryName(_fileFullPath);
            _mediatRCreateGridManager = CustomServiceCollection.MediatRCreateGridManager();
        }
        #endregion
        #region Events
        #region Include Check Box
        private void IncludeListBox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = IncludeListBox.IndexFromPoint(e.Location);
            if (index != -1 && !IncludeListBox.GetItemChecked(index))
            {
                IncludeListBox.SetItemChecked(index, true);
            }
        }
        private void IncludeListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                _includeCheckboxList.Add(IncludeListBox.SelectedItem.ToString());
            }
            else
            {
                e.NewValue = CheckState.Checked;
            }
        }
        private void ResetIncludePropertiesBtn_Click(object sender, EventArgs e)
        {
            //Selected Properties
            _propertiesCheckedList.Clear();
            //I keep system properties so that it doesn't read the main file again when I reset
            _notSystemProperties = _mainClassNotSystemProperties.ToList();
            PropertycheckedListBox.Items.Clear();
            _mainClassSystemProperties.ForEach(x => PropertycheckedListBox.Items.Add(x.PropertyNameInsideClass));
            //When I reset, I reset the ones that do not have system properties so that they do not read files again.
            IncludeListBox.Items.Clear();
            _includeCheckboxList.Clear();
            _mainClassNotSystemProperties.ForEach(x => IncludeListBox.Items.Add(x.SelectPropertyName));
            //For which keys and types I keep
            _semanticAllClassProperties.Clear();
            _semanticAllClassProperties.Add(_className, _mainClassSystemProperties);
        }
        #endregion-
        #region Properties Checkbox
        private void AddSemanticProperties(IEnumerable<SyntaxSemanticGridPropertyInfo> semantiClassPropertiesModels, string propertyName)
        {
            _semanticAllClassProperties.Add(propertyName, semantiClassPropertiesModels);
            semantiClassPropertiesModels.ForEach(x => PropertycheckedListBox.Items.Add(x.PropertyNameInsideClass));
        }
        private void PropertycheckedListBox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = PropertycheckedListBox.IndexFromPoint(e.Location);
            if (index != -1 && !PropertycheckedListBox.GetItemChecked(index))
            {
                PropertycheckedListBox.SetItemChecked(index, true);
            }
        }
        private void PropertycheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string selectedProperty = PropertycheckedListBox.Items[e.Index].ToString();
            if (e.NewValue == CheckState.Checked)
            {
                _propertiesCheckedList.Add(selectedProperty);
            }
            else
            {
                e.NewValue = CheckState.Checked;
            }
        }
        private void SelectAllBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < PropertycheckedListBox.Items.Count; i++)
            {
                PropertycheckedListBox.SetItemChecked(i, true);
            }
        }
        #endregion
        #region Form
        private void IncludeToolForm_Load(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!projectItem.FileNames[0].Contains(".Domain\\AggregateModels\\"))
            {
                VS.MessageBox.ShowError(Message.PleaseCheckFile);
                return;
            }
            if (SemanticHelper.IsGetSemanticModelWithFilePath(_fileFullPath, "MainModelCompilation",
                _className, out var semantiClassPropertiesModels))
            {
                var properties = GetSyntaxSemanticGridPropertyInfo(semantiClassPropertiesModels.ClassProperties,
                    semantiClassPropertiesModels.SemanticModel, GetFolderClassesName);
                //for Reset Btn
                _mainClassNotSystemProperties = properties.Where(x => !x.SystemProperties);
                _mainClassSystemProperties = properties.Where(x => x.SystemProperties);
                //Add İnclude Check Box
                _notSystemProperties = _mainClassNotSystemProperties.ToList();
                _notSystemProperties.ForEach(x => IncludeListBox.Items.Add(x.SelectPropertyName));
                //Not System Properties
                _mainClassSystemProperties.ForEach(x => PropertycheckedListBox.Items.Add(x.PropertyNameInsideClass));
                //Not add the class again
                _semanticAllClassProperties.Add(_className, _mainClassSystemProperties);
            }
        }
        #endregion
        #region Get Properties
        private void GetPropertiesBtn_Click(object sender, EventArgs e)
        {
            foreach (var item in _includeCheckboxList)
            {
                var getPropertiesInfo = _notSystemProperties.Find(x => x.SelectPropertyName == item);
                var className = getPropertiesInfo.Type.Replace("?", "");
                if (_semanticAllClassProperties.Keys.Any(x => x == item))
                {
                    continue;
                }
                if (getPropertiesInfo.IsGeneric)
                {
                    className = getPropertiesInfo.GenericNameArgument;
                }
                var path = Path.Combine(_aggregateModelFolderPathString, $"{className}.cs");
                if (SemanticHelper.IsGetSemanticModelWithFilePath(path, $"{className}Compilation", className,
                    out var semantiClassPropertiesModels))
                {
                    var properties = GetSyntaxSemanticGridPropertyInfo(semantiClassPropertiesModels.ClassProperties,
                       semantiClassPropertiesModels.SemanticModel, GetFolderClassesName, getPropertiesInfo.SelectPropertyName, getPropertiesInfo.IsGeneric);
                    var notSystemProperties = properties.Where(x => !x.SystemProperties);
                    _notSystemProperties.AddRange(notSystemProperties);
                    if (!getPropertiesInfo.IsGeneric)
                    {
                        notSystemProperties.ForEach(x => IncludeListBox.Items.Add(x.SelectPropertyName));
                    }
                    AddSemanticProperties(properties.Where(x => x.SystemProperties), getPropertiesInfo.SelectPropertyName);
                }
            }
        }
        #endregion
        #region Generate Code Button
        private void GenerateCodeBtn_Click(object sender, EventArgs e) => _ = GenerateCodeBtn_ClickAsync();
        private async Task GenerateCodeBtn_ClickAsync()
        {
            if (await _mediatRCreateGridManager.GenerateCodeAsync(
            new GridGenerateVeriables(_includeCheckboxList,
                _propertiesCheckedList,
                DtoName.Text,
                _semanticAllClassProperties,
                _fileFullPath,
                _className,
                PathConst.GetProjectName(projectItem),
                _differentFile
                )))
            {
                this.Close();
            }
        }
        #endregion
        #endregion
        #region Helper
        internal static IEnumerable<SyntaxSemanticGridPropertyInfo> GetSyntaxSemanticGridPropertyInfo(IEnumerable<PropertyDeclarationSyntax> classProperties,
            SemanticModel semanticModel, string[] folderFileList, string className = null, bool classIsGeneric = false)
        {
            return classProperties.Select(property =>
            {
                var typeSymbol = semanticModel.GetTypeInfo(property.Type).Type;
                var isGeneric = false;
                var genericNameArgument = "";
                var systemProperties = true;
                var typeSymbolTrue = typeSymbol.OriginalDefinition.Name;
                if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                {
                    typeSymbolTrue = typeSymbol.OriginalDefinition.Name;
                    if (namedTypeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
                    {
                        namedTypeSymbol = namedTypeSymbol.TypeArguments[0] as INamedTypeSymbol;
                        typeSymbolTrue = namedTypeSymbol.Name;
                    }
                    if (namedTypeSymbol.IsGenericType)
                    {
                        typeSymbolTrue = namedTypeSymbol.ConstructedFrom?.OriginalDefinition?.Name;
                        isGeneric = true;
                        genericNameArgument = namedTypeSymbol.TypeArguments[0].Name;
                        systemProperties = false;
                    }
                    if (folderFileList.Any(x => x == namedTypeSymbol.Name))
                    {
                        systemProperties = false;
                    }
                }
                var namespaceString = typeSymbol.ContainingNamespace?.ToString() ?? typeSymbol.BaseType.Name.GetType().Namespace ?? "";
                return new SyntaxSemanticGridPropertyInfo(property.Identifier.Text,
                        property.Type.ToString(), namespaceString, isGeneric, genericNameArgument, systemProperties, className, classIsGeneric);
            });
        }
        #endregion
    }
}
