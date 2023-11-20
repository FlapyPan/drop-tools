using DropOrganize.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DropOrganize.ViewModel
{
    public class MainViewModel
    {
        //所有配置文件
        private IList<Model.RuleItem> _rules;
        public IList<Model.RuleItem> Rules
        {
            get => _rules ?? (_rules = new List<Model.RuleItem>());
            set => _rules = value;
        }

        //加载配置文件
        private ICommand _ruleLoader;
        public ICommand RuleLoaderCommand
        {
            get => _ruleLoader ?? (_ruleLoader = new CommandHelper(RulesLoader));
            set => _ruleLoader = value;
        }
        private void RulesLoader()
        {
            _rules.Clear();

        }

        //滚轮切换配置文件
        private ICommand _toggleRuleCommand;
        public ICommand ToggleRuleCommand
        {
            get => _toggleRuleCommand ?? (_toggleRuleCommand = new CommandHelper<MouseWheelEventArgs>(ToggleRule));
            set => _toggleRuleCommand = value;
        }
        private void ToggleRule(MouseWheelEventArgs parameter)
        {
            if (parameter.Delta > 0)
            {
                byte currMode = Properties.Settings.Default.ExecuteMode;
                if (currMode == 0) Properties.Settings.Default.ExecuteMode = ModeTotalCount - 1;
                else --Properties.Settings.Default.ExecuteMode;
            }
            else if (parameter.Delta < 0)
            {
                byte currMode = Properties.Settings.Default.ExecuteMode;
                if (currMode + 1 == ModeTotalCount) Properties.Settings.Default.ExecuteMode = 0;
                else ++Properties.Settings.Default.ExecuteMode;
            }
        }

        //恢复悬浮窗默认位置
        private ICommand _defaultLocation;
        public ICommand DefaultLocationCommand
        {
            get => _defaultLocation ?? (_defaultLocation = new CommandHelper(DefaultLocation));
            set => _defaultLocation = value;
        }
        public void DefaultLocation()
        {
            Properties.Settings.Default.LocationTop = 500;
            Properties.Settings.Default.LocationLeft = 500;
            SaveSettingsProp();
        }

        //保存配置文件
        private ICommand _saveSettingsProp;
        public ICommand SaveSettingsPropCommand
        {
            get => _saveSettingsProp ?? (_saveSettingsProp = new CommandHelper(SaveSettingsProp));
            set => _saveSettingsProp = value;
        }
        private void SaveSettingsProp()
        {
            Properties.Settings.Default.Save();
        }

        //打开设置窗口
        private ICommand _openSettingsWin;
        public ICommand OpenSettingsWinCommand
        {
            get => _openSettingsWin ?? (_openSettingsWin = new CommandHelper(OpenSettingsWin));
            set => _openSettingsWin = value;
        }
        private void OpenSettingsWin()
        {
            new View.SettingWindow().Show();
        }

        //退出程序
        private ICommand _exit;
        public ICommand ExitCommand
        {
            get => _exit ?? (_exit = new CommandHelper(ExitProgram));
            set => _exit = value;
        }
        private void ExitProgram()
        {
            Application.Current.Shutdown();
        }

        //拖动文件
        private ICommand _drop;
        public ICommand DropCommand
        {
            get => _drop ?? (_drop = new CommandHelper<DragEventArgs>(Drop));
            set => _drop = value;
        }
        private void Drop(DragEventArgs args)
        {
            byte mode = Properties.Settings.Default.ExecuteMode;
            string[] filePaths = (string[])args.Data.GetData(DataFormats.FileDrop);
            switch (mode)
            {
                case Organize:
                OrganizeFunc(filePaths);
                break;
                case Rename:
                RenameFunc(filePaths);
                break;
                case Zip:
                ZipFunc(filePaths);
                break;
                case Unzip:
                UnzipFunc(filePaths);
                break;
            }
        }

        public const byte ModeTotalCount = 4;

        public const byte Organize = 0;
        private void OrganizeFunc(string[] filePaths)
        {
            bool includeDir = Properties.Settings.Default.IncludeDir;
            bool recur = Properties.Settings.Default.RecursionExec;
            bool skipNotConf = Properties.Settings.Default.SkipNotConf;
            bool skipOccupied = Properties.Settings.Default.SkipOccupied;
            bool showSuccess = Properties.Settings.Default.ShowSuccess;
            bool showError = Properties.Settings.Default.ShowError;
            IDictionary<string, string> ruleDict = GetOrganizeRuleDict();
            IDictionary<string, string> organizeList = new Dictionary<string, string>();
            ObservableCollection<Model.OperationLog> opLogs = new ObservableCollection<Model.OperationLog>();
            foreach (var path in filePaths)
            {
                GetOrganizeList(path, ruleDict, skipNotConf, skipOccupied, includeDir, recur, organizeList);
            }
            foreach (var org in organizeList)
            {
                try
                {
                    Directory.CreateDirectory(org.Value);
                    if (File.Exists(org.Key))
                    {
                        // 判断文件是否被占用
                        if (Utils.DllImports.IsFileOccupied(org.Key))
                        {
                            if (skipOccupied) //被占用且开启"跳过移动被占用的文件"设置
                                File.Copy(org.Key, Path.Combine(org.Value, Path.GetFileName(org.Key)), true);
                            else
                                throw new FileLoadException("The file is occupied and cannot be moved:" + org.Key);
                        }
                        else
                        {
                            File.Move(org.Key, Path.Combine(org.Value, Path.GetFileName(org.Key)));
                        }
                        if (showSuccess) opLogs.Add(new Model.OperationLog()
                        {
                            Code = Model.OperationLog.CodeEnum.Success,
                            Msg = "操作成功",
                            FileName = Path.GetFileName(org.Key),
                            FromPath = org.Key,
                            ToPath = org.Value,
                            Timestamp = DateTime.Now
                        });
                    }
                    else if (Directory.Exists(org.Key))
                    {
                        Utils.CopyHelper.DirectoryCopy(org.Key, Path.Combine(org.Value, Path.GetFileName(org.Key)), true);
                        if (showSuccess) opLogs.Add(new Model.OperationLog()
                        {
                            Code = Model.OperationLog.CodeEnum.Success,
                            Msg = "操作成功",
                            FileName = Path.GetFileName(org.Key),
                            FromPath = org.Key,
                            ToPath = org.Value,
                            Timestamp = DateTime.Now
                        });
                    }
                }
                catch (Exception ex)
                {
                    if (showSuccess || showError) opLogs.Add(new Model.OperationLog()
                    {
                        Code = Model.OperationLog.CodeEnum.Error,
                        Msg = "操作失败:" + ex.Message,
                        FileName = Path.GetFileName(org.Key),
                        FromPath = org.Key,
                        ToPath = org.Value,
                        Timestamp = DateTime.Now
                    });
                }
            }

            if (opLogs.Count > 0)
            {
                var logWin = new View.LogListWindow
                {
                    LogList = opLogs
                };
                logWin.Show();
            }
        }

        private void GetOrganizeList(string path, IDictionary<string, string> ruleDict, bool skipNotConf, bool includeDir, bool skipOccupied, bool recur, IDictionary<string, string> organizeList)
        {
            if (File.Exists(path)) //文件类型
            {
                string toPath = GetOrganizeLocationByExtName(Path.GetExtension(path), ruleDict, skipNotConf);
                if (toPath != null)
                    organizeList.Add(path, toPath);
            }
            else if (Directory.Exists(path)) //文件夹类型
            {
                if (includeDir) //是否处理文件夹
                {
                    if (recur)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(path);
                        FileInfo[] files = dirInfo.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            string toPath = GetOrganizeLocationByExtName(file.Extension, ruleDict, skipNotConf);
                            if (toPath != null)
                                organizeList.Add(file.FullName, toPath);
                        }
                        DirectoryInfo[] dirs = dirInfo.GetDirectories();
                        foreach (DirectoryInfo dir in dirs)
                        {
                            GetOrganizeList(dir.FullName, ruleDict, skipNotConf, skipOccupied, includeDir, recur, organizeList);
                        }
                    }
                    else
                    {
                        string toPath = GetOrganizeLocationByExtName("#DIR#", ruleDict, skipNotConf);
                        if (toPath != null)
                            organizeList.Add(path, toPath);
                    }
                }
            }
        }

        private string GetOrganizeLocationByExtName(string extName, IDictionary<string, string> ruleDict, bool skipNotConf)
        {
            if (ruleDict.TryGetValue(extName, out string toPath) // 判断是否存在配置
                        || !skipNotConf // 是否跳过不存在配置中的文件
                        && ruleDict.TryGetValue("#OTHER#", out toPath) // 判断是否存在存放未知类型的配置
                        && toPath != null)
            {
                return toPath;
            }
            return null;
        }

        private IDictionary<string, string> GetOrganizeRuleDict()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            IList<Model.RuleItem> rules = Utils.ConfigHelper.GetRules();
            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    foreach (var ext in rule.ExtName)
                    {
                        if (!dict.ContainsKey(ext)) dict.Add(ext, rule.Location);
                    }
                }
            }
            return dict;
        }

        public const byte Rename = 1;
        private void RenameFunc(string[] filePaths)
        {

        }
        public const byte Zip = 2;
        private void ZipFunc(string[] filePaths)
        {

        }
        public const byte Unzip = 3;
        private void UnzipFunc(string[] filePaths)
        {

        }

    }




}
