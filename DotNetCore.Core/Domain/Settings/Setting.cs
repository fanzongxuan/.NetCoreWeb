using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Domain.Settings
{
    public class Setting: BaseEntity
    {
        public Setting() { }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        
        public string Name { get; set; }
        
        public string Value { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
