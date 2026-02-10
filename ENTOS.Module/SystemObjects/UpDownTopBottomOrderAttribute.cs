using System;

namespace ENTOS.Module.SystemObjects
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class UpDownTopBottomOrderAttribute : System.Attribute
    {        
        public string Criteria;
        public bool AscSort = true;
        public bool ChangeBetweenRow = true;
        public bool AutoSave= false;

        public UpDownTopBottomOrderAttribute()
        {

        }

        public UpDownTopBottomOrderAttribute(string criteria, bool ascSort, bool changeBetweenRow, bool autoSave)
        {
            this.Criteria = criteria;
            this.AscSort = ascSort;
            this.ChangeBetweenRow = changeBetweenRow;
            this.AutoSave = autoSave;
        }
    }
}