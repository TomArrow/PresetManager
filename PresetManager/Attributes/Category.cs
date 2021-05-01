using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresetManager.Attributes
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    public class Category : Attribute
    {
        string _categoryName;
        public Category(string categoryName)
        {
            _categoryName = categoryName;
        }

        public string getCategoryName()
        {
            return _categoryName;
        }
    }
}
