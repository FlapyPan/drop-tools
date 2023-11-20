using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropOrganize.Model
{
    public class RuleItem : BaseModel
    {
        private string _name;
        private string _location;
        private List<string> _extNames;

        public string Name { get => _name; set => RaiseAndSetIfChanged(ref _name, value, "RuleName"); }
        public string Location { get => _location; set => RaiseAndSetIfChanged(ref _location, value, "RuleLocation"); }
        public List<string> ExtName { get => _extNames; set => RaiseAndSetIfChanged(ref _extNames, value, "ExtNames"); }

        public RuleItem()
        {
            _name = string.Empty;
            _location = string.Empty;
            _extNames = new List<string>();
        }

    }
}
